//winonly
procedure Test1(b : byte);
begin
assert(b=45);
end;

procedure Test2(c : char);
begin
assert(c='a');
end;

procedure Test3(i : integer);
begin
assert(i=45);
end;

procedure Test4(sm : smallint);
begin
assert(sm=45);
end;

procedure Test5(sh : shortint);
begin
assert(sh=45);
end;

procedure Test6(w : word);
begin
assert(w=45);
end;

procedure Test7(lw : longword);
begin
assert(lw=45);
end;

procedure Test8(li: int64);
begin
assert(li=45);
end;

procedure Test9(ui : uint64);
begin
assert(ui=45);
end;

type ByteDiap = byte(1)..byte(5);
     SmallDiap = smallint(1)..smallint(5);
     ShortDiap = shortint(1)..shortint(5);
     WordDiap = word(1)..word(5);
     IntDiap = 1..5;
     LongWordDiap = longword(1)..longword(10);
     LongDiap = int64(1)..int64(10);
     UInt64Diap = uint64(1)..uint64(10);
     
procedure DTest1(a : ByteDiap);
begin
end;

procedure DTest2(a : SmallDiap);
begin
end;

procedure DTest3(a : ShortDiap);
begin
end;

procedure DTest4(a : WordDiap);
begin
end;

procedure DTest5(a : IntDiap);
begin
end;

procedure DTest6(a : LongWordDiap);
begin
end;

procedure DTest7(a : LongDiap);
begin
end;

procedure DTest8(a : Uint64Diap);
begin
end;

procedure Test;
var d1 : IntDiap;
    d2 : 'a'..'z';
    db : ByteDiap;
    dsm : SmallDiap;
    dsh : ShortDiap;
    dw : WordDiap;
    dli : LongDiap;
    dlw : LongWordDiap;
    dui : UInt64Diap;
    
begin
 d1 := 45;
 Test1(d1); Test3(d1); Test4(d1); Test5(d1); Test6(d1); Test7(d1); Test8(d1); Test9(d1);
 db := 45;
 Test1(db); Test3(db); Test4(db); Test5(db); Test6(db); Test7(db); Test8(db); Test9(db);
 dsm := 45;
 Test1(dsm); Test3(dsm); Test4(dsm); Test5(dsm); Test6(dsm); Test7(dsm); Test8(dsm); Test9(dsm);
 dsh := 45;
 Test1(dsh); Test3(dsh); Test4(dsh); Test5(dsh); Test6(dsh); Test7(dsh); Test8(dsh); Test9(dsh);
 dw := 45;
 Test1(dw); Test3(dw); Test4(dw); Test5(dw); Test6(dw); Test7(dw); Test8(dw); Test9(dw);
 dli := 45;
 Test1(dli); Test3(dli); Test4(dli); Test5(dli); Test6(dli); Test7(dli); Test8(dli); Test9(dli);
 dlw := 45;
 Test1(dlw); Test3(dlw); Test4(dlw); Test5(dlw); Test6(dlw); Test7(dlw); Test8(dlw); Test9(dlw);
 dui := 45;
 Test1(dui); Test3(dui); Test4(dui); Test5(dui); Test6(dui); Test7(dui); Test8(dui); Test9(dui);
 d2 := 'a';
 Test2(d2);
 
 DTest1(5); DTest2(5); DTest3(5); DTest4(5); DTest6(5); DTest7(5); DTest8(5);
 DTest1(d1); DTest1(db); DTest1(dsm); DTest1(dsh); DTest1(dw); DTest1(dlw); DTest1(dli); DTest1(dui);
 DTest2(d1); 
 DTest2(db); 
 DTest2(dsm); 
 DTest2(dsh); 
 DTest2(dw); 
 DTest2(dlw); 
 DTest2(dli); 
 DTest2(dui);
 DTest3(d1); DTest3(db); DTest3(dsm); DTest3(dsh); DTest3(dw); DTest3(dlw); DTest3(dli); DTest3(dui);
 DTest4(d1); DTest4(db); DTest4(dsm); DTest4(dsh); DTest4(dw); DTest4(dlw); DTest4(dli); DTest4(dui);
 DTest5(d1); DTest5(db); DTest5(dsm); DTest5(dsh); DTest5(dw); DTest5(dlw); DTest5(dli); DTest5(dui);
 DTest6(d1); DTest6(db); DTest6(dsm); DTest6(dsh); DTest6(dw); DTest6(dlw); DTest6(dli); DTest6(dui);
 DTest7(d1); DTest7(db); DTest7(dsm); DTest7(dsh); DTest7(dw); DTest7(dlw); DTest7(dli); DTest7(dui);
 DTest8(d1); DTest8(db); DTest8(dsm); DTest8(dsh); DTest8(dw); DTest8(dlw); DTest8(dli); DTest8(dui);
