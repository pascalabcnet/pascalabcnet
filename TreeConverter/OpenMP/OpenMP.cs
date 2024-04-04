// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.TreeRealization;
using System.Collections;

namespace PascalABCCompiler.TreeConverter
{
    class OpenMPException : Exception
    {
        string msg;
        public SyntaxTree.SourceContext SC;
        public OpenMPException(string message)
        {
            msg = message;
        }
        public OpenMPException(string message, SyntaxTree.SourceContext source_context)
        {
            msg = message;
            SC = source_context;
        }
        public override string ToString()
        {
            return msg;
        }

    }
    class StackLoopVariables
    {
        Stack<string> names = new Stack<string>();

        public StackLoopVariables()
        { }
        public StackLoopVariables(string var_name)
        {
            Push(var_name);
        }
        public void Push(string var_name)
        {
            names.Push(var_name);

        }
        public string Pop()
        {
            return names.Pop();

        }
        public bool Contains(string var_name)
        {
            return names.Contains(var_name);

        }
        public void Clear()
        { names.Clear(); }
    }
    class ReductionDirective
    {
        public ReductionOperations Oper;
        public List<string> variables = new List<string>();
    }
    class VarInfoContainer
    {
        public List<SemanticTree.IConstantDefinitionNode> Constants =
                    new List<SemanticTree.IConstantDefinitionNode>();
        public List<SemanticTree.IVAriableDefinitionNode> SharedVariables =
                    new List<SemanticTree.IVAriableDefinitionNode>();
        public List<SemanticTree.IVAriableDefinitionNode> PrivateVariables =
                    new List<SemanticTree.IVAriableDefinitionNode>();
        public List<SemanticTree.IVAriableDefinitionNode> ReductionVariables =
                    new List<SemanticTree.IVAriableDefinitionNode>();
        public List<ReductionOperations> ReductionActions = new List<ReductionOperations>();

        public void UnionWith(VarInfoContainer container)
        {
            foreach (SemanticTree.IConstantDefinitionNode node in container.Constants)
                if (!Constants.Contains(node))
                    Constants.Add(node);
            foreach (SemanticTree.IVAriableDefinitionNode node in container.SharedVariables)
                if (!SharedVariables.Contains(node))
                    SharedVariables.Add(node);
            foreach (SemanticTree.IVAriableDefinitionNode node in container.PrivateVariables)
                if (!PrivateVariables.Contains(node))
                    PrivateVariables.Add(node);
            for (int i = 0; i < container.ReductionVariables.Count; ++i )
                if (!ReductionVariables.Contains(container.ReductionVariables[i]))
                {
                    ReductionVariables.Add(container.ReductionVariables[i]);
                    ReductionActions.Add(container.ReductionActions[i]);
                }
        }
    }

    class ContextInfo
    {
        public ContextInfo(syntax_tree_visitor syntax_tree_visitor)
        {
            SaveContext(syntax_tree_visitor);
        }
        public common_type_node converted_type;
        public statement_list_stack statement_list_stack;
        public SemanticTree.field_access_level curr_fal;
        public List<var_definition_node> current_var_defs;
        public Stack<code_block> block_stack;
        public statement_node_stack cycle_stack;
        public Hashtable current_member_decls;
        public common_function_node_stack function_node_stack;
        public SymbolInfo current_last_created_function;
        public bool SemanticRulesThrowErrorWithoutSave;
        public void SaveContext(syntax_tree_visitor syntax_tree_visitor)
        {
            converted_type = syntax_tree_visitor.context.converted_type;
            syntax_tree_visitor.context.converted_type = null;
            statement_list_stack = syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack;
            syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack = new statement_list_stack();
            curr_fal = syntax_tree_visitor.context.get_field_access_level();
            current_var_defs = syntax_tree_visitor.context.var_defs;
            syntax_tree_visitor.context.var_defs = new List<var_definition_node>();
            block_stack = syntax_tree_visitor.context.block_stack;
            syntax_tree_visitor.context.block_stack = new Stack<code_block>();
            cycle_stack = syntax_tree_visitor.context.CyclesStack;
            syntax_tree_visitor.context.CyclesStack = new statement_node_stack();
            current_member_decls = syntax_tree_visitor.context.member_decls;
            syntax_tree_visitor.context.member_decls = new Hashtable();
            function_node_stack = syntax_tree_visitor.context.func_stack;
            syntax_tree_visitor.context.func_stack = new common_function_node_stack();
            current_last_created_function = syntax_tree_visitor.context.last_created_function;
            SemanticRulesThrowErrorWithoutSave = SemanticRules.ThrowErrorWithoutSave;
            SemanticRules.ThrowErrorWithoutSave = true;
        }
        public void RestoreContext(syntax_tree_visitor syntax_tree_visitor)
        {
            SemanticRules.ThrowErrorWithoutSave = SemanticRulesThrowErrorWithoutSave;
            syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack = statement_list_stack;
            syntax_tree_visitor.context.converted_type = converted_type;
            syntax_tree_visitor.context.var_defs = current_var_defs;
            syntax_tree_visitor.context.set_field_access_level(curr_fal);
            syntax_tree_visitor.context.block_stack = block_stack;
            syntax_tree_visitor.context.CyclesStack = cycle_stack;
            syntax_tree_visitor.context.member_decls = current_member_decls;
            syntax_tree_visitor.context.func_stack = function_node_stack;
            syntax_tree_visitor.context.last_created_function = current_last_created_function;
        }
    }
    class DirectiveInfo
    {
        /// <summary>
        /// Вспомогательная функция возвращает содержится ли переменная в списке переменных редукции
        /// </summary>
        /// <param name="rds"></param>
        /// <param name="var"></param>
        /// <returns></returns>
        private static bool ContainsVarible(List<ReductionDirective> rds, string var)
        {
            foreach (ReductionDirective red_dir in rds)
                if (red_dir.variables.Contains(var))
                    return true;
            return false;

        }
        /// <summary>
        ///Из списка приватных переменных убираем все переменные редукции и возвращаем этот список
        /// </summary>
        /// <param name="privs"></param>
        /// <param name="reds"></param>
        /// <returns></returns>
        private static List<string> FiltrPrivVarsReductionsVars(List<string> privs, List<ReductionDirective> reds)
        {
            List<string> res = new List<string>();
            foreach (string var in privs)
                if (!ContainsVarible(reds, var))
                    res.Add(var);
            return res;
        }
        public DirectiveInfo(SyntaxTree.compiler_directive dir)
        {
            Reductions = new List<ReductionDirective>();
            Privates = new List<string>();
           
            string DirText = dir.Directive.text.ToLower();
            if (DirText.StartsWith("critical ") || DirText.Length == "critical".Length)
            {
                Kind = DirectiveKind.Critical;
                DirText = DirText.Substring("critical".Length).Trim().Replace(" ", "");
                Name = DirText;
                //Если что, потом допилим
            }
            else if (DirText.StartsWith("parallel "))
            {
                DirText = DirText.Substring("parallel".Length).Trim();
                if (DirText.StartsWith("for ") || DirText.Length == "for".Length)
                {
                    Kind = DirectiveKind.ParallelFor;
                    DirText = DirText.Substring("for".Length).Trim();
                    ProcessClauses(DirText, dir.Directive.source_context, true);
                }
                else if (DirText.StartsWith("sections ") || DirText.Length == "sections".Length)
                {
                    Kind = DirectiveKind.ParallelSections;
                    DirText = DirText.Substring("sections".Length).Trim();
                    ProcessClauses(DirText, dir.Directive.source_context, false);
                }
                else
                {
                    Kind = DirectiveKind.Unknown;
                    PutError(DirText.Length, dir.Directive.source_context, "OMPERROR_UNKNOWN_DIRECTIVE");
                }
            }
            else
            {
                Kind = DirectiveKind.Unknown;
                PutError(DirText.Length, dir.Directive.source_context != null ? dir.Directive.source_context : dir.source_context, "OMPERROR_UNKNOWN_DIRECTIVE");
            }
        }
        //DirSC - SC текста директивы
        private void ProcessClauses(string Text, SyntaxTree.SourceContext DirSC, bool AllowReduction)
        {
            if (Text == "")
                return;

            if (Text.StartsWith("private ") || Text.StartsWith("private("))
            {
                Text = Text.Substring("private".Length).Trim();
                if (!Text.StartsWith("("))
                {
                    PutError(Text.Length, DirSC, "OMPERROR_ERROR_IN_CLAUSE_PARAMETERS");
                    return;
                }
                int ClosePos = Text.IndexOf(")");
                if (ClosePos == -1)
                {
                    PutError(Text.Length, DirSC, "OMPERROR_ERROR_IN_CLAUSE_PARAMETERS");
                    return;
                }
                string ClauseParam = Text.Substring(1, ClosePos - 1).Trim();
                if (ClauseParam.Length == 0)
                {
                    PutError(Text.Length, DirSC, "OMPERROR_ERROR_IN_CLAUSE_PARAMETERS");
                    return;
                }
                int err_code;
                List<string> prs = ProcessPrivateParametrs(ClauseParam, Privates, out err_code);
                if (err_code == 0)
                {
                    prs = FiltrPrivVarsReductionsVars(prs, Reductions);
                    Privates.AddRange(prs);
                }
                else if (err_code == 1)
                {
                    PutError(Text.Length, DirSC, "OMPERROR_ERROR_IN_CLAUSE_PARAMETERS");
                    return;
                }
                else
                {
                    PutError(Text.Length, DirSC, "OMPERROR_WARNING_IN_CLAUSE_PARAMETERS_REPEATED_VARS");
                    //return;
                    if (prs != null)
                    {
                        prs = FiltrPrivVarsReductionsVars(prs, Reductions);
                        Privates.AddRange(prs);
                    }
                }
                Text = Text.Substring(ClosePos + 1).Trim();

            }
            else if (AllowReduction && (Text.StartsWith("reduction") || Text.StartsWith("reduction(")))
            {
                Text = Text.Substring("reduction".Length).Trim();
                if (!Text.StartsWith("("))
                {
                    PutError(Text.Length, DirSC, "OMPERROR_ERROR_IN_CLAUSE_PARAMETERS");
                    return;
                }
                int ClosePos = Text.IndexOf(")");
                if (ClosePos == -1)
                {
                    PutError(Text.Length, DirSC, "OMPERROR_ERROR_IN_CLAUSE_PARAMETERS");
                    return;
                }
                string ClauseParam = Text.Substring(1, ClosePos - 1).Trim();
                if (ClauseParam.Length == 0)
                {
                    PutError(Text.Length, DirSC, "OMPERROR_ERROR_IN_CLAUSE_PARAMETERS");
                    return;
                }
                int err_code;
                ReductionDirective rd = ProcessReductionParametrs(ClauseParam, Reductions, out err_code);
                if (err_code == 0)
                {
                    if (rd != null) Reductions.Add(rd);
                }
                else if (err_code == 1)
                {
                    PutError(Text.Length, DirSC, "OMPERROR_ERROR_IN_CLAUSE_PARAMETERS");
                    return;
                }
                else
                {
                    PutError(Text.Length, DirSC, "OMPERROR_WARNING_IN_CLAUSE_PARAMETERS_REPEATED_VARS");
                    if (rd != null && rd.variables.Count != 0)
                        Reductions.Add(rd);
                    //return;
                }
                Text = Text.Substring(ClosePos + 1).Trim();
            }
            else
            {
                PutError(Text.Length, DirSC, "OMPERROR_ERROR_IN_CLAUSE");
                return;
            }
            ProcessClauses(Text, DirSC, AllowReduction);

        }
        /// <summary>
        /// Обрабатывает параметры редукции и возвращает их 
        /// </summary>
        /// <param name="param_text">текст, содержащий параметры редукции</param>
        /// <param name="rds"> список уже построенных кляуз редукции</param>
        /// <param name="err_code">код ошибки. 0 - нет ошибок,1-ошибка в параметрах редукции, 2- повторно обьявленная переменная редукции проигнорирована </param>
        /// <returns></returns>
        private ReductionDirective ProcessReductionParametrs(string param_text, List<ReductionDirective> rds, out int err_code)
        {
            //string err_mes = "Ошибка в директиве OpenMP. Неверные параметры reduction. Опция проигнорирована!";
            // теперь надо найти параметры редукции
            ReductionDirective rd = new ReductionDirective();
            int state = 1;
            int ind = 0;
            err_code = 0;
            string ident = "";
            while (ind < param_text.Length)
            {
                switch (state)
                {

                    case 1:
                        if (param_text[ind] == '+')
                        {
                            rd.Oper = ReductionOperations.plus;
                            state = 7;
                        }
                        else if (param_text[ind] == '-')
                        {
                            rd.Oper = ReductionOperations.minus;
                            state = 7;
                        }
                        else if (param_text[ind] == '*')
                        {
                            rd.Oper = ReductionOperations.mult;
                            state = 7;
                        }
                        else if (param_text[ind] == 'o')
                            state = 2;
                        else if (param_text[ind] == 'x')
                            state = 3;
                        else if (param_text[ind] == 'a')
                            state = 4;
                        else if (param_text[ind] != ' ')
                        {
                            err_code = 1;
                            return null;
                        }
                        break;
                    case 2: if (param_text[ind] == 'r')
                        {
                            rd.Oper = ReductionOperations.or;
                            state = 7;
                        }
                        else
                        {
                            err_code = 1;
                            return null;
                        }
                        break;
                    case 3: if (param_text[ind] == 'o')
                        {
                            state = 5;
                        }
                        else
                        {
                            err_code = 1;
                            return null;
                        }
                        break;
                    case 4: if (param_text[ind] == 'n')
                        {
                            state = 6;
                        }
                        else
                        {
                            err_code = 1;
                            return null;
                        }
                        break;
                    case 5: if (param_text[ind] == 'r')
                        {
                            rd.Oper = ReductionOperations.xor;
                            state = 7;
                        }
                        else
                        {
                            err_code = 1;
                            return null;
                        }
                        break;
                    case 6: if (param_text[ind] == 'd')
                        {
                            rd.Oper = ReductionOperations.and;
                            state = 7;
                        }
                        else
                        {
                            err_code = 1;
                            return null;
                        }
                        break;
                    case 7: if (param_text[ind] == ':')
                        {
                            state = 8;
                        }
                        else if (param_text[ind] != ' ')
                        {
                            err_code = 1;
                            return null;
                        }
                        break;
                    case 8:
                        if (param_text[ind] == '_' || param_text[ind] >= 'A' && param_text[ind] <= 'z')
                        {
                            state = 9;
                            ident += param_text[ind];
                        }
                        else if (param_text[ind] != ' ')
                        {
                            err_code = 1;
                            return null;
                        }
                        break;
                    case 9:// читаем имя идентификатора
                        if (param_text[ind] == '_' || param_text[ind] >= 'A' && param_text[ind] <= 'z' || param_text[ind] >= '0' && param_text[ind] <= '9')
                        {
                            ident += param_text[ind];
                        }
                        else if (param_text[ind] == ' ')
                        {
                            state = 10;
                            if (ContainsVarible(rds, ident.ToLower()))
                            {
                                err_code = 2;
                            }
                            else
                            {
                                rd.variables.Add(ident.ToLower());
                            }
                            ident = "";
                        }
                        else if (param_text[ind] == ',')
                        {

                            state = 8;
                            if (rd.variables.Contains(ident.ToLower()) || ContainsVarible(rds, ident.ToLower()))
                            {
                                err_code = 2;
                            }
                            else
                            {
                                rd.variables.Add(ident.ToLower());
                            }
                            ident = "";
                        }
                        else
                        {
                            err_code = 1;
                            return null;
                        }
                        break;
                    case 10:
                        if (param_text[ind] == ',')
                            state = 8;
                        else if (param_text[ind] != ' ')
                        {
                            err_code = 1;
                            return null;
                        }
                        break;

                }

                ind++;

            }
            if (state != 9 && state != 10)
            {
                err_code = 1;
                return null;
            }
            if (state == 9)
                if (ContainsVarible(rds, ident.ToLower()))
                {
                    err_code = 2;
                }
                else
                {
                    rd.variables.Add(ident.ToLower());
                }
            return rd;
        }

