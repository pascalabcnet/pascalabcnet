type cl = class
  protected gg:integer  := 9;
  public
    function h(k: integer):string;
    begin
      var a := 5;
      foreach var tt in Seq(1,2,3) do
      begin
        var d := Seq(1,2,3).Select(x -> begin 
                                          var e := 65;
                                         result := e + x + tt + self.gg + Range(1,2).Select(y -> gg + self.gg+y+a + tt).First()
                                       end).ToList();
        assert(d[0] = 100 + (tt * 2 - 1)); 
      end;
    end;
end;

begin
  (new cl).h(5);
end.