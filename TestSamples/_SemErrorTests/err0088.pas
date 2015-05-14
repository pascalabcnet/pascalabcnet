type TClass = class
procedure Test; abstract;
end;

var t : TClass;

begin
t := TClass.Create;
end.