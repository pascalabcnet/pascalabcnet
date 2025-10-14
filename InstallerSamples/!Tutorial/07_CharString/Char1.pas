// Стандартные функции Ord и Chr

begin
  var c := 'z';
  var i := Ord(c);
  Println($'Код символа {c} в кодировке Unicode равен {i}');
  i := 1025;
  c := Chr(i);
  Println($'Символ с кодом {i} в кодировке Unicode - это {c}');
end.
