var a := 666;

procedure p;
begin
  var b := (a = 1) or (a = 3) or ('abc' is string(var a)) and (a = 'abc');
  Assert(b);
end;

begin 
  p; 
end.