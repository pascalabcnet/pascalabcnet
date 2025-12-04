unit u_while1;
begin
var i := 1;
var s := 0;
while i < 5 do
begin
  s := s + i;
  Inc(i);
end;
assert(s=10);
var flag := false;
while not flag do
begin
  i := 3;
  flag := true;
end;
assert(i=3);
var enum: (one, two, three) := one;
while enum < three do
begin
  Inc(enum);
end;
assert(enum = three);
i := 0;
var arr: array of integer;
SetLength(arr,5);
while (i<5) and (arr[i] = 0) do Inc(i);
assert(i=5);  
end.