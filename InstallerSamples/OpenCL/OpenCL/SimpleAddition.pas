uses OpenCL;
uses System;
uses System.Runtime.InteropServices;

//Описания всех подпрограмм найдёте в справке по OpenCL:
//www.khronos.org/registry/OpenCL/specs/2.2/html/OpenCL_API.html

begin
  var ec: ErrorCode;
  
  // Инициализация
  
  var platform: cl_platform_id;
  cl.GetPlatformIDs(1, @platform, nil).RaiseIfError;
  
  var device: cl_device_id;
  cl.GetDeviceIDs(platform, DeviceTypeFlags.Default, 1, @device, nil).RaiseIfError;
  
  // DeviceTypeFlags.Default это обычно GPU
  // К примеру, в ноутбуке его может не быть
  // Тогда надо хоть для чего то попытаться инициализировать
  // DeviceTypeFlags.All выберет первый любой девайс, поддерживающий OpenCL
//  cl.GetDeviceIDs(platform, DeviceTypeFlags.All, 1, @device, nil).RaiseIfError;
  
  var context := cl.CreateContext(nil, 1, @device, nil, nil, @ec);
  ec.RaiseIfError;
  
  var command_queue := cl.CreateCommandQueue(context, device, CommandQueuePropertyFlags.NONE, ec);
  ec.RaiseIfError;
  
  // Чтение и компиляция .cl файла
  
  {$resource SimpleAddition.cl} // эта строчка засовывает SimpleAddition.cl внутрь .exe, чтоб не надо было таскать его вместе с .exe
  var prog_str := System.IO.StreamReader.Create(GetResourceStream('SimpleAddition.cl')).ReadToEnd;
  var prog := cl.CreateProgramWithSource(
    context,
    1,
    new string[](prog_str),
    new UIntPtr[](new UIntPtr(prog_str.Length)),
    ec
  );
  ec.RaiseIfError;
  
  cl.BuildProgram(prog, 1, @device, nil, nil, nil).RaiseIfError;
  
  // Подготовка и запуск программы на GPU
  
  var kernel := cl.CreateKernel(prog, 'TEST', ec); // Обязательно то же имя что у карнела из .cl файла. И регистр важен!
  ec.RaiseIfError;
  
  var mem := Marshal.AllocHGlobal(40);
  Marshal.Copy(ArrFill(10,1),0,mem,10);
  var memobj := cl.CreateBuffer(context, MemoryFlags.READ_WRITE or MemoryFlags.USE_HOST_PTR, new UIntPtr(40), mem, ec); // USE_HOST_PTR значит что нужно скопировать память из mem в memobj
  ec.RaiseIfError;
  
  cl.SetKernelArg(kernel, 0, new UIntPtr(UIntPtr.Size), memobj).RaiseIfError;
  
  cl.EnqueueNDRangeKernel(command_queue, kernel, 1, nil,new UIntPtr[](new UIntPtr(10)),nil, 0,nil,nil).RaiseIfError;
  
  cl.Finish(command_queue).RaiseIfError;
  
  // Чтение и вывод результата
  
  cl.EnqueueReadBuffer(command_queue, memobj, 1, new UIntPtr(0), new UIntPtr(40), pointer(mem), 0,nil,nil).RaiseIfError;
  
  var res := new integer[10];
  Marshal.Copy(mem,res,0,10);
  res.Println;
  
end.