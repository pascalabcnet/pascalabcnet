uses OpenCLABC;

const
  MatrW = 4; // Можно поменять на любое положительное значение
  
  VecByteSize = MatrW*8;
  MatrByteSize = MatrW*MatrW*8;
  
begin
  try
    Randomize(0); // Делает так, чтобы каждое выполнение давало одинаковый результат
    
    // Чтение и компиляция .cl файла
    
    {$resource MatrMlt.cl} // Засовывает файл MatrMlt.cl внуть .exe
    // Вообще лучше прекомпилировать .cl файл
    // (загружать в переменную типа ProgramCode)
    // И сохранять с помощью метода ProgramCode.SerializeTo
    // А полученный бинарник уже подключать через $resource
    var code := new ProgramCode(Context.Default,
      System.IO.StreamReader.Create(
        System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream('MatrMlt.cl')
      ).ReadToEnd
    );
    
    // Подготовка параметров
    
    'Матрица A:'.Println;
    var A_Matr := MatrRandomReal(MatrW,MatrW,0,1).Println;
    Println;
    var A := new MemorySegment(MatrByteSize);
    
    'Матрица B:'.Println;
    var B_Mart := MatrRandomReal(MatrW,MatrW,0,1).Println;
    Println;
    var B := new MemorySegment(MatrByteSize);
    
    var C := new MemorySegment(MatrByteSize);
    
    'Вектор V1:'.Println;
    var V1_Arr := ArrRandomReal(MatrW);
    V1_Arr.Println;
    Println;
    var V1 := new MemorySegment(VecByteSize);
    
    var V2 := new MemorySegment(VecByteSize);
    
    var W := KernelArg.FromValue(MatrW);
    
    // (запись значений в параметры - позже, в очередях)
    
    // Подготовка очередей выполнения
    
    var Calc_C_Q :=
      // Выделяем ядра в форме квадрата, всего MatrW*MatrW ядер
      code['MatrMltMatr'].NewQueue.ThenExec2(MatrW, MatrW,
        A.NewQueue.ThenWriteArray2(A_Matr),
        B.NewQueue.ThenWriteArray2(B_Mart),
        C,
        W
      // DiscardResult не обязательно, но желательно
      // чтобы не использовать результат случайно
      ).DiscardResult;
    
    var Otp_C_Q :=
      C.NewQueue.ThenGetArray2&<real>(MatrW, MatrW)
      .ThenQuickUse(C_Matr->
      begin
        'Матрица С = A*B:'.Println;
        C_Matr.Println;
        Println;
      end).DiscardResult;
    
    var Calc_V2_Q :=
      code['MatrMltVec'].NewQueue.ThenExec1(MatrW,
        C,
        V1.NewQueue.ThenWriteArray1(V1_Arr),
        V2,
        W
      ).DiscardResult;
    
    var Otp_V2_Q :=
      V2.NewQueue.ThenGetArray1&<real>
      .ThenQuickUse(V2_Arr->
      begin
        'Вектор V2 = C*V1:'.Println;
        V2_Arr.Println;
        Println;
      // Единственный DiscardResult, меняющий поведение очереди:
      // С ним не выделяются ресурсы на то, чтобы передать V2_Arr
      // Из результата ThenGetArray1 в результат SyncInvoke
      end).DiscardResult;
    
    // Выполнение всего и сразу асинхронный вывод
    
    Context.Default.SyncInvoke(
      
      Calc_C_Q +
      Calc_V2_Q * Otp_C_Q + // Считать V2 и выводить C можно одновременно, поэтому тут *, т.е. параллельное выполнение
      Otp_V2_Q
      
    );
    
  except
    // except позволяет получать список ошибку,
    // возникшую при выполнении SyncInvoke
    on ae: System.AggregateException do
      foreach var e in ae.InnerExceptions do
        Println(e); 
  end;
end.