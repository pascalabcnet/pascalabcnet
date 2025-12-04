uses extensionmethods10u;

procedure Test(x: integer);
begin
  assert(x=3);
end;
begin
  var a := Seq(3,3,3);
  a.MyForeach(Test);
end.