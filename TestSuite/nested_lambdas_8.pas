begin
  var yy := 5;
  Range(1,1).Select(x -> begin
                           var l := x + yy; //6
                           begin
                              writeln(l);
                              begin
                                writeln(l);
                              end;
                           end;
                           writeln(yy);
                           result := Range(1, x).Select(y -> y + l + yy + Seq(1,2).Select(z -> l + yy).First())
                         end);
 writeln(yy);
end.