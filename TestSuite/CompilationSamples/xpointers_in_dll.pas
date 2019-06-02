{$reference lib.dll}

procedure p1;
begin
  var r: lib.r1; // запись обязательно из библиотеки
  var p: ^char;
  r.p := p; // обязательно присвоить полю значение переменной. если присваивать nil - не воспроизводится
end;

begin end.