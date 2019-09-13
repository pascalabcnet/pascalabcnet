uses OpenCLABC;

begin
  
  // Чтение и компиляция .cl файла
  
  var prog := new ProgramCode(ReadAllText('SimpleAddition.cl'));
  
  // Подготовка параметров
  
  var A := new Buffer( 10 * sizeof(integer) ); // будет хранить 10 чисел типа "integer", то есть по 4 байта каждое 
  
  // Выполнение
  
  prog['TEST'].Exec1(10, // используем 10 ядер
    
    A.NewQueue.AddFillValue(1) // заполняем весь буфер единичками, прямо перед выполнением
    as CommandQueue<Buffer> //ToDo нужно только из за issue компилятора #1981, иначе получаем странную ошибку. Когда исправят - можно будет убрать
    
  );
  
  // Чтение и вывод результата
  
  A.GetArray1&<integer>(10).Println; // читаем значение типа "array of integer", длинной в 10
  
end.