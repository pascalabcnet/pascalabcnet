type IInt = class
procedure Test;abstract;
end;

TClass = class(IInt)
procedure Test;override;
begin
inherited Test();
end;

end;

var t : TClass;

begin
t := new TClass;
t.Test;
end.