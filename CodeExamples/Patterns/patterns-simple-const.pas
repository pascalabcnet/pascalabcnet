begin
  var a := 3.14;
  match a with
    1.0, 2.0, 3.0: Println(1); // Надо исправить: double не матчит с integer - несоответствие типов.
    3.13, 3.14, 3.15, 3.16: print(2);
    123.0: print(3);
  end;
end.