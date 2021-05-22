type
  t1 = static class
    static event ev1: ()->();
    
    static procedure p1;
    begin
      var p := procedure->
      begin
        
        ev1();
        
      end;
      p;
    end;
    
  end;

begin 
  var i := 0;
  t1.ev1 += procedure() -> begin  
    Inc(i);
  end;
  t1.p1;
  assert(i = 1);
end.