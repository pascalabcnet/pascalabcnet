type
  
  Fixer<TFixer> = class
   where TFixer: record;
    
    procedure p1;
    begin
      var a: TFixer;
      var b := a <> nil;
    end;
    
  end;
  
begin end.