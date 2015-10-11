procedure test(obj: object);
begin
  assert(abs(real(obj)-3.7)<0.0001);
end;

begin
  var a := Arr(2.5,3.7);
  assert(abs(a.Max-3.7)<0.0001);
  var r := a.Max;
  assert(abs(r-3.7)<0.0001);
  test(a.Max);
end.