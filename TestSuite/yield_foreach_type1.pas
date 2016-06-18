function f: sequence of integer;
  begin
    var t := Seq(1,3,5);
    foreach var k in t do   
      yield k;
  end;

begin
  Assert(f.SequenceEqual(Seq(1,3,5)));
end.  