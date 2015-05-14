procedure p;
var arr,work: array ['à'..'ÿ'] of integer;
begin
  arr['á']:=666;
  work:=arr;
  work['á']:=777;
  writeln(arr['á']);
end;

var arr,work: array ['à'..'ÿ'] of integer;

begin
  p;
  arr['á']:=888;
  work:=arr;
  work['á']:=999;
  writeln(arr['á']);
  readln;
end.