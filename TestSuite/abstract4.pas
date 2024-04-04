type
  t1 = abstract class
    
    procedure p1(p: procedure); abstract;
    
  end;
  t2 = class(t1)
    
    procedure p1(p: procedure); override;
    begin
      p;
    end;
    
  end;
  
begin
  var o := new t2;
  var i := 0;
  o.p1(()->begin i:=1 end);
  assert(i = 1);
end.