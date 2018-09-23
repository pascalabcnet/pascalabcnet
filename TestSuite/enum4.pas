type
  [System.FlagsAttribute] 
  TEnum = (X=1, Y=2);

begin
  var enum := TEnum.X;
  assert(enum.HasFlag(TEnum.X) = true);
  assert(enum.HasFlag(TEnum.Y) = false);
end.