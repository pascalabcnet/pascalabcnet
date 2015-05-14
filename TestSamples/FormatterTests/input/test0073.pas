uses test0073u;

begin    
  a := [1,2];
  a += [4];
  assert(a=[1,2,4]);
  a -= [2];
  assert(a=[1,4]);
  a *= [4];
  assert(a=[4]);
  a := [1..5];
  b := [2..4];
  a *= b;
  assert(a=[2..4]);
end.