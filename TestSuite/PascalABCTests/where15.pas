uses System;

function ToEnum<T>(self: string): T; extensionmethod; where T: System.Enum;
begin
  Result := T(Enum.Parse(typeof(T), self));
end;

begin
  var color := 'Black'.ToEnum&<ConsoleColor>();
  assert(color = ConsoleColor.Black);
end.