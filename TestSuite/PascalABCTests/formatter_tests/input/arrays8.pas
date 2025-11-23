type TArr = array[1..3] of integer;

type TRec = record
a : integer;
b : real;
c : TArr;
d : string;
e : string[4];
f : set of char;

end;

var rec1, rec2 : TRec;
    arr1 : TArr := (1,2,3);
    arr2 : TArr := (1,2,3);
    arr3 : TArr := (1,2,4);
    
begin
rec1.a := 2; rec1.b := 2.3; rec1.d := 'abc'; rec1.e := 'abc'; 
rec1.c := arr1;
rec1.f := ['e','d'];
rec2.a := 2; rec2.b := 2.3; rec2.d := 'abc'; rec2.e := 'abc'; 
rec2.c := arr2;
rec2.f := ['e','d'];
assert(rec1=rec2);
rec1.b := 6.3;
assert(rec1<>rec2);
rec1.c[2] := 8;
assert(rec1 <> rec2);
assert(arr1=arr2);
assert(arr3<>arr1);
end.
