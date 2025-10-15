// SetRandomSeed - синоним Randomize с более понятным названием

begin
  loop 10 do
    Print(Random(100));
  Println;
  Println;

  SetRandomSeed(122);
  loop 10 do
    Print(Random(100));
  Println;
  // Вторая последовательность с тем же самым seed совпадает с первой. Нужно для тестирования
  SetRandomSeed(122);
  loop 10 do
    Print(Random(100));
end.