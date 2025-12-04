type
  t1 = class;
  
  t1 = class
    function f1: sequence of byte;
    begin
      yield 0;//обязательно yield
      yield 1;
    end;
  end;

begin 
  var o1:t1 := new t1;
  var arr := o1.f1.ToArray;
  assert(arr[1] = 1);
end.