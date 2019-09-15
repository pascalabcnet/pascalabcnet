uses OpenCLABC;

begin
  
  // Когда код на языке OpenCL-C хранят в файле - обычно ему дают расширение .cl
  // Но код можно хранить и в $resource или даже в '' в программме
  var code_text := ReadAllText('0.cl');
  
  // Создание объекта типа ProgramCode
  // Перед текстом программы - можно так же указать контекст
  // Если не указывать - используется Context.Default
  var code := new ProgramCode(code_text);
  
  var A := new Buffer( 10 * sizeof(integer) ); // будет хранить 10 чисел типа "integer"
  
  code['TEST'].Exec1(10, // используем 10 ядер
    
    A.NewQueue.AddFillValue(1) // заполняем весь буфер единичками, прямо перед выполнением
    as CommandQueue<Buffer> //ToDo нужно только из за issue компилятора #1981, иначе получаем странную ошибку. Когда исправят - можно будет убрать
    
  );
  
  A.GetArray1&<integer>(10).Println; // читаем одномерный массив с элементами "integer", длинной в 10
  
end.