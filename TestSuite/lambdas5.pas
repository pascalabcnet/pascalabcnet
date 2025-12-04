type
  t1=class
    
    function f1(aX,aY:Single):=byte(2);
    
    constructor;
    begin
      
      var MAng := 0;
      var NP2 := new byte[1];
      NP2.Fill(i->self.f1(i,MAng));
      assert(NP2[0] = 2);
    end;
    
  end;

begin 
  new t1;
end.