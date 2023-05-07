var i: integer;
type
  t1 = class
    
    private static function init_x: integer;
    begin
      Inc(i);
    end;
    private static x := init_x;
    
    static constructor;
    
  end;
  
static constructor t1.Create;
begin
  Inc(i);
end;

begin
  new t1;
  assert(i = 2);
  assert(typeof(t1).GetConstructors(
    System.Reflection.BindingFlags.Static or
    System.Reflection.BindingFlags.NonPublic
  ).Length = 1);
end.