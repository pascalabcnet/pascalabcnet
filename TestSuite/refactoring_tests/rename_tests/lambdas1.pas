type
  t1 = class
    
    public class {@}i: integer;
    
    procedure p1;
    begin
      var i2: integer;
      var p: procedure := () -> begin
        
        {!}i += 1;
        i2 += 1;
      end;
      p;
    end;
  
  end;

begin

end.