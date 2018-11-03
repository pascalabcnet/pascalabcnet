function MyReadInteger := 1;

begin
  var s: Set of Integer;
  Include(s, MyReadInteger);
  Assert(s=[1]);
end.