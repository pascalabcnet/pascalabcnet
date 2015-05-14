// Откомпилировать в Delphi
library NativeDll;

function add(a,b: integer): integer; stdcall;
begin
  Result := a+b;
end;

exports
  add;
begin
end.
 