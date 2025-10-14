// Стандартные функции OrdWindows ChrWindows

begin
  var c := 'Д';
  var i := OrdWindows(c);
  Println($'Код символа {c} в кодировке Windows равен {i}');
  i := 196;
  c := ChrWindows(i);
  Println($'Символ с кодом {i} в кодировке Windows - это {c}');
end.

