var i: integer;
type
  t1 = class
    procedure p1(x: integer);
    begin
      i := x
    end;
  end;
  
//Ошибка: Операция ':=' не применима к типу procedure(x: byte) 
function f1(a: t1) := a.p1;

begin 
  var a: t1 := new t1;
  f1(a)(2);
  assert(i = 2);
end.