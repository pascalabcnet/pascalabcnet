// Пример иллюстрирует использование значка & для снятия атрибута ключевого слова
var 
  &begin,&end: integer;
begin
  &begin := 1;
  &end := 2;
  var t: System.Type := &begin.GetType; // в System.Type использовать & не надо
  write(&begin,' ',&end,' ',t);
end.