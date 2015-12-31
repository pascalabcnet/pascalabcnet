//winonly
var arr : array of integer := (1,2,3);
    arr2 : array of integer;
    arr3 : array[,] of integer := ((1,2),(3,4));
    arr4 : array[,] of integer;
    i, j : integer;
    arr5 : array of pinteger:=(@i,@j);
    arr6 : array of pinteger;
    
begin
arr2 := Copy(arr);
assert(arr2[0]=1);
assert(arr2[1]=2);
assert(arr2[2]=3);
arr2[0] := 4;
assert(arr[0]=1);
arr4 := Copy(arr3);
assert(arr4[1,0]=3);
arr4[1,0] := 7;
assert(arr3[1,0]=3);
arr6 := Copy(arr5);
assert(arr6[0]=@i);

end.