type
  t1 = class
    {public} static a1: byte := 2;
    
    private procedure p1;
    begin
      var a2: word := 3;
      var pp1: ()->integer := ()->a1+a2;
      Assert(pp1=5);
    end;
  end;

begin 
  t1.Create.p1
end.