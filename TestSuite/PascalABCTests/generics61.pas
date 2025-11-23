procedure p1<T>(o: T); where T: Exception;
begin
  assert(o.ToString = Exception.Create.ToString);
end;

procedure p2<T>(a: T); where T: System.Array;
begin
  assert(a.Length = 2);
  assert((a as System.Array).Length = 2);
end;

begin
  p1(new Exception);
  p2(new integer[2]);
end.