begin
  var i := 0;
  //Ошибка: Неизвестное имя 'procedure()'
  var d: procedure(p: procedure);
  // Если убрать присваивание, ошибка выше пропадает
  d := p ->
  begin
    p;
  end;
  d(() -> begin Inc(i) end);
  assert(i = 1);
end.