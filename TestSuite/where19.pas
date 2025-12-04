type
  t1 = class
    f := true;
  end;
  
procedure p<T>(o: T); where T: t1;
begin
  var oo := o;
  assert(oo.f);
  oo.f := false; // Записывает неправильно
  assert(not oo.f);
  o.f := true;
  assert(oo.f);
  o.f := false;
  assert(not oo.f);
end;

begin
  var a := new t1;
  p(a);
end.