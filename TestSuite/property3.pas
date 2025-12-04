begin
  var l: LinkedList<integer>;
  l := new LinkedList<integer>;
  l.AddLast(2);
  var node := l.First;
  assert(node.Value = 2);
end.