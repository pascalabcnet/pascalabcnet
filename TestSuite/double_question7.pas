
function f0 := $12345678;

function f1(n: integer?) := (n??f0).Value;

begin
  // Выводит 0 вместо 12345678
  assert(f1(nil).ToString('X') = '12345678');
end.