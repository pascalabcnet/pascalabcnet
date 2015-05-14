function QuickSort(a: IEnumerable<integer>): IEnumerable<integer>;
begin
  if a.Count = 0 then
    Result := a
  else
  begin
    var f := a.First();
    Result := QuickSort(a.Skip(1).Where(x->x<=f)) + f + QuickSort(a.Skip(1).Where(x->x>f))
  end;
end;

begin
  var a := SeqRandom(20);
  a.Println;
  var b := QuickSort(a);
  b.Println;
end.