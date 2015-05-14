const 
  a1 : integer = 3;
  a2 = a1;
  a3 : real = a1;
  a4 : byte = a2;
  a5 = 'j';
  a6 : string = a5;
  a7 : array of char = (a5,a5,a5);
  
begin
assert(a1 = 3);
assert(a2 = 3);
assert(a3=a1);
assert(a4 = 3);
assert(a5='j');
assert(a6='j');
assert(a7[0]='j');
end.