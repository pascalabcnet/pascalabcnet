type 
   del1=procedure;
   del2=function:array of del1;

procedure p;
begin
  writeln('p');
end;

function f:array of del1;
var a:array of del1;
begin
  SetLength(a,1);
  a[0]:=p;
  result:=a;
end;

var d:del2;
    d1:del1;

begin
  d:=f;
  d()[0];
  d1:=d()[0];
  d1;
  readln;
end.