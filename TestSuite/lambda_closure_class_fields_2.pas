type
  cl = class
  private a := 5;
    
  procedure pr();
  begin
    var l := new List<integer>;
    l.Add(1);
    l.Add(2);
    l.Add(3);
    var y := 56;
    var tt := l.Select(x -> begin a *= 2; result := x + a + y end).ToList;
    assert(tt[0] = 67);
    assert(tt[1] = 78);
    assert(tt[2] = 99);
  end;
end;

begin
  (new cl()).pr();
end.