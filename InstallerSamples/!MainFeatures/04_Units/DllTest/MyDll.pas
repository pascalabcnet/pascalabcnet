// Dll-библиотека
library MyDll;

const n = 10;

function add(a,b: integer): integer;
begin
  Result := a + b;
end;

procedure PrintPascalABCNET;
begin
  Println('PascalABC.NET');
end;

end.