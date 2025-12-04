// ѕеречислимый тип
uses System;

type Months = (January,February,March,April,May,June,July,August,September,October,November,December);

begin
  var t: &Type := typeof(Months);
  var names: array of string := Enum.GetNames(t);
  writeln('÷икл по именам перечислимого типа');
  foreach name: string in names do
    write(name,' ');
  writeln; writeln;
  
  var v: &Array := Enum.GetValues(t);
  var mm: array of Months := new Months[v.Length];
  writeln('÷икл по массиву всех значений перечислимого типа');
  for var i:=0 to v.Length-1 do
    mm[i] := Months(v.GetValue(i));
     
  for var i:=0 to mm.Length-1 do
    write(mm[i],' ');    
end.  