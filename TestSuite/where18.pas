// #2519
type
  Base = class end;
  Def = class(Base) end;
  
  t0<T> = class
  where T: Base;
    
  end;
  //Ошибка: Несоответствие спецификатора where спецификатору where базового класса
  t1<T2> = class(t0<T2>)
  where T2: Def;
    
  end;
  
begin end.