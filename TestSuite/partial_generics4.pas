
var i: integer;

type
  t0<T> = class end;
function f0<T>(q: t0<T>): T;
begin
  Result := default(T);
  i := 1;
end;


type
  t1<T> = partial class end;
  t2<T> = partial class(t0<t1<T>>) end;
  
  t1<T> = partial class
    // Обязательно как угодно использовать t2<T>, имено с <T>
    v0: t2<T>;
  end;
  t2<T> = partial class(t0<t1<T>>) end;
  
  t1<T> = partial class
    //Ошибка: Нельзя преобразовать тип t1<T> к t1<T>
    function f1: t1<T> := f0(default(t2<T>));
  end;
  
begin
  var o := new t1<integer>;
  var o2 := o.f1;
  assert(o2 = nil);
  assert(i = 1);
end.