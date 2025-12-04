type
  r1=record
    a: integer;
    class Empty:r1;
  end;

begin 
  var r: r1;
  r.a := 2;
  assert(r1.Empty.a = 0);
end.