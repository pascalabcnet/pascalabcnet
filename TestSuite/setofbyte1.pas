procedure Test;
var s : set of byte := [1..4];
    s2 : set of byte := [0..5];
    
begin
assert(s=[1..4]);
assert(s2=[0..5]);
end;

var s : set of byte := [1..4];
    s2 : set of byte := [0..5];
    
begin
assert(s=[1..4]);
assert(s2=[0..5]);
Test;
end.