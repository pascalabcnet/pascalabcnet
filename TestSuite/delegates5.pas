uses delegates5u;
var f1 : TFunc1;
    f2 : TFunc2;
    f3 : TFunc3;
    f4 : TFunc4;
    f5 : TFunc5;
    i : integer;
    p1 : TProc1;
    p2 : TProc2;
    p3 : TProc3;
    
begin
f1 := Func1;
 f2 := Func2;
 f3 := Func3;
 f4 := Func4;
 f5 := Func5;
 f1;
 f2(2);
 f3(i);
 f4(1,2,3);
 f5(i);
 p1 := Proc1;
 p2 := Proc2;
 p3 := Proc3;
 p1;
 p2(2);
 p3(1,2);
 Test1(Func1);
 Test2(Func2);
 Test3(Func3);
 Test4(Func5);
end.