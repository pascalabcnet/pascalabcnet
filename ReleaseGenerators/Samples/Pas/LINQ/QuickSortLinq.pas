function QuickSort(a: sequence of integer): sequence of integer;
begin
  if a.Count = 0 then
    Result := a
  else 
  begin
    var head := a.First();
    var tail := a.Skip(1);
    Result := QuickSort(tail.Where(x->x<=head)) + 
              head + 
              QuickSort(tail.Where(x->x>head));
  end;
end; 

begin
  var a := ArrRandom(20);
  a.Println;
  QuickSort(a).Println;
end.