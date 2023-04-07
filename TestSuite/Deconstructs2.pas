var i: integer;
type
  t1 = class
    procedure Deconstruct;
    begin
      i := 1;
    end;
  end;
  
begin
  if new t1 is t1(var a) then
    begin
      i := 2;
    end;
  assert(i = 2);
end.