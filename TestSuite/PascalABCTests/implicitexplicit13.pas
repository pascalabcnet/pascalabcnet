var i: integer;
type
  
  TBase = class end;
  
  TErr<T> = class(TBase)
    
    public static function operator implicit(o: T): TErr<T>;
    begin
      i := 1;
    end;
  
  end;
  
  t1 = class(TErr<byte>) end;
  
// обязательно шаблонная подпрограмма и использование T ниже. Иначе не воспроизводится
procedure p1<T>;
begin
  
  var a: TBase := new t1;
  var b := TErr&<T>( a ); // тут вместо приведения типа вызывает operator implicit, который тут ну никак не подходит
  // если удалить operator implicit - на этой строчке оказывается обычное приведение типа, как и должно
  i := 2;
end;

begin
  p1&<byte>;
  assert(i = 2);
end.