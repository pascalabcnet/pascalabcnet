uses test0171u, test0172u;

procedure TClass.Test(a : integer);
begin
  assert(a = 3);
end;

var t : TClass;

begin
t := new TClass;
t.Test(3);
t.Test2('abc');  
end.