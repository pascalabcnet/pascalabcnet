begin
  var (a,b) := (3,2);
  match (a,b) with
  (var x,var y) when x < y: Print('a < b');
  (var x,var y) when x = y: Print('a = b');
  (var x,var y) when x > y: Print('a > b');
  end;
end.