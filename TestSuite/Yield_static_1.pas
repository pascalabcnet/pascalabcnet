type
  T = static class
    static r: integer := 3;
    static property p: integer read 1;
    static function r1(i: integer): integer := 2;    
    public static function f(a: integer): sequence of integer;
    begin
      yield r + a + p +r1(1);
    end;
  end;
  
begin
  Assert(T.f(5).First=11); 
end.