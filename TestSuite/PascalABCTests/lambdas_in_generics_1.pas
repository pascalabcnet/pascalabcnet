function psd<T>(e: IEnumerable<T>): IEnumerable<T>; where T:constructor;
begin
   result := e.OrderBy(x -> x);
end;

begin
  var ttt := psd(Seq(1,2,4,2,3,5,6));
  assert(ttt.SequenceEqual(Seq(1,2,2,3,4,5,6)));
end.