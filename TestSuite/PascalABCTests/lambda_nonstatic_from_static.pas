type
  t1 = class
    
    b: byte;
    
    static procedure p1(b: byte);
    begin
      
      var p: procedure;
      p := ()->
      begin
        p := ()->
        begin
          b := b;
        end;
      end;
      
    end;
    
  end;

begin 
  Assert(1=1);
end.