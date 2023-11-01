unit u_sizeof2;
type TRec = record
a: integer;
b: real;
c: char;
end;

type
  t1 = class
    o: t1;
  end;

const 
  isize = sizeof(integer);
  rsize = sizeof(real);
  csize = sizeof(char);
  psize = sizeof(pointer);
  recsize = sizeof(TRec);
  classsize = sizeof(t1);
begin
  assert(isize=4);
  assert(rsize=8);
  assert(csize=2);
  assert(psize=System.Runtime.InteropServices.Marshal.SizeOf(typeof(pointer)));
  assert(recsize=System.Runtime.InteropServices.Marshal.SizeOf(typeof(TRec)));
  assert(classsize > 0);
end.