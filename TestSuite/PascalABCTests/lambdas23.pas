var j := 0;
type
  t1 = class
    
    static property prop1: integer read 1;
    
    static constructor;
    begin
      var p := ()->
      begin
        j := prop1;
      end;
      p;
    end;
    
  end;
  
begin 
  var i := t1.prop1;
  assert(j = 1);
end.