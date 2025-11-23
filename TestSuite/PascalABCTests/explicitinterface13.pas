type
  i1 = interface
    procedure p1;
  end;
  //Ошибка: Функция класса c1, реализующая метод p1 интерфейса i1, должна быть нестатической с уровнем доступа public
  c1 = class(i1)
    procedure i1.p1 := exit;
  end;
  
begin 
  var obj := new c1;
end.