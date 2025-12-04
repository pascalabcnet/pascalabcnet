uses u_abstract3;

type
  
  // обязательно t3 - тип из другого модуля
  //Ошибка: Абстрактный класс не может иметь атрибут sealed
  t4 = sealed class(t3) end;
  
begin
  new t4;
end.