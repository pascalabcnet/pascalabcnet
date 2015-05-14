
function f(var j:integer):integer;
begin
j:=j+1;
result:=j;
end;

procedure p(j:integer;params arr:array of integer);
begin
writeln(j);
writeln(arr[0]);
end;

var
i:integer;

begin
i:=1;
p(f(i),i);
readln;
end.