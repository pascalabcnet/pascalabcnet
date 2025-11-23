procedure Test;
var i : integer;
    o : object;
begin
 i := 3;
 assert(i.ToString()='3');
 o := 3;
 assert(o.ToString()='3');
end;

procedure Test2(i : integer);
begin
 i := 3;
 assert(i.ToString()='3');
end;

procedure Test3(var i : integer);
begin
 i := 3;
 assert(i.ToString()='3');
end;

procedure Test4;
var i : integer;
procedure Nested;
begin
  i := 3;
 assert(i.ToString()='3');
end;
begin
 i := 3;
 assert(i.ToString()='3');
 Nested;
end;

procedure Test5(var i : integer);
procedure Nested;
begin
  i := 3;
 assert(i.ToString()='3');
end;
begin
 i := 3;
 assert(i.ToString()='3');
 Nested;
end;


var i : integer;
    o : object;
    
begin
assert(2.ToString()='2');
i := 3;
assert(i.ToString()='3'); 
o := i;
assert(o.ToString() = '3');
Test;
Test2(3);
Test3(i);
Test4;
Test5(i);
end.