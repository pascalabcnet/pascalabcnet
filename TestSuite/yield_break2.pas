function f1: sequence of byte;
begin
  while true do
  begin
    yield 1;
    break;
  end;
end;

begin 
  Assert(f1.First=1);
end.