// Модуль Controls - визуализация запросов к базе данных 
uses GraphWPF,Controls,ABCDatabases;

begin
  Window.Title := 'Модуль Controls - визуализация запросов к базе данных';
  LeftPanel(150,Colors.Orange);
  var l := SetMainControl.AsListView;

  var страны := ЗаполнитьМассивСтран;
  l.Fill(страны);

  Button('Все').Click := procedure -> l.Fill(страны);
  
  Button('Азия').Click := procedure -> 
    l.Fill(страны.Where(страна -> страна.Континент='Азия'));
    
  Button('Сорт по населению').Click := procedure -> 
    l.Fill(страны.OrderByDescending(s -> s.Население));
end.