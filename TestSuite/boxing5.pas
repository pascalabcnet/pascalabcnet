begin
  var seg := System.ArraySegment&<integer>.Create(new integer[3](2,3,4), 0, 2);
  assert(seg.Sum = 5);
end.