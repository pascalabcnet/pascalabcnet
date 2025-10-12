// Стандартные функции Ord, Chr, OrdAnsi, ChrAnsi

begin
  Println('sizeof(char) =',sizeof(char));
  Println;
  var c: char  := 'Ж';
  var i: integer := Ord(c);
  Println($'Код символа {c} в кодировке Unicode равен {i}');
  c := Chr(i);
  Println($'Символ с кодом {i} в кодировке Unicode - это {c}');
  Println;
  i := OrdAnsi(c);
  Println($'Код символа {c} в кодировке Windows равен {i}');
  c := ChrAnsi(i);
  Println($'Символ с кодом {i} в кодировке Windows - это {c}');
end.