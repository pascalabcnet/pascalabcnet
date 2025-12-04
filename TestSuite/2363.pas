procedure p0<T>(o: T);
begin
  //Print(1);
end;

procedure p1<T>; where T: record;
begin
  //Ошибка: Возможны два преобразования типов
  p0&<T?>(nil);
end;

begin 
  Assert(1=1)
end.