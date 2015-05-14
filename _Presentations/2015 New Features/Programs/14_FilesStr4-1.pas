begin
  var lines := ReadLines('freqs.txt');
  var ss := lines.Select(s->s.ToWords());
  var verblines := ss.Where(ss->ss[3] = 'verb');
  var onlyverbs := verblines.Select(ss->ss[2]);
  onlyverbs.Print(NewLine);
end.