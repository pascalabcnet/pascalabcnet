//#2760
type
  t0<T,TT> = class
    where T: class;
    where TT: record;
  end;
  
  t1<T> = class
  where T: class;
    // Обязательно метод
    function f1<TT>(o: TT): sequence of byte;
    where TT: record;
    begin
      //Ошибка: Невозможно инстанцировать, так как тип T не наследован от System.Exception
      new t0<T,TT>;
      // Обязательно yield
      yield 5;
    end;
  end;
  
begin
  Assert(t1&<object>.Create.f1(0).Single = 5);
end.