unit u_incdecdiap1;
type TDiap = 1..7;
     TDiap2 = byte(1)..byte(7);
     TDiap3 = smallint(1)..smallint(7);
     TDiap4 = shortint(1)..shortint(7);
     TDiap5 = word(1)..word(7);
     TDiap6 = longword(1)..longword(7);
     TDiap7 = int64(1)..int64(7);
     TDiap8 = uint64(1)..uint64(7);
     
procedure Test;
var a : TDiap;
    a2 : TDiap2;
    a3 : TDiap3;
    a4 : TDiap4;
    a5 : TDiap5;
    a6 : TDiap6;
    a7 : TDiap7;
    a8 : TDiap8;
    
begin
a := 3; assert(a=3);
Inc(a); assert(a=4);
Dec(a); assert(a=3);
Inc(a,2); assert(a=5);
Dec(a,2); assert(a=3);

a2 := 3; assert(a2=3);
Inc(a2); assert(a2=4);
Dec(a2); assert(a2=3);
Inc(a2,2); assert(a2=5);
Dec(a2,2); assert(a2=3);

a3 := 3; assert(a3=3);
Inc(a3); assert(a3=4);
Dec(a3); assert(a3=3);
Inc(a3,2); assert(a3=5);
Dec(a3,2); assert(a3=3);

a4 := 3; assert(a4=3);
Inc(a4); assert(a4=4);
Dec(a4); assert(a4=3);
Inc(a4,2); assert(a4=5);
Dec(a4,2); assert(a4=3);

a5 := 3; assert(a5=3);
Inc(a5); assert(a5=4);
Dec(a5); assert(a5=3);
Inc(a5,2); assert(a5=5);
Dec(a5,2); assert(a5=3);

a6 := 3; assert(a6=3);
Inc(a6); assert(a6=4);
Dec(a6); assert(a6=3);
Inc(a6,2); assert(a6=5);
Dec(a6,2); assert(a6=3);

a7 := 3; assert(a7=3);
Inc(a7); assert(a7=4);
Dec(a7); assert(a7=3);
Inc(a7,2); assert(a7=5);
Dec(a7,2); assert(a7=3);

a8 := 3; assert(a8=3);
Inc(a8); assert(a8=4);
Dec(a8); assert(a8=3);
Inc(a8,2); assert(a8=5);
Dec(a8,2); assert(a8=3);
end;

procedure Test2;
var a : TDiap;
    a2 : TDiap2;
    a3 : TDiap3;
    a4 : TDiap4;
    a5 : TDiap5;
    a6 : TDiap6;
    a7 : TDiap7;
    a8 : TDiap8;
    
procedure Nested;
begin
a := 3; assert(a=3);
Inc(a); assert(a=4);
Dec(a); assert(a=3);
Inc(a,2); assert(a=5);
Dec(a,2); assert(a=3);

a2 := 3; assert(a2=3);
Inc(a2); assert(a2=4);
Dec(a2); assert(a2=3);
Inc(a2,2); assert(a2=5);
Dec(a2,2); assert(a2=3);

a3 := 3; assert(a3=3);
Inc(a3); assert(a3=4);
Dec(a3); assert(a3=3);
Inc(a3,2); assert(a3=5);
Dec(a3,2); assert(a3=3);

a4 := 3; assert(a4=3);
Inc(a4); assert(a4=4);
Dec(a4); assert(a4=3);
Inc(a4,2); assert(a4=5);
Dec(a4,2); assert(a4=3);

a5 := 3; assert(a5=3);
Inc(a5); assert(a5=4);
Dec(a5); assert(a5=3);
Inc(a5,2); assert(a5=5);
Dec(a5,2); assert(a5=3);

a6 := 3; assert(a6=3);
Inc(a6); assert(a6=4);
Dec(a6); assert(a6=3);
Inc(a6,2); assert(a6=5);
Dec(a6,2); assert(a6=3);

