unit extensionmethods10u;
procedure MyForEach<T>(self: sequence of T; action: T -> ()); extensionmethod;
begin
  foreach x: T in Self do
    action(x);
end;
end.