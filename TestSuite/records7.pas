type
  r1=record
    
    class Empty := default(r1);
    
  end;
  r2=record
    
    a,b:byte;
    class Empty := default(r2);
    
  end;

begin
  var a := new r2;
  var b := new r2;
  assert(a = b);
  assert(not (a <> b));
  b.a := 2;
  assert(a <> b);
end.