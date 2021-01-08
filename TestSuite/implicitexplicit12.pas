var i: integer;
type
  t1 = class
    static function operator implicit<T>(o: T): t1;
    where T: record;
    begin 
      i := 1;
    end;
  end;
  
  t2<T> = class(t1) end;
  
procedure p1(q: t1) := exit;

begin
  //Ошибка: Невозможно инстанцировать, так как тип t2<byte> не является размерным
  p1(new t2<byte>);
  assert(i = 0);
end.