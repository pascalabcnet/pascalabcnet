type
  t0<T> = class end;
function f0<T>(q: t0<T>) := default(T);

type
  t1<T> = partial class end;
  t2<T> = partial class
    // Обязательно как то использовать t1<T>, именно с <T>
    o: t1<T>;
  end;
  
  t1<T> = partial class(t0<t2<T>>) end;
  t2<T> = partial class
    //Ошибка: Нельзя преобразовать тип t2<T> к t2<T>
    function f1: t2<T> := f0(default(t1<T>));
  end;
  
begin 
  var o := new t2<integer>;
  assert(o.f1() = nil);
end.