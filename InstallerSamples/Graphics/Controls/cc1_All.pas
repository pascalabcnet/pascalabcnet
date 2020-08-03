// Модуль Controls - все элементы управления
uses GraphWPF,Controls;

begin
  Window.Title := 'Модуль Controls - все элементы управления';
  // Обычно элементы управления размещаются на панели - левой или правой
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