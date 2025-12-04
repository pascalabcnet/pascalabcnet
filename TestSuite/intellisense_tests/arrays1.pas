
uses ABCDatabases;
begin
  var pupils := ЗаполнитьМассивУчеников;
  var s := pupils[0].Фамилия{@property Ученик.Фамилия: string; readonly;@};
end.