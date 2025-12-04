var i: integer;
type
  t1<T> = class
    procedure p0;
    begin
      i := 1;
    end;
  end;
  
  // Обязательно промежуточный класс между t1 и t3
  t2<T> = partial class(t1<T>) end;
  // Обязательно t3 НЕ шаблонный
  t3 = partial class(t2<byte>) 
  
  end;
  
  // Обязательно объявить t2 и t3 дважды
  t2<T> = partial class(t1<T>) end;
  t3 = partial class(t2<byte>)
    // Обязательно вызвать p0 из второго тела t3
    procedure p1 := self.p0;
  end;
  
begin
  t3.Create.p1;
  assert(i = 1);
end.