type
  t1 = class
    v1: integer;
    const c1 = 5;
    
    function f1: sequence of byte;
    begin
      yield c1; 
    end;
    
  end;

begin 
  Assert(True);
end.