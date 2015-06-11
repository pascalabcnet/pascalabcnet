type cl = class
  private gg := 9;
  public
    procedure h(k: integer);
    begin
      var a := 5;
      Seq(1,2,3).Select(x -> begin 
                               var t := 5; 
                               result := Range(1,a).Select(y -> begin
                                                                  var g := 5;
                                                                  writeln(a);
                                                                  result := Range(1,3).Where(f -> f + a > 0).Select(z -> z + g + t);
                                                                end)
                             end);
   end;
end;

begin
  (new cl).h(6);
end.