// Сборка мусора: для освобождения объекта присвойте переменной nil
uses System.Collections.Generic;

var l: List<integer> := new List<integer>;

begin
  l.Add(3);
  l.Add(5);
  l.Add(2);
  foreach x:integer in l do
    write(x,' ');
  l := nil; // после этого память, занимаемая динамическим массивом, будет собрана сборщиком мусора 
end.