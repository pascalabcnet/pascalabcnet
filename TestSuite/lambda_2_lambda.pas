type
  t1 = class
    pf: procedure;
    
    procedure p1;
    begin
      var obj: byte;
      
      var lp2: procedure := ()->
      begin
        obj := obj;
        pf := ()->
        begin
          Print(1);
        end;
        pf;
      end;
      lp2;
    end;
  
  end;

begin 
  t1.Create.p1;
end.