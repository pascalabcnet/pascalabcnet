﻿
//*****************************************************************************************************\\
// Copyright (©) Cergey Latchenko ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// This code is distributed under the Unlicense
// For details see LICENSE file or this:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\
// Copyright (©) Сергей Латченко ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// Этот код распространяется с лицензией Unlicense
// Подробнее в файле LICENSE или тут:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\

/// Модуль для внутренних типов модуля OpenCLABC
unit OpenCLABCBase;

{$region ToDo}

//===================================
// Обязательно сделать до следующего пула:

//===================================
// Запланированное:

//ToDo В методах вроде .AddWriteArray1 приходится добавлять &<>

//ToDo Подумать как об этом можно написать в справке (или не в справке):
// - ReadValue отсутствует
// --- В объяснении KernelArg из указателя всё уже сказано
// --- Надо только как то объединить, чтоб текст был не только про KernelArg...
// - FillArray отсутствует
// --- Проблема в том, что нет блокирующего варианта FillArray
// --- Вообще в теории можно написать отдельную мелкую неуправляемую .dll и $resource её
// --- Но это жесть сколько усложнений ради 1 метода...

//ToDo А что если вещи, которые могут привести к утечкам памяти при ThreadAbortException (как конструктор контекста) сувать в finally пустого try?
// - Вообще, поидее, должен быть более красивый способ добиться того же... Что то с контрактами?
// - Обязательно сравнить скорость, перед тем как применять...

//ToDo Buffer переименовать в GPUMem
//ToDo И добавить типы как GPUArray - наверное с методом вроде .Flush, для более эффективной записи
//
//ToDo Инициалию буфера из BufferCommandQueue перенести в, собственно, вызовы GPUCommand.Invoke
// - И там же вызывать GPUArray.Flush

//ToDo Можно же сохранять неуправляемые очереди в список внутри CLTask, и затем использовать несколько раз
// - И почему я раньше об этом не подумал...

//ToDo В тестеровщике, в тестах ошибок, в текстах ошибок - постоянно меняются номера лямбд...
// - Наверное стоит захардкодить в тестировщик игнор числа после "<>lambda", и так же для контейнера лямбды

//ToDo Заполнение Platform.All сейчас вылетит на компе с 0 платформ...
// - Сразу не забыть исправить описание

//ToDo Всё же стоит добавить .ThenUse - аналог .ThenConvert, не изменяющий значение, а только использующий

//ToDo IWaitQueue.CancelWait
//ToDo WaitAny(aborter, WaitAll(...));
// - Что случится с WaitAll если aborter будет первым?
// - Очереди переданные в Wait - вообще не запускаются так
// - Поэтому я и думал про что то типа CancelWait
// - А вообще лучше разрешить выполнять Wait внутри другого Wait
// - И заодно проверить чтобы Abort работало на Wait-ы
// - А вообще всё не то - это костыли
// - Надо специально разрешить передавать какой то маркер аборта
// - И только в WaitAll, другим Wait-ам это не нужно
// - Или можно забыть про всё это и сделать+использовать AbortQueue чтоб убивать и Wait-ы, и всё остальное

//ToDo Проверки и кидания исключений перед всеми cl.*, чтобы выводить норм сообщения об ошибках
// - В том числе проверки с помощью BlittableHelper

//ToDo Создание SubDevice из cl_device_id

//ToDo Очереди-маркеры для Wait-очередей
// - чтобы не приходилось использовать константные для этого

//ToDo Очередь-обработчик ошибок
// - .HandleExceptions
// - Сделать легко, надо только вставить свой промежуточный CLTaskBase
// - Единственное - для Wait очереди надо хранить так же оригинальный CLTaskBase
//ToDo И какой то аналог try-finally
// - .ThenFinally ?
//ToDo Раздел справки про обработку ошибок
// - Написать что аналог try-finally стоит использовать на Wait-маркерах для потоко-безопастности
//
//ToDo Когда будут очереди-обработчики - удалить ивенты CLTask-ов. Они, по сути, ограниченная версия.
// - И использование их тут изнутри - в целом говнокод...

//ToDo Синхронные (с припиской Fast, а может Quick) варианты всего работающего по принципу HostQueue
//
//ToDo И асинхронные умнее запускать - помнить значение, указывающее можно ли выполнить их синхронно
// - Может даже можно синхронно выполнить "HPQ(...)+HPQ(...)", в некоторых случаях?
//ToDo Enqueueabl-ы вызывают .Invoke для первого параметра и .InvokeNewQ для остальных
// - А что если все параметры кроме последнего - константы?
// - Надо как то умнее это обрабатывать
//ToDo И сделать наконец нормальный класс-контейнер состояния очереди, параметрами всё не передашь

//ToDo CommmandQueueBase.ToString для дебага
// - так же дублирующий protected метод (tabs: integer; index: Dictionary<CommandQueueBase,integer>)

//ToDo .Cycle(integer)
//ToDo .Cycle // бесконечность циклов
//ToDo .CycleWhile(***->boolean)
// - Возможность передать свой обработчик ошибок как Exception->Exception
//ToDo В продолжение Cycle: Однако всё ещё остаётся проблема - как сделать ветвление?
// - И если уже делать - стоит сделать и метод CQ.ThenIf(res->boolean; if_true, if_false: CQ)
//ToDo И ещё - AbortQueue, который, по сути, может использоваться как exit, continue или break, если с обработчиками ошибок
// - Или может метод MarkerQueue.Abort?

//ToDo Интегрировать профайлинг очередей

//ToDo Перепродумать SubBuffer, в случае перевыделения основного буфера - он плохо себя ведёт...

//ToDo Может всё же сделать защиту от дурака для "q.AddQueue(q)"?
// - И в справке тогда убрать параграф...

//===================================
// Сделать когда-нибуть:

//ToDo Пройтись по всем функциям OpenCL, посмотреть функционал каких не доступен из OpenCLABC
// - clGetKernelWorkGroupInfo - свойства кернела на определённом устройстве

{$endregion ToDo}

{$region Bugs}

//ToDo Issue компилятора:
//ToDo https://github.com/pascalabcnet/pascalabcnet/issues/{id}
// - #1981
// - #2145
// - #2221
// - #2289
// - #2290

//ToDo Баги NVidia
//ToDo https://developer.nvidia.com/nvidia_bug/{id}
// - NV#3035203

{$endregion}

{$region Debug}{$ifdef DEBUG}

{ $define EventDebug} // регистрация всех cl.RetainEvent и cl.ReleaseEvent

{$endif DEBUG}{$endregion Debug}

interface

uses System;
uses System.Threading;
uses System.Runtime.InteropServices;
uses System.Collections.ObjectModel;

uses OpenCL;

