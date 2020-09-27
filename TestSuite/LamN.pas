var (m1, n1) := Seq(7,8);
begin
  var (m, n) := Seq(3,4);
  Assert((m,n)=(3,4));
  (m, n) := Arr(5,6);
  Assert((m,n)=(5,6));
  Assert((m1,n1)=(7,8));
  ArrGen(10,x->x+m+n+m1+n1);
end.