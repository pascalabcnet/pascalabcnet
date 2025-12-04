type
  t1<T> = class end;
  i1<T> = interface end;
  
  //Ошибка: Could not load type '0.t1' from assembly '0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'.
  t2<T> = record(i1<t1<T>>)
  end;
  t3 = record(i1<t1<integer>>)
  end;
begin
  var r: t2<integer>;
  var r2: t3;
end.