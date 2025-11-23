type TClass = class
FProp : integer;
property Prop : integer read FProp write FProp; 
end;

var a : TClass;

begin
Inc(a.Prop);
end.