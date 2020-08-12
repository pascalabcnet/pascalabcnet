// Модуль Controls - элементы управления на основной панели 
uses GraphWPF,Controls;

begin
  Window.Title := 'Модуль Controls - элементы управления на основной панели';

  var x := 10;
  var w := 140;
  Button(x,10,'Кнопка',w);
  CheckBox(x,43,'Флажок');
  RadioButton(x,70,'Переключатель 1');
  RadioButton(x,97,'Переключатель 2');
  TextBox(x,120,'Поле ввода текста',w);
  var ib0 := IntegerBox(x,172,'Поле ввода целого',0,10,w);
  ib0.Tooltip := 'Покрутите колёсико мыши для изменения значения';
  var l0 := ListBox(x,221,'Список стран',w);
  l0.Height := 110;
  l0.Add('Россия');
  l0.Add('США');
  l0.Add('Китай');
  l0.Add('Германия');
  l0.Add('Франция');
  var cb0 := ComboBox(x,362,'Выпадающий список',w);
  cb0.AddRange('Россия','США','Китай','Германия','Франция');
  
  Slider(x,414,'Слайдер:',0,10,w);
  TextBlock(x,469,'Блок текста',w);
  IntegerBlock(x,497,'Блок целого:',w,64);
  RealBlock(x,526,'Блок вещественного:',w,3.5);

  LeftPanel(160,Colors.Orange);
 
  Button('Кнопка');
  CheckBox('Флажок');
  RadioButton('Переключатель 1');
  RadioButton('Переключатель 2');
  TextBox('Поле ввода текста');
  var ib := IntegerBox('Поле ввода целого',0,10);
  ib.Tooltip := 'Покрутите колёсико мыши для изменения значения';
  var l := ListBox('Список стран');
  l.Height := 110;
  l.Add('Россия');
  l.Add('США');
  l.Add('Китай');
  l.Add('Германия');
  l.Add('Франция');
  var cb := ComboBox('Выпадающий список');
  cb.AddRange('Россия','США','Китай','Германия','Франция');
  
  Slider('Слайдер:',0,10);
  TextBlock('Блок текста');
  IntegerBlock('Блок целого:',64);
  RealBlock('Блок вещественного:',3.5);

  var s := StatusBar;
  s.Text := 'Строка статуса';
end.