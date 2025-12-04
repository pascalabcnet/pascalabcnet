type
  t1 = class
    
    // Обязательно оператор
    static function operator+(p1, p2: t1): t1;
    begin
      
    end;
    
    // Обязательно yield-функция
    // Не важно, статическая или экземплярная
    function f1: sequence of byte;
    begin
      yield 1;
    end;
    
  end;
  
begin 
  var o := new t1;
  assert(o.f1.ToArray[0] = 1);
end.