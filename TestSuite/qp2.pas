type
  t1 = auto class 
    i: integer;
  end;
  
begin
  
  var a: array of t1 := Arr(new t1(2));
  var o := a?.First;
  Assert((o as t1).i = 2)
end.