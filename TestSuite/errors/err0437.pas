//!Несколько подпрограмм p0 могут быть вызваны
procedure p0(o: pointer) := exit;
procedure p0(o: array of integer) := exit;

procedure p1(p: ()->()) := exit;

begin
  p1(()-> //Ошибка: Неверный тип переменной, которой присваивается лямбда-выражение
  begin
    
    // если закомментировать эту строчку - ошибки нет
    p0(nil);
    
  end);
end.