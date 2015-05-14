type rec=record
       i:integer;
     end;

function f:rec;
var r:rec;
begin
 r.i:=1;
 f:=r;
end;

var a:array [1..2]of integer;
begin
  a[f().i]:=2;
  writeln(a[1]);
  readln;
end.