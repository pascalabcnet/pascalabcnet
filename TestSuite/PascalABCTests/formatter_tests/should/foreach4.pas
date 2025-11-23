begin
  var i := 0;
  foreach var s in Arr(1, 2, 3) do
  begin
    i := s;
    if s = 2 then
      break;
  end;
  assert(i = 2);
end.