        private List<string> ProcessPrivateParametrs(string param_text, List<string> privateVars, out int err_code)
        {
            List<string> res = new List<string>();
            int ind = 0;
            //string err_mes = "Ошибка в директиве OpenMP. Неверные параметры директивы private. Опция проигнорирована!";
            // надо найти переменные private
            int state = 1;
            err_code = 0;
            string ident = "";
            while (ind < param_text.Length)
            {
                switch (state)
                {

                    case 1:
                        if (param_text[ind] == '_' || param_text[ind] >= 'A' && param_text[ind] <= 'z')
                        {
                            state = 2;
                            ident += param_text[ind];
                        }
                        else if (param_text[ind] != ' ')
                        {
                            err_code = 1;
                            res.Clear();
                            return res;
                        }
                        break;
                    case 2:// читаем дальше имя идентификатора
                        if (param_text[ind] == '_' || param_text[ind] >= 'A' && param_text[ind] <= 'z' || param_text[ind] >= '0' && param_text[ind] <= '9')
                        {
                            ident += param_text[ind];
                        }
                        else if (param_text[ind] == ' ')
                        {
                            state = 3;
                            if (res.Contains(ident.ToLower()) || privateVars.Contains(ident.ToLower()))
                            {
                                err_code = 2;

                            }
                            else
                                res.Add(ident.ToLower());
                            ident = "";
                        }
                        else if (param_text[ind] == ',')
                        {

                            state = 1;
                            if (res.Contains(ident.ToLower()) || privateVars.Contains(ident.ToLower()))
                            {
                                err_code = 2;
                            }
                            else
                                res.Add(ident.ToLower());
                            ident = "";
                        }
                        else
                        {
                            err_code = 1;
                            res.Clear();
                            return res;
                        }
                        break;
                    case 3:
                        if (param_text[ind] == ',')
                            state = 1;
                        else if (param_text[ind] != ' ')
                        {
                            err_code = 1;
                            res.Clear();
                            return res;
                        }
                        break;

                }
                ind++;

            }
            if (state != 2 && state != 3)
            {
                err_code = 1;
                res.Clear();
                return res;
            }
            if (state == 2)
            {
                if (res.Contains(ident.ToLower()) || privateVars.Contains(ident.ToLower()))
                {
                    err_code = 2;
                }
                else
                    res.Add(ident.ToLower());
            }
            return res;
        }
        private void PutError(int TextLength, SyntaxTree.SourceContext DirSC, string ErrorName)
        {
            if (DirSC != null)
            {
                int bp = DirSC.Length - TextLength - "omp".Length;

                SC = new SyntaxTree.SourceContext(DirSC.begin_position.line_num,
                            DirSC.begin_position.column_num + bp,
                            DirSC.begin_position.line_num,
                            DirSC.begin_position.column_num + bp);
            }
            

            this.ErrorName = ErrorName;
        }
        /// <summary>
        /// Тип директивы
        /// </summary>
        public DirectiveKind Kind;
        /// <summary>
        /// Имя (в случае если это критическая секция)
        /// </summary>
        public string Name;
        /// <summary>
        /// Список предложений
        /// </summary>
        public List<string> Privates;
        public List<ReductionDirective> Reductions;
        /// <summary>
        /// Если есть ошибка - здесь будет храниться ее позиция. Иначе - null.
        /// </summary>
        public SyntaxTree.SourceContext SC;
        public string ErrorName;
    }
    class Clause
    {
        /// <summary>
        /// Тип предложения
        /// </summary>
        public ClauseKind Kind;
        /// <summary>
        /// Оператор редукции
        /// </summary>
        public ReductionOperations Oper;
        /// <summary>
        /// Список имен переменных
        /// </summary>
        public List<string> Names;
    }


    public enum ParallelPosition { Outside, InsideParallel, InsideSequential }
    public enum ReductionOperations { plus, minus, mult, xor, and, or }
    public enum DirectiveKind { Critical, ParallelFor, ParallelSections, Unknown }
    public enum ClauseKind { Private, Reduction }

    internal class OpenMP
    {
        #region Флаги
        //В программе встречаются критические секции
        public static bool LocksFound = false;
        private static bool LocksInitialized = false;
        //после инициализации хранит название класса с обьектами для блокировок
        private static string LocksName = "";
        // хранит имя булевской глобальной переменной, означающей, что мы в текущий момент в параллельной секции 
        private static string InParallelSection = "";
        private static bool InParallelSectionCreated = false;
        //временно удаленные директивы
        private static Dictionary<SyntaxTree.syntax_tree_node, SyntaxTree.compiler_directive> DisabledDirectives = new Dictionary<PascalABCCompiler.SyntaxTree.syntax_tree_node, PascalABCCompiler.SyntaxTree.compiler_directive>();
        //В программе встречаются директивы parallel for
        public static bool ForsFound = false;
        //в программе встречаются директивы parallel sections
        public static bool SectionsFound = false;
        #endregion

        #region Инициализация

        public static StackLoopVariables LoopVariables = new StackLoopVariables();
        /// <summary>
        /// Словарь, возвращающий по директиве (compiler_directive) информацию о ней (DirectiveInfo)
        /// </summary>
        public static Dictionary<SyntaxTree.compiler_directive, DirectiveInfo> DirInfosTable = new Dictionary<SyntaxTree.compiler_directive, DirectiveInfo>();
        //приведение класса в исходное состояние
        public static void InternalReset()
        {
            DisabledDirectives = new Dictionary<PascalABCCompiler.SyntaxTree.syntax_tree_node, PascalABCCompiler.SyntaxTree.compiler_directive>();
            LocksName = "";
            LocksFound = false;
            LocksInitialized = false;
            ForsFound = false;
            SectionsFound = false;
            LoopVariables.Clear();
            DirInfosTable.Clear();
            InParallelSection = "";
            InParallelSectionCreated = false;
        }

