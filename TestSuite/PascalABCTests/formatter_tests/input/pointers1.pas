procedure Test(p : ^integer);
begin
assert(p^=77);
end;

procedure Test2(var p : ^integer);
begin
assert(p^=77);
end;

procedure Test3(p : ^integer);
procedure Nested;
begin
 assert(p^=77);
end;

begin
assert(p^=77);
end;

procedure Test4(var p : ^integer);
procedure Nested;
begin
assert(p^=77);
end;
begin
assert(p^=77);
end;

procedure Test5(p : ^integer);
begin
 New(p);
 p^ := 45;
 assert(p^=45);
end;

procedure Test6(var p : ^integer);
begin
 New(p);
 p^ := 45;
 assert(p^=45);
end;

procedure Test7(p : ^integer);
procedure Nested;
begin
 New(p);
 p^ := 77;
 assert(p^=77);
end;
begin
 Nested;
 assert(p^=77);
end;

procedure Test8(var p : ^integer);
procedure Nested;
begin
 New(p);
 p^ := 55;
 assert(p^=55);
end;
begin
 Nested;
 assert(p^=55);
end;

procedure Pointers;
var p1 : ^integer;
    p2 : ^real;
    p3 : ^char;
    p4 : ^byte;
    p5 : ^smallint;
    p6 : ^shortint;
    p7 : ^word;
    p8 : ^int64;
    p9 : ^uint64;
    p10 : ^boolean;
    
    i1 : integer;
    i2 : real;
    i3 : char;
    i4 : byte;
    i5 : smallint;
    i6 : shortint;
    i7 : word;
    i8 : int64;
    i9 : uint64;
    i10 : boolean;
    
begin
i1 := 333; p1 := @i1; assert(p1^=333);
i2 := 3.14; p2 := @i2; assert(p2^=3.14);
i3 := 'b'; p3 := @i3; assert(p3^='b');
i4 := 23; p4 := @i4; assert(p4^=23);
i5 := 23; p5 := @i5; assert(p5^=23);
i6 := 23; p6 := @i6; assert(p6^=23);
i7 := 23; p7 := @i7; assert(p7^=23);
i8 := 23; p8 := @i8; assert(p8^=23);
i9 := 23; p9 := @i9; assert(p9^=23);
i10 := true; p10 := @i10; assert(p10^=true);
p1^ := 77; assert(i1=77);
p2^ := 2.71; assert(i2=2.71);
p3^ := 'k'; assert(i3='k');
p4^ := 77; assert(i4=77);
p5^ := 77; assert(i5=77);
p6^ := 77; assert(i6=77);
p7^ := 24; assert(i7=24);
p8^ := 87; assert(i8=87);
p9^ := 77; assert(i9=77);
p10^ := false; assert(i10 = false);
New(p1); New(p2); New(p3); New(p4); New(p5); New(p6); New(p7); New(p8); New(p9); New(p10);
assert(p1^ <> 77);
p1^ := 10; Test5(p1); assert(p1^ = 10);
Test6(p1); assert(p1^ = 45);
Test7(p1); assert(p1^ = 45); Test8(p1);assert(p1^ = 55);
end;

procedure PointersNested;
var p1 : ^integer;
    p2 : ^real;
    p3 : ^char;
    p4 : ^byte;
    p5 : ^smallint;
    p6 : ^shortint;
    p7 : ^word;
    p8 : ^int64;
    p9 : ^uint64;
    p10 : ^boolean;
    
    i1 : integer;
    i2 : real;
    i3 : char;
    i4 : byte;
    i5 : smallint;
    i6 : shortint;
    i7 : word;
    i8 : int64;
    i9 : uint64;
    i10 : boolean;
    
procedure Nested;
begin
 assert(p1^=333);
 assert(p2^=3.14);
assert(p3^='b');
 assert(p4^=23);
 assert(p5^=23);
assert(p6^=23);
assert(p7^=23);
assert(p8^=23);
assert(p9^=23);
assert(p10^=true);
end;
 
