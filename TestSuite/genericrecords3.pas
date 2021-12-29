type
  t1 = class end;
  
  t2<T> = record
    where T: t1;
    a: T;
  end;
  
begin 
  var r: t2<t1>;
  r.a := new t1;
  assert(r.a <> nil);
end.