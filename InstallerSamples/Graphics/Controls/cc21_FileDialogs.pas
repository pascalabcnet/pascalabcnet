// Модуль Controls - OpenFileDialog и SaveFileDialog
uses Controls,GraphWPF;

begin
  Window.Title := 'Модуль Controls - OpenFileDialog и SaveFileDialog';
  LeftPanel(150, Colors.Orange);
  var tb := SetMainControl.AsTextBox;
  tb.FontSize := 16;
  tb.ReadOnly := False;
  tb.FontName := 'Courier new Cyr';
  
  var dopen := new OpenFileDialogWPF('c:\PABCWork.NET','Программы|*.pas');
  var dSave := new SaveFileDialogWPF('c:\PABCWork.NET','Программы|*.pas');
  
  var OpenHandler: procedure := procedure -> begin
    var res := dopen.ShowDialog;
    if res then
      tb.Text := ReadAllText(dopen.FileName);
  end;
  var SaveHandler: procedure := procedure -> begin
    var res := dSave.ShowDialog;
    if res then
      WriteAllText(dSave.FileName,tb.Text);
  end;
  
  var bOpen := Button('Open');
  bOpen.Click := OpenHandler;
  
  var bSave := Button('Save');
  bSave.Click := SaveHandler;
  
  var m := Menu;
  var m1 := m.Add('File');
  var mm1 := m1.Add('Open');
  mm1.Click := OpenHandler;
  m1.Add('Save',SaveHandler);
  m1.AddSeparator;
  m1.Add('Exit',procedure->Window.Close);
end.