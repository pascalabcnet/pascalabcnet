type
  t1=record
    b:byte := 5;
    function f1 := b;
    constructor(b:byte) := self.b := b;
  end;

begin
  var b := (new t1(5)).f1;
  assert(b = 5)//должно выводить 5, но выводит мусор, там такое же преобразование как в #901
end.