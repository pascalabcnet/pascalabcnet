type
  t0=record end;
  
  t1<T1>=class
  where T1: t0;
  a: T1;
  end;
  
  t2<T>=class(t1<T>) where T: record; end;

begin
  var a := new t2<t0>;
end.