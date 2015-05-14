type TDiap = 1..3;

procedure Test(a : set of TDiap);
begin
 assert(a = [1..3]);
end;

procedure Test2(var a : set of TDiap);
begin
 assert(a = [1..3]);
end;

var a : set of TDiap;
    b : set of byte;
    s1 : set of char;
    s2 : set of boolean;
    c : 'a'..'d';
    d : 2..3;
    
begin
 a := [1..5];
 assert(a=[1..3]);
 b := a;
 assert(b=[1..3]);
 Test([1..6]);
 Test2(a);
 b := [5];
 foreach i : integer in b do
 assert(i=5);
 foreach sh:shortint in b do
 assert(sh=5);
 foreach sm:smallint in b do
 assert(sm=5);
 foreach lw:longword in b do
 assert(lw=5);
 foreach w:word in b do
 assert(w=5);
 foreach li:int64 in b do
 assert(li=5);
 foreach ui:uint64 in b do
 assert(ui=5);
 s1 := ['b'];
 foreach c2:char in s1 do
 assert(c2='b');
 s2 := [true];
 foreach f:boolean in s2 do
 assert(f=true);
end.