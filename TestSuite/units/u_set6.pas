unit u_set6;
const 
  s : set of 1..3 = [1,2,3,4];
  s2 : set of char = {['a','b']*}['b'];

var s3 : set of 1..3 := [1,2]+[3,4];

begin
assert(s=[1,2,3,4]);
assert(s2=['b']);
assert(s3=[1,2,3,4]);
end.