type TArr = array[1..4] of integer;
     TArr2 = array[1..3] of string[4];
     
begin
assert(sizeof(TArr)=4*sizeof(integer));
assert(sizeof(TArr2)=15);
end.