begin
  var (a,b) := (2,2);
  match (a,b) with
  (var x,var y) when x < y: assert(false);
  (var x,var y) when x = y: exit;
  (var x,var y) when x > y: assert(false);
  end;
  assert(false)
end.
