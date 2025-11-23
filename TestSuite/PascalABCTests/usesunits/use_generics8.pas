uses u_generics8; 

begin
  var a := new byte[5];
  p1(a);
  assert(i = 1);
  i := 0;
  p2(a);
  assert(i = 2);
end.