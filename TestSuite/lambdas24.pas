var s: string;
type
  // Обязательно разделить откуда берутся типы лямбды:
  // TInput должно браться из шаблона класса (GenericContainer)
  // TResult - из шаблона метода (TakeLambda)
  GenericContainer<TInput> = class
    
    // Не обязательно статический метод
    static procedure TakeLambda<TResult>(lambda: TInput->TResult);
    begin
      lambda(default(TInput));
    end;
    
  end;
  
procedure PErr<T>;
begin
  
  // Обязательно в качестве TInput указать не конкретный тип,
  // а использовать тип из шаблона PErr
  GenericContainer&<T>.TakeLambda(l->
  begin
    // Обязательно вычислять тип шаблона TakeLambda из тела лямбды
    // Если вызвать TakeLambda&<integer> - не воспроизводится
    Result := 0;
    // Обязательно присвоить чему то результат
    //Ошибка: ToString не объявлен в типе 
    var v := l.ToString;
    s := v;
  end);
  
end;

begin 
  PErr&<integer>;
  assert(s = '0');
end.