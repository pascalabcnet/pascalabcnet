function QuickSort(a: sequence of integer): sequence of integer;
begin
  if a.Count = 0 then
    Result := a
  else
  begin
    var f := a.First();
    var sm := QuickSort(a.Skip(1).Where(x->x<=f));
    var la := QuickSort(a.Skip(1).Where(x->x>f));
    Result := sm + f + la
  end;
end;

begin
  var a := ArrRandom(1000);
  writeln(MillisecondsDelta);
  var b := QuickSort(a);
  //b.Println;
  writeln(MillisecondsDelta);
end.