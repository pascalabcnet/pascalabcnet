type TClass = class
a : integer := 4;

class function getA : integer;
begin
end;

class property Prop1 : integer read a write a;
end;

var t : TClass;
begin

end.