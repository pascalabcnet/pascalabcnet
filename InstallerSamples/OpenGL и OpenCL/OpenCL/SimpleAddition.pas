uses OpenCL;
uses System;
uses System.Runtime.InteropServices;

const
  buf_size = 10;
  buf_byte_size = buf_size * 4;
  
begin
  var ec: ErrorCode;
  
  // Инициализация
  
  var platform: cl_platform_id;
  cl.GetPlatformIDs(1, platform, IntPtr.Zero).RaiseIfError;
  
  var device: cl_device_id;
  cl.GetDeviceIDs(platform, DeviceType.DEVICE_TYPE_DEFAULT, 1,device,IntPtr.Zero).RaiseIfError;
  
  // DEVICE_TYPE_DEFAULT это, обычно, GPU
  // Но, к примеру, в ноутбуке его может не быть
  // Тогда надо хоть для чего то попытаться инициализировать
  // DEVICE_TYPE_ALL выберет первое любое устройство, поддерживающее OpenCL
//  cl.GetDeviceIDs(platform, DeviceType.DEVICE_TYPE_ALL, 1,device,IntPtr.Zero).RaiseIfError;
  // Если всё ещё пишет что устройств нет - обновите драйверы,
  // потому что даже встроенные видеокарты поддерживают OpenCL
  
  var context := cl.CreateContext(nil, 1,device, nil,IntPtr.Zero, ec);
  ec.RaiseIfError;
  
  var command_queue := cl.CreateCommandQueueWithProperties(context, device, nil, ec);
  ec.RaiseIfError;
  
  // Чтение и компиляция .cl файла
  
  {$resource SimpleAddition.cl} // эта строчка засовывает SimpleAddition.cl внутрь .exe, чтоб не надо было таскать его вместе с .exe
  var prog_str := System.IO.StreamReader.Create(GetResourceStream('SimpleAddition.cl')).ReadToEnd;
  var prog := cl.CreateProgramWithSource(
    context,
    1,
    new string[](prog_str),
    nil,
    ec
  );
  ec.RaiseIfError;
  
  cl.BuildProgram(prog, 1,device, nil, nil,IntPtr.Zero).RaiseIfError;
  
  // Подготовка и запуск программы на GPU
  
  var kernel := cl.CreateKernel(prog, 'TEST', ec); // То же имя что у kernel'а из .cl файла. Регистр важен!
  ec.RaiseIfError;
  
  var buf := cl.CreateBuffer(context, MemFlags.MEM_READ_WRITE, new UIntPtr(buf_byte_size), IntPtr.Zero, ec);
  ec.RaiseIfError;
  
  var buf_fill_pattern := 1;
  var buf_fill_ev: cl_event;
  cl.EnqueueFillBuffer(command_queue, buf, buf_fill_pattern,new UIntPtr(sizeof(integer)), UIntPtr.Zero,new UIntPtr(buf_byte_size), 0,nil,buf_fill_ev).RaiseIfError;
  
  cl.SetKernelArg(kernel, 0, new UIntPtr(cl_mem.Size), buf).RaiseIfError;
  
  var k_ev: cl_event;
  cl.EnqueueNDRangeKernel(command_queue, kernel, 1, nil,new UIntPtr[](new UIntPtr(buf_size)),nil, 1,buf_fill_ev,k_ev).RaiseIfError;
  
  // Чтение и вывод результата
  
  var res := new integer[buf_size];
  cl.EnqueueReadBuffer(command_queue, buf, Bool.BLOCKING, UIntPtr.Zero, new UIntPtr(buf_byte_size), res[0], 1,k_ev,IntPtr.Zero).RaiseIfError;
  res.Println;
  
end.