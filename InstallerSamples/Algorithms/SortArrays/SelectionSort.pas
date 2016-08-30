// Сортировка выбором
procedure SelectionSort(a: array of real);
begin
  for var i:=0 to a.Length-2 do
  begin
    var min := a[i]; 
    var ind := i;
    for var j:=i+1 to a.Length-1 do
      if a[j]<min then
      begin
        min := a[j];
        ind := j;
      end;
    a[ind] := a[i];
    a[i] := min;
  end;
end;

begin
  var a := ArrRandomReal(20);
  writeln('Содержимое массива: ');
  a.Println;
  SelectionSort(a);
  writeln('После сортировки выбором: ');
  a.Println;
end.