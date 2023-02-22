begin
  var f: procedure(x: integer);
  var sum := 0;
  f := x -> begin sum += x end; // ранее
  f := x -> (sum += x); // сейчас
  // f := x -> sum += x; // ошибка компиляции
end.  
