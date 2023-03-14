var i: integer;
type
  Base =  class end;
  
  r1 = record
    procedure pr1<T>(par1: T); where T: Base;
    begin 
      i := 1;
    end;
    
  end;
  
begin 
  var r: r1;
  r.pr1(new Base);
  assert(i = 1);
end.