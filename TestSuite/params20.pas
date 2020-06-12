procedure p1(params a: array of ()->array of byte);
begin 
  Assert(1=1)
end;

function GetBArr := new byte[0];

begin
  p1(GetBArr);
end.