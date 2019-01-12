type
  t1=class
    
    static a: array of integer := (0,1);
    static a2: array[1..2] of integer := (0,1);
    b: array of integer := (0,1);
  end;

begin
  assert(t1.a[1] = 1);
  assert(t1.a2[2] = 1);
  var o := new t1;
  assert(o.b[1] = 1);
end.