function SkipLast1<T>(self: sequence of T; count: integer := 1): sequence of T; extensionmethod;
begin
  Result := Self.Reverse.Skip(count).Reverse;
end;

begin
  var arr := Seq(1,2,3).SkipLast1(2).ToArray;
  assert(arr[0] = 1);
end.