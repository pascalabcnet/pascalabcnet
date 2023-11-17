type
  r1 = record
    x: integer;
    constructor(x: integer) := self.x := x;
  end;
  
function f0 := new r1($12345678);

function f1(n: r1?) := (n??f0).Value;

begin
  // Выводит 0 вместо 12345678
  assert(f1(nil).x.ToString('X') = '12345678');
  var rec: r1;
  rec.x := 4;
  assert(f1(rec).x.ToString('X') = '4');
end.