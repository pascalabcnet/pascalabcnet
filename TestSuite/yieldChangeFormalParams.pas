function Step1(Self: integer): sequence of integer; extensionmethod;
begin
  while True do
  begin
    yield Self;
    Self += 1;
  end;
end;

begin
  Assert(5.Step1.Take(10).SequenceEqual(Range(5,14)));
  Assert(5.Step1.Take(10).SequenceEqual(Range(5,14)));
end.