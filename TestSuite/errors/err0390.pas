type
  t0 = abstract class end;
  
  t1<T> = class
  where T: t0;
    
    procedure p1(x: byte);
    begin
      var p: Action0 := ()->
      begin
        
       
        Writeln(x);

        Writeln(new T);
        
      end;
      p();
    end;
    
  end;
  
begin end.