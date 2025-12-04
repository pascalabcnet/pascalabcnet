var arr : array[1..5] of integer;
    arr2 : array of real;
    arr3 : array['a'..'d'] of integer;
    arr4 : array[1..3,1..5] of integer;
begin
assert(Low(arr) = 1); assert(High(arr)=5);
SetLength(arr2,12); 
assert(Low(arr2)=0); 
assert(High(arr2)=11);
assert(char(Low(arr3))='a');
assert(char(High(arr3))='d'); 
end.