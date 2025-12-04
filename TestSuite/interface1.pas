type IInt1 = interface
function Test: integer;
end;

TClass = class(IInt1)
public function Test: integer;
begin
  Result := 3;
end;

end;

var obj : IInt1;
begin
  obj := new TClass;
  assert(obj.Test=3);
end.