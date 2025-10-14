// Цикл while. Степени 2
begin
  Println('Степени 2');
  var i := 1;
  var p := 1;
  while i < 32 do
  begin
    Println($'{i,2}{p,12}');
    i += 1;
    p *= 2;
  end;
end.