end;

var d1 : IntDiap;
    d2 : 'a'..'z';
    db : ByteDiap;
    dsm : SmallDiap;
    dsh : ShortDiap;
    dw : WordDiap;
    dli : LongDiap;
    dlw : LongWordDiap;
    dui : UInt64Diap;
    
begin
 d1 := 45;
 Test1(d1); Test3(d1); Test4(d1); Test5(d1); Test6(d1); Test7(d1); Test8(d1); Test9(d1);
 db := 45;
 Test1(db); Test3(db); Test4(db); Test5(db); Test6(db); Test7(db); Test8(db); Test9(db);
 dsm := 45;
 Test1(dsm); Test3(dsm); Test4(dsm); Test5(dsm); Test6(dsm); Test7(dsm); Test8(dsm); Test9(dsm);
 dsh := 45;
 Test1(dsh); Test3(dsh); Test4(dsh); Test5(dsh); Test6(dsh); Test7(dsh); Test8(dsh); Test9(dsh);
 dw := 45;
 Test1(dw); Test3(dw); Test4(dw); Test5(dw); Test6(dw); Test7(dw); Test8(dw); Test9(dw);
 dli := 45;
 Test1(dli); Test3(dli); Test4(dli); Test5(dli); Test6(dli); Test7(dli); Test8(dli); Test9(dli);
 dlw := 45;
 Test1(dlw); Test3(dlw); Test4(dlw); Test5(dlw); Test6(dlw); Test7(dlw); Test8(dlw); Test9(dlw);
 dui := 45;
 Test1(dui); Test3(dui); Test4(dui); Test5(dui); Test6(dui); Test7(dui); Test8(dui); Test9(dui);
 d2 := 'a';
 Test2(d2);
 
 DTest1(5); DTest2(5); DTest3(5); DTest4(5); DTest6(5); DTest7(5); DTest8(5);
 DTest1(d1); DTest1(db); DTest1(dsm); DTest1(dsh); DTest1(dw); DTest1(dlw); DTest1(dli); DTest1(dui);
 DTest2(d1); 
 DTest2(db); 
 DTest2(dsm); 
 DTest2(dsh); 
 DTest2(dw); 
 DTest2(dlw); 
 DTest2(dli); 
 DTest2(dui);
 DTest3(d1); DTest3(db); DTest3(dsm); DTest3(dsh); DTest3(dw); DTest3(dlw); DTest3(dli); DTest3(dui);
 DTest4(d1); DTest4(db); DTest4(dsm); DTest4(dsh); DTest4(dw); DTest4(dlw); DTest4(dli); DTest4(dui);
 DTest5(d1); DTest5(db); DTest5(dsm); DTest5(dsh); DTest5(dw); DTest5(dlw); DTest5(dli); DTest5(dui);
 DTest6(d1); DTest6(db); DTest6(dsm); DTest6(dsh); DTest6(dw); DTest6(dlw); DTest6(dli); DTest6(dui);
 DTest7(d1); DTest7(db); DTest7(dsm); DTest7(dsh); DTest7(dw); DTest7(dlw); DTest7(dli); DTest7(dui);
 DTest8(d1); DTest8(db); DTest8(dsm); DTest8(dsh); DTest8(dw); DTest8(dlw); DTest8(dli); DTest8(dui);
 
 Test;
end.