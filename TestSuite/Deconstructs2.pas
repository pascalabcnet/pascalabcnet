var i: integer;
type
  t1 = class
    procedure Deconstruct;
    begin
      i := 1;
    end;
  end;
  
begin
  var o := new t1;
  if o is t1(var a) then
    begin
      i := 2;
      assert(o = a);
    end;
  assert(i = 2);
end.