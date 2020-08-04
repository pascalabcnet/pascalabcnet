// Модуль Controls - MessageBox
uses GraphWPF,Controls;

begin
  Window.Title := 'Модуль Controls - MessageBox';
  LeftPanel(150, Colors.Orange);
  Button('MsgBoxOK').Click := () -> MessageBox.Show('Сообщение','Заголовок');
  Button('MsgBoxYesNo').Click := () -> begin
    if MessageBox.Show('Сообщение','Заголовок',MessageBoxButton.YesNo) = MessageBoxResult.Yes then
      Print('Yes')
    else Print('No');
  end;  
  Button('MsgBoxWithQuestion').Click := () -> begin
    if MessageBox.Show('Вы уверены?','Заголовок',MessageBoxButton.YesNo,MessageBoxImage.Question) 
      = MessageBoxResult.Yes then
        Print('Уверен')
    else Print('Нет');
  end;  
end.