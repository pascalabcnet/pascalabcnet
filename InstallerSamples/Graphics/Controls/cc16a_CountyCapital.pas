uses GraphWPF, Controls, ABCDatabases;

begin
  Window.Title := 'Столицы стран';
  LeftPanel(220, Colors.LightGoldenrodYellow);
  var страны := ЗаполнитьМассивСтран;

  var a := ListBox('Страны',550);
  a.AddRange(страны.ConvertAll(страна -> страна.Название));
  
  var d := DictStr;
  foreach var страна in страны do
    d[страна.Название] := страна.Столица;
   
  Font.Size := 70;
  a.SelectionChanged := procedure -> begin
    Window.Clear;
    DrawText(GraphWindow.ClientRect,d[a.SelectedText])
  end;
end.