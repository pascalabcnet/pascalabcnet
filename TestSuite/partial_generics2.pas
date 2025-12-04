var i: integer;

procedure p0(x: byte);
begin
  i := x;
end;

type
  t1 = partial class end;
  t2 = partial class(t1) end;
  
  t0<T> = class
    x: T;
  end;
  t1 = partial class(t0<byte>) end;
  
  t2 = partial class//(t1) // Если указать наследование от t1 - ошибки нет
    
    //Ошибка: Нельзя преобразовать тип T к byte
    procedure p1 := p0(x);
    
  end;
  
begin 
  var o := new t2;
  o.x := 2;
  o.p1;
  assert(i = 2);
end.