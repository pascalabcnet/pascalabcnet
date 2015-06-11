begin
  var a := 3;
  var ttt := Seq(1,2).Select(x -> begin 
                                   var t := 5; 
                                   result := Range(1,a+x).Select(y -> begin
                                                                        var g := 5;
                                                                        t += 5;
                                                                        result := Range(1,2).Where(f -> f + a > 0).Select(z -> z + g + t);
                                                                      end)
                                  end).ToList();                             
  assert(ttt[0].ToList()[0].ToList()[0] = 31);
end.