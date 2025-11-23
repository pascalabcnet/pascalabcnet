type
  // Обязательно предыдущее определение всех 3 классов
  t1<T> = partial class end;
  t2<T> = partial class(t1<T>) end;
  t3<T> = partial class(t2<T>) end;
  
  t1<T> = partial class
    // Обязательно возвращать или T из шаблона, или тип содержащий T, как array of T
    function f1: T; virtual; begin end;
  end;
  t2<T> = partial class(t1<T>)
    
  end;
  t3<T> = partial class(t2<T>)
    //Ошибка: Нет метода для переопределения
    function f1: T; override := default(T);
  end;
  
begin 
  var o := new t3<integer>;
  assert(o.f1 = 0);
end.