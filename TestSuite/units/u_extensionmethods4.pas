unit u_extensionmethods4;
function string.ToInt2: integer;
begin
  Result := integer.Parse(Self);
end;

function string.ToWords2(params delim: array of char): array of string;
begin
  Result := Self.Split(delim,System.StringSplitOptions.RemoveEmptyEntries);
end;

begin
  var s := '123';
  //assert(s.ToInt = 123);
  var i := s.ToInt2;
  assert(i=123);
  s := '222';
  i := s.ToInt2;
  assert(i=222);
  s := '111';
  i := s.ToInt2();
  assert(i=111);
  s := 'abc def';
  var arr := s.ToWords2;
  assert(arr[0]='abc');
  arr := 'ttt xxx'.ToWords2;
  assert(arr[1] = 'xxx');
  s := 'p p p';
  foreach c: string in s.ToWords2 do
    assert(c = 'p');
end.