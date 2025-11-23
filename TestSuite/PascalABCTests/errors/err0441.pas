//!Ожидалась функция, встречена процедура p
{$reference WindowsBase.dll}
begin
var p: byte->();
System.Windows.Threading.Dispatcher(nil).Invoke(()->p(0));

end.
