type
  T = class
    public static function f: T := new T; //Обязательно static
    
    public procedure P;
    begin
      var s := f; // Повторно объявленный идентификатор s
                  // Если добавить (), то всё работает
      
      var lambda: () -> integer = () -> 1; //любая лямбда
    end;
  end;

begin
  
end.