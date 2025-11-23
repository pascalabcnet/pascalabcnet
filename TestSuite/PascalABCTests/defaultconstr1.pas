type A = class
i: integer;
procedure p;
  begin
    var w := new A;
    w.i := 2;
    assert(w.i = 2);
  end;
end;
begin
var obj := new A;
obj.p;
end.