var i: integer;
type
  t0 = class
    // Обязательно virtual
    procedure p1; virtual;
    begin
      i := 1;
    end;
  end;
  
procedure p1<T>(o: T);
where T: t0, constructor;
begin
  o.p1;
  var p := procedure -> o := o;
end;

begin
  p1(new t0);
  assert(i = 1);
end.