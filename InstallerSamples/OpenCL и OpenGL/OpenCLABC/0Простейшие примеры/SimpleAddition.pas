uses OpenCLABC;

begin
  
  // Чтение и компиляция .cl файла
  
  var prog := new ProgramCode(ReadAllText('SimpleAddition.cl'));
  
  // Подготовка параметров
  
  var A := new Buffer( 10 * sizeof(integer) ); // Буфер достаточно размера, чтоб хранить 10 элементов типа "integer"
  
  // Выполнение
  
  prog['TEST'].Exec1(10, // используем 10 ядер
    
    A.NewQueue.AddFillValue(1) // заполняем весь буфер значениями (1), прямо перед выполнением. Тип значения integer, потому что это тип по-умолчанию для целых чисел
    as CommandQueue<Buffer> //ToDo нужно только из за issue компилятора #1981, иначе получаем странную ошибку. Когда исправят - можно будет убрать
    
  );
  
  // Чтение и вывод результата
  
  A.GetArray1&<integer>.Println; // читаем всё содержимое как одномерный массив с элементами типа "integer"
  
end.