const c : array of set of char = (['a','b'],[],['k','l']);
var a : array of set of char := (['a','b'],[],['k','l']);
    b : array of set of 1..4 := ([1,2,7],[2,3],[]);
    d : array[1..3] of set of 1..4;
    
begin
assert(b[0]=[1,2]);
var e : array of set of 1..4 := ([1,2,7],[],[4]);
assert(e[0]=[1,2]);
end.