// Модуль Controls - элементы управления с заголовками
uses Controls, GraphWPF;

begin
  Window.Title := 'Модуль Controls - все элементы управления с заголовками';
  LeftPanel(170, Colors.Orange);
  
  var b := Button('Заголовки включены');
  
  var tb := new TextBoxWPF('TextBox');
  tb.Height := 40;
  tb.Text := 'Несколько строк текста';
  tb.Wrapping := True;
  
  var ib := new IntegerBoxWPF('IntegerBox', 0, 10);
  var lb := new ListBoxWPF('ListBox',100);
  lb.AddRange('1 2 3 4 5'.ToWords);
  var cb := new ComboBoxWPF('ComboBox');
  cb.AddRange('1 2 3 4 5'.ToWords);
  
  b.Click := procedure -> begin
    if b.Text = 'Заголовки включены' then
      b.Text := 'Заголовки выключены'
    else b.Text := 'Заголовки включены';
    if tb.Title = '' then
      tb.Title := 'Заголовок'
    else tb.Title := '';
    if ib.Title = '' then
      ib.Title := 'Заголовок'
    else ib.Title := '';
    if lb.Title = '' then
      lb.Title := 'Заголовок'
    else lb.Title := '';
    if cb.Title = '' then
      cb.Title := 'Заголовок'
    else cb.Title := '';
  end;
end.