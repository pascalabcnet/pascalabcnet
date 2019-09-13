uses OpenCLABC;

//ToDo Сейчас .AddWriteValue, принимающее очередь, вылетает
// - Это из за issue компилятора #2068
// - Когда её исправят - эта программа тоже станет нормально запускаться
// - А пока - смотрите на программу не запуская. Даже так - пример полезный

begin
  var N := ReadInteger('Введите размер буфера:');
  var b := new Buffer( N*sizeof(integer) );
  
  var Q_RNG_val := HFQ(()->
  begin
    Result := Random(1,100);
  end);
  
  // .WriteValue принимает значение размерного типа
  // Но вместо него - мы передали очередь (CommandQueue это класс, то есть точно не размерный тип)
  // Так можно, потому что возвращаемое значение очереди Q_RNG_val - размерный тип (integer)
  var Q_RNG_FillBuff := b.NewQueue.AddWriteValue(Q_RNG_val, 0) as CommandQueue<Buffer>;
  
  for var i := 1 to N-1 do // 0 пропускаем, потому что его уже добавили выше
    Q_RNG_FillBuff *= b.NewQueue.AddWriteValue(Q_RNG_val.Clone(), i*sizeof(integer) ) as CommandQueue<Buffer>;
  
  // Вообще, это далеко не самый эффективный и красивый способ заполнить буфер
  // В идеале - это надо делать карнелом
  // То есть написать свой алгоритм Random на языке OpenCL-C (нет, это не сложно)
  
  Context.Default.SyncInvoke(Q_RNG_FillBuff);
  
  b.GetArray1&<integer>(N).Println;
end.