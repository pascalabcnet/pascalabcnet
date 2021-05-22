procedure Test;
const
  x1 = default(integer)+1;
  x2: integer = default(integer)+1;
  x3 = default(integer);
  x4 = default(string);
  x5 = default(object);
  x6 = default(integer) + default(real);
begin
  assert(x1 = 1);
  assert(x2 = 1);
  assert(x3 = 0);
  assert(x4 = nil);
  assert(x5 = nil);
  assert(abs(x6) < 0.000001); 
end;

const
  x1 = default(integer)+1;
  x2: integer = default(integer)+1;
  x3 = default(integer);
  x4 = default(string);
  x5 = default(object);
  x6 = default(integer) + default(real);
  
begin
  assert(x1 = 1);
  assert(x2 = 1);
  assert(x3 = 0);
  assert(x4 = nil);
  assert(x5 = nil);
  assert(abs(x6) < 0.000001);
  Test;
end.