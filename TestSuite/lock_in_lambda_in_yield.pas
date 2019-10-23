function f1: sequence of byte; //Ошибка: Функции с yield не могут содержать блоков try..except и try..finally
begin
  yield 0;
  
  var p: procedure := ()->
  begin
    lock(new object) do
    begin
      
    end;
  end;
  
end;

begin 
end.