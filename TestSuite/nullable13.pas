var o: object;

procedure p0<T>(f: () -> T);
begin
  o := f();
end;

begin
  p0(()->
  begin
    Result := default(integer?);
    //Ошибка: Нельзя преобразовать тип integer к Nullable<integer>
    Result := 5;
  end);
  assert(System.Nullable&<integer>(o) = System.Nullable&<integer>(5));
end.