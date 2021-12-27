procedure p2<TList>(l: TList); where TList: IList<integer>;
begin
  l.Item[1] := l.Item[0];
  l[1] := l[0];
  assert(l[1] = 1);
end;

begin
  p2(Lst(1,2,3));
end.