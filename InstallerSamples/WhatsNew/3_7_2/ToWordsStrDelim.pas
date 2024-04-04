// Метод строк .ToWords со строкой, содержащей разделители
begin
  var s := '123.4, 6.6; 67';
  s.ToWords(' ,;').PrintLines  
end.

