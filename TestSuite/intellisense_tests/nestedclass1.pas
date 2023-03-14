type
  // Обязательно шаблонный класс
  t1<T> = class
    
    key_lookup: Dictionary<T, word>;
    // Обязательно возвращать вложенный тип (тут "Dictionary.KeyCollection")
    // Если возвращать "sequence of T" - не воспроизводится
    function GetAllKeys := key_lookup.Keys;
    
  end;
  
  // Обязательно наследовать от t1
  t2 = class(t1<byte>)
    function f1{@function t2.f1(): integer;@} := 5;
  end;
  
begin 
  var o: t2;
  
end.