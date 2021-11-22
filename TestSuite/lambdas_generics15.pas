type
  Container = record
    val: integer;
  end;
  
begin
  var c: Container;
  c.val := 2;
  var s: sequence of Container := Arr(c);
  assert(s.Max(a->a.val) = 2);
end.