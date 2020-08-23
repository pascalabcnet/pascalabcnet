uses OpenCLABC;

begin
  
  // Запустите "Прекомпилировать.pas", чтобы создать этот файл
  {$resource 0.cl.temp_bin}
  
  var prog := ProgramCode.DeserializeFrom(Context.Default,
    System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream('0.cl.temp_bin')
  );
  
  // Дальше всё так же как в "0Простейшие примеры\SimpleAddition"
  
  var A := new Buffer( 10 * sizeof(integer) );
  
  prog['TEST'].Exec1(10,
    
    A.NewQueue.AddFillValue(1)
    
  );
  
  A.GetArray1&<integer>.Println;
  
end.