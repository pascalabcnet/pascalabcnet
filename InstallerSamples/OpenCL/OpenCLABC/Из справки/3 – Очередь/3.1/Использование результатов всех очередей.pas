uses OpenCLABC;

begin
  
  var q1 := HFQ( ()->1 );
  var q2 := HFQ( ()->2 );
  
  // Выводит 2, то есть только результат последней очереди
  // Так сделано из за вопросов производительности
  Context.Default.SyncInvoke( q1+q2 ).Println;
  // Однако всё же бывает так, что нужны результаты всех сложенных/умноженных очередей
  
  // В таком случае надо использовать CombineSyncQueue и CombineAsyncQueue
  // А точнее их перегрузку, первый параметр которой - функция преобразования
  Context.Default.SyncInvoke(
    CombineSyncQueue(
      results->results.JoinIntoString, // функция преобразования. Если не указывать - CombineSyncQueue работает как обычное сложение (но быстрее если складывать >2 очередей)
      q1, q2
    )
  ).Println;
  // Теперь выводит строку "1 2". Это то же самое что вернёт "Arr(1,2).JoinIntoString"
  
end.