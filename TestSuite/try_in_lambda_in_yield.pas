function f1: sequence of byte; //Ошибка: Функции с yield не могут содержать блоков try..except и try..finally
begin
  yield 0;
  
  var p: procedure := ()->
  begin
    try
    except
    end
  end;
  
end;

begin 
end.