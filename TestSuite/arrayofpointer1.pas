//winonly
var s : string[10];
    arr : array of pinteger;
    i : integer;
    
begin
 arr := new pinteger[5];
 i := 4;
 arr[1] := @i;
 assert(arr[1]^ = 4);
 SetLength(arr,6);
 assert(arr[1]^ = 4);
end.