procedure p;

function Sum(a,b: integer): integer;
begin
  var Sum := 0;
  Sum := 666;
end;

begin
  Assert(Sum(1,10)=0);
end;

begin
  p;
end.