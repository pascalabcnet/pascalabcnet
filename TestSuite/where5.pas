type
  t0=record end;
  
  t1<T1, T2>=class
  where T1: record;
  end;
  
  t2<T>=class(t1<t0,T>) where T: record; end;

begin
  var a := new t2<integer>;
end.