uses u_generics4; 
function f1: c1<byte> := new c2<word, byte>;

begin
  var o := f1;
  assert(o <> nil);
end.