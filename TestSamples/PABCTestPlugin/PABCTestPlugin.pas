{$apptype dll}

//для доступа к IVisualPascalABCPlugin, IVisualEnvironmentCompiler
{$reference PluginsSupport.dll}
//для доступа к IVisualEnvironmentCompiler.Compiler
{$reference Compiler.dll}

{$reference System.Windows.Forms.dll}
{$reference System.Drawing.dll}

//Ресурс - иконка кнопки
{$resource PABCTestPlugin_newfile.png}

unit PABCTestPlugin;

uses System.Collections.Generic,
     System.Drawing, 
     VisualPascalABCPlugins;

const NL = #13#10;

type
  //Тестовый плагин - должен иметь имя *_VisualPascalABCPlugin и реализовывать 
  //интерфейс VisualPascalABCPlugins.IVisualPascalABCPlugin
  //Этот плагин добавляет конпку на панель и в меню, по нажатию на которую:
  // 1. создается новый pas файл
  // 2. он открывается в оболочке
  // 3. он компилируется и запускается
  PABCTestPlugin_VisualPascalABCPlugin = class(IVisualPascalABCPlugin)
  private
    //интерфейс на оболочку компилятора
    compiler: IVisualEnvironmentCompiler;
    
    //для получения нового имени файла
    function GetNewFileName():string;
    begin
      var fileTemplate := 'C:\PABCWork.NET\NewProgram{0}.pas';
      var pasFile := '';
      var i := 1;
      repeat
        pasFile := string.Format(fileTemplate, i);
        i += 1;
      until(not System.IO.File.Exists(pasFile));
      result := pasFile;
    end;
    //обработчик нажатия на кнопку
    procedure Click1;
    begin
      //создаем файл
      var fileName := GetNewFileName;
      System.IO.File.WriteAllText(fileName, string.Format('begin'+NL+'Write(''Im new program "{0}" :)'');'+NL+'end.', fileName));
      //Открываем его в оболочке
      compiler.ExecuteAction(VisualEnvironmentCompilerAction.OpenFile, fileName);
      //Запускаем компиляцию и выполнение
      compiler.ExecuteAction(VisualEnvironmentCompilerAction.Run, nil);
    end;
  
  public
    //обязателен конструктор с одним параметром типа IVisualEnvironmentCompiler
    constructor(compiler: IVisualEnvironmentCompiler);
    begin
      self.compiler := compiler;
    end;
    //Обязательно
    function get_Name:string;
    begin
      result := 'PABC Test Plugin';
    end;
    //Обязательно
    function get_Version:string;
    begin
      result := '1.0';
    end;
    //Обязательно    
    function get_Copyright:string;
    begin
      result := 'PascalABCNet Team';
    end;
    //Обязательно
    //Визуальная оболочка с помощью этой функции получает спиок кнопок на панели и кнопок в меню:
    //Плагин должен добавить в соответвующий список необходимые кнопки.
    procedure GetGUI(MenuItems:List<IPluginGUIItem>; ToolBarItems:List<IPluginGUIItem>);
    begin
      //Грузим изображение из ресурса
      var img:Image := Image.FromStream(GetResourceStream('PABCTestPlugin_newfile.png'));
      //Создаем кнопку
      var item1 := new PluginGUIItem('Новый файл', 'Создать новый файл', img, Color.Transparent, Click1);
      //Добавляем в меню
      MenuItems.Add(item1);
      //Добавляем на панель
      ToolBarItems.Add(item1);
    end;
    
  end;
  
end.
  