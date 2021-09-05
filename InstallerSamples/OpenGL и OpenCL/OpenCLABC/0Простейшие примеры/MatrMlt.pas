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
    // Вообще лучше прекомпилировать .cl файл (загружать в переменную ProgramCode)
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
    Writeln;
    var A := new MemorySegment(MatrByteSize);
    
    'Матрица B:'.Println;
    var B_Mart := MatrRandomReal(MatrW,MatrW,0,1).Println;
    Writeln;
    var B := new MemorySegment(MatrByteSize);
    
    var C := new MemorySegment(MatrByteSize);
    
    'Вектор V1:'.Println;
    var V1_Arr := ArrRandomReal(MatrW);
    V1_Arr.Println;
    Writeln;
    var V1 := new MemorySegment(VecByteSize);
    
    var V2 := new MemorySegment(VecByteSize);
    
    var W := KernelArg.FromRecord(MatrW);
    
    // (запись значений в параметры - позже, в очередях)
    
    // Подготовка очередей выполнения
    
    var Calc_C_Q :=
      code['MatrMltMatr'].NewQueue.AddExec2(MatrW, MatrW, // Выделяем ядра в форме квадрата, всего MatrW*MatrW ядер
        A.NewQueue.AddWriteArray2&<real>(A_Matr), // Тип в &<> надо указывать явно, потому что компилятор не может вычислить его из типа элементов массива
        B.NewQueue.AddWriteArray2&<real>(B_Mart),
        C,
        W
      );
    
    var Otp_C_Q :=
      C.NewQueue.AddReadArray2&<real>(A_Matr) +
      HPQ(()->
      begin
        'Матрица С = A*B:'.Println;
        A_Matr.Println;
        Writeln;
      end);
    
    var Calc_V2_Q :=
      code['MatrMltVec'].NewQueue.AddExec1(MatrW,
        C,
        V1.NewQueue.AddWriteArray1&<real>(V1_Arr),
        V2,
        W
      );
    
    var Otp_V2_Q :=
      V2.NewQueue.AddReadArray1&<real>(V1_Arr) +
      HPQ(()->
      begin
        'Вектор V2 = C*V1:'.Println;
        V1_Arr.Println;
        Writeln;
      end);
    
    // Выполнение всего и сразу асинхронный вывод
    
    Context.Default.SyncInvoke(
      
      Calc_C_Q +
      Calc_V2_Q * Otp_C_Q + // Считать V2 и выводить C можно одновременно, поэтому тут *, т.е. параллельное выполнение
      Otp_V2_Q
      
    );
    
  except
    on e: Exception do Writeln(e); // Эта строчка позволяет выводить всю ошибку, если при выполнении Context.SyncInvoke возникла ошибка
  end;
end.