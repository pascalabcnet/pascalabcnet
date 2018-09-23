type
  t1 = class end;
  t2 = class
    function f1: sequence of t1;//должен быть какой то класс, sequence of object не вызывает ошибку
    begin
      yield nil;
    end;
  end;

begin 
  var o := new t2;
  var o2 := o.f1.ToArray()[0];
  assert(o2 = nil);
end.