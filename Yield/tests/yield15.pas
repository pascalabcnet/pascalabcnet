function Gen(n: integer): sequence of real;
  var j := n;
  var jj : real := j;
  begin
      var x := 777;
      yield jj;
      for var x := 1 to j do
      begin
        yield x;
      end;
  end;

begin
  foreach var x in Gen(5) do
    Print(x);
end.