        //инициализация OpenMP
        public static void InitOpenMP(List<SyntaxTree.compiler_directive> directives, syntax_tree_visitor visitor, SyntaxTree.compilation_unit cu)
        {
            //Из-за переноса этой проверки в секцию инициализации - оно не всегда успевает инициализироваться до выполнения этого кода.
            //Будем надеяться, что условие никогда не выполнится. Без постороннего вмешательства (замена файлов старыми версиями) - не должно.
            //if (SystemLibrary.SystemLibInitializer.OMP_Available == null || SystemLibrary.SystemLibInitializer.OMP_Available.NotFound)
            //{
            //    visitor.AddWarning(new Errors.CommonWarning(PascalABCCompiler.StringResources.Get("OMPERROR_OMP_NOT_AVAILABLE"), cu.file_name, cu.source_context.begin_position.line_num, cu.source_context.begin_position.column_num));
            //    return;
            //}
            foreach (SyntaxTree.compiler_directive dir in directives)
            {
                if (dir.Name.text.ToLower() == "omp")
                {
                    string DirText = dir.Directive.text.ToLower();
                    DirectiveInfo dirInf = new DirectiveInfo(dir);
                    
                    DirInfosTable.Add(dir, dirInf);
                    if (dirInf.Kind == DirectiveKind.ParallelFor)
                        ForsFound = true;
                    else if (dirInf.Kind == DirectiveKind.ParallelSections)
                        SectionsFound = true;
                    else if (dirInf.Kind == DirectiveKind.Critical)
                        LocksFound = true;
                    else 
                    {
                        visitor.AddWarning(new Errors.CommonWarning(PascalABCCompiler.StringResources.Get(dirInf.ErrorName), 
                            dir.source_context.FileName, 
                            dirInf.SC != null ? dirInf.SC.begin_position.line_num : dir.source_context.begin_position.line_num,
                            dirInf.SC != null ? dirInf.SC.begin_position.column_num : dir.source_context.begin_position.column_num));
                    }
                }
            }
            //уже не нужно
            //if (ForsFound && SystemLibrary.SystemLibInitializer.OMP_ParallelFor.NotFound)
            //{
            //    visitor.AddWarning(new Errors.CommonWarning(PascalABCCompiler.StringResources.Get("OMPERROR_PARALLELIZATION_FOR_NOT_AVAILABLE"), cu.file_name, cu.source_context.begin_position.line_num, cu.source_context.begin_position.column_num));
            //    ForsFound = false;
            //}
            //if (SectionsFound && SystemLibrary.SystemLibInitializer.OMP_ParallelSections.NotFound)
            //{
            //    visitor.AddWarning(new Errors.CommonWarning(PascalABCCompiler.StringResources.Get("OMPERROR_PARALLELIZATION_SECTIONS_NOT_AVAILABLE"), cu.file_name, cu.source_context.begin_position.line_num, cu.source_context.begin_position.column_num));
            //    SectionsFound = false;
            //}

            //"оторванные" директивы:
            //можно использовать директивы синхронизации и вне параллельных областей.
            //if (!SectionsFound && !ForsFound && LocksFound)
            //{
            //    visitor.AddWarning(new Errors.CommonWarning(PascalABCCompiler.StringResources.Get("OMPERROR_USING_CRITICAL_SECTIONS_OUTSIDE_PARALLEL_STRUCTURES"), cu.file_name, cu.source_context.begin_position.line_num, cu.source_context.begin_position.column_num));
            //    LocksFound = false;
            //}

        }
        //инициализация критических секций с созданием класса
        private static void InitCriticals(syntax_tree_visitor visitor)
        {
            //генерируем класс
            SyntaxTree.type_declarations TypeDecls = new PascalABCCompiler.SyntaxTree.type_declarations();
            SyntaxTree.type_declaration TypeDecl = new PascalABCCompiler.SyntaxTree.type_declaration();
            TypeDecls.types_decl.Add(TypeDecl);
            LocksName = visitor.context.get_free_name("$locks_container{0}");
            TypeDecl.type_name = new PascalABCCompiler.SyntaxTree.ident(LocksName);
            SyntaxTree.class_definition ClassDef = new PascalABCCompiler.SyntaxTree.class_definition();
            TypeDecl.type_def = ClassDef;
            SyntaxTree.class_body_list ClassBody = new PascalABCCompiler.SyntaxTree.class_body_list();
            ClassDef.body = ClassBody;
            SyntaxTree.class_members ClassMember = new PascalABCCompiler.SyntaxTree.class_members();
            ClassBody.class_def_blocks.Add(ClassMember);
            ClassMember.access_mod = new PascalABCCompiler.SyntaxTree.access_modifer_node(PascalABCCompiler.SyntaxTree.access_modifer.public_modifer);

            List<string> ProcessedNames = new List<string>();

            foreach (KeyValuePair<SyntaxTree.compiler_directive, DirectiveInfo> pair in DirInfosTable)
            {

                if (pair.Value.Kind == DirectiveKind.Critical)
                {
                    string LockName = "$default";
                    if (pair.Value.Name.Length != 0)
                        LockName = pair.Value.Name;

                    if (ProcessedNames.Contains(LockName))
                        continue;
                    ProcessedNames.Add(LockName);

                    SyntaxTree.var_def_statement vds = new PascalABCCompiler.SyntaxTree.var_def_statement();
                    SyntaxTree.ident_list idl = new PascalABCCompiler.SyntaxTree.ident_list();
                    vds.vars = idl;
                    idl.Add(new SyntaxTree.ident(LockName));
                    SyntaxTree.named_type_reference ntr = new PascalABCCompiler.SyntaxTree.named_type_reference();
                    vds.vars_type = ntr;
                    ntr.Add(new SyntaxTree.ident("object"));
                    SyntaxTree.new_expr ne = new PascalABCCompiler.SyntaxTree.new_expr();
                    vds.inital_value = ne;
                    ne.type = ntr;
                    vds.var_attr = PascalABCCompiler.SyntaxTree.definition_attribute.Static;
                    ClassMember.members.Add(vds);
                }

            }
            //сохраняем контекст
            ContextInfo contextInfo = new ContextInfo(visitor);

            try
            {
                visitor.visit(TypeDecls);
                LocksInitialized = true;
            }
            finally
            {
                //восстанавливаем контекст
                contextInfo.RestoreContext(visitor);
            }
        }
        #endregion

        #region Обработка критических секций
        public static void TryConvertCritical(ref SyntaxTree.statement st, syntax_tree_visitor visitor, SyntaxTree.compiler_directive directive)
        {
            if (!LocksInitialized)
                InitCriticals(visitor);

            string LockName = "$default";
            if (DirInfosTable[directive].Name.Length != 0)
                LockName = DirInfosTable[directive].Name;
            SyntaxTree.lock_stmt LockStmt = new PascalABCCompiler.SyntaxTree.lock_stmt();
            LockStmt.lock_object = new SyntaxTree.dot_node(new SyntaxTree.ident(LocksName), new SyntaxTree.ident(LockName));
            LockStmt.stmt = st;
            st = LockStmt;
        }
        public static void DisableDirective(SyntaxTree.syntax_tree_node node, Dictionary<SyntaxTree.syntax_tree_node, SyntaxTree.compiler_directive> linker)
        {
            if (!linker.ContainsKey(node))
                return;
            SyntaxTree.compiler_directive dir = linker[node];
            if (DirInfosTable.ContainsKey(dir) && DirInfosTable[dir].Kind == DirectiveKind.Critical)
            {
                DisabledDirectives.Add(node, dir);
                linker.Remove(node);
            }
        }
        public static void EnableDirective(SyntaxTree.syntax_tree_node node, Dictionary<SyntaxTree.syntax_tree_node, SyntaxTree.compiler_directive> linker)
        {
            if (!DisabledDirectives.ContainsKey(node))
                return;
            SyntaxTree.compiler_directive dir = DisabledDirectives[node];
            DisabledDirectives.Remove(node);
            linker.Add(node, dir);
        }
        #endregion

        public static bool IsParallelSectionsDirective(SyntaxTree.compiler_directive directive)
        {
            return DirInfosTable.ContainsKey(directive) && DirInfosTable[directive].Kind == DirectiveKind.ParallelSections;
        }
        public static bool IsParallelForDirective(SyntaxTree.compiler_directive directive)
        {
            return DirInfosTable.ContainsKey(directive) && DirInfosTable[directive].Kind == DirectiveKind.ParallelFor;

        }
        public static bool IsCriticalDirective(SyntaxTree.compiler_directive directive)
        {
            return DirInfosTable.ContainsKey(directive) && DirInfosTable[directive].Kind == DirectiveKind.Critical;
        }

        #region Распараллеливание For
        //Проверки на доступность OMP и директиву проводятся перед вызовом.
        internal static statements_list TryConvertFor(statements_list for_head_stmts, SyntaxTree.for_node for_node, for_node fn, var_definition_node loop_variable, expression_node fromInclusive, expression_node toInclusive, syntax_tree_visitor syntax_tree_visitor)
        {
            try
            {
                location loc = fn.location;
                statements_list omp_stmts = new statements_list(loc);
                statements_list head_stmts = new statements_list(loc);

                syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack_push(head_stmts);

                if (!InParallelSectionCreated)
                    CreateInParallelVariable(syntax_tree_visitor, out InParallelSection);
                //если omp доступен то (выполнять паралельно) иначе (выполнять for)
                if_node ifnode = CreateIfCondition(syntax_tree_visitor, omp_stmts, for_head_stmts, loc);
                head_stmts.statements.AddElement(ifnode);

                //генерим ветку в случае когда доступен omp
                if (!GenerateOMPParallelForCall(fn.body, for_node, loop_variable, omp_stmts, syntax_tree_visitor, fromInclusive, toInclusive))
                {
                    syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack_pop();
                    return null;
                }

                syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack_pop();

                return head_stmts;
            }
            catch (OpenMPException e)
            {
                Exception ex = new Exception(e.ToString());
                syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack_pop();
                syntax_tree_visitor.WarningsList.Add(new OMP_BuildigError(ex, syntax_tree_visitor.get_location(new SyntaxTree.syntax_tree_node(e.SC))));
            }
            catch (Exception e)
            {
                syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack_pop();
                syntax_tree_visitor.WarningsList.Add(new OMP_BuildigError(e, fn.location));
            }
            return null;
        }

