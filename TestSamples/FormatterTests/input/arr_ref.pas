
var
a:array[1..10] of integer;
o:object;
procedure p(var i:integer);
begin
i:=i+1;
end;

begin
a[2]:=1;
a[1]:=1;
writeln(a[1]);
writeln(a[2]);
p(a[1]);
writeln(a[1]);
writeln(a[2]);
readln;
end.