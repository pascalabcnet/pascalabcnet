var i: integer;
type
  Base =  class end;
  Base2 =  class end;
  r1 = record
    procedure pr1<T, T2>(par1: T; par2: T2); where T: Base; where T2: Base2;
    begin 
      i := 1;
    end;
    
  end;
  
begin 
  var r: r1;
  r.pr1(new Base, new Base2);
  assert(i = 1);
end.