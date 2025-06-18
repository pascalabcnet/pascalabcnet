// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConverter;

//Визитор предназначен для обхода поддерева синтаксического дерева и поиска
//обращений к константам и элементам массива - их не получится найти в семантическом дереве.
//Запускать через RunVisitor()
//После работы визитора в нем содержится:
//- Список констант, обьявленных внутри функций или класса. Их нужно включать в генерируемую функцию в любом случае.
//- Список локальных переменных, параметров функций и полей класса. Их нужно включать только если генерируется обьект-функция.
//- Список локальных блочных переменных. Если они есть - генерировать объект-функцию и включать в нее все локальные переменные,
//      иначе можно обойтись просто локальной функцией и переменные не включать.

namespace PascalABCCompiler.TreeConverter
{
    class VarFinderSyntaxVisitor : SyntaxTree.WalkingVisitorNew
    {
        /// <summary>
        /// Содержит список определений констант, объявленных внутри функций [и класса].
        /// </summary>
        public List<SemanticTree.IConstantDefinitionNode> Constants =
            new List<SemanticTree.IConstantDefinitionNode>();
        /// <summary>
        /// Содержит список определений переменных
        /// </summary>
        public List<SemanticTree.IVAriableDefinitionNode> Variables = 
            new List<SemanticTree.IVAriableDefinitionNode>();
        /// <summary>
        /// Необходимо инициализировать для правильного поиска.
        /// </summary>
        private compilation_context context;
        /// <summary>
        /// Признак того, что мы распараллеливаем цикл for. Установить в true перед обходом тела распараллеливаемого цикла.
        /// Нужно для коррекции continue и break
        /// </summary>
        private bool isForNode = false;     
        /// <summary>
        /// Признак обхода счетчика цикла.
        /// При запрете объявления счетчиков цикла вне самого цикла - не нужно
        /// </summary>
        private bool isLoopVariable = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="root">Корень обходимого поддерева</param>
        /// <param name="context">Контекст. Необходим для поиска переменных</param>
        /// <param name="isForNode">True при обходе цикла, False при обходе секций</param>
        public VarFinderSyntaxVisitor(syntax_tree_node root, compilation_context context, bool isForNode)
        {
            this.isForNode = isForNode;
            this.context = context;
            ProcessNode(root);
            FixResultVar();
        }

        /// <summary>
        /// Исправление VariableDefinitionNode для переменной Result функций. 
        /// В семантике эта переменная называется $rv_{FuncName}, а нам нужно имя result
        /// Создается новый фиктивный экземпляр local_variable с тем же типом и именем result и возвращается вместо старого.
        /// </summary>
        private void FixResultVar()
        {
            for (int i=0; i<Variables.Count; ++i)
                if (Variables[i].name.StartsWith("$rv_"))
                {
                    TreeRealization.local_variable OldResult = Variables[i] as TreeRealization.local_variable;
                    TreeRealization.local_variable NewResult = new PascalABCCompiler.TreeRealization.local_variable(StringConstants.result_var_name, OldResult.type, OldResult.function, OldResult.loc);
                    Variables[i] = NewResult;
                }
        }

        public override void visit(ident value)
        {
            if (value == null)
                return;

            int pos = value.name.LastIndexOf('`');
            if (pos != -1)
                value.name = value.name.Remove(pos);

            SymbolInfo si = context.find_first(value.name);
            if (si == null)
                return;     //ничего не нашли => переменная совсем локальная, никуда добавлять не нужно

            if ((si.sym_info is SemanticTree.ICommonParameterNode)      //параметр или
                || (si.sym_info is SemanticTree.ILocalVariableNode)     //локальная переменная
                || (si.sym_info is SemanticTree.ICommonClassFieldNode)  //поле класса
                || (si.sym_info is SemanticTree.ILocalBlockVariableNode)//локальная блочная переменная
                || isLoopVariable)                                      //счетчик цикла
            {
                if (!Variables.Contains(si.sym_info as SemanticTree.IVAriableDefinitionNode))
                    Variables.Add(si.sym_info as SemanticTree.IVAriableDefinitionNode);
            }
            else if ((si.sym_info is SemanticTree.ICommonFunctionConstantDefinitionNode)//константа из функции
                     ||(si.sym_info is SemanticTree.IClassConstantDefinitionNode)) //константа из класса
            {
                if (!Constants.Contains(si.sym_info as SemanticTree.IConstantDefinitionNode))
                    Constants.Add(si.sym_info as SemanticTree.IConstantDefinitionNode);
            }
        }
        public override void visit(dot_node _dot_node)
        {
            ProcessNode(_dot_node.left);
            //правую часть не обходим
        }
        public override void visit(for_node _for_node)
        {
            if (!_for_node.create_loop_variable && (_for_node.type_name == null))
                throw new OpenMPException("Счетчик цикла должен быть обьявлен в заголовке цикла", _for_node.source_context);

            bool _isForNode = isForNode;
            isForNode = false;

            isLoopVariable = true;
            ProcessNode(_for_node.loop_variable);
            isLoopVariable = false;
            ProcessNode(_for_node.initial_value);
            ProcessNode(_for_node.increment_value);
            ProcessNode(_for_node.finish_value);
            ProcessNode(_for_node.type_name);
            ProcessNode(_for_node.statements);

            isForNode = _isForNode;
        }
        //public override void visit(method_call _method_call)
        public override void visit(procedure_call _proc_call)
        {
            if (isForNode)
            if ((_proc_call.func_name!= null)
                && (_proc_call.func_name is ident))
            {
                if ((_proc_call.func_name as ident).name.ToLower() == "continue")
                    (_proc_call.func_name as ident).name = "exit";
                if ((_proc_call.func_name as ident).name.ToLower() == "break")
                    throw new OpenMPException("Нельзя использовать break в распараллеливаемом цикле", _proc_call.source_context);
            }
            base.visit(_proc_call);
        }
    }
}
