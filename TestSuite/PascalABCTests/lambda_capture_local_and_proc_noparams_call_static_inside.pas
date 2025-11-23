type
  t1<T> = class
    public static procedure p0 := Print(111);
    public static i: integer := 2;
    
    public static function p1: integer;
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
  t1&<integer>.p1;
end.