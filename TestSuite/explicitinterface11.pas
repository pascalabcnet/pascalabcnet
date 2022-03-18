//winonly
type
  I1 = interface
    property X: integer read write;
  end;
  
  T1 = record(I1)
    x1: integer;
    x2: integer;
    property X: integer read x1 write x1;
    property I1.X: integer read x2 write x2;
  end;

begin
  var r: T1;
  r.X := 1;
  assert(r.X = 1);
  var r1: I1 := r;
  r1.X := 3;
  assert(r1.X = 3);
end.