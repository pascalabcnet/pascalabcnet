uses OpenCLABC;

// Вывод типа и значения объекта
// "o?.GetType" это короткая форма "o=nil ? nil : o.GetType", то есть берём или тип объекта, или nil если сам объект nil
// _ObjectToString это функция, которую использует Writeln для форматирования значений
procedure OtpObject(o: object) :=
Writeln( $'{o?.GetType}[{_ObjectToString(o)}]' );

begin
  var b0 := new Buffer(1);
  
  OtpObject(  Context.Default.SyncInvoke( b0.NewQueue as CommandQueue<Buffer>   )  ); // Тип - буфер, потому что очередь создали из буфера
  OtpObject(  Context.Default.SyncInvoke( HFQ( ()->5                          ) )  ); // Тип - Int32(integer), потому что это тип по-умолчанию для выражения "5"
  OtpObject(  Context.Default.SyncInvoke( HFQ( ()->'abc'                      ) )  ); // Тип - string, по той же причине
  OtpObject(  Context.Default.SyncInvoke( HPQ( ()->Writeln('Выполнилась HPQ') ) )  ); // Тип отсутствует, потому что HPQ возвращает nil
  
end.