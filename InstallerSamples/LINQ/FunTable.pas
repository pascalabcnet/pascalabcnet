// Вывод таблицы значений функции sin

begin
  Range(0,Pi,20).Select(x->Format('({0:f4}, {1:f7})',x,sin(x))).Println(NewLine);
end.

