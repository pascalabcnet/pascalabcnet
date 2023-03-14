type
  t1 = class
    
    procedure p1;
    begin
      // Если описать так - не воспроизводится
//      var p := procedure->exit();
      var p: procedure;
      p := ()->exit();
      p();
    end;
    
    procedure p2;
    // Обязательно вложенный тип
    type r = record end;
    begin
    end;
    
  end;
  
begin 
  var o := new t1;
  o.p1;
end.