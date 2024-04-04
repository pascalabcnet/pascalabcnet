var i: integer;
type
  t1<T> = class
    
    private static function init_x: integer;
    begin
      Inc(i);
    end;
    private static x := init_x;
   
  end;
  

begin
  new t1<byte>;
  assert(i = 1);
  assert(typeof(t1<byte>).GetConstructors(
    System.Reflection.BindingFlags.Static or
    System.Reflection.BindingFlags.NonPublic
  ).Length = 1);
end.