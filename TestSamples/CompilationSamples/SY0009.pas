//Ошибка при первой компиляции сразу после запуска оболочки.
//Предположительно, ошибка в таблице символов.

type
  rec = record
    i: integer;
  end;
  
var
  t: System.Type;
  r: rec;
  
begin
  t := r.GetType;
end.