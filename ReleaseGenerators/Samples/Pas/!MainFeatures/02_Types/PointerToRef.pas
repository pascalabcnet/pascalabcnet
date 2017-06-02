// Указатели на ссылочные типы запрещены. Исключение: указатели на строки и динамические массивы
type
  A = class
    i: integer;
  end;

var p: ^record field: A; end;

begin
end.