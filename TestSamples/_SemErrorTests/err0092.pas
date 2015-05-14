type TClass = class
function Test(i : integer): integer; abstract;
end;

TClass2 = class(TClass)
end;

var t : TClass;

begin
end.