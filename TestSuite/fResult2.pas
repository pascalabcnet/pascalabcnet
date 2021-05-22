function Sum(a,b: integer): integer;
begin
  var Sum := 0;
  for var i:=a to b do
  begin  
    Sum := Sum + i;
  end;  
end;

begin
  Assert(Sum(1,10)=0);
end.