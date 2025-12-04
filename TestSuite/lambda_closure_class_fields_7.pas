type
  t0 = class
    public static a1: byte := 2;
    
    public static function f1: integer;
    begin
      Result := 1
    end;
  end;
  t1 = class(t0)
    private procedure p1;
    begin
      var a2: word := 3;
      var pp1: ()->integer := ()->a1+a2+f1;
      Assert(pp1=6);
    end;
  end;

begin 
  t1.Create.p1
end.