unit u_delegates1;
type TFunc1 = function : integer;
     TFunc2 = function(i : integer) : integer;
     TFunc3 = function(var i : integer) : integer;
     TFunc4 = function(params a : array of integer) : integer;
     TFunc5 = function(const i : integer) : integer;
     TProc1 = procedure;
     TProc2 = procedure(i : integer);
     TProc3 = procedure(params a : array of integer);
     
function Func1 : integer;
begin
end;

function Func2(i : integer) : integer;
begin
end;

function Func3(var i : integer) : integer;
begin
end;

function Func4(params a : array of integer) : integer;
begin
end;

function Func5(const i : integer) : integer;
begin
end;

procedure Proc1;
begin
end;

procedure Proc2(i: integer);
begin
end;

procedure Proc3(params arr : array of integer);
begin
end;

procedure Test1(f : TFunc1);
begin
f;
end;

procedure Test2(f : TFunc2);
begin
f(2);
end;

procedure Test3(f : TFunc3);
begin
var i := 1;
f(i);
end;

procedure Test4(f : TFunc5);
begin
var i := 0;
f(i);
end;

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