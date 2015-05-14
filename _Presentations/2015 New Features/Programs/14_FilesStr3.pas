begin
  foreach var s in ReadLines('freqs.txt') do
  begin
    var ss := s.ToWords;
    if ss[3] = 'verb' then
      writeln(ss[2]);
  end;
end.