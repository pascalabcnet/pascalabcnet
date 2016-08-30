function QuickSort(a: sequence of integer): sequence of integer;
begin
  Result := a.Count = 0 ? a : 
    QuickSort(a.Skip(1).Where(x -> x <= a.First))
    + a.First + 
    QuickSort(a.Skip(1).Where(x -> x > a.First));
end;  

begin
  var a := ArrRandom(20);
  a.Println;
  QuickSort(a).Println;
end.