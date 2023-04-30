var i: integer;
type
  t1<T> = class
    
    private static function init_x: integer;
    begin
      Inc(i);
    end;
    private static x := init_x;
    private static y := init_x;
    
    static constructor;
    
  end;
  
static constructor t1<T>.Create;
begin
  Inc(i);
end;

begin
  new t1<byte>;
  assert(i = 3);
  assert(typeof(t1<byte>).GetConstructors(
    System.Reflection.BindingFlags.Static or
    System.Reflection.BindingFlags.NonPublic
  ).Length = 1);
end.