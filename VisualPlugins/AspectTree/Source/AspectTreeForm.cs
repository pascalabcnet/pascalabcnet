using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using PascalABCCompiler.Errors;

namespace VisualPascalABCPlugins
{
    public partial class AspectTreeForm : Form
    {
        public IVisualEnvironmentCompiler VisualEnvironmentCompiler;

        // Препроцессор для обработки аспектов
        public PascalABCCompiler.Preprocessor2.Preprocessor2 AspectPreprocessor;
        // Структура для хранения связи между кодом до работы препроцессора и после
        public PascalABCCompiler.ParserTools.SourceContextMap AspectScm;
        // Ошибки, возникающие в препроцессоре при сборе аспектов
        public List<Error> AspectErrors;
        // Имя текущего аспекта
        public string AspectTxtName;        
        // Текст текущего аспекта
        public string AspectTxt;
        // Главный файл
        public string RootFileName;
        // Текущий проект
        public PascalABCCompiler.IProjectInfo RootProjectInfo;
        // Вкладка с аспектами
        VisualPascalABC.CodeFileDocumentControl TabWithAspects;

        //public List<string> UsedUnits;

        // Конструктор формы
        public AspectTreeForm()
        {
            InitializeComponent();
            // Инициализация данных плагина
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, AspectTree_VisualPascalABCPlugin.StringsPrefix);
            SaveButtonsEnabled = false;
            SaveOneButtonEnabled = false;
            InsertButtonsEnabled = false;

            // Создание подобъектов плагина
            this.AspectScm = new PascalABCCompiler.ParserTools.SourceContextMap();
            this.AspectErrors = new List<Error>();
            //this.UsedUnits = new List<string>();
            PascalABCCompiler.SourceFilesProviderDelegate sourceFilesProvider = PascalABCCompiler.SourceFilesProviders.DefaultSourceFilesProvider;
            this.AspectPreprocessor = new PascalABCCompiler.Preprocessor2.Preprocessor2(sourceFilesProvider);           
        }

        // Сброс значений полей
        private void AspectPreprocessorReset()
        {
            this.AspectPreprocessor.AspectsNames.Clear();
            this.AspectPreprocessor.AspectsMap.Clear();
            this.AspectPreprocessor.SourceContextMap = new PascalABCCompiler.ParserTools.SourceContextMap();
            this.AspectTxt = "";
            this.AspectErrors.Clear();
            this.AspectPreprocessor.InAspect = false;
            this.AspectPreprocessor.CurrentAspectDirective = "";
            //Очищаем список ошибок
            VisualPascalABC.WorkbenchServiceFactory.Workbench.ErrorsListWindow.ClearErrorList();
            //this.UsedUnits.Clear();
        }

        // Загрузка
        private void AspectTreeForm_Load(object sender, EventArgs e)
        {

        }

        // Сброс значений перед закрытием
        private void AspectTreeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;

