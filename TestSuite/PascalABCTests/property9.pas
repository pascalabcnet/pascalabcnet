type
  t1 = abstract class
    
    private p1_val: boolean := false;
    property p1: boolean read p1_val; virtual;
    
  end;
  
  t2 = class(t1)
    
    property p1: boolean read boolean(true); reintroduce;
    
  end;
  
begin
  var a := new t2;
  assert(a.p1);
  assert(not (a as t1).p1);
end.