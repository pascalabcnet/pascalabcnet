var total : integer;

procedure Test;
var a : integer;
 	i : integer;

function Nested : integer;
 begin
  Nested := 3;
 end;

begin
i := 1; a := total;
 while i < 1000 do
begin
  a := a+1;
  i := i +1;
end; 
writeln(a);
 a := Nested;
end;

procedure Test2;
var a : integer;
 	i : integer;

function Nested2 : integer;
 begin
  Nested2 := 99999;
 end;

function Nested : integer;
 begin
  Nested := Nested2;
 end;


begin
i := 1; a := total;
 while i < 1000 do
begin
  a := a+1;
  i := i +1;
end; 

 a := Nested;
writeln(a);
end;

procedure Func;
var f : integer;

procedure Nested(b : integer);
var d : integer;

function NestedFunc : integer;
begin
 f := 567;
 if total < 10 then 
 begin
 total := total+1; 
 Nested(6);
  
 end;
writeln(b);
 NestedFunc := 234;
end;

begin
writeln(f);
 b := 666;
 f := NestedFunc;
end;

begin
 f := 1234;
 Nested(777);
end;

function fact(n : integer) : integer;
begin
 if (n=0) or (n=1) then fact := 1
 else fact := n * fact(n-1);
end;

procedure some_proc(s, t : real);overload;
var k : integer;
    //ch : char;
begin
 //ch := 'i';
 k := 1024; Func;
end;

procedure some_proc(s : real);overload;
var u, v : integer;
begin
 u := 1;
 v := 30;
 repeat
  u := u + 1;
  v := v - 1;
 until u < v
end;

function pi : real;
begin
 pi := 3.14;
end;

function sum(a, b : integer) : integer;
var c, d : integer;
    f : boolean;
begin
 //f := true and (a<b);
 //c := 3; d := 10;
 some_proc(2.7);some_proc(3,2);
 sum := a+b;
end;

procedure SuperTest(k : integer);
procedure Nested(f : real);
begin
 writeln(f+k);
end;

begin
 
 Nested(3.14);
end;

var
 i, j, s :integer;
 r : real;
//comment
begin
 total := 1;
 i := 1; 
{for i := 1 to 10000 do
j := sum(3,5);}
 Test;
Test2;
 writeln(j);
 if i < 5 then
 begin
 end;
 
 i := 1;
 while i<10 do
 begin
  s := s + i;
  i := i + 1;
 end;
 
 for i := 1 to 100 do
 begin
  r := r + i;
 end;
 //r := pi;
 s := (i+j)*3;
 SuperTest(10);
end.