// Dll-библиотека
library MyDll;

const n = 10;

function add(a,b: integer): integer;
begin
  Result := a + b;
end;

procedure PrintPascalABCNET;
begin
  writeln('PascalABC.NET');
end;

end.