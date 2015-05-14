procedure p();
begin
  var l := new List<integer>;
  l.Add(1);
  l.Add(4);
  l.Add(-2);
  begin
    var xx := 1;
    var ll := l.Select(x -> begin xx += 1; result := x + xx end).ToList();
    assert(xx = 4);
    assert(ll.Contains(3));
    assert(ll.Contains(7));
    assert(ll.Contains(2));
  end;
  begin
    var b := 1;
    var t := l.Where(x -> begin b += 1; result := x > b end).ToList;
    assert(b = 4);
    assert(t.Count = 1);
    assert(t.Contains(4));
  end;
  
  var yy := l.Where(x -> x > 0).ToList;
  assert(yy.Count = 2);
  assert(yy.Contains(1));
  assert(yy.Contains(4));
end;

begin
  p();
end.