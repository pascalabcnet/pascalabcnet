function fff: sequence of byte;
begin
  for var b: byte := 0 to byte.MaxValue do
    yield b
end;

function fff1: sequence of byte;
begin
  for var b: byte := byte.MaxValue downto 0 do
    yield b
end;

function fff2: sequence of boolean;
begin
  for var b := False to True do
    yield b
end;

begin
  Assert(fff.SequenceEqual(fff1.Reverse));
  Assert(fff2.SequenceEqual(Seq(False,True)));
end.