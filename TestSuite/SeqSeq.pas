// #2079
begin
  Seq&<sequence of byte>(Seq&<byte>);
  var q := Seq&<sequence of byte>(Seq&<byte>,Seq&<byte>);
  Assert(q.Count=2);
  Assert(q.First.Count=0);
end.