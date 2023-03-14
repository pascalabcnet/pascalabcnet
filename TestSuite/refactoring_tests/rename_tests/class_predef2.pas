type
  {@@}t1 = class;
  
  t2 = class
    b: {!}t1;
  end;
  
  {!}t1 = class
    a: integer;
  end;

begin 
  var o: t2;
  o.b.a := 2;
end.