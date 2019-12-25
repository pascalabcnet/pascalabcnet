uses OpenCLABC;

begin
  
  var q1 := HPQ(()->
  begin
    // lock надо чтоб при параллельном выполнении два потока не пытались использовать вывод одновременно. Иначе выйдет кашу
    lock output do Writeln('Очередь 1 начала выполняться');
    Sleep(500);
    lock output do Writeln('Очередь 1 закончила выполняться');
  end);
  var q2 := HPQ(()->
  begin
    lock output do Writeln('Очередь 2 начала выполняться');
    Sleep(500);
    lock output do Writeln('Очередь 2 закончила выполняться');
  end);
  
  Writeln('Последовательное выполнение:');
  Context.Default.SyncInvoke( q1 + q2 );
  
  Writeln;
  Writeln('Параллельное выполнение:');
  Context.Default.SyncInvoke( q1 * q2 );
  
end.