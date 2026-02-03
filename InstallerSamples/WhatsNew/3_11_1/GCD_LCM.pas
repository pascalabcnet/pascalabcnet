// 3.11.1. Стандартные функцции GCD, LCM
begin
  var (a,b) := (56,98);
  
  var GCD := GCD(a, b);
  var LCM := LCM(a, b);
  
  Println($'НОД({a}, {b}) = {GCD}');
  Println($'НОК({a}, {b}) = {LCM}');
end.