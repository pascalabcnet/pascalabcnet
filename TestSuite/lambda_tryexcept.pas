begin
  
  var p: procedure := ()->
  begin end;
  
  try
  except
    on e: System.ArgumentException do;
    on e: Exception do; // Ошибка: Повторно объявленный идентификатор e
  end;
  Assert(1=1)
end.