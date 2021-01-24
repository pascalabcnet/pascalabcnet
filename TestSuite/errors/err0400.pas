type
  TBase = class end;
  
  TErr<T> = class(TBase)
    
    public static function operator implicit(o: T): TErr<T>;
    begin
      raise new System.InvalidOperationException;
    end;
    
  end;
  
procedure p0<T>(o: TErr<T>) := exit;

// обязательно вызывать p0 с шаблонным параметром p1
// если вызывать p0&<word> - выводит правильное сообщение об ошибке
procedure p1<T>(q: TBase) := p0&<T>(q); // это не должно компилироваться

begin
  var q: TBase;
  p1&<byte>(q);
end.