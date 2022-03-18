var
  a: 1..4;

begin
  a := 2;
  a += 1;
  assert(a = 3);
  a -= 1;
  assert(a = 2);
  a *= 2;
  assert(a = 4);
end.