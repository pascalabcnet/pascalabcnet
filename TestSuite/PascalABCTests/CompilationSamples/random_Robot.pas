 // Демонстрация использования случайных чисел для Робота. Случайное блуждание
uses Robot;

begin
  Field(50,40);
  while True do
  begin
    var r := Random(4);
    case r of
      0: if FreeFromUp then Up;
      1: if FreeFromDown then Down;
      2: if FreeFromLeft then Left;
      3: if FreeFromRight then Right;
    end;
    Paint;
  end;
end.
