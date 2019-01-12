// Сортировка выбором
procedure SelectionSort(a: array of real);
begin
  for var i:=0 to a.Length-2 do
  begin
    var (min,ind) := (a[i],i); 
    for var j:=i+1 to a.Length-1 do
      if a[j]<min then
        (min,ind) := (a[j],j); 
    a[ind] := a[i];
    a[i] := min;
  end;
end;

begin
  var a := SeqRandomReal(20).Select(r->r.Round(2)).ToArray;
  Println('Содержимое массива: ');
  a.Println;
  SelectionSort(a);
  Println('После сортировки выбором: ');
  a.Println;
end.