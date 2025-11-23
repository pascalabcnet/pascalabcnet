unit u_property3;

type
  t1 = class
    public i: integer;
    procedure p1(ind: integer);
    
    // Обязательно default индексное свойство
    // Обязательно упомянуть p1, реализованное после объявления t2
    // Не важно как упомянуть
    public property Item[ind: byte]: word write p1(ind); default;
    
  end;
  
  // Обязательно шаблон
  // Обязательно наследовать от t1
  t2<T> = class(t1) end;
  
procedure t1.p1(ind: integer);
begin
  // Обязательно упомянуть t2
  var a: t2<real>;
  self.i := ind;
end;

end.