type
  r1=record
    x,y:integer;
    class function operator implicit(a:r1):string := a.x.ToString + a.y.ToString;
  end;

procedure test(o: object);
begin
  assert(string(o) = '57');
end;

procedure test2(o: object);
begin
  assert(string(r1(o)) = '57');
end;

begin
  var a:r1;
  (a.x,a.y) := (5,7);
  test(string(a));
  test2(a);
  test2(object(a));
end.