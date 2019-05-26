uses OpenCL;
uses System;
uses System.Runtime.InteropServices;

const
  MatrW = 4; // можно поменять на любое положительное значение
  
  MatrL = MatrW*MatrW;
  MatrByteSize = MatrL*8;

procedure MatrToUnmanagedArr(m: array[,] of real; mem: IntPtr);
begin
  var a := new real[m.Length];
  Buffer.BlockCopy(m,0, a,0, a.Length*8); // вообще не хорошо, лучше через GCHandle, тогда копировать не надо ничего
  Marshal.Copy(a,0, mem,a.Length);
end;

procedure UnmanagedArrToMatr(mem: IntPtr; m: array[,] of real);
begin
  var a := new real[m.Length];
  Marshal.Copy(mem,a, 0,a.Length);
  Buffer.BlockCopy(a,0, m,0, a.Length*8);
end;

begin
  var ec: ErrorCode;
  
  // Инициализация
  
  var platform: cl_platform_id;
  cl.GetPlatformIDs(1, @platform, nil).RaiseIfError;
  
  var device: cl_device_id;
  cl.GetDeviceIDs(platform, DeviceTypeFlags.Default, 1, @device, nil).RaiseIfError;
  
  var context := cl.CreateContext(nil, 1, @device, nil, nil, @ec);
  ec.RaiseIfError;
  
  var command_queue := cl.CreateCommandQueue(context, device, CommandQueuePropertyFlags.NONE, ec);
  ec.RaiseIfError;
  
  // Чтение и компиляция .cl файла
  
  {$resource MatrMlt.cl}
  var prog_str := System.IO.StreamReader.Create(GetResourceStream('MatrMlt.cl')).ReadToEnd;
  var prog := cl.CreateProgramWithSource(
    context,
    1,
    new string[](prog_str),
    new UIntPtr[](new UIntPtr(prog_str.Length)),
    ec
  );
  ec.RaiseIfError;
  
  cl.BuildProgram(prog, 1, @device, nil, nil, nil).RaiseIfError;
  
  var MatrMltMatrKernel := cl.CreateKernel(prog, 'MatrMltMatr', ec);
  ec.RaiseIfError;
  
  var MatrMltVecKernel := cl.CreateKernel(prog, 'MatrMltVec', ec);
  ec.RaiseIfError;
  
  // Подготовка параметров
  
  writeln('Матрица A:');
  var A := MatrRandomReal(MatrW,MatrW,0,1).Println;
  writeln;
  var Amem := Marshal.AllocHGlobal(MatrByteSize);
  MatrToUnmanagedArr(A,Amem);
  var AmemObj := cl.CreateBuffer(context, MemoryFlags.READ_WRITE or MemoryFlags.USE_HOST_PTR, new UIntPtr(MatrByteSize), Amem, ec);
  ec.RaiseIfError;
  
  writeln('Матрица B:');
  var B := MatrRandomReal(MatrW,MatrW,0,1).Println;
  writeln;
  var Bmem := Marshal.AllocHGlobal(MatrByteSize);
  MatrToUnmanagedArr(B,Bmem);
  var BmemObj := cl.CreateBuffer(context, MemoryFlags.READ_WRITE or MemoryFlags.USE_HOST_PTR, new UIntPtr(MatrByteSize), Bmem, ec);
  ec.RaiseIfError;
  
  writeln('Вектор V:');
  var V := ArrRandomReal(MatrW);
  V.Println;
  writeln;
  var Vmem := Marshal.AllocHGlobal(MatrW*8);
  Marshal.Copy(V,0, Vmem,MatrW);
  var VmemObj := cl.CreateBuffer(context, MemoryFlags.READ_WRITE or MemoryFlags.USE_HOST_PTR, new UIntPtr(MatrW*8), Vmem, ec);
  ec.RaiseIfError;
  
  var CmemObj := cl.CreateBuffer(context, MemoryFlags.READ_WRITE, new UIntPtr(MatrByteSize), nil, @ec);
  ec.RaiseIfError;
  
  var V2memObj := cl.CreateBuffer(context, MemoryFlags.READ_WRITE, new UIntPtr(MatrW*8), nil, @ec);
  ec.RaiseIfError;
  
  var MatrWParam := MatrW;
  var WmemObj := cl.CreateBuffer(context, MemoryFlags.READ_WRITE or MemoryFlags.USE_HOST_PTR, new UIntPtr(4), @MatrWParam, @ec);
  ec.RaiseIfError;
  
  // Выполнение C := A*B
  
  cl.SetKernelArg(MatrMltMatrKernel, 0, new UIntPtr(UIntPtr.Size), AmemObj).RaiseIfError;
  cl.SetKernelArg(MatrMltMatrKernel, 1, new UIntPtr(UIntPtr.Size), BmemObj).RaiseIfError;
  cl.SetKernelArg(MatrMltMatrKernel, 2, new UIntPtr(UIntPtr.Size), CmemObj).RaiseIfError;
  cl.SetKernelArg(MatrMltMatrKernel, 3, new UIntPtr(UIntPtr.Size), WmemObj).RaiseIfError;
  
  cl.EnqueueNDRangeKernel(command_queue, MatrMltMatrKernel, 2, nil,new UIntPtr[](new UIntPtr(MatrW),new UIntPtr(MatrW)),nil, 0,nil,nil).RaiseIfError;
  
  // Выполнение V2 := C*V
  
  cl.SetKernelArg(MatrMltVecKernel, 0, new UIntPtr(UIntPtr.Size), CmemObj).RaiseIfError;
  cl.SetKernelArg(MatrMltVecKernel, 1, new UIntPtr(UIntPtr.Size), VmemObj).RaiseIfError;
  cl.SetKernelArg(MatrMltVecKernel, 2, new UIntPtr(UIntPtr.Size), V2memobj).RaiseIfError;
  cl.SetKernelArg(MatrMltVecKernel, 3, new UIntPtr(UIntPtr.Size), WmemObj).RaiseIfError;
  
  cl.EnqueueNDRangeKernel(command_queue, MatrMltVecKernel, 1, nil,new UIntPtr[](new UIntPtr(MatrW)),nil, 0,nil,nil).RaiseIfError;
  
  // Чтение и вывод результата
  
  cl.EnqueueReadBuffer(command_queue, CmemObj,  0, new UIntPtr(0), new UIntPtr(MatrByteSize), Amem, 0,nil,nil).RaiseIfError;
  cl.EnqueueReadBuffer(command_queue, V2memObj, 0, new UIntPtr(0), new UIntPtr(MatrW*8),      Vmem, 0,nil,nil).RaiseIfError;
  
  cl.Finish(command_queue).RaiseIfError;
  
  writeln('Матрица С = A*B:');
  UnmanagedArrToMatr(Amem,A);
  A.Println;
  writeln;
  
  writeln('Вектор V2 = C*V:');
  Marshal.Copy(Vmem,V,0,MatrW);
  V.Println;
  
end.