procedure Test(var a : integer);
procedure Nested;
begin
a := 4;
end;

begin
Nested;
end;
    
begin
var i := 45;
Test(i);
end.