type
  cl = class
  public a := 5;
  
  procedure pr(xx: integer);
  begin
    xx := 563;
    xx += 1;
    assert(xx = 564);
    
    begin
      var l := Seq(1,2,3);
      var y := 56;
      self.a := 3;
      a := 4;
      assert(self.a = 4);
      var tt := l.Select(x -> begin a += 1; result := x + a + y + xx + self.a end).ToList;
      assert(tt[0] = 631);
      assert(tt[1] = 634);
      assert(tt[2] = 637);
      assert(a = 7);
      writeln();
      xx += 1;
      y += 1;
      assert(y = 57);
      assert(xx = 565);
    end;
  end;
end;

begin
  (new cl()).pr(563);
end.