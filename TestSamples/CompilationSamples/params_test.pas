procedure test(params args: array of integer);
var i:=0;
begin
  for i:=0 to args.length-1 do
    writeln(args[i]);
end;

var a:array of integer;

begin
  setlength(a,3);
  a[0]:=3;a[1]:=4;a[2]:=5;
  test(1,2);
  test(a);
  readln;
end.