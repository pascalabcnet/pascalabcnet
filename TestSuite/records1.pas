type TRec = record
            a: integer;
            b: string;
            end;
     TRec2 = record
             a : TRec;
             b : array[1..3] of TRec;
             end;
     TRec3 = record
             a : TRec2;
             b : array of TRec2;
             end;
   
procedure Test(r : TRec);
begin
assert(r.a = 5); assert(r.b = 'Hello');
end;

procedure Test2(var r : TRec);
begin
assert(r.a = 5); assert(r.b = 'Hello');
end;

procedure Test3(r : TRec2);
begin
assert(r.a.a = 5); assert(r.b[1].a = 5);
end;

procedure Test4(var r : TRec2);
begin
assert(r.a.a = 5); assert(r.b[1].a = 5);
end;

procedure Test5(r : TRec3);
begin
assert(r.a.a.a = 5); assert(r.a.b[1].a = 5);
end;

procedure Test6(var r : TRec3);
begin
assert(r.a.a.a = 5); assert(r.a.b[1].a = 5);
end;

procedure Test7(r : TRec3);
procedure Nested;
begin
assert(r.a.b[3].a = 444);
end;

begin
 r.a.b[3].a := 444;
 Nested;
end;

procedure Test8(var r : TRec3);
procedure Nested;
begin
assert(r.a.b[3].a = 444);
end;
begin
 r.a.b[3].a := 444;
 Nested;
end;

procedure ProcWithRecords;
var r1 : TRec;
    r2 : TRec2;
    r3 : TRec3;
    
begin
SetLength(r3.b,5);
r1.a := 5; r1.b := 'Hello';assert(r1.a = 5); assert(r1.b = 'Hello');
r2.a := r1; r2.b[1] := r1; assert(r2.a.a = 5); assert(r2.a.b = 'Hello');
assert(r2.b[1].a = 5); assert(r2.b[1].b = 'Hello');
r3.a := r2; assert(r3.a.b[1].a = 5);
Test(r1);
Test2(r1);
Test3(r2); Test4(r2);
Test5(r3); Test6(r3);
Test7(r3); 
assert(r3.a.b[3].a <> 444);
Test8(r3);
assert(r3.a.b[3].a = 444);
end;

procedure ProcWithRecords2;
var r1,r4 : TRec;
    r2,r5 : TRec2;
    r3,r6 : TRec3;

procedure Nested;
begin
assert(r1.a = 5); assert(r1.b = 'Hello');
assert(r2.a.a = 5); assert(r2.a.b = 'Hello');
assert(r2.b[1].a = 5); assert(r2.b[1].b = 'Hello');
assert(r3.a.b[1].a = 5);
end;
  
begin
SetLength(r3.b,5);
r1.a := 5; r1.b := 'Hello';assert(r1.a = 5); assert(r1.b = 'Hello');
r4 := r1; assert(r4.a = 5); assert(r4.b = 'Hello');
r2.a := r1; r2.b[1] := r1; assert(r2.a.a = 5); assert(r2.a.b = 'Hello');
assert(r2.b[1].a = 5); assert(r2.b[1].b = 'Hello');
r5 := r2; r5.b[1] := r1; assert(r5.a.a = 5); assert(r5.a.b = 'Hello');
assert(r5.b[1].a = 5); assert(r5.b[1].b = 'Hello');
r3.a := r2; assert(r3.a.b[1].a = 5);
r6 := r3; assert(r6.a.b[1].a = 5);
Nested;
Test(r1);
Test2(r1);
Test3(r2); Test4(r2);
Test5(r3); Test6(r3);
Test7(r3); 
assert(r3.a.b[3].a <> 444);
Test8(r3);
assert(r3.a.b[3].a = 444);
end;
var r1,r4 : TRec;
    r2,r5 : TRec2;
    r3,r6 : TRec3;
    
begin
SetLength(r3.b,5);
r1.a := 5; r1.b := 'Hello';assert(r1.a = 5); assert(r1.b = 'Hello');
r4 := r1;
assert(r4.a = 5); assert(r4.b = 'Hello');
r2.a := r1; r2.b[1] := r1; assert(r2.a.a = 5); assert(r2.a.b = 'Hello');
assert(r2.b[1].a = 5); assert(r2.b[1].b = 'Hello');
r5 := r2; assert(r5.a.a = 5); assert(r5.a.b = 'Hello');
assert(r5.b[1].a = 5); assert(r5.b[1].b = 'Hello');
r3.a := r2; assert(r3.a.b[1].a = 5);
r6 := r3; assert(r6.a.b[1].a = 5);
Test(r1);
Test2(r1);
Test3(r2); Test4(r2);
Test5(r3); Test6(r3);
Test7(r3); 
assert(r3.a.b[3].a <> 444);
Test8(r3);
assert(r3.a.b[3].a = 444);
ProcWithRecords;
ProcWithRecords2;
end.