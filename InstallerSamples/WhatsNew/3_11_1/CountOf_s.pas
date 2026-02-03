// 3.11.1. CountOf с параметром allowOverlap
begin
  var s := 'arara';
  s.CountOf('ara').Println;
  s.CountOf('ara',allowOverlap := True).Println;
end.