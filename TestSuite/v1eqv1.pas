begin
  var v1 := 0;
  var v2 := 0;
  
  var p: procedure := ()->
  begin
    var v1 := v1+1; // Ошибка: Неизвестное имя 'v1'
    v2 += 1; // если убрать эту строчку - ошибки нет
    Assert(v1=1);
  end;
  
  p;
  
end.