            AspectPreprocessorReset();
            BuildButtonsEnabled = true;
            TemplateButtonEnabled = true;
            SaveButtonsEnabled = false;
            SaveOneButtonEnabled = false;
            InsertButtonsEnabled = false;
            //Отключаем обработчик
            VisualEnvironmentCompiler.StandartCompiler.OnChangeCompilerState -= Compiler_OnChangeCompilerState;
            treeView.Nodes.Clear();
            treeView.Invalidate();
            treeView.Refresh();
            treeView.Update();
            //Очищаем список ошибок
            VisualPascalABC.WorkbenchServiceFactory.Workbench.ErrorsListWindow.ClearErrorList();
            // Закрываем вкладку с аспектами
            if (TabWithAspects != null)
                VisualPascalABC.VisualPABCSingleton.MainForm.CloseAspPlugin(TabWithAspects);
            // Clear Buffer

        }
       
        private void AspectTreeForm_Shown(object sender, EventArgs e)
        {            
            // Добавляем новый обработчик событий компилятора
            VisualEnvironmentCompiler.StandartCompiler.OnChangeCompilerState += new PascalABCCompiler.ChangeCompilerStateEventDelegate(Compiler_OnChangeCompilerState);
            treeView.Nodes.Clear();
        }
        
        // Изменение статуса активности кнопок
        bool BuildButtonsEnabled
        {
            set
            {
                tbRebuild.Enabled = tbBuild.Enabled = value;
            }
            get
            {
                return tbRebuild.Enabled;
            }
        }

        bool SaveButtonsEnabled
        {
            set
            {
                toolStripButton1.Enabled = value;
            }
            get
            {
                return toolStripButton1.Enabled;
            }
        }

        bool SaveOneButtonEnabled
        {
            set
            {
                toolStripButton2.Enabled = value;
            }
            get
            {
                return toolStripButton2.Enabled;
            }
        }

        bool TemplateButtonEnabled
        {
            set
            {
                toolStripButton3.Enabled  = value;
            }
            get
            {
                return toolStripButton3.Enabled;
            }
        }

        bool InsertButtonsEnabled
        {
            set
            {
                toolStripButton4.Enabled = toolStripButton5.Enabled = value;
            }
            get
            {
                return toolStripButton5.Enabled;
            }
        }
        
        // Обработчик изменения состояния компилятора
        void Compiler_OnChangeCompilerState(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FileName)
        {
            switch (State)
            {
                case PascalABCCompiler.CompilerState.CompilationStarting:
                    BuildButtonsEnabled = false;                
                    this.Refresh();
                    break;               
                case PascalABCCompiler.CompilerState.CompilationFinished:
                    /*
                    TemplateButtonEnabled = false;
                    ShowTree();
                    asp_count = this.treeView.Nodes.Count;
                    SaveButtonsEnabled = true;
                    if (asp_count <= 1)
                    {
                        BuildButtonsEnabled = true;
                        TemplateButtonEnabled = true;
                        SaveButtonsEnabled = false;
                        SaveOneButtonEnabled = false;
                        treeView.Nodes.Clear();
                        treeView.Invalidate();
                        treeView.Refresh();
                        treeView.Update();
                        AspectPreprocessorReset();
                    }                 
                    */
                    break;                                 
            }
        }

        
        // Построение и отображение дерева аспектов
        public void ShowTree()
        {           
            try
            {                
                string[] file_names = new string[100];
                file_names[0] = VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument.FileName;
                
                treeView.Nodes.Clear();
                treeView.Invalidate();
                treeView.Refresh();                
                treeView.Update();

                AspectPreprocessorReset();
                
                if (!VisualPascalABC.ProjectFactory.Instance.ProjectLoaded)
                {
                    RootFileName = file_names[0];

                    //for (int i = 0; i < UsedUnits.Count; i++)
                    //  file_names[i] = UsedUnits[i];                    
                }
                else
                {
                    //work with project
                    RootProjectInfo = VisualPascalABC.ProjectFactory.Instance.CurrentProject;
                    RootFileName = RootProjectInfo.MainFile; 
                    int i = 0;
                    foreach (PascalABCCompiler.IFileInfo fi in RootProjectInfo.SourceFiles)
                    {
                        file_names[i] = fi.Name;
                        i++;
                    }                    
                }

                List<string> temp = new List<string>();                    
                treeView.Nodes.Add("&all");
                foreach (string name in file_names)
                {
                    string[] files = new string[1];
                    files[0] = name;

                    AspectTxt = AspectPreprocessor.Build(files, AspectErrors, AspectScm);

                    // Вывод ошибок                    
                    if (AspectErrors.Count > 0)
                    {
                        PascalABCCompiler.CompilerType ct = VisualEnvironmentCompiler.DefaultCompilerType;
                        VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Standart;
                        VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.Build, null);
                        VisualEnvironmentCompiler.DefaultCompilerType = ct;
                    }
                    else
                    {
                        for (int i = 0; i < AspectPreprocessor.CompilerDirectives.Count; i++)
                        {
                            if (AspectPreprocessor.CompilerDirectives[i].Name.text == "asp")
                                if (!(temp.Contains(AspectPreprocessor.CompilerDirectives[i].Directive.text)))
                                {
                                    treeView.Nodes.Add(AspectPreprocessor.CompilerDirectives[i].Directive.text);
                                    temp.Add(AspectPreprocessor.CompilerDirectives[i].Directive.text);
                                }
                                
                                //aspdata
                                string s = AspectPreprocessor.GetAspects(AspectPreprocessor.CompilerDirectives, AspectScm);
                                if (AspectPreprocessor.CompilerDirectives[i].Name.text == "aspdata")
                                {
                                    string t = AspectPreprocessor.CompilerDirectives[i].Directive.text;
                                    if (t.IndexOf(' ') > 0)
                                    {
                                        string n = t.Substring(0, t.IndexOf(' ')).Trim();
                                        string param = t.Substring(t.IndexOf(' ')).Trim();
                                        if (AspectPreprocessor.AspectsMap.ContainsKey(n))
                                        {
                                            int c = param.IndexOf(' ');
                                            if (c > 0)
                                            {
                                                AspectPreprocessor.AspectsMap[n].Author = param.Substring(0, c).Trim();
                                                if (param.IndexOf(' ',c+1) >0)
                                                    AspectPreprocessor.AspectsMap[n].Version = param.Substring(c, param.IndexOf(' ',c+1) - c) .Trim();
                                                else
                                                    AspectPreprocessor.AspectsMap[n].Version = param.Substring(c).Trim();
                                            }
                                        }
                                    }
                                }
                                //end aspdata
                                
                        }
                        
                        //sozdat etot file esli netu
                        //VisualPascalABC.VisualPABCSingleton.MainForm.OpenFile(Path.GetDirectoryName(file_names[0]) + "\\" + Path.GetFileNameWithoutExtension(file_names[0]) + "_asp.pas", null);
                        VisualPascalABC.VisualPABCSingleton.MainForm.OpenTabWithText(Path.GetFileNameWithoutExtension(file_names[0]) + "_asp", "");
                        TabWithAspects = VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument;
               
                        VisualPascalABC.VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.ExecuteAction(VisualPascalABCPlugins.VisualEnvironmentCompilerAction.SetCurrentSourceFileText, AspectPreprocessor.GetAspects(AspectPreprocessor.CompilerDirectives, AspectScm));
                        AspectTxtName = "&all";
                    }
                }
                /*
                for (int i = 0; i < AspectPreprocessor.CompilerDirectives.Count; i++)
                {
                    //aspdata
                    string s = AspectPreprocessor.GetAspects(AspectPreprocessor.CompilerDirectives, AspectScm);
                    if (AspectPreprocessor.CompilerDirectives[i].Name.text == "aspdata")
                    {
                        string t = AspectPreprocessor.CompilerDirectives[i].Directive.text;
                        string n = t.Substring(0, t.IndexOf(' ')).Trim();
                        string param = t.Substring(t.IndexOf(' ')).Trim();
                        if (AspectPreprocessor.AspectsMap.ContainsKey(n))
                        {
                            AspectPreprocessor.AspectsMap[n].Author = param.Substring(0, param.IndexOf(' ')).Trim();
                            AspectPreprocessor.AspectsMap[n].Author = param.Substring(param.IndexOf(' ')).Trim();
                            AspectPreprocessor.AspectsMap[n].IsSavedParams = true;
                        }
                    }
                    //end aspdata
                }
                */
            }
            catch (Exception e)
            {
                //VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.AddTextToCompilerMessages, "AspectTree Exception: " + e.ToString());
            }            
        }

        // Сбор аспектов без компиляции
        private void toolStripButton1_Click(object sender, EventArgs e)
        {        
            TemplateButtonEnabled = false;
            BuildButtonsEnabled = false;
            ShowTree();
            
            // Добавление параметров
            for (int i = 0; i < treeView.Nodes.Count; i++)
                if (treeView.Nodes[i].Text != "&all")
                    treeView.Nodes[i].Text = treeView.Nodes[i].Text + "  " + AspectPreprocessor.AspectsMap[treeView.Nodes[i].Text].Author + "  " + AspectPreprocessor.AspectsMap[treeView.Nodes[i].Text].Version;
            treeView.Refresh();
            treeView.Update();

            SaveButtonsEnabled = true;
            InsertButtonsEnabled = true;

            // Обработка ошибок
            bool finished = true;
            if (AspectErrors.Count > 0)
            {
                finished = false;
                VisualPascalABC.WorkbenchServiceFactory.Workbench.ErrorsListWindow.ClearErrorList();
                //AspectErrors.Clear();
                //AspectErrors.Add(new PascalABCCompiler.Preprocessor_2.Errors.AspBeginEndError(TabWithAspects.Name, null, str));

                PascalABCCompiler.CompilerType ct = VisualEnvironmentCompiler.DefaultCompilerType;
                VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Standart;
                VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.AddMessageToErrorListWindow, AspectErrors);
                VisualEnvironmentCompiler.DefaultCompilerType = ct;
            }
            // конец обработки
            if ((treeView.Nodes.Count <= 1) || (!finished))
            {
                BuildButtonsEnabled = true;
                TemplateButtonEnabled = true;
                SaveButtonsEnabled = false;
                SaveOneButtonEnabled = false;
                InsertButtonsEnabled = false;
                treeView.Nodes.Clear();
                treeView.Invalidate();
                treeView.Refresh();
                treeView.Update();
                AspectPreprocessorReset();
            }                            
        }

        // Сбор аспектов с последующей компиляцией
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            BuildButtonsEnabled = false;            

            PascalABCCompiler.CompilerType ct = VisualEnvironmentCompiler.DefaultCompilerType;
            VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Standart;
            VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.Build, null);
            VisualEnvironmentCompiler.DefaultCompilerType = ct;

            TemplateButtonEnabled = false;
            ShowTree();
            //Добавление параметров
            for (int i = 0; i < treeView.Nodes.Count; i++)
                if (treeView.Nodes[i].Text != "&all")
                    treeView.Nodes[i].Text = treeView.Nodes[i].Text + "  " + AspectPreprocessor.AspectsMap[treeView.Nodes[i].Text].Author + "  " + AspectPreprocessor.AspectsMap[treeView.Nodes[i].Text].Version;
            treeView.Refresh();
            treeView.Update();

            SaveButtonsEnabled = true;
            InsertButtonsEnabled = true;
            if (treeView.Nodes.Count <= 1)
            {
                BuildButtonsEnabled = true;
                TemplateButtonEnabled = true;
                SaveButtonsEnabled = false;
                SaveOneButtonEnabled = false;
                InsertButtonsEnabled = false;
                treeView.Nodes.Clear();
                treeView.Invalidate();
                treeView.Refresh();
                treeView.Update();
                AspectPreprocessorReset();
            }            
        }                      
        
        // Сохранение всех аспектов
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

            if (VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument == TabWithAspects)
            {

                VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument.SelectAll();
                string txt = VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument.SelectedText;

                string[] file_names = new string[1];
                //file_names[0] = "C:\\PABCWork.NET\\program8.pas";
                file_names[0] = RootFileName;

                // Проверка сохраняемых аспектов              
                string check;
                check = txt;
                List<PascalABCCompiler.SyntaxTree.SourceContext> beg;
                bool can_save = true;
                foreach (string str in AspectPreprocessor.AspectsMap.Keys)
                {
                    beg = AspectPreprocessor.AspectsMap[str].GetBeginList();
                    int asp_num = 0;
                    int asp_pos;
                    bool error_found = false;

                    for (int i = 0; i < beg.Count; i++)
                    {
                        error_found = false;
                        asp_pos = (check.IndexOf("{$asp " + str + "}", asp_num));
                        if ((asp_pos >= 0) && (check.IndexOf("{$endasp " + str + "}", asp_num) > (asp_pos + ("{$asp " + str + "}").Length)))
                        {
                            if ((check.IndexOf("{$asp ", asp_pos + ("{$asp " + str + "}").Length, check.IndexOf("{$endasp " + str + "}", asp_num) - (asp_pos + ("{$asp " + str + "}").Length)) < 0) && (check.IndexOf("{$endasp ", asp_pos + ("{$asp " + str + "}").Length, check.IndexOf("{$endasp " + str + "}", asp_num) - (asp_pos + ("{$asp " + str + "}").Length)) < 0))
                                asp_num = check.IndexOf("{$endasp " + str + "}", asp_num) + ("{$endasp " + str + "}").Length;
                            else
                            {
                                error_found = true;
                                break;
                            }
                        }
                        else
                        {
                            error_found = true;
                            break;
                        }
                    }
                    if (error_found)
                    {
                        can_save = false;
                        VisualPascalABC.WorkbenchServiceFactory.Workbench.ErrorsListWindow.ClearErrorList();
                        AspectErrors.Clear();
                        AspectErrors.Add(new PascalABCCompiler.Preprocessor_2.Errors.AspBeginEndError(TabWithAspects.Name, null, str));

                        PascalABCCompiler.CompilerType ct = VisualEnvironmentCompiler.DefaultCompilerType;
                        VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Standart;
                        VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.AddMessageToErrorListWindow, AspectErrors);
                        VisualEnvironmentCompiler.DefaultCompilerType = ct;
                        //exit saving
                        break;
                    }
                }
                // Конец проверки

                if (can_save)
                {
                    StreamWriter sw = File.CreateText(Path.GetDirectoryName(file_names[0]) + "\\" + Path.GetFileNameWithoutExtension(file_names[0]) + "_asp.pas");
                    sw.Write(txt);
                    sw.Close();

                    if (VisualPascalABC.ProjectFactory.Instance.ProjectLoaded)
                        AspectPreprocessor.WriteAspects(txt, Path.GetDirectoryName(RootProjectInfo.Path));
                    else
                        AspectPreprocessor.WriteAspects(txt, "");

                    // Обновление вкладок
                    StreamWriter sw1;
                    VisualPascalABC.CodeFileDocumentControl tp;
                    if (!VisualPascalABC.ProjectFactory.Instance.ProjectLoaded)
                    {
                        tp = VisualPascalABC.VisualPABCSingleton.MainForm.FindTab(file_names[0]);
                        if (tp != null)
                        {
                            VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument = tp;
                            VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument.LoadFromFile(file_names[0]);
                            VisualPascalABC.VisualPABCSingleton.MainForm.SaveAspPlugin(tp);
                        }
                    }
                    else
                    {
                        foreach (PascalABCCompiler.IFileInfo fi in RootProjectInfo.SourceFiles)
                        {
                            tp = VisualPascalABC.VisualPABCSingleton.MainForm.FindTab(Path.GetDirectoryName(RootProjectInfo.Path) + "\\" + fi.Name);
                            if (tp != null)
                            {
                                VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument = tp;
                                VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument.LoadFromFile(Path.GetDirectoryName(RootProjectInfo.Path) + "\\" + fi.Name);
                                VisualPascalABC.VisualPABCSingleton.MainForm.SaveAspPlugin(tp);
                            }
                        }
                        // Открываем главный файл после обновления
                        tp = VisualPascalABC.VisualPABCSingleton.MainForm.FindTab(Path.GetDirectoryName(RootProjectInfo.Path) + "\\" + file_names[0]);
                        if (tp != null)
                        {
                            VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument = tp;
                            //VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument.LoadFromFile(file_names[0]);
                        }
                    }

                    // Закрываем вкладку с аспектами
                    if (TabWithAspects != null)
                        VisualPascalABC.VisualPABCSingleton.MainForm.CloseAspPlugin(TabWithAspects);

                    // Деактивируем пункт сохранения
                    SaveButtonsEnabled = false;
                    SaveOneButtonEnabled = false;
                    TemplateButtonEnabled = true;
                    BuildButtonsEnabled = true;
                    InsertButtonsEnabled = false;

                    treeView.Nodes.Clear();
                    treeView.Refresh();
                }
            }            
            else
            {
                AspectPreprocessorReset();
                SaveButtonsEnabled = false;
                SaveOneButtonEnabled = false;
                TemplateButtonEnabled = true;
                BuildButtonsEnabled = true;
                InsertButtonsEnabled = false;
                treeView.Nodes.Clear();
                treeView.Refresh();             
            }            
        }

        // Commented
        private void treeView_Click(object sender, EventArgs e)
        {
            /*
            string[] file_names = new string[1];
            file_names[0] = "C:\\PABCWork.NET\\program8.pas";
            PascalABCCompiler.ParserTools.SourceContextMap scm = new PascalABCCompiler.ParserTools.SourceContextMap();
            PascalABCCompiler.SourceFilesProviderDelegate sourceFilesProvider = PascalABCCompiler.SourceFilesProviders.DefaultSourceFilesProvider;
            PascalABCCompiler.Preprocessor2.Preprocessor2 prepr = new PascalABCCompiler.Preprocessor2.Preprocessor2(sourceFilesProvider);
            string Text = prepr.Build(file_names, file_names, new List<Error>(), scm);

            //for (int i = 0; i < prepr.CompilerDirectives.Count; i++)
            //{
            //    if (prepr.CompilerDirectives[i].Name.text == "asp")
            //       treeView.Nodes.Add(prepr.CompilerDirectives[i].Directive.text);
            //}

            string aspects = prepr.GetAspects(prepr.CompilerDirectives, scm);
            string CurrentAspectText = aspects.Substring(aspects.IndexOf("{$asp " + treeView.SelectedNode.Text), aspects.IndexOf("{$endasp " + treeView.SelectedNode.Text) - aspects.IndexOf("{$asp " + treeView.SelectedNode.Text) + ("{$endasp " + treeView.SelectedNode.Text + "}").Length);
            VisualPascalABC.VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.ExecuteAction(VisualPascalABCPlugins.VisualEnvironmentCompilerAction.SetCurrentSourceFileText, CurrentAspectText);


            */
        }

        // Загрузка выбранного аспекта, либо всех аспектов
        private void treeView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (treeView.SelectedNode.Text == "&all")
                    AspectTxtName = treeView.SelectedNode.Text;
                else
                    AspectTxtName = treeView.SelectedNode.Text.Substring(0, treeView.SelectedNode.Text.IndexOf(' '));
                if (treeView.SelectedNode.Text == "&all")
                {
                    SaveButtonsEnabled = true;
                    SaveOneButtonEnabled = false;

                    string[] file_names = new string[1];
                    file_names[0] = RootFileName;

                    VisualPascalABC.VisualPABCSingleton.MainForm.OpenTabWithText(Path.GetFileNameWithoutExtension(file_names[0]) + "_asp", "");
                    VisualPascalABC.VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.ExecuteAction(VisualPascalABCPlugins.VisualEnvironmentCompilerAction.SetCurrentSourceFileText, AspectPreprocessor.GetAspects(AspectPreprocessor.CompilerDirectives, AspectScm));
                }
                else
                {
                    SaveButtonsEnabled = false;
                    SaveOneButtonEnabled = true;

                    string aspects = AspectPreprocessor.GetAspects(AspectPreprocessor.CompilerDirectives, AspectScm);
                    // Get current aspect
                    string temptext = treeView.SelectedNode.Text.Substring(0, treeView.SelectedNode.Text.IndexOf(' '));
                    List<PascalABCCompiler.SyntaxTree.SourceContext> beg = AspectPreprocessor.AspectsMap[temptext].GetBeginList();
                    string CurrentAspectText = "";
                    int asp_num = 0;
                    for (int i = 0; i < beg.Count; i++)
                    {
                        CurrentAspectText = CurrentAspectText + aspects.Substring(aspects.IndexOf("{$asp " + temptext + "}", asp_num), aspects.IndexOf("{$endasp " + temptext + "}", asp_num) - aspects.IndexOf("{$asp " + temptext + "}", asp_num) + ("{$endasp " + temptext + "}").Length) + "\n";
                        asp_num = aspects.IndexOf("{$endasp " + temptext + "}", asp_num) + ("{$endasp " + temptext + "}").Length;
                    }
                    VisualPascalABC.VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.ExecuteAction(VisualPascalABCPlugins.VisualEnvironmentCompilerAction.SetCurrentSourceFileText, CurrentAspectText);
                }
                treeView.Invalidate();
                treeView.Refresh();
                treeView.Update();
            }
            catch
            {
                //VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.AddTextToCompilerMessages, "AspectTree Exception: " + e.ToString());
            }
        }

        // Вызов конструктора шаблона
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            VisualPascalABC.VisualPABCSingleton.MainForm.MakeTemplate();         
        }

        // Сохранение текущего аспекта
        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            if (VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument == TabWithAspects)
            {

                VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument.SelectAll();
                string txt = VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument.SelectedText;

                string[] file_names = new string[1];
                //file_names[0] = "C:\\PABCWork.NET\\program8.pas";
                file_names[0] = RootFileName;

                string check;
                // Проверка сохраняемых аспектов              
                check = txt;
                List<PascalABCCompiler.SyntaxTree.SourceContext> beg;
                bool can_save = true;
                //lovim posledniy vibrannii aspect i dlia nego delaem vse eto - Asptxtname
                {
                    beg = AspectPreprocessor.AspectsMap[AspectTxtName].GetBeginList();
                    int asp_num = 0;
                    int asp_pos;
                    bool error_found = false;

                    for (int i = 0; i < beg.Count; i++)
                    {
                        error_found = false;
                        asp_pos = (check.IndexOf("{$asp " + AspectTxtName + "}", asp_num));
                        if ((asp_pos >= 0) && (check.IndexOf("{$endasp " + AspectTxtName + "}", asp_num) > (asp_pos + ("{$asp " + AspectTxtName + "}").Length)))
                        {
                            if ((check.IndexOf("{$asp ", asp_pos + ("{$asp " + AspectTxtName + "}").Length, check.IndexOf("{$endasp " + AspectTxtName + "}", asp_num) - (asp_pos + ("{$asp " + AspectTxtName + "}").Length)) < 0) && (check.IndexOf("{$endasp ", asp_pos + ("{$asp " + AspectTxtName + "}").Length, check.IndexOf("{$endasp " + AspectTxtName + "}", asp_num) - (asp_pos + ("{$asp " + AspectTxtName + "}").Length)) < 0))
                                asp_num = check.IndexOf("{$endasp " + AspectTxtName + "}", asp_num) + ("{$endasp " + AspectTxtName + "}").Length;
                            else
                            {
                                error_found = true;
                                break;
                            }
                        }
                        else
                        {
                            error_found = true;
                            break;
                        }
                    }
                    if (error_found)
                    {
                        can_save = false;
                        VisualPascalABC.WorkbenchServiceFactory.Workbench.ErrorsListWindow.ClearErrorList();
                        AspectErrors.Clear();
                        AspectErrors.Add(new PascalABCCompiler.Preprocessor_2.Errors.AspBeginEndError(TabWithAspects.Name, null, AspectTxtName));

                        PascalABCCompiler.CompilerType ct = VisualEnvironmentCompiler.DefaultCompilerType;
                        VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Standart;
                        VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.AddMessageToErrorListWindow, AspectErrors);
                        VisualEnvironmentCompiler.DefaultCompilerType = ct;
                        //exit saving                        
                    }
                }
                // Конец проверки
                if (can_save)
                {

                    StreamWriter sw = File.CreateText(Path.GetDirectoryName(file_names[0]) + "\\" + Path.GetFileNameWithoutExtension(file_names[0]) + "_asp.pas");
                    sw.Write(txt);
                    sw.Close();

                    if (VisualPascalABC.ProjectFactory.Instance.ProjectLoaded)
                        AspectPreprocessor.WriteAspect(txt, Path.GetDirectoryName(RootProjectInfo.Path), AspectTxtName);
                    else
                        AspectPreprocessor.WriteAspect(txt, "", AspectTxtName);

                    // Обновление вкладок

                    StreamWriter sw1;
                    VisualPascalABC.CodeFileDocumentControl tp;
                    if (!VisualPascalABC.ProjectFactory.Instance.ProjectLoaded)
                    {
                        tp = VisualPascalABC.VisualPABCSingleton.MainForm.FindTab(file_names[0]);
                        if (tp != null)
                        {
                            VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument = tp;
                            VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument.LoadFromFile(file_names[0]);
                            VisualPascalABC.VisualPABCSingleton.MainForm.SaveAspPlugin(tp);
                        }
                    }
                    else
                    {
                        foreach (PascalABCCompiler.IFileInfo fi in RootProjectInfo.SourceFiles)
                        {
                            tp = VisualPascalABC.VisualPABCSingleton.MainForm.FindTab(Path.GetDirectoryName(RootProjectInfo.Path) + "\\" + fi.Name);
                            if (tp != null)
                            {
                                VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument = tp;
                                VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument.LoadFromFile(Path.GetDirectoryName(RootProjectInfo.Path) + "\\" + fi.Name);
                                VisualPascalABC.VisualPABCSingleton.MainForm.SaveAspPlugin(tp);
                            }
                        }
                        // Открываем главный файл после обновления
                        tp = VisualPascalABC.VisualPABCSingleton.MainForm.FindTab(Path.GetDirectoryName(RootProjectInfo.Path) + "\\" + file_names[0]);
                        if (tp != null)
                        {
                            VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument = tp;
                            //VisualPascalABC.VisualPABCSingleton.MainForm.CurrentCodeFileDocument.LoadFromFile(file_names[0]);
                        }
                    }

                    // Закрываем вкладку с аспектами
                    if (TabWithAspects != null)
                        VisualPascalABC.VisualPABCSingleton.MainForm.CloseAspPlugin(TabWithAspects);

                    // Деактивируем пункт сохранения
                    SaveButtonsEnabled = false;
                    SaveOneButtonEnabled = false;
                    TemplateButtonEnabled = true;
                    BuildButtonsEnabled = true;

                    treeView.Nodes.Clear();
                    treeView.Refresh();
                }
            }
            else
            {
                AspectPreprocessorReset();
                SaveButtonsEnabled = false;
                SaveOneButtonEnabled = false;
                TemplateButtonEnabled = true;
                BuildButtonsEnabled = true;
                treeView.Nodes.Clear();
                treeView.Refresh();             
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (AspectTxtName == "&all")
                {
                    SaveButtonsEnabled = true;
                    SaveOneButtonEnabled = false;

                    string[] file_names = new string[1];
                    file_names[0] = RootFileName;

                    VisualPascalABC.VisualPABCSingleton.MainForm.OpenTabWithText(Path.GetFileNameWithoutExtension(file_names[0]) + "_asp", "");
                    VisualPascalABC.VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.ExecuteAction(VisualPascalABCPlugins.VisualEnvironmentCompilerAction.SetCurrentSourceFileText, AspectPreprocessor.GetAspects(AspectPreprocessor.CompilerDirectives, AspectScm));
                }
                else
                {
                    SaveButtonsEnabled = false;
                    SaveOneButtonEnabled = false;

                    string aspects = AspectPreprocessor.GetAspects(AspectPreprocessor.CompilerDirectives, AspectScm);
                    string authors = "";
                    string a = AspectPreprocessor.AspectsMap[AspectTxtName].Author;

                    foreach (string str in AspectPreprocessor.AspectsMap.Keys)
                    {
                        if (AspectPreprocessor.AspectsMap[str].Author == a)
                        {
                            string temptext = str;
                            List<PascalABCCompiler.SyntaxTree.SourceContext> beg = AspectPreprocessor.AspectsMap[temptext].GetBeginList();
                            string CurrentAspectText = "";
                            int asp_num = 0;
                            for (int i = 0; i < beg.Count; i++)
                            {
                                CurrentAspectText = CurrentAspectText + aspects.Substring(aspects.IndexOf("{$asp " + temptext + "}", asp_num), aspects.IndexOf("{$endasp " + temptext + "}", asp_num) - aspects.IndexOf("{$asp " + temptext + "}", asp_num) + ("{$endasp " + temptext + "}").Length) + "\n";
                                asp_num = aspects.IndexOf("{$endasp " + temptext + "}", asp_num) + ("{$endasp " + temptext + "}").Length;
                            }
                            authors = authors + CurrentAspectText;
                        }
                    }
                    
                    VisualPascalABC.VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.ExecuteAction(VisualPascalABCPlugins.VisualEnvironmentCompilerAction.SetCurrentSourceFileText, authors);
                }
                treeView.Invalidate();
                treeView.Refresh();
                treeView.Update();
            }
            catch
            {
                //VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.AddTextToCompilerMessages, "AspectTree Exception: " + e.ToString());
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {
                if (AspectTxtName == "&all")
                {
                    SaveButtonsEnabled = true;
                    SaveOneButtonEnabled = false;

                    string[] file_names = new string[1];
                    file_names[0] = RootFileName;

                    VisualPascalABC.VisualPABCSingleton.MainForm.OpenTabWithText(Path.GetFileNameWithoutExtension(file_names[0]) + "_asp", "");
                    VisualPascalABC.VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.ExecuteAction(VisualPascalABCPlugins.VisualEnvironmentCompilerAction.SetCurrentSourceFileText, AspectPreprocessor.GetAspects(AspectPreprocessor.CompilerDirectives, AspectScm));
                }
                else
                {
                    SaveButtonsEnabled = false;
                    SaveOneButtonEnabled = false;

                    string aspects = AspectPreprocessor.GetAspects(AspectPreprocessor.CompilerDirectives, AspectScm);
                    string versions = "";
                    string a = AspectPreprocessor.AspectsMap[AspectTxtName].Version;

                    foreach (string str in AspectPreprocessor.AspectsMap.Keys)
                    {
                        if (AspectPreprocessor.AspectsMap[str].Version == a)
                        {
                            string temptext = str;
                            List<PascalABCCompiler.SyntaxTree.SourceContext> beg = AspectPreprocessor.AspectsMap[temptext].GetBeginList();
                            string CurrentAspectText = "";
                            int asp_num = 0;
                            for (int i = 0; i < beg.Count; i++)
                            {
                                CurrentAspectText = CurrentAspectText + aspects.Substring(aspects.IndexOf("{$asp " + temptext + "}", asp_num), aspects.IndexOf("{$endasp " + temptext + "}", asp_num) - aspects.IndexOf("{$asp " + temptext + "}", asp_num) + ("{$endasp " + temptext + "}").Length) + "\n";
                                asp_num = aspects.IndexOf("{$endasp " + temptext + "}", asp_num) + ("{$endasp " + temptext + "}").Length;
                            }
                            versions = versions + CurrentAspectText;
                        }
                    }

                    VisualPascalABC.VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.ExecuteAction(VisualPascalABCPlugins.VisualEnvironmentCompilerAction.SetCurrentSourceFileText, versions);
                }
                treeView.Invalidate();
                treeView.Refresh();
                treeView.Update();
            }
            catch
            {
                //VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.AddTextToCompilerMessages, "AspectTree Exception: " + e.ToString());
            }
        }      
   }
}