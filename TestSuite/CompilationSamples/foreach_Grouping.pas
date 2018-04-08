type 
  Ученик = record
    Имя: string;
    Класс: integer;
  end;

begin
  var p: Ученик;
  p.Имя := 'dfh';
  var pupils: array of Ученик := Arr(p,p,p);
  var question := pupils.GroupBy(p->p.Класс);
  foreach var group in question do
  begin
    foreach var pp: Ученик in group do
      Println(pp);
  end;
end.