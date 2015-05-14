begin
  ReadLines('freqs.txt').Select(s->s.ToWords()).
    Where(ss->ss[3] = 'verb').
    Select(ss->ss[2]).Print(NewLine);
end.