type
  t0=record end;
  
  t1<T1>=class
  where T1: record;
  a: T1;
  end;
  
  t2<T>=class(Dictionary<t1<t0>,T>) where T: record; end;

begin
  var a := new t2<integer>;
  var o := new t1<t0>;
  a.Add(o, 2);
  assert(a[o] = 2);
end.