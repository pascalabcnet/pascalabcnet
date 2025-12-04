type TRec = record
x : integer;
constructor(x : integer);
begin
self.x := x;
end;
end;

TRec2 = record
r : TRec;
end;

function GetRec : TRec2;
begin
Result.r.x := 100;
end;

var r : TRec;
    prec : ^TRec;
    arr : array[1..3] of TRec;
    arr2 : array[1..3] of ^TRec;
    r2 : TRec2;
    prec2 : ^TRec2;
    arr3 : array[1..3] of TRec2;
    i : integer;
    
begin

with r do
x := 5;
assert(r.x = 5);
prec := @r;
with prec^ do
x := 7;
assert(prec^.x = 7);
with arr[2] do
x := 8;
assert(arr[2].x = 8);
arr2[2] := @r;
with arr2[2]^ do
x := 6;
assert(arr2[2]^.x = 6);

with r2.r do
x := 10;
assert(r2.r.x = 10);
with arr3[2].r do
x := 12;
assert(arr3[2].r.x = 12);
with arr3[2] do
r.x := 13;
assert(arr3[2].r.x = 13);
i := 1;
with arr3[i+1].r do
x := 5;
assert(arr3[i+1].r.x = 5);

with GetRec do
assert(r.x=100);
r := new TRec(2);
with new TRec(2) do
begin
assert(x = 2);
assert(x=2);
end;

end.