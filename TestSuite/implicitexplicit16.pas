type
  t1 = class
    
    public static function operator implicit<T>(a: array of T): t1 := nil;
    
  end;
  
procedure p1<T>;
begin
  //Ошибка: Нельзя преобразовать тип array of T к t1
  var o: t1 := new T[5];
  assert(o = nil);
end;

begin
  //Ошибка: Нельзя преобразовать тип array of byte к t1
  var o: t1 := new byte[5];
  assert(o = nil);
  p1&<integer>;
end.