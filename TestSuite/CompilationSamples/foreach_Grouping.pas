type 
  ������ = record
    ���: string;
    �����: integer;
  end;

begin
  var p: ������;
  p.��� := 'dfh';
  var pupils: array of ������ := Arr(p,p,p);
  var question := pupils.GroupBy(p->p.�����);
  foreach var group in question do
  begin
    foreach var pp: ������ in group do
      Println(pp);
  end;
end.