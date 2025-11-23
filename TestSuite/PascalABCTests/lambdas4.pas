type
  t1=class
    
    function f1:byte:=2;
    
    constructor;
    begin
      var a:= new byte[1];
      a.Fill(i->f1);
      assert(a[0] = 2);
    end;
    
  end;

begin 
  new t1;
end.