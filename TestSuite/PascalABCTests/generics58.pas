type
  t0 = class end;
  t1 = class(List<t0>) end;
  t2 = class(t0);
begin
  var a := new t1;
  //Ошибка: Нельзя преобразовать тип t0 к T
  a.Add(new t0);
  a.Add(new t2);
  assert(a[0] <> nil);
  assert(a[1] is t2);
  assert(a.Count = 2);
end.