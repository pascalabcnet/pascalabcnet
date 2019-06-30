procedure p1(i1,i2:integer);
begin
  Assert(1=1);
end;

begin
  Seq(0).ForEach(i1->
  begin
    Seq(0).ForEach(i2 ->p1(i1, i2));
  end);
end.