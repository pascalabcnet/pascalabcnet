type
  Point = (integer, integer);
  
  T1 = class
  end;
  
  T2<T> = class
  end;
  
begin
  var a: Point := (1, 2);
  assert(a.Item1 = 1);
  assert(a.Item2 = 2);
  var b: (T1, T1) := (new T1(), new T1());
  assert(b.Item1 <> nil);
  var c: (T2<T1>, T2<T1>) := (new T2<T1>(), new T2<T1>());
  assert(c.Item1 <> nil);
  var d: (T2<integer>, T2<integer>) := (new T2<integer>(), new T2<integer>());
  assert(d.Item1 <> nil);
end.