type
  t2 = class
    public static sv := 0;
  end;
  
  t1 = class
    
    public static sv := 0;
    public sv2: integer;
    
    public static procedure p1;
    begin
      var v := 0;
      var p: ()->() := ()->begin
        
        v := v;
        sv := 5;
        t2.sv := 5;
      end;
      p;
    end;
    
    public procedure p2;
    begin
      var v := 0;
      var p: ()->() := ()->begin
        v := v;
        sv := 6;
        t2.sv := 6;
        sv2 := 2;
      end;
      p;
    end;
  end;

begin 
  t1.p1;
  assert(t1.sv = 5);
  assert(t2.sv = 5);
  var o := new t1;
  o.p2;
  assert(t1.sv = 6);
  assert(t2.sv = 6);
  assert(o.sv2 = 2);
end.