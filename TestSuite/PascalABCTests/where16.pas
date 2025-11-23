type
  
  c1<T1> = class
  where T1: c1<T1>;
    
  end;
  
  //Ошибка: Несоответствие спецификатора whеre спецификатору where базового класса
  c2<T2> = class(c1<T2>)
  where T2: c1<T2>;
    
  end;
  
  c3 = class(c1<c3>)
  end;
  
begin
  new c2<c3>;
  new c3;
end.