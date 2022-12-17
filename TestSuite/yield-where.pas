//#2760
type
  t0<T,TT> = class
    where T: class;
    where TT: record;
  end;
  
  t1<T> = class
  where T: class;
    // Обязательно метод
    function f1<TT>(o: T): sequence of byte;
    where TT: record;
    begin
      //Ошибка: Невозможно инстанцировать, так как тип T не наследован от System.Exception
      new t0<T,TT>;
      // Обязательно yield
      yield 5;
    end;
  end;
  
begin
  Assert(t1.Create.f1.Single=5);
end.