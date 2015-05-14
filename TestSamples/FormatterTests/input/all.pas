unit All;

type 
TRec1 = record
 a : integer;
 c : char;
end;

TRec2 = record
 a : array[1..3] of integer;
 b : array[1..5] of TRec1;
 c : TRec1;
end;

TDiap = 1..6;

TSet1 = set of byte;
TSet2 = set of TDiap;

TClass = class
a : TRec1;
b : TRec2;
c : TDiap;
d : TSet1;
e : TSet2;

constructor;
begin
end;

procedure Test1(b : real);
begin
assert(b=3.14);
end;
end; 

procedure Test3;
begin
 
end;

procedure Test2(i : integer);
var t : TClass;
begin
t := new TClass;
t.a.c := 'k'; assert(t.a.c='k');
t.b.b[2].a := 12; assert(t.b.b[2].a = 12); 
t.Test1(3.14);
Test3;
end;

procedure Test;
var i : integer;
    arr : array of char;
    set1 : set of char;
begin
i := 10;
case i of
 1..3,5,6..8 : i := 10;
 0,4,9..11 : i := 11;
end;
assert(i=11);
SetLength(arr,1);
arr[0] := 't';
foreach c : char in arr do
assert(c='t');
arr := new char[1];
arr[0] := 't';
foreach c : char in arr do
assert(c='t');
Include(set1,'t');
foreach c : char in set1 do
assert(c='t');
Test2(5);
end;

procedure Start;
begin
Test;
end;
end.