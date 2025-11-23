type TClass = class
  a: integer;
end;

begin
  var obj := new TClass;
  with obj do
  begin
    a := 2;
    assert(a = 2);
  end;
  var lst := Lst(1,2,3);
  with lst do
  begin
    assert(Count = 3);
  end;
end.