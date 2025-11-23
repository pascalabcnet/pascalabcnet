uses u_override;

var i: integer;
type
  
  t1 = class(tBase1)
    
    public procedure p2; override := exit;
    
  end;
  
  t2 = sealed class(tBase2)
    
    public procedure p1; override;
    begin
      i := 1;
    end;
    
  end;

begin
  var o := new t2;
  o.p1;
  assert(i = 1);
end.