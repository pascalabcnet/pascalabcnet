type
  t1 = class
    
    static a: integer := 1;//не обязательно статичное поле
    
    procedure p1;
    begin
      var a := 2;
      var p: procedure := ()->begin
        
        Assert(a=2);
        
      end;
      
      p;
    end;
  
  end;

begin
  
  (new t1).p1;
  
end.