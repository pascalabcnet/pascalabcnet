// Вывод таблицы значений функции sin

begin
  PartitionPoints(0,Pi,20).Select(x->Format('({0:f4}, {1:f7})',x,sin(x))).Println(NewLine);
end.

