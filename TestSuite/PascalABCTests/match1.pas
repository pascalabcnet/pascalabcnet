begin
  match new string('d',1) with
  //Ошибка времени выполнения: Индекс находился вне границ массива.
  ['d']: Assert(1=1);
  else Assert(1=2);
  end;
end.