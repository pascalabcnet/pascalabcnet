type IInt = interface
end;

var a : integer;
    b : IInt;
    
begin
  b := a as IInt;
end.