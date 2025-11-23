{function Reverse<T>(Self: array of T): array of T; extensionmethod;
begin
  System.Array.Reverse(Self);
  Result := Self;
end;

function BinarySearch<T>(self: array of T; item: T): integer; extensionmethod;
begin
  Result := System.Array.BinarySearch(self,item);
end;}


begin
  var a := Arr(1,2,3);
  var seq := System.Linq.Enumerable.Select(a,x->x*2);
  //a.Reverse.Print;
end.