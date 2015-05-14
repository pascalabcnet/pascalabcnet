var arr : array[1..5] of set of char;
    arr2 : array of set of char;
    arr3 : array of integer;
    
begin
arr3 := new integer[4];
arr3[3] := 34;
SetLength(arr2,4);
Include(arr2[1],'a');
SetLength(arr2,8);
//Include(arr2[1],'b');
end.