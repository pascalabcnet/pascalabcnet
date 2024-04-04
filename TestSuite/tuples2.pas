begin
  var a := |System.ValueTuple.Create('c',1)|;
  var b := false;
  foreach var (ch, i) in a do
    begin
      assert(ch = 'c');
      assert(i = 1);
      b := true;
    end;
  assert(b);
end.