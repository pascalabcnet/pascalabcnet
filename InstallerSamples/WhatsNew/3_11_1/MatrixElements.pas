// 3.11.1. Метод Elements для матрицы - элементы по строкам
begin
  var m := MatrRandomInteger(3,4,1..10);
  m.Println;
  m.Elements.Println;
  Println(m.Elements.Min,m.Elements.Max,m.Elements.Sum);
end.