uses u_interface5;

type
  
  t1 = class(I)
    
    public function I.f1: byte := 1;
    
  end;
  
begin 
  var o: I;
  o := new t1;
  assert(o.f1 = 1);
end.