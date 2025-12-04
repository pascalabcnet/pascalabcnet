var i: integer;
type
  
  TBase = class
    
    // Эта FErr должна быть шаблонная и принимать функцию возвращающую шаблонный параметр (T)
    procedure PErr<T>(f: object->T) := exit;
    
  end;
  
  TErr<T> = class(TBase)
    
    // А у этой PErr не важно какое возвращаемое значение f
    procedure PErr(f: T->byte);
    begin
      f(default(T));
      i := 1;
    end;
    
    procedure p1;
    begin
      
      // Обязательно "self.", без него не воспроизводится
      self.PErr(o->
      begin
        var p: T->() := procedure(x)->exit;
        
        // Обязательно передача в любую процедуру / функцию (не обязательно процедурную переменную)
        // При присвоении переменной не воспроизводится
        p(o); //Ошибка: Нельзя преобразовать тип object к T
        
        Result := 0;
      end);
      
    end;
    
  end;
  
begin 
var o := new TErr<integer>;
o.p1;
assert(i = 1);
end.