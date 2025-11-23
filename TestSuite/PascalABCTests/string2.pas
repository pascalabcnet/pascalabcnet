var arr : array[1..4] of string;
    arr2 : array of string;
    ptr : ^shortstring;
    
begin
for var i := 1 to 4 do
assert(arr[i] = '');
SetLength(arr2,4);
for var i := 0 to 3 do
assert(arr2[i] = '');
arr2 := new string[3];
for var i := 0 to 2 do
assert(arr2[i] = '');
New(ptr);
assert(ptr^='');
end.