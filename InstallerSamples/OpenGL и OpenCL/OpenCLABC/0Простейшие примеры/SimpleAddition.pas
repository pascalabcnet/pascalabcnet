uses OpenCLABC;

begin
  
  // Чтение и компиляция .cl файла
  
  var prog := new ProgramCode(Context.Default, ReadAllText('SimpleAddition.cl'));
  
  // Подготовка параметров
  
  var A := new Buffer( 10 * sizeof(integer) ); // Буфер достаточного размера, чтобы хранить 10 элементов типа "integer"
  
  // Выполнение
  
  prog['TEST'].Exec1(10, // используем 10 ядер
    
    // Заполняем весь буфер значениями (1), прямо перед выполнением
    // Тип значения integer, потому что это тип по-умолчанию для целых чисел
    A.NewQueue.AddFillValue(1)
    
  );
  
  // Чтение и вывод результата
  
  A.GetArray1&<integer>.Println; // Читаем всё содержимое как одномерный массив с элементами типа "integer"
  
end.