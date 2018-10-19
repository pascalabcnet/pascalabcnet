type
  t1 = class;//обязательно предописание
  
  t1 = class
    public function f1: sequence of integer;
  end;

function t1.f1: sequence of integer;//обязательно реализовать вне класса
begin
  yield 1;
end;

begin 
  var o := new t1;
  assert(o.f1().ToArray()[0] = 1);
end.