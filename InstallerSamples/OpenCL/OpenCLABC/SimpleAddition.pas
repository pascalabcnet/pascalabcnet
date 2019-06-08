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
  
  // Выполнение
  
  prog['TEST'].Exec(10,
    
    A.NewQueue.WriteData(
      ArrFill(10,1)
    ) as CommandQueue<KernelArg>
    
  );
  
  // Чтение и вывод результата
  
  A.GetArray&<array of integer>(10).Println;
  
end.