// Выполнение задания cc17 для Робота
uses Robot;

begin
  Task('cc17');
  for var i:=6 downto 1 do
  begin
    for var j:=1 to i do
    begin
      for var k:=1 to j do
      begin
        Paint;
        Up;
      end;
      for var k:=1 to j do
        Down;
      Right;  
    end;
    Right;
  end;
  Left;
end.

