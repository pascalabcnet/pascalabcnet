type TArr = array[1..3] of integer;
     TMatr = array[1..2,1..2] of integer;
     
TRec = record
arr : TArr;
end;

procedure Test;
var arr1 : TArr := (1,2,3);
    arr2 : TArr;

begin
arr2 := arr1;
assert(arr1=arr2);
arr2[1] := 3;
assert(arr1<>arr2);
end;

procedure Test2;
var arr1 : TArr := (1,2,3);
    arr2 : TArr;
    
procedure Nested;
begin
arr2 := arr1;
assert(arr1=arr2);
arr2[1] := 3;
assert(arr1<>arr2);
end;

begin
Nested;
end;

var arr1 : TArr := (1,2,3);
    arr2 : TArr;
    rec : TRec := (arr:(1,2,3));
    rec2 : TRec;
    
    matr1 : TMatr := ((1,2),(1,3));
    matr2 : TMatr;
    
begin
arr2 := arr1;
assert(arr1=arr2);
arr2[1] := 3;
assert(arr1<>arr2);
rec2 := rec;
assert(rec=rec2);
rec2.arr[1] := 2;
assert(rec<>rec2);
rec2.arr := rec.arr;
assert(rec=rec2);
assert(not(rec<>rec2));

Test;
Test2;
matr2 := matr1;
assert(matr2 = matr1);
assert(not(matr1<>matr2));
matr2[1,1] := 4;
assert(matr2<>matr1);
assert(not(matr1=matr2));
end.