type
  t1<T>=class
    static p: Action<T>;
  end;

procedure p0<T>;
begin
  var p1: Action<T>;
  p1 := o1->t1&<T>.p(o1);//Ошибка: В данной версии компилятора не поддерживается замыкание данного типа символов
  p1(default(T));
end;

var i: integer;

begin 
  t1&<integer>.p := x -> begin i := 1; end;
  p0&<integer>;
  assert(i = 1);
end.