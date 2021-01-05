// Модуль Controls - замена графического окна элементом "ListView"
uses Controls,GraphWPF;

type My = class
public
  auto property Поле1: integer;
  auto property Поле2: integer;
  constructor (п1,п2: integer) := (Поле1,Поле2) := (п1,п2);
end;

begin
  Window.Title := 'Модуль Controls - замена графического окна элементом "ListView"';
  LeftPanel(150,Colors.Orange);
  
  var l := SetMainControl.AsListView;
  
  // Заполнение объектами класса, у которых есть публичные свойства
  l.Fill(Arr(new My(2,5),new My(4,6)));
  
  Button('Очистить список').Click := procedure -> l.Clear;
  Button('Заполнить данными').Click := procedure -> l.Fill(|('Иванов',20),('Петров',19)|);
  Button('Заполнить заголовки').Click := procedure -> l.SetHeaders('Фамилия','Возраст');
end.