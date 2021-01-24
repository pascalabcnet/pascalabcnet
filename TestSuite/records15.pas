var i: integer;

type
  r1 = record 
    public function InitI1:integer;
    begin
      Result := 1;
      Inc(i);
    end;
    
    public i1: integer := InitI1;
  end;

procedure test;
var a := new r1;
begin
  
end;
begin
  var a := new r1;
  assert(i = 1);
  test;
  assert(i = 2);
end.