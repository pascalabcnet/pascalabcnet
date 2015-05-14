uses test_compiled_types_in_modules_unit;

var a:MyString;
    b:myarr;

procedure xxx(params x:myarr);
var i:integer;
begin
  for i:=0 to x.length-1 do
    write(x[i]);
end;

begin
  a:='jjjjjj';
  writeln(a);
  xxx(1,2,3,4,5);
  readln;
end.