        private static bool GenerateOMPParallelForCall(statement_node body, SyntaxTree.for_node for_node, var_definition_node loop_variable, statements_list omp_stmts, syntax_tree_visitor syntax_tree_visitor, expression_node fromInclusive, expression_node toInclusive)
        {
            SyntaxTree.statement syntax_body = for_node.statements;

            expression_node omp_call = null;
            base_function_call bfc = body as base_function_call;
            if (bfc != null && bfc.parameters.Count == 1 && bfc.parameters[0] is variable_reference &&
                ((variable_reference)bfc.parameters[0]).VariableDefinition == loop_variable && ((bfc.function.parameters[0].type as type_node).PrintableName.ToLower() == "integer"))
            {
                //если тело цикла - вызов функции с одни параметром - переменной цикла,
                //если при этом у вызываемой функции тип параметра - integer, а не какой-нибудь object, как это бывает с write и вообще может быть с перегрузкой
                //то генерировать класс не надо.
                //генерируем вызов и все
                omp_call = syntax_tree_visitor.CreateDelegateCall(bfc);
                if (omp_call == null)
                {
                    syntax_tree_visitor.AddWarning(new OMP_ConstructionNotSupportedNow(body.location));
                    syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack_pop();
                    return false;
                }
                base_function_call omp_parallel_for_call = null;
                if (SystemLibrary.SystemLibInitializer.OMP_ParallelFor.sym_info is common_namespace_function_node)
                    omp_parallel_for_call = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.OMP_ParallelFor.sym_info as common_namespace_function_node, body.location);
                else
                    omp_parallel_for_call = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.OMP_ParallelFor.sym_info as compiled_function_node, body.location);
                omp_parallel_for_call.parameters.AddElement(fromInclusive);
                omp_parallel_for_call.parameters.AddElement(toInclusive);
                omp_parallel_for_call.parameters.AddElement(omp_call);
                omp_stmts.statements.AddElement(omp_parallel_for_call);
            }
            else
            {
                //ищем используемые переменные, получаем редукцию из директивы и составляем список переменных по типам
                VarFinderSyntaxVisitor VFvis = new VarFinderSyntaxVisitor(syntax_body, syntax_tree_visitor.context, true);
                SyntaxTree.compiler_directive dir = syntax_tree_visitor.DirectivesToNodesLinks[for_node];
                //if (DirInfosTable[dir].ErrorName == "WARNING_IN_CLAUSE_PARAMETERS_REPEATED_VARS")
                //    syntax_tree_visitor.AddWarning(new Errors.CommonWarning(StringResources.Get(DirInfosTable[dir].ErrorName), for_node.source_context.file_name, DirInfosTable[dir].SC.begin_position.line_num, DirInfosTable[dir].SC.begin_position.column_num));
                //else if (DirInfosTable[dir].ErrorName == "ERROR_IN_CLAUSE_PARAMETERS")
                //{
                //    syntax_tree_visitor.AddWarning(new Errors.CommonWarning(StringResources.Get(DirInfosTable[dir].ErrorName), for_node.source_context.file_name, DirInfosTable[dir].SC.begin_position.line_num, DirInfosTable[dir].SC.begin_position.column_num));
                //}
                //else
               
                if (DirInfosTable[dir].ErrorName !=null)//== "ERROR_IN_CLAUSE")
                {
                    syntax_tree_visitor.AddWarning(new Errors.CommonWarning(PascalABCCompiler.StringResources.Get(DirInfosTable[dir].ErrorName), for_node.source_context.FileName, DirInfosTable[dir].SC.begin_position.line_num, DirInfosTable[dir].SC.begin_position.column_num));
                }

                VarInfoContainer Vars = GetVarInfoContainer(VFvis, DirInfosTable[dir].Reductions, DirInfosTable[dir].Privates, syntax_tree_visitor, dir);

                //сохраняем контекст
                ContextInfo contextInfo = new ContextInfo(syntax_tree_visitor);

                string ClassName = syntax_tree_visitor.context.get_free_name("$for_class{0}");

                try
                {
                    //создаем и конвертируем класс
                    SyntaxTree.class_members member;
                    SyntaxTree.type_declarations Decls = CreateClass(ClassName, out member, Vars);
                    member.members.Add(CreateMethod("Method", syntax_body, for_node.loop_variable.name, member, Vars));
                    syntax_tree_visitor.visit(Decls);
                }
                finally
                {
                    //восстанавливаем контекст
                    contextInfo.RestoreContext(syntax_tree_visitor);
                }

                //создаем инициализацию, вызов и финализацию
                string ObjName = syntax_tree_visitor.context.get_free_name("$for_obj{0}");

                SyntaxTree.dot_node dn = new SyntaxTree.dot_node(new SyntaxTree.ident(ObjName), new SyntaxTree.ident("Method"));

                SyntaxTree.statement_list stl = CreateInitPart(ClassName, ObjName, Vars);
                stl.subnodes.Add(CreateNestedRegionBorder(true));
                stl.subnodes.Add(CreateOMPParallelForCall(dn, for_node.initial_value, for_node.finish_value));
                stl.subnodes.Add(CreateNestedRegionBorder(false));
                stl.subnodes.AddRange(CreateFinalPart(ObjName, Vars).subnodes);
                omp_stmts.statements.AddElement(syntax_tree_visitor.ret.visit(stl));
            }
            return true;
        }
        #endregion

        #region Распараллеливание секций
        internal static statements_list TryConvertSections(statements_list semantic_stmts, SyntaxTree.statement_list syntax_stmts, syntax_tree_visitor syntax_tree_visitor)
        {
            try
            {
                location loc = semantic_stmts.location;
                statements_list omp_stmts = new statements_list(loc);
                statements_list head_stmts = new statements_list(loc);
                syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack_push(head_stmts);

                if (!InParallelSectionCreated)
                    CreateInParallelVariable(syntax_tree_visitor, out InParallelSection);
                //если omp доступен то (выполнять паралельно) иначе (выполнять последовательно операторы)
                if_node ifnode = CreateIfCondition(syntax_tree_visitor, omp_stmts, semantic_stmts, loc);
                head_stmts.statements.AddElement(ifnode);
                
                //генерим ветку в случае когда доступен omp
                if (!GenerateOMPParallelSectionsCall(semantic_stmts, syntax_stmts, omp_stmts, syntax_tree_visitor))
                {
                    syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack_pop();
                    return null;
                }

                syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack_pop();

                return head_stmts;
            }
            catch (OpenMPException e)
            {
                Exception ex = new Exception(e.ToString());
                syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack_pop();
                syntax_tree_visitor.WarningsList.Add(new OMP_BuildigError(ex, syntax_tree_visitor.get_location(new SyntaxTree.syntax_tree_node(e.SC))));
            }
            catch (Exception e)
            {
                syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack_pop();
                syntax_tree_visitor.WarningsList.Add(new OMP_BuildigError(e, semantic_stmts.location));
            }
            return null;
        }
        private static bool GenerateOMPParallelSectionsCall(statements_list stmts, SyntaxTree.statement_list syntax_stmts, statements_list omp_stmts, syntax_tree_visitor syntax_tree_visitor)
        {

            SyntaxTree.expression_list delegates = new PascalABCCompiler.SyntaxTree.expression_list();
            SyntaxTree.statement_list stlInit = new PascalABCCompiler.SyntaxTree.statement_list();
            SyntaxTree.statement_list stlFinal = new PascalABCCompiler.SyntaxTree.statement_list();
            VarInfoContainer Vars = new VarInfoContainer();
            string ClassName = syntax_tree_visitor.context.get_free_name("$section_class{0}");
            List<SyntaxTree.statement> Sections = new List<PascalABCCompiler.SyntaxTree.statement>();
            foreach (SyntaxTree.statement syntax_statement in syntax_stmts.subnodes)
            {
                if (syntax_statement is SyntaxTree.empty_statement)
                    continue;       //А зачем? ;-)
                if (syntax_statement is SyntaxTree.var_statement)
                {
                    //выдать предупреждение. Это не нормально для параллельных секций
                    syntax_tree_visitor.visit(syntax_statement as SyntaxTree.var_statement);
                }
                else
                {
                    //ищем используемые переменные
                    VarFinderSyntaxVisitor VFvis = new VarFinderSyntaxVisitor(syntax_statement, syntax_tree_visitor.context, false);
                    SyntaxTree.compiler_directive dir = syntax_tree_visitor.DirectivesToNodesLinks[syntax_stmts];
                    
                    //if (DirInfosTable[dir].ErrorName == "WARNING_IN_CLAUSE_PARAMETERS_REPEATED_VARS")
                    //    syntax_tree_visitor.AddWarning(new Errors.CommonWarning(StringResources.Get(DirInfosTable[dir].ErrorName), syntax_stmts.source_context.file_name, DirInfosTable[dir].SC.begin_position.line_num, DirInfosTable[dir].SC.begin_position.column_num));
                    //else if (DirInfosTable[dir].ErrorName == "ERROR_IN_CLAUSE_PARAMETERS")
                    //{
                    //    syntax_tree_visitor.AddWarning(new Errors.CommonWarning(StringResources.Get(DirInfosTable[dir].ErrorName), syntax_stmts.source_context.file_name, DirInfosTable[dir].SC.begin_position.line_num, DirInfosTable[dir].SC.begin_position.column_num));
                    //}
                    //else
                    if (DirInfosTable[dir].ErrorName != null)
                    {
                        syntax_tree_visitor.AddWarning(new Errors.CommonWarning(PascalABCCompiler.StringResources.Get(DirInfosTable[dir].ErrorName), syntax_stmts.source_context.FileName, DirInfosTable[dir].SC.begin_position.line_num, DirInfosTable[dir].SC.begin_position.column_num));
                    }
                    Vars.UnionWith(GetVarInfoContainer(VFvis, null, DirInfosTable[dir].Privates, syntax_tree_visitor, dir));
                    Sections.Add(syntax_statement);
                }
            }
            //сохраняем контекст
            ContextInfo contextInfo = new ContextInfo(syntax_tree_visitor);

            try
            {
                //создание и конвертирование класса
                SyntaxTree.class_members member;
                SyntaxTree.type_declarations Decls = CreateClass(ClassName, out member, Vars);
                for (int i = 0; i < Sections.Count; ++i)
                    member.members.Add(CreateMethod("method" + i.ToString(), Sections[i], "", member, Vars));
                syntax_tree_visitor.visit(Decls);
            }
            finally
            {
                //восстанавливаем контекст
                contextInfo.RestoreContext(syntax_tree_visitor);
            }
            //создаем инициализацию и финализацию
            int NameNum = 0;
            string ObjName = GetFreeName("$section_obj", ref NameNum, syntax_tree_visitor.context);

            stlInit.subnodes.AddRange(CreateInitPart(ClassName, ObjName, Vars).subnodes);
            stlFinal.subnodes.AddRange(CreateFinalPart(ObjName, Vars).subnodes);

            SyntaxTree.procedure_call pc = new PascalABCCompiler.SyntaxTree.procedure_call();
            SyntaxTree.method_call mc = new PascalABCCompiler.SyntaxTree.method_call();
            mc.dereferencing_value = CreateTPLFunctionReference("Invoke");
            pc.func_name = mc;
            SyntaxTree.expression_list exl = new PascalABCCompiler.SyntaxTree.expression_list();
            //foreach (string str in ObjNames)
            for (int i=0; i<Sections.Count; ++i)
                exl.Add(new SyntaxTree.dot_node(new SyntaxTree.ident(ObjName), new SyntaxTree.ident("Method"+i.ToString())));
            mc.parameters = exl;

            stlInit.subnodes.Add(CreateNestedRegionBorder(true));
            stlInit.subnodes.Add(pc);
            stlInit.subnodes.AddRange(stlFinal.subnodes);
            stlInit.subnodes.Add(CreateNestedRegionBorder(false));

            statement_node st = syntax_tree_visitor.ret.visit(stlInit);
            omp_stmts.statements.AddElement(st);
            return true;
        }
        #endregion

        #region Создание обьектов-функций для параллельных конструкций

        /// <summary>
        /// Вспомогательная функция - возвращает подстроку между квадратными скобками, при поиске индексеров массива
        /// </summary>
        /// <param name="type_str"></param>
        /// <returns></returns>
        private static string get_indexer_string(string type_str)
        {
            string res = "";
            int from = type_str.IndexOf("[");
            if (from == -1)
                return res;
            int to = type_str.IndexOf("]");
            if (to == -1)
                return res;
            res = type_str.Substring(from + 1, to - from - 1);
            return res;
        }
        /// <summary>
        /// Возращает диапазоны массива из строки индексеров
        /// </summary>
        /// <param name="index_str"></param>
        /// <returns></returns>
        private static List<SyntaxTree.diapason> get_diapasons(string index_str)
        {
            List<SyntaxTree.diapason> res = new List<PascalABCCompiler.SyntaxTree.diapason>();
            string[] diaps = index_str.Split(',');
            foreach (string str in diaps)
            {

                if (str.Trim() == "")
                {
                    res.Add(null);
                }
                else
                {
                    string left = str.Substring(0, str.IndexOf('.')).Trim();
                    int from = str.LastIndexOf('.');
                    string right = str.Substring(from + 1, str.Length - from - 1).Trim();
                    int val;
                    SyntaxTree.expression left_expr, right_expr;
                    if (Int32.TryParse(left, out val))
                    {
                        left_expr = new SyntaxTree.int32_const(val);
                    }
                    else left_expr = new SyntaxTree.ident(left);
                    if (Int32.TryParse(right, out val))
                    {
                        right_expr = new SyntaxTree.int32_const(val);
                    }
                    else right_expr = new SyntaxTree.ident(right);
                    res.Add(new PascalABCCompiler.SyntaxTree.diapason(left_expr, right_expr));
                }
            }
            return res;
        }
        /// <summary>
        /// Вспомогательная функция - разбивает строку по точкам и возвращает подстроки в виде списка ident-в
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static List<SyntaxTree.ident> get_idents_from_dot_string(string s, SyntaxTree.SourceContext sc)
        {
            List<SyntaxTree.ident> idents = new List<PascalABCCompiler.SyntaxTree.ident>();
            string[] strs = s.Split('.');
            foreach (string id in strs)
                idents.Add(new SyntaxTree.ident(id, sc));
            return idents;

        }
        /// <summary>
        /// Возвращает список ident-в, необходимых для генерации named_type_reference
        /// </summary>
        /// <param name="sem_type">семантический тип</param>
        /// <returns></returns>
        private static List<SyntaxTree.ident> get_idents_from_generic_type(type_node sem_type, SyntaxTree.SourceContext sc)
        {
            if (sem_type != null)
            {
                List<SyntaxTree.ident> idents = null;
                if (sem_type.original_generic != null)
                    idents = get_idents_from_dot_string(sem_type.original_generic.full_name, sc);

                return idents;

            }
            return null;
        }

        private static SyntaxTree.type_definition get_diapason(type_node sem_type)
        {
            if (sem_type is compiled_type_node)
                return new PascalABCCompiler.SyntaxTree.named_type_reference(get_idents_from_dot_string(sem_type.PrintableName, sem_type.location));

            if (sem_type is common_type_node)
            {
                SyntaxTree.diapason diap = new PascalABCCompiler.SyntaxTree.diapason();
                diap.source_context = sem_type.location;
                common_type_node ctn = sem_type as common_type_node;
                diap.left = ConvertConstant(ctn.low_bound);
                diap.right = ConvertConstant(ctn.upper_bound);

                return diap;

            }
            return null;
        }

        /// <summary>
        /// Возвращает по семантическому типу соответсвующий ему синтаксический тип
        /// </summary>
        /// <param name="sem_type">семантический тип</param>
        /// <returns>Синтаксический тип</returns>
        public static SyntaxTree.type_definition ConvertToSyntaxType(type_node sem_type)
        {
            // SSM 29/12/18 для перечислимого типа возвращает null!
            if (sem_type.IsPointer)// если указатель
            {
                SyntaxTree.ref_type rt = new PascalABCCompiler.SyntaxTree.ref_type();
                if (sem_type is compiled_type_node)
                    rt.pointed_to = ConvertToSyntaxType((sem_type as compiled_type_node).element_type);
                else
                    rt.pointed_to = ConvertToSyntaxType((sem_type as ref_type_node).pointed_type);
                return rt;

            }
            else
                if (sem_type.type_special_kind == SemanticTree.type_special_kind.none_kind
                    || sem_type.type_special_kind == SemanticTree.type_special_kind.record || sem_type.type_special_kind == SemanticTree.type_special_kind.text_file)
            {
                if (sem_type.is_generic_type_instance)// это шаблонный тип
                {
                    SyntaxTree.template_type_reference ttr = new PascalABCCompiler.SyntaxTree.template_type_reference();
                    SyntaxTree.named_type_reference ntr = new PascalABCCompiler.SyntaxTree.named_type_reference();
                    ttr.name = ntr;
                    ttr.source_context = sem_type.location;
                    ntr.source_context = ttr.source_context;
                    ntr.names.AddRange(get_idents_from_generic_type(sem_type, sem_type.location));
                    SyntaxTree.template_param_list tpl = new PascalABCCompiler.SyntaxTree.template_param_list();
                    ttr.params_list = tpl;
                    foreach (type_node tn in sem_type.instance_params)
                        tpl.params_list.Add(ConvertToSyntaxType(tn));

                    return ttr;
                }
                else if (sem_type.IsEnum)
                    return new PascalABCCompiler.SyntaxTree.named_type_reference(get_idents_from_dot_string(sem_type.name, sem_type.location), sem_type.location);
                else if (sem_type is compiled_type_node ctn1 && ctn1.compiled_type == typeof(System.IntPtr))
                    return new PascalABCCompiler.SyntaxTree.named_type_reference(get_idents_from_dot_string("System.IntPtr", sem_type.location), sem_type.location);
                else if (sem_type is compiled_type_node ctn2 && ctn2.compiled_type == typeof(System.UIntPtr))
                    return new PascalABCCompiler.SyntaxTree.named_type_reference(get_idents_from_dot_string("System.UIntPtr", sem_type.location), sem_type.location);
                else if (sem_type is common_type_node && (sem_type as common_type_node).IsDelegate)
                {
                    var tn = sem_type as common_type_node;
                    var invokeMeth = tn.find_first_in_type("Invoke");
                    if (invokeMeth != null)
                    {
                        var fn = invokeMeth.sym_info as function_node;
                        PascalABCCompiler.SyntaxTree.procedure_header header;
                        if (fn.return_value_type != null)
                        {
                            header = new PascalABCCompiler.SyntaxTree.function_header(ConvertToSyntaxType(fn.return_value_type));
                        }
                        else
                        {
                            header = new PascalABCCompiler.SyntaxTree.procedure_header();
                        }
                        header.parameters = new PascalABCCompiler.SyntaxTree.formal_parameters();
                        foreach (var param in fn.parameters)
                        {
                            var tparam = new PascalABCCompiler.SyntaxTree.typed_parameters();
                            tparam.vars_type = ConvertToSyntaxType(param.type);
                            tparam.idents = new PascalABCCompiler.SyntaxTree.ident_list();
                            tparam.idents.Add(new PascalABCCompiler.SyntaxTree.ident(param.name));
                            header.parameters.Add(tparam);

                        }
                        return header;
                    }
                    else
                        return new PascalABCCompiler.SyntaxTree.named_type_reference(get_idents_from_dot_string(sem_type.PrintableName, sem_type.location), sem_type.location);
                }
                else
                    return new PascalABCCompiler.SyntaxTree.named_type_reference(get_idents_from_dot_string(sem_type.PrintableName, sem_type.location), sem_type.location);
            }
            else if (sem_type.type_special_kind == SemanticTree.type_special_kind.array_kind || sem_type.type_special_kind == SemanticTree.type_special_kind.array_wrapper)
            {
                //значит тип-это массив
                SyntaxTree.array_type arr_t = new PascalABCCompiler.SyntaxTree.array_type();
                arr_t.source_context = sem_type.location;
                // Cоздаем индексер для массива
                SyntaxTree.indexers_types indt = new PascalABCCompiler.SyntaxTree.indexers_types();
                indt.source_context = arr_t.source_context;
                if (sem_type is common_type_node)
                {
                    SyntaxTree.diapason diap = new PascalABCCompiler.SyntaxTree.diapason();

                    common_type_node ctn = sem_type as common_type_node;
                    if (ctn.constants.Length > 1)
                    {
                        diap.left = ConvertConstant(ctn.constants[0].constant_value);
                        diap.right = ConvertConstant(ctn.constants[1].constant_value);
                        indt.indexers.Add(diap);
                    }
                    else
                    {
                        if (ctn.rank == 1)
                            indt = null;
                        else
                            for (int i = 0; i < ctn.rank; i++)
                                indt.indexers.Add(null);
                    }

                    arr_t.indexers = indt;
                }
                else if (sem_type is compiled_type_node)
                {
                    compiled_type_node ctn = sem_type as compiled_type_node;
                    if (ctn.rank > 1)
                    {
                        for (int i = 0; i < ctn.rank; i++)
                            indt.indexers.Add(null);
                        arr_t.indexers = indt;
                    }
                    //Получаем индексеры из строки
                    //string ind_str = sem_type.PrintableName;
                    //ind_str = get_indexer_string(ind_str);

                    //if (ind_str.Length != 0)
                    //{
                    //    indt.indexers.AddRange(get_diapasons(ind_str).ToArray());
                    //    arr_t.indexers = indt;
                    //}
                }
                //проверяем тип элементов массива               
                if (sem_type.element_type != null)
                {
                    arr_t.elements_type = ConvertToSyntaxType(sem_type.element_type);
                }
                return arr_t;
            }
            else if (sem_type.type_special_kind == SemanticTree.type_special_kind.typed_file || sem_type.type_special_kind == SemanticTree.type_special_kind.binary_file)
            {
                SyntaxTree.file_type ft = new PascalABCCompiler.SyntaxTree.file_type();
                ft.source_context = sem_type.location;
                if (sem_type.element_type != null)
                    ft.file_of_type = ConvertToSyntaxType(sem_type.element_type);
                //SyntaxTree.named_type_reference ntr = null;
                //if (sem_type.element_type!= null)
                //    ntr= new PascalABCCompiler.SyntaxTree.named_type_reference(get_idents_from_dot_string(sem_type.element_type.name));
                //ft.file_of_type = ntr;
                return ft;
            }
            else if (sem_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.short_string)
            {
                SyntaxTree.string_num_definition snd = new PascalABCCompiler.SyntaxTree.string_num_definition();
                snd.source_context = sem_type.location;
                snd.name = new SyntaxTree.ident(sem_type.name.Substring(0, sem_type.name.IndexOf('[')));
                snd.num_of_symbols = new SyntaxTree.int32_const(Int32.Parse(get_indexer_string(sem_type.PrintableName)));
                return snd;
            }
            else if (sem_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.set_type)
            {
                SyntaxTree.set_type_definition std = new PascalABCCompiler.SyntaxTree.set_type_definition();
                std.source_context = sem_type.location;
                if (sem_type.element_type != null)
                    std.of_type = ConvertToSyntaxType(sem_type.element_type);
                return std;
            }
            else if (sem_type.type_special_kind == SemanticTree.type_special_kind.diap_type)
            {
                return get_diapason(sem_type);


            }
            else if (sem_type.type_special_kind == SemanticTree.type_special_kind.enum_kind)
                return new SyntaxTree.semantic_type_node(sem_type);

            return null;
        }

        /// <summary>
        /// Возвращает ассоциативный массив диапазонов массива из синтаксического узла
        /// </summary>
        /// <param name="arr">массив-синтаксический узел</param>
        /// <returns></returns>
        private static Dictionary<int, List<SyntaxTree.diapason>> get_diapasons_from_array(SyntaxTree.array_type arr)
        {

            if (arr != null)
            {
                Dictionary<int, List<SyntaxTree.diapason>> res = new Dictionary<int, List<SyntaxTree.diapason>>();
                int i = 0;
                while (true)
                {

                    if (arr.indexers != null)
                    {
                        List<SyntaxTree.diapason> diaps = new List<PascalABCCompiler.SyntaxTree.diapason>();
                        foreach (SyntaxTree.type_definition td in arr.indexers.indexers)
                        {
                            if (td is SyntaxTree.diapason)
                                diaps.Add(td as SyntaxTree.diapason);

                        }
                        res.Add(i, diaps);
                    }
                    if (arr.elements_type is SyntaxTree.array_type)
                        arr = arr.elements_type as SyntaxTree.array_type;
                    else break;

                    i++;
                }
                return res;
            }
            return null;
        }

        /// <summary>
        /// Возвращает список диапазонов массива из синтаксического узла
        /// </summary>
        /// <param name="arr">массив-синтаксический узел</param>
        /// <returns></returns>
        private static List<List<SyntaxTree.diapason>> get_list_of_diapasons(SyntaxTree.array_type arr)
        {

            if (arr != null)
            {
                List<List<SyntaxTree.diapason>> res = new List<List<SyntaxTree.diapason>>();

                while (true)
                {
                    List<SyntaxTree.diapason> diaps = new List<PascalABCCompiler.SyntaxTree.diapason>();
                    if (arr.indexers != null)
                    {

                        foreach (SyntaxTree.type_definition td in arr.indexers.indexers)
                        {
                            diaps.Add(td as SyntaxTree.diapason);

                        }

                    }
                    res.Add(diaps);

                    if (arr.elements_type is SyntaxTree.array_type)
                        arr = arr.elements_type as SyntaxTree.array_type;
                    else break;


                }
                return res;
            }
            return null;
        }
        private static SyntaxTree.statement_list AssignArrs(SyntaxTree.array_type ArrFrom, SyntaxTree.addressed_value IdFrom, SyntaxTree.addressed_value IdTo)
        {
            SyntaxTree.statement_list OuterSTL = new PascalABCCompiler.SyntaxTree.statement_list();
            SyntaxTree.statement_list InnerSTL = OuterSTL;
            List<List<SyntaxTree.diapason>> DiapasonsList = get_list_of_diapasons(ArrFrom);
            List<List<SyntaxTree.ident>> IdentsList = new List<List<PascalABCCompiler.SyntaxTree.ident>>();
            int VarNum = 0;
            if (DiapasonsList == null)
                DiapasonsList = new List<List<PascalABCCompiler.SyntaxTree.diapason>>();
            foreach (List<SyntaxTree.diapason> Diapasons in DiapasonsList)
            {
                bool IsDynamicArray = false;
                List<SyntaxTree.method_call> Lens = new List<PascalABCCompiler.SyntaxTree.method_call>();
                if ((Diapasons.Count == 0) || (Diapasons[0] == null))
                {
                    //массив динамический, нужно делать setlength
                    IsDynamicArray = true;
                    SyntaxTree.procedure_call SetLenPC = new PascalABCCompiler.SyntaxTree.procedure_call();
                    SyntaxTree.method_call SetLenMC = new PascalABCCompiler.SyntaxTree.method_call();
                    SetLenMC.dereferencing_value = new SyntaxTree.ident("SetLength");
                    SetLenPC.func_name = SetLenMC;
                    SyntaxTree.expression_list SetLenParamsExl = new PascalABCCompiler.SyntaxTree.expression_list();
                    SetLenMC.parameters = SetLenParamsExl;

                    //индексное выражение для массива-приемника
                    SyntaxTree.addressed_value IndexerTo = IdTo;
                    foreach (List<SyntaxTree.ident> Idents in IdentsList)
                    {
                        SyntaxTree.indexer InnerInd = new PascalABCCompiler.SyntaxTree.indexer();
                        InnerInd.dereferencing_value = IndexerTo;
                        SyntaxTree.expression_list IndexersExl = new PascalABCCompiler.SyntaxTree.expression_list();
                        foreach (SyntaxTree.ident Ident in Idents)
                            IndexersExl.expressions.Add(new SyntaxTree.ident(Ident.name));
                        InnerInd.indexes = IndexersExl;
                        IndexerTo = InnerInd;
                    }

                    //индексное выражение для массива-источника
                    SyntaxTree.addressed_value IndexerFrom = IdFrom;
                    foreach (List<SyntaxTree.ident> Idents in IdentsList)
                    {
                        SyntaxTree.indexer InnerInd = new PascalABCCompiler.SyntaxTree.indexer();
                        InnerInd.dereferencing_value = IndexerFrom;
                        SyntaxTree.expression_list IndexersExl = new PascalABCCompiler.SyntaxTree.expression_list();
                        foreach (SyntaxTree.ident Ident in Idents)
                            IndexersExl.expressions.Add(new SyntaxTree.ident(Ident.name));
                        InnerInd.indexes = IndexersExl;
                        IndexerFrom = InnerInd;
                    }

                    SetLenParamsExl.expressions.Add(IndexerTo);

                    //List<SyntaxTree.method_call> Lengths = new List<PascalABCCompiler.SyntaxTree.method_call>();
                    if (Diapasons.Count == 0)
                    {
                        SyntaxTree.method_call LengthMC = new PascalABCCompiler.SyntaxTree.method_call();
                        LengthMC.dereferencing_value = new SyntaxTree.ident("Length");
                        SyntaxTree.expression_list LenMCExl = new PascalABCCompiler.SyntaxTree.expression_list();
                        LengthMC.parameters = LenMCExl;
                        LenMCExl.expressions.Add(IndexerFrom);
                        SetLenParamsExl.expressions.Add(LengthMC);
                        Lens.Add(LengthMC);
                    }
                    else
                    {
                        for (int i = 0; i < Diapasons.Count; ++i)
                        {
                            SyntaxTree.method_call LengthMC = new PascalABCCompiler.SyntaxTree.method_call();
                            LengthMC.dereferencing_value = new SyntaxTree.ident("Length");
                            SyntaxTree.expression_list LenMCExl = new PascalABCCompiler.SyntaxTree.expression_list();
                            LengthMC.parameters = LenMCExl;
                            LenMCExl.expressions.Add(IndexerFrom);
                            LenMCExl.expressions.Add(new SyntaxTree.int32_const(i));
                            SetLenParamsExl.expressions.Add(LengthMC);
                            Lens.Add(LengthMC);
                        }
                    }
                    InnerSTL.subnodes.Add(SetLenPC);
                }
                //к этому моменту вызов SetLength с параметрами сформирован
                //в Lens содержатся вызовы length по нужной размерности если массив динамический
                //теперь нужно создать циклы for и список переменных, по которым идут циклы
                List<SyntaxTree.ident> LoopIdents = new List<SyntaxTree.ident>();
                for (int i = 0; i < Math.Max(Diapasons.Count, 1); ++i)
                {
                    SyntaxTree.for_node ForNode = new PascalABCCompiler.SyntaxTree.for_node();
                    SyntaxTree.ident LoopVar = new PascalABCCompiler.SyntaxTree.ident("$i" + (VarNum++).ToString());
                    LoopIdents.Add(LoopVar);
                    ForNode.loop_variable = LoopVar;
                    ForNode.create_loop_variable = true;
                    if (IsDynamicArray)
                    {
                        ForNode.initial_value = new SyntaxTree.int32_const(0);
                        ForNode.finish_value = new PascalABCCompiler.SyntaxTree.bin_expr(Lens[i], new SyntaxTree.int32_const(1), PascalABCCompiler.SyntaxTree.Operators.Minus);
                    }
                    else
                    {
                        ForNode.initial_value = Diapasons[i].left;
                        ForNode.finish_value = Diapasons[i].right;
                    }
                    InnerSTL.subnodes.Add(ForNode);
                    ForNode.statements = new PascalABCCompiler.SyntaxTree.statement_list();
                    InnerSTL = ForNode.statements as SyntaxTree.statement_list;
                }
                IdentsList.Add(LoopIdents);
            }

            // к этому моменту создано гнездо циклов с установкой длины динамических массивов и перебором
            // вообще всех массивов. Осталось создать самое внутреннее присваивание
            SyntaxTree.addressed_value AssignIndexerFrom = IdFrom;
            SyntaxTree.addressed_value AssignIndexerTo = IdTo;
            foreach (List<SyntaxTree.ident> AssignIdents in IdentsList)
            {
                SyntaxTree.indexer FromIndexer = new PascalABCCompiler.SyntaxTree.indexer();
                SyntaxTree.indexer ToIndexer = new PascalABCCompiler.SyntaxTree.indexer();
                FromIndexer.dereferencing_value = AssignIndexerFrom;
                ToIndexer.dereferencing_value = AssignIndexerTo;

                SyntaxTree.expression_list Exl = new PascalABCCompiler.SyntaxTree.expression_list();
                foreach (SyntaxTree.ident id in AssignIdents)
                    Exl.expressions.Add(id);
                FromIndexer.indexes = Exl;
                ToIndexer.indexes = Exl;

                AssignIndexerFrom = FromIndexer;
                AssignIndexerTo = ToIndexer;
            }
            SyntaxTree.assign Assign = new PascalABCCompiler.SyntaxTree.assign();
            Assign.from = AssignIndexerFrom;
            Assign.to = AssignIndexerTo;
            Assign.operator_type = PascalABCCompiler.SyntaxTree.Operators.Assignment;
            InnerSTL.subnodes.Add(Assign);

            return OuterSTL;
        }
        /// <summary>
        /// По списку диапазонов, определяет динаимеческий или нет. Если нет, то возвращает в ind номер первого непустого диапазона
        /// </summary>
        /// <param name="diaps">Ассоциативный массив диапазонов</param>
        /// <param name="ind">выходной параметр - индекс непустого дипазона. Если динамический массив, то возвращает -1 </param>
        /// <returns></returns>
        private static bool is_dyn_arr(SyntaxTree.array_type arr)
        {
            Dictionary<int, List<SyntaxTree.diapason>> diaps = get_diapasons_from_array(arr);

            if (diaps == null)
                return true;
            foreach (int key in diaps.Keys)
            {
                foreach (SyntaxTree.diapason d in diaps[key])
                    if (d != null)
                    {
                        return false;
                    }

            }
            return true;
        }
        private static SyntaxTree.type_declarations CreateClass(string ClassName,out SyntaxTree.class_members ClassMember, VarInfoContainer Vars)
        {
            //генерация класса
            SyntaxTree.type_declarations TypeDecls = new PascalABCCompiler.SyntaxTree.type_declarations();
            SyntaxTree.type_declaration TypeDecl = new PascalABCCompiler.SyntaxTree.type_declaration();
            TypeDecls.types_decl.Add(TypeDecl);
            TypeDecl.type_name = new PascalABCCompiler.SyntaxTree.ident(ClassName);
            SyntaxTree.class_definition ClassDef = new PascalABCCompiler.SyntaxTree.class_definition();
            TypeDecl.type_def = ClassDef;
            SyntaxTree.class_body_list ClassBody = new PascalABCCompiler.SyntaxTree.class_body_list();
            ClassDef.body = ClassBody;
            ClassMember = new PascalABCCompiler.SyntaxTree.class_members();
            ClassBody.class_def_blocks.Add(ClassMember);
            ClassMember.access_mod = new PascalABCCompiler.SyntaxTree.access_modifer_node(PascalABCCompiler.SyntaxTree.access_modifer.public_modifer);

            //  генерация полей класса
            //  shared переменные
            for (int i = 0; i < Vars.SharedVariables.Count; ++i)
                ClassMember.members.Add(CreateClassMember(Vars.SharedVariables[i], ""));
            //  переменные редукции - с долларовым префиксом
            for (int i = 0; i < Vars.ReductionVariables.Count; ++i)
                ClassMember.members.Add(CreateClassMember(Vars.ReductionVariables[i], "$"));

            
            return TypeDecls;
        }
        private static SyntaxTree.procedure_definition CreateMethod(string MethodName, SyntaxTree.statement Body, string LoopVariableName, SyntaxTree.class_members ClassMember, VarInfoContainer Vars)
        {
            //  генерация метода
            SyntaxTree.procedure_definition ProcDef = new PascalABCCompiler.SyntaxTree.procedure_definition();
            //ClassMember.members.Add(ProcDef);
            SyntaxTree.procedure_header ProcHead = new PascalABCCompiler.SyntaxTree.procedure_header();
            ProcDef.proc_header = ProcHead;
            ProcHead.name = new PascalABCCompiler.SyntaxTree.method_name(null, null, new PascalABCCompiler.SyntaxTree.ident(MethodName), null);
            if (LoopVariableName != "")
            {
                //  параметр, счетчик цикла
                string ParamType = "integer";
                SyntaxTree.formal_parameters FormalParams = new PascalABCCompiler.SyntaxTree.formal_parameters();
                ProcHead.parameters = FormalParams;
                SyntaxTree.typed_parameters TypedParams = new PascalABCCompiler.SyntaxTree.typed_parameters();
                FormalParams.params_list.Add(TypedParams);
                SyntaxTree.ident_list idl = new PascalABCCompiler.SyntaxTree.ident_list();
                TypedParams.idents = idl;
                idl.Add(new SyntaxTree.ident(LoopVariableName));
                SyntaxTree.named_type_reference ntr = new PascalABCCompiler.SyntaxTree.named_type_reference();
                TypedParams.vars_type = ntr;
                ntr.Add(new SyntaxTree.ident(ParamType));
            }

            SyntaxTree.block ProcBlock = new PascalABCCompiler.SyntaxTree.block();
            ProcDef.proc_body = ProcBlock;
            ProcBlock.defs = new PascalABCCompiler.SyntaxTree.declarations();
            if (Vars.Constants.Count > 0)
            {
                SyntaxTree.consts_definitions_list cdl = new PascalABCCompiler.SyntaxTree.consts_definitions_list();
                ProcBlock.defs.defs.Add(cdl);
                //  константы - в методе
                for (int i = 0; i < Vars.Constants.Count; ++i)
                    cdl.Add(CreateClassMember(Vars.Constants[i], "") as SyntaxTree.typed_const_definition);
            }
            if ((Vars.ReductionVariables.Count > 0) || (Vars.PrivateVariables.Count > 0))
            {
                //  переменные редукции - в методе тоже, но без префикса
                SyntaxTree.variable_definitions vds = new PascalABCCompiler.SyntaxTree.variable_definitions();
                ProcBlock.defs.defs.Add(vds);
                for (int i = 0; i < Vars.ReductionVariables.Count; ++i)
                    vds.Add(CreateClassMember(Vars.ReductionVariables[i], "") as SyntaxTree.var_def_statement);
                //  и приватные переменные
                for (int i = 0; i < Vars.PrivateVariables.Count; ++i)
                    vds.Add(CreateClassMember(Vars.PrivateVariables[i], "") as SyntaxTree.var_def_statement);
            }

            if (Body is SyntaxTree.statement_list)
                ProcBlock.program_code = Body as SyntaxTree.statement_list;
            else
            {
                SyntaxTree.statement_list stl = new PascalABCCompiler.SyntaxTree.statement_list();
                stl.subnodes.Add(Body);
                ProcBlock.program_code = stl;
            }

            //присваивания для переменных редукции
            if (Vars.ReductionVariables.Count > 0)
            {
                SyntaxTree.statement_list LoopBodyInit = new PascalABCCompiler.SyntaxTree.statement_list();
                SyntaxTree.statement_list LoopBodyFinal = new PascalABCCompiler.SyntaxTree.statement_list();

                for (int i = 0; i < Vars.ReductionVariables.Count; ++i)
                {
                    //присваивание начального значения
                    SyntaxTree.assign Assign = new PascalABCCompiler.SyntaxTree.assign();
                    Assign.operator_type = PascalABCCompiler.SyntaxTree.Operators.Assignment;
                    Assign.to = new SyntaxTree.ident(Vars.ReductionVariables[i].name);
                    bool isBool = Vars.ReductionVariables[i].type.name.ToLower() == "boolean";
                    switch (Vars.ReductionActions[i])
                    {
                        case ReductionOperations.and:
                            {
                                if (isBool)
                                    Assign.from = new SyntaxTree.bool_const(true);
                                else
                                {
                                    //отрицание нуля
                                    Assign.from = new SyntaxTree.int32_const(0);
                                    LoopBodyInit.subnodes.Add(Assign);
                                    Assign = new PascalABCCompiler.SyntaxTree.assign();
                                    Assign.operator_type = PascalABCCompiler.SyntaxTree.Operators.Assignment;
                                    Assign.to = new SyntaxTree.ident(Vars.ReductionVariables[i].name);
                                    SyntaxTree.un_expr ue = new PascalABCCompiler.SyntaxTree.un_expr();
                                    ue.operation_type = PascalABCCompiler.SyntaxTree.Operators.LogicalNOT;
                                    ue.subnode = new SyntaxTree.ident(Vars.ReductionVariables[i].name);
                                    Assign.from = ue;
                                }
                                break;
                            }
                        case ReductionOperations.or:
                            if (isBool)
                                Assign.from = new SyntaxTree.bool_const(false);
                            else
                            {
                                Assign.from = new SyntaxTree.int32_const(0);
                            }
                            break;
                        case ReductionOperations.xor:  //
                        case ReductionOperations.plus: //см следующую ветку
                        case ReductionOperations.minus: Assign.from = new SyntaxTree.int32_const(0); break;
                        case ReductionOperations.mult: Assign.from = new SyntaxTree.int32_const(1); break;
                    }
                    LoopBodyInit.Add(Assign);

                    //присваивание после итерации
                    Assign = new PascalABCCompiler.SyntaxTree.assign();
                    Assign.operator_type = PascalABCCompiler.SyntaxTree.Operators.Assignment;
                    Assign.to = new SyntaxTree.ident("$" + Vars.ReductionVariables[i].name);
                    SyntaxTree.bin_expr From = new PascalABCCompiler.SyntaxTree.bin_expr();
                    From.left = new SyntaxTree.ident("$" + Vars.ReductionVariables[i].name);
                    From.right = new SyntaxTree.ident(Vars.ReductionVariables[i].name);
                    Assign.from = From;
                    switch (Vars.ReductionActions[i])
                    {
                        case ReductionOperations.and: From.operation_type = PascalABCCompiler.SyntaxTree.Operators.LogicalAND; break;
                        case ReductionOperations.or: From.operation_type = PascalABCCompiler.SyntaxTree.Operators.LogicalOR; break;
                        case ReductionOperations.xor: From.operation_type = PascalABCCompiler.SyntaxTree.Operators.BitwiseXOR; break;
                        case ReductionOperations.plus: From.operation_type = PascalABCCompiler.SyntaxTree.Operators.Plus; break;
                        case ReductionOperations.minus: From.operation_type = PascalABCCompiler.SyntaxTree.Operators.Minus; break;
                        case ReductionOperations.mult: From.operation_type = PascalABCCompiler.SyntaxTree.Operators.Multiplication; break;
                    }
                    LoopBodyFinal.Add(Assign);
                }

                //создаем обьект для блокировки в классе
                SyntaxTree.var_def_statement Lvds = new PascalABCCompiler.SyntaxTree.var_def_statement();
                SyntaxTree.ident_list Lidl = new PascalABCCompiler.SyntaxTree.ident_list();
                Lvds.vars = Lidl;
                Lidl.Add(new SyntaxTree.ident("$ReductionLock"));
                SyntaxTree.named_type_reference Lntr = new PascalABCCompiler.SyntaxTree.named_type_reference();
                Lvds.vars_type = Lntr;
                Lntr.Add(new SyntaxTree.ident("object"));
                SyntaxTree.new_expr Lne = new PascalABCCompiler.SyntaxTree.new_expr();
                Lvds.inital_value = Lne;
                Lne.type = Lntr;
                ClassMember.members.Add(Lvds);

                //создаем lock Statement на обьекте с присваиваниями в конце итерации
                SyntaxTree.lock_stmt reductionLock = new PascalABCCompiler.SyntaxTree.lock_stmt();
                reductionLock.lock_object = new SyntaxTree.ident("$ReductionLock");
                reductionLock.stmt = LoopBodyFinal;

                //собираем все вместе и присваиваем это телу процедуры
                LoopBodyInit.subnodes.AddRange(ProcBlock.program_code.subnodes);
                LoopBodyInit.subnodes.Add(reductionLock);
                ProcBlock.program_code = LoopBodyInit;
            }
            return ProcDef;
        }

        private static SyntaxTree.declaration CreateClassMember(SemanticTree.IDefinitionNode Def, string Prefix)
        {
            if (Def is SemanticTree.IConstantDefinitionNode)
            {
                SemanticTree.IConstantDefinitionNode ConstDef = Def as SemanticTree.IConstantDefinitionNode;
                SyntaxTree.typed_const_definition tcd = new PascalABCCompiler.SyntaxTree.typed_const_definition();
                tcd.const_name = new PascalABCCompiler.SyntaxTree.ident(Prefix + ConstDef.name);
                tcd.const_type = ConvertToSyntaxType(ConstDef.type as type_node);
                tcd.const_value = ConvertConstant(ConstDef.constant_value);
                return tcd;
            }
            else
            {
                SemanticTree.IVAriableDefinitionNode VarDef = Def as SemanticTree.IVAriableDefinitionNode;
                SyntaxTree.var_def_statement vds = new PascalABCCompiler.SyntaxTree.var_def_statement();
                SyntaxTree.ident_list idl = new PascalABCCompiler.SyntaxTree.ident_list();
                vds.vars = idl;
                idl.Add(new SyntaxTree.ident(Prefix + VarDef.name));
                vds.vars_type = ConvertToSyntaxType(VarDef.type as type_node);
                return vds;
            }
        }

        private static SyntaxTree.statement_list CreateInitPart(string ClassName, string ObjName, VarInfoContainer Vars)
        {
            SyntaxTree.statement_list stl = new PascalABCCompiler.SyntaxTree.statement_list();

            //Var Statement - объявление экземпляра обьекта-функции
            SyntaxTree.var_statement ClassVar = new PascalABCCompiler.SyntaxTree.var_statement();
            stl.subnodes.Add(ClassVar);
            SyntaxTree.var_def_statement ClassVarDef = new PascalABCCompiler.SyntaxTree.var_def_statement();
            ClassVar.var_def = ClassVarDef;
            SyntaxTree.ident_list ClassIdl = new PascalABCCompiler.SyntaxTree.ident_list();
            ClassVarDef.vars = ClassIdl;
            ClassIdl.idents.Add(new PascalABCCompiler.SyntaxTree.ident(ObjName));
            SyntaxTree.named_type_reference ClassTypeNTR = new PascalABCCompiler.SyntaxTree.named_type_reference();
            ClassVarDef.vars_type = ClassTypeNTR;
            ClassTypeNTR.names.Add(new PascalABCCompiler.SyntaxTree.ident(ClassName));
            SyntaxTree.new_expr ClassInitNE = new PascalABCCompiler.SyntaxTree.new_expr();
            ClassVarDef.inital_value = ClassInitNE;
            SyntaxTree.named_type_reference ClassInitNTR = new PascalABCCompiler.SyntaxTree.named_type_reference();
            ClassInitNE.type = ClassInitNTR;
            ClassInitNTR.names.Add(new PascalABCCompiler.SyntaxTree.ident(ClassName));

            //создаем присваивания разделяемым переменным
            for (int i = 0; i < Vars.SharedVariables.Count; ++i)
            {
                string VarName = Vars.SharedVariables[i].name;

                SyntaxTree.dot_node DotNode = new PascalABCCompiler.SyntaxTree.dot_node();
                DotNode.left = new SyntaxTree.ident(ObjName);
                DotNode.right = new SyntaxTree.ident(VarName);

                SyntaxTree.array_type arrType = ConvertToSyntaxType(Vars.SharedVariables[i].type as type_node) as SyntaxTree.array_type;
                if (arrType != null && !is_dyn_arr(arrType))
                {
                    stl.subnodes.Add(AssignArrs(arrType, new SyntaxTree.ident(VarName), DotNode));
                }
                else
                {
                    SyntaxTree.assign Assign = new PascalABCCompiler.SyntaxTree.assign();
                    Assign.operator_type = PascalABCCompiler.SyntaxTree.Operators.Assignment;
                    Assign.from = new SyntaxTree.ident(VarName);
                    Assign.to = DotNode;
                    stl.subnodes.Add(Assign);
                }
            }
            //создаем присваивания переменным редукции
            for (int i = 0; i < Vars.ReductionVariables.Count; ++i)
            {
                string VarName = Vars.ReductionVariables[i].name;

                SyntaxTree.dot_node DotNode = new PascalABCCompiler.SyntaxTree.dot_node();
                DotNode.left = new SyntaxTree.ident(ObjName);
                DotNode.right = new SyntaxTree.ident("$" + VarName);

                SyntaxTree.array_type arrType = ConvertToSyntaxType(Vars.ReductionVariables[i].type as type_node) as SyntaxTree.array_type;
                if (arrType != null && !is_dyn_arr(arrType))
                {
                    stl.subnodes.Add(AssignArrs(arrType, new SyntaxTree.ident(VarName), DotNode));
                }
                else
                {
                    SyntaxTree.assign Assign = new PascalABCCompiler.SyntaxTree.assign();
                    Assign.operator_type = PascalABCCompiler.SyntaxTree.Operators.Assignment;
                    Assign.from = new SyntaxTree.ident(VarName);
                    Assign.to = DotNode;
                    stl.subnodes.Add(Assign);
                }
            }
            return stl;
        }

        private static SyntaxTree.statement_list CreateFinalPart(string ObjName, VarInfoContainer Vars)
        {
            SyntaxTree.statement_list stl = new PascalABCCompiler.SyntaxTree.statement_list();

            //создаем присваивания разделяемым переменным
            for (int i = 0; i < Vars.SharedVariables.Count; ++i)
            {
                string VarName = Vars.SharedVariables[i].name;
                if (LoopVariables.Contains(VarName.ToLower()))
                    continue;

                SyntaxTree.dot_node DotNode = new PascalABCCompiler.SyntaxTree.dot_node();
                DotNode.left = new SyntaxTree.ident(ObjName);
                DotNode.right = new SyntaxTree.ident(VarName);

                SyntaxTree.array_type arrType = ConvertToSyntaxType(Vars.SharedVariables[i].type as type_node) as SyntaxTree.array_type;
                if (arrType != null && !is_dyn_arr(arrType))
                {
                    stl.subnodes.Add(AssignArrs(arrType, DotNode, new SyntaxTree.ident(VarName)));
                }
                else
                {
                    SyntaxTree.assign Assign = new PascalABCCompiler.SyntaxTree.assign();
                    Assign.operator_type = PascalABCCompiler.SyntaxTree.Operators.Assignment;
                    Assign.to = new SyntaxTree.ident(VarName);
                    Assign.from = DotNode;
                    stl.subnodes.Add(Assign);
                }

            }

            //создаем присваивания переменным редукции
            for (int i = 0; i < Vars.ReductionVariables.Count; ++i)
            {
                string VarName = Vars.ReductionVariables[i].name;

                SyntaxTree.dot_node DotNode = new PascalABCCompiler.SyntaxTree.dot_node();
                DotNode.left = new SyntaxTree.ident(ObjName);
                DotNode.right = new SyntaxTree.ident("$" + VarName);

                SyntaxTree.array_type arrType = ConvertToSyntaxType(Vars.ReductionVariables[i].type as type_node) as SyntaxTree.array_type;
                if (arrType != null && !is_dyn_arr(arrType))
                {
                    stl.subnodes.Add(AssignArrs(arrType, DotNode, new SyntaxTree.ident(VarName)));
                }
                else
                {
                    SyntaxTree.assign Assign = new PascalABCCompiler.SyntaxTree.assign();
                    Assign.operator_type = PascalABCCompiler.SyntaxTree.Operators.Assignment;
                    Assign.to = new SyntaxTree.ident(VarName);
                    Assign.from = DotNode;
                    stl.subnodes.Add(Assign);
                }
            }

            return stl;
        }

        private static SyntaxTree.procedure_call CreateOMPParallelForCall(SyntaxTree.dot_node dn, SyntaxTree.expression FromInclusive, SyntaxTree.expression ToInclusive)
        {
            SyntaxTree.procedure_call pc = new PascalABCCompiler.SyntaxTree.procedure_call();
            SyntaxTree.method_call mc = new PascalABCCompiler.SyntaxTree.method_call();
            pc.func_name = mc;
            mc.dereferencing_value = CreateTPLFunctionReference("For");

            //Здесь прибавляем единицу ко второму параметру, так как в паскале верхняя граница включается, а в parallel.for - нет
            SyntaxTree.bin_expr ToExclusive = new SyntaxTree.bin_expr();
            ToExclusive.left = ToInclusive;
            ToExclusive.right = new SyntaxTree.int32_const(1);
            ToExclusive.operation_type = SyntaxTree.Operators.Plus;

            mc.parameters = new PascalABCCompiler.SyntaxTree.expression_list();
            mc.parameters.Add(FromInclusive);
            mc.parameters.Add(ToExclusive);
            mc.parameters.Add(dn);
            return pc;
        }

        private static if_node CreateIfCondition(syntax_tree_visitor syntax_tree_visitor, statements_list ifthen, statements_list ifelse, location loc)
        {
            //сохраняем контекст
            ContextInfo contextInfo = new ContextInfo(syntax_tree_visitor);
            
            if_node res = null;
            try
            {
                SyntaxTree.if_node if_node = new PascalABCCompiler.SyntaxTree.if_node();
                SyntaxTree.un_expr une = new PascalABCCompiler.SyntaxTree.un_expr();
                une.operation_type = SyntaxTree.Operators.LogicalNOT;
                une.subnode = new SyntaxTree.ident(InParallelSection);
                if_node.condition = une;
                if_node.then_body = new SyntaxTree.empty_statement();
                if_node.then_body = new SyntaxTree.empty_statement();
                res = syntax_tree_visitor.convert_strong(if_node) as if_node;
                res.then_body = ifthen;
                res.else_body = ifelse;
                res.location = loc;
            }
            finally
            {
                //восстанавливаем контекст
                contextInfo.RestoreContext(syntax_tree_visitor);
            }
            return res;
        }

        private static SyntaxTree.dot_node CreateTPLFunctionReference(string FunctionName)
        {
            //Создаем конструкцию вида "System.Threading.Tasks.Parallel.<FunctionName>"
            SyntaxTree.dot_node dn1 = new SyntaxTree.dot_node();
            dn1.left = new SyntaxTree.ident("System");
            dn1.right = new SyntaxTree.ident("Threading");

            SyntaxTree.dot_node dn2 = new SyntaxTree.dot_node();
            dn2.right = new SyntaxTree.ident("Tasks");
            dn2.left = dn1;

            SyntaxTree.dot_node dn3 = new SyntaxTree.dot_node();
            dn3.right = new SyntaxTree.ident("Parallel");
            dn3.left = dn2;

            SyntaxTree.dot_node Result = new SyntaxTree.dot_node();
            Result.right = new SyntaxTree.ident(FunctionName);
            Result.left = dn3;
            
            return Result;
        }

        private static void CreateInParallelVariable(syntax_tree_visitor syntax_tree_visitor, out string VarName)
        {
            //сохраняем контекст
            ContextInfo contextInfo = new ContextInfo(syntax_tree_visitor);

            VarName = "$InParallelSection";

            try
            {
                //создание и конвертирование переменной                
                SyntaxTree.var_def_statement vds = new PascalABCCompiler.SyntaxTree.var_def_statement();
                SyntaxTree.variable_definitions var_def = new PascalABCCompiler.SyntaxTree.variable_definitions(vds, null);
                SyntaxTree.ident_list idl = new PascalABCCompiler.SyntaxTree.ident_list();
                vds.vars = idl;
                idl.Add(new SyntaxTree.ident(VarName));
                SyntaxTree.named_type_reference ntr = new PascalABCCompiler.SyntaxTree.named_type_reference();
                vds.vars_type = ntr;
                ntr.names.Add(new PascalABCCompiler.SyntaxTree.ident("boolean"));
                vds.inital_value = new PascalABCCompiler.SyntaxTree.ident("false");
                syntax_tree_visitor.visit(var_def);
            }
            finally
            {
                //восстанавливаем контекст
                contextInfo.RestoreContext(syntax_tree_visitor);
            }
            InParallelSectionCreated = true;
        }
        
        private static SyntaxTree.if_node CreateNestedRegionBorder(bool ifEnter)
        {
            SyntaxTree.if_node result = new PascalABCCompiler.SyntaxTree.if_node();
            SyntaxTree.un_expr ue = new PascalABCCompiler.SyntaxTree.un_expr();
            ue.operation_type = PascalABCCompiler.SyntaxTree.Operators.LogicalNOT;
            ue.subnode = new SyntaxTree.ident(TreeConverter.compiler_string_consts.OMP_NESTED);
            result.condition = ue;

            SyntaxTree.assign AssignInParVar = new PascalABCCompiler.SyntaxTree.assign();
            AssignInParVar.operator_type = PascalABCCompiler.SyntaxTree.Operators.Assignment;
            AssignInParVar.from = new SyntaxTree.ident(ifEnter ? "true" : "false");
            AssignInParVar.to = new SyntaxTree.ident(InParallelSection);

            result.then_body = AssignInParVar;

            return result;
        }

        private static SyntaxTree.expression ConvertConstant(SemanticTree.IConstantNode value)
        {
            if (value is bool_const_node)
                return new SyntaxTree.ident((value as bool_const_node).constant_value ? "true" : "false");
            else if (value is null_const_node)
                return new SyntaxTree.nil_const();
            else if (value is byte_const_node)
                return new SyntaxTree.int32_const((value as byte_const_node).constant_value);
            else if (value is char_const_node)
                return new SyntaxTree.char_const((value as char_const_node).constant_value);
            else if (value is double_const_node)
                return new SyntaxTree.double_const((value as double_const_node).constant_value);
            else if (value is float_const_node)
                return new SyntaxTree.double_const((value as float_const_node).constant_value);
            else if (value is int_const_node)
                return new SyntaxTree.int32_const((value as int_const_node).constant_value);
            else if (value is long_const_node)
                return new SyntaxTree.int64_const((value as long_const_node).constant_value);
            else if (value is sbyte_const_node)
                return new SyntaxTree.int32_const((value as sbyte_const_node).constant_value);
            else if (value is short_const_node)
                return new SyntaxTree.int32_const((value as short_const_node).constant_value);
            else if (value is uint_const_node)
                return new SyntaxTree.uint64_const((value as uint_const_node).constant_value);
            else if (value is ulong_const_node)
                return new SyntaxTree.uint64_const((value as ulong_const_node).constant_value);
            else if (value is ushort_const_node)
                return new SyntaxTree.int32_const((value as ushort_const_node).constant_value);
            else if (value is string_const_node)
                return new SyntaxTree.string_const((value as string_const_node).constant_value);
            else if (value is array_const)
            {
                array_const ac = value as array_const;
                SyntaxTree.expression_list el = new PascalABCCompiler.SyntaxTree.expression_list();
                for (int i = 0; i < ac.element_values.Count; ++i)
                    el.Add(ConvertConstant(ac.element_values[i]));
                SyntaxTree.array_const synAC = new PascalABCCompiler.SyntaxTree.array_const();
                synAC.elements = el;
                return synAC;
            }
            else if (value is enum_const_node)
            {
                //Есть сомнения, что будет работать во всех случаях
                enum_const_node ec = value as enum_const_node;
                SyntaxTree.ident r = new SyntaxTree.ident((ec.type as common_type_node).const_defs[ec.constant_value].name);
                SyntaxTree.ident l = new SyntaxTree.ident(ec.type.name);
                return new SyntaxTree.dot_node(l, r);
            }
            else if (value is record_constant)
            {
                record_constant rc = value as record_constant;
                SyntaxTree.record_const synRC = new PascalABCCompiler.SyntaxTree.record_const();
                for (int i = 0; i < rc.field_values.Count; ++i)
                {
                    SyntaxTree.record_const_definition rcd = new PascalABCCompiler.SyntaxTree.record_const_definition();
                    rcd.name = rc.record_const_definition_list[i].name;
                    rcd.val = rc.record_const_definition_list[i].val;
                    synRC.rec_consts.Add(rcd);
                }
                return synRC;
            }
            else
                throw new Exception("Не реализовано");         //выяснить, что тут может быть
        }

        /// <summary>
        /// выдает свободное имя
        /// </summary>
        /// <param name="template">шаблон</param>
        /// <param name="StartFrom">перебирать имена, приписывая к шаблону числа начиная с этой позиции. После того как найдет - возвращает следующее число</param>
        /// <param name="context">контекст, в котором искать</param>
        /// <returns>первое свободное имя</returns>
        private static string GetFreeName(string template, ref int StartFrom, compilation_context context)
        {
            string name;
            while (context.CurrentScope.Find((name = string.Format(template + StartFrom.ToString(), StartFrom++))) != null)
                ;
            return name;
        }

        private static VarInfoContainer GetVarInfoContainer(VarFinderSyntaxVisitor VFVis, List<ReductionDirective> Reductions, List<string> PrivateVars, syntax_tree_visitor visitor, SyntaxTree.compiler_directive dir)
        {
            if (Reductions == null)
                Reductions = new List<ReductionDirective>();
            if (PrivateVars == null)
                PrivateVars = new List<string>();
            VarInfoContainer Result = new VarInfoContainer();

            //константы копируются сразу
            Result.Constants = VFVis.Constants;

            //список переменных для редукции
            //ищем переменные с такими именами, проверяем их наличие, тип и если все хорошо - добавляем в список
            foreach (ReductionDirective rd in Reductions)
                foreach (string rdVarName in rd.variables)
                {
                    if (LoopVariables.Contains(rdVarName))
                    {
                        visitor.AddWarning(new PascalABCCompiler.Errors.CommonWarning(String.Format(PascalABCCompiler.StringResources.Get("OMPERROR_REDUCTION_WITH_LOOPVAR_{0}"), rdVarName), visitor.CurrentDocument.file_name, dir.source_context.begin_position.line_num, dir.source_context.begin_position.column_num));
                        continue;
                    }
                    SymbolInfo si = visitor.context.find_first(rdVarName);
                    if (si == null)
                    {
                        visitor.AddWarning(new PascalABCCompiler.Errors.CommonWarning(String.Format(PascalABCCompiler.StringResources.Get("OMPERROR_UNKNOWN_VARNAME_{0}"), rdVarName), visitor.CurrentDocument.file_name, dir.source_context.begin_position.line_num, dir.source_context.begin_position.column_num));
                        continue;
                    }
                    if (!(si.sym_info is SemanticTree.IVAriableDefinitionNode))
                    {
                        visitor.AddWarning(new PascalABCCompiler.Errors.CommonWarning(String.Format(PascalABCCompiler.StringResources.Get("OMPERROR_NAME_IS_NOT_VAR_{0}"), rdVarName), visitor.CurrentDocument.file_name, dir.source_context.begin_position.line_num, dir.source_context.begin_position.column_num));
                        continue;
                    }
                    if (!IsValidVarForReduction(si.sym_info as SemanticTree.IVAriableDefinitionNode))
                    {
                        visitor.AddWarning(new PascalABCCompiler.Errors.CommonWarning(String.Format(PascalABCCompiler.StringResources.Get("OMPERROR_IS_NOT_POSSIBLE_REDUCTION_WITH_THIS_VAR_{0}"), rdVarName), visitor.CurrentDocument.file_name, dir.source_context.begin_position.line_num, dir.source_context.begin_position.column_num));
                        continue;
                    }
                    Result.ReductionVariables.Add(si.sym_info as SemanticTree.IVAriableDefinitionNode);
                    Result.ReductionActions.Add(rd.Oper);
                    //for (int i = 0; i < VFVis.Variables.Count; ++i)
                    //    if (VFVis.Variables[i].name.ToLower() == rdVarName.ToLower())
                    //    {
                    //        Result.ReductionVariables.Add(VFVis.Variables[i]);
                    //        Result.ReductionActions.Add(rd.Oper);
                    //        break;
                    //    }
                }

            //приватные переменные - аналогично, но без проверки на тип
            foreach (string privateVar in PrivateVars)
            {
                SymbolInfo si = visitor.context.find_first(privateVar);
                if (si == null)
                {
                    visitor.AddWarning(new PascalABCCompiler.Errors.CommonWarning(String.Format(PascalABCCompiler.StringResources.Get("OMPERROR_UNKNOWN_VARNAME_{0}"), privateVar), visitor.CurrentDocument.file_name, dir.source_context.begin_position.line_num, dir.source_context.begin_position.column_num));
                    continue;
                }
                if (!(si.sym_info is SemanticTree.IVAriableDefinitionNode))
                {
                    visitor.AddWarning(new PascalABCCompiler.Errors.CommonWarning(String.Format(PascalABCCompiler.StringResources.Get("OMPERROR_NAME_IS_NOT_VAR_{0}"), privateVar), visitor.CurrentDocument.file_name, dir.source_context.begin_position.line_num, dir.source_context.begin_position.column_num));
                    continue;
                }
                Result.PrivateVariables.Add(si.sym_info as SemanticTree.IVAriableDefinitionNode);
            }

            //по всем переменным:
            //если она не приватная и по ней нет редукции - переписываем ее в список разделяемых
            foreach (SemanticTree.IVAriableDefinitionNode var in VFVis.Variables)
            {
                bool NotShared = false;
                foreach (SemanticTree.IVAriableDefinitionNode rdn in Result.ReductionVariables)
                    if (rdn.name.ToLower() == var.name.ToLower())
                    {
                        NotShared = true;
                        break;
                    }
                if (!NotShared)
                    foreach (SemanticTree.IVAriableDefinitionNode prVar in Result.PrivateVariables)
                        if (prVar.name.ToLower() == var.name.ToLower())
                        {
                            NotShared = true;
                            break;
                        }
                if (!NotShared)
                    Result.SharedVariables.Add(var);
            }

            return Result;
        }
        private static bool IsValidVarForReduction(SemanticTree.IVAriableDefinitionNode var)
        {
            //допустимые типы:
            //shortint	byte	smallint	word	integer	longword	int64	uint64	single	real
            //+ страховка на случай наличия записей вида "type integer = object"
            if (var.type.type_special_kind != PascalABCCompiler.SemanticTree.type_special_kind.none_kind)
                return false;
            if (var.type.is_class)
                return false;
            return ((var.type.name.ToLower() == "shortint")
                || (var.type.name.ToLower() == "byte")
                || (var.type.name.ToLower() == "smallint")
                || (var.type.name.ToLower() == "integer")
                || (var.type.name.ToLower() == "longword")
                || (var.type.name.ToLower() == "word")
                || (var.type.name.ToLower() == "int64")
                || (var.type.name.ToLower() == "uint64")
                || (var.type.name.ToLower() == "single")
                || (var.type.name.ToLower() == "real")
                || (var.type.name.ToLower() == "boolean"));
        }
        #endregion
    }
}