a7 := 3; assert(a7=3);
Inc(a7); assert(a7=4);
Dec(a7); assert(a7=3);
Inc(a7,2); assert(a7=5);
Dec(a7,2); assert(a7=3);

a8 := 3; assert(a8=3);
Inc(a8); assert(a8=4);
Dec(a8); assert(a8=3);
Inc(a8,2); assert(a8=5);
Dec(a8,2); assert(a8=3);
end;
    
begin
a := 3; assert(a=3);
Inc(a); assert(a=4);
Dec(a); assert(a=3);
Inc(a,2); assert(a=5);
Dec(a,2); assert(a=3);

a2 := 3; assert(a2=3);
Inc(a2); assert(a2=4);
Dec(a2); assert(a2=3);
Inc(a2,2); assert(a2=5);
Dec(a2,2); assert(a2=3);

a3 := 3; assert(a3=3);
Inc(a3); assert(a3=4);
Dec(a3); assert(a3=3);
Inc(a3,2); assert(a3=5);
Dec(a3,2); assert(a3=3);

a4 := 3; assert(a4=3);
Inc(a4); assert(a4=4);
Dec(a4); assert(a4=3);
Inc(a4,2); assert(a4=5);
Dec(a4,2); assert(a4=3);

a5 := 3; assert(a5=3);
Inc(a5); assert(a5=4);
Dec(a5); assert(a5=3);
Inc(a5,2); assert(a5=5);
Dec(a5,2); assert(a5=3);

a6 := 3; assert(a6=3);
Inc(a6); assert(a6=4);
Dec(a6); assert(a6=3);
Inc(a6,2); assert(a6=5);
Dec(a6,2); assert(a6=3);

a7 := 3; assert(a7=3);
Inc(a7); assert(a7=4);
Dec(a7); assert(a7=3);
Inc(a7,2); assert(a7=5);
Dec(a7,2); assert(a7=3);

a8 := 3; assert(a8=3);
Inc(a8); assert(a8=4);
Dec(a8); assert(a8=3);
Inc(a8,2); assert(a8=5);
Dec(a8,2); assert(a8=3);
Nested;
end;

     
var a : TDiap;
    a2 : TDiap2;
    a3 : TDiap3;
    a4 : TDiap4;
    a5 : TDiap5;
    a6 : TDiap6;
    a7 : TDiap7;
    a8 : TDiap8;
    
begin
a := 3; assert(a=3);
Inc(a); assert(a=4);
Dec(a); assert(a=3);
Inc(a,2); assert(a=5);
Dec(a,2); assert(a=3);

a2 := 3; assert(a2=3);
Inc(a2); assert(a2=4);
Dec(a2); assert(a2=3);
Inc(a2,2); assert(a2=5);
Dec(a2,2); assert(a2=3);

a3 := 3; assert(a3=3);
Inc(a3); assert(a3=4);
Dec(a3); assert(a3=3);
Inc(a3,2); assert(a3=5);
Dec(a3,2); assert(a3=3);

a4 := 3; assert(a4=3);
Inc(a4); assert(a4=4);
Dec(a4); assert(a4=3);
Inc(a4,2); assert(a4=5);
Dec(a4,2); assert(a4=3);

a5 := 3; assert(a5=3);
Inc(a5); assert(a5=4);
Dec(a5); assert(a5=3);
Inc(a5,2); assert(a5=5);
Dec(a5,2); assert(a5=3);

a6 := 3; assert(a6=3);
Inc(a6); assert(a6=4);
Dec(a6); assert(a6=3);
Inc(a6,2); assert(a6=5);
Dec(a6,2); assert(a6=3);

a7 := 3; assert(a7=3);
Inc(a7); assert(a7=4);
Dec(a7); assert(a7=3);
Inc(a7,2); assert(a7=5);
Dec(a7,2); assert(a7=3);

a8 := 3; assert(a8=3);
Inc(a8); assert(a8=4);
Dec(a8); assert(a8=3);
Inc(a8,2); assert(a8=5);
Dec(a8,2); assert(a8=3);
Test;
Test2;
end.