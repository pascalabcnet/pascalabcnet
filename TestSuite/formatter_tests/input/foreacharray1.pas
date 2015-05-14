procedure Test2;
var a : array of integer;
procedure Nested;
begin
SetLength(a,1);
a[0] := 3;
foreach b:byte in a do
assert(b=3);
foreach sh:shortint in a do
assert(sh=3);
foreach sm:smallint in a do
assert(sm=3);
foreach w:word in a do
assert(w=3);
foreach i:integer in a do
assert(i=3);
foreach lw:longword in a do
assert(lw=3);
foreach li:int64 in a do
assert(li=3);
foreach ui:uint64 in a do
assert(ui=3);
end;

begin
SetLength(a,1);
a[0] := 3;
foreach b:byte in a do
assert(b=3);
foreach sh:shortint in a do
assert(sh=3);
foreach sm:smallint in a do
assert(sm=3);
foreach w:word in a do
assert(w=3);
foreach i:integer in a do
assert(i=3);
foreach lw:longword in a do
assert(lw=3);
foreach li:int64 in a do
assert(li=3);
foreach ui:uint64 in a do
assert(ui=3);
Nested;
end;

procedure Test;
var a : array of integer;
begin
SetLength(a,1);
a[0] := 3;
foreach b:byte in a do
assert(b=3);
foreach sh:shortint in a do
assert(sh=3);
foreach sm:smallint in a do
assert(sm=3);
foreach w:word in a do
assert(w=3);
foreach i:integer in a do
assert(i=3);
foreach lw:longword in a do
assert(lw=3);
foreach li:int64 in a do
assert(li=3);
foreach ui:uint64 in a do
assert(ui=3);
end;

var a : array of integer;

begin
SetLength(a,1);
a[0] := 3;
foreach b:byte in a do
assert(b=3);
foreach sh:shortint in a do
assert(sh=3);
foreach sm:smallint in a do
assert(sm=3);
foreach w:word in a do
assert(w=3);
foreach i:integer in a do
assert(i=3);
foreach lw:longword in a do
assert(lw=3);
foreach li:int64 in a do
assert(li=3);
foreach ui:uint64 in a do
assert(ui=3);
Test;
Test2;
end.