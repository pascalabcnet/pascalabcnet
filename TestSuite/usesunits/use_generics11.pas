uses u_generics11; 

var a: t1<integer>;

begin
  a := new t1<integer>;
  assert(not (a = a));
  assert(a <> a);
end.