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
    
  end;
  
begin
  var a: r1 := (i1: 2; i2: 3);
  assert(i = 0);
end.