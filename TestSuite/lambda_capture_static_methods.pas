type
  T = class
    public static procedure p1(a: integer) := exit;
    
    public static procedure P := &Array.ForEach(Arr(0), x -> p1(x)); // В данной версии компилятора не поддерживается замыкание данного типа символов
  end;

begin
  Assert(1=1)
end.