
function BoolToString(Condition: boolean): string;
begin
  Result := Condition ? 'Yes' : 'No';
end;

function BoolToString1(Condition: boolean): string;
begin
  if Condition then 
    Result := 'Yes'
  else
    Result := 'No';
end;

begin
  Writeln(BoolToString(1>2));
  readln;
end.