// Модуль Controls - создание элементов вызовом конструктора
uses GraphWPF,Controls;

begin
  Window.Title := 'Модуль Controls - создание элементов вызовом конструктора + правая панель';
  // Размещение элементов на правой ранели
  new RightPanelWPF(160,Colors.Orange);
 
  new ButtonWPF('Кнопка');
  new CheckBoxWPF('Флажок');
  new RadioButtonWPF('Переключатель 1');
  new RadioButtonWPF('Переключатель 2');
  new TextBoxWPF('Поле ввода текста');
  var ib := new IntegerBoxWPF('Поле ввода целого',0,10);
  ib.Tooltip := 'Покрутите колёсико мыши для изменения значения';
  var l := new ListBoxWPF('Список стран');
  l.Height := 110;
  l.Add('Россия');
  l.Add('США');
  l.Add('Китай');
  l.Add('Германия');
  l.Add('Франция');
  var cb := new ComboBoxWPF('Выпадающий список');
  cb.AddRange('Россия','США','Китай','Германия','Франция');
  
  new SliderWPF('Слайдер:',0,10);
  new TextBlockWPF('Блок текста');
  new IntegerBlockWPF('Блок целого:',64);
  new RealBlockWPF('Блок вещественного:',3.5);

  var s := new StatusBarWPF;
  s.Text := 'Строка статуса';
end.