type
  
  {$region Properties}
  
  {$region Base}
  
  NtvPropertiesBase<TNtv, TInfo> = abstract class
    protected ntv: TNtv;
    public constructor(ntv: TNtv) := self.ntv := ntv;
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected procedure GetSizeImpl(id: TInfo; var sz: UIntPtr); abstract;
    protected procedure GetValImpl(id: TInfo; sz: UIntPtr; var res: byte); abstract;
    
    protected function GetSize(id: TInfo): UIntPtr;
    begin GetSizeImpl(id, Result); end;
    
    protected procedure FillPtr(id: TInfo; sz: UIntPtr; ptr: IntPtr) :=
    GetValImpl(id, sz, PByte(pointer(ptr))^);
    protected procedure FillVal<T>(id: TInfo; sz: UIntPtr; var res: T) :=
    GetValImpl(id, sz, PByte(pointer(@res))^);
    
    protected function GetVal<T>(id: TInfo): T;
    begin FillVal(id, new UIntPtr(Marshal.SizeOf&<T>), Result); end;
    protected function GetValArr<T>(id: TInfo): array of T;
    begin
      var sz := GetSize(id);
      Result := new T[uint64(sz) div Marshal.SizeOf&<T>];
      
      if Result.Length<>0 then
        FillVal(id, sz, Result[0]);
      
    end;
    protected function GetValArrArr<T>(id: TInfo; szs: array of UIntPtr): array of array of T;
    type PT = ^T;
    begin
      if szs.Length=0 then
      begin
        SetLength(Result,0);
        exit;
      end;
      
      var res := new IntPtr[szs.Length];
      SetLength(Result, szs.Length);
      
      for var i := 0 to szs.Length-1 do res[i] := Marshal.AllocHGlobal(IntPtr(pointer(szs[i])));
      try
        
        FillVal(id, new UIntPtr(szs.Length*Marshal.SizeOf&<IntPtr>), res[0]);
        
        var tsz := Marshal.SizeOf&<T>;
        for var i := 0 to szs.Length-1 do
        begin
          Result[i] := new T[uint64(szs[i]) div tsz];
          //ToDo более эффективное копирование
          for var i2 := 0 to Result[i].Length-1 do
            Result[i][i2] := PT(pointer(res[i]+tsz*i2))^;
        end;
        
      finally
        for var i := 0 to szs.Length-1 do Marshal.FreeHGlobal(res[i]);
      end;
      
    end;
    
    {$region GetInt}
    
    private function GetIntPtr(id: TInfo) := GetVal&<IntPtr>(id);
    
    private function GetUInt32(id: TInfo) := GetVal&<UInt32>(id);
    private function GetUInt64(id: TInfo) := GetVal&<UInt64>(id);
    private function GetUIntPtr(id: TInfo) := GetVal&<UIntPtr>(id);
    
    {$endregion GetInt}
    
    {$region GetIntArr}
    
    private function GetByteArr(id: TInfo) := GetValArr&<Byte>(id);
    private function GetUIntPtrArr(id: TInfo) := GetValArr&<UIntPtr>(id);
    
    private function GetByteArrArr(id: TInfo; szs: array of UIntPtr) := GetValArrArr&<Byte>(id, szs);
    
    {$endregion GetIntArr}
    
    {$region GetString}
    
    private function GetString(id: TInfo): string;
    begin
      var sz := GetSize(id);
      
      var str_ptr := Marshal.AllocHGlobal(IntPtr(pointer(sz)));
      try
        FillPtr(id, sz, str_ptr);
        Result := Marshal.PtrToStringAnsi(str_ptr);
      finally
        Marshal.FreeHGlobal(str_ptr);
      end;
      
    end;
    
    {$endregion GetString}
    
    {$region GetBoolean}
    
    private function GetBoolean(id: TInfo) := GetVal&<Bool>(id).val <> 0;
    
    {$endregion GetBoolean}
    
  end;
  
  {$endregion Base}
  
  {$region Platform}
  
  PlatformProperties = sealed class(NtvPropertiesBase<cl_platform_id, PlatformInfo>)
    
    private static function clGetSize(platform: cl_platform_id; param_name: PlatformInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformInfo';
    private static function clGetVal(platform: cl_platform_id; param_name: PlatformInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformInfo';
    
    protected procedure GetSizeImpl(id: PlatformInfo; var sz: UIntPtr); override :=
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: PlatformInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
    public property Profile:             String read GetString(PlatformInfo.PLATFORM_PROFILE);
    public property Version:             String read GetString(PlatformInfo.PLATFORM_VERSION);
    public property Name:                String read GetString(PlatformInfo.PLATFORM_NAME);
    public property Vendor:              String read GetString(PlatformInfo.PLATFORM_VENDOR);
    public property Extensions:          String read GetString(PlatformInfo.PLATFORM_EXTENSIONS);
    public property HostTimerResolution: UInt64 read GetUInt64(PlatformInfo.PLATFORM_HOST_TIMER_RESOLUTION);
    
  end;
  
  {$endregion Platform}
  
  {$region Device}
  
  DeviceProperties = sealed class(NtvPropertiesBase<cl_device_id, DeviceInfo>)
    
    private static function clGetSize(device: cl_device_id; param_name: DeviceInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceInfo';
    private static function clGetVal(device: cl_device_id; param_name: DeviceInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceInfo';
    
    protected procedure GetSizeImpl(id: DeviceInfo; var sz: UIntPtr); override :=
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: DeviceInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
    private function GetDeviceType(id: DeviceInfo)                  := GetVal&<DeviceType>(id);
    private function GetDeviceFPConfig(id: DeviceInfo)              := GetVal&<DeviceFPConfig>(id);
    private function GetDeviceMemCacheType(id: DeviceInfo)          := GetVal&<DeviceMemCacheType>(id);
    private function GetDeviceLocalMemType(id: DeviceInfo)          := GetVal&<DeviceLocalMemType>(id);
    private function GetDeviceExecCapabilities(id: DeviceInfo)      := GetVal&<DeviceExecCapabilities>(id);
    private function GetCommandQueueProperties(id: DeviceInfo)      := GetVal&<CommandQueueProperties>(id);
    private function GetDeviceAffinityDomain(id: DeviceInfo)        := GetVal&<DeviceAffinityDomain>(id);
    private function GetDeviceSVMCapabilities(id: DeviceInfo)       := GetVal&<DeviceSVMCapabilities>(id);
    
    private function GetDevicePartitionPropertyArr(id: DeviceInfo)  := GetValArr&<DevicePartitionProperty>(id);
    
    public property &Type:                              DeviceType                       read GetDeviceType                (DeviceInfo.DEVICE_TYPE);
    public property VendorId:                           UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_VENDOR_ID);
    public property MaxComputeUnits:                    UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_COMPUTE_UNITS);
    public property MaxWorkItemDimensions:              UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_WORK_ITEM_DIMENSIONS);
    public property MaxWorkItemSizes:                   array of UIntPtr                 read GetUIntPtrArr                (DeviceInfo.DEVICE_MAX_WORK_ITEM_SIZES);
    public property MaxWorkGroupSize:                   UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_MAX_WORK_GROUP_SIZE);
    public property PreferredVectorWidthChar:           UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_CHAR);
    public property PreferredVectorWidthShort:          UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_SHORT);
    public property PreferredVectorWidthInt:            UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_INT);
    public property PreferredVectorWidthLong:           UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_LONG);
    public property PreferredVectorWidthFloat:          UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_FLOAT);
    public property PreferredVectorWidthDouble:         UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_DOUBLE);
    public property PreferredVectorWidthHalf:           UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_HALF);
    public property NativeVectorWidthChar:              UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_CHAR);
    public property NativeVectorWidthShort:             UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_SHORT);
    public property NativeVectorWidthInt:               UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_INT);
    public property NativeVectorWidthLong:              UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_LONG);
    public property NativeVectorWidthFloat:             UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_FLOAT);
    public property NativeVectorWidthDouble:            UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_DOUBLE);
    public property NativeVectorWidthHalf:              UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_HALF);
    public property MaxClockFrequency:                  UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_CLOCK_FREQUENCY);
    public property AddressBits:                        UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_ADDRESS_BITS);
    public property MaxMemAllocSize:                    UInt64                           read GetUInt64                    (DeviceInfo.DEVICE_MAX_MEM_ALLOC_SIZE);
    public property ImageSupport:                       Boolean                          read GetBoolean                   (DeviceInfo.DEVICE_IMAGE_SUPPORT);
    public property MaxReadImageArgs:                   UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_READ_IMAGE_ARGS);
    public property MaxWriteImageArgs:                  UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_WRITE_IMAGE_ARGS);
    public property MaxReadWriteImageArgs:              UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_READ_WRITE_IMAGE_ARGS);
    public property IlVersion:                          String                           read GetString                    (DeviceInfo.DEVICE_IL_VERSION);
    public property Image2dMaxWidth:                    UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_IMAGE2D_MAX_WIDTH);
    public property Image2dMaxHeight:                   UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_IMAGE2D_MAX_HEIGHT);
    public property Image3dMaxWidth:                    UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_IMAGE3D_MAX_WIDTH);
    public property Image3dMaxHeight:                   UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_IMAGE3D_MAX_HEIGHT);
    public property Image3dMaxDepth:                    UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_IMAGE3D_MAX_DEPTH);
    public property ImageMaxBufferSize:                 UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_IMAGE_MAX_BUFFER_SIZE);
    public property ImageMaxArraySize:                  UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_IMAGE_MAX_ARRAY_SIZE);
    public property MaxSamplers:                        UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_SAMPLERS);
    public property ImagePitchAlignment:                UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_IMAGE_PITCH_ALIGNMENT);
    public property ImageBaseAddressAlignment:          UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_IMAGE_BASE_ADDRESS_ALIGNMENT);
    public property MaxPipeArgs:                        UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_PIPE_ARGS);
    public property PipeMaxActiveReservations:          UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PIPE_MAX_ACTIVE_RESERVATIONS);
    public property PipeMaxPacketSize:                  UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PIPE_MAX_PACKET_SIZE);
    public property MaxParameterSize:                   UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_MAX_PARAMETER_SIZE);
    public property MemBaseAddrAlign:                   UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MEM_BASE_ADDR_ALIGN);
    public property SingleFpConfig:                     DeviceFPConfig                   read GetDeviceFPConfig            (DeviceInfo.DEVICE_SINGLE_FP_CONFIG);
    public property DoubleFpConfig:                     DeviceFPConfig                   read GetDeviceFPConfig            (DeviceInfo.DEVICE_DOUBLE_FP_CONFIG);
    public property GlobalMemCacheType:                 DeviceMemCacheType               read GetDeviceMemCacheType        (DeviceInfo.DEVICE_GLOBAL_MEM_CACHE_TYPE);
    public property GlobalMemCachelineSize:             UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_GLOBAL_MEM_CACHELINE_SIZE);
    public property GlobalMemCacheSize:                 UInt64                           read GetUInt64                    (DeviceInfo.DEVICE_GLOBAL_MEM_CACHE_SIZE);
    public property GlobalMemSize:                      UInt64                           read GetUInt64                    (DeviceInfo.DEVICE_GLOBAL_MEM_SIZE);
    public property MaxConstantBufferSize:              UInt64                           read GetUInt64                    (DeviceInfo.DEVICE_MAX_CONSTANT_BUFFER_SIZE);
    public property MaxConstantArgs:                    UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_CONSTANT_ARGS);
    public property MaxGlobalVariableSize:              UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_MAX_GLOBAL_VARIABLE_SIZE);
    public property GlobalVariablePreferredTotalSize:   UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_GLOBAL_VARIABLE_PREFERRED_TOTAL_SIZE);
    public property LocalMemType:                       DeviceLocalMemType               read GetDeviceLocalMemType        (DeviceInfo.DEVICE_LOCAL_MEM_TYPE);
    public property LocalMemSize:                       UInt64                           read GetUInt64                    (DeviceInfo.DEVICE_LOCAL_MEM_SIZE);
    public property ErrorCorrectionSupport:             Boolean                          read GetBoolean                   (DeviceInfo.DEVICE_ERROR_CORRECTION_SUPPORT);
    public property ProfilingTimerResolution:           UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_PROFILING_TIMER_RESOLUTION);
    public property EndianLittle:                       Boolean                          read GetBoolean                   (DeviceInfo.DEVICE_ENDIAN_LITTLE);
    public property Available:                          Boolean                          read GetBoolean                   (DeviceInfo.DEVICE_AVAILABLE);
    public property CompilerAvailable:                  Boolean                          read GetBoolean                   (DeviceInfo.DEVICE_COMPILER_AVAILABLE);
    public property LinkerAvailable:                    Boolean                          read GetBoolean                   (DeviceInfo.DEVICE_LINKER_AVAILABLE);
    public property ExecutionCapabilities:              DeviceExecCapabilities           read GetDeviceExecCapabilities    (DeviceInfo.DEVICE_EXECUTION_CAPABILITIES);
    public property QueueOnHostProperties:              CommandQueueProperties           read GetCommandQueueProperties    (DeviceInfo.DEVICE_QUEUE_ON_HOST_PROPERTIES);
    public property QueueOnDeviceProperties:            CommandQueueProperties           read GetCommandQueueProperties    (DeviceInfo.DEVICE_QUEUE_ON_DEVICE_PROPERTIES);
    public property QueueOnDevicePreferredSize:         UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_QUEUE_ON_DEVICE_PREFERRED_SIZE);
    public property QueueOnDeviceMaxSize:               UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_QUEUE_ON_DEVICE_MAX_SIZE);
    public property MaxOnDeviceQueues:                  UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_ON_DEVICE_QUEUES);
    public property MaxOnDeviceEvents:                  UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_ON_DEVICE_EVENTS);
    public property BuiltInKernels:                     String                           read GetString                    (DeviceInfo.DEVICE_BUILT_IN_KERNELS);
    public property Name:                               String                           read GetString                    (DeviceInfo.DEVICE_NAME);
    public property Vendor:                             String                           read GetString                    (DeviceInfo.DEVICE_VENDOR);
    public property Profile:                            String                           read GetString                    (DeviceInfo.DEVICE_PROFILE);
    public property Version:                            String                           read GetString                    (DeviceInfo.DEVICE_VERSION);
    public property OpenclCVersion:                     String                           read GetString                    (DeviceInfo.DEVICE_OPENCL_C_VERSION);
    public property Extensions:                         String                           read GetString                    (DeviceInfo.DEVICE_EXTENSIONS);
    public property PrintfBufferSize:                   UIntPtr                          read GetUIntPtr                   (DeviceInfo.DEVICE_PRINTF_BUFFER_SIZE);
    public property PreferredInteropUserSync:           Boolean                          read GetBoolean                   (DeviceInfo.DEVICE_PREFERRED_INTEROP_USER_SYNC);
    public property PartitionMaxSubDevices:             UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PARTITION_MAX_SUB_DEVICES);
    public property PartitionProperties:                array of DevicePartitionProperty read GetDevicePartitionPropertyArr(DeviceInfo.DEVICE_PARTITION_PROPERTIES);
    public property PartitionAffinityDomain:            DeviceAffinityDomain             read GetDeviceAffinityDomain      (DeviceInfo.DEVICE_PARTITION_AFFINITY_DOMAIN);
    public property PartitionType:                      array of DevicePartitionProperty read GetDevicePartitionPropertyArr(DeviceInfo.DEVICE_PARTITION_TYPE);
    public property ReferenceCount:                     UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_REFERENCE_COUNT);
    public property SvmCapabilities:                    DeviceSVMCapabilities            read GetDeviceSVMCapabilities     (DeviceInfo.DEVICE_SVM_CAPABILITIES);
    public property PreferredPlatformAtomicAlignment:   UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PREFERRED_PLATFORM_ATOMIC_ALIGNMENT);
    public property PreferredGlobalAtomicAlignment:     UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PREFERRED_GLOBAL_ATOMIC_ALIGNMENT);
    public property PreferredLocalAtomicAlignment:      UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_PREFERRED_LOCAL_ATOMIC_ALIGNMENT);
    public property MaxNumSubGroups:                    UInt32                           read GetUInt32                    (DeviceInfo.DEVICE_MAX_NUM_SUB_GROUPS);
    public property SubGroupIndependentForwardProgress: Boolean                          read GetBoolean                   (DeviceInfo.DEVICE_SUB_GROUP_INDEPENDENT_FORWARD_PROGRESS);
    
  end;
  
  {$endregion Device}
  
  {$region Context}
  
  ContextProperties = sealed class(NtvPropertiesBase<cl_context, ContextInfo>)
    
    private static function clGetSize(context: cl_context; param_name: ContextInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetContextInfo';
    private static function clGetVal(context: cl_context; param_name: ContextInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetContextInfo';
    
    protected procedure GetSizeImpl(id: ContextInfo; var sz: UIntPtr); override :=
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: ContextInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
    private function GetContextPropertiesArr(id: ContextInfo) := GetValArr&<ContextProperties>(id);
    
    public property ReferenceCount: UInt32                     read GetUInt32              (ContextInfo.CONTEXT_REFERENCE_COUNT);
    public property NumDevices:     UInt32                     read GetUInt32              (ContextInfo.CONTEXT_NUM_DEVICES);
    public property Properties:     array of ContextProperties read GetContextPropertiesArr(ContextInfo.CONTEXT_PROPERTIES);
    
  end;
  
  {$endregion Context}
  
  {$region Buffer}
  
  BufferProperties = sealed class(NtvPropertiesBase<cl_mem, MemInfo>)
    
    private static function clGetSize(memobj: cl_mem; param_name: MemInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    private static function clGetVal(memobj: cl_mem; param_name: MemInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    
    protected procedure GetSizeImpl(id: MemInfo; var sz: UIntPtr); override :=
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: MemInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
    private function GetMemObjectType(id: MemInfo)  := GetVal&<MemObjectType>(id);
    private function GetMemFlags(id: MemInfo)       := GetVal&<MemFlags>(id);
    
    public property &Type:          MemObjectType read GetMemObjectType(MemInfo.MEM_TYPE);
    public property Flags:          MemFlags      read GetMemFlags     (MemInfo.MEM_FLAGS);
    public property Size:           UIntPtr       read GetUIntPtr      (MemInfo.MEM_SIZE);
    public property HostPtr:        IntPtr        read GetIntPtr       (MemInfo.MEM_HOST_PTR);
    public property MapCount:       UInt32        read GetUInt32       (MemInfo.MEM_MAP_COUNT);
    public property ReferenceCount: UInt32        read GetUInt32       (MemInfo.MEM_REFERENCE_COUNT);
    public property UsesSvmPointer: Boolean       read GetBoolean      (MemInfo.MEM_USES_SVM_POINTER);
    public property Offset:         UIntPtr       read GetUIntPtr      (MemInfo.MEM_OFFSET);
    
  end;
  
  {$endregion Buffer}
  
  {$region Kernel}
  
  KernelProperties = sealed class(NtvPropertiesBase<cl_kernel, KernelInfo>)
    
    private static function clGetSize(kernel: cl_kernel; param_name: KernelInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelInfo';
    private static function clGetVal(kernel: cl_kernel; param_name: KernelInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelInfo';
    
    protected procedure GetSizeImpl(id: KernelInfo; var sz: UIntPtr); override :=
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: KernelInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
    public property FunctionName:   String read GetString(KernelInfo.KERNEL_FUNCTION_NAME);
    public property NumArgs:        UInt32 read GetUInt32(KernelInfo.KERNEL_NUM_ARGS);
    public property ReferenceCount: UInt32 read GetUInt32(KernelInfo.KERNEL_REFERENCE_COUNT);
    public property Attributes:     String read GetString(KernelInfo.KERNEL_ATTRIBUTES);
    
  end;
  
  {$endregion Kernel}
  
  {$region Program}
  
  ProgramProperties = sealed class(NtvPropertiesBase<cl_program, ProgramInfo>)
    
    private static function clGetSize(&program: cl_program; param_name: ProgramInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramInfo';
    private static function clGetVal(&program: cl_program; param_name: ProgramInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramInfo';
    
    protected procedure GetSizeImpl(id: ProgramInfo; var sz: UIntPtr); override :=
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: ProgramInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
    public property ReferenceCount:          UInt32                 read GetUInt32    (ProgramInfo.PROGRAM_REFERENCE_COUNT);
    public property Source:                  String                 read GetString    (ProgramInfo.PROGRAM_SOURCE);
    public property Il:                      array of Byte          read GetByteArr   (ProgramInfo.PROGRAM_IL);
    public property BinarySizes:             array of UIntPtr       read GetUIntPtrArr(ProgramInfo.PROGRAM_BINARY_SIZES);
    public property Binaries:                array of array of Byte read GetByteArrArr(ProgramInfo.PROGRAM_BINARIES, BinarySizes);
    public property NumKernels:              UIntPtr                read GetUIntPtr   (ProgramInfo.PROGRAM_NUM_KERNELS);
    public property KernelNames:             String                 read GetString    (ProgramInfo.PROGRAM_KERNEL_NAMES);
    public property ScopeGlobalCtorsPresent: Boolean                read GetBoolean   (ProgramInfo.PROGRAM_SCOPE_GLOBAL_CTORS_PRESENT);
    public property ScopeGlobalDtorsPresent: Boolean                read GetBoolean   (ProgramInfo.PROGRAM_SCOPE_GLOBAL_DTORS_PRESENT);
    
  end;
  
  {$endregion Program}
  
  {$endregion Properties}
  
  {$region Wrappers}
  
  {$region Base}
  
  WrapperBase<TNtv, TProp> = abstract class
  where TProp: class;
    private ntv: TNtv;
    
    private _properties: TProp;
    protected function GetProperties: TProp;
    begin
      if _properties=nil then _properties := CreateProp;
      Result := _properties;
    end;
    private function CreateProp: TProp; abstract;
    
    ///--
    public function Equals(obj: object): boolean; override :=
    (obj is WrapperBase<TNtv, TProp>(var wr)) and (self.ntv=wr.ntv);
    
  end;
  
  {$endregion Base}
  
  {$region Platform}
  
  ///Представляет платформу OpenCL, объединяющую одно или несколько устройств
  Platform = sealed class(WrapperBase<cl_platform_id, PlatformProperties>)
    private function CreateProp: PlatformProperties; override := new PlatformProperties(ntv);
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_platform_id read ntv;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: PlatformProperties read GetProperties;
    
    {$region constructor's}
    
    ///Создаёт обёртку для указанного неуправляемого объекта
    public constructor(pl: cl_platform_id) :=
    self.ntv := pl;
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    private static _all: IList<Platform>;
    private static function MakePlatformList: IList<Platform>;
    begin
      if _all=nil then
      begin
        var c: UInt32;
        cl.GetPlatformIDs(0, IntPtr.Zero, c).RaiseIfError;
        
        var all_arr := new cl_platform_id[c];
        cl.GetPlatformIDs(c, all_arr[0], IntPtr.Zero).RaiseIfError;
        
        _all := new ReadOnlyCollection<Platform>(all_arr.ConvertAll(pl->new Platform(pl)));
      end;
      Result := _all;
    end;
    ///Возвращает список всех доступных платформ OpenCL
    ///Данный список создаётся 1 раз, при первом обращении
    public static property All: IList<Platform> read MakePlatformList;
    
    {$endregion constructor's}
    
    {$region operator's}
    
    public static function operator=(pl1, pl2: Platform): boolean := pl1.ntv = pl2.ntv;
    public static function operator<>(pl1, pl2: Platform): boolean := pl1.ntv <> pl2.ntv;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}]';
    
    {$endregion operator's}
    
  end;
  
  {$endregion Platform}
  
  {$region Device}
  
  SubDevice = class;
  ///Представляет устройство, поддерживающее OpenCL
  Device = class(WrapperBase<cl_device_id, DeviceProperties>)
    private function CreateProp: DeviceProperties; override := new DeviceProperties(ntv);
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_device_id read ntv;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: DeviceProperties read GetProperties;
    
    ///Возвращает платформу данного устройства
    public property BasePlatform: Platform read new Platform(Properties.GetVal&<cl_platform_id>(DeviceInfo.DEVICE_PLATFORM));
    
    {$region constructor's}
    
    ///Создаёт обёртку для указанного неуправляемого объекта
    public constructor(dvc: cl_device_id) :=
    self.ntv := dvc;
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    ///Собирает массив устройств указанного типа для указанной платформы
    ///Возвращает nil, если ни одно устройство не найдено
    public static function GetAllFor(pl: Platform; t: DeviceType): array of Device;
    begin
      
      var c: UInt32;
      var ec := cl.GetDeviceIDs(pl.Native, t, 0, IntPtr.Zero, c);
      if ec=ErrorCode.DEVICE_NOT_FOUND then exit;
      ec.RaiseIfError;
      
      var all := new cl_device_id[c];
      cl.GetDeviceIDs(pl.Native, t, c, all[0], IntPtr.Zero).RaiseIfError;
      
      Result := all.ConvertAll(dvc->new Device(dvc));
    end;
    ///Собирает массив устройств GPU для указанной платформы
    ///Возвращает nil, если ни одно устройство не найдено
    public static function GetAllFor(pl: Platform) := GetAllFor(pl, DeviceType.DEVICE_TYPE_GPU);
    
    private static supported_split_modes: array of DevicePartitionProperty := nil;
    private function Split(props: array of DevicePartitionProperty): array of SubDevice;
    
    ///Создаёт максимальное возможное количество виртуальных устройств,
    ///каждое из которых содержит CUCount ядер данного устройства
    public function SplitEqually(CUCount: integer): array of SubDevice;
    begin
      if CUCount <= 0 then raise new ArgumentException($'Количество ядер должно быть положительным числом, а не {CUCount}');
      Result := Split(
        new DevicePartitionProperty[](
          DevicePartitionProperty.DEVICE_PARTITION_EQUALLY,
          DevicePartitionProperty.Create(CUCount),
          DevicePartitionProperty.Create(0)
        )
      );
    end;
    
    
    ///Создаёт массив виртуальных устройств, каждое из которых содержит указанное кол-во ядер
    public function SplitByCounts(params CUCounts: array of integer): array of SubDevice;
    begin
      foreach var CUCount in CUCounts do
        if CUCount <= 0 then raise new ArgumentException($'Количество ядер должно быть положительным числом, а не {CUCount}');
      
      var props := new DevicePartitionProperty[CUCounts.Length+2];
      props[0] := DevicePartitionProperty.DEVICE_PARTITION_BY_COUNTS;
      for var i := 0 to CUCounts.Length-1 do
        props[i+1] := new DevicePartitionProperty(CUCounts[i]);
      props[props.Length-1] := DevicePartitionProperty.DEVICE_PARTITION_BY_COUNTS_LIST_END;
      
      Result := Split(props);
    end;
    
    ///Разделяет данное устройство на отдельные группы ядер так,
    ///чтобы у каждой группы ядер был общий кэш указанного уровня
    public function SplitByAffinityDomain(affinity_domain: DeviceAffinityDomain) :=
    Split(
      new DevicePartitionProperty[](
        DevicePartitionProperty.DEVICE_PARTITION_EQUALLY,
        DevicePartitionProperty.Create(new IntPtr(affinity_domain.val)),
        DevicePartitionProperty.Create(0)
      )
    );
    
    {$endregion constructor's}
    
    {$region operator's}
    
    public static function operator=(dvc1, dvc2: Device): boolean := dvc1.ntv = dvc2.ntv;
    public static function operator<>(dvc1, dvc2: Device): boolean := dvc1.ntv <> dvc2.ntv;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}]';
    
    {$endregion operator's}
    
  end;
  ///Представляет виртуальное устройство, использующее часть ядер другого устройства
  ///Объекты данного типа обычно создаются методами "Device.Split*"
  SubDevice = sealed class(Device)
    private _parent: Device;
    ///Возвращает родительское устройство, часть ядер которого использует данное устройство
    public property Parent: Device read _parent;
    
    {$region constructor's}
    
    private constructor(dvc: cl_device_id; parent: Device);
    begin
      inherited Create(dvc);
      self._parent := parent;
    end;
    private constructor := inherited;
    
    protected procedure Finalize; override :=
    cl.ReleaseDevice(ntv).RaiseIfError;
    
    {$endregion constructor's}
    
    {$region operator's}
    
    public static function operator in(sub_dvc: SubDevice; dvc: Device): boolean := sub_dvc.Parent=dvc;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}] of {Parent}';
    
    {$endregion operator's}
    
  end;
  
  {$endregion Device}
  
  {$region Context}
  
  CommandQueueBase = class;
  CommandQueue<T> = class;
  CLTaskBase = class;
  CLTask<T> = class;
  ///Представляет контекст для хранения данных и выполнения команд на GPU
  Context = sealed class(WrapperBase<cl_context, ContextProperties>, IDisposable)
    private dvcs: IList<Device>;
    private main_dvc: Device;
    
    private function CreateProp: ContextProperties; override := new ContextProperties(ntv);
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native:     cl_context    read ntv;
    ///Возвращает список устройств, используемых данным контекстом
    public property AllDevices: IList<Device> read dvcs;
    ///Возвращает главное устройство контекста, на котором выделяется память под буферы и внутренние объекты очередей
    public property MainDevice: Device        read main_dvc;
    
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: ContextProperties read GetProperties;
    
    private function GetAllNtvDevices: array of cl_device_id;
    begin
      Result := new cl_device_id[dvcs.Count];
      for var i := 0 to Result.Length-1 do
        Result[i] := dvcs[i].Native;
    end;
    
    {$region Default}
    
    private static default_need_init := true;
    private static default_init_lock := new object;
    private static _default: Context;
    
    private static function GetDefault: Context;
    begin
      
      if default_need_init then lock default_init_lock do if default_need_init then
      begin
        default_need_init := false;
        _default := MakeNewDefaultContext;
      end;
      
      Result := _default;
    end;
    private static procedure SetDefault(new_default: Context);
    begin
      default_need_init := false;
      _default := new_default;
    end;
    ///Возвращает или задаёт главный контекст, используемый там, где контекст не указывается явно (как неявные очереди)
    ///При первом обращении к данному свойству OpenCLABC пытается создать новый контекст
    ///При создании главного контекста приоритет отдаётся полноценным GPU, но если таких нет - берётся любое устройство, поддерживающее OpenCL
    ///
    ///Если устройств поддерживающих OpenCL нет, то Context.Default изначально будет nil
    ///Но это свидетельствует скорее об отсутствии драйверов, чем отстутсвии устройств
    public static property &Default: Context read GetDefault write SetDefault;
    
    private static function MakeNewDefaultContext: Context;
    begin
      var pls := Platform.All;
      if pls=nil then exit;
      
      foreach var pl in pls do
      begin
        var dvcs := Device.GetAllFor(pl);
        if dvcs=nil then continue;
        Result := new Context(dvcs);
        exit;
      end;
      
      foreach var pl in pls do
      begin
        var dvcs := Device.GetAllFor(pl, DeviceType.DEVICE_TYPE_ALL);
        if dvcs=nil then continue;
        Result := new Context(dvcs);
        exit;
      end;
      
      Result := nil;
    end;
    
    {$endregion Default}
    
    {$region constructor's}
    
    private static procedure CheckMainDevice(main_dvc: Device; dvc_lst: IList<Device>) :=
    if not dvc_lst.Contains(main_dvc) then raise new ArgumentException($'main_dvc должен быть в списке устройств контекста');
    
    ///Создаёт контекст с указанными AllDevices и MainDevice
    public constructor(dvcs: IList<Device>; main_dvc: Device);
    begin
      CheckMainDevice(main_dvc, dvcs);
      
      var ntv_dvcs := new cl_device_id[dvcs.Count];
      for var i := 0 to ntv_dvcs.Length-1 do
        ntv_dvcs[i] := dvcs[i].Native;
      
      var ec: ErrorCode;
      //ToDo позволить использовать CL_CONTEXT_INTEROP_USER_SYNC в свойствах
      self.ntv := cl.CreateContext(nil, ntv_dvcs.Count, ntv_dvcs, nil, IntPtr.Zero, ec);
      ec.RaiseIfError;
      
      self.dvcs := new ReadOnlyCollection<Device>(dvcs);
      self.main_dvc := main_dvc;
    end;
    ///Создаёт контекст с указанными AllDevices
    ///В качестве MainDevice берётся первое устройство из массива
    public constructor(params dvcs: array of Device) := Create(dvcs, dvcs[0]);
    
    private static function GetContextDevices(ntv: cl_context): array of Device;
    begin
      
      var sz: UIntPtr;
      cl.GetContextInfo(ntv, ContextInfo.CONTEXT_DEVICES, UIntPtr.Zero, nil, sz).RaiseIfError;
      
      var res := new cl_device_id[uint64(sz) div Marshal.SizeOf&<cl_device_id>];
      cl.GetContextInfo(ntv, ContextInfo.CONTEXT_DEVICES, sz, res[0], IntPtr.Zero).RaiseIfError;
      
      Result := res.ConvertAll(dvc->new Device(dvc));
    end;
    private procedure InitFromNtv(ntv: cl_context; dvcs: IList<Device>; main_dvc: Device);
    begin
      CheckMainDevice(main_dvc, dvcs);
      cl.RetainContext(ntv).RaiseIfError;
      self.ntv := ntv;
      self.dvcs := new ReadOnlyCollection<Device>(dvcs);
      self.main_dvc := main_dvc;
    end;
    ///Создаёт обёртку для указанного неуправляемого объекта
    ///При успешном создании обёртки вызывается cl.Retain
    ///А во время вызова .Dispose - cl.Release
    public constructor(ntv: cl_context; main_dvc: Device) :=
    InitFromNtv(ntv, GetContextDevices(ntv), main_dvc);
    
    ///Создаёт обёртку для указанного неуправляемого объекта
    ///При успешном создании обёртки вызывается cl.Retain
    ///А во время вызова .Dispose - cl.Release
    public constructor(ntv: cl_context);
    begin
      var dvcs := GetContextDevices(ntv);
      InitFromNtv(ntv, dvcs, dvcs[0]);
    end;
    
    private constructor(c: Context; main_dvc: Device) :=
    InitFromNtv(c.ntv, c.dvcs, main_dvc);
    ///Создаёт совместимый контекст, равный данному с одним отличием - MainDevice заменён на dvc
    public function MakeSibling(new_main_dvc: Device) := new Context(self, new_main_dvc);
    
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose :=
    if ntv<>cl_context.Zero then lock self do
    begin
      cl.ReleaseContext(ntv).RaiseIfError;
      ntv := cl_context.Zero;
    end;
    protected procedure Finalize; override := Dispose;
    
    {$endregion constructor's}
    
    {$region operator's}
    
    public static function operator=(c1, c2: Context): boolean := c1.ntv = c2.ntv;
    public static function operator<>(c1, c2: Context): boolean := c1.ntv <> c2.ntv;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}] on devices: {AllDevices.JoinToString('', '')}; Main device: {MainDevice}';
    
    {$endregion operator's}
    
    {$region Invoke}
    
    ///Запускает данную очередь и все её подочереди
    ///Как только всё запущено: возвращает объект типа CLTask<>, через который можно следить за процессом выполнения
    public function BeginInvoke<T>(q: CommandQueue<T>): CLTask<T>;
    ///Запускает данную очередь и все её подочереди
    ///Как только всё запущено: возвращает объект типа CLTask<>, через который можно следить за процессом выполнения
    public function BeginInvoke(q: CommandQueueBase): CLTaskBase;
    
    ///Запускает данную очередь и все её подочереди
    ///Затем ожидает окончания выполнения и возвращает полученный результат
    public function SyncInvoke<T>(q: CommandQueue<T>): T;
    ///Запускает данную очередь и все её подочереди
    ///Затем ожидает окончания выполнения и возвращает полученный результат
    public function SyncInvoke(q: CommandQueueBase): Object;
    
    {$endregion Invoke}
    
  end;
  
  {$endregion Context}
  
  {$region Buffer}
  
  BufferCommandQueue = class;
  ///Представляет область памяти устройства OpenCL
  Buffer = class(WrapperBase<cl_mem, BufferProperties>, IDisposable)
    private sz: UIntPtr;
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_mem read ntv;
    
    private function CreateProp: BufferProperties; override;
    begin
      if ntv=cl_mem.Zero then raise new InvalidOperationException($'Ожидался инициализированный буфер. Используйте .Init, или конструктор принимающий контекст');
      Result := new BufferProperties(ntv);
    end;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: BufferProperties read GetProperties;
    
    ///Возвращает размер буфера в байтах
    public property Size: UIntPtr read sz;
    ///Возвращает размер буфера в байтах
    public property Size32: UInt32 read sz.ToUInt32;
    ///Возвращает размер буфера в байтах
    public property Size64: UInt64 read sz.ToUInt64;
    
    ///Создаёт новую очередь-контейнер для команд GPU, применяемых к данному буферу
    public function NewQueue: BufferCommandQueue;
    
    {$region constructor's}
    
    ///Создаёт буфер указанного в байтах размера
    ///Память на GPU не выделяется до вызова метода .Init
    public constructor(size: UIntPtr) := self.sz := size;
    ///Создаёт буфер указанного в байтах размера
    ///Память на GPU не выделяется до вызова метода .Init
    public constructor(size: integer) := Create(new UIntPtr(size));
    ///Создаёт буфер указанного в байтах размера
    ///Память на GPU не выделяется до вызова метода .Init
    public constructor(size: int64)   := Create(new UIntPtr(size));
    
    ///Создаёт буфер указанного в байтах размера
    ///Память на GPU выделяется сразу, на явно указанном контексте
    public constructor(size: UIntPtr; c: Context);
    begin
      Create(size);
      Init(c);
    end;
    ///Создаёт буфер указанного в байтах размера
    ///Память на GPU выделяется сразу, на явно указанном контексте
    public constructor(size: integer; c: Context) := Create(new UIntPtr(size), c);
    ///Создаёт буфер указанного в байтах размера
    ///Память на GPU выделяется сразу, на явно указанном контексте
    public constructor(size: int64; c: Context)   := Create(new UIntPtr(size), c);
    
    ///Создаёт обёртку для указанного неуправляемого объекта
    ///При успешном создании обёртки вызывается cl.Retain
    ///А во время вызова .Dispose - cl.Release
    protected constructor(ntv: cl_mem);
    begin
      cl.RetainMemObject(ntv).RaiseIfError;
      self.ntv := ntv;
      
      cl.GetMemObjectInfo(ntv, MemInfo.MEM_SIZE, new UIntPtr(Marshal.SizeOf&<UIntPtr>), self.sz, IntPtr.Zero).RaiseIfError;
      GC.AddMemoryPressure(Size64);
      
    end;
    
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    ///Выделяет память для данного буфера в указанном контексте
    ///Если память уже выделена, то она освобождается и выделяется заново
    public procedure Init(c: Context); virtual :=
    lock self do
    begin
      
      var ec: ErrorCode;
      var new_ntv := cl.CreateBuffer(c.Native, MemFlags.MEM_READ_WRITE, sz, IntPtr.Zero, ec);
      ec.RaiseIfError;
      
      if self.ntv=cl_mem.Zero then
        GC.AddMemoryPressure(Size64) else
        cl.ReleaseMemObject(self.ntv).RaiseIfError;
      
      self.ntv := new_ntv;
    end;
    
    ///Выделяет память для данного буфера в указанном контексте
    ///Если память уже выделена, то данный методы ничего не делает
    public procedure InitIfNeed(c: Context); virtual :=
    if self.ntv=cl_mem.Zero then lock self do
    begin
      if self.ntv<>cl_mem.Zero then exit; // Во время ожидания lock могли инициализировать
      
      var ec: ErrorCode;
      var new_ntv := cl.CreateBuffer(c.Native, MemFlags.MEM_READ_WRITE, sz, IntPtr.Zero, ec);
      ec.RaiseIfError;
      
      GC.AddMemoryPressure(Size64);
      self.ntv := new_ntv;
    end;
    
    ///Освобождает память, выделенную под данный буфер, если она выделена
    ///Внимание, если снова использовать данный буфер - память выделится заново
    public procedure Dispose; virtual :=
    if ntv<>cl_mem.Zero then lock self do
    begin
      if self.ntv=cl_mem.Zero then exit; // Во время ожидания lock могли удалить
      self._properties := nil;
      GC.RemoveMemoryPressure(Size64);
      cl.ReleaseMemObject(ntv).RaiseIfError;
      ntv := cl_mem.Zero;
    end;
    protected procedure Finalize; override := Dispose;
    
    {$endregion constructor's}
    
    {$region operator's}
    
    public static function operator=(b1, b2: Buffer): boolean := b1.ntv = b2.ntv;
    public static function operator<>(b1, b2: Buffer): boolean := b1.ntv <> b2.ntv;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}] of size {Size}';
    
    {$endregion operator's}
    
    {$region 1#Write&Read}
    
    ///Заполняет весь буфер данными, находящимися по указанному адресу в RAM
    public function WriteData(ptr: CommandQueue<IntPtr>): Buffer;
    
    ///Копирует всё содержимое буфера в RAM, по указанному адресу
    public function ReadData(ptr: CommandQueue<IntPtr>): Buffer;
    
    ///Заполняет часть буфер данными, находящимися по указанному адресу в RAM
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function WriteData(ptr: CommandQueue<IntPtr>; buff_offset, len: CommandQueue<integer>): Buffer;
    
    ///Копирует часть содержимого буфера в RAM, по указанному адресу
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function ReadData(ptr: CommandQueue<IntPtr>; buff_offset, len: CommandQueue<integer>): Buffer;
    
    ///Заполняет весь буфер данными, находящимися по указанному адресу в RAM
    public function WriteData(ptr: pointer): Buffer;
    
    ///Копирует всё содержимое буфера в RAM, по указанному адресу
    public function ReadData(ptr: pointer): Buffer;
    
    ///Заполняет часть буфер данными, находящимися по указанному адресу в RAM
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function WriteData(ptr: pointer; buff_offset, len: CommandQueue<integer>): Buffer;
    
    ///Копирует часть содержимого буфера в RAM, по указанному адресу
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function ReadData(ptr: pointer; buff_offset, len: CommandQueue<integer>): Buffer;
    
    ///Записывает указанное значение размерного типа в начало буфера
    public function WriteValue<TRecord>(val: TRecord): Buffer; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в буфер
    ///buff_offset указывает отступ от начала буфера, в байтах
    public function WriteValue<TRecord>(val: TRecord; buff_offset: CommandQueue<integer>): Buffer; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в начало буфера
    public function WriteValue<TRecord>(val: CommandQueue<TRecord>): Buffer; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в буфер
    ///buff_offset указывает отступ от начала буфера, в байтах
    public function WriteValue<TRecord>(val: CommandQueue<TRecord>; buff_offset: CommandQueue<integer>): Buffer; where TRecord: record;
    
    ///Записывает весь массив в начало буфера
    public function WriteArray1<TRecord>(a: CommandQueue<array of TRecord>): Buffer; where TRecord: record;
    
    ///Записывает весь массив в начало буфера
    public function WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): Buffer; where TRecord: record;
    
    ///Записывает весь массив в начало буфера
    public function WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): Buffer; where TRecord: record;
    
    ///Читает из буфера достаточно байт чтоб заполнить весь массив
    public function ReadArray1<TRecord>(a: CommandQueue<array of TRecord>): Buffer; where TRecord: record;
    
    ///Читает из буфера достаточно байт чтоб заполнить весь массив
    public function ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): Buffer; where TRecord: record;
    
    ///Читает из буфера достаточно байт чтоб заполнить весь массив
    public function ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): Buffer; where TRecord: record;
    
    ///Записывает указанный участок массива в буфер
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    public function WriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, buff_offset: CommandQueue<integer>): Buffer; where TRecord: record;
    
    ///Записывает указанный участок массива в буфер
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, buff_offset: CommandQueue<integer>): Buffer; where TRecord: record;
    
    ///Записывает указанный участок массива в буфер
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, buff_offset: CommandQueue<integer>): Buffer; where TRecord: record;
    
    ///Читает в буфер указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    public function ReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, buff_offset: CommandQueue<integer>): Buffer; where TRecord: record;
    
    ///Читает в буфер указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, buff_offset: CommandQueue<integer>): Buffer; where TRecord: record;
    
    ///Читает в буфер указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, buff_offset: CommandQueue<integer>): Buffer; where TRecord: record;
    
    {$endregion 1#Write&Read}
    
    {$region 2#Fill}
    
    ///Читает pattern_len байт из RAM по указанному адресу и заполняет их копиями весь буфер
    public function FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): Buffer;
    
    ///Читает pattern_len байт из RAM по указанному адресу и заполняет их копиями часть буфера
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function FillData(ptr: CommandQueue<IntPtr>; pattern_len, buff_offset, len: CommandQueue<integer>): Buffer;
    
    ///Заполняет весь буфер копиями указанного значения размерного типа
    public function FillValue<TRecord>(val: TRecord): Buffer; where TRecord: record;
    
    ///Заполняет часть буфера копиями указанного значения размерного типа
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function FillValue<TRecord>(val: TRecord; buff_offset, len: CommandQueue<integer>): Buffer; where TRecord: record;
    
    ///Заполняет весь буфер копиями указанного значения размерного типа
    public function FillValue<TRecord>(val: CommandQueue<TRecord>): Buffer; where TRecord: record;
    
    ///Заполняет часть буфера копиями указанного значения размерного типа
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function FillValue<TRecord>(val: CommandQueue<TRecord>; buff_offset, len: CommandQueue<integer>): Buffer; where TRecord: record;
    
    {$endregion 2#Fill}
    
    {$region 3#Copy}
    
    ///Копирует данные из текущего буфера в b
    ///Если буферы имеют разный размер - в качестве объёма данных берётся размер меньшего буфера
    public function CopyTo(b: CommandQueue<Buffer>): Buffer;
    
    ///Копирует данные из b в текущий буфер
    ///Если буферы имеют разный размер - в качестве объёма данных берётся размер меньшего буфера
    public function CopyForm(b: CommandQueue<Buffer>): Buffer;
    
    ///Копирует данные из текущего буфера в b
    ///from_pos указывает отступ в байтах от начала буфера, из которого копируют
    ///to_pos указывает отступ в байтах от начала буфера, в который копируют
    ///len указывает кол-во копируемых байт
    public function CopyTo(b: CommandQueue<Buffer>; from_pos, to_pos, len: CommandQueue<integer>): Buffer;
    
    ///Копирует данные из b в текущий буфер
    ///from_pos указывает отступ в байтах от начала буфера, из которого копируют
    ///to_pos указывает отступ в байтах от начала буфера, в который копируют
    ///len указывает кол-во копируемых байт
    public function CopyForm(b: CommandQueue<Buffer>; from_pos, to_pos, len: CommandQueue<integer>): Buffer;
    
    {$endregion 3#Copy}
    
    {$region Get}
    
    ///Выделяет область неуправляемой памяти и копирует в неё всё содержимое данного буфера
    public function GetData: IntPtr;
    
    ///Выделяет область неуправляемой памяти и копирует в неё часть содержимого данного буфера
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function GetData(buff_offset, len: CommandQueue<integer>): IntPtr;
    
    ///Читает значение указанного размерного типа из начала буфера
    public function GetValue<TRecord>: TRecord; where TRecord: record;
    
    ///Читает значение указанного размерного типа из буфера
    ///buff_offset указывает отступ от начала буфера, в байтах
    public function GetValue<TRecord>(buff_offset: CommandQueue<integer>): TRecord; where TRecord: record;
    
    ///Создаёт массив максимального размера (на сколько хватит байт буфера) и копирует в него содержимое буфера
    public function GetArray1<TRecord>: array of TRecord; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое буфера
    public function GetArray1<TRecord>(len: CommandQueue<integer>): array of TRecord; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое буфера
    public function GetArray2<TRecord>(len1,len2: CommandQueue<integer>): array[,] of TRecord; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое буфера
    public function GetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): array[,,] of TRecord; where TRecord: record;
    
    {$endregion Get}
    
  end;
  ///Представляет область памяти внутри другого буфера
  SubBuffer = sealed class(Buffer)
    private _parent: Buffer;
    ///Возвращает родительский буфер
    public property Parent: Buffer read _parent;
    
    {$region operator's}
    
    public static function operator in(sub_b: SubBuffer; b: Buffer): boolean := sub_b.Parent=b;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}] of size {Size} inside {Parent}';
    
    {$endregion operator's}
    
    {$region constructor's}
    
    protected constructor(parent: Buffer; reg: cl_buffer_region);
    begin
      inherited Create(reg.size);
      
      var parent_ntv := parent.Native;
      if parent_ntv=cl_mem.Zero then raise new InvalidOperationException($'Ожидался инициализированный буфер. Используйте .Init, или конструктор принимающий контекст');
      
      var ec: ErrorCode;
      self.ntv := cl.CreateSubBuffer(parent_ntv, MemFlags.MEM_READ_WRITE, BufferCreateType.BUFFER_CREATE_TYPE_REGION, reg, ec);
      ec.RaiseIfError;
      
      self._parent := parent;
    end;
    ///Создаёт буфер из области памяти родительского буфера parent
    ///origin указывает отступ в байтах от начала parent
    ///size указывает размер нового буфера
    ///Память parent должна быть выделена перед вызовом данного конструктора,
    ///потому что новый буфер будет использовать память parent, вместо создания новой области памяти
    public constructor(parent: Buffer; origin, size: UIntPtr) := Create(parent, new cl_buffer_region(origin, size));
    
    ///Создаёт буфер из области памяти родительского буфера parent
    ///origin указывает отступ в байтах от начала parent
    ///size указывает размер нового буфера
    ///Память parent должна быть выделена перед вызовом данного конструктора,
    ///потому что новый буфер будет использовать память parent, вместо создания новой области памяти
    public constructor(parent: Buffer; origin, size: UInt32) := Create(parent, new UIntPtr(origin), new UIntPtr(size));
    ///Создаёт буфер из области памяти родительского буфера parent
    ///origin указывает отступ в байтах от начала parent
    ///size указывает размер нового буфера
    ///Память parent должна быть выделена перед вызовом данного конструктора,
    ///потому что новый буфер будет использовать память parent, вместо создания новой области памяти
    public constructor(parent: Buffer; origin, size: UInt64) := Create(parent, new UIntPtr(origin), new UIntPtr(size));
    
    private procedure InitIgnoreOrErr :=
    if self.ntv=cl_mem.Zero then raise new NotSupportedException($'SubBuffer нельзя инициализировать, потому что он использует память другого буфера');
    ///--
    public procedure Init(c: Context); override := InitIgnoreOrErr;
    ///--
    public procedure InitIfNeed(c: Context); override := InitIgnoreOrErr;
    
    ///--
    public procedure Dispose; override :=
    if ntv<>cl_mem.Zero then lock self do
    begin
      if self.ntv=cl_mem.Zero then exit; // Во время ожидания lock могли удалить
      self._properties := nil;
      cl.ReleaseMemObject(ntv).RaiseIfError;
      ntv := cl_mem.Zero;
    end;
    
    {$endregion constructor's}
    
  end;
  
  {$endregion Buffer}
  
  {$region Kernel}
  
  KernelCommandQueue = class;
  KernelArg = class;
  ///Представляет подпрограмму, выполняемую на GPU
  Kernel = sealed class(WrapperBase<cl_kernel, KernelProperties>, IDisposable)
    private function CreateProp: KernelProperties; override := new KernelProperties(ntv);
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_kernel read ntv;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: KernelProperties read GetProperties;
    
    private _prog: cl_program;
    private _name: string;
    ///Возвращает имя данной подпрограммы
    public property Name: string read _name;
    
    ///Создаёт новую очередь-контейнер для команд GPU, применяемых к данному kernel-у
    public function NewQueue: KernelCommandQueue;
    
    {$region constructor's}
    
    private function MakeNewNtv: cl_kernel;
    begin
      var ec: ErrorCode;
      Result := cl.CreateKernel(_prog, _name, ec);
      ec.RaiseIfError;
    end;
    protected constructor(prog: cl_program; name: string);
    begin
      self._prog := prog;
      self._name := name;
      self.ntv := self.MakeNewNtv;
    end;
    
    ///Создаёт обёртку для указанного неуправляемого объекта
    ///При успешном создании обёртки вызывается cl.Retain
    ///А во время вызова .Dispose - cl.Release
    public constructor(ntv: cl_kernel; retain: boolean := true);
    begin
      
      cl.GetKernelInfo(ntv, KernelInfo.KERNEL_PROGRAM, new UIntPtr(cl_program.Size), self._prog, IntPtr.Zero).RaiseIfError;
      
      var sz: UIntPtr;
      cl.GetKernelInfo(ntv, KernelInfo.KERNEL_FUNCTION_NAME, UIntPtr.Zero, nil, sz).RaiseIfError;
      var str_ptr := Marshal.AllocHGlobal(IntPtr(pointer(sz)));
      try
        cl.GetKernelInfo(ntv, KernelInfo.KERNEL_FUNCTION_NAME, sz, str_ptr, IntPtr.Zero).RaiseIfError;
        self._name := Marshal.PtrToStringAnsi(str_ptr);
      finally
        Marshal.FreeHGlobal(str_ptr);
      end;
      
      if retain then cl.RetainKernel(ntv).RaiseIfError;
      self.ntv := ntv;
    end;
    
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose :=
    if ntv<>cl_kernel.Zero then lock self do
    begin
      if ntv=cl_kernel.Zero then exit;
      cl.ReleaseKernel(ntv).RaiseIfError;
      ntv := cl_kernel.Zero;
    end;
    protected procedure Finalize; override := Dispose;
    
    {$endregion constructor's}
    
    {$region operator's}
    
    public static function operator=(k1, k2: Kernel): boolean := k1.ntv = k2.ntv;
    public static function operator<>(k1, k2: Kernel): boolean := k1.ntv <> k2.ntv;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{Name}:{ntv.val}]';
    
    {$endregion operator's}
    
    {$region UseExclusiveNative}
    
    private exclusive_ntv_lock := new object;
    ///Гарантирует что неуправляемый объект будет использоваться только в 1 потоке одновременно
    ///Если неуправляемый объект данного kernel-а используется другим потоком - в процедурную переменную передаётся его независимый клон
    ///Внимание, клон неуправляемого объекта будет удалён сразу после выхода из вашей процедурной переменной, если не вызвать cl.RetainKernel
    public procedure UseExclusiveNative(p: cl_kernel->());
    begin
      var owned := Monitor.TryEnter(exclusive_ntv_lock);
      var k: cl_kernel;
      try
        k := owned ? ntv : MakeNewNtv;
        p(k);
      finally
        if owned then
          Monitor.Exit(exclusive_ntv_lock) else
          cl.ReleaseKernel(k).RaiseIfError;
      end;
    end;
    ///Гарантирует что неуправляемый объект будет использоваться только в 1 потоке одновременно
    ///Если неуправляемый объект данного kernel-а используется другим потоком - в процедурную переменную передаётся его независимый клон
    ///Внимание, клон неуправляемого объекта будет удалён сразу после выхода из вашей процедурной переменной, если не вызвать cl.RetainKernel
    public function UseExclusiveNative<T>(f: cl_kernel->T): T;
    begin
      var owned := Monitor.TryEnter(exclusive_ntv_lock);
      var k: cl_kernel;
      try
        k := owned ? ntv : MakeNewNtv;
        Result := f(k);
      finally
        if owned then
          Monitor.Exit(exclusive_ntv_lock) else
          cl.ReleaseKernel(k).RaiseIfError;
      end;
    end;
    
    {$endregion UseExclusiveNative}
    
    {$region 1#Exec}
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function Exec1(sz1: CommandQueue<integer>; params args: array of KernelArg): Kernel;
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function Exec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): Kernel;
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function Exec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): Kernel;
    
    ///Выполняет kernel с расширенным набором параметров
    ///Данная перегрузка используется в первую очередь для тонких оптимизаций
    ///Если она вам понадобилась по другой причина - пожалуйста, напишите в issue
    public function Exec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): Kernel;
    
    {$endregion 1#Exec}
    
  end;
  
  {$endregion Kernel}
  
  {$region ProgramCode}
  
  ///Представляет контейнер с откомпилированным кодом для GPU, содержащим подпрограммы-kernel'ы
  ProgramCode = sealed class(WrapperBase<cl_program, ProgramProperties>)
    private function CreateProp: ProgramProperties; override := new ProgramProperties(ntv);
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_program read ntv;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: ProgramProperties read GetProperties;
    
    protected _c: Context;
    ///Возвращает контекст, на котором компилировали данный код для GPU
    public property BaseContext: Context read _c;
    
    {$region constructor's}
    
    private procedure Build;
    begin
      
      var ec := cl.BuildProgram(self.ntv, _c.dvcs.Count,_c.GetAllNtvDevices, nil, nil,IntPtr.Zero);
      if ec=ErrorCode.BUILD_PROGRAM_FAILURE then
      begin
        var sb := new StringBuilder($'Ошибка компиляции OpenCL программы:');
        
        foreach var dvc in _c.AllDevices do
        begin
          sb += #10#10;
          sb += dvc.ToString;
          sb += ':'#10;
          
          var sz: UIntPtr;
          cl.GetProgramBuildInfo(self.ntv, dvc.Native, ProgramBuildInfo.PROGRAM_BUILD_LOG, UIntPtr.Zero,IntPtr.Zero,sz).RaiseIfError;
          
          var str_ptr := Marshal.AllocHGlobal(IntPtr(pointer(sz)));
          try
            cl.GetProgramBuildInfo(self.ntv, dvc.Native, ProgramBuildInfo.PROGRAM_BUILD_LOG, sz,str_ptr,IntPtr.Zero).RaiseIfError;
            sb += Marshal.PtrToStringAnsi(str_ptr);
          finally
            Marshal.FreeHGlobal(str_ptr);
          end;
          
        end;
        
        raise new OpenCLException(ec, sb.ToString);
      end else
        ec.RaiseIfError;
      
    end;
    
    ///Компилирует указанные тексты программ на указанном контексте
    ///Внимание! Именно тексты, Не имена файлов
    public constructor(c: Context; params files_texts: array of string);
    begin
      
      var ec: ErrorCode;
      self.ntv := cl.CreateProgramWithSource(c.Native, files_texts.Length, files_texts, nil, ec);
      ec.RaiseIfError;
      
      self._c := c;
      self.Build;
      
    end;
    
    private constructor(ntv: cl_program; c: Context);
    begin
      cl.RetainProgram(ntv).RaiseIfError;
      self._c := c;
      self.ntv := ntv;
    end;
    
    private static function GetProgContext(ntv: cl_program): Context;
    begin
      var c: cl_context;
      cl.GetProgramInfo(ntv, ProgramInfo.PROGRAM_CONTEXT, new UIntPtr(Marshal.SizeOf&<cl_context>), c, IntPtr.Zero).RaiseIfError;
      Result := new Context(c);
    end;
    ///Создаёт обёртку для указанного неуправляемого объекта
    ///При успешном создании обёртки вызывается cl.Retain
    ///А во время вызова .Dispose - cl.Release
    public constructor(ntv: cl_program) :=
    Create(ntv, GetProgContext(ntv));
    
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose :=
    if ntv<>cl_program.Zero then lock self do
    begin
      if ntv=cl_program.Zero then exit;
      cl.ReleaseProgram(ntv).RaiseIfError;
      ntv := cl_program.Zero;
    end;
    protected procedure Finalize; override := Dispose;
    
    {$endregion constructor's}
    
    {$region operator's}
    
    public static function operator=(code1, code2: ProgramCode): boolean := code1.ntv = code2.ntv;
    public static function operator<>(code1, code2: ProgramCode): boolean := code1.ntv <> code2.ntv;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}]';
    
    {$endregion operator's}
    
    {$region GetKernel}
    
    ///Находит в коде kernel с указанным именем
    ///Регистр имени важен!
    public property KernelByName[kname: string]: Kernel read new Kernel(ntv, kname); default;
    
    ///Создаёт массив из всех kernel-ов данного кода
    public function GetAllKernels: array of Kernel;
    begin
      
      var c: UInt32;
      cl.CreateKernelsInProgram(ntv, 0, IntPtr.Zero, c).RaiseIfError;
      
      var res := new cl_kernel[c];
      cl.CreateKernelsInProgram(ntv, c, res[0], IntPtr.Zero).RaiseIfError;
      
      Result := res.ConvertAll(k->new Kernel(k, false));
    end;
    
    {$endregion GetKernel}
    
    {$region Serialize}
    
    ///Сохраняет прекомпилированную программу как набор байт
    public function Serialize: array of array of byte :=
    self.Properties.Binaries;
    
    ///Сохраняет прекомпилированную программу в поток
    public procedure SerializeTo(bw: System.IO.BinaryWriter);
    begin
      var bin := Serialize;
      
      bw.Write(bin.Length);
      foreach var a in bin do
      begin
        bw.Write(a.Length);
        bw.Write(a);
      end;
      
    end;
    ///Сохраняет прекомпилированную программу в поток
    public procedure SerializeTo(str: System.IO.Stream) :=
    SerializeTo(new System.IO.BinaryWriter(str));
    
    {$endregion Serialize}
    
    {$region Deserialize}
    
    ///Загружает прекомпилированную программу из набора байт
    public static function Deserialize(c: Context; bin: array of array of byte): ProgramCode;
    begin
      var ntv: cl_program;
      
      var dvcs := c.GetAllNtvDevices;
      
      var ec: ErrorCode;
      ntv := cl.CreateProgramWithBinary(
        c.Native, c.AllDevices.Count, dvcs[0],
        bin.ConvertAll(a->new UIntPtr(a.Length))[0], bin,
        IntPtr.Zero, ec
      );
      ec.RaiseIfError;
      
      Result := new ProgramCode(ntv, c);
      Result.Build;
      
    end;
    
    ///Загружает прекомпилированную программу из потока
    public static function DeserializeFrom(c: Context; br: System.IO.BinaryReader): ProgramCode;
    begin
      var bin: array of array of byte;
      
      SetLength(bin, br.ReadInt32);
      for var i := 0 to bin.Length-1 do
      begin
        var len := br.ReadInt32;
        bin[i] := br.ReadBytes(len);
        if bin[i].Length<>len then raise new System.IO.EndOfStreamException;
      end;
      
      Result := Deserialize(c, bin);
    end;
    ///Загружает прекомпилированную программу из потока
    public static function DeserializeFrom(c: Context; str: System.IO.Stream) :=
    DeserializeFrom(c, new System.IO.BinaryReader(str));
    
    {$endregion Deserialize}
    
  end;
  
  {$endregion ProgramCode}
  
  {$endregion Wrappers}
  
  {$region Util type's}
  
  {$region EventDebug}{$ifdef EventDebug}
  
  EventRetainReleaseData = record
    private is_release: boolean;
    private reason: string;
    
    private static debug_time_counter := Stopwatch.StartNew;
    private time: TimeSpan;
    
    public constructor(is_release: boolean; reason: string);
    begin
      self.is_release := is_release;
      self.reason := reason;
      self.time := debug_time_counter.Elapsed;
    end;
    
    private function GetActStr := is_release ? 'Released' : 'Retained';
    public function ToString: string; override :=
    $'{time} | {GetActStr} when: {reason}';
    
  end;
  EventDebug = static class
    
    {$region Retain/Release}
    
    private static RefCounter := new Dictionary<cl_event, List<EventRetainReleaseData>>;
    private static function RefCounterFor(ev: cl_event): List<EventRetainReleaseData>;
    begin
      lock RefCounter do
        if not RefCounter.TryGetValue(ev, Result) then
        begin
          Result := new List<EventRetainReleaseData>;
          RefCounter[ev] := Result;
        end;
    end;
    
    public static procedure RegisterEventRetain(ev: cl_event; reason: string);
    begin
      var lst := RefCounterFor(ev);
      lock lst do lst += new EventRetainReleaseData(false, reason);
    end;
    public static procedure RegisterEventRelease(ev: cl_event; reason: string);
    begin
      var lst := RefCounterFor(ev);
      lock lst do lst += new EventRetainReleaseData(true, reason);
    end;
    
    public static procedure ReportRefCounterInfo :=
    lock output do lock RefCounter do
    begin
      
      foreach var ev in RefCounter.Keys do
      begin
        $'Logging state change of {ev}'.Println;
        var lst := RefCounter[ev];
        var c := 0;
        lock lst do
          foreach var act in lst do
          begin
            if act.is_release then
              c -= 1 else
              c += 1;
            $'{c,3} | {act}'.Println;
          end;
        Writeln('-'*30);
      end;
      
      Writeln('='*40);
    end;
    
    {$endregion Retain/Release}
    
  end;
  
  {$endif EventDebug}{$endregion EventDebug}
  
  {$region CLTaskExt}
  
  UserEvent = class;
  CLTaskExt = static class
    
    static procedure AddErr(tsk: CLTaskBase; e: Exception);
    static function AddErr(tsk: CLTaskBase; ec: ErrorCode): boolean;
    static function AddErr(tsk: CLTaskBase; st: CommandExecutionStatus): boolean;
    
  end;
  
  {$endregion CLTaskExt}
  
  {$region NativeUtils}
  
  NativeUtils = static class
    public static AbortStatus := new CommandExecutionStatus(integer.MinValue);
    
    public static function CopyToUnm<TRecord>(a: TRecord): IntPtr; where TRecord: record;
    begin
      Result := Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>);
      var res: ^TRecord := pointer(Result);
      res^ := a;
    end;
    
    public static function AsPtr<T>(p: pointer): ^T := p;
    public static function AsPtr<T>(p: IntPtr) := AsPtr&<T>(pointer(p));
    
    public static function GCHndAlloc(o: object) :=
    CopyToUnm(GCHandle.Alloc(o));
    
    public static procedure GCHndFree(gc_hnd_ptr: IntPtr);
    begin
      AsPtr&<GCHandle>(gc_hnd_ptr)^.Free;
      Marshal.FreeHGlobal(gc_hnd_ptr);
    end;
    
    public static function StartNewThread(p: Action): Thread;
    begin
      Result := new Thread(p);
      Result.IsBackground := true;
      Result.Start;
    end;
    
    protected static procedure FixCQ(c: cl_context; dvc: cl_device_id; var cq: cl_command_queue);
    begin
      if cq <> cl_command_queue.Zero then exit;
      var ec: ErrorCode;
      cq := cl.CreateCommandQueue(c, dvc, CommandQueueProperties.NONE, ec);
      ec.RaiseIfError;
    end;
    
  end;
  
  {$endregion NativeUtils}
  
  {$region Blittable}
  
  BlittableException = sealed class(Exception)
    public constructor(t, blame: System.Type; source_name: string) :=
    inherited Create(t=blame ? $'Тип {t} нельзя использовать в {source_name}' : $'Тип {t} нельзя использовать в {source_name}, потому что он содержит тип {blame}' );
  end;
  BlittableHelper = static class
    
    private static blittable_cache := new Dictionary<System.Type, System.Type>;
    public static function Blame(t: System.Type): System.Type;
    begin
      if t.IsPointer then exit;
      if t.IsClass then
      begin
        Result := t;
        exit;
      end;
      
      //ToDo протестировать - может быстрее будет без blittable_cache, потому что всё заинлайнится?
      if blittable_cache.TryGetValue(t, Result) then exit;
      
      foreach var fld in t.GetFields(System.Reflection.BindingFlags.Instance or System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.NonPublic) do
        if fld.FieldType<>t then
        begin
          Result := Blame(fld.FieldType);
          if Result<>nil then break;
        end;
      
      blittable_cache[t] := Result;
    end;
    
    public static function IsBlittable(t: System.Type) := Blame(t)=nil;
    public static procedure RaiseIfNeed(t: System.Type; source_name: string);
    begin
      var blame := BlittableHelper.Blame(t);
      if blame=nil then exit;
      raise new BlittableException(t, blame, source_name);
    end;
    
  end;
  
  {$endregion Blittable}
  
  {$region EventList}
  
  EventList = sealed class
    public evs: array of cl_event;
    public count := 0;
    public abortable := false; // true только если можно моментально отменить
    
    {$region Misc}
    
    public property Item[i: integer]: cl_event read evs[i]; default;
    
    public static function operator=(l1, l2: EventList): boolean;
    begin
      if object.ReferenceEquals(l1, l2) then
      begin
        Result := true;
        exit;
      end;
      if object.ReferenceEquals(l1, nil) then exit;
      if object.ReferenceEquals(l2, nil) then exit;
      if l1.count <> l2.count then exit;
      for var i := 0 to l1.count-1 do
        if l1[i]<>l2[i] then exit;
      Result := true;
    end;
    public static function operator<>(l1, l2: EventList): boolean := not (l1=l2);
    
    {$endregion Misc}
    
    {$region constructor's}
    
    public constructor := exit;
    public constructor(count: integer) :=
    if count<>0 then self.evs := new cl_event[count];
    
    public static function operator implicit(ev: cl_event): EventList;
    begin
      if ev=cl_event.Zero then
        Result := new EventList else
      begin
        Result := new EventList(1);
        Result += ev;
      end;
    end;
    
    public constructor(params evs: array of cl_event);
    begin
      self.evs := evs;
      self.count := evs.Length;
    end;
    
    {$endregion constructor's}
    
    {$region operator+}
    
    public static procedure operator+=(l: EventList; ev: cl_event);
    begin
      l.evs[l.count] := ev;
      l.count += 1;
    end;
    
    public static procedure operator+=(l: EventList; ev: EventList);
    begin
      for var i := 0 to ev.count-1 do
        l += ev[i];
      if ev.abortable then l.abortable := true;
    end;
    
    public static function operator+(l1, l2: EventList): EventList;
    begin
      Result := new EventList(l1.count+l2.count);
      Result += l1;
      Result += l2;
      Result.abortable := l1.abortable and l2.abortable;
    end;
    
    public static function operator+(l: EventList; ev: cl_event): EventList;
    begin
      Result := new EventList(l.count+1);
      Result += l;
      Result += ev;
    end;
    
    private static function Combine(evs: IList<EventList>; tsk: CLTaskBase; c: cl_context; main_dvc: cl_device_id; var cq: cl_command_queue): EventList;
    
    {$endregion operator+}
    
    {$region cl_event.AttachCallback}
    
    public static procedure AttachNativeCallback(ev: cl_event; cb: EventCallback) :=
    cl.SetEventCallback(ev, CommandExecutionStatus.COMPLETE, cb, NativeUtils.GCHndAlloc(cb)).RaiseIfError;
    
    private static function DefaultStatusErr(tsk: CLTaskBase; st: CommandExecutionStatus; save_err: boolean): boolean := save_err ? CLTaskExt.AddErr(tsk, st) : st.IS_ERROR;
    
    public static procedure AttachCallback(ev: cl_event; work: Action; tsk: CLTaskBase; need_ev_release: boolean{$ifdef EventDebug}; reason: string{$endif}; st_err_handler: (CLTaskBase, CommandExecutionStatus, boolean)->boolean := DefaultStatusErr; save_err: boolean := true) :=
    AttachNativeCallback(ev, (ev,st,data)->
    begin
      if need_ev_release then
      begin
        CLTaskExt.AddErr(tsk, cl.ReleaseEvent(ev));
        {$ifdef EventDebug}
        EventDebug.RegisterEventRelease(ev, $'discarding after use in AttachCallback, working on {reason}');
        {$endif EventDebug}
      end;
      
      if not st_err_handler(tsk, st, save_err) then
      try
        work;
      except
        on e: Exception do CLTaskExt.AddErr(tsk, e);
      end;
      
      NativeUtils.GCHndFree(data);
    end);
    
    public static procedure AttachFinallyCallback(ev: cl_event; work: Action; tsk: CLTaskBase; need_ev_release: boolean{$ifdef EventDebug}; reason: string{$endif}) :=
    AttachNativeCallback(ev, (ev,st,data)->
    begin
      if need_ev_release then
      begin
        CLTaskExt.AddErr(tsk, cl.ReleaseEvent(ev));
        {$ifdef EventDebug}
        if reason=nil then raise new InvalidOperationException;
        EventDebug.RegisterEventRelease(ev, $'discarding after use in AttachFinallyCallback, working on {reason}');
        {$endif EventDebug}
      end;
      
      try
        work;
      except
        on e: Exception do CLTaskExt.AddErr(tsk, e);
      end;
      
      NativeUtils.GCHndFree(data);
    end);
    
    {$endregion cl_event.AttachCallback}
    
    {$region EventList.AttachCallback}
    
    private function ToMarker(c: cl_context; dvc: cl_device_id; var cq: cl_command_queue; expect_smart_status_err: boolean): cl_event;
    begin
      {$ifdef DEBUG}
      if count <= 1 then raise new System.NotSupportedException;
      {$endif DEBUG}
      
      NativeUtils.FixCQ(c, dvc, cq);
      cl.EnqueueMarkerWithWaitList(cq, self.count, self.evs, Result).RaiseIfError;
      {$ifdef EventDebug}
      EventDebug.RegisterEventRetain(Result, $'enq''ed marker for evs: {evs.JoinToString}');
      {$endif EventDebug}
      if expect_smart_status_err then self.Retain(
        {$ifdef EventDebug}$'making sure ev isn''t deleted until SmartStatusErr'{$endif}
      );
      
    end;
    
    private function SmartStatusErr(tsk: CLTaskBase; org_st: CommandExecutionStatus; save_err: boolean; need_release: boolean): boolean;
    begin
      //ToDo NV#3035203
      //ToDo И добавить использование save_err, когда раскомметирую
//      if not org_st.IS_ERROR then exit;
//      if org_st.val <> ErrorCode.EXEC_STATUS_ERROR_FOR_EVENTS_IN_WAIT_LIST.val then
//        Result := CLTaskExt.AddErr(tsk, org_st) else
      
      {$ifdef DEBUG}
      if count <= 1 then raise new System.NotSupportedException;
      {$endif DEBUG}
      
      for var i := 0 to count-1 do
      begin
        var st: CommandExecutionStatus;
        var ec := cl.GetEventInfo(
          evs[i], EventInfo.EVENT_COMMAND_EXECUTION_STATUS,
          new UIntPtr(sizeof(CommandExecutionStatus)), st, IntPtr.Zero
        );
        
        if save_err then
        begin
          if CLTaskExt.AddErr(tsk, ec) then continue;
          if CLTaskExt.AddErr(tsk, st) then Result := true;
        end else
        begin
          if ec.IS_ERROR then continue;
          if st.IS_ERROR then Result := true;
        end;
        
      end;
      
      if need_release then self.Release(
        {$ifdef EventDebug}$'after use in SmartStatusErr'{$endif}
      );
      
      //ToDo NV#3035203 - без бага эта часть не нужна
      if org_st.val <> ErrorCode.EXEC_STATUS_ERROR_FOR_EVENTS_IN_WAIT_LIST.val then
        if save_err ? CLTaskExt.AddErr(tsk, org_st) : org_st.IS_ERROR then Result := true;
    end;
    
    public procedure AttachCallback(work: Action; tsk: CLTaskBase; c: cl_context; dvc: cl_device_id; var cq: cl_command_queue{$ifdef EventDebug}; reason: string{$endif}; save_err: boolean := true) :=
    case self.count of
      0: work;
      1: AttachCallback(self.evs[0], work, tsk, false{$ifdef EventDebug}, nil{$endif}, DefaultStatusErr, save_err);
      else AttachCallback(self.ToMarker(c, dvc, cq, true), work, tsk, true{$ifdef EventDebug}, reason{$endif}, (tsk,st,save_err)->SmartStatusErr(tsk,st,save_err,true), save_err);
    end;
    
    public procedure AttachFinallyCallback(work: Action; tsk: CLTaskBase; c: cl_context; dvc: cl_device_id; var cq: cl_command_queue{$ifdef EventDebug}; reason: string{$endif}) :=
    case self.count of
      0: work;
      1: AttachFinallyCallback(self.evs[0], work, tsk, false{$ifdef EventDebug}, nil{$endif});
      else AttachFinallyCallback(self.ToMarker(c, dvc, cq, false), work, tsk, true{$ifdef EventDebug}, reason{$endif});
    end;
    
    {$endregion EventList.AttachCallback}
    
    {$region Retain/Release}
    
    public procedure Retain({$ifdef EventDebug}reason: string{$endif}) :=
    for var i := 0 to count-1 do
    begin
      cl.RetainEvent(evs[i]).RaiseIfError;
      {$ifdef EventDebug}
      EventDebug.RegisterEventRetain(evs[i], $'{reason}, together with evs: {evs.JoinToString}');
      {$endif EventDebug}
    end;
    
    public procedure Release({$ifdef EventDebug}reason: string{$endif}) :=
    for var i := 0 to count-1 do
    begin
      cl.ReleaseEvent(evs[i]).RaiseIfError;
      {$ifdef EventDebug}
      EventDebug.RegisterEventRelease(evs[i], $'{reason}, together with evs: {evs.JoinToString}');
      {$endif EventDebug}
    end;
    
    /// True если возникла ошибка
    public function WaitAndRelease(tsk: CLTaskBase): boolean;
    begin
      {$ifdef DEBUG}
      if count=0 then raise new NotSupportedException;
      {$endif DEBUG}
      
      var st := CommandExecutionStatus(cl.WaitForEvents(self.count, self.evs));
      if count=1 then
        Result := DefaultStatusErr(tsk, st, true) else
        Result := SmartStatusErr(tsk, st, true, false);
      
      self.Release({$ifdef EventDebug}$'discarding after being waited upon'{$endif EventDebug});
    end;
    
    {$endregion Retain/Release}
    
  end;
  
  {$endregion EventList}
  
  {$region UserEvent}
  
  UserEvent = sealed class
    private uev: cl_event;
    private done := false;
    
    {$region constructor's}
    
    private constructor(c: cl_context{$ifdef EventDebug}; reason: string{$endif});
    begin
      var ec: ErrorCode;
      self.uev := cl.CreateUserEvent(c, ec);
      ec.RaiseIfError;
      {$ifdef EventDebug}
      EventDebug.RegisterEventRetain(self.uev, $'Created for {reason}');
      {$endif EventDebug}
    end;
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    public static function MakeUserEvent(tsk: CLTaskBase; c: cl_context{$ifdef EventDebug}; reason: string{$endif}): UserEvent;
    
    public static function StartBackgroundWork(after: EventList; work: Action; c: cl_context; tsk: CLTaskBase{$ifdef EventDebug}; reason: string{$endif}): UserEvent;
    begin
      var res := MakeUserEvent(tsk, c
        {$ifdef EventDebug}, $'BackgroundWork, executing {reason}'{$endif}
      );
      
      var abort_thr_ev := new AutoResetEvent(false);
      EventList.AttachFinallyCallback(res, ()->abort_thr_ev.Set(), tsk, false{$ifdef EventDebug}, nil{$endif});
      
      var work_thr: Thread;
      var abort_thr := NativeUtils.StartNewThread(()->
      begin
        abort_thr_ev.WaitOne; // изначальная пауза, чтобы work_thr не убили до того как он успеет запуститься и выполнить after.Release
        abort_thr_ev.WaitOne;
        work_thr.Abort;
      end);
      
      work_thr := NativeUtils.StartNewThread(()->
      try
        var err := false;
        try
          if (after<>nil) and (after.count<>0) then
          begin
            if not after.abortable then
              after := after + MakeUserEvent(tsk,c
                {$ifdef EventDebug}, $'abortability of BackgroundWork wait on: {after.evs.JoinToString}'{$endif}
              );
            err := after.WaitAndRelease(tsk);
          end;
        finally
          abort_thr_ev.Set;
          // Далее - в любом случае выполняется res.SetStatus, который вызывает
          // содержимое res.AttachFinallyCallback выше
          // Поэтому abort_thr никогда не застрянет
        end;
        
        if err then
        begin
          abort_thr.Abort;
          res.Abort;
        end else
        begin
          work;
          abort_thr.Abort;
          res.SetStatus(CommandExecutionStatus.COMPLETE);
        end;
        
      except
        on e: Exception do
        begin
          CLTaskExt.AddErr(tsk, e);
          // Первый .AddErr всегда сам вызывает .Abort на всех UserEvent
          // А значит и abort_thr.Abort уже выполнило выше
          // Единственное исключение - если "e is ThreadAbortException"
          // Но это случится только если abort_thr уже доработало
//          abort_thr.Abort;
//          res.Abort;
        end;
      end);
      
      Result := res;
    end;
    
    {$endregion constructor's}
    
    {$region Status}
    
    public property CanRemove: boolean read done;
    
    /// True если статус получилось изменить
    public function SetStatus(st: CommandExecutionStatus): boolean;
    begin
      lock self do
      begin
        if done then exit;
        cl.SetUserEventStatus(uev, st).RaiseIfError;
        done := true;
        Result := true;
      end;
    end;
    /// True если статус получилось изменить
    public function SetStatus(st: CommandExecutionStatus; tsk: CLTaskBase): boolean;
    begin
      lock self do
      begin
        if done then exit;
        if CLTaskExt.AddErr(tsk, cl.SetUserEventStatus(uev, st)) then exit;
        done := true;
        Result := true;
      end;
    end;
    public function Abort := SetStatus(NativeUtils.AbortStatus);
    
    {$endregion Status}
    
    {$region operator's}
    
    public static function operator implicit(ev: UserEvent): cl_event := ev.uev;
    public static function operator implicit(ev: UserEvent): EventList;
    begin
      Result := ev.uev;
      Result.abortable := true;
    end;
    
    public static function operator+(ev1: EventList; ev2: UserEvent): EventList;
    begin
      Result := ev1 + ev2.uev;
      Result.abortable := true;
    end;
    
    {$endregion operator's}
    
  end;
  
  {$endregion UserEvent}
  
  {$region QueueRes}
  
  IPtrQueueRes<T> = interface
    function GetPtr: ^T;
  end;
  QRPtrWrap<T> = sealed class(IPtrQueueRes<T>)
    private ptr: ^T := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<T>));
    
    public constructor(val: T) := self.ptr^ := val;
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected procedure Finalize; override :=
    Marshal.FreeHGlobal(new IntPtr(ptr));
    
    public function GetPtr: ^T := ptr;
    
  end;
  
  QueueRes<T> = class;
  QueueResBase = abstract class
    public ev: EventList;
    public can_set_ev := true;
    
    public constructor(ev: EventList) :=
    self.ev := ev;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    public function TrySetEvBase(new_ev: EventList): QueueResBase; abstract;
    
    public function GetResBase: object; abstract;
    
    public function LazyQuickTransformBase<T2>(f: object->T2): QueueRes<T2>; abstract;
    
  end;
  
  QueueResDelayedPtr<T> = class;
  QueueRes<T> = abstract class(QueueResBase)
    
    public function GetRes: T; abstract;
    public function GetResBase: object; override := GetRes;
    
    public function Clone: QueueRes<T>; abstract;
    
    public function TrySetEv(new_ev: EventList): QueueRes<T>;
    begin
      if self.ev=new_ev then
        Result := self else
      begin
        Result := can_set_ev ? self : Clone;
        Result.ev := new_ev;
      end;
    end;
    public function TrySetEvBase(new_ev: EventList): QueueResBase; override := TrySetEv(new_ev);
    
    public function EnsureAbortability(tsk: CLTaskBase; c: Context): QueueRes<T>;
    begin
      Result := self;
      if (ev.count<>0) and not ev.abortable then
        Result := Result.TrySetEv(ev + UserEvent.MakeUserEvent(tsk, c.Native
          {$ifdef EventDebug}, $'abortability of QueueRes with .ev: {ev.evs.JoinToString}'{$endif}
        ));
    end;
    
    public function LazyQuickTransform<T2>(f: T->T2): QueueRes<T2>; abstract;
    public function LazyQuickTransformBase<T2>(f: object->T2): QueueRes<T2>; override :=
    LazyQuickTransform(o->f(o));
    
    /// Должно выполнятся только после ожидания ивентов
    public function ToPtr: IPtrQueueRes<T>; abstract;
    
  end;
  
  // Результат который просто есть
  IQueueResConst = interface end;
  QueueResConst<T> = sealed class(QueueRes<T>, IQueueResConst)
    private res: T;
    
    public constructor(res: T; ev: EventList);
    begin
      inherited Create(ev);
      self.res := res;
    end;
    private constructor := inherited;
    
    public function Clone: QueueRes<T>; override := new QueueResConst<T>(res, ev);
    
    public function GetRes: T; override := res;
    
    public function LazyQuickTransform<T2>(f: T->T2): QueueRes<T2>; override :=
    new QueueResConst<T2>(f(self.res), self.ev);
    
    public function ToPtr: IPtrQueueRes<T>; override := new QRPtrWrap<T>(res);
    
  end;
  
  // Результат который надо будет сначала дождаться, а потом ещё досчитать
  IQueueResFunc = interface
    function GetF: ()->object;
  end;
  QueueResFunc<T> = sealed class(QueueRes<T>, IQueueResFunc)
    private f: ()->T;
    
    public constructor(f: ()->T; ev: EventList);
    begin
      inherited Create(ev);
      self.f := f;
    end;
    private constructor := inherited;
    
    public function Clone: QueueRes<T>; override := new QueueResFunc<T>(f, ev);
    
    public function GetRes: T; override := f();
    public function IQueueResFunc.GetF: ()->object := ()->f();
    
    public function LazyQuickTransform<T2>(f: T->T2): QueueRes<T2>; override :=
    new QueueResFunc<T2>(()->f(self.f()), self.ev);
    
    public function ToPtr: IPtrQueueRes<T>; override := new QRPtrWrap<T>(f());
    
  end;
  
  // Результат который будет сохранён куда то, надо только дождаться
  IQueueResDelayed = interface end;
  QueueResDelayedBase<T> = abstract class(QueueRes<T>, IQueueResDelayed)
    
    public static function MakeNew(need_ptr_qr: boolean): QueueResDelayedBase<T>;
    
    public procedure SetRes(value: T); abstract;
    
    public function Clone: QueueRes<T>; override := new QueueResFunc<T>(self.GetRes, ev);
    
    public function LazyQuickTransform<T2>(f: T->T2): QueueRes<T2>; override :=
    new QueueResFunc<T2>(()->f(self.GetRes()), self.ev);
    
  end;
  QueueResDelayedObj<T> = sealed class(QueueResDelayedBase<T>)
    private res := default(T);
    
    public constructor :=
    inherited Create(nil);
    
    public function GetRes: T; override := res;
    public procedure SetRes(value: T); override := res := value;
    
    public function ToPtr: IPtrQueueRes<T>; override := new QRPtrWrap<T>(res);
    
  end;
  QueueResDelayedPtr<T> = sealed class(QueueResDelayedBase<T>, IPtrQueueRes<T>)
    private ptr: ^T := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<T>));
    
    public constructor :=
    inherited Create(nil);
    
    public constructor(res: T; ev: EventList);
    begin
      inherited Create(ev);
      self.ptr^ := res;
    end;
    
    public function GetPtr: ^T := ptr;
    public function GetRes: T; override := ptr^;
    public procedure SetRes(value: T); override := ptr^ := value;
    
    protected procedure Finalize; override :=
    Marshal.FreeHGlobal(new IntPtr(ptr));
    
    public function ToPtr: IPtrQueueRes<T>; override := self;
    
  end;
  
  {$endregion QueueRes}
  
  {$region MWEventContainer}
  
  MWEventContainer = sealed class // MW = Multi Wait
    private curr_handlers := new Queue<()->boolean>;
    private cached := 0;
    
    public procedure AddHandler(handler: ()->boolean) :=
    lock self do
      if cached=0 then
        curr_handlers += handler else
      if handler() then
        cached -= 1;
    
    public procedure ExecuteHandler :=
    lock self do
    begin
      while curr_handlers.Count<>0 do
        if curr_handlers.Dequeue()() then
          exit;
      cached += 1;
    end;
    
  end;
  
  {$endregion MWEventContainer}
  
  {$endregion Util type's}
  
  {$region Multiusable}
  
  MultiusableCommandQueueHubBase = abstract class
    
  end;
  
  {$endregion Multiusable}
  
  {$region CLTask}
  
  ///Представляет задачу выполнения очереди, создаваемую методом Context.BeginInvoke
  CLTaskBase = abstract class
    
    protected wh := new ManualResetEvent(false);
    protected wh_lock := new object;
    
    protected mu_res := new Dictionary<MultiusableCommandQueueHubBase, QueueResBase>;
    
    {$region property's}
    
    private function OrgQueueBase: CommandQueueBase; abstract;
    ///Возвращает очередь, которую выполняет данный CLTask
    public property OrgQueue: CommandQueueBase read OrgQueueBase;
    
    private org_c: Context;
    ///Возвращает контекст, в котором выполняется данный CLTask
    public property OrgContext: Context read org_c;
    
    {$endregion property's}
    
    {$region AddErr}
    protected err_lst := new List<Exception>;
    
    /// lock err_lst do err_lst.ToArray
    private function GetErrArr: array of Exception;
    begin
      lock err_lst do
        Result := err_lst.ToArray;
    end;
    
    ///Возвращает исключение, полученное при выполнении очереди
    ///Возвращает nil, если исключений не было
    public property Error: AggregateException read err_lst.Count=0 ? nil : new AggregateException($'При выполнении очереди было вызвано {err_lst.Count} исключений. Используйте try чтоб получить больше информации', GetErrArr);
    
    protected procedure AddErr(e: Exception) :=
    begin
      if e is ThreadAbortException then exit;
      lock err_lst do err_lst += e;
      lock user_events do
      begin
        for var i := user_events.Count-1 downto 0 do
          user_events[i].Abort;
        user_events.Clear;
      end;
    end;
    
    /// True если ошибка есть
    protected function AddErr(ec: ErrorCode): boolean;
    begin
      if not ec.IS_ERROR then exit;
      AddErr(new OpenCLException(ec, $'Внутренняя ошибка OpenCLABC: {ec}{#10}{Environment.StackTrace}'));
      Result := true;
    end;
    /// True если ошибка есть
    protected function AddErr(st: CommandExecutionStatus) :=
    (st=NativeUtils.AbortStatus) or (st.IS_ERROR and AddErr(ErrorCode(st)));
    
    {$endregion AddErr}
    
    {$region UserEvent's}
    protected user_events := new List<UserEvent>;
    
    protected function MakeUserEvent(c: cl_context{$ifdef EventDebug}; reason: string{$endif}): UserEvent;
    begin
      Result := new UserEvent(c{$ifdef EventDebug}, reason{$endif});
      
      lock user_events do
      begin
        if err_lst.Count<>0 then
          Result.Abort else
          user_events += Result;
      end;
      
    end;
    
    {$endregion UserEvent's}
    
    {$region CLTask event's}
    
    protected procedure WhenDoneBase(cb: Action<CLTaskBase>); abstract;
    ///Добавляет подпрограмму-обработчик, которая будет вызвана когда выполнение очереди завершится (успешно или с ошибой)
    public procedure WhenDone(cb: Action<CLTaskBase>) := WhenDoneBase(cb);
    
    protected procedure WhenCompleteBase(cb: Action<CLTaskBase, object>); abstract;
    ///Добавляет подпрограмму-обработчик, которая будет вызвана когда- и если выполнение очереди завершится успешно
    public procedure WhenComplete(cb: Action<CLTaskBase, object>) := WhenCompleteBase(cb);
    
    protected procedure WhenErrorBase(cb: Action<CLTaskBase, array of Exception>); abstract;
    ///Добавляет подпрограмму-обработчик, которая будет вызвана когда- и если при выполнении очереди будет вызвано исключение
    public procedure WhenError(cb: Action<CLTaskBase, array of Exception>) := WhenErrorBase(cb);
    
    /// True если очередь уже завершилась
    private function AddEventHandler<T>(ev: List<T>; cb: T): boolean; where T: Delegate;
    begin
      lock wh_lock do
      begin
        Result := wh.WaitOne(0);
        if not Result then ev += cb;
      end;
    end;
    
    {$endregion CLTask event's}
    
    {$region SyncRes}
    
    ///Ожидает окончания выполнения очереди (если оно ещё не завершилось)
    ///Вызывает исключение, если оно было вызвано при выполнении очереди
    public procedure Wait;
    begin
      wh.WaitOne;
      var err := self.Error;
      if err<>nil then raise err;
    end;
    
    protected function WaitResBase: object; abstract;
    ///Ожидает окончания выполнения очереди (если оно ещё не завершилось)
    ///Вызывает исключение, если оно было вызвано при выполнении очереди
    ///А затем возвращает результат выполнения
    public function WaitRes := WaitResBase;
    
    {$endregion}
    
  end;
  
  ///Представляет задачу выполнения очереди, создаваемую методом Context.BeginInvoke
  CLTask<T> = sealed class(CLTaskBase)
    protected q: CommandQueue<T>;
    protected q_res: T;
    
    ///Возвращает очередь, которую выполняет данный CLTask
    public property OrgQueue: CommandQueue<T> read q; reintroduce;
    protected function OrgQueueBase: CommandQueueBase; override;
    
    private procedure RegisterWaitables(q: CommandQueue<T>);
    private function InvokeQueue(q: CommandQueue<T>; c: Context; var cq: cl_command_queue): QueueRes<T>;
    protected constructor(q: CommandQueue<T>; c: Context);
    begin
      self.q := q;
      self.org_c := c;
      RegisterWaitables(q);
      
      var cq: cl_command_queue;
      var qr := InvokeQueue(q, c, cq);
      
      // mu выполняют лишний .Retain, чтобы ивент не удалился пока очередь ещё запускается
      foreach var mu_qr in mu_res.Values do
        mu_qr.ev.Release({$ifdef EventDebug}$'excessive mu ev'{$endif});
      mu_res := nil;
      
      {$ifdef DEBUG}
      //CQ.Invoke всегда выполняет UserEvent.EnsureAbortability, поэтому тут оно не нужно
      if (qr.ev.count<>0) and not qr.ev.abortable then raise new NotSupportedException;
      {$endif DEBUG}
      
      //CQ.Invoke всегда выполняет UserEvent.EnsureAbortability, поэтому тут оно не нужно
      qr.ev.AttachFinallyCallback(()->
      begin
        qr.ev.Release({$ifdef EventDebug}$'last ev of CLTask'{$endif});
        if cq<>cl_command_queue.Zero then
          System.Threading.Tasks.Task.Run(()->self.AddErr( cl.ReleaseCommandQueue(cq) ));
        OnQDone(qr);
      end, self, c.Native, c.MainDevice.Native, cq{$ifdef EventDebug}, $'CLTask.OnQDone'{$endif});
      
    end;
    
    {$region CLTask event's}
    
    protected EvDone := new List<Action<CLTask<T>>>;
    ///Добавляет подпрограмму-обработчик, которая будет вызвана когда выполнение очереди завершится (успешно или с ошибой)
    public procedure WhenDone(cb: Action<CLTask<T>>); reintroduce :=
    if AddEventHandler(EvDone, cb) then cb(self);
    protected procedure WhenDoneBase(cb: Action<CLTaskBase>); override :=
    WhenDone(cb as object as Action<CLTask<T>>); //ToDo #2221
    
    protected EvComplete := new List<Action<CLTask<T>, T>>;
    ///Добавляет подпрограмму-обработчик, которая будет вызвана когда- и если выполнение очереди завершится успешно
    public procedure WhenComplete(cb: Action<CLTask<T>, T>); reintroduce :=
    if AddEventHandler(EvComplete, cb) then cb(self, q_res);
    protected procedure WhenCompleteBase(cb: Action<CLTaskBase, object>); override :=
    WhenComplete(cb as object as Action<CLTask<T>, T>); //ToDo #2221
    
    protected EvError := new List<Action<CLTask<T>, array of Exception>>;
    ///Добавляет подпрограмму-обработчик, которая будет вызвана когда- и если при выполнении очереди будет вызвано исключение
    public procedure WhenError(cb: Action<CLTask<T>, array of Exception>); reintroduce :=
    if AddEventHandler(EvError, cb) then cb(self, GetErrArr);
    protected procedure WhenErrorBase(cb: Action<CLTaskBase, array of Exception>); override :=
    WhenError(cb as object as Action<CLTask<T>, array of Exception>); //ToDo #2221
    
    {$endregion CLTask event's}
    
    {$region Execution}
    
    private procedure OnQDone(qr: QueueRes<T>);
    begin
      var l_EvDone:     array of Action<CLTask<T>>;
      var l_EvComplete: array of Action<CLTask<T>, T>;
      var l_EvError:    array of Action<CLTask<T>, array of Exception>;
      
      lock wh_lock do
      try
        
        l_EvDone      := EvDone.ToArray;
        l_EvComplete  := EvComplete.ToArray;
        l_EvError     := EvError.ToArray;
        
        if err_lst.Count=0 then self.q_res := qr.GetRes;
      finally
        wh.Set;
      end;
      
      foreach var ev in l_EvDone do
      try
        ev(self);
      except
        on e: Exception do AddErr(e);
      end;
      
      if err_lst.Count=0 then
      begin
        
        foreach var ev in l_EvComplete do
        try
          ev(self, self.q_res);
        except
          on e: Exception do AddErr(e);
        end;
        
      end else
      if l_EvError.Length<>0 then
      begin
        var err_arr := GetErrArr;
        
        foreach var ev in l_EvError do
        try
          ev(self, err_arr);
        except
          on e: Exception do AddErr(e);
        end;
        
      end;
      
    end;
    
    ///Ожидает окончания выполнения очереди (если оно ещё не завершилось)
    ///Вызывает исключение, если оно было вызвано при выполнении очереди
    ///А затем возвращает результат выполнения
    public function WaitRes: T; reintroduce;
    begin
      Wait;
      Result := self.q_res;
    end;
    protected function WaitResBase: object; override := WaitRes;
    
    {$endregion Execution}
    
  end;
  
  {$endregion CLTask}
  
  {$region CommandQueue}
  
  {$region Base}
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueueBase = abstract class
    
    {$region Invoke}
    
    protected function InvokeBase(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueResBase; abstract;
    
    protected function InvokeNewQBase(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; prev_ev: EventList): QueueResBase; abstract;
    
    {$endregion Invoke}
    
    {$region MW}
    
    /// добавляет tsk в качестве ключа для всех ожидаемых очередей
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); abstract;
    
    private mw_evs := new Dictionary<CLTaskBase, MWEventContainer>;
    private procedure RegisterWaiterTask(tsk: CLTaskBase) :=
    lock mw_evs do if not mw_evs.ContainsKey(tsk) then
    begin
      mw_evs[tsk] := new MWEventContainer;
      tsk.WhenDone(tsk->lock mw_evs do mw_evs.Remove(tsk));
    end;
    
    private procedure AddMWHandler(tsk: CLTaskBase; handler: ()->boolean);
    begin
      var cont: MWEventContainer;
      lock mw_evs do cont := mw_evs[tsk];
      cont.AddHandler(handler);
    end;
    
    private procedure ExecuteMWHandlers;
    begin
      if mw_evs.Count=0 then exit;
      var conts: array of MWEventContainer;
      lock mw_evs do conts := mw_evs.Values.ToArray;
      for var i := 0 to conts.Length-1 do conts[i].ExecuteHandler;
    end;
    
    {$endregion MW}
    
    {$region ConstQueue}
    
    public static function operator implicit(o: object): CommandQueueBase;
    
    {$endregion ConstQueue}
    
    {$region Cast}
    
    ///Если данная очередь проходит по условию "... is CommandQueue<T>" - возвращает себя же
    ///Иначе возвращает очередь-обёртку, выполняющую "res := T(res)", где res - результат данной очереди
    public function Cast<T>: CommandQueue<T>;
    
    {$endregion}
    
    {$region ThenConvert}
    
    protected function ThenConvertBase<TOtp>(f: (object, Context)->TOtp): CommandQueue<TOtp>; abstract;
    
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди
    public function ThenConvert<TOtp>(f: object->TOtp           ) := ThenConvertBase((o,c)->f(o));
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди и контекст выполнения
    public function ThenConvert<TOtp>(f: (object, Context)->TOtp) := ThenConvertBase(f);
    
    {$endregion ThenConvert}
    
    {$region +/*}
    
    protected function AfterQueueSyncBase(q: CommandQueueBase): CommandQueueBase; abstract;
    protected function AfterQueueAsyncBase(q: CommandQueueBase): CommandQueueBase; abstract;
    
    public static function operator+(q1, q2: CommandQueueBase): CommandQueueBase := q2.AfterQueueSyncBase(q1);
    public static function operator*(q1, q2: CommandQueueBase): CommandQueueBase := q2.AfterQueueAsyncBase(q1);
    
    public static procedure operator+=(var q1: CommandQueueBase; q2: CommandQueueBase) := q1 := q1+q2;
    public static procedure operator*=(var q1: CommandQueueBase; q2: CommandQueueBase) := q1 := q1*q2;
    
    {$endregion +/*}
    
    {$region Multiusable}
    
    protected function MultiusableBase: ()->CommandQueueBase; abstract;
    
    ///Создаёт функцию, вызывая которую можно создать любое кол-во очередей-удлинителей для данной очереди
    ///Подробнее в справке: "Очередь>>Создание очередей>>Множественное использование очереди"
    public function Multiusable := MultiusableBase;
    
    {$endregion Multiusable}
    
    {$region ThenWait}
    
    protected function ThenWaitForAllBase(qs: sequence of CommandQueueBase): CommandQueueBase; abstract;
    protected function ThenWaitForAnyBase(qs: sequence of CommandQueueBase): CommandQueueBase; abstract;
    
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала выполненности от каждой из заданых очередей
    public function ThenWaitForAll(params qs: array of CommandQueueBase) := ThenWaitForAllBase(qs);
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала выполненности от каждой из заданых очередей
    public function ThenWaitForAll(qs: sequence of CommandQueueBase    ) := ThenWaitForAllBase(qs);
    
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую первого сигнала выполненности от любой из заданных очередей
    public function ThenWaitForAny(params qs: array of CommandQueueBase) := ThenWaitForAnyBase(qs);
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую первого сигнала выполненности от любой из заданных очередей
    public function ThenWaitForAny(qs: sequence of CommandQueueBase    ) := ThenWaitForAnyBase(qs);
    
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала выполненности от заданой очереди
    public function ThenWaitFor(q: CommandQueueBase) := ThenWaitForAll(q);
    
    {$endregion ThenWait}
    
  end;
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueue<T> = abstract class(CommandQueueBase)
    
    {$region Invoke}
    
    protected function InvokeImpl(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>; abstract;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>;
    begin
      Result := InvokeImpl(tsk, c, main_dvc, need_ptr_qr, cq, prev_ev).EnsureAbortability(tsk, c);
      Result.ev.AttachCallback(self.ExecuteMWHandlers, tsk, c.Native, main_dvc, cq{$ifdef EventDebug}, $'ExecuteMWHandlers'{$endif}, false);
    end;
    protected function InvokeBase(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueResBase; override :=
    Invoke(tsk, c, main_dvc, need_ptr_qr, cq, prev_ev);
    
    protected function InvokeNewQ(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; prev_ev: EventList): QueueRes<T>;
    begin
      var cq := cl_command_queue.Zero;
      Result := Invoke(tsk, c, main_dvc, need_ptr_qr, cq, prev_ev);
      
      {$ifdef DEBUG}
      // Result.ev.abortable уже true, потому что .EnsureAbortability в Invoke
      if (Result.ev.count<>0) and not Result.ev.abortable then raise new System.NotSupportedException;
      {$endif DEBUG}
      
      if cq<>cl_command_queue.Zero then
        Result.ev.AttachFinallyCallback(()->
        begin
          System.Threading.Tasks.Task.Run(()->tsk.AddErr(cl.ReleaseCommandQueue(cq)))
        end, tsk, c.Native, main_dvc, cq{$ifdef EventDebug}, $'cl.ReleaseCommandQueue'{$endif});
      
    end;
    protected function InvokeNewQBase(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; prev_ev: EventList): QueueResBase; override :=
    InvokeNewQ(tsk, c, main_dvc, need_ptr_qr, prev_ev);
    
    {$endregion Invoke}
    
    {$region ConstQueue}
    
    public static function operator implicit(o: T): CommandQueue<T>;
    
    {$endregion ConstQueue}
    
    {$region ThenConvert}
    
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди
    public function ThenConvert<TOtp>(f: T->TOtp): CommandQueue<TOtp> := ThenConvert((o,c)->f(o));
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди и контекст выполнения
    public function ThenConvert<TOtp>(f: (T, Context)->TOtp): CommandQueue<TOtp>;
    
    protected function ThenConvertBase<TOtp>(f: (object, Context)->TOtp): CommandQueue<TOtp>; override :=
    ThenConvert(f as object as Func2<T, Context, TOtp>);
    
    {$endregion ThenConvert}
    
    {$region +/*}
    
    public static function operator+(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T>;
    public static function operator*(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T>;
    
    protected function AfterQueueSyncBase (q: CommandQueueBase): CommandQueueBase; override := q+self;
    protected function AfterQueueAsyncBase(q: CommandQueueBase): CommandQueueBase; override := q*self;
    
    public static procedure operator+=(var q1: CommandQueue<T>; q2: CommandQueue<T>) := q1 := q1+q2;
    public static procedure operator*=(var q1: CommandQueue<T>; q2: CommandQueue<T>) := q1 := q1*q2;
    
    {$endregion +/*}
    
    {$region Multiusable}
    
    ///Создаёт функцию, вызывая которую можно создать любое кол-во очередей-удлинителей для данной очереди
    ///Подробнее в справке: "Очередь>>Создание очередей>>Множественное использование очереди"
    public function Multiusable: ()->CommandQueue<T>;
    
    protected function MultiusableBase: ()->CommandQueueBase; override := Multiusable as object as Func<CommandQueueBase>; //ToDo #2221
    
    {$endregion Multiusable}
    
    {$region ThenWait}
    
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала выполненности от каждой из заданых очередей
    public function ThenWaitForAll(params qs: array of CommandQueueBase): CommandQueue<T> := ThenWaitForAll(qs.AsEnumerable);
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала выполненности от каждой из заданых очередей
    public function ThenWaitForAll(qs: sequence of CommandQueueBase): CommandQueue<T>;
    
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую первого сигнала выполненности от любой из заданных очередей
    public function ThenWaitForAny(params qs: array of CommandQueueBase): CommandQueue<T> := ThenWaitForAny(qs.AsEnumerable);
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую первого сигнала выполненности от любой из заданных очередей
    public function ThenWaitForAny(qs: sequence of CommandQueueBase): CommandQueue<T>;
    
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала выполненности от заданой очереди
    public function ThenWaitFor(q: CommandQueueBase) := ThenWaitForAll(q);
    
    protected function ThenWaitForAllBase(qs: sequence of CommandQueueBase): CommandQueueBase; override := ThenWaitForAll(qs);
    protected function ThenWaitForAnyBase(qs: sequence of CommandQueueBase): CommandQueueBase; override := ThenWaitForAny(qs);
    
    {$endregion ThenWait}
    
  end;
  
  {$endregion Base}
  
  {$region Host}
  
  /// очередь, выполняющая какую то работу на CPU, всегда в отдельном потоке
  HostQueue<TInp,TRes> = abstract class(CommandQueue<TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<TInp>; abstract;
    
    protected function ExecFunc(o: TInp; c: Context): TRes; abstract;
    
    protected function InvokeImpl(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<TRes>; override;
    begin
      var prev_qr := InvokeSubQs(tsk, c, main_dvc, cq, prev_ev);
      
      var qr := QueueResDelayedBase&<TRes>.MakeNew(need_ptr_qr);
      qr.ev := UserEvent.StartBackgroundWork(prev_qr.ev, ()->qr.SetRes( ExecFunc(prev_qr.GetRes(), c) ), c.Native, tsk
        {$ifdef EventDebug}, $'body of {self.GetType}'{$endif}
      );
      
      Result := qr;
    end;
    
  end;
  
  {$endregion Host}
  
  {$region Const}
  
  ///Интерфейс, который реализован только классом ConstQueue<T>
  ///Позволяет получить значение, из которого была создана константая очередь, не зная его типа
  IConstQueue = interface
    ///Возвращает значение из которого была создана данная константная очередь
    function GetConstVal: Object;
  end;
  ///Представляет константную очередь
  ///Константные очереди ничего не выполняют и возвращает заданное при создании значение
  ConstQueue<T> = sealed class(CommandQueue<T>, IConstQueue)
    private res: T;
    
    ///Создаёт новую константную очередь из заданного значения
    public constructor(o: T) :=
    self.res := o;
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    public function IConstQueue.GetConstVal: object := self.res;
    ///Возвращает значение из которого была создана данная константная очередь
    public property Val: T read self.res;
    
    protected function InvokeImpl(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>; override;
    begin
      if prev_ev=nil then prev_ev := new EventList;
      
      if need_ptr_qr then
        Result := new QueueResDelayedPtr<T> (self.res, prev_ev) else
        Result := new QueueResConst<T>      (self.res, prev_ev);
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override := exit;
    
  end;
  
  {$endregion Const}
  
  {$region Array}
  
  {$region Simple}
  
  ISimpleQueueArray = interface
    function GetQS: array of CommandQueueBase;
  end;
  SimpleQueueArray<T> = abstract class(CommandQueue<T>, ISimpleQueueArray)
    protected qs: array of CommandQueueBase;
    
    public constructor(params qs: array of CommandQueueBase) := self.qs := qs;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    public function GetQS: array of CommandQueueBase := qs;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override :=
    foreach var q in qs do q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
  ISimpleSyncQueueArray = interface(ISimpleQueueArray) end;
  SimpleSyncQueueArray<T> = sealed class(SimpleQueueArray<T>, ISimpleSyncQueueArray)
    
    protected function InvokeImpl(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>; override;
    begin
      
      for var i := 0 to qs.Length-2 do
        prev_ev := qs[i].InvokeBase(tsk, c, main_dvc, false, cq, prev_ev).ev;
      
      Result := (qs[qs.Length-1] as CommandQueue<T>).Invoke(tsk, c, main_dvc, need_ptr_qr, cq, prev_ev);
    end;
    
  end;
  
  ISimpleAsyncQueueArray = interface(ISimpleQueueArray) end;
  SimpleAsyncQueueArray<T> = sealed class(SimpleQueueArray<T>, ISimpleAsyncQueueArray)
    
    protected function InvokeImpl(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>; override;
    begin
      if (prev_ev<>nil) and (prev_ev.count<>0) then loop qs.Length-1 do prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var evs := new EventList[qs.Length];
      
      for var i := 0 to qs.Length-2 do
        evs[i] := qs[i].InvokeNewQBase(tsk, c, main_dvc, false, prev_ev).ev;
      
      // Используем внешнюю cq, чтобы не создавать лишнюю
      Result := (qs[qs.Length-1] as CommandQueue<T>).Invoke(tsk, c, main_dvc, need_ptr_qr, cq, prev_ev);
      evs[evs.Length-1] := Result.ev;
      
      Result := Result.TrySetEv( EventList.Combine(evs, tsk, c.Native, main_dvc, cq) ?? new EventList );
    end;
    
  end;
  
  {$endregion Simple}
  
  {$region Conv}
  
  {$region Generic}
  
  ConvQueueArrayBase<TInp, TRes> = abstract class(HostQueue<array of TInp, TRes>)
    protected qs: array of CommandQueue<TInp>;
    protected f: Func<array of TInp, Context, TRes>;
    
    public constructor(qs: array of CommandQueue<TInp>; f: Func<array of TInp, Context, TRes>);
    begin
      self.qs := qs;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override :=
    foreach var q in qs do q.RegisterWaitables(tsk, prev_hubs);
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; override := f(o, c);
    
  end;
  
  ConvSyncQueueArray<TInp, TRes> = sealed class(ConvQueueArrayBase<TInp, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<array of TInp>; override;
    begin
      var qrs := new QueueRes<TInp>[qs.Length];
      
      for var i := 0 to qs.Length-1 do
      begin
        var qr := qs[i].Invoke(tsk, c, main_dvc, false, cq, prev_ev);
        prev_ev := qr.ev;
        qrs[i] := qr;
      end;
      
      Result := new QueueResFunc<array of TInp>(()->
      begin
        Result := new TInp[qrs.Length];
        for var i := 0 to qrs.Length-1 do
          Result[i] := qrs[i].GetRes;
      end, prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray<TInp, TRes> = sealed class(ConvQueueArrayBase<TInp, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<array of TInp>; override;
    begin
      if (prev_ev<>nil) and (prev_ev.count<>0) then loop qs.Length-1 do prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qrs := new QueueRes<TInp>[qs.Length];
      var evs := new EventList[qs.Length];
      
      for var i := 0 to qs.Length-2 do
      begin
        var qr := qs[i].InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
        qrs[i] := qr;
        evs[i] := qr.ev;
      end;
      
      // Отдельно, чтобы не создавать лишнюю cq
      var qr := qs[qs.Length-1].Invoke(tsk, c, main_dvc, false, cq, prev_ev);
      qrs[evs.Length-1] := qr;
      evs[evs.Length-1] := qr.ev;
      
      Result := new QueueResFunc<array of TInp>(()->
      begin
        Result := new TInp[qrs.Length];
        for var i := 0 to qrs.Length-1 do
          Result[i] := qrs[i].GetRes;
      end, EventList.Combine(evs, tsk, c.Native, main_dvc, cq) ?? new EventList);
    end;
    
  end;
  
  {$endregion Generic}
  
  {$region [2]}
  
  ConvQueueArrayBase2<TInp1, TInp2, TRes> = abstract class(HostQueue<ValueTuple<TInp1, TInp2>, TRes>)
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected f: (TInp1, TInp2, Context)->TRes;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; f: (TInp1, TInp2, Context)->TRes);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      self.q1.RegisterWaitables(tsk, prev_hubs);
      self.q2.RegisterWaitables(tsk, prev_hubs);
    end;
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2>; c: Context): TRes; override := f(t.Item1, t.Item2, c);
    
  end;
  
  ConvSyncQueueArray2<TInp1, TInp2, TRes> = sealed class(ConvQueueArrayBase2<TInp1, TInp2, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2>>; override;
    begin
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr1.ev;
      var qr2 := q2.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr2.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes()), prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray2<TInp1, TInp2, TRes> = sealed class(ConvQueueArrayBase2<TInp1, TInp2, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2>>; override;
    begin
      if (prev_ev<>nil) and (prev_ev.count<>0) then loop 1 do prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev);
      var qr2 := q2.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes()), EventList.Combine(new EventList[](qr1.ev, qr2.ev), tsk, c.Native, main_dvc, cq));
    end;
    
  end;
  
  {$endregion [2]}
  
  {$region [3]}
  
  ConvQueueArrayBase3<TInp1, TInp2, TInp3, TRes> = abstract class(HostQueue<ValueTuple<TInp1, TInp2, TInp3>, TRes>)
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected f: (TInp1, TInp2, TInp3, Context)->TRes;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; f: (TInp1, TInp2, TInp3, Context)->TRes);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.q3 := q3;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      self.q1.RegisterWaitables(tsk, prev_hubs);
      self.q2.RegisterWaitables(tsk, prev_hubs);
      self.q3.RegisterWaitables(tsk, prev_hubs);
    end;
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, c);
    
  end;
  
  ConvSyncQueueArray3<TInp1, TInp2, TInp3, TRes> = sealed class(ConvQueueArrayBase3<TInp1, TInp2, TInp3, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3>>; override;
    begin
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr1.ev;
      var qr2 := q2.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr2.ev;
      var qr3 := q3.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr3.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes()), prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray3<TInp1, TInp2, TInp3, TRes> = sealed class(ConvQueueArrayBase3<TInp1, TInp2, TInp3, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3>>; override;
    begin
      if (prev_ev<>nil) and (prev_ev.count<>0) then loop 2 do prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev);
      var qr2 := q2.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr3 := q3.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes()), EventList.Combine(new EventList[](qr1.ev, qr2.ev, qr3.ev), tsk, c.Native, main_dvc, cq));
    end;
    
  end;
  
  {$endregion [3]}
  
  {$region [4]}
  
  ConvQueueArrayBase4<TInp1, TInp2, TInp3, TInp4, TRes> = abstract class(HostQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4>, TRes>)
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected f: (TInp1, TInp2, TInp3, TInp4, Context)->TRes;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; f: (TInp1, TInp2, TInp3, TInp4, Context)->TRes);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.q3 := q3;
      self.q4 := q4;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      self.q1.RegisterWaitables(tsk, prev_hubs);
      self.q2.RegisterWaitables(tsk, prev_hubs);
      self.q3.RegisterWaitables(tsk, prev_hubs);
      self.q4.RegisterWaitables(tsk, prev_hubs);
    end;
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, c);
    
  end;
  
  ConvSyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(ConvQueueArrayBase4<TInp1, TInp2, TInp3, TInp4, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4>>; override;
    begin
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr1.ev;
      var qr2 := q2.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr2.ev;
      var qr3 := q3.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr3.ev;
      var qr4 := q4.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr4.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes()), prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(ConvQueueArrayBase4<TInp1, TInp2, TInp3, TInp4, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4>>; override;
    begin
      if (prev_ev<>nil) and (prev_ev.count<>0) then loop 3 do prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev);
      var qr2 := q2.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr3 := q3.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr4 := q4.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes()), EventList.Combine(new EventList[](qr1.ev, qr2.ev, qr3.ev, qr4.ev), tsk, c.Native, main_dvc, cq));
    end;
    
  end;
  
  {$endregion [4]}
  
  {$region [5]}
  
  ConvQueueArrayBase5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = abstract class(HostQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>, TRes>)
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected q5: CommandQueue<TInp5>;
    protected f: (TInp1, TInp2, TInp3, TInp4, TInp5, Context)->TRes;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; f: (TInp1, TInp2, TInp3, TInp4, TInp5, Context)->TRes);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.q3 := q3;
      self.q4 := q4;
      self.q5 := q5;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      self.q1.RegisterWaitables(tsk, prev_hubs);
      self.q2.RegisterWaitables(tsk, prev_hubs);
      self.q3.RegisterWaitables(tsk, prev_hubs);
      self.q4.RegisterWaitables(tsk, prev_hubs);
      self.q5.RegisterWaitables(tsk, prev_hubs);
    end;
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, c);
    
  end;
  
  ConvSyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(ConvQueueArrayBase5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>; override;
    begin
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr1.ev;
      var qr2 := q2.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr2.ev;
      var qr3 := q3.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr3.ev;
      var qr4 := q4.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr4.ev;
      var qr5 := q5.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr5.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes()), prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(ConvQueueArrayBase5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>; override;
    begin
      if (prev_ev<>nil) and (prev_ev.count<>0) then loop 4 do prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev);
      var qr2 := q2.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr3 := q3.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr4 := q4.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr5 := q5.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes()), EventList.Combine(new EventList[](qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev), tsk, c.Native, main_dvc, cq));
    end;
    
  end;
  
  {$endregion [5]}
  
  {$region [6]}
  
  ConvQueueArrayBase6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = abstract class(HostQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>, TRes>)
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected q5: CommandQueue<TInp5>;
    protected q6: CommandQueue<TInp6>;
    protected f: (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context)->TRes;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; f: (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context)->TRes);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.q3 := q3;
      self.q4 := q4;
      self.q5 := q5;
      self.q6 := q6;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      self.q1.RegisterWaitables(tsk, prev_hubs);
      self.q2.RegisterWaitables(tsk, prev_hubs);
      self.q3.RegisterWaitables(tsk, prev_hubs);
      self.q4.RegisterWaitables(tsk, prev_hubs);
      self.q5.RegisterWaitables(tsk, prev_hubs);
      self.q6.RegisterWaitables(tsk, prev_hubs);
    end;
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, c);
    
  end;
  
  ConvSyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(ConvQueueArrayBase6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>; override;
    begin
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr1.ev;
      var qr2 := q2.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr2.ev;
      var qr3 := q3.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr3.ev;
      var qr4 := q4.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr4.ev;
      var qr5 := q5.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr5.ev;
      var qr6 := q6.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr6.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes()), prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(ConvQueueArrayBase6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>; override;
    begin
      if (prev_ev<>nil) and (prev_ev.count<>0) then loop 5 do prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev);
      var qr2 := q2.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr3 := q3.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr4 := q4.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr5 := q5.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr6 := q6.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes()), EventList.Combine(new EventList[](qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev, qr6.ev), tsk, c.Native, main_dvc, cq));
    end;
    
  end;
  
  {$endregion [6]}
  
  {$region [7]}
  
  ConvQueueArrayBase7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = abstract class(HostQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>, TRes>)
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected q5: CommandQueue<TInp5>;
    protected q6: CommandQueue<TInp6>;
    protected q7: CommandQueue<TInp7>;
    protected f: (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context)->TRes;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>; f: (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context)->TRes);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.q3 := q3;
      self.q4 := q4;
      self.q5 := q5;
      self.q6 := q6;
      self.q7 := q7;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      self.q1.RegisterWaitables(tsk, prev_hubs);
      self.q2.RegisterWaitables(tsk, prev_hubs);
      self.q3.RegisterWaitables(tsk, prev_hubs);
      self.q4.RegisterWaitables(tsk, prev_hubs);
      self.q5.RegisterWaitables(tsk, prev_hubs);
      self.q6.RegisterWaitables(tsk, prev_hubs);
      self.q7.RegisterWaitables(tsk, prev_hubs);
    end;
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, c);
    
  end;
  
  ConvSyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(ConvQueueArrayBase7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>; override;
    begin
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr1.ev;
      var qr2 := q2.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr2.ev;
      var qr3 := q3.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr3.ev;
      var qr4 := q4.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr4.ev;
      var qr5 := q5.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr5.ev;
      var qr6 := q6.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr6.ev;
      var qr7 := q7.Invoke(tsk, c, main_dvc, false, cq, prev_ev); prev_ev := qr7.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes(), qr7.GetRes()), prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(ConvQueueArrayBase7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>; override;
    begin
      if (prev_ev<>nil) and (prev_ev.count<>0) then loop 6 do prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qr1 := q1.Invoke(tsk, c, main_dvc, false, cq, prev_ev);
      var qr2 := q2.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr3 := q3.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr4 := q4.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr5 := q5.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr6 := q6.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      var qr7 := q7.InvokeNewQ(tsk, c, main_dvc, false, prev_ev);
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes(), qr7.GetRes()), EventList.Combine(new EventList[](qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev, qr6.ev, qr7.ev), tsk, c.Native, main_dvc, cq));
    end;
    
  end;
  
  {$endregion [7]}
  
  {$endregion Conv}
  
  {$region Utils}
  
  QueueArrayUtils = static class
    
    public static function FlattenQueueArray<T>(inp: sequence of CommandQueueBase): array of CommandQueueBase; where T: ISimpleQueueArray;
    begin
      var enmr := inp.GetEnumerator;
      if not enmr.MoveNext then raise new InvalidOperationException('Функции CombineSyncQueue/CombineAsyncQueue не могут принимать 0 очередей');
      
      var res := new List<CommandQueueBase>;
      while true do
      begin
        var curr := enmr.Current;
        var next := enmr.MoveNext;
        
        if not (curr is IConstQueue) or not next then
          if curr as object is T(var sqa) then //ToDo #2290
            res.AddRange(sqa.GetQS) else
            res += curr;
        
        if not next then break;
      end;
      
      Result := res.ToArray;
    end;
    
    public static function  FlattenSyncQueueArray(inp: sequence of CommandQueueBase) := FlattenQueueArray&< ISimpleSyncQueueArray>(inp);
    public static function FlattenAsyncQueueArray(inp: sequence of CommandQueueBase) := FlattenQueueArray&<ISimpleAsyncQueueArray>(inp);
    
  end;
  
  {$endregion}
  
  {$endregion Array}
  
  {$endregion CommandQueue}
  
  {$region WCQWaiter}
  
  WCQWaiter = abstract class
    private waitables: array of CommandQueueBase;
    
    public constructor(waitables: array of CommandQueueBase) := self.waitables := waitables;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    public procedure RegisterWaitables(tsk: CLTaskBase) :=
    foreach var q in waitables do q.RegisterWaiterTask(tsk);
    
    public function GetWaitEv(tsk: CLTaskBase; c: Context): UserEvent; abstract;
    
  end;
  
  WCQWaiterAll = sealed class(WCQWaiter)
    
    public function GetWaitEv(tsk: CLTaskBase; c: Context): UserEvent; override;
    begin
      var uev := tsk.MakeUserEvent(c.Native
        {$ifdef EventDebug}, $'WCQWaiterAll[{waitables.Length}]'{$endif}
      );
      
      var done := 0;
      var total := waitables.Length;
      var done_lock := new object;
      
      for var i := 0 to waitables.Length-1 do
        waitables[i].AddMWHandler(tsk, ()->
        begin
          if uev.CanRemove then exit;
          
          lock done_lock do
          begin
            done += 1;
            if done=total then
              // Если uev.Abort вызовет между .CanRemove и этой строчкой - значит это было в отдельном потоке,
              // т.е. в заведомо не_безопастном месте. А значит проверять тут - нет смысла
              uev.SetStatus(CommandExecutionStatus.COMPLETE);
          end;
          
          Result := true;
        end);
      
      Result := uev;
    end;
    
  end;
  WCQWaiterAny = sealed class(WCQWaiter)
    
    public function GetWaitEv(tsk: CLTaskBase; c: Context): UserEvent; override;
    begin
      var uev := tsk.MakeUserEvent(c.Native
        {$ifdef EventDebug}, $'WCQWaiterAny[{waitables.Length}]'{$endif}
      );
      
      for var i := 0 to waitables.Length-1 do
        waitables[i].AddMWHandler(tsk, ()->uev.SetStatus(CommandExecutionStatus.COMPLETE));
      
      Result := uev;
    end;
    
  end;
  
  {$endregion WCQWaiter}
  
  {$region GPUCommand}
  
  {$region Base}
  
  GPUCommand<T> = abstract class
    
    protected function InvokeObj  (o: T;                      tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): EventList; abstract;
    protected function InvokeQueue(o_q: ()->CommandQueue<T>;  tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): EventList; abstract;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); abstract;
    
  end;
  
  {$endregion Base}
  
  {$region Queue}
  
  QueueCommand<T> = sealed class(GPUCommand<T>)
    public q: CommandQueueBase;
    
    public constructor(q: CommandQueueBase) := self.q := q;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    private function Invoke(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList) := q.InvokeBase(tsk, c, main_dvc, false, cq, prev_ev).ev;
    
    protected function InvokeObj  (o: T;                      tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): EventList; override := Invoke(tsk, c, main_dvc, cq, prev_ev);
    protected function InvokeQueue(o_q: ()->CommandQueue<T>;  tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): EventList; override := Invoke(tsk, c, main_dvc, cq, prev_ev);
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override :=
    q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
  {$endregion Queue}
  
  {$region Proc}
  
  ProcCommand<T> = sealed class(GPUCommand<T>)
    public p: (T,Context)->();
    
    public constructor(p: (T,Context)->()) := self.p := p;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function InvokeObj(o: T; tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): EventList; override :=
    UserEvent.StartBackgroundWork(prev_ev, ()->p(o, c), c.Native, tsk
      {$ifdef EventDebug}, $'const body of {self.GetType}'{$endif}
    );
    
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): EventList; override;
    begin
      var o_q_res := o_q().Invoke(tsk, c, main_dvc, false, cq, prev_ev);
      Result := UserEvent.StartBackgroundWork(o_q_res.ev, ()->p(o_q_res.GetRes(), c), c.Native, tsk
        {$ifdef EventDebug}, $'queue body of {self.GetType}'{$endif}
      );
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override := exit;
    
  end;
  
  {$endregion Proc}
  
  {$region Wait}
  
  WaitCommand<T> = sealed class(GPUCommand<T>)
    public waiter: WCQWaiter;
    
    public constructor(waiter: WCQWaiter) := self.waiter := waiter;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    private function Invoke(tsk: CLTaskBase; c: Context; prev_ev: EventList): EventList;
    begin
      if prev_ev=nil then
        Result := waiter.GetWaitEv(tsk, c) else
        Result := prev_ev + waiter.GetWaitEv(tsk, c);
    end;
    
    protected function InvokeObj  (o: T;                      tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): EventList; override := Invoke(tsk, c, prev_ev);
    protected function InvokeQueue(o_q: ()->CommandQueue<T>;  tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): EventList; override := Invoke(tsk, c, prev_ev);
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override :=
    waiter.RegisterWaitables(tsk);
    
  end;
  
  {$endregion Wait}
  
  {$endregion GPUCommand}
  
  {$region GPUCommandContainer}
  
  {$region Base}
  
  GPUCommandContainer<T> = class;
  GPUCommandContainerBody<T> = abstract class
    private cc: GPUCommandContainer<T>;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>; abstract;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); abstract;
    
  end;
  
  GPUCommandContainer<T> = abstract class(CommandQueue<T>)
    protected body: GPUCommandContainerBody<T>;
    protected commands := new List<GPUCommand<T>>;
    
    {$region def}
    
    protected procedure InitObj(obj: T; c: Context); virtual := exit;
    
    {$endregion def}
    
    {$region Common}
    
    protected constructor(o: T);
    protected constructor(q: CommandQueue<T>);
    
    protected function MakeQueueCommand(q: CommandQueueBase) := new QueueCommand<T>(q);
    
    protected function MakeProcCommand(p: T->()) := new ProcCommand<T>((o,c)->p(o));
    protected function MakeProcCommand(p: (T,Context)->()) := new ProcCommand<T>(p);
    
    protected function MakeWaitAllCommand(qs: sequence of CommandQueueBase) := new WaitCommand<T>(new WCQWaiterAll(qs.ToArray));
    protected function MakeWaitAnyCommand(qs: sequence of CommandQueueBase) := new WaitCommand<T>(new WCQWaiterAny(qs.ToArray));
    
    {$endregion Common}
    
    {$region sub implementation}
    
    protected function InvokeImpl(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>; override :=
    body.Invoke(tsk, c, main_dvc, need_ptr_qr, cq, prev_ev);
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      body.RegisterWaitables(tsk, prev_hubs);
      foreach var comm in commands do comm.RegisterWaitables(tsk, prev_hubs);
    end;
    
    {$endregion sub implementation}
    
    {$region reintroduce методы}
    
    //ToDo #2145
    ///--
    public function Equals(obj: object): boolean; reintroduce := inherited Equals(obj);
    ///--
    public function ToString: string; reintroduce := inherited ToString();
    ///--
    public function GetType: System.Type; reintroduce := inherited GetType();
    ///--
    public function GetHashCode: integer; reintroduce := inherited GetHashCode();
    
    {$endregion reintroduce методы}
    
  end;
  
  {$endregion Base}
  
  {$region BufferCommandQueue}
  
  ///Представляет очередь-контейнер для команд GPU, применяемых к объекту типа Buffer
  BufferCommandQueue = sealed class(GPUCommandContainer<Buffer>)
    
    {$region constructor's}
    
    protected procedure InitObj(obj: Buffer; c: Context); override := obj.InitIfNeed(c);
    protected static function InitBuffer(b: Buffer; c: Context): Buffer;
    begin
      b.InitIfNeed(c);
      Result := b;
    end;
    
    ///Создаёт контейнер команд, который будет применять команды к указанному объекту
    public constructor(b: Buffer) := inherited;
    ///Создаёт контейнер команд, который будет применять команды к объекту, который вернёт указанная очередь
    ///За каждое одно выполнение контейнера - q выполнится ровно один раз
    public constructor(q: CommandQueue<Buffer>) :=
    inherited Create(q.ThenConvert(InitBuffer));
    
    {$endregion constructor's}
    
    {$region Non-command add's}
    
    protected function AddCommand(comm: GPUCommand<Buffer>): BufferCommandQueue;
    begin
      self.commands += comm;
      Result := self;
    end;
    
    {$region Queue}
    
    ///Добавляет выполнение очереди в список обычных команд для GPU
    public function AddQueue(q: CommandQueueBase) := AddCommand(MakeQueueCommand(q));
    
    {$endregion Queue}
    
    {$region Proc}
    
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    public function AddProc(p: Buffer->())            := AddCommand(MakeProcCommand(p));
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    public function AddProc(p: (Buffer, Context)->()) := AddCommand(MakeProcCommand(p));
    
    {$endregion Proc}
    
    {$region Wait}
    
    ///Добавляет ожидание сигнала выполненности от всех заданных очередей
    public function AddWaitAll(params qs: array of CommandQueueBase)  := AddCommand(MakeWaitAllCommand(qs));
    ///Добавляет ожидание сигнала выполненности от всех заданных очередей
    public function AddWaitAll(qs: sequence of CommandQueueBase)      := AddCommand(MakeWaitAllCommand(qs));
    
    ///Добавляет ожидание первого сигнала выполненности от одной из заданных очередей
    public function AddWaitAny(params qs: array of CommandQueueBase)  := AddCommand(MakeWaitAnyCommand(qs));
    ///Добавляет ожидание первого сигнала выполненности от одной из заданных очередей
    public function AddWaitAny(qs: sequence of CommandQueueBase)      := AddCommand(MakeWaitAnyCommand(qs));
    
    ///Добавляет ожидание сигнала выполненности от заданной очереди
    public function AddWait(q: CommandQueueBase) := AddWaitAll(q);
    
    {$endregion Wait}
    
    {$endregion Non-command add's}
    
    {$region 1#Write&Read}
    
    ///Заполняет весь буфер данными, находящимися по указанному адресу в RAM
    public function AddWriteData(ptr: CommandQueue<IntPtr>): BufferCommandQueue;
    
    ///Копирует всё содержимое буфера в RAM, по указанному адресу
    public function AddReadData(ptr: CommandQueue<IntPtr>): BufferCommandQueue;
    
    ///Заполняет часть буфер данными, находящимися по указанному адресу в RAM
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function AddWriteData(ptr: CommandQueue<IntPtr>; buff_offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///Копирует часть содержимого буфера в RAM, по указанному адресу
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function AddReadData(ptr: CommandQueue<IntPtr>; buff_offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///Заполняет весь буфер данными, находящимися по указанному адресу в RAM
    public function AddWriteData(ptr: pointer): BufferCommandQueue;
    
    ///Копирует всё содержимое буфера в RAM, по указанному адресу
    public function AddReadData(ptr: pointer): BufferCommandQueue;
    
    ///Заполняет часть буфер данными, находящимися по указанному адресу в RAM
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function AddWriteData(ptr: pointer; buff_offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///Копирует часть содержимого буфера в RAM, по указанному адресу
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function AddReadData(ptr: pointer; buff_offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///Записывает указанное значение размерного типа в начало буфера
    public function AddWriteValue<TRecord>(val: TRecord): BufferCommandQueue; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в буфер
    ///buff_offset указывает отступ от начала буфера, в байтах
    public function AddWriteValue<TRecord>(val: TRecord; buff_offset: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в начало буфера
    public function AddWriteValue<TRecord>(val: CommandQueue<TRecord>): BufferCommandQueue; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в буфер
    ///buff_offset указывает отступ от начала буфера, в байтах
    public function AddWriteValue<TRecord>(val: CommandQueue<TRecord>; buff_offset: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    ///Записывает весь массив в начало буфера
    public function AddWriteArray1<TRecord>(a: CommandQueue<array of TRecord>): BufferCommandQueue; where TRecord: record;
    
    ///Записывает весь массив в начало буфера
    public function AddWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): BufferCommandQueue; where TRecord: record;
    
    ///Записывает весь массив в начало буфера
    public function AddWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): BufferCommandQueue; where TRecord: record;
    
    ///Читает из буфера достаточно байт чтоб заполнить весь массив
    public function AddReadArray1<TRecord>(a: CommandQueue<array of TRecord>): BufferCommandQueue; where TRecord: record;
    
    ///Читает из буфера достаточно байт чтоб заполнить весь массив
    public function AddReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): BufferCommandQueue; where TRecord: record;
    
    ///Читает из буфера достаточно байт чтоб заполнить весь массив
    public function AddReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): BufferCommandQueue; where TRecord: record;
    
    ///Записывает указанный участок массива в буфер
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    public function AddWriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, buff_offset: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    ///Записывает указанный участок массива в буфер
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, buff_offset: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    ///Записывает указанный участок массива в буфер
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, buff_offset: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    ///Читает в буфер указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    public function AddReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, buff_offset: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    ///Читает в буфер указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, buff_offset: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    ///Читает в буфер указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, buff_offset: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    {$endregion 1#Write&Read}
    
    {$region 2#Fill}
    
    ///Читает pattern_len байт из RAM по указанному адресу и заполняет их копиями весь буфер
    public function AddFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): BufferCommandQueue;
    
    ///Читает pattern_len байт из RAM по указанному адресу и заполняет их копиями часть буфера
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, buff_offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///Заполняет весь буфер копиями указанного значения размерного типа
    public function AddFillValue<TRecord>(val: TRecord): BufferCommandQueue; where TRecord: record;
    
    ///Заполняет часть буфера копиями указанного значения размерного типа
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function AddFillValue<TRecord>(val: TRecord; buff_offset, len: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    ///Заполняет весь буфер копиями указанного значения размерного типа
    public function AddFillValue<TRecord>(val: CommandQueue<TRecord>): BufferCommandQueue; where TRecord: record;
    
    ///Заполняет часть буфера копиями указанного значения размерного типа
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function AddFillValue<TRecord>(val: CommandQueue<TRecord>; buff_offset, len: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    {$endregion 2#Fill}
    
    {$region 3#Copy}
    
    ///Копирует данные из текущего буфера в b
    ///Если буферы имеют разный размер - в качестве объёма данных берётся размер меньшего буфера
    public function AddCopyTo(b: CommandQueue<Buffer>): BufferCommandQueue;
    
    ///Копирует данные из b в текущий буфер
    ///Если буферы имеют разный размер - в качестве объёма данных берётся размер меньшего буфера
    public function AddCopyForm(b: CommandQueue<Buffer>): BufferCommandQueue;
    
    ///Копирует данные из текущего буфера в b
    ///from_pos указывает отступ в байтах от начала буфера, из которого копируют
    ///to_pos указывает отступ в байтах от начала буфера, в который копируют
    ///len указывает кол-во копируемых байт
    public function AddCopyTo(b: CommandQueue<Buffer>; from_pos, to_pos, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///Копирует данные из b в текущий буфер
    ///from_pos указывает отступ в байтах от начала буфера, из которого копируют
    ///to_pos указывает отступ в байтах от начала буфера, в который копируют
    ///len указывает кол-во копируемых байт
    public function AddCopyForm(b: CommandQueue<Buffer>; from_pos, to_pos, len: CommandQueue<integer>): BufferCommandQueue;
    
    {$endregion 3#Copy}
    
    {$region Get}
    
    ///Выделяет область неуправляемой памяти и копирует в неё всё содержимое данного буфера
    public function AddGetData: CommandQueue<IntPtr>;
    
    ///Выделяет область неуправляемой памяти и копирует в неё часть содержимого данного буфера
    ///buff_offset указывает отступ от начала буфера, в байтах
    ///len указывает кол-во задействованных байт буфера
    public function AddGetData(buff_offset, len: CommandQueue<integer>): CommandQueue<IntPtr>;
    
    ///Читает значение указанного размерного типа из начала буфера
    public function AddGetValue<TRecord>: CommandQueue<TRecord>; where TRecord: record;
    
    ///Читает значение указанного размерного типа из буфера
    ///buff_offset указывает отступ от начала буфера, в байтах
    public function AddGetValue<TRecord>(buff_offset: CommandQueue<integer>): CommandQueue<TRecord>; where TRecord: record;
    
    ///Создаёт массив максимального размера (на сколько хватит байт буфера) и копирует в него содержимое буфера
    public function AddGetArray1<TRecord>: CommandQueue<array of TRecord>; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое буфера
    public function AddGetArray1<TRecord>(len: CommandQueue<integer>): CommandQueue<array of TRecord>; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое буфера
    public function AddGetArray2<TRecord>(len1,len2: CommandQueue<integer>): CommandQueue<array[,] of TRecord>; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое буфера
    public function AddGetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): CommandQueue<array[,,] of TRecord>; where TRecord: record;
    
    {$endregion Get}
    
  end;
  
  {$endregion BufferCommandQueue}
  
  {$region KernelCommandQueue}
  
  ///Представляет очередь-контейнер для команд GPU, применяемых к объекту типа Kernel
  KernelCommandQueue = sealed class(GPUCommandContainer<Kernel>)
    
    {$region constructor's}
    
    ///Создаёт контейнер команд, который будет применять команды к указанному объекту
    public constructor(k: Kernel) := inherited;
    ///Создаёт контейнер команд, который будет применять команды к объекту, который вернёт указанная очередь
    ///За каждое одно выполнение контейнера - q выполнится ровно один раз
    public constructor(q: CommandQueue<Kernel>) := inherited;
    
    {$endregion constructor's}
    
    {$region Non-command add's}
    
    protected function AddCommand(comm: GPUCommand<Kernel>): KernelCommandQueue;
    begin
      self.commands += comm;
      Result := self;
    end;
    
    {$region Queue}
    
    ///Добавляет выполнение очереди в список обычных команд для GPU
    public function AddQueue(q: CommandQueueBase) := AddCommand(MakeQueueCommand(q));
    
    {$endregion Queue}
    
    {$region Proc}
    
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    public function AddProc(p: Kernel->())            := AddCommand(MakeProcCommand(p));
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    public function AddProc(p: (Kernel, Context)->()) := AddCommand(MakeProcCommand(p));
    
    {$endregion Proc}
    
    {$region Wait}
    
    ///Добавляет ожидание сигнала выполненности от всех заданных очередей
    public function AddWaitAll(params qs: array of CommandQueueBase)  := AddCommand(MakeWaitAllCommand(qs));
    ///Добавляет ожидание сигнала выполненности от всех заданных очередей
    public function AddWaitAll(qs: sequence of CommandQueueBase)      := AddCommand(MakeWaitAllCommand(qs));
    
    ///Добавляет ожидание первого сигнала выполненности от одной из заданных очередей
    public function AddWaitAny(params qs: array of CommandQueueBase)  := AddCommand(MakeWaitAnyCommand(qs));
    ///Добавляет ожидание первого сигнала выполненности от одной из заданных очередей
    public function AddWaitAny(qs: sequence of CommandQueueBase)      := AddCommand(MakeWaitAnyCommand(qs));
    
    ///Добавляет ожидание сигнала выполненности от заданной очереди
    public function AddWait(q: CommandQueueBase) := AddWaitAll(q);
    
    {$endregion Wait}
    
    {$endregion Non-command add's}
    
    {$region 1#Exec}
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function AddExec1(sz1: CommandQueue<integer>; params args: array of KernelArg): KernelCommandQueue;
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function AddExec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): KernelCommandQueue;
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function AddExec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): KernelCommandQueue;
    
    ///Выполняет kernel с расширенным набором параметров
    ///Данная перегрузка используется в первую очередь для тонких оптимизаций
    ///Если она вам понадобилась по другой причина - пожалуйста, напишите в issue
    public function AddExec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): KernelCommandQueue;
    
    {$endregion 1#Exec}
    
  end;
  
  {$endregion KernelCommandQueue}
  
  {$endregion GPUCommandContainer}
  
  {$region Enqueueable's}
  
  EnqueueableGPUCommand<T> = abstract class(GPUCommand<T>)
    
    // Если это True - InvokeParams должен возращать (...)->cl_event.Zero
    // Иначе останется ивент, который никто не удалил
    protected function NeedThread: boolean; virtual := false;
    
    private function MakeEvList(exp_size: integer; start_ev: EventList): List<EventList>;
    begin
      var need_start_ev := (start_ev<>nil) and (start_ev.count<>0);
      Result := new List<EventList>(exp_size + integer(need_start_ev));
      if need_start_ev then Result += start_ev;
    end;
    protected function ParamCountL1: integer; abstract;
    protected function ParamCountL2: integer; abstract;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (T, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; abstract;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_qr: QueueRes<T>; l2_start_ev: EventList): EventList;
    begin
      var need_thread := self.NeedThread;
      
      var evs_l1 := MakeEvList(ParamCountL1, prev_qr.ev); // ожидание до Enqueue
      var evs_l2 := MakeEvList(ParamCountL2, l2_start_ev); // ожидание, передаваемое в Enqueue
      
      var enq_f := InvokeParams(tsk, c, main_dvc, cq, evs_l1, evs_l2);
      var ev_l1 := EventList.Combine(evs_l1, tsk, c.Native, main_dvc, cq);
      var ev_l2 := EventList.Combine(evs_l2, tsk, c.Native, main_dvc, cq) ?? new EventList;
      
      NativeUtils.FixCQ(c.Native, main_dvc, cq);
      
      if not need_thread and (ev_l1=nil) then
      begin
        Result := enq_f(prev_qr.GetRes, cq, tsk, c, ev_l2);
        {$ifdef EventDebug}
        EventDebug.RegisterEventRetain(Result.evs.Single, $'Enq by {self.GetType}, waiting on [{ev_l2.evs?.JoinToString}]');
        {$endif EventDebug}
        Result.abortable := true; // ev_l2 тут всегда напрямую передаётся в cl.Enqueue*... и ev_l2.abortable всегда true
        //ToDo С другой стороны, если ev_l2 абортится - получаем CL_EXEC_STATUS_ERROR_FOR_EVENTS_IN_WAIT_LIST в качестве статуса
        // - Надо бы проверить как это всё работает
        // - Наверное надо возвращать в Result не только результат enq_f, но и ev_l2, чтоб SmartStatusErr видел другие ивенты
        // - Ну и не забыть в .Get* так же поменять. Или лучше объединить этот дубль кода, сколько можно...
      end else
      begin
        var res_ev: UserEvent;
        
        // Асинхронное Enqueue, придётся пересоздать cq
        var lcq := cq;
        cq := cl_command_queue.Zero;
        
        if need_thread then
          res_ev := UserEvent.StartBackgroundWork(ev_l1, ()->enq_f(prev_qr.GetRes, lcq, tsk, c, ev_l2), c.Native, tsk
            {$ifdef EventDebug}, $'enq of {self.GetType}'{$endif}
          ) else
        begin
          res_ev := tsk.MakeUserEvent(c.Native
            {$ifdef EventDebug}, $'{self.GetType}, temp for nested AttachCallback: [{ev_l1?.evs.JoinToString}], then [{ev_l2.evs?.JoinToString}]'{$endif}
          );
          
          //ВНИМАНИЕ "ev_l1=nil" не может случится, из за условий выше
          ev_l1.AttachCallback(()->
          begin
            ev_l1.Release({$ifdef EventDebug}$'after waiting before Enq of {self.GetType}'{$endif});
            var enq_ev := enq_f(prev_qr.GetRes, lcq, tsk, c, ev_l2);
            {$ifdef EventDebug}
            EventDebug.RegisterEventRetain(enq_ev, $'Enq by {self.GetType}, waiting on [{ev_l2.evs?.JoinToString}]');
            {$endif EventDebug}
            EventList.AttachCallback(enq_ev, ()->res_ev.SetStatus(CommandExecutionStatus.COMPLETE), tsk, true{$ifdef EventDebug}, $'propagating Enq ev of {self.GetType} to res_ev: {res_ev.uev}'{$endif});
          end, tsk, c.Native, main_dvc, lcq{$ifdef EventDebug}, $'calling Enq of {self.GetType}'{$endif});
          
        end;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          System.Threading.Tasks.Task.Run(()->tsk.AddErr(cl.ReleaseCommandQueue(lcq)));
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        Result := res_ev; //ВНИМАНИЕ: "Result.abortable" тут установлено автоматически
      end;
      
    end;
    
    protected function InvokeObj(o: T; tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): EventList; override :=
    Invoke(tsk, c, main_dvc, cq, new QueueResConst<T>(o, nil), prev_ev);
    
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): EventList; override :=
    Invoke(tsk, c, main_dvc, cq, o_q().Invoke(tsk, c, main_dvc, false, cq, prev_ev), nil);
    
  end;
  
  EnqueueableGetCommand<TObj, TRes> = abstract class(CommandQueue<TRes>)
    protected prev_commands: GPUCommandContainer<TObj>;
    
    public constructor(prev_commands: GPUCommandContainer<TObj>) :=
    self.prev_commands := prev_commands;
    
    // Если это True - InvokeParams должен возращать (...)->cl_event.Zero
    // Иначе останется ивент, который никто не удалил
    protected function NeedThread: boolean; virtual := false;
    
    protected function ForcePtrQr: boolean; virtual := false;
    
    private function MakeEvList(exp_size: integer; start_ev: EventList): List<EventList>;
    begin
      var need_start_ev := (start_ev<>nil) and (start_ev.count<>0);
      Result := new List<EventList>(exp_size + integer(need_start_ev));
      if need_start_ev then Result += start_ev;
    end;
    protected function ParamCountL1: integer; abstract;
    protected function ParamCountL2: integer; abstract;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (TObj, cl_command_queue, CLTaskBase, EventList, QueueResDelayedBase<TRes>)->cl_event; abstract;
    
    protected function InvokeImpl(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<TRes>; override;
    begin
      var need_thread := self.NeedThread;
      var prev_qr := prev_commands.Invoke(tsk, c, main_dvc, false, cq, prev_ev);
      
      var evs_l1 := MakeEvList(ParamCountL1, prev_qr.ev); // ожидание до Enqueue
      var evs_l2 := MakeEvList(ParamCountL2, nil); // ожидание, передаваемое в Enqueue
      
      var enq_f := InvokeParams(tsk, c, main_dvc, cq, evs_l1, evs_l2);
      var ev_l1 := EventList.Combine(evs_l1, tsk, c.Native, main_dvc, cq);
      var ev_l2 := EventList.Combine(evs_l2, tsk, c.Native, main_dvc, cq) ?? new EventList;
      
      NativeUtils.FixCQ(c.Native, main_dvc, cq);
      
      var qr := QueueResDelayedBase&<TRes>.MakeNew(need_ptr_qr or ForcePtrQr);
      Result := qr;
      
      if not need_thread and (ev_l1=nil) then
      begin
        Result.ev := enq_f(prev_qr.GetRes, cq, tsk, ev_l2, qr);
        {$ifdef EventDebug}
        EventDebug.RegisterEventRetain(Result.ev.evs.Single, $'Enq by {self.GetType}, waiting on [{ev_l2.evs?.JoinToString}]');
        {$endif EventDebug}
        Result.ev.abortable := true; //ToDo та же история что выше
      end else
      begin
        var res_ev: UserEvent;
        
        // Асинхронное Enqueue, придётся пересоздать cq
        var lcq := cq;
        cq := cl_command_queue.Zero;
        
        if need_thread then
          res_ev := UserEvent.StartBackgroundWork(ev_l1, ()->enq_f(prev_qr.GetRes, lcq, tsk, ev_l2, qr), c.Native, tsk
            {$ifdef EventDebug}, $'enq of {self.GetType}'{$endif}
          ) else
        begin
          res_ev := tsk.MakeUserEvent(c.Native
            {$ifdef EventDebug}, $'{self.GetType}, temp for nested AttachCallback: [{ev_l1?.evs.JoinToString}], then [{ev_l2.evs?.JoinToString}]'{$endif}
          );
          
          //ВНИМАНИЕ "ev_l1=nil" не может случится, из за условий выше
          ev_l1.AttachCallback(()->
          begin
            ev_l1.Release({$ifdef EventDebug}$'after waiting before Enq of {self.GetType}'{$endif});
            var enq_ev := enq_f(prev_qr.GetRes, lcq, tsk, ev_l2, qr);
            {$ifdef EventDebug}
            EventDebug.RegisterEventRetain(enq_ev, $'Enq by {self.GetType}, waiting on [{ev_l2.evs?.JoinToString}]');
            {$endif EventDebug}
            EventList.AttachCallback(enq_ev, ()->res_ev.SetStatus(CommandExecutionStatus.COMPLETE), tsk, true{$ifdef EventDebug}, $'propagating Enq ev of {self.GetType} to res_ev: {res_ev.uev}'{$endif});
          end, tsk, c.Native, main_dvc, lcq{$ifdef EventDebug}, $'calling Enq of {self.GetType}'{$endif});
          
        end;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          System.Threading.Tasks.Task.Run(()->tsk.AddErr(cl.ReleaseCommandQueue(lcq)));
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        Result.ev := res_ev; //ВНИМАНИЕ: "Result.abortable" тут установлено автоматически
      end;
      
    end;
    
  end;
  
  {$endregion Enqueueable's}
  
  {$region KernelArg}
  
  ISetableKernelArg = interface
    procedure SetArg(k: cl_kernel; ind: UInt32; c: Context);
  end;
  ///Представляет аргумент, передаваемый в вызов kernel-а
  KernelArg = abstract class
    
    {$region Def}
    
    protected function Invoke(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id): QueueRes<ISetableKernelArg>; abstract;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); abstract;
    
    {$endregion Def}
    
    {$region Buffer}
    
    ///Создаёт аргумент kernel-а, представляющий буфер
    public static function FromBuffer(b: Buffer): KernelArg;
    public static function operator implicit(b: Buffer): KernelArg := FromBuffer(b);
    
    ///Создаёт аргумент kernel-а, представляющий буфер
    public static function FromBufferCQ(bq: CommandQueue<Buffer>): KernelArg;
    public static function operator implicit(bq: CommandQueue<Buffer>): KernelArg := FromBufferCQ(bq);
    public static function operator implicit(bq: BufferCommandQueue): KernelArg := FromBufferCQ(bq as CommandQueue<Buffer>);
    
    {$endregion Buffer}
    
    {$region Record}
    
    ///Создаёт аргумент kernel-а, представляющий небольшое значение размерного типа
    public static function FromRecord<TRecord>(val: TRecord): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(val: TRecord): KernelArg; where TRecord: record; begin Result := FromRecord(val); end;
    
    ///Создаёт аргумент kernel-а, представляющий небольшое значение размерного типа
    public static function FromRecordCQ<TRecord>(valq: CommandQueue<TRecord>): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(valq: CommandQueue<TRecord>): KernelArg; where TRecord: record; begin Result := FromRecordCQ(valq); end;
    
    {$endregion Record}
    
    {$region Ptr}
    
    ///Создаёт аргумент kernel-а, представляющий адрес в неуправляемой памяти
    public static function FromPtr(ptr: IntPtr; sz: UIntPtr): KernelArg;
    
    ///Создаёт аргумент kernel-а, представляющий адрес в неуправляемой памяти
    public static function FromPtrCQ(ptr_q: CommandQueue<IntPtr>; sz_q: CommandQueue<UIntPtr>): KernelArg;
    
    ///Создаёт аргумент kernel-а, представляющий адрес размерного значения со стека
    ///Внимание! Адрес должен ссылаться именно на стек, иначе программа может время от времени падать с ошибками доступа к памяти
    ///Это значит, что передавать можно только адрес локальной переменной, не захваченной ни одной лямбдой
    public static function FromRecordPtr<TRecord>(ptr: ^TRecord): KernelArg; where TRecord: record; begin Result := FromPtr(new IntPtr(ptr), new UIntPtr(Marshal.SizeOf&<TRecord>)); end;
    public static function operator implicit<TRecord>(ptr: ^TRecord): KernelArg; where TRecord: record; begin Result := FromRecordPtr(ptr); end;
    
    {$endregion Ptr}
    
  end;
  
  {$endregion KernelArg}
  
implementation

{$region DelayedImpl}

{$region Device}

function Device.Split(props: array of DevicePartitionProperty): array of SubDevice;
begin
  if supported_split_modes=nil then supported_split_modes := Properties.PartitionType;
  if not supported_split_modes.Contains(props[0]) then
    raise new NotSupportedException($'Данный режим .Split не поддерживается выбранным устройством');
  
  var c: UInt32;
  cl.CreateSubDevices(self.Native, props, 0, IntPtr.Zero, c).RaiseIfError;
  
  var res := new cl_device_id[int64(c)];
  cl.CreateSubDevices(self.Native, props, c, res[0], IntPtr.Zero).RaiseIfError;
  
  Result := res.ConvertAll(sdvc->new SubDevice(sdvc, self));
end;

{$endregion Device}

{$region Buffer}

function Buffer.NewQueue := new BufferCommandQueue(self);

{$endregion Buffer}

{$region Kernel}

function Kernel.NewQueue := new KernelCommandQueue(self);

{$endregion Kernel}

{$region EventList}

static function EventList.Combine(evs: IList<EventList>; tsk: CLTaskBase; c: cl_context; main_dvc: cl_device_id; var cq: cl_command_queue): EventList;
begin
  var count := 0;
  var need_abort_ev := true;
  
  for var i := 0 to evs.Count-1 do
  begin
    count += evs[i].count;
    if need_abort_ev and evs[i].abortable then need_abort_ev := false;
  end;
  if count=0 then exit;
  
  Result := new EventList(count + integer(need_abort_ev));
  
  for var i := 0 to evs.Count-1 do
    Result += evs[i];
  
  if need_abort_ev then
  begin
    var uev := tsk.MakeUserEvent(c
      {$ifdef EventDebug}, $'abortability of EventList.Combine of: {Result.evs.Take(Result.count).JoinToString}'{$endif}
    );
    Result.AttachCallback(()->uev.SetStatus(CommandExecutionStatus.COMPLETE), tsk, c, main_dvc, cq
      {$ifdef EventDebug}, $'setting abort ev: {uev.uev}'{$endif}
    );
    Result += cl_event(uev);
    Result.abortable := true;
  end;
  
end;

{$endregion EventList}

{$region QueueRes}

static function QueueResDelayedBase<T>.MakeNew(need_ptr_qr: boolean) := need_ptr_qr ?
new QueueResDelayedPtr<T> as QueueResDelayedBase<T> :
new QueueResDelayedObj<T> as QueueResDelayedBase<T>;

{$endregion QueueRes}

{$region UserEvent}

static function UserEvent.MakeUserEvent(tsk: CLTaskBase; c: cl_context{$ifdef EventDebug}; reason: string{$endif}) := tsk.MakeUserEvent(c{$ifdef EventDebug}, reason{$endif});

{$endregion UserEvent}

{$region CLTask}

static procedure CLTaskExt.AddErr(tsk: CLTaskBase; e: Exception) := tsk.AddErr(e);
static function CLTaskExt.AddErr(tsk: CLTaskBase; ec: ErrorCode) := tsk.AddErr(ec);
static function CLTaskExt.AddErr(tsk: CLTaskBase; st: CommandExecutionStatus) := tsk.AddErr(st);

function CLTask<T>.OrgQueueBase: CommandQueueBase := self.OrgQueue;

procedure CLTask<T>.RegisterWaitables(q: CommandQueue<T>) := q.RegisterWaitables(self, new HashSet<MultiusableCommandQueueHubBase>);

function CLTask<T>.InvokeQueue(q: CommandQueue<T>; c: Context; var cq: cl_command_queue): QueueRes<T> :=
q.Invoke(self, c, c.MainDevice.Native, false, cq, nil);

{$endregion CLTask}

{$region CommandQueue}

static function CommandQueueBase.operator implicit(o: object): CommandQueueBase :=
new ConstQueue<object>(o);

static function CommandQueue<T>.operator implicit(o: T): CommandQueue<T> :=
new ConstQueue<T>(o);

{$endregion CommandQueue}

{$endregion DelayedImpl}

{$region CLTaskResLess}

type
  CLTaskResLess = sealed class(CLTaskBase)
    protected q: CommandQueueBase;
    protected q_res: object;
    
    protected function OrgQueueBase: CommandQueueBase; override := q;
    
    protected constructor(q: CommandQueueBase; c: Context);
    begin
      self.q := q;
      self.org_c := c;
      q.RegisterWaitables(self, new HashSet<MultiusableCommandQueueHubBase>);
      
      var cq: cl_command_queue;
      var qr := q.InvokeBase(self, c, c.MainDevice.Native, false, cq, nil);
      
      // mu выполняют лишний .Retain, чтобы ивент не удалился пока очередь ещё запускается
      foreach var mu_qr in mu_res.Values do
        mu_qr.ev.Release({$ifdef EventDebug}$'excessive mu ev'{$endif});
      mu_res := nil;
      
      {$ifdef DEBUG}
      //CQ.Invoke всегда выполняет UserEvent.EnsureAbortability, поэтому тут оно не нужно
      if (qr.ev.count<>0) and not qr.ev.abortable then raise new NotSupportedException;
      {$endif DEBUG}
      
      qr.ev.AttachFinallyCallback(()->
      begin
        qr.ev.Release({$ifdef EventDebug}$'last ev of CLTask'{$endif});
        if cq<>cl_command_queue.Zero then
          System.Threading.Tasks.Task.Run(()->self.AddErr( cl.ReleaseCommandQueue(cq) ));
        OnQDone(qr);
      end, self, c.Native, c.MainDevice.Native, cq{$ifdef EventDebug}, $'CLTaskResLess.OnQDone'{$endif});
      
    end;
    
    {$region CLTask event's}
    
    protected EvDone := new List<Action<CLTaskBase>>;
    protected procedure WhenDoneBase(cb: Action<CLTaskBase>); override :=
    if AddEventHandler(EvDone, cb) then cb(self);
    
    protected EvComplete := new List<Action<CLTaskBase, object>>;
    protected procedure WhenCompleteBase(cb: Action<CLTaskBase, object>); override :=
    if AddEventHandler(EvComplete, cb) then cb(self, q_res);
    
    protected EvError := new List<Action<CLTaskBase, array of Exception>>;
    protected procedure WhenErrorBase(cb: Action<CLTaskBase, array of Exception>); override :=
    if AddEventHandler(EvError, cb) then cb(self, GetErrArr);
    
    {$endregion CLTask event's}
    
    {$region Execution}
    
    private procedure OnQDone(qr: QueueResBase);
    begin
      var l_EvDone:     array of Action<CLTaskBase>;
      var l_EvComplete: array of Action<CLTaskBase, object>;
      var l_EvError:    array of Action<CLTaskBase, array of Exception>;
      
      lock wh_lock do
      try
        
        l_EvDone      := EvDone.ToArray;
        l_EvComplete  := EvComplete.ToArray;
        l_EvError     := EvError.ToArray;
        
        if err_lst.Count=0 then self.q_res := qr.GetResBase;
      finally
        wh.Set;
      end;
      
      foreach var ev in l_EvDone do
      try
        ev(self);
      except
        on e: Exception do AddErr(e);
      end;
      
      if err_lst.Count=0 then
      begin
        
        foreach var ev in l_EvComplete do
        try
          ev(self, self.q_res);
        except
          on e: Exception do AddErr(e);
        end;
        
      end else
      if l_EvError.Length<>0 then
      begin
        var err_arr := GetErrArr;
        
        foreach var ev in l_EvError do
        try
          ev(self, err_arr);
        except
          on e: Exception do AddErr(e);
        end;
        
      end;
      
    end;
    
    protected function WaitResBase: object; override;
    begin
      Wait;
      Result := q_res;
    end;
    
    {$endregion Execution}
    
  end;
  
function Context.BeginInvoke<T>(q: CommandQueue<T>) := new CLTask<T>(q, self);
function Context.BeginInvoke(q: CommandQueueBase) := new CLTaskResLess(q, self);

function Context.SyncInvoke<T>(q: CommandQueue<T>) := BeginInvoke(q).WaitRes;
function Context.SyncInvoke(q: CommandQueueBase) := BeginInvoke(q).WaitRes;

{$endregion CLTaskResLess}

{$region Queue converter's}

{$region Cast}

type
  CastQueue<T> = sealed class(CommandQueue<T>)
    private q: CommandQueueBase;
    
    public constructor(q: CommandQueueBase) := self.q := q;
    
    protected function InvokeImpl(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>; override :=
    q.InvokeBase(tsk, c, main_dvc, false, cq, prev_ev).LazyQuickTransformBase(o->T(o));
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override :=
    q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
function CommandQueueBase.Cast<T> := new CastQueue<T>(self);

{$endregion Cast}

{$region ThenConvert}

type
  CommandQueueThenConvert<TInp, TRes> = sealed class(HostQueue<TInp, TRes>)
    protected q: CommandQueue<TInp>;
    protected f: (TInp, Context)->TRes;
    
    public constructor(q: CommandQueue<TInp>; f: (TInp, Context)->TRes);
    begin
      self.q := q;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override :=
    q.RegisterWaitables(tsk, prev_hubs);
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<TInp>; override :=
    q.Invoke(tsk, c, main_dvc, false, cq, prev_ev);
    
    protected function ExecFunc(o: TInp; c: Context): TRes; override := f(o, c);
    
  end;
  
function CommandQueue<T>.ThenConvert<TOtp>(f: (T, Context)->TOtp) :=
new CommandQueueThenConvert<T, TOtp>(self, f);

{$endregion ThenConvert}

{$region QueueArray}

static function CommandQueue<T>.operator+(q1: CommandQueueBase; q2: CommandQueue<T>) := new SimpleSyncQueueArray<T>(q1, q2);
static function CommandQueue<T>.operator*(q1: CommandQueueBase; q2: CommandQueue<T>) := new SimpleAsyncQueueArray<T>(q1, q2);

{$endregion QueueArray}

{$region Multiusable}

type
  MultiusableCommandQueueHub<T> = sealed class(MultiusableCommandQueueHubBase)
    public q: CommandQueue<T>;
    public constructor(q: CommandQueue<T>) := self.q := q;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    public function OnNodeInvoked(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean): QueueRes<T>;
    begin
      
      var res_o: QueueResBase;
      if tsk.mu_res.TryGetValue(self, res_o) then
        Result := QueueRes&<T>( res_o ) else
      begin
        Result := self.q.InvokeNewQ(tsk, c, main_dvc, need_ptr_qr, nil);
        Result.can_set_ev := false;
        tsk.mu_res[self] := Result;
      end;
      
      Result.ev.Retain({$ifdef EventDebug}$'for all mu branches'{$endif});
    end;
    
    public function MakeNode: CommandQueue<T>;
    
  end;
  
  MultiusableCommandQueueNode<T> = sealed class(CommandQueue<T>)
    public hub: MultiusableCommandQueueHub<T>;
    public constructor(hub: MultiusableCommandQueueHub<T>) := self.hub := hub;
    
    protected function InvokeImpl(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>; override;
    begin
      Result := hub.OnNodeInvoked(tsk, c, main_dvc, need_ptr_qr);
      if prev_ev<>nil then Result := Result.TrySetEv( prev_ev + Result.ev );
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override :=
    if prev_hubs.Add(hub) then hub.q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
function MultiusableCommandQueueHub<T>.MakeNode :=
new MultiusableCommandQueueNode<T>(self);

function CommandQueue<T>.Multiusable: ()->CommandQueue<T> := MultiusableCommandQueueHub&<T>.Create(self).MakeNode;

{$endregion Multiusable}

{$region ThenWait}

type
  CommandQueueThenWaitFor<T> = sealed class(CommandQueue<T>)
    public q: CommandQueue<T>;
    public waiter: WCQWaiter;
    
    public constructor(q: CommandQueue<T>; waiter: WCQWaiter);
    begin
      self.q := q;
      self.waiter := waiter;
    end;
    
    protected function InvokeImpl(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>; override;
    begin
      Result := q.Invoke(tsk, c, main_dvc, need_ptr_qr, cq, prev_ev);
      Result := Result.TrySetEv( Result.ev + waiter.GetWaitEv(tsk, c) );
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      q.RegisterWaitables(tsk, prev_hubs);
      waiter.RegisterWaitables(tsk);
    end;
    
  end;
  
function CommandQueue<T>.ThenWaitForAll(qs: sequence of CommandQueueBase) :=
new CommandQueueThenWaitFor<T>(self, new WCQWaiterAll(qs.ToArray));

function CommandQueue<T>.ThenWaitForAny(qs: sequence of CommandQueueBase) :=
new CommandQueueThenWaitFor<T>(self, new WCQWaiterAny(qs.ToArray));

{$endregion ThenWait}

{$endregion Queue converter's}

{$region GPUCommandContainerBody}

type
  CCBObj<T> = sealed class(GPUCommandContainerBody<T>)
    public o: T;
    
    public constructor(o: T; cc: GPUCommandContainer<T>);
    begin
      self.o := o;
      self.cc := cc;
    end;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>; override;
    begin
      var res_obj := self.o;
      cc.InitObj(res_obj, c);
      
      foreach var comm in cc.commands do
        prev_ev := comm.InvokeObj(res_obj, tsk, c, main_dvc, cq, prev_ev);
      
      Result := new QueueResConst<T>(res_obj, prev_ev ?? new EventList);
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override := exit;
    
  end;
  CCBQueue<T> = sealed class(GPUCommandContainerBody<T>)
    public hub: MultiusableCommandQueueHub<T>;
    
    public constructor(q: CommandQueue<T>; cc: GPUCommandContainer<T>);
    begin
      self.hub := new MultiusableCommandQueueHub<T>(q);
      self.cc := cc;
    end;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<T>; override;
    begin
      var new_plug: ()->CommandQueue<T> := hub.MakeNode;
      // new_plub всегда делает mu ноду, а она не использует prev_ev
      // это тут, чтобы хаб передал need_ptr_qr. Он делает это при первом Invoke
      Result := new_plug().Invoke(tsk, c, main_dvc, need_ptr_qr, cq, nil);
      
      foreach var comm in cc.commands do
        prev_ev := comm.InvokeQueue(new_plug, tsk, c, main_dvc, cq, prev_ev);
      
      Result := Result.TrySetEv( prev_ev ?? new EventList );
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override :=
    hub.q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
constructor GPUCommandContainer<T>.Create(o: T) :=
self.body := new CCBObj<T>(o, self);

constructor GPUCommandContainer<T>.Create(q: CommandQueue<T>) :=
self.body := new CCBQueue<T>(q, self);

{$endregion GPUCommandContainerBody}

{$region KernelArg}

{$region Const}

{$region Base}

type
  ConstKernelArg = abstract class(KernelArg, ISetableKernelArg)
    
    protected function Invoke(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id): QueueRes<ISetableKernelArg>; override :=
    new QueueResConst<ISetableKernelArg>(self, new EventList);
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override := exit;
    
    public procedure SetArg(k: cl_kernel; ind: UInt32; c: Context); abstract;
    
  end;
  
{$endregion Base}

{$region Buffer}

type
  KernelArgBuffer = sealed class(ConstKernelArg)
    private b: Buffer;
    
    public constructor(b: Buffer) := self.b := b;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    public procedure SetArg(k: cl_kernel; ind: UInt32; c: Context); override;
    begin
      b.InitIfNeed(c);
      cl.SetKernelArg(k, ind, new UIntPtr(cl_mem.Size), b.ntv).RaiseIfError; 
    end;
    
  end;
  
static function KernelArg.FromBuffer(b: Buffer) := new KernelArgBuffer(b) as KernelArg; //ToDo #1981

{$endregion Buffer}

{$region Record}

type
  KernelArgRecord<TRecord> = sealed class(ConstKernelArg)
  where TRecord: record;
    private val: ^TRecord := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>));
    
    public constructor(val: TRecord) := self.val^ := val;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected procedure Finalize; override :=
    Marshal.FreeHGlobal(new IntPtr(val));
    
    public procedure SetArg(k: cl_kernel; ind: UInt32; c: Context); override :=
    cl.SetKernelArg(k, ind, new UIntPtr(Marshal.SizeOf&<TRecord>), pointer(self.val)).RaiseIfError; 
    
  end;
  
static function KernelArg.FromRecord<TRecord>(val: TRecord) := new KernelArgRecord<TRecord>(val) as KernelArg; //ToDo #1981

{$endregion Record}

{$region Ptr}

type
  KernelArgPtr = sealed class(ConstKernelArg)
    private ptr: IntPtr;
    private sz: UIntPtr;
    
    public constructor(ptr: IntPtr; sz: UIntPtr);
    begin
      self.ptr := ptr;
      self.sz := sz;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    public procedure SetArg(k: cl_kernel; ind: UInt32; c: Context); override :=
    cl.SetKernelArg(k, ind, sz, pointer(ptr)).RaiseIfError;
    
  end;
  
static function KernelArg.FromPtr(ptr: IntPtr; sz: UIntPtr) := new KernelArgPtr(ptr, sz) as KernelArg; //ToDo #1981

{$endregion Ptr}

{$endregion Const}

{$region Invokeable}

{$region Base}

type
  InvokeableKernelArg = abstract class(KernelArg) end;
  
{$endregion Base}

{$region Buffer}

type
  KernelArgBufferCQ = sealed class(InvokeableKernelArg)
    public q: CommandQueue<Buffer>;
    public constructor(q: CommandQueue<Buffer>) := self.q := q;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function Invoke(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id): QueueRes<ISetableKernelArg>; override :=
    q.InvokeNewQ(tsk, c, main_dvc, false, nil).LazyQuickTransform(b->new KernelArgBuffer(b) as ConstKernelArg as ISetableKernelArg); //ToDo #2289
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override :=
    q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
static function KernelArg.FromBufferCQ(bq: CommandQueue<Buffer>) :=
new KernelArgBufferCQ(bq) as KernelArg;

{$endregion Buffer}

{$region Record}

type
  KernelArgRecordQR<TRecord> = sealed class(ISetableKernelArg)
  where TRecord: record;
    public qr: QueueRes<TRecord>;
    public constructor(qr: QueueRes<TRecord>) := self.qr := qr;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    public procedure SetArg(k: cl_kernel; ind: UInt32; c: Context);
    begin
      var sz := new UIntPtr(Marshal.SizeOf&<TRecord>);
      if qr is QueueResDelayedPtr<TRecord>(var pqr) then
        cl.SetKernelArg(k, ind, sz, pointer(pqr.ptr)).RaiseIfError else
      begin
        var val := qr.GetRes;
        cl.SetKernelArg(k, ind, sz, val).RaiseIfError;
      end;
    end;
    
  end;
  KernelArgRecordCQ<TRecord> = sealed class(InvokeableKernelArg)
  where TRecord: record;
    public q: CommandQueue<TRecord>;
    public constructor(q: CommandQueue<TRecord>) := self.q := q;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function Invoke(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id): QueueRes<ISetableKernelArg>; override;
    begin
      var prev_qr := q.InvokeNewQ(tsk, c, main_dvc, true, nil);
      Result := new QueueResConst<ISetableKernelArg>(new KernelArgRecordQR<TRecord>(prev_qr), prev_qr.ev);
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override :=
    q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
static function KernelArg.FromRecordCQ<TRecord>(valq: CommandQueue<TRecord>) :=
new KernelArgRecordCQ<TRecord>(valq) as KernelArg;

{$endregion Record}

{$region Ptr}

type
  KernelArgPtrCQ = sealed class(InvokeableKernelArg)
    public ptr_q: CommandQueue<IntPtr>;
    public sz_q: CommandQueue<UIntPtr>;
    public constructor(ptr_q: CommandQueue<IntPtr>; sz_q: CommandQueue<UIntPtr>);
    begin
      self.ptr_q := ptr_q;
      self.sz_q := sz_q;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function Invoke(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id): QueueRes<ISetableKernelArg>; override;
    begin
      var ptr_qr  := ptr_q.InvokeNewQ(tsk, c, main_dvc, false, nil);
      var sz_qr   :=  sz_q.InvokeNewQ(tsk, c, main_dvc, false, nil);
      Result := new QueueResFunc<ISetableKernelArg>(()->new KernelArgPtr(ptr_qr.GetRes, sz_qr.GetRes), ptr_qr.ev+sz_qr.ev);
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      ptr_q.RegisterWaitables(tsk, prev_hubs);
       sz_q.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
static function KernelArg.FromPtrCQ(ptr_q: CommandQueue<IntPtr>; sz_q: CommandQueue<UIntPtr>) :=
new KernelArgPtrCQ(ptr_q, sz_q) as KernelArg;

{$endregion Ptr}

{$endregion Invokeable}

{$endregion KernelArg}

{$region CommonCommands}

{$region BufferCQ}

{$region 1#Write&Read}

{$region WriteDataAutoSize}

type
  BufferCommandWriteDataAutoSize = sealed class(EnqueueableGPUCommand<Buffer>)
    private ptr: CommandQueue<IntPtr>;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>);
    begin
      self.ptr := ptr;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var ptr_qr := ptr.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += ptr_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, o.Size,
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      ptr.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion WriteDataAutoSize}

function BufferCommandQueue.AddWriteData(ptr: CommandQueue<IntPtr>): BufferCommandQueue :=
AddCommand(new BufferCommandWriteDataAutoSize(ptr));

{$region ReadDataAutoSize}

type
  BufferCommandReadDataAutoSize = sealed class(EnqueueableGPUCommand<Buffer>)
    private ptr: CommandQueue<IntPtr>;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>);
    begin
      self.ptr := ptr;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var ptr_qr := ptr.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += ptr_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, o.Size,
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      ptr.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion ReadDataAutoSize}

function BufferCommandQueue.AddReadData(ptr: CommandQueue<IntPtr>): BufferCommandQueue :=
AddCommand(new BufferCommandReadDataAutoSize(ptr));

{$region WriteData}

type
  BufferCommandWriteData = sealed class(EnqueueableGPUCommand<Buffer>)
    private         ptr: CommandQueue<IntPtr>;
    private buff_offset: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    
    protected function ParamCountL1: integer; override := 3;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; buff_offset, len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.buff_offset := buff_offset;
      self.        len :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var         ptr_qr :=         ptr.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=         ptr_qr.ev;
      var buff_offset_qr := buff_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += buff_offset_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var buff_offset := buff_offset_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(buff_offset), new UIntPtr(len),
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
              ptr.RegisterWaitables(tsk, prev_hubs);
      buff_offset.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion WriteData}

function BufferCommandQueue.AddWriteData(ptr: CommandQueue<IntPtr>; buff_offset, len: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandWriteData(ptr, buff_offset, len));

{$region ReadData}

type
  BufferCommandReadData = sealed class(EnqueueableGPUCommand<Buffer>)
    private         ptr: CommandQueue<IntPtr>;
    private buff_offset: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    
    protected function ParamCountL1: integer; override := 3;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; buff_offset, len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.buff_offset := buff_offset;
      self.        len :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var         ptr_qr :=         ptr.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=         ptr_qr.ev;
      var buff_offset_qr := buff_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += buff_offset_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var buff_offset := buff_offset_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(buff_offset), new UIntPtr(len),
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
              ptr.RegisterWaitables(tsk, prev_hubs);
      buff_offset.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion ReadData}

function BufferCommandQueue.AddReadData(ptr: CommandQueue<IntPtr>; buff_offset, len: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandReadData(ptr, buff_offset, len));

function BufferCommandQueue.AddWriteData(ptr: pointer): BufferCommandQueue :=
AddWriteData(IntPtr(ptr));

function BufferCommandQueue.AddReadData(ptr: pointer): BufferCommandQueue :=
AddReadData(IntPtr(ptr));

function BufferCommandQueue.AddWriteData(ptr: pointer; buff_offset, len: CommandQueue<integer>): BufferCommandQueue :=
AddWriteData(IntPtr(ptr), buff_offset, len);

function BufferCommandQueue.AddReadData(ptr: pointer; buff_offset, len: CommandQueue<integer>): BufferCommandQueue :=
AddReadData(IntPtr(ptr), buff_offset, len);

function BufferCommandQueue.AddWriteValue<TRecord>(val: TRecord): BufferCommandQueue :=
AddWriteValue(val, 0);

{$region WriteValue}

type
  BufferCommandWriteValue<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private         val: ^TRecord := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>));
    private buff_offset: CommandQueue<integer>;
    
    protected procedure Finalize; override;
    begin
      Marshal.FreeHGlobal(new IntPtr(val));
    end;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(val: TRecord; buff_offset: CommandQueue<integer>);
    begin
      self.        val^ :=         val;
      self.buff_offset  := buff_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var buff_offset_qr := buff_offset.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += buff_offset_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var buff_offset := buff_offset_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(buff_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr(val),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      buff_offset.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion WriteValue}

function BufferCommandQueue.AddWriteValue<TRecord>(val: TRecord; buff_offset: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandWriteValue<TRecord>(val, buff_offset));

function BufferCommandQueue.AddWriteValue<TRecord>(val: CommandQueue<TRecord>): BufferCommandQueue :=
AddWriteValue(val, 0);

{$region WriteValueQ}

type
  BufferCommandWriteValueQ<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private         val: CommandQueue<TRecord>;
    private buff_offset: CommandQueue<integer>;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 1;
    
    public constructor(val: CommandQueue<TRecord>; buff_offset: CommandQueue<integer>);
    begin
      self.        val :=         val;
      self.buff_offset := buff_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var         val_qr :=         val.Invoke    (tsk, c, main_dvc,  True, cq, nil); evs_l2 +=         val_qr.ev;
      var buff_offset_qr := buff_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += buff_offset_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var         val :=         val_qr.ToPtr;
        var buff_offset := buff_offset_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(buff_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr(val.GetPtr),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
          val_hnd.Free;
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
              val.RegisterWaitables(tsk, prev_hubs);
      buff_offset.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion WriteValueQ}

function BufferCommandQueue.AddWriteValue<TRecord>(val: CommandQueue<TRecord>; buff_offset: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandWriteValueQ<TRecord>(val, buff_offset));

{$region WriteArray1AutoSize}

type
  BufferCommandWriteArray1AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private a: CommandQueue<array of TRecord>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var a_qr := a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += a_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var a := a_qr.GetRes;
        
        try
          cl.EnqueueWriteBuffer(
            cq, o.Native, Bool.BLOCKING,
            UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
            a[0],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      a.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion WriteArray1AutoSize}

function BufferCommandQueue.AddWriteArray1<TRecord>(a: CommandQueue<array of TRecord>): BufferCommandQueue :=
AddCommand(new BufferCommandWriteArray1AutoSize<TRecord>(a));

{$region WriteArray2AutoSize}

type
  BufferCommandWriteArray2AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private a: CommandQueue<array[,] of TRecord>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array[,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var a_qr := a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += a_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var a := a_qr.GetRes;
        
        try
          cl.EnqueueWriteBuffer(
            cq, o.Native, Bool.BLOCKING,
            UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
            a[0,0],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      a.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion WriteArray2AutoSize}

function BufferCommandQueue.AddWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): BufferCommandQueue :=
AddCommand(new BufferCommandWriteArray2AutoSize<TRecord>(a));

{$region WriteArray3AutoSize}

type
  BufferCommandWriteArray3AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private a: CommandQueue<array[,,] of TRecord>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array[,,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var a_qr := a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += a_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var a := a_qr.GetRes;
        
        try
          cl.EnqueueWriteBuffer(
            cq, o.Native, Bool.BLOCKING,
            UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
            a[0,0,0],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      a.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion WriteArray3AutoSize}

function BufferCommandQueue.AddWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): BufferCommandQueue :=
AddCommand(new BufferCommandWriteArray3AutoSize<TRecord>(a));

{$region ReadArray1AutoSize}

type
  BufferCommandReadArray1AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private a: CommandQueue<array of TRecord>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var a_qr := a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += a_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var a := a_qr.GetRes;
        
        try
          cl.EnqueueReadBuffer(
            cq, o.Native, Bool.BLOCKING,
            UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
            a[0],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      a.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion ReadArray1AutoSize}

function BufferCommandQueue.AddReadArray1<TRecord>(a: CommandQueue<array of TRecord>): BufferCommandQueue :=
AddCommand(new BufferCommandReadArray1AutoSize<TRecord>(a));

{$region ReadArray2AutoSize}

type
  BufferCommandReadArray2AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private a: CommandQueue<array[,] of TRecord>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array[,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var a_qr := a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += a_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var a := a_qr.GetRes;
        
        try
          cl.EnqueueReadBuffer(
            cq, o.Native, Bool.BLOCKING,
            UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
            a[0,0],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      a.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion ReadArray2AutoSize}

function BufferCommandQueue.AddReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): BufferCommandQueue :=
AddCommand(new BufferCommandReadArray2AutoSize<TRecord>(a));

{$region ReadArray3AutoSize}

type
  BufferCommandReadArray3AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private a: CommandQueue<array[,,] of TRecord>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array[,,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var a_qr := a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += a_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var a := a_qr.GetRes;
        
        try
          cl.EnqueueReadBuffer(
            cq, o.Native, Bool.BLOCKING,
            UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
            a[0,0,0],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      a.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion ReadArray3AutoSize}

function BufferCommandQueue.AddReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): BufferCommandQueue :=
AddCommand(new BufferCommandReadArray3AutoSize<TRecord>(a));

{$region WriteArray1}

type
  BufferCommandWriteArray1<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private           a: CommandQueue<array of TRecord>;
    private    a_offset: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    private buff_offset: CommandQueue<integer>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 4;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array of TRecord>; a_offset, len, buff_offset: CommandQueue<integer>);
    begin
      self.          a :=           a;
      self.   a_offset :=    a_offset;
      self.        len :=         len;
      self.buff_offset := buff_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var           a_qr :=           a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=           a_qr.ev;
      var    a_offset_qr :=    a_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=    a_offset_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      var buff_offset_qr := buff_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += buff_offset_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var           a :=           a_qr.GetRes;
        var    a_offset :=    a_offset_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var buff_offset := buff_offset_qr.GetRes;
        
        try
          cl.EnqueueWriteBuffer(
            cq, o.Native, Bool.BLOCKING,
            new UIntPtr(buff_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
            a[a_offset],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
                a.RegisterWaitables(tsk, prev_hubs);
         a_offset.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
      buff_offset.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion WriteArray1}

function BufferCommandQueue.AddWriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, buff_offset: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandWriteArray1<TRecord>(a, a_offset, len, buff_offset));

{$region WriteArray2}

type
  BufferCommandWriteArray2<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private           a: CommandQueue<array[,] of TRecord>;
    private   a_offset1: CommandQueue<integer>;
    private   a_offset2: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    private buff_offset: CommandQueue<integer>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 5;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, buff_offset: CommandQueue<integer>);
    begin
      self.          a :=           a;
      self.  a_offset1 :=   a_offset1;
      self.  a_offset2 :=   a_offset2;
      self.        len :=         len;
      self.buff_offset := buff_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var           a_qr :=           a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=           a_qr.ev;
      var   a_offset1_qr :=   a_offset1.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   a_offset1_qr.ev;
      var   a_offset2_qr :=   a_offset2.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   a_offset2_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      var buff_offset_qr := buff_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += buff_offset_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var           a :=           a_qr.GetRes;
        var   a_offset1 :=   a_offset1_qr.GetRes;
        var   a_offset2 :=   a_offset2_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var buff_offset := buff_offset_qr.GetRes;
        
        try
          cl.EnqueueWriteBuffer(
            cq, o.Native, Bool.BLOCKING,
            new UIntPtr(buff_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
            a[a_offset1,a_offset2],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
                a.RegisterWaitables(tsk, prev_hubs);
        a_offset1.RegisterWaitables(tsk, prev_hubs);
        a_offset2.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
      buff_offset.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion WriteArray2}

function BufferCommandQueue.AddWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, buff_offset: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandWriteArray2<TRecord>(a, a_offset1, a_offset2, len, buff_offset));

{$region WriteArray3}

type
  BufferCommandWriteArray3<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private           a: CommandQueue<array[,,] of TRecord>;
    private   a_offset1: CommandQueue<integer>;
    private   a_offset2: CommandQueue<integer>;
    private   a_offset3: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    private buff_offset: CommandQueue<integer>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 6;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, buff_offset: CommandQueue<integer>);
    begin
      self.          a :=           a;
      self.  a_offset1 :=   a_offset1;
      self.  a_offset2 :=   a_offset2;
      self.  a_offset3 :=   a_offset3;
      self.        len :=         len;
      self.buff_offset := buff_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var           a_qr :=           a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=           a_qr.ev;
      var   a_offset1_qr :=   a_offset1.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   a_offset1_qr.ev;
      var   a_offset2_qr :=   a_offset2.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   a_offset2_qr.ev;
      var   a_offset3_qr :=   a_offset3.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   a_offset3_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      var buff_offset_qr := buff_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += buff_offset_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var           a :=           a_qr.GetRes;
        var   a_offset1 :=   a_offset1_qr.GetRes;
        var   a_offset2 :=   a_offset2_qr.GetRes;
        var   a_offset3 :=   a_offset3_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var buff_offset := buff_offset_qr.GetRes;
        
        try
          cl.EnqueueWriteBuffer(
            cq, o.Native, Bool.BLOCKING,
            new UIntPtr(buff_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
            a[a_offset1,a_offset2,a_offset3],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
                a.RegisterWaitables(tsk, prev_hubs);
        a_offset1.RegisterWaitables(tsk, prev_hubs);
        a_offset2.RegisterWaitables(tsk, prev_hubs);
        a_offset3.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
      buff_offset.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion WriteArray3}

function BufferCommandQueue.AddWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, buff_offset: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandWriteArray3<TRecord>(a, a_offset1, a_offset2, a_offset3, len, buff_offset));

{$region ReadArray1}

type
  BufferCommandReadArray1<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private           a: CommandQueue<array of TRecord>;
    private    a_offset: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    private buff_offset: CommandQueue<integer>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 4;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array of TRecord>; a_offset, len, buff_offset: CommandQueue<integer>);
    begin
      self.          a :=           a;
      self.   a_offset :=    a_offset;
      self.        len :=         len;
      self.buff_offset := buff_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var           a_qr :=           a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=           a_qr.ev;
      var    a_offset_qr :=    a_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=    a_offset_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      var buff_offset_qr := buff_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += buff_offset_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var           a :=           a_qr.GetRes;
        var    a_offset :=    a_offset_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var buff_offset := buff_offset_qr.GetRes;
        
        try
          cl.EnqueueReadBuffer(
            cq, o.Native, Bool.BLOCKING,
            new UIntPtr(buff_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
            a[a_offset],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
                a.RegisterWaitables(tsk, prev_hubs);
         a_offset.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
      buff_offset.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion ReadArray1}

function BufferCommandQueue.AddReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, buff_offset: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandReadArray1<TRecord>(a, a_offset, len, buff_offset));

{$region ReadArray2}

type
  BufferCommandReadArray2<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private           a: CommandQueue<array[,] of TRecord>;
    private   a_offset1: CommandQueue<integer>;
    private   a_offset2: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    private buff_offset: CommandQueue<integer>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 5;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, buff_offset: CommandQueue<integer>);
    begin
      self.          a :=           a;
      self.  a_offset1 :=   a_offset1;
      self.  a_offset2 :=   a_offset2;
      self.        len :=         len;
      self.buff_offset := buff_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var           a_qr :=           a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=           a_qr.ev;
      var   a_offset1_qr :=   a_offset1.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   a_offset1_qr.ev;
      var   a_offset2_qr :=   a_offset2.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   a_offset2_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      var buff_offset_qr := buff_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += buff_offset_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var           a :=           a_qr.GetRes;
        var   a_offset1 :=   a_offset1_qr.GetRes;
        var   a_offset2 :=   a_offset2_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var buff_offset := buff_offset_qr.GetRes;
        
        try
          cl.EnqueueReadBuffer(
            cq, o.Native, Bool.BLOCKING,
            new UIntPtr(buff_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
            a[a_offset1,a_offset2],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
                a.RegisterWaitables(tsk, prev_hubs);
        a_offset1.RegisterWaitables(tsk, prev_hubs);
        a_offset2.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
      buff_offset.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion ReadArray2}

function BufferCommandQueue.AddReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, buff_offset: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandReadArray2<TRecord>(a, a_offset1, a_offset2, len, buff_offset));

{$region ReadArray3}

type
  BufferCommandReadArray3<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private           a: CommandQueue<array[,,] of TRecord>;
    private   a_offset1: CommandQueue<integer>;
    private   a_offset2: CommandQueue<integer>;
    private   a_offset3: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    private buff_offset: CommandQueue<integer>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 6;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, buff_offset: CommandQueue<integer>);
    begin
      self.          a :=           a;
      self.  a_offset1 :=   a_offset1;
      self.  a_offset2 :=   a_offset2;
      self.  a_offset3 :=   a_offset3;
      self.        len :=         len;
      self.buff_offset := buff_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var           a_qr :=           a.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=           a_qr.ev;
      var   a_offset1_qr :=   a_offset1.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   a_offset1_qr.ev;
      var   a_offset2_qr :=   a_offset2.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   a_offset2_qr.ev;
      var   a_offset3_qr :=   a_offset3.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   a_offset3_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      var buff_offset_qr := buff_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += buff_offset_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var           a :=           a_qr.GetRes;
        var   a_offset1 :=   a_offset1_qr.GetRes;
        var   a_offset2 :=   a_offset2_qr.GetRes;
        var   a_offset3 :=   a_offset3_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var buff_offset := buff_offset_qr.GetRes;
        
        try
          cl.EnqueueReadBuffer(
            cq, o.Native, Bool.BLOCKING,
            new UIntPtr(buff_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
            a[a_offset1,a_offset2,a_offset3],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
                a.RegisterWaitables(tsk, prev_hubs);
        a_offset1.RegisterWaitables(tsk, prev_hubs);
        a_offset2.RegisterWaitables(tsk, prev_hubs);
        a_offset3.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
      buff_offset.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion ReadArray3}

function BufferCommandQueue.AddReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, buff_offset: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandReadArray3<TRecord>(a, a_offset1, a_offset2, a_offset3, len, buff_offset));

{$endregion 1#Write&Read}

{$region 2#Fill}

{$region FillDataAutoSize}

type
  BufferCommandFillDataAutoSize = sealed class(EnqueueableGPUCommand<Buffer>)
    private         ptr: CommandQueue<IntPtr>;
    private pattern_len: CommandQueue<integer>;
    
    protected function ParamCountL1: integer; override := 2;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.pattern_len := pattern_len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var         ptr_qr :=         ptr.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=         ptr_qr.ev;
      var pattern_len_qr := pattern_len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += pattern_len_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.ntv,
          ptr, new UIntPtr(pattern_len),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
              ptr.RegisterWaitables(tsk, prev_hubs);
      pattern_len.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion FillDataAutoSize}

function BufferCommandQueue.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandFillDataAutoSize(ptr, pattern_len));

{$region FillData}

type
  BufferCommandFillData = sealed class(EnqueueableGPUCommand<Buffer>)
    private         ptr: CommandQueue<IntPtr>;
    private pattern_len: CommandQueue<integer>;
    private buff_offset: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    
    protected function ParamCountL1: integer; override := 4;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len, buff_offset, len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.pattern_len := pattern_len;
      self.buff_offset := buff_offset;
      self.        len :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var         ptr_qr :=         ptr.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=         ptr_qr.ev;
      var pattern_len_qr := pattern_len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += pattern_len_qr.ev;
      var buff_offset_qr := buff_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += buff_offset_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var buff_offset := buff_offset_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.ntv,
          ptr, new UIntPtr(pattern_len),
          new UIntPtr(buff_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
              ptr.RegisterWaitables(tsk, prev_hubs);
      pattern_len.RegisterWaitables(tsk, prev_hubs);
      buff_offset.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion FillData}

function BufferCommandQueue.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, buff_offset, len: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandFillData(ptr, pattern_len, buff_offset, len));

{$region FillValueAutoSize}

type
  BufferCommandFillValueAutoSize<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private val: ^TRecord := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>));
    
    protected procedure Finalize; override;
    begin
      Marshal.FreeHGlobal(new IntPtr(val));
    end;
    
    protected function ParamCountL1: integer; override := 0;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(val: TRecord);
    begin
      self.val^ := val;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.ntv,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
    end;
    
  end;
  
{$endregion FillValueAutoSize}

function BufferCommandQueue.AddFillValue<TRecord>(val: TRecord): BufferCommandQueue :=
AddCommand(new BufferCommandFillValueAutoSize<TRecord>(val));

{$region FillValue}

type
  BufferCommandFillValue<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private         val: ^TRecord := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>));
    private buff_offset: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    
    protected procedure Finalize; override;
    begin
      Marshal.FreeHGlobal(new IntPtr(val));
    end;
    
    protected function ParamCountL1: integer; override := 2;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(val: TRecord; buff_offset, len: CommandQueue<integer>);
    begin
      self.        val^ :=         val;
      self.buff_offset  := buff_offset;
      self.        len  :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var buff_offset_qr := buff_offset.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += buff_offset_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var buff_offset := buff_offset_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.ntv,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new UIntPtr(buff_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      buff_offset.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion FillValue}

function BufferCommandQueue.AddFillValue<TRecord>(val: TRecord; buff_offset, len: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandFillValue<TRecord>(val, buff_offset, len));

{$region FillValueAutoSizeQ}

type
  BufferCommandFillValueAutoSizeQ<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private val: CommandQueue<TRecord>;
    
    protected function ParamCountL1: integer; override := 0;
    protected function ParamCountL2: integer; override := 1;
    
    public constructor(val: CommandQueue<TRecord>);
    begin
      self.val := val;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var val_qr := val.Invoke    (tsk, c, main_dvc,  True, cq, nil); evs_l2 += val_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var val := val_qr.ToPtr;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.ntv,
          new IntPtr(val.GetPtr), new UIntPtr(Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
          val_hnd.Free;
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      val.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion FillValueAutoSizeQ}

function BufferCommandQueue.AddFillValue<TRecord>(val: CommandQueue<TRecord>): BufferCommandQueue :=
AddCommand(new BufferCommandFillValueAutoSizeQ<TRecord>(val));

{$region FillValueQ}

type
  BufferCommandFillValueQ<TRecord> = sealed class(EnqueueableGPUCommand<Buffer>)
  where TRecord: record;
    private         val: CommandQueue<TRecord>;
    private buff_offset: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    
    protected function ParamCountL1: integer; override := 2;
    protected function ParamCountL2: integer; override := 1;
    
    public constructor(val: CommandQueue<TRecord>; buff_offset, len: CommandQueue<integer>);
    begin
      self.        val :=         val;
      self.buff_offset := buff_offset;
      self.        len :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var         val_qr :=         val.Invoke    (tsk, c, main_dvc,  True, cq, nil); evs_l2 +=         val_qr.ev;
      var buff_offset_qr := buff_offset.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += buff_offset_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var         val :=         val_qr.ToPtr;
        var buff_offset := buff_offset_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.ntv,
          new IntPtr(val.GetPtr), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new UIntPtr(buff_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
          val_hnd.Free;
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
              val.RegisterWaitables(tsk, prev_hubs);
      buff_offset.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion FillValueQ}

function BufferCommandQueue.AddFillValue<TRecord>(val: CommandQueue<TRecord>; buff_offset, len: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandFillValueQ<TRecord>(val, buff_offset, len));

{$endregion 2#Fill}

{$region 3#Copy}

{$region CopyToAutoSize}

type
  BufferCommandCopyToAutoSize = sealed class(EnqueueableGPUCommand<Buffer>)
    private b: CommandQueue<Buffer>;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(b: CommandQueue<Buffer>);
    begin
      self.b := b;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var b_qr := b.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += b_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var b := b_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, o.ntv,b.ntv,
          UIntPtr.Zero, UIntPtr.Zero,
          o.Size64<b.Size64 ? o.Size : b.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      b.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion CopyToAutoSize}

function BufferCommandQueue.AddCopyTo(b: CommandQueue<Buffer>): BufferCommandQueue :=
AddCommand(new BufferCommandCopyToAutoSize(b));

{$region CopyFormAutoSize}

type
  BufferCommandCopyFormAutoSize = sealed class(EnqueueableGPUCommand<Buffer>)
    private b: CommandQueue<Buffer>;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(b: CommandQueue<Buffer>);
    begin
      self.b := b;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var b_qr := b.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += b_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var b := b_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, b.ntv,o.ntv,
          UIntPtr.Zero, UIntPtr.Zero,
          o.Size64<b.Size64 ? o.Size : b.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      b.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion CopyFormAutoSize}

function BufferCommandQueue.AddCopyForm(b: CommandQueue<Buffer>): BufferCommandQueue :=
AddCommand(new BufferCommandCopyFormAutoSize(b));

{$region CopyTo}

type
  BufferCommandCopyTo = sealed class(EnqueueableGPUCommand<Buffer>)
    private        b: CommandQueue<Buffer>;
    private from_pos: CommandQueue<integer>;
    private   to_pos: CommandQueue<integer>;
    private      len: CommandQueue<integer>;
    
    protected function ParamCountL1: integer; override := 4;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(b: CommandQueue<Buffer>; from_pos, to_pos, len: CommandQueue<integer>);
    begin
      self.       b :=        b;
      self.from_pos := from_pos;
      self.  to_pos :=   to_pos;
      self.     len :=      len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var        b_qr :=        b.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=        b_qr.ev;
      var from_pos_qr := from_pos.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += from_pos_qr.ev;
      var   to_pos_qr :=   to_pos.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   to_pos_qr.ev;
      var      len_qr :=      len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=      len_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var        b :=        b_qr.GetRes;
        var from_pos := from_pos_qr.GetRes;
        var   to_pos :=   to_pos_qr.GetRes;
        var      len :=      len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, o.ntv,b.ntv,
          new UIntPtr(from_pos), new UIntPtr(to_pos),
          new UIntPtr(len),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
             b.RegisterWaitables(tsk, prev_hubs);
      from_pos.RegisterWaitables(tsk, prev_hubs);
        to_pos.RegisterWaitables(tsk, prev_hubs);
           len.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion CopyTo}

function BufferCommandQueue.AddCopyTo(b: CommandQueue<Buffer>; from_pos, to_pos, len: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandCopyTo(b, from_pos, to_pos, len));

{$region CopyForm}

type
  BufferCommandCopyForm = sealed class(EnqueueableGPUCommand<Buffer>)
    private        b: CommandQueue<Buffer>;
    private from_pos: CommandQueue<integer>;
    private   to_pos: CommandQueue<integer>;
    private      len: CommandQueue<integer>;
    
    protected function ParamCountL1: integer; override := 4;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(b: CommandQueue<Buffer>; from_pos, to_pos, len: CommandQueue<integer>);
    begin
      self.       b :=        b;
      self.from_pos := from_pos;
      self.  to_pos :=   to_pos;
      self.     len :=      len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var        b_qr :=        b.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=        b_qr.ev;
      var from_pos_qr := from_pos.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += from_pos_qr.ev;
      var   to_pos_qr :=   to_pos.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   to_pos_qr.ev;
      var      len_qr :=      len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=      len_qr.ev;
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var        b :=        b_qr.GetRes;
        var from_pos := from_pos_qr.GetRes;
        var   to_pos :=   to_pos_qr.GetRes;
        var      len :=      len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, b.ntv,o.ntv,
          new UIntPtr(from_pos), new UIntPtr(to_pos),
          new UIntPtr(len),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
             b.RegisterWaitables(tsk, prev_hubs);
      from_pos.RegisterWaitables(tsk, prev_hubs);
        to_pos.RegisterWaitables(tsk, prev_hubs);
           len.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion CopyForm}

function BufferCommandQueue.AddCopyForm(b: CommandQueue<Buffer>; from_pos, to_pos, len: CommandQueue<integer>): BufferCommandQueue :=
AddCommand(new BufferCommandCopyForm(b, from_pos, to_pos, len));

{$endregion 3#Copy}

{$region Get}

{$region GetDataAutoSize}

type
  BufferCommandGetDataAutoSize = sealed class(EnqueueableGetCommand<Buffer, IntPtr>)
    
    protected function ParamCountL1: integer; override := 0;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: BufferCommandQueue);
    begin
      inherited Create(ccq);
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, EventList, QueueResDelayedBase<IntPtr>)->cl_event; override;
    begin
      
      Result := (o, cq, tsk, evs, own_qr)->
      begin
        var res_ev: cl_event;
        
        var res := Marshal.AllocHGlobal(IntPtr(pointer(o.Size))); own_qr.SetRes(res);
        //ToDo А что если результат уже получен и освобождёт сдедующей .ThenConvert
        // - Вообще .WhenError тут (и в +1 месте) - говнокод
        tsk.WhenError((tsk,err)->Marshal.FreeHGlobal(res));
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, o.Size,
          res,
          evs.count, evs.evs, res_ev
        );
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override := exit;
    
  end;
  
{$endregion GetDataAutoSize}

function BufferCommandQueue.AddGetData: CommandQueue<IntPtr> :=
new BufferCommandGetDataAutoSize(self) as CommandQueue<IntPtr>;

{$region GetData}

type
  BufferCommandGetData = sealed class(EnqueueableGetCommand<Buffer, IntPtr>)
    private buff_offset: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    
    protected function ParamCountL1: integer; override := 2;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: BufferCommandQueue; buff_offset, len: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.buff_offset := buff_offset;
      self.        len :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, EventList, QueueResDelayedBase<IntPtr>)->cl_event; override;
    begin
      var buff_offset_qr := buff_offset.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += buff_offset_qr.ev;
      var         len_qr :=         len.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=         len_qr.ev;
      
      Result := (o, cq, tsk, evs, own_qr)->
      begin
        var buff_offset := buff_offset_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var res_ev: cl_event;
        
        var res := Marshal.AllocHGlobal(IntPtr(pointer(o.Size))); own_qr.SetRes(res);
        tsk.WhenError((tsk,err)->Marshal.FreeHGlobal(res));
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(buff_offset), new UIntPtr(len),
          res,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      buff_offset.RegisterWaitables(tsk, prev_hubs);
              len.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion GetData}

function BufferCommandQueue.AddGetData(buff_offset, len: CommandQueue<integer>): CommandQueue<IntPtr> :=
new BufferCommandGetData(self, buff_offset, len) as CommandQueue<IntPtr>;

function BufferCommandQueue.AddGetValue<TRecord>: CommandQueue<TRecord> :=
AddGetValue&<TRecord>(0);

{$region GetValue}

type
  BufferCommandGetValue<TRecord> = sealed class(EnqueueableGetCommand<Buffer, TRecord>)
  where TRecord: record;
    private buff_offset: CommandQueue<integer>;
    
    protected function ForcePtrQr: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: BufferCommandQueue; buff_offset: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.buff_offset := buff_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, EventList, QueueResDelayedBase<TRecord>)->cl_event; override;
    begin
      var buff_offset_qr := buff_offset.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += buff_offset_qr.ev;
      
      Result := (o, cq, tsk, evs, own_qr)->
      begin
        var buff_offset := buff_offset_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(buff_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr((own_qr as QueueResDelayedPtr<TRecord>).ptr),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var own_qr_hnd := GCHandle.Alloc(own_qr);
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
          own_qr_hnd.Free;
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      buff_offset.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion GetValue}

function BufferCommandQueue.AddGetValue<TRecord>(buff_offset: CommandQueue<integer>): CommandQueue<TRecord> :=
new BufferCommandGetValue<TRecord>(self, buff_offset) as CommandQueue<TRecord>;

{$region GetArray1AutoSize}

type
  BufferCommandGetArray1AutoSize<TRecord> = sealed class(EnqueueableGetCommand<Buffer, array of TRecord>)
  where TRecord: record;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 0;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: BufferCommandQueue);
    begin
      inherited Create(ccq);
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, EventList, QueueResDelayedBase<array of TRecord>)->cl_event; override;
    begin
      
      Result := (o, cq, tsk, evs, own_qr)->
      begin
        
        try
          var len := o.Size64 div Marshal.SizeOf&<TRecord>;
          var res := new TRecord[len]; own_qr.SetRes(res);
          cl.EnqueueReadBuffer(
            cq, o.Native, Bool.BLOCKING,
            new UIntPtr(0), new UIntPtr(len * Marshal.SizeOf&<TRecord>),
            res[0],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override := exit;
    
  end;
  
{$endregion GetArray1AutoSize}

function BufferCommandQueue.AddGetArray1<TRecord>: CommandQueue<array of TRecord> :=
new BufferCommandGetArray1AutoSize<TRecord>(self) as CommandQueue<array of TRecord>;

{$region GetArray1}

type
  BufferCommandGetArray1<TRecord> = sealed class(EnqueueableGetCommand<Buffer, array of TRecord>)
  where TRecord: record;
    private len: CommandQueue<integer>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 1;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: BufferCommandQueue; len: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.len := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, EventList, QueueResDelayedBase<array of TRecord>)->cl_event; override;
    begin
      var len_qr := len.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += len_qr.ev;
      
      Result := (o, cq, tsk, evs, own_qr)->
      begin
        var len := len_qr.GetRes;
        
        try
          var res := new TRecord[len]; own_qr.SetRes(res);
          cl.EnqueueReadBuffer(
            cq, o.Native, Bool.BLOCKING,
            new UIntPtr(0), new UIntPtr(int64(len) * Marshal.SizeOf&<TRecord>),
            res[0],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      len.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion GetArray1}

function BufferCommandQueue.AddGetArray1<TRecord>(len: CommandQueue<integer>): CommandQueue<array of TRecord> :=
new BufferCommandGetArray1<TRecord>(self, len) as CommandQueue<array of TRecord>;

{$region GetArray2}

type
  BufferCommandGetArray2<TRecord> = sealed class(EnqueueableGetCommand<Buffer, array[,] of TRecord>)
  where TRecord: record;
    private len1: CommandQueue<integer>;
    private len2: CommandQueue<integer>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 2;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: BufferCommandQueue; len1,len2: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.len1 := len1;
      self.len2 := len2;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, EventList, QueueResDelayedBase<array[,] of TRecord>)->cl_event; override;
    begin
      var len1_qr := len1.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += len1_qr.ev;
      var len2_qr := len2.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += len2_qr.ev;
      
      Result := (o, cq, tsk, evs, own_qr)->
      begin
        var len1 := len1_qr.GetRes;
        var len2 := len2_qr.GetRes;
        
        try
          var res := new TRecord[len1,len2]; own_qr.SetRes(res);
          cl.EnqueueReadBuffer(
            cq, o.Native, Bool.BLOCKING,
            new UIntPtr(0), new UIntPtr(int64(len1)*len2 * Marshal.SizeOf&<TRecord>),
            res[0,0],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      len1.RegisterWaitables(tsk, prev_hubs);
      len2.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion GetArray2}

function BufferCommandQueue.AddGetArray2<TRecord>(len1,len2: CommandQueue<integer>): CommandQueue<array[,] of TRecord> :=
new BufferCommandGetArray2<TRecord>(self, len1, len2) as CommandQueue<array[,] of TRecord>;

{$region GetArray3}

type
  BufferCommandGetArray3<TRecord> = sealed class(EnqueueableGetCommand<Buffer, array[,,] of TRecord>)
  where TRecord: record;
    private len1: CommandQueue<integer>;
    private len2: CommandQueue<integer>;
    private len3: CommandQueue<integer>;
    
    protected function NeedThread: boolean; override := true;
    
    protected function ParamCountL1: integer; override := 3;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: BufferCommandQueue; len1,len2,len3: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.len1 := len1;
      self.len2 := len2;
      self.len3 := len3;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Buffer, cl_command_queue, CLTaskBase, EventList, QueueResDelayedBase<array[,,] of TRecord>)->cl_event; override;
    begin
      var len1_qr := len1.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += len1_qr.ev;
      var len2_qr := len2.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += len2_qr.ev;
      var len3_qr := len3.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 += len3_qr.ev;
      
      Result := (o, cq, tsk, evs, own_qr)->
      begin
        var len1 := len1_qr.GetRes;
        var len2 := len2_qr.GetRes;
        var len3 := len3_qr.GetRes;
        
        try
          var res := new TRecord[len1,len2,len3]; own_qr.SetRes(res);
          cl.EnqueueReadBuffer(
            cq, o.Native, Bool.BLOCKING,
            new UIntPtr(0), new UIntPtr(int64(len1)*len2*len3 * Marshal.SizeOf&<TRecord>),
            res[0,0,0],
            evs.count, evs.evs, IntPtr.Zero
          ).RaiseIfError;
        finally
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      len1.RegisterWaitables(tsk, prev_hubs);
      len2.RegisterWaitables(tsk, prev_hubs);
      len3.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion GetArray3}

function BufferCommandQueue.AddGetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): CommandQueue<array[,,] of TRecord> :=
new BufferCommandGetArray3<TRecord>(self, len1, len2, len3) as CommandQueue<array[,,] of TRecord>;

{$endregion Get}

{$endregion BufferCQ}

{$region Buffer}

{$region 1#Write&Read}

function Buffer.WriteData(ptr: CommandQueue<IntPtr>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteData(ptr) as CommandQueue<Buffer>);

function Buffer.ReadData(ptr: CommandQueue<IntPtr>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddReadData(ptr) as CommandQueue<Buffer>);

function Buffer.WriteData(ptr: CommandQueue<IntPtr>; buff_offset, len: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteData(ptr, buff_offset, len) as CommandQueue<Buffer>);

function Buffer.ReadData(ptr: CommandQueue<IntPtr>; buff_offset, len: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddReadData(ptr, buff_offset, len) as CommandQueue<Buffer>);

function Buffer.WriteData(ptr: pointer): Buffer :=
WriteData(IntPtr(ptr));

function Buffer.ReadData(ptr: pointer): Buffer :=
ReadData(IntPtr(ptr));

function Buffer.WriteData(ptr: pointer; buff_offset, len: CommandQueue<integer>): Buffer :=
WriteData(IntPtr(ptr), buff_offset, len);

function Buffer.ReadData(ptr: pointer; buff_offset, len: CommandQueue<integer>): Buffer :=
ReadData(IntPtr(ptr), buff_offset, len);

function Buffer.WriteValue<TRecord>(val: TRecord): Buffer :=
WriteValue(val, 0);

function Buffer.WriteValue<TRecord>(val: TRecord; buff_offset: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteValue&<TRecord>(val, buff_offset) as CommandQueue<Buffer>);

function Buffer.WriteValue<TRecord>(val: CommandQueue<TRecord>): Buffer :=
WriteValue(val, 0);

function Buffer.WriteValue<TRecord>(val: CommandQueue<TRecord>; buff_offset: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteValue&<TRecord>(val, buff_offset) as CommandQueue<Buffer>);

function Buffer.WriteArray1<TRecord>(a: CommandQueue<array of TRecord>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray1&<TRecord>(a) as CommandQueue<Buffer>);

function Buffer.WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray2&<TRecord>(a) as CommandQueue<Buffer>);

function Buffer.WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray3&<TRecord>(a) as CommandQueue<Buffer>);

function Buffer.ReadArray1<TRecord>(a: CommandQueue<array of TRecord>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray1&<TRecord>(a) as CommandQueue<Buffer>);

function Buffer.ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray2&<TRecord>(a) as CommandQueue<Buffer>);

function Buffer.ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray3&<TRecord>(a) as CommandQueue<Buffer>);

function Buffer.WriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, buff_offset: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray1&<TRecord>(a, a_offset, len, buff_offset) as CommandQueue<Buffer>);

function Buffer.WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, buff_offset: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray2&<TRecord>(a, a_offset1, a_offset2, len, buff_offset) as CommandQueue<Buffer>);

function Buffer.WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, buff_offset: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray3&<TRecord>(a, a_offset1, a_offset2, a_offset3, len, buff_offset) as CommandQueue<Buffer>);

function Buffer.ReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, buff_offset: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray1&<TRecord>(a, a_offset, len, buff_offset) as CommandQueue<Buffer>);

function Buffer.ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, buff_offset: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray2&<TRecord>(a, a_offset1, a_offset2, len, buff_offset) as CommandQueue<Buffer>);

function Buffer.ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, buff_offset: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray3&<TRecord>(a, a_offset1, a_offset2, a_offset3, len, buff_offset) as CommandQueue<Buffer>);

{$endregion 1#Write&Read}

{$region 2#Fill}

function Buffer.FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddFillData(ptr, pattern_len) as CommandQueue<Buffer>);

function Buffer.FillData(ptr: CommandQueue<IntPtr>; pattern_len, buff_offset, len: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddFillData(ptr, pattern_len, buff_offset, len) as CommandQueue<Buffer>);

function Buffer.FillValue<TRecord>(val: TRecord): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val) as CommandQueue<Buffer>);

function Buffer.FillValue<TRecord>(val: TRecord; buff_offset, len: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val, buff_offset, len) as CommandQueue<Buffer>);

function Buffer.FillValue<TRecord>(val: CommandQueue<TRecord>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val) as CommandQueue<Buffer>);

function Buffer.FillValue<TRecord>(val: CommandQueue<TRecord>; buff_offset, len: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val, buff_offset, len) as CommandQueue<Buffer>);

{$endregion 2#Fill}

{$region 3#Copy}

function Buffer.CopyTo(b: CommandQueue<Buffer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyTo(b) as CommandQueue<Buffer>);

function Buffer.CopyForm(b: CommandQueue<Buffer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyForm(b) as CommandQueue<Buffer>);

function Buffer.CopyTo(b: CommandQueue<Buffer>; from_pos, to_pos, len: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyTo(b, from_pos, to_pos, len) as CommandQueue<Buffer>);

function Buffer.CopyForm(b: CommandQueue<Buffer>; from_pos, to_pos, len: CommandQueue<integer>): Buffer :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyForm(b, from_pos, to_pos, len) as CommandQueue<Buffer>);

{$endregion 3#Copy}

{$region Get}

function Buffer.GetData: IntPtr :=
Context.Default.SyncInvoke(self.NewQueue.AddGetData as CommandQueue<IntPtr>);

function Buffer.GetData(buff_offset, len: CommandQueue<integer>): IntPtr :=
Context.Default.SyncInvoke(self.NewQueue.AddGetData(buff_offset, len) as CommandQueue<IntPtr>);

function Buffer.GetValue<TRecord>: TRecord :=
GetValue&<TRecord>(0);

function Buffer.GetValue<TRecord>(buff_offset: CommandQueue<integer>): TRecord :=
Context.Default.SyncInvoke(self.NewQueue.AddGetValue&<TRecord>(buff_offset) as CommandQueue<TRecord>);

function Buffer.GetArray1<TRecord>: array of TRecord :=
Context.Default.SyncInvoke(self.NewQueue.AddGetArray1&<TRecord> as CommandQueue<array of TRecord>);

function Buffer.GetArray1<TRecord>(len: CommandQueue<integer>): array of TRecord :=
Context.Default.SyncInvoke(self.NewQueue.AddGetArray1&<TRecord>(len) as CommandQueue<array of TRecord>);

function Buffer.GetArray2<TRecord>(len1,len2: CommandQueue<integer>): array[,] of TRecord :=
Context.Default.SyncInvoke(self.NewQueue.AddGetArray2&<TRecord>(len1, len2) as CommandQueue<array[,] of TRecord>);

function Buffer.GetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): array[,,] of TRecord :=
Context.Default.SyncInvoke(self.NewQueue.AddGetArray3&<TRecord>(len1, len2, len3) as CommandQueue<array[,,] of TRecord>);

{$endregion Get}

{$endregion Buffer}

{$region KernelCQ}

{$region 1#Exec}

{$region Exec1}

type
  KernelCommandExec1 = sealed class(EnqueueableGPUCommand<Kernel>)
    private  sz1: CommandQueue<integer>;
    private args: array of KernelArg;
    
    protected function ParamCountL1: integer; override := 2;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(sz1: CommandQueue<integer>; params args: array of KernelArg);
    begin
      self. sz1 :=  sz1;
      self.args := args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Kernel, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var  sz1_qr :=  sz1.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=  sz1_qr.ev;
      var args_qr := args.ConvertAll(temp1->begin Result := temp1.Invoke(tsk, c, main_dvc); evs_l1 += Result.ev; end);
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var  sz1 :=  sz1_qr.GetRes;
        var args := args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        o.UseExclusiveNative(ntv->
        begin
          
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i, c);
          
          cl.EnqueueNDRangeKernel(
            cq, ntv, 1,
            nil,
            new UIntPtr[](new UIntPtr(sz1)),
            nil,
            evs.count, evs.evs, res_ev
          );
          
          cl.RetainKernel(ntv).RaiseIfError;
          var args_hnd := GCHandle.Alloc(args);
          
          EventList.AttachFinallyCallback(res_ev, ()->
          begin
            cl.ReleaseKernel(ntv).RaiseIfError();
            args_hnd.Free;
          end, tsk, false{$ifdef EventDebug}, nil{$endif});
        end);
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
       sz1.RegisterWaitables(tsk, prev_hubs);
      foreach var temp1 in args do temp1.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion Exec1}

function KernelCommandQueue.AddExec1(sz1: CommandQueue<integer>; params args: array of KernelArg): KernelCommandQueue :=
AddCommand(new KernelCommandExec1(sz1, args));

{$region Exec2}

type
  KernelCommandExec2 = sealed class(EnqueueableGPUCommand<Kernel>)
    private  sz1: CommandQueue<integer>;
    private  sz2: CommandQueue<integer>;
    private args: array of KernelArg;
    
    protected function ParamCountL1: integer; override := 3;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg);
    begin
      self. sz1 :=  sz1;
      self. sz2 :=  sz2;
      self.args := args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Kernel, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var  sz1_qr :=  sz1.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=  sz1_qr.ev;
      var  sz2_qr :=  sz2.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=  sz2_qr.ev;
      var args_qr := args.ConvertAll(temp1->begin Result := temp1.Invoke(tsk, c, main_dvc); evs_l1 += Result.ev; end);
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var  sz1 :=  sz1_qr.GetRes;
        var  sz2 :=  sz2_qr.GetRes;
        var args := args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        o.UseExclusiveNative(ntv->
        begin
          
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i, c);
          
          cl.EnqueueNDRangeKernel(
            cq, ntv, 2,
            nil,
            new UIntPtr[](new UIntPtr(sz1),new UIntPtr(sz2)),
            nil,
            evs.count, evs.evs, res_ev
          );
          
          cl.RetainKernel(ntv).RaiseIfError;
          var args_hnd := GCHandle.Alloc(args);
          
          EventList.AttachFinallyCallback(res_ev, ()->
          begin
            cl.ReleaseKernel(ntv).RaiseIfError();
            args_hnd.Free;
          end, tsk, false{$ifdef EventDebug}, nil{$endif});
        end);
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
       sz1.RegisterWaitables(tsk, prev_hubs);
       sz2.RegisterWaitables(tsk, prev_hubs);
      foreach var temp1 in args do temp1.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion Exec2}

function KernelCommandQueue.AddExec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): KernelCommandQueue :=
AddCommand(new KernelCommandExec2(sz1, sz2, args));

{$region Exec3}

type
  KernelCommandExec3 = sealed class(EnqueueableGPUCommand<Kernel>)
    private  sz1: CommandQueue<integer>;
    private  sz2: CommandQueue<integer>;
    private  sz3: CommandQueue<integer>;
    private args: array of KernelArg;
    
    protected function ParamCountL1: integer; override := 4;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg);
    begin
      self. sz1 :=  sz1;
      self. sz2 :=  sz2;
      self. sz3 :=  sz3;
      self.args := args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Kernel, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var  sz1_qr :=  sz1.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 +=  sz1_qr.ev;
      var  sz2_qr :=  sz2.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=  sz2_qr.ev;
      var  sz3_qr :=  sz3.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=  sz3_qr.ev;
      var args_qr := args.ConvertAll(temp1->begin Result := temp1.Invoke(tsk, c, main_dvc); evs_l1 += Result.ev; end);
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var  sz1 :=  sz1_qr.GetRes;
        var  sz2 :=  sz2_qr.GetRes;
        var  sz3 :=  sz3_qr.GetRes;
        var args := args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        o.UseExclusiveNative(ntv->
        begin
          
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i, c);
          
          cl.EnqueueNDRangeKernel(
            cq, ntv, 3,
            nil,
            new UIntPtr[](new UIntPtr(sz1),new UIntPtr(sz2),new UIntPtr(sz3)),
            nil,
            evs.count, evs.evs, res_ev
          );
          
          cl.RetainKernel(ntv).RaiseIfError;
          var args_hnd := GCHandle.Alloc(args);
          
          EventList.AttachFinallyCallback(res_ev, ()->
          begin
            cl.ReleaseKernel(ntv).RaiseIfError();
            args_hnd.Free;
          end, tsk, false{$ifdef EventDebug}, nil{$endif});
        end);
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
       sz1.RegisterWaitables(tsk, prev_hubs);
       sz2.RegisterWaitables(tsk, prev_hubs);
       sz3.RegisterWaitables(tsk, prev_hubs);
      foreach var temp1 in args do temp1.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion Exec3}

function KernelCommandQueue.AddExec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): KernelCommandQueue :=
AddCommand(new KernelCommandExec3(sz1, sz2, sz3, args));

{$region Exec}

type
  KernelCommandExec = sealed class(EnqueueableGPUCommand<Kernel>)
    private global_work_offset: CommandQueue<array of UIntPtr>;
    private   global_work_size: CommandQueue<array of UIntPtr>;
    private    local_work_size: CommandQueue<array of UIntPtr>;
    private               args: array of KernelArg;
    
    protected function ParamCountL1: integer; override := 4;
    protected function ParamCountL2: integer; override := 0;
    
    public constructor(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg);
    begin
      self.global_work_offset := global_work_offset;
      self.  global_work_size :=   global_work_size;
      self.   local_work_size :=    local_work_size;
      self.              args :=               args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; evs_l1, evs_l2: List<EventList>): (Kernel, cl_command_queue, CLTaskBase, Context, EventList)->cl_event; override;
    begin
      var global_work_offset_qr := global_work_offset.Invoke    (tsk, c, main_dvc, False, cq, nil); evs_l1 += global_work_offset_qr.ev;
      var   global_work_size_qr :=   global_work_size.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=   global_work_size_qr.ev;
      var    local_work_size_qr :=    local_work_size.InvokeNewQ(tsk, c, main_dvc, False,     nil); evs_l1 +=    local_work_size_qr.ev;
      var               args_qr :=               args.ConvertAll(temp1->begin Result := temp1.Invoke(tsk, c, main_dvc); evs_l1 += Result.ev; end);
      
      Result := (o, cq, tsk, c, evs)->
      begin
        var global_work_offset := global_work_offset_qr.GetRes;
        var   global_work_size :=   global_work_size_qr.GetRes;
        var    local_work_size :=    local_work_size_qr.GetRes;
        var               args :=               args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        o.UseExclusiveNative(ntv->
        begin
          
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i, c);
          
          cl.EnqueueNDRangeKernel(
            cq, ntv, global_work_size.Length,
            global_work_offset,
            global_work_size,
            local_work_size,
            evs.count, evs.evs, res_ev
          );
          
          cl.RetainKernel(ntv).RaiseIfError;
          var args_hnd := GCHandle.Alloc(args);
          
          EventList.AttachFinallyCallback(res_ev, ()->
          begin
            cl.ReleaseKernel(ntv).RaiseIfError();
            args_hnd.Free;
          end, tsk, false{$ifdef EventDebug}, nil{$endif});
        end);
        
        EventList.AttachFinallyCallback(res_ev, ()->
        begin
          evs.Release({$ifdef EventDebug}$'after use in waiting of {self.GetType}'{$endif});
        end, tsk, false{$ifdef EventDebug}, nil{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override;
    begin
      global_work_offset.RegisterWaitables(tsk, prev_hubs);
        global_work_size.RegisterWaitables(tsk, prev_hubs);
         local_work_size.RegisterWaitables(tsk, prev_hubs);
      foreach var temp1 in args do temp1.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
{$endregion Exec}

function KernelCommandQueue.AddExec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): KernelCommandQueue :=
AddCommand(new KernelCommandExec(global_work_offset, global_work_size, local_work_size, args));

{$endregion 1#Exec}

{$endregion KernelCQ}

{$region Kernel}

{$region 1#Exec}

function Kernel.Exec1(sz1: CommandQueue<integer>; params args: array of KernelArg): Kernel :=
Context.Default.SyncInvoke(self.NewQueue.AddExec1(sz1, args) as CommandQueue<Kernel>);

function Kernel.Exec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): Kernel :=
Context.Default.SyncInvoke(self.NewQueue.AddExec2(sz1, sz2, args) as CommandQueue<Kernel>);

function Kernel.Exec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): Kernel :=
Context.Default.SyncInvoke(self.NewQueue.AddExec3(sz1, sz2, sz3, args) as CommandQueue<Kernel>);

function Kernel.Exec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): Kernel :=
Context.Default.SyncInvoke(self.NewQueue.AddExec(global_work_offset, global_work_size, local_work_size, args) as CommandQueue<Kernel>);

{$endregion 1#Exec}

{$endregion Kernel}

{$endregion CommonCommands}

end.