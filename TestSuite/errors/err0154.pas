type TArr = array of integer;

procedure Test(var a : TArr);
begin
end;

const s : TArr = (1,2,3);

begin
Test(s);
end.