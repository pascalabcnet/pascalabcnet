uses OpenCLABC;

//ToDo issue компилятора:
// - #1981

begin
  
  // Чтение и компиляция .cl файла
  
  {$resource SimpleAddition.cl} // эта строчка засовывает SimpleAddition.cl внутрь .exe, чтоб не надо было таскать его вместе с .exe
  var prog := new ProgramCode(Context.Default,
    System.IO.StreamReader.Create(GetResourceStream('SimpleAddition.cl')).ReadToEnd
  );
  
  // Подготовка параметров
  
  var A := new KernelArg(40);
  
  // Подготовка очередей выполнения
  
  var prog_Q :=
    prog['TEST'].NewQueue.Exec(10,
      
      A.NewQueue.WriteData(
        ArrFill(10,1)
      ) as CommandQueue<KernelArg>
      
    ) as CommandQueue<Kernel>;
  
  // Выполнение
  
  Context.Default.SyncInvoke(prog_Q);
  
  // Чтение и вывод результата
  
  A.GetArray&<array of integer>(10).Println;
  
end.