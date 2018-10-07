var c: string := '123'[1:3];
var c1: string := '123'[1];

begin
  Assert(c='12');
  Assert(c1='1')
end.