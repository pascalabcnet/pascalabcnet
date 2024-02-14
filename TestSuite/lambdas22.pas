begin
  var i := 0;
  var p: Action0 := ()->begin i := 1 end;
  var l := Lst&<Action0>();
  l.Add(()->begin i := 2 end);
  var l2 := l.ConvertAll(a->p);
  l2[0]();
  assert(i = 1);
end.