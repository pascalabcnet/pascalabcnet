const 
  s : set of 1..3 = [1,2]+[3,4];
  s2 : set of char = ['a','b']*['b'];

var s3 : set of 1..3 := [1,2]+[3,4];

begin
assert(s=[1,2,3]);
assert(s2=['b']);
assert(s3=[1,2,3]);
end.