procedure Nested2;
procedure Nested3;
begin
i1 := 333; p1 := @i1; assert(p1^=333);
i2 := 3.14; p2 := @i2; assert(p2^=3.14);
i3 := 'b'; p3 := @i3; assert(p3^='b');
i4 := 23; p4 := @i4; assert(p4^=23);
i5 := 23; p5 := @i5; assert(p5^=23);
i6 := 23; p6 := @i6; assert(p6^=23);
i7 := 23; p7 := @i7; assert(p7^=23);
i8 := 23; p8 := @i8; assert(p8^=23);
i9 := 23; p9 := @i9; assert(p9^=23);
i10 := true; p10 := @i10; assert(p10^=true);
Nested;
p1^ := 77; assert(i1=77);
p2^ := 2.71; assert(i2=2.71);
p3^ := 'k'; assert(i3='k');
p4^ := 77; assert(i4=77);
p5^ := 77; assert(i5=77);
p6^ := 77; assert(i6=77);
p7^ := 24; assert(i7=24);
p8^ := 87; assert(i8=87);
p9^ := 77; assert(i9=77);
p10^ := false; assert(i10 = false);
New(p1); New(p2); New(p3); New(p4); New(p5); New(p6); New(p7); New(p8); New(p9); New(p10);
assert(p1^ <> 77);
p1^ := 10; Test5(p1); assert(p1^ = 10);
Test6(p1); assert(p1^ = 45);
Test7(p1); assert(p1^ = 45); Test8(p1);assert(p1^ = 55);
end;

begin
Nested3;
end;

begin
i1 := 333; p1 := @i1; assert(p1^=333);
i2 := 3.14; p2 := @i2; assert(p2^=3.14);
i3 := 'b'; p3 := @i3; assert(p3^='b');
i4 := 23; p4 := @i4; assert(p4^=23);
i5 := 23; p5 := @i5; assert(p5^=23);
i6 := 23; p6 := @i6; assert(p6^=23);
i7 := 23; p7 := @i7; assert(p7^=23);
i8 := 23; p8 := @i8; assert(p8^=23);
i9 := 23; p9 := @i9; assert(p9^=23);
i10 := true; p10 := @i10; assert(p10^=true);
Nested;
p1^ := 77; assert(i1=77);
p2^ := 2.71; assert(i2=2.71);
p3^ := 'k'; assert(i3='k');
p4^ := 77; assert(i4=77);
p5^ := 77; assert(i5=77);
p6^ := 77; assert(i6=77);
p7^ := 24; assert(i7=24);
p8^ := 87; assert(i8=87);
p9^ := 77; assert(i9=77);
p10^ := false; assert(i10 = false);
New(p1); New(p2); New(p3); New(p4); New(p5); New(p6); New(p7); New(p8); New(p9); New(p10);
assert(p1^ <> 77);
p1^ := 10; Test5(p1); assert(p1^ = 10);
Test6(p1); assert(p1^ = 45);
Test7(p1); assert(p1^ = 45); Test8(p1);assert(p1^ = 55);
Nested2;
end;

var p1 : ^integer;
    p2 : ^real;
    p3 : ^char;
    p4 : ^byte;
    p5 : ^smallint;
    p6 : ^shortint;
    p7 : ^word;
    p8 : ^int64;
    p9 : ^uint64;
    p10 : ^boolean;
    
    i1 : integer;
    i2 : real;
    i3 : char;
    i4 : byte;
    i5 : smallint;
    i6 : shortint;
    i7 : word;
    i8 : int64;
    i9 : uint64;
    i10 : boolean;
    
begin
i1 := 333; p1 := @i1; assert(p1^=333);
i2 := 3.14; p2 := @i2; assert(p2^=3.14);
i3 := 'b'; p3 := @i3; assert(p3^='b');
i4 := 23; p4 := @i4; assert(p4^=23);
i5 := 23; p5 := @i5; assert(p5^=23);
i6 := 23; p6 := @i6; assert(p6^=23);
i7 := 23; p7 := @i7; assert(p7^=23);
i8 := 23; p8 := @i8; assert(p8^=23);
i9 := 23; p9 := @i9; assert(p9^=23);
i10 := true; p10 := @i10; assert(p10^=true);
p1^ := 77; assert(i1=77);
p2^ := 2.71; assert(i2=2.71);
p3^ := 'k'; assert(i3='k');
p4^ := 77; assert(i4=77);
p5^ := 77; assert(i5=77);
p6^ := 77; assert(i6=77);
p7^ := 24; assert(i7=24);
p8^ := 87; assert(i8=87);
p9^ := 77; assert(i9=77);
p10^ := false; assert(i10 = false);
Test(p1); Test2(p1); Test3(p1); Test4(p1);
Pointers;
PointersNested;
New(p1); New(p2); New(p3); New(p4); New(p5); New(p6); New(p7); New(p8); New(p9); New(p10);
assert(p1^ <> 77);
p1^ := 10; Test5(p1); assert(p1^ = 10);
Test6(p1); assert(p1^ = 45);
Test7(p1); assert(p1^ = 45); Test8(p1);assert(p1^ = 55);
end.