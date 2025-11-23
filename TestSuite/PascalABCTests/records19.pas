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
  
function f1 := default(r1);

begin
  var a := f1;
  assert(i = 1);
end.