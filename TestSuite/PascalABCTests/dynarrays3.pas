
type TRec = record
arr : array[,] of real;
arr2 : array[1..3] of integer;
end;

TRec2 = record
arr : array[,] of integer;
end;

const arr : array[,] of integer = ((1,2,3),(2,3,4));
      arr3 : array[,] of integer = arr;
      rec : TRec = (arr:((1.2,2.5),(3.2,2.3));arr2:(2,4,6));
      rec2 : TRec2 = (arr:arr);
      
procedure Test;
const arr2 : array[,] of integer = ((1,2,3),(2,3,4));
begin
  assert(arr2[1,1]=3);
  assert(arr[1,1]=3);
end;

procedure Test2;
const arr2 : array[,] of integer = ((1,2,3),(2,3,4));
procedure Nested;
begin
  assert(arr2[1,1]=3);
  assert(arr[1,1]=3);
end;
begin
  assert(arr2[1,1]=3);
  assert(arr[1,1]=3);
  Nested;
end;

begin
assert(arr[1,1]=3); assert(arr[0,2]=3);
assert(arr3[1,1]=3); assert(arr3[0,2]=3);
assert(rec.arr[1,1] = 2.3);
assert(rec2.arr[1,1] = 3);
Test;
Test2;
end.