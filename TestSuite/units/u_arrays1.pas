unit u_arrays1;
type TArr = array[1..4] of integer;
procedure Test(var a : TArr);
begin
assert(a[3] = 777);
end;

procedure Test2(a : TArr);
begin
assert(a[3] = 777);
end;

procedure Test3(var a : TArr);
procedure Nested;
begin
assert(a[3] = 777);
end;

begin
assert(a[3] = 777);
Nested;
end;

procedure Test4(a : TArr);
procedure Nested;
begin
assert(a[3] = 777);
end;

begin
assert(a[3] = 777);
Nested;
end;

procedure Test5(a : Tarr);
begin
 a[3] := 666;
end;

procedure Test6(var a : Tarr);
begin
 a[3] := 888;
end;

type TRec = record a : TArr; end;

var a,b : TArr;
    rec : TRec;
    
begin
a[3] := 777; assert(a[3] = 777);
Test(a);
Test2(a);
Test3(a);
Test4(a);
rec.a := a; assert(rec.a[3] = 777);
b := a;
assert(b[3] = 777);
Test5(b);
assert(b[3] = 777);
Test6(b);
assert(b[3] = 888);
end.