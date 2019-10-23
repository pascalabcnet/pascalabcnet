function MyStep(Self: real; mystep: real): sequence of real; extensionmethod;
begin
  yield mystep;
end;

function YourStep(yourstep: real): sequence of real;
begin
  yield yourstep;
end;


begin
  var a := 0.0.Step(0.3);
end.