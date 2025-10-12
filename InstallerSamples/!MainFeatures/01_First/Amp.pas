// Пример иллюстрирует использование знака "&" для снятия атрибута ключевого слова

begin
  var &begin,&end: integer;  

  &begin := 1;
  &end := 2;
  var t: System.Type := &begin.GetType; // в System.Type использовать & не надо
  write(&begin,' ',&end,' ',t);
end.