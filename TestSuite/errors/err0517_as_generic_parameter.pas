//!Операция as не может быть применена к обобщённому параметру T, не имеющему спецификатора "class"
type
  A<T> = class
    public static procedure Test(x: T) := Print(x as T);
  end;

begin
  A&<integer>.Test(2); // выводит мусор
end.
