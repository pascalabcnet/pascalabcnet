type
  // Обязательно шаблон
  //Ошибка: Данная конструкция не поддерживается текущей версией компилятора
  t1<T> = sealed class
    
    // Обязательно static
    static procedure p1 := exit;
    
    function f1: sequence of byte;
    begin
      // Если расскомментировать - не воспроизводится
//      t1&<T>.
      p1;
      // Обязательно yield
      yield 1;
    end;
    
  end;
  
begin 
  var t := new t1<integer>;
  var j := 0;
  foreach var i in t.f1 do
    begin
      j := i;
    end;
  assert(j = 1);
end.