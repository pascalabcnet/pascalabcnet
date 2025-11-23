type
  t1 = class
    public procedure p0  := exit;
    public i: integer := 2;
    
    public procedure p1;
    begin
      var v := 0;
      var p: procedure := ()->begin
        v := v; 
        Assert(i=2); 
        p0; 
      end;
      p;
    end;
  end;

begin 
  t1.Create.p1;
end.