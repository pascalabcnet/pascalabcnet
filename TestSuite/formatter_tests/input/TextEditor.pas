{$apptype windows}
{$reference 'System.Windows.Forms.dll'}
{$reference 'System.Drawing.dll'}
{$resource 'Open.png'}
{$resource 'Save.png'}

uses System.Windows.Forms;

const 
  TextFileExt = 'txt';
  TextFileFilter = 'Текстовые файлы (*.'+TextFileExt+')|*.'+TextFileExt;

var
  myForm: Form;
  TextBox1: TextBox;

procedure SaveFile(FileName: string);
begin
  //Создаем файловый поток с кодировкой Windows 1251, необходимо для корректного сохранения русских букв
  var f := new System.IO.StreamWriter(FileName, false, System.Text.Encoding.Default);
  f.Write(TextBox1.Text);
  f.Close;
end;

procedure OpenFile(FileName: string);
begin
  //Создаем файловый поток с кодировкой Windows 1251, необходимо для корректного чтения русских букв
  var f := new System.IO.StreamReader(FileName, System.Text.Encoding.Default);
  TextBox1.Text := f.ReadToEnd;
  f.Close;
end;

procedure FormClose(sender: object; args: System.EventArgs);
begin
  myForm.Close;  
end;

procedure MenuSaveClick(sender:object; args:System.EventArgs);
begin
  //Диалог для выбора файла
  var sd := new SaveFileDialog;
  //Расширение поумолчанию
  sd.DefaultExt := TextFileExt;
  //Фильтр для диалга
  sd.Filter := TextFileFilter;
  if sd.ShowDialog=DialogResult.OK then 
    //если результат выполнения sd.ShowDialog это нажатие кнопки подтверждения то
    SaveFile(sd.FileName);
end;

procedure MenuOpenClick(sender:object; args:System.EventArgs);
begin
  var sd := new OpenFileDialog;
  sd.DefaultExt := TextFileExt;
  sd.Filter := TextFileFilter;
  if sd.ShowDialog = DialogResult.OK then 
    OpenFile(sd.FileName);
end;

begin
  myForm := new Form;
  myForm.Text := 'Простой текстовый редактор';

  TextBox1 := new TextBox;
  TextBox1.Multiline := True;
  TextBox1.Height := 100;
  TextBox1.Dock := DockStyle.Fill;
  //Полосы прокрутки
  TextBox1.ScrollBars := ScrollBars.Both;
  //Устанавливаем шрифт
  TextBox1.Font := new System.Drawing.Font('Courier New',10);

  myForm.Controls.Add(TextBox1);
  
  //Создаем меню
  var toolStrip1 := new ToolStrip;
  toolStrip1.GripStyle := System.Windows.Forms.ToolStripGripStyle.Hidden;
  var miFile := new ToolStripMenuItem('Файл');  
  miFile.DropDownItems.Add(new ToolStripMenuItem('Открыть',         new System.Drawing.Bitmap(GetResourceStream('Open.png')),MenuOpenClick));
  miFile.DropDownItems.Add(new ToolStripMenuItem('Сохранить как...',new System.Drawing.Bitmap(GetResourceStream('Save.png')),MenuSaveClick));
  miFile.DropDownItems.Add(new ToolStripMenuItem('Выход',nil,FormClose));
  toolStrip1.Items.Add(miFile);
  myForm.Controls.Add(toolStrip1);
  
  //Посмотрим в аргументы командной строки
  //Если их количество = 1, то открываем 
  if CommandLineArgs.Length = 1 then 
    OpenFile(CommandLineArgs[0]);
      
  Application.Run(myForm);
end.