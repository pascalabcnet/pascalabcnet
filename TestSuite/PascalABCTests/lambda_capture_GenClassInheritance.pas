type
  t0<T> = class end;
  
  t1 = class(t0<word>) 
    
    public procedure p1;
    begin
      var b: byte;
      
      var p: procedure := ()->
      begin
        b += 1; 
      end;
      
    end;
    
  end;

begin 
end.