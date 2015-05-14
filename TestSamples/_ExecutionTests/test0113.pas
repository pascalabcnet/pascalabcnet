type TDynArray = array of integer;
     PDynArray = ^TDynArray;

var parr : PDynArray;
    arr : TDynArray:=(1,2,3,4);
    
begin
parr := @arr;
assert(parr^[2]=3);
end.