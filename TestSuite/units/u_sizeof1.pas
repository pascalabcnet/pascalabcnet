unit u_sizeof1;
type TRec = record
a: integer;
b: real;
c: char;
end;

begin
  assert(sizeof(integer)=4);
  assert(sizeof(real)=8);
  assert(sizeof(char)=2);
  assert(sizeof(pointer)=System.Runtime.InteropServices.Marshal.SizeOf(typeof(pointer)));
  assert(sizeof(TRec)=System.Runtime.InteropServices.Marshal.SizeOf(typeof(TRec)));
end.