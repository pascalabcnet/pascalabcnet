var i: integer;
type
  r1 = record 
    public function InitI1:integer;
    begin
      Result := 1;
      Inc(i);
    end;
    
    public i1: integer := InitI1;
    public i2: integer;
    
    public constructor(i2:integer) :=
    self.i2 := i2;
    
    public static function operator implicit(i: integer): r1 := new r1(i);
    
  end;
  
begin
  var a: r1 := 5;
  assert(i = 2);
end.