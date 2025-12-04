type
  I1 = interface
    property p1: byte read write;
  end;
  //Ошибка: Класс t1 не реализует метод get_p1:byte интерфейса I1
  t1 = class(I1)
    public b: byte;
    b2: byte;
    public property I1.p1: byte read b write b;
    public property p1: byte read b2;
  end;
  
begin 
  var o: I1 := new t1;
  o.p1 := 1;
  assert(o.p1 = 1);
  var o2 := o as t1;
  assert(o2.p1 = 0);
end.