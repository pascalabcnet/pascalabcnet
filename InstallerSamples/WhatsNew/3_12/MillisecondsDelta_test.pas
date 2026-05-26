// Реализация Milliseconds и MillisecondsDelta через StopWatch - более стабильная
// Ожидается вывод без скачков значений
begin
  Println('MillisecondsDelta test:');
  MillisecondsDelta;
  for var i := 1 to 200 do
  begin
    var sum := 0;
    for var k := 1 to 3000000 do 
      sum += k;
    
    Print(MillisecondsDelta);
  end;
end.