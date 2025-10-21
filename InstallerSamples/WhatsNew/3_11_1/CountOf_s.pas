begin
  var s := 'arara';
  s.CountOf('ara').Println;
  s.CountOf('ara',allowOverlap := True).Println;
end.