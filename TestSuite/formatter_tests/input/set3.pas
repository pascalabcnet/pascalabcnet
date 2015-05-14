var arr : array[1..3] of integer;
    s : set of integer;
begin
 arr[1] := 1;
 arr[2] := 2;
 arr[3] := 3;
 foreach i : integer in arr do
 begin
 Include(s,arr[i]);
 end;
 assert(s=[1,2,3]);
end.