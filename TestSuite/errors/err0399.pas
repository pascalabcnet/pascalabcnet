type
  t1 = class
    static function operator implicit<T>(o: T): t1;
    where T: record;
    begin end;
  end;
  
  t2 = class end;
  
procedure p1(q: t1) := exit;

begin
  p1(new t2);//Здесь теперь правильная ошибка: Нельзя преобразовать тип t2 к t1
end.