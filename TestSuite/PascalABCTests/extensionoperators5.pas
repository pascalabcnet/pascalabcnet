function operator=<T>(x,y: Stack<T>): boolean; extensionmethod;
begin
  Result := True
end;

function operator<><T>(x,y: Stack<T>): boolean; extensionmethod;
begin
  Result := not(x = y);
end;

begin
  var a1 := new Stack<integer>;
  var a2 := new Stack<integer>;
  assert(a1 = a2);
  assert(not (a1 <> a2));
end.