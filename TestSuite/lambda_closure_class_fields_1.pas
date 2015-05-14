type
  cl = class
  public a := 5;
  
  function pr(): List<integer>;
  begin
    var l := Seq(1,2);
    var y := 56;
    result := l.Select(x -> x + a + y).ToList;
  end;
end;

begin
  var l := (new cl).pr;
  assert(l[0] = 62);
  assert(l[1] = 63);
end.