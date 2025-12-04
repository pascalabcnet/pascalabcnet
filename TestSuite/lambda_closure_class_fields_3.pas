type
  cl = class
  public a := 5;
  
  procedure pr();
  begin
    var l := new List<integer>;
    l.Add(1);
    l.Add(2);
    l.Add(3);
    var y := 56;
    self.a := 3;
    a *= 2;
    var tt := l.Select(x -> (x + a).ToString + self.ToString()).ToList;
    foreach var t in tt do
      writeln(t);
  end;
end;

begin
  (new cl()).pr();
end.