type 
  TClass = auto class
    i: integer
  end;

function operator implicit(i: integer): set of integer; extensionmethod;
begin
  Result := [i];
end;

function operator implicit(i: integer): TClass; extensionmethod;
begin
  Result := new TClass(i);
end;

begin
  var s: set of integer := 2;
  assert(s = [2]);
  var o: TClass := 2;
  assert(o.i = 2);
end.