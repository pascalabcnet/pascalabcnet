
//*****************************************************************************************************\\
// Copyright (©) Sergey Latchenko ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// This code is distributed under the Unlicense
// For details see LICENSE file or this:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\
// Copyright (©) Сергей Латченко ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// Данный код распространяется с лицензией Unlicense
// Подробнее в файле LICENSE или тут:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\

///
///Высокоуровневая оболочка модуля OpenCL
///   OpenCL и OpenCLABC можно использовать одновременно
///   Но контактировать они практически не будут
///
///Если не хватает типа/метода или найдена ошибка - писать сюда:
///   https://github.com/SunSerega/POCGL/issues
///
///Справка данного модуля находится в папке примеров
///   По-умолчанию, её можно найти в "C:\PABCWork.NET\Samples\OpenGL и OpenCL"
///
unit OpenCLABC;

{$region TODO}

//===================================
// Обязательно сделать до следующей стабильной версии:

//TODO Тесты:
// - .ThenQuickConvert.Multiusable: сколько раз вычисляется?
// - Потеря результата Quick обработки:
// --- HFQ(()->5).ThenQuickConvert.ThenQuickUse + HPQ(()->begin end)
// --- CombineQuickConv + HPQ(()->begin end)
// - HPQ + HPQQ + HPQ: HPQQ должен выполнится последним

//TODO Справка:
// - ThenQuick[Convert,Use]
// - CombineQuickConv[Sync,Async]Queue
// --- Добавил Conv в название!!!
// - AddQuickProc
// - HPQQ/HFQQ
//
// - Quick очереди срабатывают в последний допустимый момент
// --- HPQ + HPQQ + HPQ: HPQQ выполнится последним
//
// - В обработке исключений написать, что обработчики всегда Quick

//===================================
// Запланированное:

//TODO Пройтись по интерфейсу, порасставлять кидание исключений
//TODO Проверки и кидания исключений перед всеми cl.*, чтобы выводить норм сообщения об ошибках

//TODO Использовать cl.EnqueueMapBuffer
// - В виде .AddMap((MappedArray,Context)->())
// - Недодел своей ветке

//TODO .pcu с неправильной позицией зависимости, или не теми настройками - должен игнорироваться
// - Иначе сейчас модули в примерах ссылаются на .pcu, который существует только во время работы Tester, ломая компилятор

//TODO Может всё же сделать защиту от дурака для "q.AddQueue(q)"?
// - И в справке тогда убрать параграф...

//TODO Порядок Wait очередей в Wait группах
// - Проверить сочетание с каждой другой фичей

//TODO .Cycle(integer)
//TODO .Cycle // бесконечность циклов
//TODO .CycleWhile(***->boolean)
//TODO В продолжение Cycle: Однако всё ещё остаётся проблема - как сделать ветвление?
// - И если уже делать - стоит сделать и метод CQ.ThenIf(res->boolean; if_true, if_false: CQ)
//TODO И ещё - AbortQueue, который, по сути, может использоваться как exit, continue или break, если с обработчиками ошибок
// - Или может метод MarkerQueue.Abort?
//
//TODO Несколько TODO в:
// - Queue converter's >> Wait

//TODO CLArray2 и CLArray3?
// - Основная проблема использовать только CLArray<> сейчас - через него не прочитаешь/не запишешь многомерные массивы из RAM
// - Вообще на стороне OpenCL запутывает, по строкам или по столбцам передался массив?
// - С этой стороны, лучше иметь только одномерный CLArray, ради безопасности
// - По хорошему, в коде использующем OpenCLABC, надо объявляться MatrixByRows/MatrixByCols и т.п.
// - Но это будет объёмно, а ради простых примеров...

//TODO Интегрировать профайлинг очередей

//TODO Исправить перегрузки Kernel.Exec

//TODO Проверить, будет ли оптимизацией, создавать свой ThreadPool для каждого CLTaskBase
// - (HPQ+HPQ).Handle.Handle, тут создаётся 4 UserEvent, хотя всё можно было бы выполнять синхронно

//===================================
// Сделать когда-нибуть:

//TODO Пройтись по всем функциям OpenCL, посмотреть функционал каких не доступен из OpenCLABC
// - clGetKernelWorkGroupInfo - свойства кернела на определённом устройстве
// - clCreateContext: CL_CONTEXT_INTEROP_USER_SYNC

//TODO Слишком новые фичи, которые могут много чего изменить:
// - cl_khr_command_buffer
// --- Буферы, хранящие список комманд
// - cl_khr_semaphore
// --- Как cl_event, но многоразовые

//===================================

{$endregion TODO}

{$region Bugs}

//TODO Issue компилятора:
//TODO https://github.com/pascalabcnet/pascalabcnet/issues/{id}
// - #2221
// - #2550
// - #2589
// - #2604
// - #2607
// - #2610

//TODO Баги NVidia
//TODO https://developer.nvidia.com/nvidia_bug/{id}
// - NV#3035203

{$endregion}

{$region DEBUG}{$ifdef DEBUG}

// Регистрация всех cl.RetainEvent и cl.ReleaseEvent
{ $define EventDebug}

// Регистрация использований cl_command_queue
{ $define QueueDebug}

// Регистрация активаций/деактиваций всех WaitHandler-ов
{ $define WaitDebug}

{ $define ForceMaxDebug}
{$ifdef ForceMaxDebug}
  {$define EventDebug}
  {$define QueueDebug}
  {$define WaitDebug}
{$endif ForceMaxDebug}

{$endif DEBUG}{$endregion DEBUG}

interface

uses System;
uses System.Threading;
uses System.Runtime.InteropServices;
uses System.Collections.ObjectModel;
uses System.Collections.Concurrent;

uses OpenCL;

type
  
  {$region Re-definition's}
  
  ///Класс исключений из OpenCL
  OpenCLException         = OpenCL.OpenCLException;
  
  ///Тип устройства, поддерживающего OpenCL
  DeviceType              = OpenCL.DeviceType;
  ///Уровень кэша, используемый в Device.SplitByAffinityDomain
  DeviceAffinityDomain    = OpenCL.DeviceAffinityDomain;
  
  {$endregion Re-definition's}
  
  {$region OpenCLABCInternalException}
  
  ///Исключение, кидаемое при неожиданном поведении внутренностей OpenCLABC
  ///Если это исключение было кинуто - пишите в issue
  OpenCLABCInternalException = sealed class(Exception)
    
    private constructor(message: string) :=
    inherited Create(message);
//    private constructor(message: string; ec: ErrorCode) :=
//    inherited Create($'{message} with {ec}');
    private constructor(ec: ErrorCode) :=
    inherited Create('', new OpenCLException(ec));
    private constructor;
    begin
      inherited Create($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
      raise self;
    end;
    
    private static procedure RaiseIfError(ec: ErrorCode) :=
    if ec.IS_ERROR then raise new OpenCLABCInternalException(ec);
    
    private static procedure RaiseIfError(st: CommandExecutionStatus) :=
    if st.val<0 then RaiseIfError(ErrorCode(st));
    
  end;
  
  {$endregion OpenCLABCInternalException}
  
  {$region DEBUG}
  
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
    
    private static RefCounter := new ConcurrentDictionary<cl_event, ConcurrentQueue<EventRetainReleaseData>>;
    private static function RefCounterFor(ev: cl_event) := RefCounter.GetOrAdd(ev, ev->new ConcurrentQueue<EventRetainReleaseData>);
    
    public static procedure RegisterEventRetain(ev: cl_event; reason: string) :=
    RefCounterFor(ev).Enqueue(new EventRetainReleaseData(false, reason));
    public static procedure RegisterEventRelease(ev: cl_event; reason: string);
    begin
      EventDebug.CheckExists(ev, reason);
      RefCounterFor(ev).Enqueue(new EventRetainReleaseData(true, reason));
    end;
    
    public static procedure ReportRefCounterInfo(otp: System.IO.TextWriter := Console.Out) :=
    lock otp do
    begin
      System.Environment.StackTrace.Println;
      
      foreach var kvp in RefCounter do
      begin
        $'Logging state change of {kvp.Key}'.Println;
        var c := 0;
        foreach var act in kvp.Value do
        begin
          c += if act.is_release then -1 else +1;
          $'{c,3} | {act}'.Println;
        end;
        Writeln('-'*30);
      end;
      
      Writeln('='*40);
      output.Flush;
    end;
    
    public static function CountRetains(ev: cl_event) :=
    RefCounter[ev].Sum(act->act.is_release ? -1 : +1);
    public static procedure CheckExists(ev: cl_event; reason: string) :=
    if CountRetains(ev)<=0 then lock output do
    begin
      ReportRefCounterInfo(Console.Error);
      Sleep(1000);
      raise new OpenCLABCInternalException($'Event {ev} was released before last use ({reason}) at');
    end;
    
    public static procedure AssertDone :=
    foreach var ev in RefCounter.Keys do if CountRetains(ev)<>0 then
    begin
      ReportRefCounterInfo(Console.Error);
      Sleep(1000);
      raise new OpenCLABCInternalException(ev.ToString);
    end;
    
    {$endregion Retain/Release}
    
  end;
  
  {$endif}{$endregion EventDebug}
  
  {$region QueueDebug}{$ifdef QueueDebug}
  
  QueueDebug = static class
    
    private static QueueUses := new ConcurrentDictionary<cl_command_queue, ConcurrentQueue<string>>;
    private static function QueueUsesFor(cq: cl_command_queue) := QueueUses.GetOrAdd(cq, cq->new ConcurrentQueue<string>);
    private static procedure Add(cq: cl_command_queue; use: string) := QueueUsesFor(cq).Enqueue(use);
    
    public static procedure ReportQueueUses :=
    foreach var kvp in QueueUses.OrderBy(kvp->kvp.Value.Count) do
    begin
      $'Logging uses of {kvp.Key}'.Println;
      kvp.Value.PrintLines;
      Println('='*30);
    end;
    
  end;
  
  {$endif}{$endregion QueueDebug}
  
  {$region WaitDebug}{$ifdef WaitDebug}
  
  WaitDebug = static class
    
    private static WaitActions := new ConcurrentDictionary<object, ConcurrentQueue<string>>;
    
    private static procedure RegisterAction(handler: object; act: string) :=
    WaitActions.GetOrAdd(handler, hc->new System.Collections.Concurrent.ConcurrentQueue<string>).Enqueue(act);
    
    public static procedure ReportWaitActions :=
    foreach var kvp in WaitActions do
    begin
      $'Logging actions of handler[{kvp.Key.GetHashCode}]'.Println;
      kvp.Value.PrintLines;
      Println('='*30);
    end;
    
  end;
  
  {$endif}{$endregion WaitDebug}
  
  {$endregion DEBUG}
  
  {$region Properties}
  
  {$region Platform}
  
  PlatformProperties = partial class
    
    public constructor(ntv: cl_platform_id);
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    private function GetProfile: String;
    private function GetVersion: String;
    private function GetName: String;
    private function GetVendor: String;
    private function GetExtensions: String;
    private function GetHostTimerResolution: UInt64;
    
    public property Profile:             String read GetProfile;
    public property Version:             String read GetVersion;
    public property Name:                String read GetName;
    public property Vendor:              String read GetVendor;
    public property Extensions:          String read GetExtensions;
    public property HostTimerResolution: UInt64 read GetHostTimerResolution;
    
  end;
  
  {$endregion Platform}
  
  {$region Device}
  
  DeviceProperties = partial class
    
    public constructor(ntv: cl_device_id);
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    private function GetType: DeviceType;
    private function GetVendorId: UInt32;
    private function GetMaxComputeUnits: UInt32;
    private function GetMaxWorkItemDimensions: UInt32;
    private function GetMaxWorkItemSizes: array of UIntPtr;
    private function GetMaxWorkGroupSize: UIntPtr;
    private function GetPreferredVectorWidthChar: UInt32;
    private function GetPreferredVectorWidthShort: UInt32;
    private function GetPreferredVectorWidthInt: UInt32;
    private function GetPreferredVectorWidthLong: UInt32;
    private function GetPreferredVectorWidthFloat: UInt32;
    private function GetPreferredVectorWidthDouble: UInt32;
    private function GetPreferredVectorWidthHalf: UInt32;
    private function GetNativeVectorWidthChar: UInt32;
    private function GetNativeVectorWidthShort: UInt32;
    private function GetNativeVectorWidthInt: UInt32;
    private function GetNativeVectorWidthLong: UInt32;
    private function GetNativeVectorWidthFloat: UInt32;
    private function GetNativeVectorWidthDouble: UInt32;
    private function GetNativeVectorWidthHalf: UInt32;
    private function GetMaxClockFrequency: UInt32;
    private function GetAddressBits: UInt32;
    private function GetMaxMemAllocSize: UInt64;
    private function GetImageSupport: Bool;
    private function GetMaxReadImageArgs: UInt32;
    private function GetMaxWriteImageArgs: UInt32;
    private function GetMaxReadWriteImageArgs: UInt32;
    private function GetIlVersion: String;
    private function GetImage2dMaxWidth: UIntPtr;
    private function GetImage2dMaxHeight: UIntPtr;
    private function GetImage3dMaxWidth: UIntPtr;
    private function GetImage3dMaxHeight: UIntPtr;
    private function GetImage3dMaxDepth: UIntPtr;
    private function GetImageMaxBufferSize: UIntPtr;
    private function GetImageMaxArraySize: UIntPtr;
    private function GetMaxSamplers: UInt32;
    private function GetImagePitchAlignment: UInt32;
    private function GetImageBaseAddressAlignment: UInt32;
    private function GetMaxPipeArgs: UInt32;
    private function GetPipeMaxActiveReservations: UInt32;
    private function GetPipeMaxPacketSize: UInt32;
    private function GetMaxParameterSize: UIntPtr;
    private function GetMemBaseAddrAlign: UInt32;
    private function GetSingleFpConfig: DeviceFPConfig;
    private function GetDoubleFpConfig: DeviceFPConfig;
    private function GetGlobalMemCacheType: DeviceMemCacheType;
    private function GetGlobalMemCachelineSize: UInt32;
    private function GetGlobalMemCacheSize: UInt64;
    private function GetGlobalMemSize: UInt64;
    private function GetMaxConstantBufferSize: UInt64;
    private function GetMaxConstantArgs: UInt32;
    private function GetMaxGlobalVariableSize: UIntPtr;
    private function GetGlobalVariablePreferredTotalSize: UIntPtr;
    private function GetLocalMemType: DeviceLocalMemType;
    private function GetLocalMemSize: UInt64;
    private function GetErrorCorrectionSupport: Bool;
    private function GetProfilingTimerResolution: UIntPtr;
    private function GetEndianLittle: Bool;
    private function GetAvailable: Bool;
    private function GetCompilerAvailable: Bool;
    private function GetLinkerAvailable: Bool;
    private function GetExecutionCapabilities: DeviceExecCapabilities;
    private function GetQueueOnHostProperties: CommandQueueProperties;
    private function GetQueueOnDeviceProperties: CommandQueueProperties;
    private function GetQueueOnDevicePreferredSize: UInt32;
    private function GetQueueOnDeviceMaxSize: UInt32;
    private function GetMaxOnDeviceQueues: UInt32;
    private function GetMaxOnDeviceEvents: UInt32;
    private function GetBuiltInKernels: String;
    private function GetName: String;
    private function GetVendor: String;
    private function GetProfile: String;
    private function GetVersion: String;
    private function GetOpenclCVersion: String;
    private function GetExtensions: String;
    private function GetPrintfBufferSize: UIntPtr;
    private function GetPreferredInteropUserSync: Bool;
    private function GetPartitionMaxSubDevices: UInt32;
    private function GetPartitionProperties: array of DevicePartitionProperty;
    private function GetPartitionAffinityDomain: DeviceAffinityDomain;
    private function GetPartitionType: array of DevicePartitionProperty;
    private function GetReferenceCount: UInt32;
    private function GetSvmCapabilities: DeviceSVMCapabilities;
    private function GetPreferredPlatformAtomicAlignment: UInt32;
    private function GetPreferredGlobalAtomicAlignment: UInt32;
    private function GetPreferredLocalAtomicAlignment: UInt32;
    private function GetMaxNumSubGroups: UInt32;
    private function GetSubGroupIndependentForwardProgress: Bool;
    
    public property &Type:                              DeviceType                       read GetType;
    public property VendorId:                           UInt32                           read GetVendorId;
    public property MaxComputeUnits:                    UInt32                           read GetMaxComputeUnits;
    public property MaxWorkItemDimensions:              UInt32                           read GetMaxWorkItemDimensions;
    public property MaxWorkItemSizes:                   array of UIntPtr                 read GetMaxWorkItemSizes;
    public property MaxWorkGroupSize:                   UIntPtr                          read GetMaxWorkGroupSize;
    public property PreferredVectorWidthChar:           UInt32                           read GetPreferredVectorWidthChar;
    public property PreferredVectorWidthShort:          UInt32                           read GetPreferredVectorWidthShort;
    public property PreferredVectorWidthInt:            UInt32                           read GetPreferredVectorWidthInt;
    public property PreferredVectorWidthLong:           UInt32                           read GetPreferredVectorWidthLong;
    public property PreferredVectorWidthFloat:          UInt32                           read GetPreferredVectorWidthFloat;
    public property PreferredVectorWidthDouble:         UInt32                           read GetPreferredVectorWidthDouble;
    public property PreferredVectorWidthHalf:           UInt32                           read GetPreferredVectorWidthHalf;
    public property NativeVectorWidthChar:              UInt32                           read GetNativeVectorWidthChar;
    public property NativeVectorWidthShort:             UInt32                           read GetNativeVectorWidthShort;
    public property NativeVectorWidthInt:               UInt32                           read GetNativeVectorWidthInt;
    public property NativeVectorWidthLong:              UInt32                           read GetNativeVectorWidthLong;
    public property NativeVectorWidthFloat:             UInt32                           read GetNativeVectorWidthFloat;
    public property NativeVectorWidthDouble:            UInt32                           read GetNativeVectorWidthDouble;
    public property NativeVectorWidthHalf:              UInt32                           read GetNativeVectorWidthHalf;
    public property MaxClockFrequency:                  UInt32                           read GetMaxClockFrequency;
    public property AddressBits:                        UInt32                           read GetAddressBits;
    public property MaxMemAllocSize:                    UInt64                           read GetMaxMemAllocSize;
    public property ImageSupport:                       Bool                             read GetImageSupport;
    public property MaxReadImageArgs:                   UInt32                           read GetMaxReadImageArgs;
    public property MaxWriteImageArgs:                  UInt32                           read GetMaxWriteImageArgs;
    public property MaxReadWriteImageArgs:              UInt32                           read GetMaxReadWriteImageArgs;
    public property IlVersion:                          String                           read GetIlVersion;
    public property Image2dMaxWidth:                    UIntPtr                          read GetImage2dMaxWidth;
    public property Image2dMaxHeight:                   UIntPtr                          read GetImage2dMaxHeight;
    public property Image3dMaxWidth:                    UIntPtr                          read GetImage3dMaxWidth;
    public property Image3dMaxHeight:                   UIntPtr                          read GetImage3dMaxHeight;
    public property Image3dMaxDepth:                    UIntPtr                          read GetImage3dMaxDepth;
    public property ImageMaxBufferSize:                 UIntPtr                          read GetImageMaxBufferSize;
    public property ImageMaxArraySize:                  UIntPtr                          read GetImageMaxArraySize;
    public property MaxSamplers:                        UInt32                           read GetMaxSamplers;
    public property ImagePitchAlignment:                UInt32                           read GetImagePitchAlignment;
    public property ImageBaseAddressAlignment:          UInt32                           read GetImageBaseAddressAlignment;
    public property MaxPipeArgs:                        UInt32                           read GetMaxPipeArgs;
    public property PipeMaxActiveReservations:          UInt32                           read GetPipeMaxActiveReservations;
    public property PipeMaxPacketSize:                  UInt32                           read GetPipeMaxPacketSize;
    public property MaxParameterSize:                   UIntPtr                          read GetMaxParameterSize;
    public property MemBaseAddrAlign:                   UInt32                           read GetMemBaseAddrAlign;
    public property SingleFpConfig:                     DeviceFPConfig                   read GetSingleFpConfig;
    public property DoubleFpConfig:                     DeviceFPConfig                   read GetDoubleFpConfig;
    public property GlobalMemCacheType:                 DeviceMemCacheType               read GetGlobalMemCacheType;
    public property GlobalMemCachelineSize:             UInt32                           read GetGlobalMemCachelineSize;
    public property GlobalMemCacheSize:                 UInt64                           read GetGlobalMemCacheSize;
    public property GlobalMemSize:                      UInt64                           read GetGlobalMemSize;
    public property MaxConstantBufferSize:              UInt64                           read GetMaxConstantBufferSize;
    public property MaxConstantArgs:                    UInt32                           read GetMaxConstantArgs;
    public property MaxGlobalVariableSize:              UIntPtr                          read GetMaxGlobalVariableSize;
    public property GlobalVariablePreferredTotalSize:   UIntPtr                          read GetGlobalVariablePreferredTotalSize;
    public property LocalMemType:                       DeviceLocalMemType               read GetLocalMemType;
    public property LocalMemSize:                       UInt64                           read GetLocalMemSize;
    public property ErrorCorrectionSupport:             Bool                             read GetErrorCorrectionSupport;
    public property ProfilingTimerResolution:           UIntPtr                          read GetProfilingTimerResolution;
    public property EndianLittle:                       Bool                             read GetEndianLittle;
    public property Available:                          Bool                             read GetAvailable;
    public property CompilerAvailable:                  Bool                             read GetCompilerAvailable;
    public property LinkerAvailable:                    Bool                             read GetLinkerAvailable;
    public property ExecutionCapabilities:              DeviceExecCapabilities           read GetExecutionCapabilities;
    public property QueueOnHostProperties:              CommandQueueProperties           read GetQueueOnHostProperties;
    public property QueueOnDeviceProperties:            CommandQueueProperties           read GetQueueOnDeviceProperties;
    public property QueueOnDevicePreferredSize:         UInt32                           read GetQueueOnDevicePreferredSize;
    public property QueueOnDeviceMaxSize:               UInt32                           read GetQueueOnDeviceMaxSize;
    public property MaxOnDeviceQueues:                  UInt32                           read GetMaxOnDeviceQueues;
    public property MaxOnDeviceEvents:                  UInt32                           read GetMaxOnDeviceEvents;
    public property BuiltInKernels:                     String                           read GetBuiltInKernels;
    public property Name:                               String                           read GetName;
    public property Vendor:                             String                           read GetVendor;
    public property Profile:                            String                           read GetProfile;
    public property Version:                            String                           read GetVersion;
    public property OpenclCVersion:                     String                           read GetOpenclCVersion;
    public property Extensions:                         String                           read GetExtensions;
    public property PrintfBufferSize:                   UIntPtr                          read GetPrintfBufferSize;
    public property PreferredInteropUserSync:           Bool                             read GetPreferredInteropUserSync;
    public property PartitionMaxSubDevices:             UInt32                           read GetPartitionMaxSubDevices;
    public property PartitionProperties:                array of DevicePartitionProperty read GetPartitionProperties;
    public property PartitionAffinityDomain:            DeviceAffinityDomain             read GetPartitionAffinityDomain;
    public property PartitionType:                      array of DevicePartitionProperty read GetPartitionType;
    public property ReferenceCount:                     UInt32                           read GetReferenceCount;
    public property SvmCapabilities:                    DeviceSVMCapabilities            read GetSvmCapabilities;
    public property PreferredPlatformAtomicAlignment:   UInt32                           read GetPreferredPlatformAtomicAlignment;
    public property PreferredGlobalAtomicAlignment:     UInt32                           read GetPreferredGlobalAtomicAlignment;
    public property PreferredLocalAtomicAlignment:      UInt32                           read GetPreferredLocalAtomicAlignment;
    public property MaxNumSubGroups:                    UInt32                           read GetMaxNumSubGroups;
    public property SubGroupIndependentForwardProgress: Bool                             read GetSubGroupIndependentForwardProgress;
    
  end;
  
  {$endregion Device}
  
  {$region Context}
  
  ContextProperties = partial class
    
    public constructor(ntv: cl_context);
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    private function GetReferenceCount: UInt32;
    private function GetNumDevices: UInt32;
    private function GetProperties: array of ContextProperties;
    
    public property ReferenceCount: UInt32                     read GetReferenceCount;
    public property NumDevices:     UInt32                     read GetNumDevices;
    public property Properties:     array of ContextProperties read GetProperties;
    
  end;
  
  {$endregion Context}
  
  {$region ProgramCode}
  
  ProgramCodeProperties = partial class
    
    public constructor(ntv: cl_program);
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    private function GetReferenceCount: UInt32;
    private function GetSource: String;
    private function GetIl: array of Byte;
    private function GetNumKernels: UIntPtr;
    private function GetKernelNames: String;
    private function GetScopeGlobalCtorsPresent: Bool;
    private function GetScopeGlobalDtorsPresent: Bool;
    
    public property ReferenceCount:          UInt32        read GetReferenceCount;
    public property Source:                  String        read GetSource;
    public property Il:                      array of Byte read GetIl;
    public property NumKernels:              UIntPtr       read GetNumKernels;
    public property KernelNames:             String        read GetKernelNames;
    public property ScopeGlobalCtorsPresent: Bool          read GetScopeGlobalCtorsPresent;
    public property ScopeGlobalDtorsPresent: Bool          read GetScopeGlobalDtorsPresent;
    
  end;
  
  {$endregion ProgramCode}
  
  {$region Kernel}
  
  KernelProperties = partial class
    
    public constructor(ntv: cl_kernel);
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    private function GetFunctionName: String;
    private function GetNumArgs: UInt32;
    private function GetReferenceCount: UInt32;
    private function GetAttributes: String;
    
    public property FunctionName:   String read GetFunctionName;
    public property NumArgs:        UInt32 read GetNumArgs;
    public property ReferenceCount: UInt32 read GetReferenceCount;
    public property Attributes:     String read GetAttributes;
    
  end;
  
  {$endregion Kernel}
  
  {$region MemorySegment}
  
  MemorySegmentProperties = partial class
    
    public constructor(ntv: cl_mem);
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    private function GetFlags: MemFlags;
    private function GetHostPtr: IntPtr;
    private function GetMapCount: UInt32;
    private function GetReferenceCount: UInt32;
    private function GetUsesSvmPointer: Bool;
    
    public property Flags:          MemFlags read GetFlags;
    public property HostPtr:        IntPtr   read GetHostPtr;
    public property MapCount:       UInt32   read GetMapCount;
    public property ReferenceCount: UInt32   read GetReferenceCount;
    public property UsesSvmPointer: Bool     read GetUsesSvmPointer;
    
  end;
  
  {$endregion MemorySegment}
  
  {$region MemorySubSegment}
  
  MemorySubSegmentProperties = partial class(MemorySegmentProperties)
    
    public constructor(ntv: cl_mem);
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    private function GetOffset: UIntPtr;
    
    public property Offset: UIntPtr read GetOffset;
    
  end;
  
  {$endregion MemorySubSegment}
  
  {$region CLArray}
  
  CLArrayProperties = partial class
    
    public constructor(ntv: cl_mem);
    private constructor := raise new System.InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    private function GetFlags: MemFlags;
    private function GetHostPtr: IntPtr;
    private function GetMapCount: UInt32;
    private function GetReferenceCount: UInt32;
    private function GetUsesSvmPointer: Bool;
    
    public property Flags:          MemFlags read GetFlags;
    public property HostPtr:        IntPtr   read GetHostPtr;
    public property MapCount:       UInt32   read GetMapCount;
    public property ReferenceCount: UInt32   read GetReferenceCount;
    public property UsesSvmPointer: Bool     read GetUsesSvmPointer;
    
  end;
  
  {$endregion CLArray}
  
  {$endregion Properties}
  
  {$region Wrappers}
  // Для параметров команд
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает значение типа T
  CommandQueue<T> = abstract partial class end;
  ///Представляет аргумент, передаваемый в вызов kernel-а
  KernelArg = abstract partial class end;
  
  {$region Platform}
  
  ///Представляет платформу OpenCL, объединяющую одно или несколько устройств
  Platform = partial class
    private ntv: cl_platform_id;
    
    ///Создаёт обёртку для указанного неуправляемого объекта
    public constructor(ntv: cl_platform_id) := self.ntv := ntv;
    private constructor := raise new OpenCLABCInternalException;
    
    private static all_need_init := true;
    private static _all: IList<Platform>;
    private static function GetAll: IList<Platform>;
    begin
      if all_need_init then
      begin
        var c: UInt32;
        OpenCLABCInternalException.RaiseIfError(
          cl.GetPlatformIDs(0, IntPtr.Zero, c)
        );
        
        if c<>0 then
        begin
          var all_arr := new cl_platform_id[c];
          OpenCLABCInternalException.RaiseIfError(
            cl.GetPlatformIDs(c, all_arr[0], IntPtr.Zero)
          );
          
          _all := new ReadOnlyCollection<Platform>(all_arr.ConvertAll(pl->new Platform(pl)));
        end else
          _all := nil;
        
        all_need_init := false;
      end;
      Result := _all;
    end;
    ///Возвращает список всех доступных платформ OpenCL
    ///Данный список создаётся 1 раз, при первом обращении
    public static property All: IList<Platform> read GetAll;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}]';
    
  end;
  
  {$endregion Platform}
  
  {$region Device}
  
  ///Представляет устройство, поддерживающее OpenCL
  Device = partial class
    private ntv: cl_device_id;
    
    private constructor(ntv: cl_device_id) := self.ntv := ntv;
    ///Создаёт обёртку для указанного неуправляемого объекта
    public static function FromNative(ntv: cl_device_id): Device;
    
    private constructor := raise new OpenCLABCInternalException;
    
    private function GetBasePlatform: Platform;
    begin
      var pl: cl_platform_id;
      OpenCLABCInternalException.RaiseIfError(
        cl.GetDeviceInfo(self.ntv, DeviceInfo.DEVICE_PLATFORM, new UIntPtr(sizeof(cl_platform_id)), pl, IntPtr.Zero)
      );
      Result := new Platform(pl);
    end;
    ///Возвращает платформу данного устройства
    public property BasePlatform: Platform read GetBasePlatform;
    
    ///Собирает массив устройств указанного типа для указанной платформы
    ///Возвращает nil, если ни одно устройство не найдено
    public static function GetAllFor(pl: Platform; t: DeviceType): array of Device;
    begin
      
      var c: UInt32;
      var ec := cl.GetDeviceIDs(pl.ntv, t, 0, IntPtr.Zero, c);
      if ec=ErrorCode.DEVICE_NOT_FOUND then exit;
      OpenCLABCInternalException.RaiseIfError(ec);
      
      var all := new cl_device_id[c];
      OpenCLABCInternalException.RaiseIfError(
        cl.GetDeviceIDs(pl.ntv, t, c, all[0], IntPtr.Zero)
      );
      
      Result := all.ConvertAll(dvc->new Device(dvc));
    end;
    ///Собирает массив устройств GPU для указанной платформы
    ///Возвращает nil, если ни одно устройство не найдено
    public static function GetAllFor(pl: Platform) := GetAllFor(pl, DeviceType.DEVICE_TYPE_GPU);
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}]';
    
  end;
  
  {$endregion Device}
  
  {$region SubDevice}
  
  ///Представляет виртуальное устройство, использующее часть ядер другого устройства
  ///Объекты данного типа обычно создаются методами "Device.Split*"
  SubDevice = partial class(Device)
    private _parent: cl_device_id;
    ///Возвращает родительское устройство, часть ядер которого использует данное устройство
    public property Parent: Device read Device.FromNative(_parent);
    
    private constructor(parent, ntv: cl_device_id);
    begin
      inherited Create(ntv);
      self._parent := parent;
    end;
    
    private constructor := inherited;
    
    ///Вызывает Dispose. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override :=
    OpenCLABCInternalException.RaiseIfError(cl.ReleaseDevice(ntv));
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{inherited ToString} of {Parent}';
    
  end;
  
  {$endregion SubDevice}
  
  {$region Context}
  
  ///Представляет контекст для хранения данных и выполнения команд на GPU
  Context = partial class
    private ntv: cl_context;
    
    private dvcs: IList<Device>;
    ///Возвращает список устройств, используемых данным контекстом
    public property AllDevices: IList<Device> read dvcs;
    
    private main_dvc: Device;
    ///Возвращает главное устройство контекста, на котором выделяется память
    public property MainDevice: Device        read main_dvc;
    
    private function GetAllNtvDevices: array of cl_device_id;
    begin
      Result := new cl_device_id[dvcs.Count];
      for var i := 0 to Result.Length-1 do
        Result[i] := dvcs[i].ntv;
    end;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}] on devices: [{AllDevices.JoinToString('', '')}]; Main device: {MainDevice}';
    
    {$region Default}
    
    private static default_was_inited := 0;
    private static _default: Context;
    
    private static function GetDefault: Context;
    begin
      if Interlocked.CompareExchange(default_was_inited, 1, 0)=0 then
        Interlocked.CompareExchange(_default, MakeNewDefaultContext, nil);
      Result := _default;
    end;
    private static procedure SetDefault(new_default: Context);
    begin
      default_was_inited := 1;
      _default := new_default;
    end;
    ///Возвращает или задаёт главный контекст, используемый там, где контекст не указывается явно (как неявные очереди)
    ///При первом обращении к данному свойству OpenCLABC пытается создать новый контекст
    ///При создании главного контекста приоритет отдаётся полноценным GPU, но если таких нет - берётся любое устройство, поддерживающее OpenCL
    ///
    ///Если устройств поддерживающих OpenCL нет, то Context.Default изначально будет nil
    ///Но это свидетельствует скорее об отсутствии драйверов, чем отстутсвии устройств
    public static property &Default: Context read GetDefault write SetDefault;
    
    ///Создаёт новый контекст, соответствующий изначальному значению Context.Default
    protected static function MakeNewDefaultContext: Context;
    begin
      Result := nil;
      
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
        ntv_dvcs[i] := dvcs[i].ntv;
      
      var ec: ErrorCode;
      self.ntv := cl.CreateContext(nil, ntv_dvcs.Count, ntv_dvcs, nil, IntPtr.Zero, ec);
      OpenCLABCInternalException.RaiseIfError(ec);
      
      self.dvcs := if dvcs.IsReadOnly then dvcs else new ReadOnlyCollection<Device>(dvcs.ToArray);
      self.main_dvc := main_dvc;
    end;
    ///Создаёт контекст с указанными AllDevices
    ///В качестве MainDevice берётся первое устройство из массива
    public constructor(params dvcs: array of Device) := Create(dvcs, dvcs[0]);
    
    ///Получает неуправляемые устройства указанного неуправляемого контекста
    protected static function GetContextDevices(ntv: cl_context): array of Device;
    begin
      
      var sz: UIntPtr;
      OpenCLABCInternalException.RaiseIfError(
        cl.GetContextInfo(ntv, ContextInfo.CONTEXT_DEVICES, UIntPtr.Zero, nil, sz)
      );
      
      var res := new cl_device_id[uint64(sz) div Marshal.SizeOf&<cl_device_id>];
      OpenCLABCInternalException.RaiseIfError(
        cl.GetContextInfo(ntv, ContextInfo.CONTEXT_DEVICES, sz, res[0], IntPtr.Zero)
      );
      
      Result := res.ConvertAll(dvc->new Device(dvc));
    end;
    private procedure InitFromNtv(ntv: cl_context; dvcs: IList<Device>; main_dvc: Device);
    begin
      CheckMainDevice(main_dvc, dvcs);
      OpenCLABCInternalException.RaiseIfError( cl.RetainContext(ntv) );
      self.ntv := ntv;
      // Копирование должно происходить в вызывающих методах
      self.dvcs := if dvcs.IsReadOnly then dvcs else new ReadOnlyCollection<Device>(dvcs);
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
    
    private constructor := raise new OpenCLABCInternalException;
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose;
    begin
      var prev := Interlocked.Exchange(self.ntv.val, IntPtr.Zero);
      if prev=IntPtr.Zero then exit;
      OpenCLABCInternalException.RaiseIfError( cl.ReleaseContext(new cl_context(prev)) );
    end;
    ///Вызывает Dispose. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override := Dispose;
    
    {$endregion constructor's}
    
  end;
  
  {$endregion Context}
  
  {$region ProgramCode}
  
  ///Представляет контейнер с откомпилированным кодом для GPU, содержащим подпрограммы-kernel'ы
  ProgramCode = partial class
    private ntv: cl_program;
    
    private _c: Context;
    ///Возвращает контекст, на котором компилировали данный код для GPU
    public property BaseContext: Context read _c;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}]';
    
    {$region constructor's}
    
    private procedure Build;
    begin
      var ec := cl.BuildProgram(self.ntv, _c.dvcs.Count,_c.GetAllNtvDevices, nil, nil,IntPtr.Zero);
      if not ec.IS_ERROR then exit;
      
      if ec=ErrorCode.BUILD_PROGRAM_FAILURE then
      begin
        var sb := new StringBuilder($'Ошибка компиляции OpenCL программы:');
        
        foreach var dvc in _c.AllDevices do
        begin
          sb += #10#10;
          sb += dvc.ToString;
          sb += ':'#10;
          
          var sz: UIntPtr;
          OpenCLABCInternalException.RaiseIfError(
            cl.GetProgramBuildInfo(self.ntv, dvc.ntv, ProgramBuildInfo.PROGRAM_BUILD_LOG, UIntPtr.Zero,IntPtr.Zero,sz)
          );
          
          var str_ptr := Marshal.AllocHGlobal(IntPtr(pointer(sz)));
          try
            OpenCLABCInternalException.RaiseIfError(
              cl.GetProgramBuildInfo(self.ntv, dvc.ntv, ProgramBuildInfo.PROGRAM_BUILD_LOG, sz,str_ptr,IntPtr.Zero)
            );
            sb += Marshal.PtrToStringAnsi(str_ptr);
          finally
            Marshal.FreeHGlobal(str_ptr);
          end;
          
        end;
        
        raise new OpenCLException(ec, sb.ToString);
      end else
        OpenCLABCInternalException.RaiseIfError(ec);
      
    end;
    
    ///Компилирует указанные тексты программ в указанном контексте
    ///Внимание! Именно тексты, Не имена файлов
    public constructor(c: Context; params file_texts: array of string);
    begin
      
      var ec: ErrorCode;
      self.ntv := cl.CreateProgramWithSource(c.ntv, file_texts.Length, file_texts, nil, ec);
      OpenCLABCInternalException.RaiseIfError(ec);
      
      self._c := c;
      self.Build;
    end;
    ///Компилирует указанные тексты программ в контексте Context.Default
    ///Внимание! Именно тексты, Не имена файлов
    public constructor(params file_texts: array of string) := Create(Context.Default, file_texts);
    
    private constructor(ntv: cl_program; c: Context);
    begin
      OpenCLABCInternalException.RaiseIfError( cl.RetainProgram(ntv) );
      self._c := c;
      self.ntv := ntv;
    end;
    
    private static function GetProgContext(ntv: cl_program): Context;
    begin
      var c: cl_context;
      OpenCLABCInternalException.RaiseIfError(
        cl.GetProgramInfo(ntv, ProgramInfo.PROGRAM_CONTEXT, new UIntPtr(Marshal.SizeOf&<cl_context>), c, IntPtr.Zero)
      );
      Result := new Context(c);
    end;
    ///Создаёт обёртку для указанного неуправляемого объекта
    ///При успешном создании обёртки вызывается cl.Retain
    ///А во время вызова .Dispose - cl.Release
    public constructor(ntv: cl_program) :=
    Create(ntv, GetProgContext(ntv));
    
    private constructor := raise new OpenCLABCInternalException;
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose;
    begin
      var prev := Interlocked.Exchange(self.ntv.val, IntPtr.Zero);
      if prev=IntPtr.Zero then exit;
      OpenCLABCInternalException.RaiseIfError( cl.ReleaseProgram(new cl_program(prev)) );
    end;
    ///Вызывает Dispose. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override := Dispose;
    
    {$endregion constructor's}
    
    {$region Serialize}
    
    ///Сохраняет прекомпилированную программу как набор байт
    public function Serialize: array of array of byte;
    begin
      var sz: UIntPtr;
      
      OpenCLABCInternalException.RaiseIfError( cl.GetProgramInfo(ntv, ProgramInfo.PROGRAM_BINARY_SIZES, UIntPtr.Zero, nil, sz) );
      var szs := new UIntPtr[sz.ToUInt64 div sizeof(UIntPtr)];
      OpenCLABCInternalException.RaiseIfError( cl.GetProgramInfo(ntv, ProgramInfo.PROGRAM_BINARY_SIZES, sz, szs[0], IntPtr.Zero) );
      
      var res := new IntPtr[szs.Length];
      SetLength(Result, szs.Length);
      
      for var i := 0 to szs.Length-1 do res[i] := Marshal.AllocHGlobal(IntPtr(pointer(szs[i])));
      try
        OpenCLABCInternalException.RaiseIfError(
          cl.GetProgramInfo(ntv, ProgramInfo.PROGRAM_BINARIES, sz, res[0], IntPtr.Zero)
        );
        for var i := 0 to szs.Length-1 do
        begin
          var a := new byte[szs[i].ToUInt64];
          Marshal.Copy(res[i], a, 0, a.Length);
          Result[i] := a;
        end;
      finally
        for var i := 0 to szs.Length-1 do Marshal.FreeHGlobal(res[i]);
      end;
      
    end;
    
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
        c.ntv, dvcs.Length, dvcs[0],
        bin.ConvertAll(a->new UIntPtr(a.Length))[0], bin,
        IntPtr.Zero, ec
      );
      OpenCLABCInternalException.RaiseIfError(ec);
      
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
  
  {$region Kernel}
  
  ///Представляет подпрограмму, выполняемую на GPU
  Kernel = partial class
    private ntv: cl_kernel;
    
    private code: ProgramCode;
    ///Возвращает контейнер кода, содержащий данную подпрограмму
    public property CodeContainer: ProgramCode read code;
    
    private k_name: string;
    ///Возвращает имя данной подпрограммы
    public property Name: string read k_name;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{Name}:{ntv.val}] from {code}';
    
    {$region constructor's}
    
    ///Создаёт независимый клон неуправляемого объекта
    ///Внимание: Клон НЕ будет удалён автоматически при сборке мусора. Для него надо вручную вызывать cl.ReleaseKernel
    protected function MakeNewNtv: cl_kernel;
    begin
      var ec: ErrorCode;
      Result := cl.CreateKernel(code.ntv, k_name, ec);
      OpenCLABCInternalException.RaiseIfError(ec);
    end;
    private constructor(code: ProgramCode; name: string);
    begin
      self.code := code;
      self.k_name := name;
      self.ntv := self.MakeNewNtv;
    end;
    
    ///Создаёт обёртку для указанного неуправляемого объекта
    ///При успешном создании обёртки вызывается cl.Retain
    ///А во время вызова .Dispose - cl.Release
    public constructor(ntv: cl_kernel; retain: boolean := true);
    begin
      
      var code_ntv: cl_program;
      OpenCLABCInternalException.RaiseIfError(
        cl.GetKernelInfo(ntv, KernelInfo.KERNEL_PROGRAM, new UIntPtr(cl_program.Size), code_ntv, IntPtr.Zero)
      );
      self.code := new ProgramCode(code_ntv);
      
      var sz: UIntPtr;
      OpenCLABCInternalException.RaiseIfError(
        cl.GetKernelInfo(ntv, KernelInfo.KERNEL_FUNCTION_NAME, UIntPtr.Zero, nil, sz)
      );
      var str_ptr := Marshal.AllocHGlobal(IntPtr(pointer(sz)));
      try
        OpenCLABCInternalException.RaiseIfError(
          cl.GetKernelInfo(ntv, KernelInfo.KERNEL_FUNCTION_NAME, sz, str_ptr, IntPtr.Zero)
        );
        self.k_name := Marshal.PtrToStringAnsi(str_ptr);
      finally
        Marshal.FreeHGlobal(str_ptr);
      end;
      
      if retain then OpenCLABCInternalException.RaiseIfError( cl.RetainKernel(ntv) );
      self.ntv := ntv;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose;
    begin
      var prev := Interlocked.Exchange(self.ntv.val, IntPtr.Zero);
      if prev=IntPtr.Zero then exit;
      OpenCLABCInternalException.RaiseIfError( cl.ReleaseKernel(new cl_kernel(prev)) );
    end;
    ///Вызывает Dispose. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override := Dispose;
    
    {$endregion constructor's}
    
    {$region UseExclusiveNative}
    
    private ntv_in_use := 0;
    protected [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    ///Гарантирует что неуправляемый объект будет использоваться только в 1 потоке одновременно
    ///Если неуправляемый объект данного kernel-а используется другим потоком - в процедурную переменную передаётся его независимый клон
    ///Внимание: Клон неуправляемого объекта будет удалён сразу после выхода из вашей процедурной переменной, если не вызвать cl.RetainKernel
    procedure UseExclusiveNative(p: cl_kernel->()) :=
    if Interlocked.CompareExchange(ntv_in_use, 1, 0)=0 then
    try
      p(self.ntv);
    finally
      ntv_in_use := 0;
    end else
    begin
      var k := MakeNewNtv;
      try
        p(k);
      finally
        OpenCLABCInternalException.RaiseIfError( cl.ReleaseKernel(k) );
      end;
    end;
    protected [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    ///Гарантирует что неуправляемый объект будет использоваться только в 1 потоке одновременно
    ///Если неуправляемый объект данного kernel-а используется другим потоком - в процедурную переменную передаётся его независимый клон
    ///Внимание: Клон неуправляемого объекта будет удалён сразу после выхода из вашей процедурной переменной, если не вызвать cl.RetainKernel
    function UseExclusiveNative<T>(f: cl_kernel->T): T;
    begin
      
      if Interlocked.CompareExchange(ntv_in_use, 1, 0)=0 then
      try
        Result := f(self.ntv);
      finally
        ntv_in_use := 0;
      end else
      begin
        var k := MakeNewNtv;
        try
          Result := f(k);
        finally
          OpenCLABCInternalException.RaiseIfError( cl.ReleaseKernel(k) );
        end;
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
  
  ///Представляет контейнер с откомпилированным кодом для GPU, содержащим подпрограммы-kernel'ы
  ProgramCode = partial class
    
    ///Находит в коде kernel с указанным именем
    ///Регистр имени важен!
    public property KernelByName[kname: string]: Kernel read new Kernel(self, kname); default;
    
    ///Создаёт массив из всех kernel-ов данного кода
    public function GetAllKernels: array of Kernel;
    begin
      
      var c: UInt32;
      OpenCLABCInternalException.RaiseIfError( cl.CreateKernelsInProgram(ntv, 0, IntPtr.Zero, c) );
      
      var res := new cl_kernel[c];
      OpenCLABCInternalException.RaiseIfError( cl.CreateKernelsInProgram(ntv, c, res[0], IntPtr.Zero) );
      
      Result := res.ConvertAll(k->new Kernel(k, false));
    end;
    
  end;
  
  {$endregion Kernel}
  
  {$region NativeValue}
  
  ///Представляет запись, значение которой хранится в неуправляемой области памяти
  NativeValue<T> = partial class(System.IDisposable)
  where T: record;
    private ptr := Marshal.AllocHGlobal(Marshal.SizeOf&<T>);
    
    ///Выделяет участок неуправляемой памяти и сохраняет в него указанное значение
    public constructor(o: T) := self.Value := o;
    public static function operator implicit(o: T): NativeValue<T> := new NativeValue<T>(o);
    
    private function PtrUntyped := pointer(ptr);
    ///Возвращает указатель на значение, сохранённое неуправляемой памяти
    public property Pointer: ^T read PtrUntyped();
    ///Возвращает или задаёт значение, сохранённое неуправляемой памяти
    public property Value: T read Pointer^ write Pointer^ := value;
    
    ///Освобождает значение, сохранённое неуправляемой памяти
    ///Ничего не делает, если значение уже освобождено
    ///Этот метод потоко-безопасен
    public procedure Dispose;
    begin
      var l_ptr := Interlocked.Exchange(self.ptr, IntPtr.Zero);
      Marshal.FreeHGlobal(l_ptr);
    end;
    ///Вызывает Dispose. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override := Dispose;
    
  end;
  
  {$endregion NativeValue}
  
  {$region MemorySegment}
  
  ///Представляет область памяти устройства OpenCL (обычно GPU)
  MemorySegment = partial class
    private ntv: cl_mem;
    
    private static function GetSize(ntv: cl_mem): UIntPtr;
    begin
      OpenCLABCInternalException.RaiseIfError(
        cl.GetMemObjectInfo(ntv, MemInfo.MEM_SIZE, new UIntPtr(UIntPtr.Size), Result, IntPtr.Zero)
      );
    end;
    ///Возвращает размер области памяти в байтах
    public property Size: UIntPtr read GetSize(ntv);
    ///Возвращает размер области памяти в байтах
    public property Size32: UInt32 read Size.ToUInt32;
    ///Возвращает размер области памяти в байтах
    public property Size64: UInt64 read Size.ToUInt64;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name}[{ntv.val}] of size {Size}';
    
    {$region constructor's}
    
    ///Выделяет область памяти устройства OpenCL указанного в байтах размера
    ///Память выделяется в указанном контексте
    public constructor(size: UIntPtr; c: Context);
    begin
      
      var ec: ErrorCode;
      self.ntv := cl.CreateBuffer(c.ntv, MemFlags.MEM_READ_WRITE, size, IntPtr.Zero, ec);
      OpenCLABCInternalException.RaiseIfError(ec);
      
      GC.AddMemoryPressure(size.ToUInt64);
      
    end;
    ///Выделяет область памяти устройства OpenCL указанного в байтах размера
    ///Память выделяется в указанном контексте
    public constructor(size: integer; c: Context) := Create(new UIntPtr(size), c);
    ///Выделяет область памяти устройства OpenCL указанного в байтах размера
    ///Память выделяется в указанном контексте
    public constructor(size: int64; c: Context)   := Create(new UIntPtr(size), c);
    
    ///Выделяет область памяти устройства OpenCL указанного в байтах размера
    ///Память выделяется в контексте Context.Default
    public constructor(size: UIntPtr) := Create(size, Context.Default);
    ///Выделяет область памяти устройства OpenCL указанного в байтах размера
    ///Память выделяется в контексте Context.Default
    public constructor(size: integer) := Create(new UIntPtr(size));
    ///Выделяет область памяти устройства OpenCL указанного в байтах размера
    ///Память выделяется в контексте Context.Default
    public constructor(size: int64)   := Create(new UIntPtr(size));
    
    private constructor(ntv: cl_mem);
    begin
      self.ntv := ntv;
      cl.RetainMemObject(ntv);
    end;
    ///Создаёт обёртку для указанного неуправляемого объекта
    ///При успешном создании обёртки вызывается cl.Retain
    ///А во время вызова .Dispose - cl.Release
    public static function FromNative(ntv: cl_mem): MemorySegment;
    
    private constructor := raise new OpenCLABCInternalException;
    
    {$endregion constructor's}
    
    {$region 1#Write&Read}
    
    ///Заполняет всю область памяти данными, находящимися по указанному адресу в RAM
    public function WriteData(ptr: CommandQueue<IntPtr>): MemorySegment;
    
    ///Заполняет часть области памяти данными, находящимися по указанному адресу в RAM
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function WriteData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegment;
    
    ///Читает всё содержимое области памяти в RAM, по указанному адресу
    public function ReadData(ptr: CommandQueue<IntPtr>): MemorySegment;
    
    ///Читает часть содержимого области памяти в RAM, по указанному адресу
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function ReadData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegment;
    
    ///Заполняет всю область памяти данными, находящимися по указанному адресу в RAM
    public function WriteData(ptr: pointer): MemorySegment;
    
    ///Заполняет часть области памяти данными, находящимися по указанному адресу в RAM
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function WriteData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegment;
    
    ///Читает всё содержимое области памяти в RAM, по указанному адресу
    public function ReadData(ptr: pointer): MemorySegment;
    
    ///Читает часть содержимого области памяти в RAM, по указанному адресу
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function ReadData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegment;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function WriteValue<TRecord>(val: TRecord): MemorySegment; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function WriteValue<TRecord>(val: CommandQueue<TRecord>): MemorySegment; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function WriteValue<TRecord>(val: NativeValue<TRecord>): MemorySegment; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function WriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegment; where TRecord: record;
    
    ///Читает значение размерного типа из начала области памяти в указанное значение
    public function ReadValue<TRecord>(val: NativeValue<TRecord>): MemorySegment; where TRecord: record;
    
    ///Читает значение размерного типа из начала области памяти в указанное значение
    public function ReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegment; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function WriteValue<TRecord>(val: TRecord; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function WriteValue<TRecord>(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function WriteValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function WriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Читает значение размерного типа из области памяти в указанное значение
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ReadValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Читает значение размерного типа из области памяти в указанное значение
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function WriteArray1<TRecord>(a: array of TRecord): MemorySegment; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function WriteArray2<TRecord>(a: array[,] of TRecord): MemorySegment; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function WriteArray3<TRecord>(a: array[,,] of TRecord): MemorySegment; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ReadArray1<TRecord>(a: array of TRecord): MemorySegment; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ReadArray2<TRecord>(a: array[,] of TRecord): MemorySegment; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ReadArray3<TRecord>(a: array[,,] of TRecord): MemorySegment; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function WriteArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function WriteArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function WriteArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ReadArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ReadArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ReadArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function WriteArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegment; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ReadArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegment; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function WriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    {$endregion 1#Write&Read}
    
    {$region 2#Fill}
    
    ///Читает pattern_len байт из RAM по указанному адресу и заполняет их копиями всю область памяти
    public function FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): MemorySegment;
    
    ///Читает pattern_len байт из RAM по указанному адресу и заполняет их копиями часть области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function FillData(ptr: CommandQueue<IntPtr>; pattern_len, mem_offset, len: CommandQueue<integer>): MemorySegment;
    
    ///Заполняет всю область памяти копиями указанного значения размерного типа
    public function FillValue<TRecord>(val: TRecord): MemorySegment; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного значения размерного типа
    public function FillValue<TRecord>(val: CommandQueue<TRecord>): MemorySegment; where TRecord: record;
    
    ///Заполняет часть области памяти копиями указанного значения размерного типа
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function FillValue<TRecord>(val: TRecord; mem_offset, len: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Заполняет часть области памяти копиями указанного значения размерного типа
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function FillValue<TRecord>(val: CommandQueue<TRecord>; mem_offset, len: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного массива
    public function FillArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegment; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного массива
    public function FillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного массива
    public function FillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment; where TRecord: record;
    
    ///Заполняет часть области памяти копиями части указанного массива
    ///a_offset(-ы) указывают индекс в массиве
    ///pattern_len указывает кол-во задействованных элементов массива
    ///len указывает кол-во задействованных в операции байт
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function FillArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Заполняет часть области памяти копиями части указанного массива
    ///a_offset(-ы) указывают индекс в массиве
    ///pattern_len указывает кол-во задействованных элементов массива
    ///len указывает кол-во задействованных в операции байт
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function FillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Заполняет часть области памяти копиями части указанного массива
    ///a_offset(-ы) указывают индекс в массиве
    ///pattern_len указывает кол-во задействованных элементов массива
    ///len указывает кол-во задействованных в операции байт
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function FillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    {$endregion 2#Fill}
    
    {$region 3#Copy}
    
    ///Копирует данные из текущей области памяти в mem
    ///Если области памяти имеют разный размер - в качестве объёма данных берётся размер меньшей области
    public function CopyTo(mem: CommandQueue<MemorySegment>): MemorySegment;
    
    ///Копирует данные из текущей области памяти в mem
    ///from_pos указывает отступ в байтах от начала области памяти, из которой копируют
    ///to_pos указывает отступ в байтах от начала области памяти, в которую копируют
    ///len указывает кол-во копируемых байт
    public function CopyTo(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegment;
    
    ///Копирует данные из mem в текущюу область памяти
    ///Если области памяти имеют разный размер - в качестве объёма данных берётся размер меньшей области
    public function CopyFrom(mem: CommandQueue<MemorySegment>): MemorySegment;
    
    ///Копирует данные из mem в текущюу область памяти
    ///from_pos указывает отступ в байтах от начала области памяти, из которой копируют
    ///to_pos указывает отступ в байтах от начала области памяти, в которую копируют
    ///len указывает кол-во копируемых байт
    public function CopyFrom(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegment;
    
    {$endregion 3#Copy}
    
    {$region Get}
    
    ///Читает значение указанного размерного типа из начала области памяти
    public function GetValue<TRecord>: TRecord; where TRecord: record;
    
    ///Читает значение указанного размерного типа из области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function GetValue<TRecord>(mem_offset: CommandQueue<integer>): TRecord; where TRecord: record;
    
    ///Читает массив максимального размера, на сколько хватит байт данной области памяти
    public function GetArray1<TRecord>: array of TRecord; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое данный области памяти
    public function GetArray1<TRecord>(len: CommandQueue<integer>): array of TRecord; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое данный области памяти
    public function GetArray2<TRecord>(len1,len2: CommandQueue<integer>): array[,] of TRecord; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое данный области памяти
    public function GetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): array[,,] of TRecord; where TRecord: record;
    
    {$endregion Get}
    
    private procedure InformGCOfRelease(prev_ntv: cl_mem); virtual :=
    GC.RemoveMemoryPressure( GetSize(prev_ntv).ToUInt64 );
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose;
    begin
      var prev_ntv := new cl_mem( Interlocked.Exchange(self.ntv.val, IntPtr.Zero) );
      if prev_ntv=cl_mem.Zero then exit;
      InformGCOfRelease(prev_ntv);
      OpenCLABCInternalException.RaiseIfError( cl.ReleaseMemObject(prev_ntv) );
    end;
    ///Вызывает Dispose. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override := Dispose;
    
  end;
  
  {$endregion MemorySegment}
  
  {$region MemorySubSegment}
  
  ///Представляет виртуальную область памяти, выделенную внутри MemorySegment
  MemorySubSegment = partial class(MemorySegment)
    
    // Только чтоб не вызвалось GC.RemoveMemoryPressure
    private parent_dispose_lock: MemorySegment;
    
    private _parent: cl_mem;
    ///Возвращает родительскую область памяти
    public property Parent: MemorySegment read MemorySegment.FromNative(_parent);
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{inherited ToString} inside {Parent}';
    
    {$region constructor's}
    
    private static function MakeSubNtv(parent: cl_mem; reg: cl_buffer_region): cl_mem;
    begin
      var ec: ErrorCode;
      Result := cl.CreateSubBuffer(parent, MemFlags.MEM_READ_WRITE, BufferCreateType.BUFFER_CREATE_TYPE_REGION, reg, ec);
      OpenCLABCInternalException.RaiseIfError(ec);
    end;
    
    ///Создаёт виртуальную область памяти, использующую указанную область из parent
    ///origin указывает отступ в байтах от начала parent
    ///size указывает размер новой области памяти
    public constructor(parent: MemorySegment; origin, size: UIntPtr);
    begin
      inherited Create( MakeSubNtv(parent.ntv, new cl_buffer_region(origin, size)) );
      self._parent := parent.ntv;
      self.parent_dispose_lock := parent;
    end;
    ///Создаёт виртуальную область памяти, использующую указанную область из parent
    ///origin указывает отступ в байтах от начала parent
    ///size указывает размер новой области памяти
    public constructor(parent: MemorySegment; origin, size: UInt32) := Create(parent, new UIntPtr(origin), new UIntPtr(size));
    ///Создаёт виртуальную область памяти, использующую указанную область из parent
    ///origin указывает отступ в байтах от начала parent
    ///size указывает размер новой области памяти
    public constructor(parent: MemorySegment; origin, size: UInt64) := Create(parent, new UIntPtr(origin), new UIntPtr(size));
    
    private constructor(parent, ntv: cl_mem);
    begin
      inherited Create(ntv);
      self.parent_dispose_lock := nil;
      self._parent := parent;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    {$endregion constructor's}
    
    private procedure InformGCOfRelease(prev_ntv: cl_mem); override := exit;
    
  end;
  
  {$endregion MemorySubSegment}
  
  {$region CLArray}
  
  ///Представляет массив записей, содержимое которого хранится на устройстве OpenCL (обычно GPU)
  CLArray<T> = partial class
  where T: record;
    private ntv: cl_mem;
    
    private len: integer;
    ///Возвращает длину массива
    public property Length: integer read len;
    ///Возвращает размер области памяти, занимаемой массивом, в байтах
    public property ByteSize: int64 read int64(len) * Marshal.SizeOf&<T>;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{self.GetType.Name.Remove(self.GetType.Name.IndexOf(''`''))}<{typeof(T).Name}>[{ntv.val}] of size {Length}';
    
    {$region constructor's}
    
    private procedure InitByLen(c: Context);
    begin
      
      var ec: ErrorCode;
      self.ntv := cl.CreateBuffer(c.ntv, MemFlags.MEM_READ_WRITE, new UIntPtr(ByteSize), IntPtr.Zero, ec);
      OpenCLABCInternalException.RaiseIfError(ec);
      
      GC.AddMemoryPressure(ByteSize);
    end;
    private procedure InitByVal(c: Context; var els: T);
    begin
      
      var ec: ErrorCode;
      self.ntv := cl.CreateBuffer(c.ntv, MemFlags.MEM_READ_WRITE + MemFlags.MEM_COPY_HOST_PTR, new UIntPtr(ByteSize), els, ec);
      OpenCLABCInternalException.RaiseIfError(ec);
      
      GC.AddMemoryPressure(ByteSize);
    end;
    
    ///Создаёт массив, указанной длины
    ///Память выделяется в указанном контексте
    public constructor(c: Context; len: integer);
    begin
      self.len := len;
      InitByLen(c);
    end;
    ///Создаёт массив, указанной длины
    ///Память выделяется в контексте Context.Default
    public constructor(len: integer) := Create(Context.Default, len);
    
    ///Создаёт массив-копию указанного массива
    ///Память выделяется в указанном контексте
    public constructor(c: Context; els: array of T);
    begin
      self.len := els.Length;
      InitByVal(c, els[0]);
    end;
    ///Создаёт массив-копию указанного массива
    ///Память выделяется в контексте Context.Default
    public constructor(els: array of T) := Create(Context.Default, els);
    
    ///Создаёт массив-копию участка указанного массива
    ///Память выделяется в указанном контексте
    public constructor(c: Context; els_from, len: integer; params els: array of T);
    begin
      self.len := len;
      InitByVal(c, els[els_from]);
    end;
    ///Создаёт массив-копию участка указанного массива
    ///Память выделяется в контексте Context.Default
    public constructor(els_from, len: integer; params els: array of T) := Create(Context.Default, els_from, len, els);
    
    ///Создаёт обёртку для указанного неуправляемого объекта
    ///При успешном создании обёртки вызывается cl.Retain
    ///А во время вызова .Dispose - cl.Release
    public constructor(ntv: cl_mem);
    begin
      
      var byte_size: UIntPtr;
      OpenCLABCInternalException.RaiseIfError(
        cl.GetMemObjectInfo(ntv, MemInfo.MEM_SIZE, new UIntPtr(Marshal.SizeOf&<UIntPtr>), byte_size, IntPtr.Zero)
      );
      
      self.len := byte_size.ToUInt64 div Marshal.SizeOf&<T>;
      self.ntv := ntv;
      
      OpenCLABCInternalException.RaiseIfError( cl.RetainMemObject(ntv) );
      GC.AddMemoryPressure(ByteSize);
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    {$endregion constructor's}
    
    private function GetItemProp(ind: integer): T;
    private procedure SetItemProp(ind: integer; value: T);
    ///Возвращает или задаёт один элемент массива
    ///Внимание! Данные свойство использует неявные очереди при каждом обращение, поэтому может быть очень не эффективным
    public property Item[ind: integer]: T read GetItemProp write SetItemProp; default;
    
    private function GetSectionProp(range: IntRange): array of T;
    private procedure SetSectionProp(range: IntRange; value: array of T);
    ///Возвращает или задаёт элементы массива в заданном диапазоне
    ///Внимание! Данные свойство использует неявные очереди при каждом обращение, поэтому может быть очень не эффективным
    public property Section[range: IntRange]: array of T read GetSectionProp write SetSectionProp;
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose;
    begin
      var prev := Interlocked.Exchange(self.ntv.val, IntPtr.Zero);
      if prev=IntPtr.Zero then exit;
      GC.RemoveMemoryPressure(ByteSize);
      OpenCLABCInternalException.RaiseIfError( cl.ReleaseMemObject(new cl_mem(prev)) );
    end;
    ///Вызывает Dispose. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override := Dispose;
    
    {$region 1#Write&Read}
    
    ///Заполняет весь данный объект CLArray<T> данными, находящимися по указанному адресу в RAM
    public function WriteData(ptr: CommandQueue<IntPtr>): CLArray<T>;
    
    ///Заполняет len элементов начиная с индекса ind данного объекта CLArray<T> данными, находящимися по указанному адресу в RAM
    public function WriteData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArray<T>;
    
    ///Читает всё содержимое данного объекта CLArray<T> в RAM, по указанному адресу
    public function ReadData(ptr: CommandQueue<IntPtr>): CLArray<T>;
    
    ///Читает len элементов начиная с индекса ind данного объекта CLArray<T> в RAM, по указанному адресу
    public function ReadData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArray<T>;
    
    ///Заполняет весь данный объект CLArray<T> данными, находящимися по указанному адресу в RAM
    public function WriteData(ptr: pointer): CLArray<T>;
    
    ///Заполняет len элементов начиная с индекса ind данного объекта CLArray<T> данными, находящимися по указанному адресу в RAM
    public function WriteData(ptr: pointer; ind, len: CommandQueue<integer>): CLArray<T>;
    
    ///Читает всё содержимое данного объекта CLArray<T> в RAM, по указанному адресу
    public function ReadData(ptr: pointer): CLArray<T>;
    
    ///Читает len элементов начиная с индекса ind данного объекта CLArray<T> в RAM, по указанному адресу
    public function ReadData(ptr: pointer; ind, len: CommandQueue<integer>): CLArray<T>;
    
    ///Записывает указанное значение по индексу ind
    public function WriteValue(val: &T; ind: CommandQueue<integer>): CLArray<T>;
    
    ///Записывает указанное значение по индексу ind
    public function WriteValue(val: CommandQueue<&T>; ind: CommandQueue<integer>): CLArray<T>;
    
    ///Записывает указанное значение по индексу ind
    public function WriteValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArray<T>;
    
    ///Записывает указанное значение по индексу ind
    public function WriteValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArray<T>;
    
    ///Читает значение по индексу ind в указанное значение
    public function ReadValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArray<T>;
    
    ///Читает значение по индексу ind в указанное значение
    public function ReadValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArray<T>;
    
    ///Записывает весь указанный массив в начало данного объекта CLArray<T>
    public function WriteArray(a: CommandQueue<array of &T>): CLArray<T>;
    
    ///Записывает len элементов массива a, начиная с индекса a_ind, в данный объект CLArray<T> по индексу ind
    public function WriteArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArray<T>;
    
    ///Читает начало данного объекта CLArray<T> в указанный массив
    public function ReadArray(a: CommandQueue<array of &T>): CLArray<T>;
    
    ///Читает len элементов данного объекта CLArray<T>, начиная с индекса ind, в массив a по индексу a_ind
    public function ReadArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArray<T>;
    
    {$endregion 1#Write&Read}
    
    {$region 2#Fill}
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями весь данный объект CLArray<T>
    public function FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): CLArray<T>;
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями len элементов начиная с индекса ind данного объекта CLArray<T>
    public function FillData(ptr: CommandQueue<IntPtr>; pattern_len, ind, len: CommandQueue<integer>): CLArray<T>;
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями весь данный объект CLArray<T>
    public function FillData(ptr: pointer; pattern_len: CommandQueue<integer>): CLArray<T>;
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями len элементов начиная с индекса ind данного объекта CLArray<T>
    public function FillData(ptr: pointer; pattern_len, ind, len: CommandQueue<integer>): CLArray<T>;
    
    ///Заполняет весь массив копиями указанного значения
    public function FillValue(val: &T): CLArray<T>;
    
    ///Заполняет весь массив копиями указанного значения
    public function FillValue(val: CommandQueue<&T>): CLArray<T>;
    
    ///Заполняет len элементов начиная с индекса ind копиями указанного значения
    public function FillValue(val: &T; ind, len: CommandQueue<integer>): CLArray<T>;
    
    ///Заполняет len элементов начиная с индекса ind копиями указанного значения
    public function FillValue(val: CommandQueue<&T>; ind, len: CommandQueue<integer>): CLArray<T>;
    
    ///Заполняет данный объект CLArray<T> компиями указанного массива
    public function FillArray(a: CommandQueue<array of &T>): CLArray<T>;
    
    ///Заполняет len элементов начиная с индекса ind компиями части указанного массива
    ///Из указанного массива берётся pattern_len элементов, начиная с индекса a_offset
    public function FillArray(a: CommandQueue<array of &T>; a_offset,pattern_len, ind,len: CommandQueue<integer>): CLArray<T>;
    
    {$endregion 2#Fill}
    
    {$region 3#Copy}
    
    ///Копирует данные из текущего массива в a
    ///Если у массивов разный размер - копируется кол-во элементов меньшего массива
    public function CopyTo(a: CommandQueue<CLArray<T>>): CLArray<T>;
    
    ///Копирует данные из текущего массива в a
    ///from_ind указывает индекс в массиве, из которого копируют
    ///to_ind указывает индекс в массиве, в который копируют
    ///len указывает кол-во копируемых элементов
    public function CopyTo(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArray<T>;
    
    ///Копирует данные из a в текущий массив
    ///Если у массивов разный размер - копируется кол-во элементов меньшего массива
    public function CopyFrom(a: CommandQueue<CLArray<T>>): CLArray<T>;
    
    ///Копирует данные из a в текущий массив
    ///from_ind указывает индекс в массиве, из которого копируют
    ///to_ind указывает индекс в массиве, в который копируют
    ///len указывает кол-во копируемых элементов
    public function CopyFrom(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArray<T>;
    
    {$endregion 3#Copy}
    
    {$region Get}
    
    ///Читает элемент по указанному индексу
    public function GetValue(ind: CommandQueue<integer>): &T;
    
    ///Читает весь CLArray<T> как обычный массив array of T
    public function GetArray: array of &T;
    
    ///Читает len элементов начиная с индекса ind из CLArray<T> как обычный массив array of T
    public function GetArray(ind, len: CommandQueue<integer>): array of &T;
    
    {$endregion Get}
    
  end;
  
  {$endregion CLArray}
  
  {$region Common}
  
  ///Представляет платформу OpenCL, объединяющую одно или несколько устройств
  Platform = partial class
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_platform_id read ntv;
    
    private prop: PlatformProperties;
    private function GetProperties: PlatformProperties;
    begin
      if prop=nil then prop := new PlatformProperties(ntv);
      Result := prop;
    end;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: PlatformProperties read GetProperties;
    
    public static function operator=(wr1, wr2: Platform): boolean := wr1.ntv = wr2.ntv;
    public static function operator<>(wr1, wr2: Platform): boolean := wr1.ntv <> wr2.ntv;
    
    ///--
    public function Equals(obj: object): boolean; override :=
    (obj is Platform(var wr)) and (self = wr);
    
  end;
  
  ///Представляет устройство, поддерживающее OpenCL
  Device = partial class
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_device_id read ntv;
    
    private prop: DeviceProperties;
    private function GetProperties: DeviceProperties;
    begin
      if prop=nil then prop := new DeviceProperties(ntv);
      Result := prop;
    end;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: DeviceProperties read GetProperties;
    
    public static function operator=(wr1, wr2: Device): boolean := wr1.ntv = wr2.ntv;
    public static function operator<>(wr1, wr2: Device): boolean := wr1.ntv <> wr2.ntv;
    
    ///--
    public function Equals(obj: object): boolean; override :=
    (obj is Device(var wr)) and (self = wr);
    
  end;
  
  ///Представляет контекст для хранения данных и выполнения команд на GPU
  Context = partial class
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_context read ntv;
    
    private prop: ContextProperties;
    private function GetProperties: ContextProperties;
    begin
      if prop=nil then prop := new ContextProperties(ntv);
      Result := prop;
    end;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: ContextProperties read GetProperties;
    
    public static function operator=(wr1, wr2: Context): boolean := wr1.ntv = wr2.ntv;
    public static function operator<>(wr1, wr2: Context): boolean := wr1.ntv <> wr2.ntv;
    
    ///--
    public function Equals(obj: object): boolean; override :=
    (obj is Context(var wr)) and (self = wr);
    
  end;
  
  ///Представляет контейнер с откомпилированным кодом для GPU, содержащим подпрограммы-kernel'ы
  ProgramCode = partial class
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_program read ntv;
    
    private prop: ProgramCodeProperties;
    private function GetProperties: ProgramCodeProperties;
    begin
      if prop=nil then prop := new ProgramCodeProperties(ntv);
      Result := prop;
    end;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: ProgramCodeProperties read GetProperties;
    
    public static function operator=(wr1, wr2: ProgramCode): boolean := wr1.ntv = wr2.ntv;
    public static function operator<>(wr1, wr2: ProgramCode): boolean := wr1.ntv <> wr2.ntv;
    
    ///--
    public function Equals(obj: object): boolean; override :=
    (obj is ProgramCode(var wr)) and (self = wr);
    
  end;
  
  ///Представляет подпрограмму, выполняемую на GPU
  Kernel = partial class
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_kernel read ntv;
    
    private prop: KernelProperties;
    private function GetProperties: KernelProperties;
    begin
      if prop=nil then prop := new KernelProperties(ntv);
      Result := prop;
    end;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: KernelProperties read GetProperties;
    
    public static function operator=(wr1, wr2: Kernel): boolean := wr1.ntv = wr2.ntv;
    public static function operator<>(wr1, wr2: Kernel): boolean := wr1.ntv <> wr2.ntv;
    
    ///--
    public function Equals(obj: object): boolean; override :=
    (obj is Kernel(var wr)) and (self = wr);
    
  end;
  
  ///Представляет область памяти устройства OpenCL (обычно GPU)
  MemorySegment = partial class
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_mem read ntv;
    
    private prop: MemorySegmentProperties;
    private function GetProperties: MemorySegmentProperties;
    begin
      if prop=nil then prop := new MemorySegmentProperties(ntv);
      Result := prop;
    end;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: MemorySegmentProperties read GetProperties;
    
    public static function operator=(wr1, wr2: MemorySegment): boolean := wr1.ntv = wr2.ntv;
    public static function operator<>(wr1, wr2: MemorySegment): boolean := wr1.ntv <> wr2.ntv;
    
    ///--
    public function Equals(obj: object): boolean; override :=
    (obj is MemorySegment(var wr)) and (self = wr);
    
  end;
  
  ///Представляет виртуальную область памяти, выделенную внутри MemorySegment
  MemorySubSegment = partial class(MemorySegment)
    
    private prop: MemorySubSegmentProperties;
    private function GetProperties: MemorySubSegmentProperties;
    begin
      if prop=nil then prop := new MemorySubSegmentProperties(ntv);
      Result := prop;
    end;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: MemorySubSegmentProperties read GetProperties;
    
  end;
  
  ///Представляет массив записей, содержимое которого хранится на устройстве OpenCL (обычно GPU)
  CLArray<T> = partial class
    
    ///Возвращает имя (дескриптор) неуправляемого объекта
    public property Native: cl_mem read ntv;
    
    private prop: CLArrayProperties;
    private function GetProperties: CLArrayProperties;
    begin
      if prop=nil then prop := new CLArrayProperties(ntv);
      Result := prop;
    end;
    ///Возвращает контейнер свойств неуправляемого объекта
    public property Properties: CLArrayProperties read GetProperties;
    
    public static function operator=(wr1, wr2: CLArray<T>): boolean := wr1.ntv = wr2.ntv;
    public static function operator<>(wr1, wr2: CLArray<T>): boolean := wr1.ntv <> wr2.ntv;
    
    ///--
    public function Equals(obj: object): boolean; override :=
    (obj is CLArray<T>(var wr)) and (self = wr);
    
  end;
  
  {$endregion Common}
  
  {$region Misc}
  
  ///Представляет устройство, поддерживающее OpenCL
  Device = partial class
    
    private supported_split_modes: array of DevicePartitionProperty := nil;
    private function GetSSM: array of DevicePartitionProperty;
    begin
      if supported_split_modes=nil then supported_split_modes := Properties.PartitionType;
      Result := supported_split_modes;
    end;
    
    private function Split(params props: array of DevicePartitionProperty): array of SubDevice;
    begin
      if not GetSSM.Contains(props[0]) then
        raise new NotSupportedException($'Данный режим .Split не поддерживается выбранным устройством');
      
      var c: UInt32;
      OpenCLABCInternalException.RaiseIfError( cl.CreateSubDevices(self.ntv, props, 0, IntPtr.Zero, c) );
      
      var res := new cl_device_id[int64(c)];
      OpenCLABCInternalException.RaiseIfError( cl.CreateSubDevices(self.ntv, props, c, res[0], IntPtr.Zero) );
      
      Result := res.ConvertAll(sdvc->new SubDevice(self.ntv, sdvc));
    end;
    
    ///Указывает, поддерживает ли это устройство вызов метода .SplitEqually
    public property CanSplitEqually: boolean read DevicePartitionProperty.DEVICE_PARTITION_EQUALLY in GetSSM;
    ///Создаёт максимальное возможное количество виртуальных устройств,
    ///каждое из которых содержит CUCount ядер данного устройства
    public function SplitEqually(CUCount: integer): array of SubDevice;
    begin
      if CUCount <= 0 then raise new ArgumentException($'Количество ядер должно быть положительным числом, а не {CUCount}');
      Result := Split(
        DevicePartitionProperty.DEVICE_PARTITION_EQUALLY,
        DevicePartitionProperty.Create(CUCount),
        DevicePartitionProperty.Create(0)
      );
    end;
    
    ///Указывает, поддерживает ли это устройство вызов метода .SplitByCounts
    public property CanSplitByCounts: boolean read DevicePartitionProperty.DEVICE_PARTITION_BY_COUNTS in GetSSM;
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
    
    ///Указывает, поддерживает ли это устройство вызов метода .SplitByAffinityDomain
    public property CanSplitByAffinityDomain: boolean read DevicePartitionProperty.DEVICE_PARTITION_BY_AFFINITY_DOMAIN in GetSSM;
    ///Разделяет данное устройство на отдельные группы ядер так,
    ///чтобы у каждой группы ядер был общий кэш указанного уровня
    public function SplitByAffinityDomain(affinity_domain: DeviceAffinityDomain) :=
    Split(
      DevicePartitionProperty.DEVICE_PARTITION_BY_AFFINITY_DOMAIN,
      DevicePartitionProperty.Create(new IntPtr(affinity_domain.val)),
      DevicePartitionProperty.Create(0)
    );
    
  end;
  
  {$endregion Misc}
  
  {$endregion Wrappers}
  
  {$region CommandQueue}
  
  {$region ToString}
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    private static function DisplayNameForType(t: System.Type): string;
    begin
      Result := t.Name;
      
      if t.IsGenericType then
      begin
        var ind := Result.IndexOf('`');
        Result := Result.Remove(ind) + '<' + t.GenericTypeArguments.JoinToString(', ') + '>';
      end;
      
    end;
    private function DisplayName: string; virtual := DisplayNameForType(self.GetType);
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); abstract;
    
    private static function GetValueRuntimeType<T>(val: T) :=
    if typeof(T).IsValueType then
      typeof(T) else
    if val = default(T) then
      nil else val.GetType;
    
    private function ToStringHeader(sb: StringBuilder; index: Dictionary<object,integer>): boolean;
    begin
      sb += DisplayName;
      
      var ind: integer;
      Result := not index.TryGetValue(self, ind);
      
      if Result then
      begin
        ind := index.Count;
        index[self] := ind;
      end;
      
      sb += '#';
      sb.Append(ind);
      
    end;
    private static procedure ToStringWriteDelegate(sb: StringBuilder; d: System.Delegate);
    begin
      if d.Target<>nil then
      begin
        sb += d.Target.ToString;
        sb += ' => ';
      end;
      sb += d.Method.ToString;
    end;
    private procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>; write_tabs: boolean := true);
    begin
      delayed.Remove(self);
      
      if write_tabs then sb.Append(#9, tabs);
      ToStringHeader(sb, index);
      ToStringImpl(sb, tabs+1, index, delayed);
      
      if tabs=0 then foreach var q in delayed do
      begin
        sb += #10;
        q.ToString(sb, 0, index, new HashSet<CommandQueueBase>);
      end;
      
    end;
    
    ///Возвращает строковое представление данного объекта
    ///Используйте это значение только для отладки, потому что данный метод не оптимизирован
    public function ToString: string; override;
    begin
      var sb := new StringBuilder;
      ToString(sb, 0, new Dictionary<object, integer>, new HashSet<CommandQueueBase>);
      Result := sb.ToString;
    end;
    
    ///Вызывает Write(ToString) для данного объекта и возвращает его же
    public function Print: CommandQueueBase;
    begin
      Write(self.ToString);
      Result := self;
    end;
    ///Вызывает Writeln(ToString) для данного объекта и возвращает его же
    public function Println: CommandQueueBase;
    begin
      Writeln(self.ToString);
      Result := self;
    end;
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает nil
  CommandQueueNil = abstract partial class(CommandQueueBase)
    
    ///Вызывает Write(ToString) для данного объекта и возвращает его же
    public function Print: CommandQueueNil;
    begin
      inherited Print;
      Result := self;
    end;
    ///Вызывает Writeln(ToString) для данного объекта и возвращает его же
    public function Println: CommandQueueNil;
    begin
      inherited Println;
      Result := self;
    end;
    
  end;
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает значение типа T
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    ///Вызывает Write(ToString) для данного объекта и возвращает его же
    public function Print: CommandQueue<T>;
    begin
      inherited Print;
      Result := self;
    end;
    ///Вызывает Writeln(ToString) для данного объекта и возвращает его же
    public function Println: CommandQueue<T>;
    begin
      inherited Println;
      Result := self;
    end;
    
  end;
  
  {$endregion ToString}
  
  {$region Use/Convert Typed}
  
  ///Представляет интерфейс типа, содержащего отдельные алгоритмы обработки, очереди без- и с возвращаемым значением
  ITypedCQUser = interface
    
    ///Вызывается если у очереди нет возвращаемого значения
    procedure UseNil(cq: CommandQueueNil);
    ///Вызывается если у очереди есть возвращаемое значение
    procedure Use<T>(cq: CommandQueue<T>);
    
  end;
  ///Представляет интерфейс типа, содержащего отдельные алгоритмы обработки, очереди без- и с возвращаемым значением
  ITypedCQConverter<TRes> = interface
    
    ///Вызывается если у очереди нет возвращаемого значения
    function ConvertNil(cq: CommandQueueNil): TRes;
    ///Вызывается если у очереди есть возвращаемое значение
    function Convert<T>(cq: CommandQueue<T>): TRes;
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    ///Проверяет, какой тип результата у данной очереди
    ///Передаёт результат указанному объекту
    public procedure UseTyped(user: ITypedCQUser); abstract;
    ///Проверяет, какой тип результата у данной очереди
    ///Передаёт результат указанному объекту
    public function ConvertTyped<TRes>(converter: ITypedCQConverter<TRes>): TRes; abstract;
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает nil
  CommandQueueNil = abstract partial class(CommandQueueBase)
    
    ///--
    public procedure UseTyped(user: ITypedCQUser); override := user.UseNil(self);
    ///--
    public function ConvertTyped<TRes>(converter: ITypedCQConverter<TRes>): TRes; override := converter.ConvertNil(self);
    
  end;
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает значение типа T
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    ///--
    public procedure UseTyped(user: ITypedCQUser); override := user.Use(self);
    ///--
    public function ConvertTyped<TRes>(converter: ITypedCQConverter<TRes>): TRes; override := converter.Convert(self);
    
  end;
  
  {$endregion Use/Convert Typed}
  
  {$region Const}
  
  ///Представляет константную очередь
  ///Константные очереди ничего не выполняют и возвращает заданное при создании значение
  ConstQueue<T> = sealed partial class(CommandQueue<T>)
    private res: T;
    
    ///Создаёт новую константную очередь из заданного значения
    public constructor(o: T) := self.res := o;
    private constructor := raise new OpenCLABCInternalException;
    
    ///Возвращает значение из которого была создана данная константная очередь
    public property Val: T read self.res;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ' ';
      var rt := GetValueRuntimeType(res);
      if typeof(T) <> rt then
        sb.Append(rt);
      sb += '{ ';
      if rt<>nil then
        sb.Append(Val) else
        sb += 'nil';
      sb += ' }'#10;
    end;
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает nil
  CommandQueueNil = abstract partial class(CommandQueueBase) end;
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает значение типа T
  CommandQueue<T> = abstract partial class
    
    public static function operator implicit(o: T): CommandQueue<T> :=
    new ConstQueue<T>(o);
    
  end;
  
  {$endregion Const}
  
  {$region Cast}
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    ///Если данная очередь проходит по условию "... is CommandQueue<T>" - возвращает себя же
    ///Иначе возвращает очередь-обёртку, выполняющую "res := T(res)", где res - результат данной очереди
    public function Cast<T>: CommandQueue<T>;
    
  end;
  
  {$endregion Cast}
  
  {$region ThenConvert}
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  CommandQueueBase = abstract partial class
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает nil
  CommandQueueNil = abstract partial class(CommandQueueBase) end;
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает значение типа T
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".ThenQuick..."
    public function ThenConvert<TOtp>(f: T->TOtp): CommandQueue<TOtp>;
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди и контекст выполнения
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".ThenQuick..."
    public function ThenConvert<TOtp>(f: (T, Context)->TOtp): CommandQueue<TOtp>;
    
    ///Создаёт очередь, которая выполнит данную и вернёт её результат
    ///Но перед этим выполнит на CPU процедуру p, используя полученный результат
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".ThenQuick..."
    public function ThenUse(p: T->()           ): CommandQueue<T>;
    ///Создаёт очередь, которая выполнит данную и вернёт её результат
    ///Но перед этим выполнит на CPU процедуру p, используя полученный результат и контекст выполнения
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".ThenQuick..."
    public function ThenUse(p: (T, Context)->()): CommandQueue<T>;
    
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди
    ///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации функции "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickConvert<TOtp>(f: T->TOtp): CommandQueue<TOtp>;
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди и контекст выполнения
    ///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации функции "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickConvert<TOtp>(f: (T, Context)->TOtp): CommandQueue<TOtp>;
    
    ///Создаёт очередь, которая выполнит данную и вернёт её результат
    ///Но перед этим выполнит на CPU процедуру p, используя полученный результат
    ///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации функции "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickUse(p: T->()           ): CommandQueue<T>;
    ///Создаёт очередь, которая выполнит данную и вернёт её результат
    ///Но перед этим выполнит на CPU процедуру p, используя полученный результат и контекст выполнения
    ///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации функции "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickUse(p: (T, Context)->()): CommandQueue<T>;
    
  end;
  
  {$endregion ThenConvert}
  
  {$region +/*}
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    private function  AfterQueueSyncBase(q: CommandQueueBase): CommandQueueBase; abstract;
    private function AfterQueueAsyncBase(q: CommandQueueBase): CommandQueueBase; abstract;
    
    public static function operator+(q1, q2: CommandQueueBase): CommandQueueBase := q2.AfterQueueSyncBase(q1);
    public static function operator*(q1, q2: CommandQueueBase): CommandQueueBase := q2.AfterQueueAsyncBase(q1);
    
    public static procedure operator+=(var q1: CommandQueueBase; q2: CommandQueueBase) := q1 := q1+q2;
    public static procedure operator*=(var q1: CommandQueueBase; q2: CommandQueueBase) := q1 := q1*q2;
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает nil
  CommandQueueNil = abstract partial class(CommandQueueBase)
    
    private function  AfterQueueSyncBase(q: CommandQueueBase): CommandQueueBase; override := q+self;
    private function AfterQueueAsyncBase(q: CommandQueueBase): CommandQueueBase; override := q*self;
    
    public static function operator+(q1: CommandQueueBase; q2: CommandQueueNil): CommandQueueNil;
    public static function operator*(q1: CommandQueueBase; q2: CommandQueueNil): CommandQueueNil;
    
    public static procedure operator+=(var q1: CommandQueueNil; q2: CommandQueueNil) := q1 := q1+q2;
    public static procedure operator*=(var q1: CommandQueueNil; q2: CommandQueueNil) := q1 := q1*q2;
    
  end;
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает значение типа T
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    private function AfterQueueSyncBase (q: CommandQueueBase): CommandQueueBase; override := q+self;
    private function AfterQueueAsyncBase(q: CommandQueueBase): CommandQueueBase; override := q*self;
    
    public static function operator+(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T>;
    public static function operator*(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T>;
    
    public static procedure operator+=(var q1: CommandQueue<T>; q2: CommandQueue<T>) := q1 := q1+q2;
    public static procedure operator*=(var q1: CommandQueue<T>; q2: CommandQueue<T>) := q1 := q1*q2;
    
  end;
  
  {$endregion +/*}
  
  {$region Multiusable}
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    private function MultiusableBase: ()->CommandQueueBase; abstract;
    ///Создаёт функцию, вызывая которую можно создать любое кол-во очередей-удлинителей для данной очереди
    ///Подробнее в справке: "Очередь>>Создание очередей>>Множественное использование очереди"
    public function Multiusable := MultiusableBase;
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает nil
  CommandQueueNil = abstract partial class(CommandQueueBase)
    
    private function MultiusableBase: ()->CommandQueueBase; override := Multiusable() as object as Func<CommandQueueBase>; //TODO #2221
    ///Создаёт функцию, вызывая которую можно создать любое кол-во очередей-удлинителей для данной очереди
    ///Подробнее в справке: "Очередь>>Создание очередей>>Множественное использование очереди"
    public function Multiusable: ()->CommandQueueNil;
    
  end;
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает значение типа T
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    private function MultiusableBase: ()->CommandQueueBase; override := Multiusable() as object as Func<CommandQueueBase>; //TODO #2221
    ///Создаёт функцию, вызывая которую можно создать любое кол-во очередей-удлинителей для данной очереди
    ///Подробнее в справке: "Очередь>>Создание очередей>>Множественное использование очереди"
    public function Multiusable: ()->CommandQueue<T>;
    
  end;
  
  {$endregion Multiusable}
  
  {$region Finally+Handle}
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    private function AfterTry(try_do: CommandQueueBase): CommandQueueBase; abstract;
    public static function operator>=(try_do, do_finally: CommandQueueBase) := do_finally.AfterTry(try_do);
    
    private function ConvertErrHandler<TException>(handler: TException->boolean): Exception->boolean; where TException: Exception;
    begin Result := e->(e is TException) and handler(TException(e)) end;
    
    ///Создаёт очередь, сначала выполняющую данную, а затем обрабатывающую кинутые в ней исключения
    ///Созданная очередь возвращает nil не зависимо от исключений при выполнении данной очереди
    public function HandleWithoutRes<TException>(handler: TException->boolean): CommandQueueNil; where TException: Exception;
    begin Result := HandleWithoutRes(ConvertErrHandler(handler)) end;
    ///Создаёт очередь, сначала выполняющую данную, а затем обрабатывающую кинутые в ней исключения
    ///Созданная очередь возвращает nil не зависимо от исключений при выполнении данной очереди
    public function HandleWithoutRes(handler: Exception->boolean): CommandQueueNil;
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает nil
  CommandQueueNil = abstract partial class(CommandQueueBase)
    
    private function AfterTry(try_do: CommandQueueBase): CommandQueueBase; override := try_do >= self;
    public static function operator>=(try_do: CommandQueueBase; do_finally: CommandQueueNil): CommandQueueNil;
    
  end;
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает значение типа T
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    private function AfterTry(try_do: CommandQueueBase): CommandQueueBase; override := try_do >= self;
    public static function operator>=(try_do: CommandQueueBase; do_finally: CommandQueue<T>): CommandQueue<T>;
    
    ///Создаёт очередь, сначала выполняющую данную, а затем обрабатывающую кинутые в ней исключения
    ///В конце выполнения созданная очередь возвращает то, что вернула данная, если исключений небыло и указанное значение если обработчик был успешно выполнен
    public function HandleDefaultRes<TException>(handler: TException->boolean; def: T): CommandQueue<T>; where TException: Exception;
    begin Result := HandleDefaultRes(ConvertErrHandler(handler), def) end;
    ///Создаёт очередь, сначала выполняющую данную, а затем обрабатывающую кинутые в ней исключения
    ///В конце выполнения созданная очередь возвращает то, что вернула данная, если исключений небыло и указанное значение если обработчик был успешно выполнен
    public function HandleDefaultRes(handler: Exception->boolean; def: T): CommandQueue<T>;
    
    ///Создаёт очередь, сначала выполняющую данную, а затем обрабатывающую кинутые в ней исключения
    ///Для того чтоб пометить исключение обработанным - его надо удалить из полученного списка
    ///Возвращаемое значение обработчика указывает на что надо заменить возвращаемое значение данной очереди, если обработчик был успешно выполнен
    public function HandleReplaceRes(handler: List<Exception> -> T): CommandQueue<T>;
    
  end;
  
  {$endregion Finally+Handle}
  
  {$region Wait}
  
  ///Представляет маркер для Wait очередей
  ///Данный тип не является очередью
  ///Но при выполнении преобразуется в очередь, выполняющую .SendSignal исходного маркера
  WaitMarker = abstract partial class
    
    ///Создаёт новый простой маркер
    public static function Create: WaitMarker;
    
    ///Посылает сигнал выполненности всем ожидающим Wait очередям
    public procedure SendSignal; abstract;
    
    public static function operator and(m1, m2: WaitMarker): WaitMarker;
    public static function operator or(m1, m2: WaitMarker): WaitMarker;
    
    private function ConvertToQBase: CommandQueueBase; abstract;
    public static function operator implicit(m: WaitMarker): CommandQueueBase := m.ConvertToQBase;
    
    {$region ToString}
    
    private function ToStringHeader(sb: StringBuilder; index: Dictionary<object,integer>): boolean;
    begin
      sb += DisplayName;
      
      var ind: integer;
      Result := not index.TryGetValue(self, ind);
      
      if Result then
      begin
        ind := index.Count;
        index[self] := ind;
      end;
      
      sb += '#';
      sb.Append(ind);
      
    end;
    private function DisplayName: string; virtual := CommandQueueBase.DisplayNameForType(self.GetType);
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); abstract;
    
    private procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>; write_tabs: boolean := true);
    begin
      if write_tabs then sb.Append(#9, tabs);
      ToStringHeader(sb, index);
      ToStringImpl(sb, tabs+1, index, delayed);
      
      if tabs=0 then foreach var q in delayed do
      begin
        sb += #10;
        q.ToString(sb, 0, index, new HashSet<CommandQueueBase>);
      end;
      
    end;
    
    ///Возвращает строковое представление данного объекта
    ///Используйте это значение только для отладки, потому что данный метод не оптимизирован
    public function ToString: string; override;
    begin
      var sb := new StringBuilder;
      ToString(sb, 0, new Dictionary<object, integer>, new HashSet<CommandQueueBase>);
      Result := sb.ToString;
    end;
    
    ///Вызывает Write(ToString) для данного объекта и возвращает его же
    public function Print: WaitMarker;
    begin
      Write(self.ToString);
      Result := self;
    end;
    ///Вызывает Writeln(ToString) для данного объекта и возвращает его же
    public function Println: WaitMarker;
    begin
      Writeln(self.ToString);
      Result := self;
    end;
    
    {$endregion ToString}
    
  end;
  
  ///Представляет оторванный сигнал маркера, являющийся обёрткой очереди без возвращаемого значения
  ///Данный тип не является маркером, но преобразуется в него при передаче в Wait-очереди
  DetachedMarkerSignalNil = sealed partial class
    
    private function get_signal_in_finally: boolean;
    ///Указывает, будут ли проигнорированы ошибки выполнения исходной очереди при автоматическом вызове .SendSignal
    public property SignalInFinally: boolean read get_signal_in_finally;
    
    ///Создаёт новый оторванный сигнал маркера
    ///При выполнении сначала будет выполнена очередь q, а затем метод .SendSignal
    ///signal_in_finally указывает, будут ли проигнорированы ошибки выполнения q при автоматическом вызове .SendSignal
    public constructor(q: CommandQueueNil; signal_in_finally: boolean);
    private constructor := raise new OpenCLABCInternalException;
    
    public static function operator implicit(dms: DetachedMarkerSignalNil): WaitMarker;
    
    ///Посылает сигнал выполненности всем ожидающим Wait очередям
    public procedure SendSignal := WaitMarker(self).SendSignal;
    public static function operator and(m1, m2: DetachedMarkerSignalNil) := WaitMarker(m1) and WaitMarker(m2);
    public static function operator or(m1, m2: DetachedMarkerSignalNil) := WaitMarker(m1) or WaitMarker(m2);
    
  end;
  ///Представляет оторванный сигнал маркера, являющийся обёрткой очереди с возвращаемым значением
  ///Данный тип не является маркером, но преобразуется в него при передаче в Wait-очереди
  DetachedMarkerSignal<T> = sealed partial class
    
    private function get_signal_in_finally: boolean;
    ///Указывает, будут ли проигнорированы ошибки выполнения исходной очереди при автоматическом вызове .SendSignal
    public property SignalInFinally: boolean read get_signal_in_finally;
    
    ///Создаёт новый оторванный сигнал маркера
    ///При выполнении сначала будет выполнена очередь q, а затем метод .SendSignal
    ///signal_in_finally указывает, будут ли проигнорированы ошибки выполнения q при автоматическом вызове .SendSignal
    public constructor(q: CommandQueue<T>; signal_in_finally: boolean);
    private constructor := raise new OpenCLABCInternalException;
    
    public static function operator implicit(dms: DetachedMarkerSignal<T>): WaitMarker;
    
    ///Посылает сигнал выполненности всем ожидающим Wait очередям
    public procedure SendSignal := WaitMarker(self).SendSignal;
    public static function operator and(m1, m2: DetachedMarkerSignal<T>) := WaitMarker(m1) and WaitMarker(m2);
    public static function operator or(m1, m2: DetachedMarkerSignal<T>) := WaitMarker(m1) or WaitMarker(m2);
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  CommandQueueBase = abstract partial class end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает nil
  CommandQueueNil = abstract partial class(CommandQueueBase)
    
    ///Создаёт очередь, сначала выполняющую данную, а затем вызывающую свой .SendSignal
    ///При передаче в Wait-очереди, полученная очередь превращается в маркер
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenMarkerSignal := new DetachedMarkerSignalNil(self, false);
    ///Создаёт очередь, сначала выполняющую данную, а затем вызывающую свой .SendSignal не зависимо от исключений при выполнении данной очереди
    ///При передаче в Wait-очереди, полученная очередь превращается в маркер
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenFinallyMarkerSignal := new DetachedMarkerSignalNil(self, true);
    
  end;
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает значение типа T
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    ///Создаёт очередь, сначала выполняющую данную, а затем вызывающую свой .SendSignal
    ///При передаче в Wait-очереди, полученная очередь превращается в маркер
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenMarkerSignal := new DetachedMarkerSignal<T>(self, false);
    ///Создаёт очередь, сначала выполняющую данную, а затем вызывающую свой .SendSignal не зависимо от исключений при выполнении данной очереди
    ///При передаче в Wait-очереди, полученная очередь превращается в маркер
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenFinallyMarkerSignal := new DetachedMarkerSignal<T>(self, true);
    
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала от указанного маркера
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenWaitFor(marker: WaitMarker): CommandQueue<T>;
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала от указанного маркера не зависимо от исключений при выполнении данной очереди
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenFinallyWaitFor(marker: WaitMarker): CommandQueue<T>;
    
  end;
  
  {$endregion Wait}
  
  {$endregion CommandQueue}
  
  {$region CLTask}
  
  ///Представляет задачу выполнения очереди, создаваемую методом Context.BeginInvoke
  CLTaskBase = abstract partial class
    private org_c: Context;
    private wh := new ManualResetEvent(false);
    private err_lst: List<Exception>;
    
    private function OrgQueueBase: CommandQueueBase; abstract;
    ///Возвращает очередь, которую выполняет данный CLTask
    public property OrgQueue: CommandQueueBase read OrgQueueBase;
    
    ///Возвращает контекст, в котором выполняется данный CLTask
    public property OrgContext: Context read org_c;
    
    ///Ожидает окончания выполнения очереди (если оно ещё не завершилось)
    ///Кидает System.AggregateException, содержащие ошибки при выполнении очереди, если такие имеются
    public procedure Wait;
    begin
      wh.WaitOne;
      if err_lst.Count=0 then exit;
      raise new AggregateException($'При выполнении очереди было вызвано {err_lst.Count} исключений. Используйте try чтоб получить больше информации', err_lst.ToArray);
    end;
    
  end;
  
  ///Представляет задачу выполнения очереди, создаваемую методом Context.BeginInvoke
  CLTaskNil = sealed partial class(CLTaskBase)
    private q: CommandQueueNil;
    
    private constructor := raise new OpenCLABCInternalException;
    
    ///Возвращает очередь, которую выполняет данный CLTask
    public property OrgQueue: CommandQueueNil read q; reintroduce;
    private function OrgQueueBase: CommandQueueBase; override := self.OrgQueue;
    
  end;
  
  ///Представляет задачу выполнения очереди, создаваемую методом Context.BeginInvoke
  CLTask<T> = sealed partial class(CLTaskBase)
    private q: CommandQueue<T>;
    
    private constructor := raise new OpenCLABCInternalException;
    
    ///Возвращает очередь, которую выполняет данный CLTask
    public property OrgQueue: CommandQueue<T> read q; reintroduce;
    private function OrgQueueBase: CommandQueueBase; override := self.OrgQueue;
    
    ///Ожидает окончания выполнения очереди (если оно ещё не завершилось)
    ///Кидает System.AggregateException, содержащие ошибки при выполнении очереди, если такие имеются
    ///А затем возвращает результат выполнения
    public function WaitRes: T;
    
  end;
  
  ///Представляет контекст для хранения данных и выполнения команд на GPU
  Context = partial class
    
    ///Запускает данную очередь и все её подочереди
    ///Как только всё запущено: возвращает CLTask, через который можно следить за процессом выполнения
    public function BeginInvoke(q: CommandQueueBase): CLTaskBase;
    ///Запускает данную очередь и все её подочереди
    ///Как только всё запущено: возвращает CLTask, через который можно следить за процессом выполнения
    public function BeginInvoke(q: CommandQueueNil): CLTaskNil;
    ///Запускает данную очередь и все её подочереди
    ///Как только всё запущено: возвращает CLTask, через который можно следить за процессом выполнения
    public function BeginInvoke<T>(q: CommandQueue<T>): CLTask<T>;
    
    ///Запускает данную очередь и все её подочереди
    ///Затем ожидает окончания выполнения
    public procedure SyncInvoke(q: CommandQueueBase) := BeginInvoke(q).Wait;
    ///Запускает данную очередь и все её подочереди
    ///Затем ожидает окончания выполнения
    public procedure SyncInvoke(q: CommandQueueNil) := BeginInvoke(q).Wait;
    ///Запускает данную очередь и все её подочереди
    ///Затем ожидает окончания выполнения и возвращает полученный результат
    public function SyncInvoke<T>(q: CommandQueue<T>) := BeginInvoke(q).WaitRes;
    
  end;
  
  {$endregion CLTask}
  
  {$region KernelArg}
  
  ///Представляет аргумент, передаваемый в вызов kernel-а
  KernelArg = abstract partial class
    
    {$region MemorySegment}
    
    ///Создаёт аргумент kernel'а, представляющий область памяти GPU
    public static function FromMemorySegment(mem: MemorySegment): KernelArg;
    public static function operator implicit(mem: MemorySegment): KernelArg := FromMemorySegment(mem);
    
    ///Создаёт аргумент kernel'а, представляющий область памяти GPU
    public static function FromMemorySegmentCQ(mem_q: CommandQueue<MemorySegment>): KernelArg;
    public static function operator implicit(mem_q: CommandQueue<MemorySegment>): KernelArg := FromMemorySegmentCQ(mem_q);
    
    {$endregion MemorySegment}
    
    {$region CLArray}
    
    ///Создаёт аргумент kernel'а, представляющий массив данных, хранимых на GPU
    public static function FromCLArray<T>(a: CLArray<T>): KernelArg; where T: record;
    public static function operator implicit<T>(a: CLArray<T>): KernelArg; where T: record;
    begin Result := FromCLArray(a); end;
    
    ///Создаёт аргумент kernel'а, представляющий массив данных, хранимых на GPU
    public static function FromCLArrayCQ<T>(a_q: CommandQueue<CLArray<T>>): KernelArg; where T: record;
    public static function operator implicit<T>(a_q: CommandQueue<CLArray<T>>): KernelArg; where T: record;
    begin Result := FromCLArrayCQ(a_q); end;
    
    {$endregion CLArray}
    
    {$region Data}
    
    ///Создаёт аргумент kernel'а, представляющий адрес в неуправляемой памяти или на стэке
    public static function FromData(ptr: IntPtr; sz: UIntPtr): KernelArg;
    
    ///Создаёт аргумент kernel'а, представляющий адрес в неуправляемой памяти или на стэке
    public static function FromDataCQ(ptr_q: CommandQueue<IntPtr>; sz_q: CommandQueue<UIntPtr>): KernelArg;
    
    ///Создаёт аргумент kernel'а, представляющий адрес в неуправляемой памяти или на стэке
    public static function FromValueData<TRecord>(ptr: ^TRecord): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(ptr: ^TRecord): KernelArg; where TRecord: record; begin Result := FromValueData(ptr); end;
    
    {$endregion Data}
    
    {$region Value}
    
    ///Создаёт аргумент kernel'а, представляющий небольшое значение размерного типа
    public static function FromValue<TRecord>(val: TRecord): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(val: TRecord): KernelArg; where TRecord: record; begin Result := FromValue(val); end;
    
    ///Создаёт аргумент kernel'а, представляющий небольшое значение размерного типа
    public static function FromValueCQ<TRecord>(valq: CommandQueue<TRecord>): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(valq: CommandQueue<TRecord>): KernelArg; where TRecord: record; begin Result := FromValueCQ(valq); end;
    
    {$endregion Value}
    
    {$region NativeValue}
    
    ///Создаёт аргумент kernel'а, ссылающийся на неуправляемое значение
    public static function FromNativeValue<TRecord>(val: NativeValue<TRecord>): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(val: NativeValue<TRecord>): KernelArg; where TRecord: record; begin Result := FromNativeValue(val); end;
    
    ///Создаёт аргумент kernel'а, ссылающийся на неуправляемое значение
    public static function FromNativeValueCQ<TRecord>(valq: CommandQueue<NativeValue<TRecord>>): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(valq: CommandQueue<NativeValue<TRecord>>): KernelArg; where TRecord: record; begin Result := FromNativeValueCQ(valq); end;
    
    {$endregion NativeValue}
    
    {$region Array}
    
    ///Создаёт аргумент kernel'а, ссылающийся на указанный массив, на элемент с индексом ind
    public static function FromArray<TRecord>(a: array of TRecord; ind: integer := 0): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(a: array of TRecord): KernelArg; where TRecord: record; begin Result := FromArray(a); end;
    
    ///Создаёт аргумент kernel'а, ссылающийся на указанный массив, на элемент с индексом ind
    public static function FromArrayCQ<TRecord>(a_q: CommandQueue<array of TRecord>; ind_q: CommandQueue<integer> := 0): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(a_q: CommandQueue<array of TRecord>): KernelArg; where TRecord: record; begin Result := FromArrayCQ(a_q); end;
    
    {$endregion Array}
    
    {$region ToString}
    
    private function DisplayName: string; virtual := CommandQueueBase.DisplayNameForType(self.GetType);
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); abstract;
    
    private procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>; write_tabs: boolean := true);
    begin
      if write_tabs then sb.Append(#9, tabs);
      sb += DisplayName;
      
      ToStringImpl(sb, tabs+1, index, delayed);
      
      if tabs=0 then foreach var q in delayed do
      begin
        sb += #10;
        q.ToString(sb, 0, index, new HashSet<CommandQueueBase>);
      end;
      
    end;
    
    ///Возвращает строковое представление данного объекта
    ///Используйте это значение только для отладки, потому что данный метод не оптимизирован
    public function ToString: string; override;
    begin
      var sb := new StringBuilder;
      ToString(sb, 0, new Dictionary<object, integer>, new HashSet<CommandQueueBase>);
      Result := sb.ToString;
    end;
    
    ///Вызывает Write(ToString) для данного объекта и возвращает его же
    public function Print: KernelArg;
    begin
      Write(self.ToString);
      Result := self;
    end;
    ///Вызывает Writeln(ToString) для данного объекта и возвращает его же
    public function Println: KernelArg;
    begin
      Writeln(self.ToString);
      Result := self;
    end;
    
    {$endregion ToString}
    
  end;
  
  {$endregion KernelArg}
  
  {$region KernelCCQ}
  
  ///Представляет очередь-контейнер для команд GPU, применяемых к объекту типа Kernel
  KernelCCQ = sealed partial class
    
    ///Создаёт контейнер команд, который будет применять команды к указанному объекту
    public constructor(o: Kernel);
    ///Создаёт контейнер команд, который будет применять команды к объекту, который вернёт указанная очередь
    ///За каждое одно выполнение контейнера - q выполнится ровно один раз
    public constructor(q: CommandQueue<Kernel>);
    private constructor;
    
    {$region Special .Add's}
    
    ///Добавляет выполнение очереди в список обычных команд для GPU
    public function AddQueue(q: CommandQueueBase): KernelCCQ;
    
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".AddQuick..."
    public function AddProc(p: Kernel->()): KernelCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".AddQuick..."
    public function AddProc(p: (Kernel, Context)->()): KernelCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации функции "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function AddQuickProc(p: Kernel->()): KernelCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации функции "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function AddQuickProc(p: (Kernel, Context)->()): KernelCCQ;
    
    ///Добавляет ожидание сигнала выполненности от заданного маркера
    public function AddWait(marker: WaitMarker): KernelCCQ;
    
    {$endregion Special .Add's}
    
    {$region 1#Exec}
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function AddExec1(sz1: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function AddExec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function AddExec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
    
    ///Выполняет kernel с расширенным набором параметров
    ///Данная перегрузка используется в первую очередь для тонких оптимизаций
    ///Если она вам понадобилась по другой причина - пожалуйста, напишите в issue
    public function AddExec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): KernelCCQ;
    
    {$endregion 1#Exec}
    
  end;
  
  ///Представляет подпрограмму, выполняемую на GPU
  Kernel = partial class
    ///Создаёт новую очередь-контейнер для команд GPU, применяемых к данному объекту
    public function NewQueue := new KernelCCQ(self);
  end;
  
  {$endregion KernelCCQ}
  
  {$region MemorySegmentCCQ}
  
  ///Представляет очередь-контейнер для команд GPU, применяемых к объекту типа MemorySegment
  MemorySegmentCCQ = sealed partial class
    
    ///Создаёт контейнер команд, который будет применять команды к указанному объекту
    public constructor(o: MemorySegment);
    ///Создаёт контейнер команд, который будет применять команды к объекту, который вернёт указанная очередь
    ///За каждое одно выполнение контейнера - q выполнится ровно один раз
    public constructor(q: CommandQueue<MemorySegment>);
    private constructor;
    
    {$region Special .Add's}
    
    ///Добавляет выполнение очереди в список обычных команд для GPU
    public function AddQueue(q: CommandQueueBase): MemorySegmentCCQ;
    
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".AddQuick..."
    public function AddProc(p: MemorySegment->()): MemorySegmentCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".AddQuick..."
    public function AddProc(p: (MemorySegment, Context)->()): MemorySegmentCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации функции "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function AddQuickProc(p: MemorySegment->()): MemorySegmentCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации функции "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function AddQuickProc(p: (MemorySegment, Context)->()): MemorySegmentCCQ;
    
    ///Добавляет ожидание сигнала выполненности от заданного маркера
    public function AddWait(marker: WaitMarker): MemorySegmentCCQ;
    
    {$endregion Special .Add's}
    
    {$region 1#Write&Read}
    
    ///Заполняет всю область памяти данными, находящимися по указанному адресу в RAM
    public function AddWriteData(ptr: CommandQueue<IntPtr>): MemorySegmentCCQ;
    
    ///Заполняет часть области памяти данными, находящимися по указанному адресу в RAM
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function AddWriteData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Читает всё содержимое области памяти в RAM, по указанному адресу
    public function AddReadData(ptr: CommandQueue<IntPtr>): MemorySegmentCCQ;
    
    ///Читает часть содержимого области памяти в RAM, по указанному адресу
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function AddReadData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Заполняет всю область памяти данными, находящимися по указанному адресу в RAM
    public function AddWriteData(ptr: pointer): MemorySegmentCCQ;
    
    ///Заполняет часть области памяти данными, находящимися по указанному адресу в RAM
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function AddWriteData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Читает всё содержимое области памяти в RAM, по указанному адресу
    public function AddReadData(ptr: pointer): MemorySegmentCCQ;
    
    ///Читает часть содержимого области памяти в RAM, по указанному адресу
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function AddReadData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function AddWriteValue<TRecord>(val: TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function AddWriteValue<TRecord>(val: CommandQueue<TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function AddWriteValue<TRecord>(val: NativeValue<TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function AddWriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает значение размерного типа из начала области памяти в указанное значение
    public function AddReadValue<TRecord>(val: NativeValue<TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает значение размерного типа из начала области памяти в указанное значение
    public function AddReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddWriteValue<TRecord>(val: TRecord; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddWriteValue<TRecord>(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddWriteValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddWriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает значение размерного типа из области памяти в указанное значение
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddReadValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает значение размерного типа из области памяти в указанное значение
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function AddWriteArray1<TRecord>(a: array of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function AddWriteArray2<TRecord>(a: array[,] of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function AddWriteArray3<TRecord>(a: array[,,] of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function AddReadArray1<TRecord>(a: array of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function AddReadArray2<TRecord>(a: array[,] of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function AddReadArray3<TRecord>(a: array[,,] of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddWriteArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddWriteArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddWriteArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddReadArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddReadArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddReadArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function AddWriteArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function AddWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function AddWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function AddReadArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function AddReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function AddReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddWriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    {$endregion 1#Write&Read}
    
    {$region 2#Fill}
    
    ///Читает pattern_len байт из RAM по указанному адресу и заполняет их копиями всю область памяти
    public function AddFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Читает pattern_len байт из RAM по указанному адресу и заполняет их копиями часть области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Заполняет всю область памяти копиями указанного значения размерного типа
    public function AddFillValue<TRecord>(val: TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного значения размерного типа
    public function AddFillValue<TRecord>(val: CommandQueue<TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет часть области памяти копиями указанного значения размерного типа
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function AddFillValue<TRecord>(val: TRecord; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет часть области памяти копиями указанного значения размерного типа
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function AddFillValue<TRecord>(val: CommandQueue<TRecord>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного массива
    public function AddFillArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного массива
    public function AddFillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного массива
    public function AddFillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет часть области памяти копиями части указанного массива
    ///a_offset(-ы) указывают индекс в массиве
    ///pattern_len указывает кол-во задействованных элементов массива
    ///len указывает кол-во задействованных в операции байт
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddFillArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет часть области памяти копиями части указанного массива
    ///a_offset(-ы) указывают индекс в массиве
    ///pattern_len указывает кол-во задействованных элементов массива
    ///len указывает кол-во задействованных в операции байт
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddFillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет часть области памяти копиями части указанного массива
    ///a_offset(-ы) указывают индекс в массиве
    ///pattern_len указывает кол-во задействованных элементов массива
    ///len указывает кол-во задействованных в операции байт
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function AddFillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    {$endregion 2#Fill}
    
    {$region 3#Copy}
    
    ///Копирует данные из текущей области памяти в mem
    ///Если области памяти имеют разный размер - в качестве объёма данных берётся размер меньшей области
    public function AddCopyTo(mem: CommandQueue<MemorySegment>): MemorySegmentCCQ;
    
    ///Копирует данные из текущей области памяти в mem
    ///from_pos указывает отступ в байтах от начала области памяти, из которой копируют
    ///to_pos указывает отступ в байтах от начала области памяти, в которую копируют
    ///len указывает кол-во копируемых байт
    public function AddCopyTo(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Копирует данные из mem в текущюу область памяти
    ///Если области памяти имеют разный размер - в качестве объёма данных берётся размер меньшей области
    public function AddCopyFrom(mem: CommandQueue<MemorySegment>): MemorySegmentCCQ;
    
    ///Копирует данные из mem в текущюу область памяти
    ///from_pos указывает отступ в байтах от начала области памяти, из которой копируют
    ///to_pos указывает отступ в байтах от начала области памяти, в которую копируют
    ///len указывает кол-во копируемых байт
    public function AddCopyFrom(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    {$endregion 3#Copy}
    
    {$region Get}
    
    ///Читает значение указанного размерного типа из начала области памяти
    public function AddGetValue<TRecord>: CommandQueue<TRecord>; where TRecord: record;
    
    ///Читает значение указанного размерного типа из области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddGetValue<TRecord>(mem_offset: CommandQueue<integer>): CommandQueue<TRecord>; where TRecord: record;
    
    ///Читает массив максимального размера, на сколько хватит байт данной области памяти
    public function AddGetArray1<TRecord>: CommandQueue<array of TRecord>; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое данный области памяти
    public function AddGetArray1<TRecord>(len: CommandQueue<integer>): CommandQueue<array of TRecord>; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое данный области памяти
    public function AddGetArray2<TRecord>(len1,len2: CommandQueue<integer>): CommandQueue<array[,] of TRecord>; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое данный области памяти
    public function AddGetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): CommandQueue<array[,,] of TRecord>; where TRecord: record;
    
    {$endregion Get}
    
  end;
  
  ///Представляет область памяти устройства OpenCL (обычно GPU)
  MemorySegment = partial class
    ///Создаёт новую очередь-контейнер для команд GPU, применяемых к данному объекту
    public function NewQueue := new MemorySegmentCCQ(self);
  end;
  
  ///Представляет аргумент, передаваемый в вызов kernel-а
  KernelArg = abstract partial class
    public static function operator implicit(mem_q: MemorySegmentCCQ): KernelArg;
  end;
  
  {$endregion MemorySegmentCCQ}
  
  {$region CLArrayCCQ}
  
  ///Представляет очередь-контейнер для команд GPU, применяемых к объекту типа CLArray
  CLArrayCCQ<T> = sealed partial class
  where T: record;
    
    ///Создаёт контейнер команд, который будет применять команды к указанному объекту
    public constructor(o: CLArray<T>);
    ///Создаёт контейнер команд, который будет применять команды к объекту, который вернёт указанная очередь
    ///За каждое одно выполнение контейнера - q выполнится ровно один раз
    public constructor(q: CommandQueue<CLArray<T>>);
    private constructor;
    
    {$region Special .Add's}
    
    ///Добавляет выполнение очереди в список обычных команд для GPU
    public function AddQueue(q: CommandQueueBase): CLArrayCCQ<T>;
    
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".AddQuick..."
    public function AddProc(p: CLArray<T>->()): CLArrayCCQ<T>;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".AddQuick..."
    public function AddProc(p: (CLArray<T>, Context)->()): CLArrayCCQ<T>;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации функции "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function AddQuickProc(p: CLArray<T>->()): CLArrayCCQ<T>;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации функции "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function AddQuickProc(p: (CLArray<T>, Context)->()): CLArrayCCQ<T>;
    
    ///Добавляет ожидание сигнала выполненности от заданного маркера
    public function AddWait(marker: WaitMarker): CLArrayCCQ<T>;
    
    {$endregion Special .Add's}
    
    {$region 1#Write&Read}
    
    ///Заполняет весь данный объект CLArray<T> данными, находящимися по указанному адресу в RAM
    public function AddWriteData(ptr: CommandQueue<IntPtr>): CLArrayCCQ<T>;
    
    ///Заполняет len элементов начиная с индекса ind данного объекта CLArray<T> данными, находящимися по указанному адресу в RAM
    public function AddWriteData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Читает всё содержимое данного объекта CLArray<T> в RAM, по указанному адресу
    public function AddReadData(ptr: CommandQueue<IntPtr>): CLArrayCCQ<T>;
    
    ///Читает len элементов начиная с индекса ind данного объекта CLArray<T> в RAM, по указанному адресу
    public function AddReadData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Заполняет весь данный объект CLArray<T> данными, находящимися по указанному адресу в RAM
    public function AddWriteData(ptr: pointer): CLArrayCCQ<T>;
    
    ///Заполняет len элементов начиная с индекса ind данного объекта CLArray<T> данными, находящимися по указанному адресу в RAM
    public function AddWriteData(ptr: pointer; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Читает всё содержимое данного объекта CLArray<T> в RAM, по указанному адресу
    public function AddReadData(ptr: pointer): CLArrayCCQ<T>;
    
    ///Читает len элементов начиная с индекса ind данного объекта CLArray<T> в RAM, по указанному адресу
    public function AddReadData(ptr: pointer; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Записывает указанное значение по индексу ind
    public function AddWriteValue(val: &T; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Записывает указанное значение по индексу ind
    public function AddWriteValue(val: CommandQueue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Записывает указанное значение по индексу ind
    public function AddWriteValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Записывает указанное значение по индексу ind
    public function AddWriteValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Читает значение по индексу ind в указанное значение
    public function AddReadValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Читает значение по индексу ind в указанное значение
    public function AddReadValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Записывает весь указанный массив в начало данного объекта CLArray<T>
    public function AddWriteArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
    
    ///Записывает len элементов массива a, начиная с индекса a_ind, в данный объект CLArray<T> по индексу ind
    public function AddWriteArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Читает начало данного объекта CLArray<T> в указанный массив
    public function AddReadArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
    
    ///Читает len элементов данного объекта CLArray<T>, начиная с индекса ind, в массив a по индексу a_ind
    public function AddReadArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    {$endregion 1#Write&Read}
    
    {$region 2#Fill}
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями весь данный объект CLArray<T>
    public function AddFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями len элементов начиная с индекса ind данного объекта CLArray<T>
    public function AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями весь данный объект CLArray<T>
    public function AddFillData(ptr: pointer; pattern_len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями len элементов начиная с индекса ind данного объекта CLArray<T>
    public function AddFillData(ptr: pointer; pattern_len, ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Заполняет весь массив копиями указанного значения
    public function AddFillValue(val: &T): CLArrayCCQ<T>;
    
    ///Заполняет весь массив копиями указанного значения
    public function AddFillValue(val: CommandQueue<&T>): CLArrayCCQ<T>;
    
    ///Заполняет len элементов начиная с индекса ind копиями указанного значения
    public function AddFillValue(val: &T; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Заполняет len элементов начиная с индекса ind копиями указанного значения
    public function AddFillValue(val: CommandQueue<&T>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Заполняет данный объект CLArray<T> компиями указанного массива
    public function AddFillArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
    
    ///Заполняет len элементов начиная с индекса ind компиями части указанного массива
    ///Из указанного массива берётся pattern_len элементов, начиная с индекса a_offset
    public function AddFillArray(a: CommandQueue<array of &T>; a_offset,pattern_len, ind,len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    {$endregion 2#Fill}
    
    {$region 3#Copy}
    
    ///Копирует данные из текущего массива в a
    ///Если у массивов разный размер - копируется кол-во элементов меньшего массива
    public function AddCopyTo(a: CommandQueue<CLArray<T>>): CLArrayCCQ<T>;
    
    ///Копирует данные из текущего массива в a
    ///from_ind указывает индекс в массиве, из которого копируют
    ///to_ind указывает индекс в массиве, в который копируют
    ///len указывает кол-во копируемых элементов
    public function AddCopyTo(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Копирует данные из a в текущий массив
    ///Если у массивов разный размер - копируется кол-во элементов меньшего массива
    public function AddCopyFrom(a: CommandQueue<CLArray<T>>): CLArrayCCQ<T>;
    
    ///Копирует данные из a в текущий массив
    ///from_ind указывает индекс в массиве, из которого копируют
    ///to_ind указывает индекс в массиве, в который копируют
    ///len указывает кол-во копируемых элементов
    public function AddCopyFrom(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    {$endregion 3#Copy}
    
    {$region Get}
    
    ///Читает элемент по указанному индексу
    public function AddGetValue(ind: CommandQueue<integer>): CommandQueue<&T>;
    
    ///Читает весь CLArray<T> как обычный массив array of T
    public function AddGetArray: CommandQueue<array of &T>;
    
    ///Читает len элементов начиная с индекса ind из CLArray<T> как обычный массив array of T
    public function AddGetArray(ind, len: CommandQueue<integer>): CommandQueue<array of &T>;
    
    {$endregion Get}
    
  end;
  
  ///Представляет массив записей, содержимое которого хранится на устройстве OpenCL (обычно GPU)
  CLArray<T> = partial class
    ///Создаёт новую очередь-контейнер для команд GPU, применяемых к данному объекту
    public function NewQueue := new CLArrayCCQ<T>(self);
  end;
  
  ///Представляет аргумент, передаваемый в вызов kernel-а
  KernelArg = abstract partial class
    public static function operator implicit<T>(a_q: CLArrayCCQ<T>): KernelArg; where T: record;
  end;
  
  {$endregion CLArrayCCQ}
  
{$region Global subprograms}

{$region HFQ/HPQ}

///Создаёт очередь, выполняющую указанную функцию на CPU
///И возвращающую результат этой функци
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, с дополнительным "Q" (что значит Quick) в конце
function HFQ<T>(f: ()->T): CommandQueue<T>;
///Создаёт очередь, выполняющую указанную функцию на CPU
///И возвращающую результат этой функци
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, с дополнительным "Q" (что значит Quick) в конце
function HFQ<T>(f: Context->T): CommandQueue<T>;
///Создаёт очередь, выполняющую указанную функцию на CPU
///И возвращающую результат этой функци
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function HFQQ<T>(f: ()->T): CommandQueue<T>;
///Создаёт очередь, выполняющую указанную функцию на CPU
///И возвращающую результат этой функци
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function HFQQ<T>(f: Context->T): CommandQueue<T>;

///Создаёт очередь, выполняющую указанную процедуру на CPU
///И возвращающую nil
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, с дополнительным "Q" (что значит Quick) в конце
function HPQ(p: ()->()): CommandQueueNil;
///Создаёт очередь, выполняющую указанную процедуру на CPU
///И возвращающую nil
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, с дополнительным "Q" (что значит Quick) в конце
function HPQ(p: Context->()): CommandQueueNil;
///Создаёт очередь, выполняющую указанную процедуру на CPU
///И возвращающую nil
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function HPQQ(p: ()->()): CommandQueueNil;
///Создаёт очередь, выполняющую указанную процедуру на CPU
///И возвращающую nil
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function HPQQ(p: Context->()): CommandQueueNil;

{$endregion HFQ/HPQ}

{$region Wait}

///Создаёт маркер-комбинацию, который активируется когда активированы каждых из указанных маркеров
function WaitAll(params sub_markers: array of WaitMarker): WaitMarker;
///Создаёт маркер-комбинацию, который активируется когда активированы каждых из указанных маркеров
function WaitAll(sub_markers: sequence of WaitMarker): WaitMarker;

///Создаёт маркер-комбинацию, который активируется когда активированы любого из указанных маркеров
function WaitAny(params sub_markers: array of WaitMarker): WaitMarker;
///Создаёт маркер-комбинацию, который активируется когда активированы любого из указанных маркеров
function WaitAny(sub_markers: sequence of WaitMarker): WaitMarker;

///Создаёт очередь, ожидающую сигнала выполненности от заданного маркера
function WaitFor(marker: WaitMarker): CommandQueueNil;

{$endregion Wait}

{$region CombineQueue's}

{$region Sync}

{$region NonConv}

///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueueBase(params qs: array of CommandQueueBase): CommandQueueBase;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueueBase(qs: sequence of CommandQueueBase): CommandQueueBase;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueueNil(params qs: array of CommandQueueNil): CommandQueueNil;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueueNil(qs: sequence of CommandQueueNil): CommandQueueNil;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueue<T>(params qs: array of CommandQueue<T>): CommandQueue<T>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueue<T>(qs: sequence of CommandQueue<T>): CommandQueue<T>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueueNil(qs: sequence of CommandQueueBase; last: CommandQueueNil): CommandQueueNil;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>): CommandQueue<T>;

{$endregion NonConv}

{$region Conv}

{$region NonContext}

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

{$endregion NonContext}

{$region Context}

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvSyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

{$endregion Context}

{$endregion Conv}

{$endregion Sync}

{$region Async}

{$region NonConv}

///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueueBase(params qs: array of CommandQueueBase): CommandQueueBase;
///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueueBase(qs: sequence of CommandQueueBase): CommandQueueBase;

///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueueNil(params qs: array of CommandQueueNil): CommandQueueNil;
///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueueNil(qs: sequence of CommandQueueNil): CommandQueueNil;

///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueue<T>(params qs: array of CommandQueue<T>): CommandQueue<T>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueue<T>(qs: sequence of CommandQueue<T>): CommandQueue<T>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueueNil(qs: sequence of CommandQueueBase; last: CommandQueueNil): CommandQueueNil;

///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>): CommandQueue<T>;

{$endregion NonConv}

{$region Conv}

{$region NonContext}

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

{$endregion NonContext}

{$region Context}

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на "CombineQuick..."
function CombineConvAsyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат выполняется в одном из уже существующих потоков выполнения и как можно позже, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, как "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации функции "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

{$endregion Context}

{$endregion Conv}

{$endregion Async}

{$endregion CombineQueue's}

{$endregion Global subprograms}

implementation

{$region Properties}

{$region Base}

type
  NtvPropertiesBase<TNtv, TInfo> = abstract class
    protected ntv: TNtv;
    public constructor(ntv: TNtv) := self.ntv := ntv;
    private constructor := raise new OpenCLABCInternalException;
    
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
//    protected function GetValArrArr<T>(id: TInfo; szs: array of UIntPtr): array of array of T;
//    type PT = ^T;
//    begin
//      if szs.Length=0 then
//      begin
//        SetLength(Result,0);
//        exit;
//      end;
//      
//      var res := new IntPtr[szs.Length];
//      SetLength(Result, szs.Length);
//      
//      for var i := 0 to szs.Length-1 do res[i] := Marshal.AllocHGlobal(IntPtr(pointer(szs[i])));
//      try
//        
//        FillVal(id, new UIntPtr(szs.Length*Marshal.SizeOf&<IntPtr>), res[0]);
//        
//        var tsz := Marshal.SizeOf&<T>;
//        for var i := 0 to szs.Length-1 do
//        begin
//          Result[i] := new T[uint64(szs[i]) div tsz];
//          //To Do более эффективное копирование
//          for var i2 := 0 to Result[i].Length-1 do
//            Result[i][i2] := PT(pointer(res[i]+tsz*i2))^;
//        end;
//        
//      finally
//        for var i := 0 to szs.Length-1 do Marshal.FreeHGlobal(res[i]);
//      end;
//      
//    end;
    
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
    
  end;
  
{$endregion Base}

{$region Platform}

type
  PlatformProperties = partial class(NtvPropertiesBase<cl_platform_id, PlatformInfo>)
    
    private static function clGetSize(ntv: cl_platform_id; param_name: PlatformInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformInfo';
    private static function clGetVal(ntv: cl_platform_id; param_name: PlatformInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformInfo';
    
    protected procedure GetSizeImpl(id: PlatformInfo; var sz: UIntPtr); override :=
    OpenCLABCInternalException.RaiseIfError( clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz) );
    protected procedure GetValImpl(id: PlatformInfo; sz: UIntPtr; var res: byte); override :=
    OpenCLABCInternalException.RaiseIfError( clGetVal(ntv, id, sz, res, IntPtr.Zero) );
    
  end;
  
constructor PlatformProperties.Create(ntv: cl_platform_id) := inherited Create(ntv);

function PlatformProperties.GetProfile             := GetString(PlatformInfo.PLATFORM_PROFILE);
function PlatformProperties.GetVersion             := GetString(PlatformInfo.PLATFORM_VERSION);
function PlatformProperties.GetName                := GetString(PlatformInfo.PLATFORM_NAME);
function PlatformProperties.GetVendor              := GetString(PlatformInfo.PLATFORM_VENDOR);
function PlatformProperties.GetExtensions          := GetString(PlatformInfo.PLATFORM_EXTENSIONS);
function PlatformProperties.GetHostTimerResolution := GetVal&<UInt64>(PlatformInfo.PLATFORM_HOST_TIMER_RESOLUTION);

{$endregion Platform}

{$region Device}

type
  DeviceProperties = partial class(NtvPropertiesBase<cl_device_id, DeviceInfo>)
    
    private static function clGetSize(ntv: cl_device_id; param_name: DeviceInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceInfo';
    private static function clGetVal(ntv: cl_device_id; param_name: DeviceInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceInfo';
    
    protected procedure GetSizeImpl(id: DeviceInfo; var sz: UIntPtr); override :=
    OpenCLABCInternalException.RaiseIfError( clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz) );
    protected procedure GetValImpl(id: DeviceInfo; sz: UIntPtr; var res: byte); override :=
    OpenCLABCInternalException.RaiseIfError( clGetVal(ntv, id, sz, res, IntPtr.Zero) );
    
  end;
  
constructor DeviceProperties.Create(ntv: cl_device_id) := inherited Create(ntv);

function DeviceProperties.GetType                               := GetVal&<DeviceType>(DeviceInfo.DEVICE_TYPE);
function DeviceProperties.GetVendorId                           := GetVal&<UInt32>(DeviceInfo.DEVICE_VENDOR_ID);
function DeviceProperties.GetMaxComputeUnits                    := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_COMPUTE_UNITS);
function DeviceProperties.GetMaxWorkItemDimensions              := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_WORK_ITEM_DIMENSIONS);
function DeviceProperties.GetMaxWorkItemSizes                   := GetValArr&<UIntPtr>(DeviceInfo.DEVICE_MAX_WORK_ITEM_SIZES);
function DeviceProperties.GetMaxWorkGroupSize                   := GetVal&<UIntPtr>(DeviceInfo.DEVICE_MAX_WORK_GROUP_SIZE);
function DeviceProperties.GetPreferredVectorWidthChar           := GetVal&<UInt32>(DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_CHAR);
function DeviceProperties.GetPreferredVectorWidthShort          := GetVal&<UInt32>(DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_SHORT);
function DeviceProperties.GetPreferredVectorWidthInt            := GetVal&<UInt32>(DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_INT);
function DeviceProperties.GetPreferredVectorWidthLong           := GetVal&<UInt32>(DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_LONG);
function DeviceProperties.GetPreferredVectorWidthFloat          := GetVal&<UInt32>(DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_FLOAT);
function DeviceProperties.GetPreferredVectorWidthDouble         := GetVal&<UInt32>(DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_DOUBLE);
function DeviceProperties.GetPreferredVectorWidthHalf           := GetVal&<UInt32>(DeviceInfo.DEVICE_PREFERRED_VECTOR_WIDTH_HALF);
function DeviceProperties.GetNativeVectorWidthChar              := GetVal&<UInt32>(DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_CHAR);
function DeviceProperties.GetNativeVectorWidthShort             := GetVal&<UInt32>(DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_SHORT);
function DeviceProperties.GetNativeVectorWidthInt               := GetVal&<UInt32>(DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_INT);
function DeviceProperties.GetNativeVectorWidthLong              := GetVal&<UInt32>(DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_LONG);
function DeviceProperties.GetNativeVectorWidthFloat             := GetVal&<UInt32>(DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_FLOAT);
function DeviceProperties.GetNativeVectorWidthDouble            := GetVal&<UInt32>(DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_DOUBLE);
function DeviceProperties.GetNativeVectorWidthHalf              := GetVal&<UInt32>(DeviceInfo.DEVICE_NATIVE_VECTOR_WIDTH_HALF);
function DeviceProperties.GetMaxClockFrequency                  := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_CLOCK_FREQUENCY);
function DeviceProperties.GetAddressBits                        := GetVal&<UInt32>(DeviceInfo.DEVICE_ADDRESS_BITS);
function DeviceProperties.GetMaxMemAllocSize                    := GetVal&<UInt64>(DeviceInfo.DEVICE_MAX_MEM_ALLOC_SIZE);
function DeviceProperties.GetImageSupport                       := GetVal&<Bool>(DeviceInfo.DEVICE_IMAGE_SUPPORT);
function DeviceProperties.GetMaxReadImageArgs                   := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_READ_IMAGE_ARGS);
function DeviceProperties.GetMaxWriteImageArgs                  := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_WRITE_IMAGE_ARGS);
function DeviceProperties.GetMaxReadWriteImageArgs              := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_READ_WRITE_IMAGE_ARGS);
function DeviceProperties.GetIlVersion                          := GetString(DeviceInfo.DEVICE_IL_VERSION);
function DeviceProperties.GetImage2dMaxWidth                    := GetVal&<UIntPtr>(DeviceInfo.DEVICE_IMAGE2D_MAX_WIDTH);
function DeviceProperties.GetImage2dMaxHeight                   := GetVal&<UIntPtr>(DeviceInfo.DEVICE_IMAGE2D_MAX_HEIGHT);
function DeviceProperties.GetImage3dMaxWidth                    := GetVal&<UIntPtr>(DeviceInfo.DEVICE_IMAGE3D_MAX_WIDTH);
function DeviceProperties.GetImage3dMaxHeight                   := GetVal&<UIntPtr>(DeviceInfo.DEVICE_IMAGE3D_MAX_HEIGHT);
function DeviceProperties.GetImage3dMaxDepth                    := GetVal&<UIntPtr>(DeviceInfo.DEVICE_IMAGE3D_MAX_DEPTH);
function DeviceProperties.GetImageMaxBufferSize                 := GetVal&<UIntPtr>(DeviceInfo.DEVICE_IMAGE_MAX_BUFFER_SIZE);
function DeviceProperties.GetImageMaxArraySize                  := GetVal&<UIntPtr>(DeviceInfo.DEVICE_IMAGE_MAX_ARRAY_SIZE);
function DeviceProperties.GetMaxSamplers                        := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_SAMPLERS);
function DeviceProperties.GetImagePitchAlignment                := GetVal&<UInt32>(DeviceInfo.DEVICE_IMAGE_PITCH_ALIGNMENT);
function DeviceProperties.GetImageBaseAddressAlignment          := GetVal&<UInt32>(DeviceInfo.DEVICE_IMAGE_BASE_ADDRESS_ALIGNMENT);
function DeviceProperties.GetMaxPipeArgs                        := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_PIPE_ARGS);
function DeviceProperties.GetPipeMaxActiveReservations          := GetVal&<UInt32>(DeviceInfo.DEVICE_PIPE_MAX_ACTIVE_RESERVATIONS);
function DeviceProperties.GetPipeMaxPacketSize                  := GetVal&<UInt32>(DeviceInfo.DEVICE_PIPE_MAX_PACKET_SIZE);
function DeviceProperties.GetMaxParameterSize                   := GetVal&<UIntPtr>(DeviceInfo.DEVICE_MAX_PARAMETER_SIZE);
function DeviceProperties.GetMemBaseAddrAlign                   := GetVal&<UInt32>(DeviceInfo.DEVICE_MEM_BASE_ADDR_ALIGN);
function DeviceProperties.GetSingleFpConfig                     := GetVal&<DeviceFPConfig>(DeviceInfo.DEVICE_SINGLE_FP_CONFIG);
function DeviceProperties.GetDoubleFpConfig                     := GetVal&<DeviceFPConfig>(DeviceInfo.DEVICE_DOUBLE_FP_CONFIG);
function DeviceProperties.GetGlobalMemCacheType                 := GetVal&<DeviceMemCacheType>(DeviceInfo.DEVICE_GLOBAL_MEM_CACHE_TYPE);
function DeviceProperties.GetGlobalMemCachelineSize             := GetVal&<UInt32>(DeviceInfo.DEVICE_GLOBAL_MEM_CACHELINE_SIZE);
function DeviceProperties.GetGlobalMemCacheSize                 := GetVal&<UInt64>(DeviceInfo.DEVICE_GLOBAL_MEM_CACHE_SIZE);
function DeviceProperties.GetGlobalMemSize                      := GetVal&<UInt64>(DeviceInfo.DEVICE_GLOBAL_MEM_SIZE);
function DeviceProperties.GetMaxConstantBufferSize              := GetVal&<UInt64>(DeviceInfo.DEVICE_MAX_CONSTANT_BUFFER_SIZE);
function DeviceProperties.GetMaxConstantArgs                    := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_CONSTANT_ARGS);
function DeviceProperties.GetMaxGlobalVariableSize              := GetVal&<UIntPtr>(DeviceInfo.DEVICE_MAX_GLOBAL_VARIABLE_SIZE);
function DeviceProperties.GetGlobalVariablePreferredTotalSize   := GetVal&<UIntPtr>(DeviceInfo.DEVICE_GLOBAL_VARIABLE_PREFERRED_TOTAL_SIZE);
function DeviceProperties.GetLocalMemType                       := GetVal&<DeviceLocalMemType>(DeviceInfo.DEVICE_LOCAL_MEM_TYPE);
function DeviceProperties.GetLocalMemSize                       := GetVal&<UInt64>(DeviceInfo.DEVICE_LOCAL_MEM_SIZE);
function DeviceProperties.GetErrorCorrectionSupport             := GetVal&<Bool>(DeviceInfo.DEVICE_ERROR_CORRECTION_SUPPORT);
function DeviceProperties.GetProfilingTimerResolution           := GetVal&<UIntPtr>(DeviceInfo.DEVICE_PROFILING_TIMER_RESOLUTION);
function DeviceProperties.GetEndianLittle                       := GetVal&<Bool>(DeviceInfo.DEVICE_ENDIAN_LITTLE);
function DeviceProperties.GetAvailable                          := GetVal&<Bool>(DeviceInfo.DEVICE_AVAILABLE);
function DeviceProperties.GetCompilerAvailable                  := GetVal&<Bool>(DeviceInfo.DEVICE_COMPILER_AVAILABLE);
function DeviceProperties.GetLinkerAvailable                    := GetVal&<Bool>(DeviceInfo.DEVICE_LINKER_AVAILABLE);
function DeviceProperties.GetExecutionCapabilities              := GetVal&<DeviceExecCapabilities>(DeviceInfo.DEVICE_EXECUTION_CAPABILITIES);
function DeviceProperties.GetQueueOnHostProperties              := GetVal&<CommandQueueProperties>(DeviceInfo.DEVICE_QUEUE_ON_HOST_PROPERTIES);
function DeviceProperties.GetQueueOnDeviceProperties            := GetVal&<CommandQueueProperties>(DeviceInfo.DEVICE_QUEUE_ON_DEVICE_PROPERTIES);
function DeviceProperties.GetQueueOnDevicePreferredSize         := GetVal&<UInt32>(DeviceInfo.DEVICE_QUEUE_ON_DEVICE_PREFERRED_SIZE);
function DeviceProperties.GetQueueOnDeviceMaxSize               := GetVal&<UInt32>(DeviceInfo.DEVICE_QUEUE_ON_DEVICE_MAX_SIZE);
function DeviceProperties.GetMaxOnDeviceQueues                  := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_ON_DEVICE_QUEUES);
function DeviceProperties.GetMaxOnDeviceEvents                  := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_ON_DEVICE_EVENTS);
function DeviceProperties.GetBuiltInKernels                     := GetString(DeviceInfo.DEVICE_BUILT_IN_KERNELS);
function DeviceProperties.GetName                               := GetString(DeviceInfo.DEVICE_NAME);
function DeviceProperties.GetVendor                             := GetString(DeviceInfo.DEVICE_VENDOR);
function DeviceProperties.GetProfile                            := GetString(DeviceInfo.DEVICE_PROFILE);
function DeviceProperties.GetVersion                            := GetString(DeviceInfo.DEVICE_VERSION);
function DeviceProperties.GetOpenclCVersion                     := GetString(DeviceInfo.DEVICE_OPENCL_C_VERSION);
function DeviceProperties.GetExtensions                         := GetString(DeviceInfo.DEVICE_EXTENSIONS);
function DeviceProperties.GetPrintfBufferSize                   := GetVal&<UIntPtr>(DeviceInfo.DEVICE_PRINTF_BUFFER_SIZE);
function DeviceProperties.GetPreferredInteropUserSync           := GetVal&<Bool>(DeviceInfo.DEVICE_PREFERRED_INTEROP_USER_SYNC);
function DeviceProperties.GetPartitionMaxSubDevices             := GetVal&<UInt32>(DeviceInfo.DEVICE_PARTITION_MAX_SUB_DEVICES);
function DeviceProperties.GetPartitionProperties                := GetValArr&<DevicePartitionProperty>(DeviceInfo.DEVICE_PARTITION_PROPERTIES);
function DeviceProperties.GetPartitionAffinityDomain            := GetVal&<DeviceAffinityDomain>(DeviceInfo.DEVICE_PARTITION_AFFINITY_DOMAIN);
function DeviceProperties.GetPartitionType                      := GetValArr&<DevicePartitionProperty>(DeviceInfo.DEVICE_PARTITION_TYPE);
function DeviceProperties.GetReferenceCount                     := GetVal&<UInt32>(DeviceInfo.DEVICE_REFERENCE_COUNT);
function DeviceProperties.GetSvmCapabilities                    := GetVal&<DeviceSVMCapabilities>(DeviceInfo.DEVICE_SVM_CAPABILITIES);
function DeviceProperties.GetPreferredPlatformAtomicAlignment   := GetVal&<UInt32>(DeviceInfo.DEVICE_PREFERRED_PLATFORM_ATOMIC_ALIGNMENT);
function DeviceProperties.GetPreferredGlobalAtomicAlignment     := GetVal&<UInt32>(DeviceInfo.DEVICE_PREFERRED_GLOBAL_ATOMIC_ALIGNMENT);
function DeviceProperties.GetPreferredLocalAtomicAlignment      := GetVal&<UInt32>(DeviceInfo.DEVICE_PREFERRED_LOCAL_ATOMIC_ALIGNMENT);
function DeviceProperties.GetMaxNumSubGroups                    := GetVal&<UInt32>(DeviceInfo.DEVICE_MAX_NUM_SUB_GROUPS);
function DeviceProperties.GetSubGroupIndependentForwardProgress := GetVal&<Bool>(DeviceInfo.DEVICE_SUB_GROUP_INDEPENDENT_FORWARD_PROGRESS);

{$endregion Device}

{$region Context}

type
  ContextProperties = partial class(NtvPropertiesBase<cl_context, ContextInfo>)
    
    private static function clGetSize(ntv: cl_context; param_name: ContextInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetContextInfo';
    private static function clGetVal(ntv: cl_context; param_name: ContextInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetContextInfo';
    
    protected procedure GetSizeImpl(id: ContextInfo; var sz: UIntPtr); override :=
    OpenCLABCInternalException.RaiseIfError( clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz) );
    protected procedure GetValImpl(id: ContextInfo; sz: UIntPtr; var res: byte); override :=
    OpenCLABCInternalException.RaiseIfError( clGetVal(ntv, id, sz, res, IntPtr.Zero) );
    
  end;
  
constructor ContextProperties.Create(ntv: cl_context) := inherited Create(ntv);

function ContextProperties.GetReferenceCount := GetVal&<UInt32>(ContextInfo.CONTEXT_REFERENCE_COUNT);
function ContextProperties.GetNumDevices     := GetVal&<UInt32>(ContextInfo.CONTEXT_NUM_DEVICES);
function ContextProperties.GetProperties     := GetValArr&<ContextProperties>(ContextInfo.CONTEXT_PROPERTIES);

{$endregion Context}

{$region ProgramCode}

type
  ProgramCodeProperties = partial class(NtvPropertiesBase<cl_program, ProgramInfo>)
    
    private static function clGetSize(ntv: cl_program; param_name: ProgramInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramInfo';
    private static function clGetVal(ntv: cl_program; param_name: ProgramInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramInfo';
    
    protected procedure GetSizeImpl(id: ProgramInfo; var sz: UIntPtr); override :=
    OpenCLABCInternalException.RaiseIfError( clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz) );
    protected procedure GetValImpl(id: ProgramInfo; sz: UIntPtr; var res: byte); override :=
    OpenCLABCInternalException.RaiseIfError( clGetVal(ntv, id, sz, res, IntPtr.Zero) );
    
  end;
  
constructor ProgramCodeProperties.Create(ntv: cl_program) := inherited Create(ntv);

function ProgramCodeProperties.GetReferenceCount          := GetVal&<UInt32>(ProgramInfo.PROGRAM_REFERENCE_COUNT);
function ProgramCodeProperties.GetSource                  := GetString(ProgramInfo.PROGRAM_SOURCE);
function ProgramCodeProperties.GetIl                      := GetValArr&<Byte>(ProgramInfo.PROGRAM_IL);
function ProgramCodeProperties.GetNumKernels              := GetVal&<UIntPtr>(ProgramInfo.PROGRAM_NUM_KERNELS);
function ProgramCodeProperties.GetKernelNames             := GetString(ProgramInfo.PROGRAM_KERNEL_NAMES);
function ProgramCodeProperties.GetScopeGlobalCtorsPresent := GetVal&<Bool>(ProgramInfo.PROGRAM_SCOPE_GLOBAL_CTORS_PRESENT);
function ProgramCodeProperties.GetScopeGlobalDtorsPresent := GetVal&<Bool>(ProgramInfo.PROGRAM_SCOPE_GLOBAL_DTORS_PRESENT);

{$endregion ProgramCode}

{$region Kernel}

type
  KernelProperties = partial class(NtvPropertiesBase<cl_kernel, KernelInfo>)
    
    private static function clGetSize(ntv: cl_kernel; param_name: KernelInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelInfo';
    private static function clGetVal(ntv: cl_kernel; param_name: KernelInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelInfo';
    
    protected procedure GetSizeImpl(id: KernelInfo; var sz: UIntPtr); override :=
    OpenCLABCInternalException.RaiseIfError( clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz) );
    protected procedure GetValImpl(id: KernelInfo; sz: UIntPtr; var res: byte); override :=
    OpenCLABCInternalException.RaiseIfError( clGetVal(ntv, id, sz, res, IntPtr.Zero) );
    
  end;
  
constructor KernelProperties.Create(ntv: cl_kernel) := inherited Create(ntv);

function KernelProperties.GetFunctionName   := GetString(KernelInfo.KERNEL_FUNCTION_NAME);
function KernelProperties.GetNumArgs        := GetVal&<UInt32>(KernelInfo.KERNEL_NUM_ARGS);
function KernelProperties.GetReferenceCount := GetVal&<UInt32>(KernelInfo.KERNEL_REFERENCE_COUNT);
function KernelProperties.GetAttributes     := GetString(KernelInfo.KERNEL_ATTRIBUTES);

{$endregion Kernel}

{$region MemorySegment}

type
  MemorySegmentProperties = partial class(NtvPropertiesBase<cl_mem, MemInfo>)
    
    private static function clGetSize(ntv: cl_mem; param_name: MemInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    private static function clGetVal(ntv: cl_mem; param_name: MemInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    
    protected procedure GetSizeImpl(id: MemInfo; var sz: UIntPtr); override :=
    OpenCLABCInternalException.RaiseIfError( clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz) );
    protected procedure GetValImpl(id: MemInfo; sz: UIntPtr; var res: byte); override :=
    OpenCLABCInternalException.RaiseIfError( clGetVal(ntv, id, sz, res, IntPtr.Zero) );
    
  end;
  
constructor MemorySegmentProperties.Create(ntv: cl_mem) := inherited Create(ntv);

function MemorySegmentProperties.GetFlags          := GetVal&<MemFlags>(MemInfo.MEM_FLAGS);
function MemorySegmentProperties.GetHostPtr        := GetVal&<IntPtr>(MemInfo.MEM_HOST_PTR);
function MemorySegmentProperties.GetMapCount       := GetVal&<UInt32>(MemInfo.MEM_MAP_COUNT);
function MemorySegmentProperties.GetReferenceCount := GetVal&<UInt32>(MemInfo.MEM_REFERENCE_COUNT);
function MemorySegmentProperties.GetUsesSvmPointer := GetVal&<Bool>(MemInfo.MEM_USES_SVM_POINTER);

{$endregion MemorySegment}

{$region MemorySubSegment}

type
  MemorySubSegmentProperties = partial class(MemorySegmentProperties)
    
    private static function clGetSize(ntv: cl_mem; param_name: MemInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    private static function clGetVal(ntv: cl_mem; param_name: MemInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    
    protected procedure GetSizeImpl(id: MemInfo; var sz: UIntPtr); override :=
    OpenCLABCInternalException.RaiseIfError( clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz) );
    protected procedure GetValImpl(id: MemInfo; sz: UIntPtr; var res: byte); override :=
    OpenCLABCInternalException.RaiseIfError( clGetVal(ntv, id, sz, res, IntPtr.Zero) );
    
  end;
  
constructor MemorySubSegmentProperties.Create(ntv: cl_mem) := inherited Create(ntv);

function MemorySubSegmentProperties.GetOffset := GetVal&<UIntPtr>(MemInfo.MEM_OFFSET);

{$endregion MemorySubSegment}

{$region CLArray}

type
  CLArrayProperties = partial class(NtvPropertiesBase<cl_mem, MemInfo>)
    
    private static function clGetSize(ntv: cl_mem; param_name: MemInfo; param_value_size: UIntPtr; param_value: IntPtr; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    private static function clGetVal(ntv: cl_mem; param_name: MemInfo; param_value_size: UIntPtr; var param_value: byte; param_value_size_ret: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    
    protected procedure GetSizeImpl(id: MemInfo; var sz: UIntPtr); override :=
    OpenCLABCInternalException.RaiseIfError( clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz) );
    protected procedure GetValImpl(id: MemInfo; sz: UIntPtr; var res: byte); override :=
    OpenCLABCInternalException.RaiseIfError( clGetVal(ntv, id, sz, res, IntPtr.Zero) );
    
  end;
  
constructor CLArrayProperties.Create(ntv: cl_mem) := inherited Create(ntv);

function CLArrayProperties.GetFlags          := GetVal&<MemFlags>(MemInfo.MEM_FLAGS);
function CLArrayProperties.GetHostPtr        := GetVal&<IntPtr>(MemInfo.MEM_HOST_PTR);
function CLArrayProperties.GetMapCount       := GetVal&<UInt32>(MemInfo.MEM_MAP_COUNT);
function CLArrayProperties.GetReferenceCount := GetVal&<UInt32>(MemInfo.MEM_REFERENCE_COUNT);
function CLArrayProperties.GetUsesSvmPointer := GetVal&<Bool>(MemInfo.MEM_USES_SVM_POINTER);

{$endregion CLArray}

{$endregion Properties}

{$region Wrappers}

{$region Device}

static function Device.FromNative(ntv: cl_device_id): Device;
begin
  
  var parent: cl_device_id;
  OpenCLABCInternalException.RaiseIfError(
    cl.GetDeviceInfo(ntv, DeviceInfo.DEVICE_PARENT_DEVICE, new UIntPtr(cl_device_id.Size), parent, IntPtr.Zero)
  );
  
  if parent=cl_device_id.Zero then
    Result := new Device(ntv) else
    Result := new SubDevice(parent, ntv);
  
end;

{$endregion Device}

{$region MemorySegment}

static function MemorySegment.FromNative(ntv: cl_mem): MemorySegment;
begin
  var t: MemObjectType;
  OpenCLABCInternalException.RaiseIfError(
    cl.GetMemObjectInfo(ntv, MemInfo.MEM_TYPE, new UIntPtr(sizeof(MemObjectType)), t, IntPtr.Zero)
  );
  
  if t<>MemObjectType.MEM_OBJECT_BUFFER then
    raise new ArgumentException($'Неправильный тип неуправляемого объекта памяти. Ожидалось [MEM_OBJECT_BUFFER], а не [{t}]');
  
  var parent: cl_mem;
  OpenCLABCInternalException.RaiseIfError(
    cl.GetMemObjectInfo(ntv, MemInfo.MEM_ASSOCIATED_MEMOBJECT, new UIntPtr(cl_mem.Size), parent, IntPtr.Zero)
  );
  
  if parent=cl_mem.Zero then
  begin
    Result := new MemorySegment(ntv);
    GC.AddMemoryPressure(Result.Size64);
  end else
    Result := new MemorySubSegment(parent, ntv);
  
end;

{$endregion MemorySegment}

{$region CLArray}

function CLArray<T>.GetItemProp(ind: integer): T :=
GetValue(ind);
procedure CLArray<T>.SetItemProp(ind: integer; value: T) :=
WriteValue(value, ind);

function CLArray<T>.GetSectionProp(range: IntRange): array of T :=
GetArray(range.Low, range.High-range.Low+1);
procedure CLArray<T>.SetSectionProp(range: IntRange; value: array of T) :=
WriteArray(value, range.Low, range.High-range.Low+1, 0);

{$endregion CLArray}

{$endregion Wrappers}

{$region Util type's}

{$region Blittable}

type
  BlittableException = sealed class(Exception)
    public constructor(t, blame: System.Type; source_name: string) :=
    inherited Create(t=blame ? $'Значения типа {t} нельзя {source_name}' : $'Значения типа {t} нельзя {source_name}, потому что он содержит тип {blame}' );
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
      
      foreach var fld in t.GetFields(System.Reflection.BindingFlags.Instance or System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.NonPublic) do
        if fld.FieldType<>t then
        begin
          Result := Blame(fld.FieldType);
          if Result<>nil then break;
        end;
      
      if Result=nil then
      begin
        var o := System.Activator.CreateInstance(t);
        try
          GCHandle.Alloc(o, GCHandleType.Pinned).Free;
        except
          on System.ArgumentException do
            Result := t;
        end;
      end;
      
      blittable_cache[t] := Result;
    end;
    
    public static procedure RaiseIfBad(t: System.Type; source_name: string);
    begin
      var blame := BlittableHelper.Blame(t);
      if blame=nil then exit;
      raise new BlittableException(t, blame, source_name);
    end;
    
  end;
  
  NativeValue<T> = partial class
    static constructor :=
    BlittableHelper.RaiseIfBad(typeof(T), 'использовать как элементы CLArray<>');
  end;
  
  CLArray<T> = partial class
    static constructor :=
    BlittableHelper.RaiseIfBad(typeof(T), 'использовать как элементы CLArray<>');
  end;
  
{$endregion Blittable}

{$region InterlockedBoolean}

type
  InterlockedBoolean = record
    private val := 0;
    
    public function TrySet(b: boolean): boolean;
    begin
      var prev := integer(not b);
      var curr := integer(b);
      Result := Interlocked.CompareExchange(val, curr, prev)=prev;
    end;
    
    public static function operator implicit(b: InterlockedBoolean): boolean := b.val<>0;
    
  end;
  
{$endregion InterlockedBoolean}

{$region NativeUtils}

type
  NativeUtils = static class
    
    public static function AsPtr<T>(p: pointer): ^T := p;
    public static function AsPtr<T>(p: IntPtr) := AsPtr&<T>(pointer(p));
    
    public static function CopyToUnm<TRecord>(a: TRecord): IntPtr; where TRecord: record;
    begin
      Result := Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>);
      AsPtr&<TRecord>(Result)^ := a;
    end;
    
    public static function StartNewBgThread(p: Action): Thread;
    begin
      Result := new Thread(p);
      Result.IsBackground := true;
      Result.Start;
    end;
    
  end;
  
{$endregion NativeUtils}

{$region CLTaskErrHandler}

type
  CLTaskErrHandler = abstract class
    private local_err_lst := new List<Exception>;
    
    {$region AddErr}
    
    protected procedure AddErr(e: Exception);
    begin
      if e is OpenCLABCInternalException then
        // Внутренние ошибки не регестрируем
        System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e).Throw;
      // HPQ(()->exit()) + HPQ(()->raise)
      // Тут сначала вычисляет HadError как false, а затем переключает на true
      had_error_cache := true;
      local_err_lst += e;
    end;
    
    {$endregion AddErr}
    
    function get_local_err_lst: List<Exception>;
    begin
      had_error_cache := nil;
      Result := local_err_lst;
    end;
    
    private had_error_cache := default(boolean?);
    protected function HadErrorInPrev(can_cache: boolean): boolean; abstract;
    public function HadError(can_cache: boolean): boolean;
    begin
      if had_error_cache<>nil then
      begin
        Result := had_error_cache.Value;
        exit;
      end;
      Result := (local_err_lst.Count<>0) or HadErrorInPrev(can_cache);
      if can_cache then had_error_cache := Result;
    end;
    
    protected function TryRemoveErrorsInPrev(origin_cache: Dictionary<CLTaskErrHandler, boolean>; handler: Exception->boolean): boolean; abstract;
    protected function TryRemoveErrors(origin_cache: Dictionary<CLTaskErrHandler, boolean>; handler: Exception->boolean): boolean;
    begin
      Result := false;
      if had_error_cache=false then exit;
      
      Result := TryRemoveErrorsInPrev(origin_cache, handler);
      
      Result := (local_err_lst.RemoveAll(handler)<>0) or Result;
      if Result then had_error_cache := nil;
    end;
    public procedure TryRemoveErrors(handler: Exception->boolean) :=
    TryRemoveErrors(new Dictionary<CLTaskErrHandler, boolean>, handler);
    
    protected procedure FillErrLstWithPrev(origin_cache: HashSet<CLTaskErrHandler>; lst: List<Exception>); abstract;
    protected procedure FillErrLst(origin_cache: HashSet<CLTaskErrHandler>; lst: List<Exception>);
    begin
      {$ifndef DEBUG}
      if not HadError(true) then exit;
      {$endif DEBUG}
      
      FillErrLstWithPrev(origin_cache, lst);
      
      lst.AddRange(local_err_lst);
    end;
    public procedure FillErrLst(lst: List<Exception>) :=
    FillErrLst(new HashSet<CLTaskErrHandler>, lst);
    
    public procedure SanityCheck(err_lst: List<Exception>);
    begin
      
      // QErr*QErr - second cache wouldn't be calculated
//      if had_error_cache=nil then
//        raise new OpenCLABCInternalException($'SanityCheck expects all had_error_cache to exist');
      
      begin
        var had_error := self.HadError(true);
        if had_error <> (err_lst.Count<>0) then
          raise new OpenCLABCInternalException($'{had_error} <> {err_lst.Count}');
      end;
      
      // In case "A + B*C" handler of C would see error in A, but ignore it in FillErrLst
//      if prev_action <> EPA_Ignore then
//        foreach var h in prev do
//          h.SanityCheck;
      
    end;
//    public procedure SanityCheck;
//    begin
//      var err_lst := new List<Exception>;
//      FillErrLst(err_lst);
//      SanityCheck(err_lst);
//    end;
    
  end;
  
  CLTaskErrHandlerEmpty = sealed class(CLTaskErrHandler)
    
    public constructor := exit;
    
    protected function HadErrorInPrev(can_cache: boolean): boolean; override := false;
    
    protected function TryRemoveErrorsInPrev(origin_cache: Dictionary<CLTaskErrHandler, boolean>; handler: Exception->boolean): boolean; override := false;
    
    protected procedure FillErrLstWithPrev(origin_cache: HashSet<CLTaskErrHandler>; lst: List<Exception>); override := exit;
    
  end;
  
  CLTaskErrHandlerBranchBase = sealed class(CLTaskErrHandler)
    private origin: CLTaskErrHandler;
    
    public constructor(origin: CLTaskErrHandler) := self.origin := origin;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function HadErrorInPrev(can_cache: boolean): boolean; override := origin.HadError(can_cache);
    
    protected function TryRemoveErrorsInPrev(origin_cache: Dictionary<CLTaskErrHandler, boolean>; handler: Exception->boolean): boolean; override;
    begin
      if origin_cache.TryGetValue(origin, Result) then exit;
      // Can't remove from here, because "A + B*C.Handle" would otherwise consume error in A
//      Result := origin.TryRemoveErrors(origin_cache, handler);
    end;
    
    protected procedure FillErrLstWithPrev(origin_cache: HashSet<CLTaskErrHandler>; lst: List<Exception>); override;
    begin
      if origin_cache.Contains(origin) then exit;
      origin.FillErrLst(origin_cache, lst);
    end;
    
  end;
  CLTaskErrHandlerBranchCombinator = sealed class(CLTaskErrHandler)
    private origin: CLTaskErrHandler;
    private branches: array of CLTaskErrHandler;
    
    public constructor(origin: CLTaskErrHandler; branches: array of CLTaskErrHandler);
    begin
      self.origin := origin;
      self.branches := branches;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function HadErrorInPrev(can_cache: boolean): boolean; override;
    begin
      Result := origin.HadError(can_cache);
      if Result then exit;
      foreach var h in branches do
      begin
        Result := h.HadError(can_cache);
        if Result then exit;
      end;
    end;
    
    protected function TryRemoveErrorsInPrev(origin_cache: Dictionary<CLTaskErrHandler, boolean>; handler: Exception->boolean): boolean; override;
    begin
      Result := origin.TryRemoveErrors(origin_cache, handler);
      origin_cache.Add(origin, Result);
      foreach var h in branches do
        Result := h.TryRemoveErrors(origin_cache, handler) or Result;
      origin_cache.Remove(origin);
    end;
    
    protected procedure FillErrLstWithPrev(origin_cache: HashSet<CLTaskErrHandler>; lst: List<Exception>); override;
    begin
      origin.FillErrLst(origin_cache, lst);
      {$ifdef DEBUG}if not{$endif}origin_cache.Add(origin)
      {$ifdef DEBUG}then
        raise new OpenCLABCInternalException($'Origin added multiple times');
      {$endif DEBUG};
      foreach var h in branches do
        h.FillErrLst(origin_cache, lst);
      origin_cache.Remove(origin);
    end;
    
  end;
  
  CLTaskErrHandlerThiefBase = abstract class(CLTaskErrHandler)
    protected victim: CLTaskErrHandler;
    
    public constructor(victim: CLTaskErrHandler) := self.victim := victim;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function CanSteal: boolean; abstract;
    public procedure StealPrevErrors;
    begin
      if victim=nil then exit;
      if CanSteal then
        victim.FillErrLst(self.local_err_lst);
      victim := nil;
    end;
    
    protected function HadErrorInVictim(can_cache: boolean): boolean :=
    (victim<>nil) and victim.HadError(can_cache);
    
  end;
  CLTaskErrHandlerThief = sealed class(CLTaskErrHandlerThiefBase)
    
    protected function CanSteal: boolean; override := true;
    
    protected function HadErrorInPrev(can_cache: boolean): boolean; override := HadErrorInVictim(can_cache);
    
    protected function TryRemoveErrorsInPrev(origin_cache: Dictionary<CLTaskErrHandler, boolean>; handler: Exception->boolean): boolean; override;
    begin
      StealPrevErrors;
      Result := false;
    end;
    
    protected procedure FillErrLstWithPrev(origin_cache: HashSet<CLTaskErrHandler>; lst: List<Exception>); override;
    begin
      StealPrevErrors;
    end;
    
  end;
  CLTaskErrHandlerMultiusableRepeater = sealed class(CLTaskErrHandlerThiefBase)
    private prev_handler: CLTaskErrHandler;
    
    public constructor(prev_handler, mu_handler: CLTaskErrHandler);
    begin
      inherited Create(mu_handler);
      self.prev_handler := prev_handler;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function CanSteal: boolean; override :=
    not prev_handler.HadError(true);
    
    protected function HadErrorInPrev(can_cache: boolean): boolean; override :=
    // mu_handler.HadError would be called more often,
    // so it's more likely to already have cache
    HadErrorInVictim(can_cache) or prev_handler.HadError(can_cache);
    
    protected function TryRemoveErrorsInPrev(origin_cache: Dictionary<CLTaskErrHandler, boolean>; handler: Exception->boolean): boolean; override;
    begin
      Result := prev_handler.TryRemoveErrors(origin_cache, handler);
      if not prev_handler.HadError(true) then StealPrevErrors;
    end;
    
    protected procedure FillErrLstWithPrev(origin_cache: HashSet<CLTaskErrHandler>; lst: List<Exception>); override;
    begin
      var prev_c := lst.Count;
      prev_handler.FillErrLst(lst);
      if prev_c=lst.Count then StealPrevErrors;
    end;
    
  end;
  
{$endregion CLTaskErrHandler}

{$region CLTaskData}

type
  CLTaskGlobalData = sealed partial class
    public tsk: CLTaskBase;
    
    public c: Context;
    public cl_c: cl_context;
    public cl_dvc: cl_device_id;
    
    private curr_inv_cq := cl_command_queue.Zero;
    private outer_cq := cl_command_queue.Zero;
    private free_cqs := new System.Collections.Concurrent.ConcurrentBag<cl_command_queue>;
    
    public curr_err_handler: CLTaskErrHandler := new CLTaskErrHandlerEmpty;
    
    private constructor := raise new OpenCLABCInternalException;
    
    public function GetCQ(async_enqueue: boolean := false): cl_command_queue;
    begin
      Result := curr_inv_cq;
      
      if Result=cl_command_queue.Zero then
      begin
        if outer_cq<>cl_command_queue.Zero then
        begin
          Result := outer_cq;
          outer_cq := cl_command_queue.Zero;
        end else
        if free_cqs.TryTake(Result) then
          else
        begin
          var ec: ErrorCode;
          Result := cl.CreateCommandQueue(cl_c, cl_dvc, CommandQueueProperties.NONE, ec);
          OpenCLABCInternalException.RaiseIfError(ec);
        end;
      end;
      
      curr_inv_cq := if async_enqueue then cl_command_queue.Zero else Result;
    end;
    
  end;
  
{$endregion CLTaskData}

{$region EventList}

type
  AttachCallbackData = sealed class
    public work: Action;
    {$ifdef EventDebug}
    public reason: string;
    {$endif EventDebug}
    
    public constructor(work: Action{$ifdef EventDebug}; reason: string{$endif});
    begin
      self.work := work;
      {$ifdef EventDebug}
      self.reason := reason;
      {$endif EventDebug}
    end;
    private constructor := raise new OpenCLABCInternalException;
    
  end;
  
  MultiAttachCallbackData = sealed class
    public work: Action;
    public left_c: integer;
    {$ifdef EventDebug}
    public reason: string;
    {$endif EventDebug}
    
    public constructor(work: Action; left_c: integer{$ifdef EventDebug}; reason: string{$endif});
    begin
      self.work := work;
      self.left_c := left_c;
      {$ifdef EventDebug}
      self.reason := reason;
      {$endif EventDebug}
    end;
    private constructor := raise new OpenCLABCInternalException;
    
  end;
  
  EventList = record
    public evs: array of cl_event;
    public count := 0;
    
    {$region Misc}
    
    public property Item[i: integer]: cl_event read evs[i]; default;
    
    public static function operator=(l1, l2: EventList): boolean;
    begin
      Result := false;
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
    
    public constructor(count: integer) :=
    self.evs := new cl_event[count];
    public constructor := raise new OpenCLABCInternalException;
    public static Empty := default(EventList);
    
    public static function operator implicit(ev: cl_event): EventList;
    begin
      if ev=cl_event.Zero then
        Result := Empty else
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
    
    public static procedure operator+=(var l: EventList; ev: cl_event);
    begin
      l.evs[l.count] := ev;
      l.count += 1;
    end;
    
    public static procedure operator+=(var l: EventList; ev: EventList);
    begin
      for var i := 0 to ev.count-1 do
        l += ev[i];
    end;
    
    public static function operator+(l1, l2: EventList): EventList;
    begin
      Result := new EventList(l1.count+l2.count);
      Result += l1;
      Result += l2;
    end;
    
    public static function operator+(l: EventList; ev: cl_event): EventList;
    begin
      Result := new EventList(l.count+1);
      Result += l;
      Result += ev;
    end;
    
    private static function Combine<TList>(evs: TList): EventList; where TList: IList<EventList>;
    begin
      Result := EventList.Empty;
      var count := 0;
      
      //TODO #2589
      for var i := 0 to (evs as IList<EventList>).Count-1 do
        count += evs.Item[i].count;
      if count=0 then exit;
      
      Result := new EventList(count);
      //TODO #2589
      for var i := 0 to (evs as IList<EventList>).Count-1 do
        Result += evs.Item[i];
      
    end;
    
    {$endregion operator+}
    
    {$region AttachCallback}
    
    private static procedure CheckEvErr(ev: cl_event{$ifdef EventDebug}; reason: string{$endif});
    begin
      {$ifdef EventDebug}
      EventDebug.CheckExists(ev, reason);
      {$endif EventDebug}
      var st: CommandExecutionStatus;
      var ec := cl.GetEventInfo(ev, EventInfo.EVENT_COMMAND_EXECUTION_STATUS, new UIntPtr(sizeof(CommandExecutionStatus)), st, IntPtr.Zero);
      OpenCLABCInternalException.RaiseIfError(ec);
      OpenCLABCInternalException.RaiseIfError(st);
    end;
    
    private static procedure InvokeAttachedCallback(ev: cl_event; st: CommandExecutionStatus; data: IntPtr);
    begin
      var hnd := GCHandle(data);
      var cb_data := AttachCallbackData(hnd.Target);
      // st копирует значение переданное в cl.SetEventCallback, поэтому он не подходит
      CheckEvErr(ev{$ifdef EventDebug}, cb_data.reason{$endif});
      {$ifdef EventDebug}
      EventDebug.RegisterEventRelease(ev, $'released in callback, working on {cb_data.reason}');
      {$endif EventDebug}
      OpenCLABCInternalException.RaiseIfError( cl.ReleaseEvent(ev) );
      hnd.Free;
      cb_data.work();
    end;
    private static attachable_callback: EventCallback := InvokeAttachedCallback;
    
    public static procedure AttachCallback(midway: boolean; ev: cl_event; work: Action{$ifdef EventDebug}; reason: string{$endif});
    begin
      if midway then
      begin
        {$ifdef EventDebug}
        EventDebug.RegisterEventRetain(ev, $'retained before midway callback, working on {reason}');
        {$endif EventDebug}
        OpenCLABCInternalException.RaiseIfError(cl.RetainEvent(ev));
      end;
      var cb_data := new AttachCallbackData(work{$ifdef EventDebug}, reason{$endif});
      var ec := cl.SetEventCallback(ev, CommandExecutionStatus.COMPLETE, attachable_callback, GCHandle.ToIntPtr(GCHandle.Alloc(cb_data)));
      OpenCLABCInternalException.RaiseIfError(ec);
    end;
    
    {$endregion AttachCallback}
    
    {$region MultiAttachCallback}
    
    private static procedure InvokeMultiAttachedCallback(ev: cl_event; st: CommandExecutionStatus; data: IntPtr);
    begin
      var hnd := GCHandle(data);
      var cb_data := MultiAttachCallbackData(hnd.Target);
      // st копирует значение переданное в cl.SetEventCallback, поэтому он не подходит
      CheckEvErr(ev{$ifdef EventDebug}, cb_data.reason{$endif});
      {$ifdef EventDebug}
      EventDebug.RegisterEventRelease(ev, $'released in multi-callback, working on {cb_data.reason}');
      {$endif EventDebug}
      OpenCLABCInternalException.RaiseIfError(cl.ReleaseEvent(ev));
      if Interlocked.Decrement(cb_data.left_c) <> 0 then exit;
      hnd.Free;
      cb_data.work();
    end;
    private static multi_attachable_callback: EventCallback := InvokeMultiAttachedCallback;
    
    public procedure MultiAttachCallback(midway: boolean; work: Action{$ifdef EventDebug}; reason: string{$endif}) :=
    case self.count of
      0: work;
      1: AttachCallback(midway, self.evs[0], work{$ifdef EventDebug}, reason{$endif});
      else
      begin
        if midway then self.Retain({$ifdef EventDebug}$'retained before midway multi-callback, working on {reason}'{$endif});
        var cb_data := new MultiAttachCallbackData(work, self.count{$ifdef EventDebug}, reason{$endif});
        var hnd_ptr := GCHandle.ToIntPtr(GCHandle.Alloc(cb_data));
        for var i := 0 to count-1 do
        begin
          var ec := cl.SetEventCallback(evs[i], CommandExecutionStatus.COMPLETE, multi_attachable_callback, hnd_ptr);
          OpenCLABCInternalException.RaiseIfError(ec);
        end;
      end;
    end;
    
    {$endregion MultiAttachCallback}
    
    {$region Retain/Release}
    
    public procedure Retain({$ifdef EventDebug}reason: string{$endif}) :=
    for var i := 0 to count-1 do
    begin
      {$ifdef EventDebug}
      EventDebug.RegisterEventRetain(evs[i], $'{reason}, together with evs: {evs.JoinToString}');
      {$endif EventDebug}
      OpenCLABCInternalException.RaiseIfError( cl.RetainEvent(evs[i]) );
    end;
    
    public procedure Release({$ifdef EventDebug}reason: string{$endif}) :=
    for var i := 0 to count-1 do
    begin
      {$ifdef EventDebug}
      EventDebug.RegisterEventRelease(evs[i], $'{reason}, together with evs: {evs.JoinToString}');
      {$endif EventDebug}
      OpenCLABCInternalException.RaiseIfError( cl.ReleaseEvent(evs[i]) );
    end;
    
    public procedure WaitAndRelease({$ifdef EventDebug}reason: string{$endif});
    begin
      if count=0 then exit;
      
      var ec := cl.WaitForEvents(self.count, self.evs);
      if ec<>ErrorCode.EXEC_STATUS_ERROR_FOR_EVENTS_IN_WAIT_LIST then
        OpenCLABCInternalException.RaiseIfError(ec) else
      for var i := 0 to count-1 do
        CheckEvErr(evs[i]{$ifdef EventDebug}, reason{$endif});
      
      self.Release({$ifdef EventDebug}$'discarding after being waited upon for {reason}'{$endif EventDebug});
    end;
    
    {$endregion Retain/Release}
    
  end;
  
{$endregion EventList}

{$region UserEvent}

type
  UserEvent = sealed class
    private uev: cl_event;
    private done := new InterlockedBoolean;
    
    {$region constructor's}
    
    private constructor(c: cl_context{$ifdef EventDebug}; reason: string{$endif});
    begin
      var ec: ErrorCode;
      self.uev := cl.CreateUserEvent(c, ec);
      OpenCLABCInternalException.RaiseIfError(ec);
      {$ifdef EventDebug}
      EventDebug.RegisterEventRetain(self.uev, $'Created for {reason}');
      {$endif EventDebug}
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    public static function StartBackgroundWork(after: EventList; work: Action; g: CLTaskGlobalData{$ifdef EventDebug}; reason: string{$endif}): UserEvent;
    begin
      var res := new UserEvent(g.cl_c
        {$ifdef EventDebug}, $'BackgroundWork, executing {reason}, after waiting on: {after.evs?.JoinToString}'{$endif}
      );
      
      var err_handler := g.curr_err_handler;
      
      NativeUtils.StartNewBgThread(()->
      begin
        after.WaitAndRelease({$ifdef EventDebug}$'Background work with res_ev={res}'{$endif});
        
        if err_handler.HadError(true) then
        begin
          res.SetComplete;
          exit;
        end;
        
        try
          work;
        except
          on e: Exception do
            err_handler.AddErr(e);
        end;
        
        res.SetComplete;
      end);
      
      Result := res;
    end;
    
    {$endregion constructor's}
    
    {$region Status}
    
    /// True если статус получилось изменить
    public function SetStatus(st: CommandExecutionStatus): boolean;
    begin
      Result := done.TrySet(true);
      if Result then OpenCLABCInternalException.RaiseIfError(
        cl.SetUserEventStatus(uev, st)
      );
    end;
    public function SetComplete := SetStatus(CommandExecutionStatus.COMPLETE);
    
    {$endregion Status}
    
    {$region operator's}
    
    public static function operator implicit(ev: UserEvent): cl_event := ev.uev;
    public static function operator implicit(ev: UserEvent): EventList := ev.uev;
    
    //TODO #????
//    public static function operator+(ev1: EventList; ev2: UserEvent): EventList;
//    begin
//      Result := ev1 + ev2.uev;
//      Result.abortable := true;
//    end;
//    public static procedure operator+=(ev1: EventList; ev2: UserEvent);
//    begin
//      ev1 += ev2.uev;
//      ev1.abortable := true;
//    end;
    
    public function ToString: string; override := $'UserEvent[{uev.val}]';
    
    {$endregion operator's}
    
  end;
  
{$endregion UserEvent}

{$region QueueRes}

type
  {$region Base}
  
  QueueResBase = abstract partial class
    public ev: EventList;
    public can_set_ev := true;
    
    public constructor(ev: EventList) := self.ev := ev;
    private constructor := raise new OpenCLABCInternalException;
    
    public function CloneBase(new_ev: EventList): QueueResBase; abstract;
    
    public function TrySetEvBase(new_ev: EventList): QueueResBase;
    begin
      if object.ReferenceEquals(self.ev.evs, new_ev.evs) then
        Result := self else
      if can_set_ev then
      begin
        self.ev := new_ev;
        Result := self;
      end else
        Result := self.CloneBase(new_ev);
    end;
    
    public function ThenInvokeIfDelegateRes(g: CLTaskGlobalData; need_ptr_qr: boolean): QueueResBase; abstract;
    
  end;
  
  {$endregion Base}
  
  {$region Nil}
  
  {$region General}
  
  QueueResNil = abstract partial class(QueueResBase)
    
    public function CloneBase(new_ev: EventList): QueueResBase; override := Clone(new_ev);
    public function Clone(new_ev: EventList): QueueResNil; abstract;
    
    public function TrySetEv(new_ev: EventList) := QueueResNil(TrySetEvBase(new_ev));
    
    public function ThenInvokeIfProcRes(g: CLTaskGlobalData): QueueResNil; abstract;
    public function ThenInvokeIfDelegateRes(g: CLTaskGlobalData; need_ptr_qr: boolean): QueueResBase; override := self.ThenInvokeIfProcRes(g);
    
  end;
  
  {$endregion General}
  
  {$region Const}
  
  QueueResConstNil = sealed partial class(QueueResNil)
    
    public function Clone(new_ev: EventList): QueueResNil; override := new QueueResConstNil(new_ev);
    
    public function ThenInvokeIfProcRes(g: CLTaskGlobalData): QueueResNil; override := self;
    
  end;
  
  {$endregion Const}
  
  {$region Proc}
  
  QueueResProcNil = sealed partial class(QueueResNil)
    private p: ()->();
    
    public constructor(p: ()->(); ev: EventList);
    begin
      inherited Create(ev);
      self.p := p;
    end;
    private constructor := inherited Create;
    
    public function Clone(new_ev: EventList): QueueResNil; override := new QueueResProcNil(self.p, new_ev);
    
    {$ifdef DEBUG}
    private was_invoked := false;
    {$endif DEBUG}
    public procedure Invoke;
    begin
      {$ifdef DEBUG}
      if was_invoked then raise new System.InvalidProgramException($'{self.GetType}: {System.Environment.StackTrace}');
      was_invoked := true;
      {$endif DEBUG}
      p();
    end;
    
    {$ifdef DEBUG}
    protected procedure Finalize; override :=
    if not was_invoked then raise new System.InvalidProgramException($'{self.GetType}');
    {$endif DEBUG}
    
    public function ThenInvokeIfProcRes(g: CLTaskGlobalData): QueueResNil; override;
    begin
      var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for QueueResProcNil.ThenInvokeIfProcRes'{$endif});
      
      var err_handler := g.curr_err_handler;
      self.ev.MultiAttachCallback(false, ()->
      begin
        if not err_handler.HadError(true) then
        try
          self.Invoke;
        except
          on e: Exception do err_handler.AddErr(e);
        end;
        res_ev.SetComplete;
      end{$ifdef EventDebug}, $'body of QueueResProcNil.ThenInvokeIfProcRes with res_ev={res_ev}'{$endif});
      
      
      Result := new QueueResConstNil(res_ev);
    end;
    
  end;
  
  {$endregion Proc}
  
  {$endregion Nil}
  
  {$region <T>}
  
  {$region Misc}
  
  IPtrQueueRes<T> = interface
    function GetPtr: ^T;
  end;
  QRPtrWrap<T> = sealed class(IPtrQueueRes<T>)
    private ptr: ^T := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<T>));
    
    public constructor(val: T) := self.ptr^ := val;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure Finalize; override :=
    Marshal.FreeHGlobal(new IntPtr(ptr));
    
    public function GetPtr: ^T := ptr;
    
  end;
  
  {$endregion Misc}
  
  {$region General}
  
  QueueRes<T> = abstract partial class(QueueResBase)
    
    public function GetRes: T; abstract;
    
    public function TrySetEv(new_ev: EventList) := QueueRes&<T>(TrySetEvBase(new_ev));
    
    public function CloneBase(new_ev: EventList): QueueResBase; override := Clone(new_ev);
    public function Clone(new_ev: EventList): QueueRes<T>; abstract;
    
    public function LazyQuickTransform<T2>(f: T->T2): QueueRes<T2>; abstract;
    
    // Only usable after waiting on .ev
    public function ToPtr: IPtrQueueRes<T>; abstract;
    
    public function ThenInvokeIfFuncRes(g: CLTaskGlobalData; need_ptr_qr: boolean): QueueRes<T>; abstract;
    public function ThenInvokeIfDelegateRes(g: CLTaskGlobalData; need_ptr_qr: boolean): QueueResBase; override := self.ThenInvokeIfFuncRes(g, need_ptr_qr);
    
  end;
  
  {$endregion General}
  
  {$region Const}
  
  IQueueResConst = interface end;
  // Результат который просто есть
  QueueResConst<T> = sealed partial class(QueueRes<T>, IQueueResConst)
    private res: T;
    
    public constructor(res: T; ev: EventList);
    begin
      inherited Create(ev);
      self.res := res;
    end;
    private constructor := inherited;
    
    public function Clone(new_ev: EventList): QueueRes<T>; override := new QueueResConst<T>(res, new_ev);
    
    public function GetRes: T; override := res;
    
    public function LazyQuickTransform<T2>(f: T->T2): QueueRes<T2>; override;
    
    public function ToPtr: IPtrQueueRes<T>; override := new QRPtrWrap<T>(res);
    
    public function ThenInvokeIfFuncRes(g: CLTaskGlobalData; need_ptr_qr: boolean): QueueRes<T>; override := self;
    
  end;
  
  {$endregion Const}
  
  {$region Delayed}
  
  // Результат который будет сохранён куда то, надо только дождаться
  QueueResDelayedBase<T> = abstract partial class(QueueRes<T>)
    
    public constructor := inherited Create(EventList.Empty);
    
    public function Clone(new_ev: EventList): QueueRes<T>; override;
    
    public procedure SetRes(value: T); abstract;
    
    public function LazyQuickTransform<T2>(f: T->T2): QueueRes<T2>; override;
    
    public function ThenInvokeIfFuncRes(g: CLTaskGlobalData; need_ptr_qr: boolean): QueueRes<T>; override := self;
    
  end;
  
  QueueResDelayedObj<T> = sealed partial class(QueueResDelayedBase<T>)
    private res := default(T);
    
    public function GetRes: T; override := res;
    public procedure SetRes(value: T); override := res := value;
    
    public function ToPtr: IPtrQueueRes<T>; override := new QRPtrWrap<T>(res);
    
  end;
  
  IQueueResDelayedPtr = interface end;
  QueueResDelayedPtr<T> = sealed partial class(QueueResDelayedBase<T>, IPtrQueueRes<T>, IQueueResDelayedPtr)
    private ptr: ^T := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<T>));
    
    static constructor :=
    BlittableHelper.RaiseIfBad(typeof(T), 'использовать в некоторой внутренней ситуации (напишите об этом в issue)');
    
    public constructor(res: T; ev: EventList);
    begin
      inherited Create(ev);
      self.ptr^ := res;
    end;
    public constructor := inherited Create;
    
    public function GetPtr: ^T := ptr;
    public function GetRes: T; override := ptr^;
    public procedure SetRes(value: T); override := ptr^ := value;
    
    protected procedure Finalize; override :=
    Marshal.FreeHGlobal(new IntPtr(ptr));
    
    public function ToPtr: IPtrQueueRes<T>; override := self;
    
  end;
  
  QueueRes<T> = abstract partial class(QueueResBase)
    
    public static function MakeNewConstOrPtr(need_ptr_qr: boolean; res: T; ev: EventList) :=
    if need_ptr_qr then
      new QueueResDelayedPtr<T>(res, ev) as QueueRes<T> else
      new QueueResConst<T>(res, ev) as QueueRes<T>;
    
    public static function MakeNewDelayedOrPtr(need_ptr_qr: boolean) :=
    if need_ptr_qr then
      new QueueResDelayedPtr<T> as QueueResDelayedBase<T> else
      new QueueResDelayedObj<T> as QueueResDelayedBase<T>;
    
  end;
  
  {$endregion Delayed}
  
  {$region Func}
  
  // Результат который надо будет сначала дождаться, а потом ещё досчитать
  QueueResFunc<T> = sealed partial class(QueueRes<T>)
    private f: ()->T;
    
    public constructor(f: ()->T; ev: EventList);
    begin
      inherited Create(ev);
      self.f := f;
    end;
    private constructor := inherited;
    
    public function Clone(new_ev: EventList): QueueRes<T>; override := new QueueResFunc<T>(f, new_ev);
    
    {$ifdef DEBUG}
    private was_invoked := false;
    {$endif DEBUG}
    public function GetRes: T; override;
    begin
      {$ifdef DEBUG}
      if was_invoked then raise new System.InvalidProgramException($'{self.GetType}: {System.Environment.StackTrace}');
      was_invoked := true;
      {$endif DEBUG}
      Result := f();
    end;
    
    {$ifdef DEBUG}
    protected procedure Finalize; override :=
    if not was_invoked then raise new System.InvalidProgramException($'{self.GetType}');
    {$endif DEBUG}
    
    public function LazyQuickTransform<T2>(f2: T->T2): QueueRes<T2>; override :=
    new QueueResFunc<T2>(()->f2(self.GetRes), self.ev);
    
    public function ToPtr: IPtrQueueRes<T>; override := new QRPtrWrap<T>(self.GetRes);
    
    public function ThenInvokeIfFuncRes(g: CLTaskGlobalData; need_ptr_qr: boolean): QueueRes<T>; override;
    begin
      var res := QueueRes&<T>.MakeNewDelayedOrPtr(need_ptr_qr);
      var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for QueueResFunc<{typeof(T)}>.ThenInvokeIfFuncRes'{$endif});
      res.ev := res_ev;
      
      var err_handler := g.curr_err_handler;
      self.ev.MultiAttachCallback(false, ()->
      begin
        if not err_handler.HadError(true) then
        try
          res.SetRes(self.GetRes);
        except
          on e: Exception do err_handler.AddErr(e);
        end;
        res_ev.SetComplete;
      end{$ifdef EventDebug}, $'body of QueueResFunc<{typeof(T)}>.ThenInvokeIfFuncRes with res_ev={res_ev}'{$endif});
      
      Result := res;
    end;
    
  end;
  
  {$endregion Func}
  
  {$endregion <T>}
  
function QueueResConst<T>.LazyQuickTransform<T2>(f: T->T2): QueueRes<T2>;
begin
  try
    var n_res := f(self.res);
    Result := new QueueResConst<T2>(n_res, self.ev);
  except
    on e: Exception do
    begin
      var edi := System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e);
      Result := new QueueResFunc<T2>(()->
      begin
        Result := default(T2);
        edi.Throw;
      end, self.ev);
    end;
  end;
end;

function QueueResDelayedBase<T>.Clone(new_ev: EventList) :=
new QueueResFunc<T>(self.GetRes, new_ev);

function QueueResDelayedBase<T>.LazyQuickTransform<T2>(f: T->T2) :=
new QueueResFunc<T2>(()->f(self.GetRes()), self.ev);

{$endregion QueueRes}

{$region MultiusableBase}

type
  IMultiusableCommandQueueHub = interface end;
  MultiuseableResultData = record
    public qres: QueueResBase;
    public err_handler: CLTaskErrHandler;
    
    public constructor(qres: QueueResBase; err_handler: CLTaskErrHandler);
    begin
      self.qres := qres;
      self.err_handler := err_handler;
    end;
    
  end;
  
{$endregion MultiusableBase}

{$region CLTaskData}

type
  ICLTaskLocalData = interface
    property PrevEv: EventList read write;
    property NeedPtrQr: boolean read;
  end;
  
  CLTaskLocalData = record(ICLTaskLocalData)
    public need_ptr_qr := false;
    public prev_ev := EventList.Empty;
    
    //TODO #2607
    public property ICLTaskLocalData.PrevEv: EventList read EventList(prev_ev) write prev_ev := value;
    public property ICLTaskLocalData.NeedPtrQr: boolean read boolean(need_ptr_qr);
    
    public procedure CheckInvalidNeedPtrQr(source: object) :=
    if need_ptr_qr then raise new OpenCLABCInternalException($'{source.GetType} with need_ptr_qr');
    
  end;
  CLTaskLocalDataNil = record(ICLTaskLocalData)
    public prev_ev := EventList.Empty;
    
    //TODO #2607
    public property ICLTaskLocalData.PrevEv: EventList read EventList(prev_ev) write prev_ev := value;
    public property ICLTaskLocalData.NeedPtrQr: boolean read boolean(false);
    
    public static function operator explicit(l: CLTaskLocalData): CLTaskLocalDataNil;
    begin
      Result.prev_ev := l.prev_ev;
    end;
    
  end;
  
function WithPtrNeed<TLData>(self: TLData; need_ptr_qr: boolean): CLTaskLocalData; extensionmethod; where TLData: ICLTaskLocalData;
begin
  Result.need_ptr_qr := need_ptr_qr;
  Result.prev_ev := self.PrevEv;
end;

type
  CLTaskBranchInvoker<TLData> = sealed class
  where TLData: ICLTaskLocalData;
    private prev_cq: cl_command_queue;
    private g: CLTaskGlobalData;
    private l: TLData;
    private branch_handlers := new List<CLTaskErrHandler>;
    private make_base_err_handler: ()->CLTaskErrHandler;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    constructor(g: CLTaskGlobalData; l: TLData; as_new: boolean; capacity: integer);
    begin
      self.prev_cq := if as_new then g.curr_inv_cq else cl_command_queue.Zero;
      self.g := g;
      self.l := l;
      self.branch_handlers.Capacity := capacity;
      if as_new then
        self.make_base_err_handler := ()->new CLTaskErrHandlerEmpty else
      begin
        var origin_handler := g.curr_err_handler;
        self.make_base_err_handler := ()->new CLTaskErrHandlerBranchBase(origin_handler);
      end;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    function InvokeBranch<TR>(branch: (CLTaskGlobalData, TLData)->TR): TR; where TR: QueueResBase;
    begin
      g.curr_err_handler := make_base_err_handler();
      
      Result := branch(g, l);
      
      var cq := g.curr_inv_cq;
      if cq<>cl_command_queue.Zero then
      begin
        g.curr_inv_cq := cl_command_queue.Zero;
        if prev_cq=cl_command_queue.Zero then
          prev_cq := cq else
          Result.ev.MultiAttachCallback(true, ()->
          begin
            {$ifdef QueueDebug}
            QueueDebug.Add(cq, '----- return -----');
            {$endif QueueDebug}
            g.free_cqs.Add(cq);
          end{$ifdef EventDebug}, $'returning cq to bag'{$endif});
      end;
      
      // Как можно позже, потому что вызовы использующие
      // err_handler могут заменять его на новый, свой собственный
      branch_handlers += g.curr_err_handler;
    end;
    
  end;
  
  CLTaskGlobalData = sealed partial class
    
    public mu_res := new Dictionary<IMultiusableCommandQueueHub, MultiuseableResultData>;
    
    public constructor(tsk: CLTaskBase);
    begin
      self.tsk := tsk;
      
      self.c := tsk.OrgContext;
      self.cl_c := c.ntv;
      self.cl_dvc := c.main_dvc.ntv;
      
    end;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    procedure ParallelInvoke<TLData>(l: TLData; as_new: boolean; capacity: integer; use: CLTaskBranchInvoker<TLData>->()); where TLData: ICLTaskLocalData;
    begin
      var invoker := new CLTaskBranchInvoker<TLData>(self, l, as_new, capacity);
      var origin_handler := self.curr_err_handler;
      
      // Только в случае A + B*C, то есть "not as_new", можно использовать curr_inv_cq - и только как outer_cq
      if not as_new and (curr_inv_cq<>cl_command_queue.Zero) then
      begin
        {$ifdef DEBUG}
        if outer_cq<>cl_command_queue.Zero then raise new OpenCLABCInternalException($'OuterCQ confusion');
        {$endif DEBUG}
        outer_cq := curr_inv_cq;
      end;
      curr_inv_cq := cl_command_queue.Zero;
      
      use(invoker);
      
      {$ifdef DEBUG}
      if invoker.branch_handlers.Count<>capacity then
        raise new OpenCLABCInternalException($'{invoker.branch_handlers.Count} <> {capacity}');
      {$endif DEBUG}
      self.curr_err_handler := new CLTaskErrHandlerBranchCombinator(origin_handler, invoker.branch_handlers.ToArray);
      
      self.curr_inv_cq := invoker.prev_cq;
    end;
    
    public procedure FinishInvoke;
    begin
      
      // mu выполняют лишний .Retain, чтобы ивент не удалился пока очередь ещё запускается
      foreach var mrd in mu_res.Values do
        mrd.qres.ev.Release({$ifdef EventDebug}$'excessive mu ev'{$endif});
      mu_res := nil;
      
    end;
    
    public procedure FinishExecution(var err_lst: List<Exception>);
    begin
      
      if curr_inv_cq<>cl_command_queue.Zero then
      begin
        {$ifdef QueueDebug}
        QueueDebug.Add(curr_inv_cq, '----- last q -----');
        {$endif QueueDebug}
        free_cqs.Add(curr_inv_cq);
      end;
      
      foreach var cq in free_cqs do
        OpenCLABCInternalException.RaiseIfError( cl.ReleaseCommandQueue(cq) );
      
      err_lst := new List<Exception>;
      curr_err_handler.FillErrLst(err_lst);
      {$ifdef DEBUG}
      curr_err_handler.SanityCheck(err_lst);
      {$endif DEBUG}
    end;
    
  end;
  
{$endregion CLTaskData}

{$endregion Util type's}

{$region CommandQueue}

{$region Base}

type
  CommandQueueBase = abstract partial class
    
    protected function InvokeBase(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResBase; abstract;
    
    /// Добавление tsk в качестве ключа для всех ожидаемых очередей
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); abstract;
    
  end;
  
  CommandQueueNil = abstract partial class(CommandQueueBase)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueResNil; abstract;
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;
    begin
      {$ifdef DEBUG}
      l.CheckInvalidNeedPtrQr(self);
      {$endif DEBUG}
      Result := Invoke(g, CLTaskLocalDataNil(l));
    end;
    protected function InvokeBase(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResBase; override := Invoke(g, l);
    
  end;
  
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; abstract;
    protected function InvokeBase(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResBase; override :=
    Invoke(g, l);
    
  end;
  
{$endregion Base}

{$region Const}

type
  ConstQueue<T> = sealed partial class(CommandQueue<T>)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override :=
    QueueRes&<T>.MakeNewConstOrPtr(l.need_ptr_qr, self.res, l.prev_ev);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
  end;
  
{$endregion Const}

{$region Host}

type
  /// очередь, выполняющая какую то работу на CPU, всегда в отдельном потоке
  HostQueue<TInp,TRes> = abstract class(CommandQueue<TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<TInp>; abstract;
    
    protected function ExecFunc(o: TInp; c: Context): TRes; abstract;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var prev_qr := InvokeSubQs(g, CLTaskLocalDataNil(l));
      var c := g.c;
      
      var qr := QueueResDelayedBase&<TRes>.MakeNewDelayedOrPtr(l.need_ptr_qr);
      qr.ev := UserEvent.StartBackgroundWork(prev_qr.ev, ()->qr.SetRes( ExecFunc(prev_qr.GetRes(), c) ), g
        {$ifdef EventDebug}, $'body of {self.GetType}'{$endif}
      );
      
      Result := qr;
    end;
    
  end;
  
{$endregion Host}

{$endregion CommandQueue}

{$region CLTask}

type
  CLTaskBase = abstract partial class
    
  end;
  
  CLTaskNil = sealed partial class(CLTaskBase)
    
    private constructor(q: CommandQueueNil; c: Context);
    begin
      self.q := q;
      self.org_c := c;
      
      var g_data := new CLTaskGlobalData(self);
      var l_data := new CLTaskLocalDataNil;
      
      q.RegisterWaitables(g_data, new HashSet<IMultiusableCommandQueueHub>);
      var res_ev := q.Invoke(g_data, l_data).ThenInvokeIfProcRes(g_data).ev;
      g_data.FinishInvoke;
      
      NativeUtils.StartNewBgThread(()->
      begin
        res_ev.WaitAndRelease({$ifdef EventDebug}$'CLTaskNil.FinishExecution'{$endif});
        g_data.FinishExecution(self.err_lst);
        wh.Set;
      end);
      
    end;
    
  end;
  CLTask<T> = sealed partial class(CLTaskBase)
    private q_res: QueueRes<T>;
    
    private constructor(q: CommandQueue<T>; c: Context);
    begin
      self.q := q;
      self.org_c := c;
      
      var g_data := new CLTaskGlobalData(self);
      var l_data := new CLTaskLocalData;
      
      q.RegisterWaitables(g_data, new HashSet<IMultiusableCommandQueueHub>);
      self.q_res := q.Invoke(g_data, l_data).ThenInvokeIfFuncRes(g_data, false);
      g_data.FinishInvoke;
      
      NativeUtils.StartNewBgThread(()->
      begin
        self.q_res.ev.WaitAndRelease({$ifdef EventDebug}$'CLTask<T>.FinishExecution'{$endif});
        g_data.FinishExecution(self.err_lst);
        wh.Set;
      end);
      
    end;
    
  end;
  
  CLTaskFactory = record(ITypedCQConverter<CLTaskBase>)
    private c: Context;
    public constructor(c: Context) := self.c := c;
    public constructor := raise new OpenCLABCInternalException;
    
    public function ConvertNil(cq: CommandQueueNil): CLTaskBase := new CLTaskNil(cq, c);
    public function Convert<T>(cq: CommandQueue<T>): CLTaskBase := new CLTask<T>(cq, c);
    
  end;
  
function Context.BeginInvoke(q: CommandQueueBase) := q.ConvertTyped(new CLTaskFactory(self));
function Context.BeginInvoke(q: CommandQueueNil) := new CLTaskNil(q, self);
function Context.BeginInvoke<T>(q: CommandQueue<T>) := new CLTask<T>(q, self);

function CLTask<T>.WaitRes: T;
begin
  Wait;
  Result := q_res.GetRes;
end;

{$endregion CLTask}

{$region Queue converter's}

{$region Cast}

type
  TypedNilQueue<T> = sealed class(CommandQueue<T>)
    private static nil_val := default(T);
    private q: CommandQueueNil;
    
    static constructor;
    begin
      if object(nil_val)<>nil then
        raise new System.InvalidCastException($'.Cast не может преобразовывать nil в {typeof(T)}');
    end;
    public constructor(q: CommandQueueNil) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := q.RegisterWaitables(g, prev_hubs);
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override := new QueueResConst<T>(nil_val, q.Invoke(g, l).ThenInvokeIfProcRes(g).ev);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  CastQueueBase<TRes> = abstract class(CommandQueue<TRes>)
    
    public property SourceBase: CommandQueueBase read; abstract;
    
  end;
  
  CastQueue<TInp, TRes> = sealed class(CastQueueBase<TRes>)
    private q: CommandQueue<TInp>;
    
    static constructor;
    begin
      if typeof(TInp)=typeof(object) then exit;
      try
        var res := TRes(object(default(TInp)));
        System.GC.KeepAlive(res);
      except
        raise new System.InvalidCastException($'.Cast не может преобразовывать {typeof(TInp)} в {typeof(TRes)}');
      end;
    end;
    public constructor(q: CommandQueue<TInp>) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    public property SourceBase: CommandQueueBase read q as CommandQueueBase; override;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var err_handler := g.curr_err_handler;
      Result := q.Invoke(g, l.WithPtrNeed(false)).LazyQuickTransform(o->
      try
        Result := TRes(object(o));
      except
        on e: Exception do
          err_handler.AddErr(e);
      end);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  CastQueueFactory<TRes> = record(ITypedCQConverter<CommandQueue<TRes>>)
    
    public function ConvertNil(cq: CommandQueueNil): CommandQueue<TRes> := new TypedNilQueue<TRes>(cq);
    public function Convert<TInp>(cq: CommandQueue<TInp>): CommandQueue<TRes>;
    begin
      if cq is CastQueueBase<TInp>(var cqb) then
        Result := cqb.SourceBase.Cast&<TRes> else
      if cq is ConstQueue<TInp>(var ccq) then
      try
        Result := new ConstQueue<TRes>(TRes(object(ccq.Val)))
      except
        on e: InvalidCastException do
          raise new System.InvalidCastException($'.Cast не может преобразовывать {typeof(TInp)} в {typeof(TRes)}');
      end else
        Result := new CastQueue<TInp, TRes>(cq);
    end;
    
  end;
  
function CommandQueueBase.Cast<T>: CommandQueue<T>;
begin
  if self is CommandQueue<T>(var tcq) then
    Result := tcq else
  try
    Result := self.ConvertTyped(new CastQueueFactory<T>);
  except
    on e: TypeInitializationException do
      raise e.InnerException;
    on e: InvalidCastException do
      raise e;
  end;
end;

{$endregion Cast}

{$region ThenBackgroundConvert}

type
  CommandQueueThenBackgroundConvertBase<TInp, TRes, TFunc> = abstract class(HostQueue<TInp, TRes>)
  where TFunc: Delegate;
    private q: CommandQueue<TInp>;
    private f: TFunc;
    
    public constructor(q: CommandQueue<TInp>; f: TFunc);
    begin
      self.q := q;
      self.f := f;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<TInp>; override := q.Invoke(g, l.WithPtrNeed(false));
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      q.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, f);
      sb += #10;
      
    end;
    
  end;
  
  CommandQueueThenBackgroundConvert<TInp, TRes> = sealed class(CommandQueueThenBackgroundConvertBase<TInp, TRes, TInp->TRes>)
    
    protected function ExecFunc(o: TInp; c: Context): TRes; override := f(o);
    
  end;
  CommandQueueThenBackgroundConvertC<TInp, TRes> = sealed class(CommandQueueThenBackgroundConvertBase<TInp, TRes, (TInp, Context)->TRes>)
    
    protected function ExecFunc(o: TInp; c: Context): TRes; override := f(o, c);
    
  end;
  
function CommandQueue<T>.ThenConvert<TOtp>(f: T->TOtp) :=
new CommandQueueThenBackgroundConvert<T, TOtp>(self, f);

function CommandQueue<T>.ThenConvert<TOtp>(f: (T, Context)->TOtp) :=
new CommandQueueThenBackgroundConvertC<T, TOtp>(self, f);

{$endregion ThenConvert}

{$region ThenUse}

type
  CommandQueueThenBackgroundUseBase<T, TProc> = abstract class(CommandQueue<T>)
  where TProc: Delegate;
    private q: CommandQueue<T>;
    private p: TProc;
    
    public constructor(q: CommandQueue<T>; p: TProc);
    begin
      self.q := q;
      self.p := p;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    protected procedure ExecProc(o: T; c: Context); abstract;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      var prev_qr := q.Invoke(g, l);
      var c := g.c;
      
      var err_handler := g.curr_err_handler;
      // QueueResFunc.GetRes shouldn't be called twice
      if prev_qr is QueueResFunc<T>(var prev_f_qr) then
      begin
        var qr := QueueRes&<T>.MakeNewDelayedOrPtr(l.need_ptr_qr);
        qr.ev := UserEvent.StartBackgroundWork(prev_f_qr.ev, ()->
        if not err_handler.HadError(true) then
        begin
          var res := prev_f_qr.GetRes;
          ExecProc(res, c);
          qr.SetRes(res);
        end, g{$ifdef EventDebug}, $'body of {self.GetType}'{$endif});
        Result := qr;
      end else
        Result := prev_qr.TrySetEv(
          UserEvent.StartBackgroundWork(prev_qr.ev, ()->
            if not err_handler.HadError(true) then
              ExecProc(prev_qr.GetRes, c),
          g{$ifdef EventDebug}, $'body of {self.GetType}'{$endif})
        );
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      q.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, p);
      sb += #10;
      
    end;
    
  end;
  
  CommandQueueThenBackgroundUse<T> = sealed class(CommandQueueThenBackgroundUseBase<T, T->()>)
    
    protected procedure ExecProc(o: T; c: Context); override := p(o);
    
  end;
  CommandQueueThenBackgroundUseC<T> = sealed class(CommandQueueThenBackgroundUseBase<T, (T, Context)->()>)
    
    protected procedure ExecProc(o: T; c: Context); override := p(o, c);
    
  end;
  
function CommandQueue<T>.ThenUse(p: T->()): CommandQueue<T> :=
new CommandQueueThenBackgroundUse<T>(self, p);

function CommandQueue<T>.ThenUse(p: (T, Context)->()): CommandQueue<T> :=
new CommandQueueThenBackgroundUseC<T>(self, p);

{$endregion ThenUse}

{$region ThenQuickConvert}

type
  CommandQueueThenQuickConvertBase<TInp, TRes, TFunc> = abstract class(CommandQueue<TRes>)
  where TFunc: Delegate;
    private q: CommandQueue<TInp>;
    private f: TFunc;
    
    public constructor(q: CommandQueue<TInp>; f: TFunc);
    begin
      self.q := q;
      self.f := f;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function ExecFunc(o: TInp; c: Context): TRes; abstract;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var c := g.c;
      Result := q.Invoke(g, l).LazyQuickTransform(o->ExecFunc(o, c));
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      q.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, f);
      sb += #10;
      
    end;
    
  end;
  
  CommandQueueThenQuickConvert<TInp, TRes> = sealed class(CommandQueueThenQuickConvertBase<TInp, TRes, TInp->TRes>)
    
    protected function ExecFunc(o: TInp; c: Context): TRes; override := f(o);
    
  end;
  CommandQueueThenQuickConvertC<TInp, TRes> = sealed class(CommandQueueThenQuickConvertBase<TInp, TRes, (TInp, Context)->TRes>)
    
    protected function ExecFunc(o: TInp; c: Context): TRes; override := f(o, c);
    
  end;
  
function CommandQueue<T>.ThenQuickConvert<TOtp>(f: T->TOtp) :=
new CommandQueueThenQuickConvert<T, TOtp>(self, f);

function CommandQueue<T>.ThenQuickConvert<TOtp>(f: (T, Context)->TOtp) :=
new CommandQueueThenQuickConvertC<T, TOtp>(self, f);

{$endregion ThenQuickConvert}

{$region ThenQuickUse}

type
  CommandQueueThenQuickUseBase<T, TProc> = abstract class(CommandQueue<T>)
  where TProc: Delegate;
    private q: CommandQueue<T>;
    private p: TProc;
    
    public constructor(q: CommandQueue<T>; p: TProc);
    begin
      self.q := q;
      self.p := p;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure ExecProc(o: T; c: Context); abstract;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      var prev_qr := q.Invoke(g, l).ThenInvokeIfFuncRes(g, l.need_ptr_qr);
      
      var c := g.c;
      var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for {self.GetType}'{$endif});
      var err_handler := g.curr_err_handler;
      prev_qr.ev.MultiAttachCallback(false, ()->
      begin
        if not err_handler.HadError(true) then
        try
          ExecProc(prev_qr.GetRes, c);
        except
          on e: Exception do err_handler.AddErr(e);
        end;
        res_ev.SetComplete;
      end{$ifdef EventDebug}, $'body of {self.GetType}'{$endif});
      
      Result := prev_qr.TrySetEv(res_ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      q.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, p);
      sb += #10;
      
    end;
    
  end;
  
  CommandQueueThenQuickUse<T> = sealed class(CommandQueueThenQuickUseBase<T, T->()>)
    
    protected procedure ExecProc(o: T; c: Context); override := p(o);
    
  end;
  CommandQueueThenQuickUseC<T> = sealed class(CommandQueueThenQuickUseBase<T, (T, Context)->()>)
    
    protected procedure ExecProc(o: T; c: Context); override := p(o, c);
    
  end;
  
function CommandQueue<T>.ThenQuickUse(p: T->()) :=
new CommandQueueThenQuickUse<T>(self, p);

function CommandQueue<T>.ThenQuickUse(p: (T, Context)->()) :=
new CommandQueueThenQuickUseC<T>(self, p);

{$endregion ThenQuickUse}

{$region +/*}

{$region Simple}

type
  SimpleQueueArrayCommon<TQ> = record
  where TQ: CommandQueueBase;
    public qs: array of CommandQueueBase;
    public last: TQ;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    function GetQS: sequence of CommandQueueBase := qs.Append&<CommandQueueBase>(last);
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>);
    begin
      foreach var q in qs do q.RegisterWaitables(g, prev_hubs);
      last.RegisterWaitables(g, prev_hubs);
    end;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    function InvokeSync<TLData,TR>(g: CLTaskGlobalData; l: TLData; invoke_last: (CLTaskGlobalData,TLData)->TR): TR; where TLData: ICLTaskLocalData; where TR: QueueResBase;
    begin
      for var i := 0 to qs.Length-1 do
        l.PrevEv := qs[i].InvokeBase(g, l.WithPtrNeed(false)).ThenInvokeIfDelegateRes(g, false).ev;
      
      Result := invoke_last(g, l);
    end;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    function InvokeAsync<TLData,TR>(g: CLTaskGlobalData; l: TLData; invoke_last: (CLTaskGlobalData,TLData)->TR): TR; where TLData: ICLTaskLocalData; where TR: QueueResBase;
    begin
      if l.PrevEv.count<>0 then loop qs.Length do
        l.PrevEv.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var evs := new EventList[qs.Length+1];
      
      var res: TR;
      g.ParallelInvoke(l, false, qs.Length+1, invoker->
      begin
        for var i := 0 to qs.Length-1 do
          //TODO #2610
          evs[i] := invoker.InvokeBranch&<QueueResBase>((g,l)->
            qs[i].InvokeBase(g, l.WithPtrNeed(false)).ThenInvokeIfDelegateRes(g, false)
          ).ev;
        var l_res := invoker.InvokeBranch(invoke_last);
        res := l_res;
        evs[qs.Length] := l_res.ev;
      end);
      
      Result := TR(res.TrySetEvBase( EventList.Combine(evs) ));
    end;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>);
    begin
      sb += #10;
      foreach var q in qs do
        q.ToString(sb, tabs, index, delayed);
      last.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  ISimpleQueueArray = interface
    function GetQS: sequence of CommandQueueBase;
  end;
  ISimpleSyncQueueArray = interface(ISimpleQueueArray) end;
  ISimpleAsyncQueueArray = interface(ISimpleQueueArray) end;
  
  SimpleQueueArrayNil = abstract class(CommandQueueNil, ISimpleQueueArray)
    protected data := new SimpleQueueArrayCommon< CommandQueueNil >;
    
    public constructor(qs: array of CommandQueueBase; last: CommandQueueNil);
    begin
      data.qs := qs;
      data.last := last;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    public function GetQS: sequence of CommandQueueBase := data.GetQS;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    data.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    data.ToString(sb, tabs, index, delayed);
    
  end;
  
  SimpleSyncQueueArrayNil = sealed class(SimpleQueueArrayNil, ISimpleSyncQueueArray)
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueResNil; override := data.InvokeSync(g, l, data.last.Invoke);
  end;
  SimpleAsyncQueueArrayNil = sealed class(SimpleQueueArrayNil, ISimpleAsyncQueueArray)
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueResNil; override := data.InvokeAsync(g, l, data.last.Invoke);
  end;
  
  SimpleQueueArray<T> = abstract class(CommandQueue<T>, ISimpleQueueArray)
    protected data := new SimpleQueueArrayCommon< CommandQueue<T> >;
    
    public constructor(qs: array of CommandQueueBase; last: CommandQueue<T>);
    begin
      data.qs := qs;
      data.last := last;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    public function GetQS: sequence of CommandQueueBase := data.GetQS;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    data.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    data.ToString(sb, tabs, index, delayed);
    
  end;
  
  SimpleSyncQueueArray<T> = sealed class(SimpleQueueArray<T>, ISimpleSyncQueueArray)
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override := data.InvokeSync(g, l, data.last.Invoke);
  end;
  SimpleAsyncQueueArray<T> = sealed class(SimpleQueueArray<T>, ISimpleAsyncQueueArray)
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override := data.InvokeAsync(g, l, data.last.Invoke);
  end;
  
{$endregion Simple}

{$region Conv}

{$region Generic}

type
  
  {$region Background}
  
  {$region Base}
  
  BackgroundConvQueueArrayBase<TInp, TRes, TFunc> = abstract class(HostQueue<array of TInp, TRes>)
  where TFunc: Delegate;
    protected qs: array of CommandQueue<TInp>;
    protected f: TFunc;
    
    public constructor(qs: array of CommandQueue<TInp>; f: TFunc);
    begin
      self.qs := qs;
      self.f := f;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function CombineQRs(qrs: array of QueueRes<TInp>; ev: EventList): QueueRes<array of TInp>;
    begin
      if qrs.All(qr->qr is QueueResConst<TInp>) then
      begin
        var res := qrs.ConvertAll(qr->QueueResConst&<TInp>(qr).res);
        Result := new QueueResConst<array of TInp>(res, ev);
      end else
        Result := new QueueResFunc<array of TInp>(()->
        begin
          Result := new TInp[qrs.Length];
          for var i := 0 to qrs.Length-1 do
            Result[i] := qrs[i].GetRes;
        end, ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    foreach var q in qs do q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      foreach var q in qs do
        q.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, f);
      sb += #10;
    end;
    
  end;
  
  {$endregion Base}
  
  {$region Sync}
  
  BackgroundConvSyncQueueArrayBase<TInp, TRes, TFunc> = abstract class(BackgroundConvQueueArrayBase<TInp, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<array of TInp>; override;
    begin
      var qrs := new QueueRes<TInp>[qs.Length];
      
      for var i := 0 to qs.Length-1 do
      begin
        var qr := qs[i].Invoke(g, l.WithPtrNeed(false));
        l.prev_ev := qr.ev;
        qrs[i] := qr;
      end;
      
      Result := CombineQRs(qrs, l.prev_ev);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray<TInp, TRes> = sealed class(BackgroundConvSyncQueueArrayBase<TInp, TRes, Func<array of TInp, TRes>>)
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; override := f(o);
    
  end;
  BackgroundConvSyncQueueArrayC<TInp, TRes> = sealed class(BackgroundConvSyncQueueArrayBase<TInp, TRes, Func<array of TInp, Context, TRes>>)
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; override := f(o, c);
    
  end;
  
  {$endregion Sync}
  
  {$region Async}
  
  BackgroundConvAsyncQueueArrayBase<TInp, TRes, TFunc> = abstract class(BackgroundConvQueueArrayBase<TInp, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<array of TInp>; override;
    begin
      if l.prev_ev.count<>0 then loop qs.Length-1 do
        l.prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qrs := new QueueRes<TInp>[qs.Length];
      var evs := new EventList[qs.Length];
      
      g.ParallelInvoke(l.WithPtrNeed(false), false, qs.Length, invoker->
      for var i := 0 to qs.Length-1 do
      begin
        var qr := invoker.InvokeBranch(qs[i].Invoke);
        qrs[i] := qr;
        evs[i] := qr.ev;
      end);
      
      var res_ev := EventList.Combine(evs);
      Result := CombineQRs(qrs, res_ev);
    end;
    
  end;
  
  BackgroundConvAsyncQueueArray<TInp, TRes> = sealed class(BackgroundConvAsyncQueueArrayBase<TInp, TRes, Func<array of TInp, TRes>>)
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; override := f(o);
    
  end;
  BackgroundConvAsyncQueueArrayC<TInp, TRes> = sealed class(BackgroundConvAsyncQueueArrayBase<TInp, TRes, Func<array of TInp, Context, TRes>>)
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; override := f(o, c);
    
  end;
  
  {$endregion Async}
  
  {$endregion Background}
  
  {$region Quick}
  
  {$region Base}
  
  QuickConvQueueArrayBase<TInp, TRes, TFunc> = abstract class(CommandQueue<TRes>)
  where TFunc: Delegate;
    protected qs: array of CommandQueue<TInp>;
    protected f: TFunc;
    
    public constructor(qs: array of CommandQueue<TInp>; f: TFunc);
    begin
      self.qs := qs;
      self.f := f;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function CombineQRs(qrs: array of QueueRes<TInp>; ev: EventList; need_ptr_qr: boolean; c: Context): QueueRes<TRes>;
    begin
      if qrs.All(qr->qr is QueueResConst<TInp>) then
      begin
        var res := ExecFunc(qrs.ConvertAll(qr->QueueResConst&<TInp>(qr).res), c);
        Result := QueueResConst&<TRes>.MakeNewConstOrPtr(need_ptr_qr, res, ev);
      end else
        Result := new QueueResFunc<TRes>(()->
        begin
          var res := new TInp[qrs.Length];
          for var i := 0 to qrs.Length-1 do
            res[i] := qrs[i].GetRes;
          Result := ExecFunc(res, c);
        end, ev);
    end;
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    foreach var q in qs do q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      foreach var q in qs do
        q.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, f);
      sb += #10;
    end;
    
  end;
  
  {$endregion Base}
  
  {$region Sync}
  
  QuickConvSyncQueueArrayBase<TInp, TRes, TFunc> = abstract class(QuickConvQueueArrayBase<TInp, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var qrs := new QueueRes<TInp>[qs.Length];
      
      for var i := 0 to qs.Length-1 do
      begin
        var qr := qs[i].Invoke(g, l.WithPtrNeed(false));
        l.prev_ev := qr.ev;
        qrs[i] := qr;
      end;
      
      Result := CombineQRs(qrs, l.prev_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvSyncQueueArray<TInp, TRes> = sealed class(QuickConvSyncQueueArrayBase<TInp, TRes, Func<array of TInp, TRes>>)
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; override := f(o);
    
  end;
  QuickConvSyncQueueArrayC<TInp, TRes> = sealed class(QuickConvSyncQueueArrayBase<TInp, TRes, Func<array of TInp, Context, TRes>>)
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; override := f(o, c);
    
  end;
  
  {$endregion Sync}
  
  {$region Async}
  
  QuickConvAsyncQueueArrayBase<TInp, TRes, TFunc> = abstract class(QuickConvQueueArrayBase<TInp, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      if l.prev_ev.count<>0 then loop qs.Length-1 do
        l.prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qrs := new QueueRes<TInp>[qs.Length];
      var evs := new EventList[qs.Length];
      
      g.ParallelInvoke(l.WithPtrNeed(false), false, qs.Length, invoker->
      for var i := 0 to qs.Length-1 do
      begin
        var qr := invoker.InvokeBranch(qs[i].Invoke);
        qrs[i] := qr;
        evs[i] := qr.ev;
      end);
      
      var res_ev := EventList.Combine(evs);
      Result := CombineQRs(qrs, res_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvAsyncQueueArray<TInp, TRes> = sealed class(QuickConvAsyncQueueArrayBase<TInp, TRes, Func<array of TInp, TRes>>)
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; override := f(o);
    
  end;
  QuickConvAsyncQueueArrayC<TInp, TRes> = sealed class(QuickConvAsyncQueueArrayBase<TInp, TRes, Func<array of TInp, Context, TRes>>)
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; override := f(o, c);
    
  end;
  
  {$endregion Async}
  
  {$endregion Quick}
  
{$endregion Generic}

{$region [2]}

type
  BackgroundConvQueueArray2Base<TInp1, TInp2, TRes, TFunc> = abstract class(HostQueue<ValueTuple<TInp1, TInp2>, TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; f: TFunc);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; ev: EventList): QueueRes<ValueTuple<TInp1, TInp2>>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2));
      if all_const then
      begin
        var res := ValueTuple.Create(cqr1.GetRes(), cqr2.GetRes());
        Result := new QueueResConst<ValueTuple<TInp1, TInp2>>(res, ev);
      end else
        Result := new QueueResFunc<ValueTuple<TInp1, TInp2>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes()), ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  QuickConvQueueArray2Base<TInp1, TInp2, TRes, TFunc> = abstract class(CommandQueue<TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; f: TFunc);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; ev: EventList; need_ptr_qr: boolean; c: Context): QueueRes<TRes>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2));
      if all_const then
      begin
        var res := ExecFunc(cqr1.GetRes(), cqr2.GetRes(), c);
        Result := QueueRes&<TRes>.MakeNewConstOrPtr(need_ptr_qr, res, ev);
      end else
        Result := new QueueResFunc<TRes>(()->ExecFunc(qr1.GetRes(), qr2.GetRes(), c), ev);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; c: Context): TRes; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray2Base<TInp1, TInp2, TRes, TFunc> = abstract class(BackgroundConvQueueArray2Base<TInp1, TInp2, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      Result := CombineQRs(qr1, qr2, l.prev_ev);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray2<TInp1, TInp2, TRes> = sealed class(BackgroundConvSyncQueueArray2Base<TInp1, TInp2, TRes, (TInp1, TInp2)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2>; c: Context): TRes; override := f(t.Item1, t.Item2);
    
  end;
  BackgroundConvSyncQueueArray2C<TInp1, TInp2, TRes> = sealed class(BackgroundConvSyncQueueArray2Base<TInp1, TInp2, TRes, (TInp1, TInp2, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2>; c: Context): TRes; override := f(t.Item1, t.Item2, c);
    
  end;
  
  BackgroundConvAsyncQueueArray2Base<TInp1, TInp2, TRes, TFunc> = abstract class(BackgroundConvQueueArray2Base<TInp1, TInp2, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 1 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      g.ParallelInvoke(l, false, 2, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev|);
      Result := CombineQRs(qr1, qr2, res_ev);
    end;
    
  end;
  
  BackgroundConvAsyncQueueArray2<TInp1, TInp2, TRes> = sealed class(BackgroundConvAsyncQueueArray2Base<TInp1, TInp2, TRes, (TInp1, TInp2)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2>; c: Context): TRes; override := f(t.Item1, t.Item2);
    
  end;
  BackgroundConvAsyncQueueArray2C<TInp1, TInp2, TRes> = sealed class(BackgroundConvAsyncQueueArray2Base<TInp1, TInp2, TRes, (TInp1, TInp2, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2>; c: Context): TRes; override := f(t.Item1, t.Item2, c);
    
  end;
  
  QuickConvSyncQueueArray2Base<TInp1, TInp2, TRes, TFunc> = abstract class(QuickConvQueueArray2Base<TInp1, TInp2, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      Result := CombineQRs(qr1, qr2, l.prev_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvSyncQueueArray2<TInp1, TInp2, TRes> = sealed class(QuickConvSyncQueueArray2Base<TInp1, TInp2, TRes, (TInp1, TInp2)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; c: Context): TRes; override := f(o1, o2);
    
  end;
  QuickConvSyncQueueArray2C<TInp1, TInp2, TRes> = sealed class(QuickConvSyncQueueArray2Base<TInp1, TInp2, TRes, (TInp1, TInp2, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; c: Context): TRes; override := f(o1, o2, c);
    
  end;
  
  QuickConvAsyncQueueArray2Base<TInp1, TInp2, TRes, TFunc> = abstract class(QuickConvQueueArray2Base<TInp1, TInp2, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      if l.prev_ev.count<>0 then loop 1 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      g.ParallelInvoke(l, false, 2, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev|);
      Result := CombineQRs(qr1, qr2, res_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvAsyncQueueArray2<TInp1, TInp2, TRes> = sealed class(QuickConvAsyncQueueArray2Base<TInp1, TInp2, TRes, (TInp1, TInp2)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; c: Context): TRes; override := f(o1, o2);
    
  end;
  QuickConvAsyncQueueArray2C<TInp1, TInp2, TRes> = sealed class(QuickConvAsyncQueueArray2Base<TInp1, TInp2, TRes, (TInp1, TInp2, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; c: Context): TRes; override := f(o1, o2, c);
    
  end;

{$endregion [2]}

{$region [3]}

type
  BackgroundConvQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc> = abstract class(HostQueue<ValueTuple<TInp1, TInp2, TInp3>, TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; f: TFunc);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.q3 := q3;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3>>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2)) and (qr3 is QueueResConst<TInp3>(var cqr3));
      if all_const then
      begin
        var res := ValueTuple.Create(cqr1.GetRes(), cqr2.GetRes(), cqr3.GetRes());
        Result := new QueueResConst<ValueTuple<TInp1, TInp2, TInp3>>(res, ev);
      end else
        Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes()), ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  QuickConvQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc> = abstract class(CommandQueue<TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; f: TFunc);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.q3 := q3;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; ev: EventList; need_ptr_qr: boolean; c: Context): QueueRes<TRes>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2)) and (qr3 is QueueResConst<TInp3>(var cqr3));
      if all_const then
      begin
        var res := ExecFunc(cqr1.GetRes(), cqr2.GetRes(), cqr3.GetRes(), c);
        Result := QueueRes&<TRes>.MakeNewConstOrPtr(need_ptr_qr, res, ev);
      end else
        Result := new QueueResFunc<TRes>(()->ExecFunc(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), c), ev);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; c: Context): TRes; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc> = abstract class(BackgroundConvQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2, TInp3>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      Result := CombineQRs(qr1, qr2, qr3, l.prev_ev);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray3<TInp1, TInp2, TInp3, TRes> = sealed class(BackgroundConvSyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, (TInp1, TInp2, TInp3)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3);
    
  end;
  BackgroundConvSyncQueueArray3C<TInp1, TInp2, TInp3, TRes> = sealed class(BackgroundConvSyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, (TInp1, TInp2, TInp3, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, c);
    
  end;
  
  BackgroundConvAsyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc> = abstract class(BackgroundConvQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2, TInp3>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 2 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      g.ParallelInvoke(l, false, 3, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
        qr3 := invoker.InvokeBranch(q3.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev, qr3.ev|);
      Result := CombineQRs(qr1, qr2, qr3, res_ev);
    end;
    
  end;
  
  BackgroundConvAsyncQueueArray3<TInp1, TInp2, TInp3, TRes> = sealed class(BackgroundConvAsyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, (TInp1, TInp2, TInp3)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3);
    
  end;
  BackgroundConvAsyncQueueArray3C<TInp1, TInp2, TInp3, TRes> = sealed class(BackgroundConvAsyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, (TInp1, TInp2, TInp3, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, c);
    
  end;
  
  QuickConvSyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc> = abstract class(QuickConvQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      Result := CombineQRs(qr1, qr2, qr3, l.prev_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvSyncQueueArray3<TInp1, TInp2, TInp3, TRes> = sealed class(QuickConvSyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, (TInp1, TInp2, TInp3)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; c: Context): TRes; override := f(o1, o2, o3);
    
  end;
  QuickConvSyncQueueArray3C<TInp1, TInp2, TInp3, TRes> = sealed class(QuickConvSyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, (TInp1, TInp2, TInp3, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; c: Context): TRes; override := f(o1, o2, o3, c);
    
  end;
  
  QuickConvAsyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc> = abstract class(QuickConvQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      if l.prev_ev.count<>0 then loop 2 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      g.ParallelInvoke(l, false, 3, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
        qr3 := invoker.InvokeBranch(q3.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev, qr3.ev|);
      Result := CombineQRs(qr1, qr2, qr3, res_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvAsyncQueueArray3<TInp1, TInp2, TInp3, TRes> = sealed class(QuickConvAsyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, (TInp1, TInp2, TInp3)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; c: Context): TRes; override := f(o1, o2, o3);
    
  end;
  QuickConvAsyncQueueArray3C<TInp1, TInp2, TInp3, TRes> = sealed class(QuickConvAsyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, (TInp1, TInp2, TInp3, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; c: Context): TRes; override := f(o1, o2, o3, c);
    
  end;

{$endregion [3]}

{$region [4]}

type
  BackgroundConvQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc> = abstract class(HostQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4>, TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; f: TFunc);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.q3 := q3;
      self.q4 := q4;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4>>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2)) and (qr3 is QueueResConst<TInp3>(var cqr3)) and (qr4 is QueueResConst<TInp4>(var cqr4));
      if all_const then
      begin
        var res := ValueTuple.Create(cqr1.GetRes(), cqr2.GetRes(), cqr3.GetRes(), cqr4.GetRes());
        Result := new QueueResConst<ValueTuple<TInp1, TInp2, TInp3, TInp4>>(res, ev);
      end else
        Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes()), ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
      self.q4.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
      self.q4.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  QuickConvQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc> = abstract class(CommandQueue<TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; f: TFunc);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.q3 := q3;
      self.q4 := q4;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; ev: EventList; need_ptr_qr: boolean; c: Context): QueueRes<TRes>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2)) and (qr3 is QueueResConst<TInp3>(var cqr3)) and (qr4 is QueueResConst<TInp4>(var cqr4));
      if all_const then
      begin
        var res := ExecFunc(cqr1.GetRes(), cqr2.GetRes(), cqr3.GetRes(), cqr4.GetRes(), c);
        Result := QueueRes&<TRes>.MakeNewConstOrPtr(need_ptr_qr, res, ev);
      end else
        Result := new QueueResFunc<TRes>(()->ExecFunc(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), c), ev);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; c: Context): TRes; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
      self.q4.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
      self.q4.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc> = abstract class(BackgroundConvQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      Result := CombineQRs(qr1, qr2, qr3, qr4, l.prev_ev);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(BackgroundConvSyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, (TInp1, TInp2, TInp3, TInp4)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4);
    
  end;
  BackgroundConvSyncQueueArray4C<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(BackgroundConvSyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, (TInp1, TInp2, TInp3, TInp4, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, c);
    
  end;
  
  BackgroundConvAsyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc> = abstract class(BackgroundConvQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 3 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      g.ParallelInvoke(l, false, 4, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
        qr3 := invoker.InvokeBranch(q3.Invoke);
        qr4 := invoker.InvokeBranch(q4.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, res_ev);
    end;
    
  end;
  
  BackgroundConvAsyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(BackgroundConvAsyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, (TInp1, TInp2, TInp3, TInp4)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4);
    
  end;
  BackgroundConvAsyncQueueArray4C<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(BackgroundConvAsyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, (TInp1, TInp2, TInp3, TInp4, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, c);
    
  end;
  
  QuickConvSyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc> = abstract class(QuickConvQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      Result := CombineQRs(qr1, qr2, qr3, qr4, l.prev_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvSyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(QuickConvSyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, (TInp1, TInp2, TInp3, TInp4)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; c: Context): TRes; override := f(o1, o2, o3, o4);
    
  end;
  QuickConvSyncQueueArray4C<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(QuickConvSyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, (TInp1, TInp2, TInp3, TInp4, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; c: Context): TRes; override := f(o1, o2, o3, o4, c);
    
  end;
  
  QuickConvAsyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc> = abstract class(QuickConvQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      if l.prev_ev.count<>0 then loop 3 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      g.ParallelInvoke(l, false, 4, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
        qr3 := invoker.InvokeBranch(q3.Invoke);
        qr4 := invoker.InvokeBranch(q4.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, res_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvAsyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(QuickConvAsyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, (TInp1, TInp2, TInp3, TInp4)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; c: Context): TRes; override := f(o1, o2, o3, o4);
    
  end;
  QuickConvAsyncQueueArray4C<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(QuickConvAsyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, (TInp1, TInp2, TInp3, TInp4, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; c: Context): TRes; override := f(o1, o2, o3, o4, c);
    
  end;

{$endregion [4]}

{$region [5]}

type
  BackgroundConvQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc> = abstract class(HostQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>, TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected q5: CommandQueue<TInp5>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; f: TFunc);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.q3 := q3;
      self.q4 := q4;
      self.q5 := q5;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2)) and (qr3 is QueueResConst<TInp3>(var cqr3)) and (qr4 is QueueResConst<TInp4>(var cqr4)) and (qr5 is QueueResConst<TInp5>(var cqr5));
      if all_const then
      begin
        var res := ValueTuple.Create(cqr1.GetRes(), cqr2.GetRes(), cqr3.GetRes(), cqr4.GetRes(), cqr5.GetRes());
        Result := new QueueResConst<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>(res, ev);
      end else
        Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes()), ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
      self.q4.RegisterWaitables(g, prev_hubs);
      self.q5.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
      self.q4.ToString(sb, tabs, index, delayed);
      self.q5.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  QuickConvQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc> = abstract class(CommandQueue<TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected q5: CommandQueue<TInp5>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; f: TFunc);
    begin
      self.q1 := q1;
      self.q2 := q2;
      self.q3 := q3;
      self.q4 := q4;
      self.q5 := q5;
      self.f := f;
    end;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; ev: EventList; need_ptr_qr: boolean; c: Context): QueueRes<TRes>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2)) and (qr3 is QueueResConst<TInp3>(var cqr3)) and (qr4 is QueueResConst<TInp4>(var cqr4)) and (qr5 is QueueResConst<TInp5>(var cqr5));
      if all_const then
      begin
        var res := ExecFunc(cqr1.GetRes(), cqr2.GetRes(), cqr3.GetRes(), cqr4.GetRes(), cqr5.GetRes(), c);
        Result := QueueRes&<TRes>.MakeNewConstOrPtr(need_ptr_qr, res, ev);
      end else
        Result := new QueueResFunc<TRes>(()->ExecFunc(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), c), ev);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; c: Context): TRes; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
      self.q4.RegisterWaitables(g, prev_hubs);
      self.q5.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
      self.q4.ToString(sb, tabs, index, delayed);
      self.q5.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc> = abstract class(BackgroundConvQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      var qr5 := q5.Invoke(g, l); l.prev_ev := qr5.ev;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, l.prev_ev);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(BackgroundConvSyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
    
  end;
  BackgroundConvSyncQueueArray5C<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(BackgroundConvSyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, c);
    
  end;
  
  BackgroundConvAsyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc> = abstract class(BackgroundConvQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 4 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      g.ParallelInvoke(l, false, 5, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
        qr3 := invoker.InvokeBranch(q3.Invoke);
        qr4 := invoker.InvokeBranch(q4.Invoke);
        qr5 := invoker.InvokeBranch(q5.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, res_ev);
    end;
    
  end;
  
  BackgroundConvAsyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(BackgroundConvAsyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5);
    
  end;
  BackgroundConvAsyncQueueArray5C<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(BackgroundConvAsyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, c);
    
  end;
  
  QuickConvSyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc> = abstract class(QuickConvQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      var qr5 := q5.Invoke(g, l); l.prev_ev := qr5.ev;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, l.prev_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvSyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(QuickConvSyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; c: Context): TRes; override := f(o1, o2, o3, o4, o5);
    
  end;
  QuickConvSyncQueueArray5C<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(QuickConvSyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; c: Context): TRes; override := f(o1, o2, o3, o4, o5, c);
    
  end;
  
  QuickConvAsyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc> = abstract class(QuickConvQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      if l.prev_ev.count<>0 then loop 4 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      g.ParallelInvoke(l, false, 5, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
        qr3 := invoker.InvokeBranch(q3.Invoke);
        qr4 := invoker.InvokeBranch(q4.Invoke);
        qr5 := invoker.InvokeBranch(q5.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, res_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvAsyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(QuickConvAsyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; c: Context): TRes; override := f(o1, o2, o3, o4, o5);
    
  end;
  QuickConvAsyncQueueArray5C<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(QuickConvAsyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; c: Context): TRes; override := f(o1, o2, o3, o4, o5, c);
    
  end;

{$endregion [5]}

{$region [6]}

type
  BackgroundConvQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc> = abstract class(HostQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>, TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected q5: CommandQueue<TInp5>;
    protected q6: CommandQueue<TInp6>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; f: TFunc);
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
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; qr6: QueueRes<TInp6>; ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2)) and (qr3 is QueueResConst<TInp3>(var cqr3)) and (qr4 is QueueResConst<TInp4>(var cqr4)) and (qr5 is QueueResConst<TInp5>(var cqr5)) and (qr6 is QueueResConst<TInp6>(var cqr6));
      if all_const then
      begin
        var res := ValueTuple.Create(cqr1.GetRes(), cqr2.GetRes(), cqr3.GetRes(), cqr4.GetRes(), cqr5.GetRes(), cqr6.GetRes());
        Result := new QueueResConst<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>(res, ev);
      end else
        Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes()), ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
      self.q4.RegisterWaitables(g, prev_hubs);
      self.q5.RegisterWaitables(g, prev_hubs);
      self.q6.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
      self.q4.ToString(sb, tabs, index, delayed);
      self.q5.ToString(sb, tabs, index, delayed);
      self.q6.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  QuickConvQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc> = abstract class(CommandQueue<TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected q5: CommandQueue<TInp5>;
    protected q6: CommandQueue<TInp6>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; f: TFunc);
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
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; qr6: QueueRes<TInp6>; ev: EventList; need_ptr_qr: boolean; c: Context): QueueRes<TRes>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2)) and (qr3 is QueueResConst<TInp3>(var cqr3)) and (qr4 is QueueResConst<TInp4>(var cqr4)) and (qr5 is QueueResConst<TInp5>(var cqr5)) and (qr6 is QueueResConst<TInp6>(var cqr6));
      if all_const then
      begin
        var res := ExecFunc(cqr1.GetRes(), cqr2.GetRes(), cqr3.GetRes(), cqr4.GetRes(), cqr5.GetRes(), cqr6.GetRes(), c);
        Result := QueueRes&<TRes>.MakeNewConstOrPtr(need_ptr_qr, res, ev);
      end else
        Result := new QueueResFunc<TRes>(()->ExecFunc(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes(), c), ev);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; c: Context): TRes; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
      self.q4.RegisterWaitables(g, prev_hubs);
      self.q5.RegisterWaitables(g, prev_hubs);
      self.q6.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
      self.q4.ToString(sb, tabs, index, delayed);
      self.q5.ToString(sb, tabs, index, delayed);
      self.q6.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc> = abstract class(BackgroundConvQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      var qr5 := q5.Invoke(g, l); l.prev_ev := qr5.ev;
      var qr6 := q6.Invoke(g, l); l.prev_ev := qr6.ev;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, l.prev_ev);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(BackgroundConvSyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
    
  end;
  BackgroundConvSyncQueueArray6C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(BackgroundConvSyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, c);
    
  end;
  
  BackgroundConvAsyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc> = abstract class(BackgroundConvQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 5 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      var qr6: QueueRes<TInp6>;
      g.ParallelInvoke(l, false, 6, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
        qr3 := invoker.InvokeBranch(q3.Invoke);
        qr4 := invoker.InvokeBranch(q4.Invoke);
        qr5 := invoker.InvokeBranch(q5.Invoke);
        qr6 := invoker.InvokeBranch(q6.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev, qr6.ev|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, res_ev);
    end;
    
  end;
  
  BackgroundConvAsyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(BackgroundConvAsyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
    
  end;
  BackgroundConvAsyncQueueArray6C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(BackgroundConvAsyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, c);
    
  end;
  
  QuickConvSyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc> = abstract class(QuickConvQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      var qr5 := q5.Invoke(g, l); l.prev_ev := qr5.ev;
      var qr6 := q6.Invoke(g, l); l.prev_ev := qr6.ev;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, l.prev_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvSyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(QuickConvSyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6);
    
  end;
  QuickConvSyncQueueArray6C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(QuickConvSyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6, c);
    
  end;
  
  QuickConvAsyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc> = abstract class(QuickConvQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      if l.prev_ev.count<>0 then loop 5 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      var qr6: QueueRes<TInp6>;
      g.ParallelInvoke(l, false, 6, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
        qr3 := invoker.InvokeBranch(q3.Invoke);
        qr4 := invoker.InvokeBranch(q4.Invoke);
        qr5 := invoker.InvokeBranch(q5.Invoke);
        qr6 := invoker.InvokeBranch(q6.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev, qr6.ev|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, res_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvAsyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(QuickConvAsyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6);
    
  end;
  QuickConvAsyncQueueArray6C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(QuickConvAsyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6, c);
    
  end;

{$endregion [6]}

{$region [7]}

type
  BackgroundConvQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc> = abstract class(HostQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>, TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected q5: CommandQueue<TInp5>;
    protected q6: CommandQueue<TInp6>;
    protected q7: CommandQueue<TInp7>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>; f: TFunc);
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
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; qr6: QueueRes<TInp6>; qr7: QueueRes<TInp7>; ev: EventList): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2)) and (qr3 is QueueResConst<TInp3>(var cqr3)) and (qr4 is QueueResConst<TInp4>(var cqr4)) and (qr5 is QueueResConst<TInp5>(var cqr5)) and (qr6 is QueueResConst<TInp6>(var cqr6)) and (qr7 is QueueResConst<TInp7>(var cqr7));
      if all_const then
      begin
        var res := ValueTuple.Create(cqr1.GetRes(), cqr2.GetRes(), cqr3.GetRes(), cqr4.GetRes(), cqr5.GetRes(), cqr6.GetRes(), cqr7.GetRes());
        Result := new QueueResConst<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>(res, ev);
      end else
        Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes(), qr7.GetRes()), ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
      self.q4.RegisterWaitables(g, prev_hubs);
      self.q5.RegisterWaitables(g, prev_hubs);
      self.q6.RegisterWaitables(g, prev_hubs);
      self.q7.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
      self.q4.ToString(sb, tabs, index, delayed);
      self.q5.ToString(sb, tabs, index, delayed);
      self.q6.ToString(sb, tabs, index, delayed);
      self.q7.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  QuickConvQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc> = abstract class(CommandQueue<TRes>)
  where TFunc: Delegate;
    protected q1: CommandQueue<TInp1>;
    protected q2: CommandQueue<TInp2>;
    protected q3: CommandQueue<TInp3>;
    protected q4: CommandQueue<TInp4>;
    protected q5: CommandQueue<TInp5>;
    protected q6: CommandQueue<TInp6>;
    protected q7: CommandQueue<TInp7>;
    protected f: TFunc;
    
    public constructor(q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>; f: TFunc);
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
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; qr6: QueueRes<TInp6>; qr7: QueueRes<TInp7>; ev: EventList; need_ptr_qr: boolean; c: Context): QueueRes<TRes>;
    begin
      //TODO #????
      var all_const := (qr1 is QueueResConst<TInp1>(var cqr1)) and (qr2 is QueueResConst<TInp2>(var cqr2)) and (qr3 is QueueResConst<TInp3>(var cqr3)) and (qr4 is QueueResConst<TInp4>(var cqr4)) and (qr5 is QueueResConst<TInp5>(var cqr5)) and (qr6 is QueueResConst<TInp6>(var cqr6)) and (qr7 is QueueResConst<TInp7>(var cqr7));
      if all_const then
      begin
        var res := ExecFunc(cqr1.GetRes(), cqr2.GetRes(), cqr3.GetRes(), cqr4.GetRes(), cqr5.GetRes(), cqr6.GetRes(), cqr7.GetRes(), c);
        Result := QueueRes&<TRes>.MakeNewConstOrPtr(need_ptr_qr, res, ev);
      end else
        Result := new QueueResFunc<TRes>(()->ExecFunc(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes(), qr7.GetRes(), c), ev);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; o7: TInp7; c: Context): TRes; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
      self.q4.RegisterWaitables(g, prev_hubs);
      self.q5.RegisterWaitables(g, prev_hubs);
      self.q6.RegisterWaitables(g, prev_hubs);
      self.q7.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
      self.q4.ToString(sb, tabs, index, delayed);
      self.q5.ToString(sb, tabs, index, delayed);
      self.q6.ToString(sb, tabs, index, delayed);
      self.q7.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc> = abstract class(BackgroundConvQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      var qr5 := q5.Invoke(g, l); l.prev_ev := qr5.ev;
      var qr6 := q6.Invoke(g, l); l.prev_ev := qr6.ev;
      var qr7 := q7.Invoke(g, l); l.prev_ev := qr7.ev;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, qr7, l.prev_ev);
    end;
    
  end;
  
  BackgroundConvSyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(BackgroundConvSyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
    
  end;
  BackgroundConvSyncQueueArray7C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(BackgroundConvSyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, c);
    
  end;
  
  BackgroundConvAsyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc> = abstract class(BackgroundConvQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l_nil: CLTaskLocalDataNil): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>; override;
    begin
      var l := l_nil.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 6 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      var qr6: QueueRes<TInp6>;
      var qr7: QueueRes<TInp7>;
      g.ParallelInvoke(l, false, 7, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
        qr3 := invoker.InvokeBranch(q3.Invoke);
        qr4 := invoker.InvokeBranch(q4.Invoke);
        qr5 := invoker.InvokeBranch(q5.Invoke);
        qr6 := invoker.InvokeBranch(q6.Invoke);
        qr7 := invoker.InvokeBranch(q7.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev, qr6.ev, qr7.ev|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, qr7, res_ev);
    end;
    
  end;
  
  BackgroundConvAsyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(BackgroundConvAsyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);
    
  end;
  BackgroundConvAsyncQueueArray7C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(BackgroundConvAsyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context)->TRes>)
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, c);
    
  end;
  
  QuickConvSyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc> = abstract class(QuickConvQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      var qr5 := q5.Invoke(g, l); l.prev_ev := qr5.ev;
      var qr6 := q6.Invoke(g, l); l.prev_ev := qr6.ev;
      var qr7 := q7.Invoke(g, l); l.prev_ev := qr7.ev;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, qr7, l.prev_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvSyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(QuickConvSyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; o7: TInp7; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6, o7);
    
  end;
  QuickConvSyncQueueArray7C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(QuickConvSyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; o7: TInp7; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6, o7, c);
    
  end;
  
  QuickConvAsyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc> = abstract class(QuickConvQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc>)
  where TFunc: Delegate;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      if l.prev_ev.count<>0 then loop 6 do l.prev_ev.Retain({$ifdef EventDebug}'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      var qr6: QueueRes<TInp6>;
      var qr7: QueueRes<TInp7>;
      g.ParallelInvoke(l, false, 7, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
        qr3 := invoker.InvokeBranch(q3.Invoke);
        qr4 := invoker.InvokeBranch(q4.Invoke);
        qr5 := invoker.InvokeBranch(q5.Invoke);
        qr6 := invoker.InvokeBranch(q6.Invoke);
        qr7 := invoker.InvokeBranch(q7.Invoke);
      end);
      var res_ev := EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev, qr6.ev, qr7.ev|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, qr7, res_ev, l.need_ptr_qr, g.c);
    end;
    
  end;
  
  QuickConvAsyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(QuickConvAsyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; o7: TInp7; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6, o7);
    
  end;
  QuickConvAsyncQueueArray7C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(QuickConvAsyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; o7: TInp7; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6, o7, c);
    
  end;

{$endregion [7]}

{$endregion Conv}

{$region Utils}

type
  QueueArrayFlattener<TArray> = sealed class(ITypedCQUser)
  where TArray: ISimpleQueueArray;
    public qs := new List<CommandQueueBase>;
    private has_next := false;
    
    public procedure ProcessSeq(s: sequence of CommandQueueBase);
    begin
      var enmr := s.GetEnumerator;
      if not enmr.MoveNext then raise new System.ArgumentException('Функции CombineSyncQueue/CombineAsyncQueue не могут принимать 0 очередей');
      
      var upper_had_next := self.has_next;
      while true do
      begin
        var curr := enmr.Current;
        var l_has_next := enmr.MoveNext;
        self.has_next := upper_had_next or l_has_next;
        curr.UseTyped(self);
        if not l_has_next then break;
      end;
      self.has_next := upper_had_next;
      
    end;
    
    public procedure ITypedCQUser.UseNil(cq: CommandQueueNil);
    begin
      // Нельзя пропускать - тут можно быть HPQ, WaitFor и т.п. работа без результата
//      if has_next then exit;
      qs.Add(cq);
    end;
    public procedure ITypedCQUser.Use<T>(cq: CommandQueue<T>);
    begin
      if has_next then
      begin
        if cq is ConstQueue<T> then exit;
        if cq is CastQueueBase<T>(var cqb) then
        begin
          cqb.SourceBase.UseTyped(self);
          exit;
        end;
      end;
      if cq is TArray(var sqa) then
        ProcessSeq(sqa.GetQs) else
        qs.Add(cq);
    end;
    
  end;
  
  QueueArrayConstructorBase = abstract class
    private body: array of CommandQueueBase;
    
    public constructor(body: array of CommandQueueBase) := self.body := body;
    private constructor := raise new OpenCLABCInternalException;
    
  end;
  
  QueueArraySyncConstructor = sealed class(QueueArrayConstructorBase, ITypedCQConverter<CommandQueueBase>)
    public function ConvertNil(last: CommandQueueNil): CommandQueueBase := new SimpleSyncQueueArrayNil(body, last);
    public function Convert<T>(last: CommandQueue<T>): CommandQueueBase := new SimpleSyncQueueArray<T>(body, last);
  end;
  QueueArrayAsyncConstructor = sealed class(QueueArrayConstructorBase, ITypedCQConverter<CommandQueueBase>)
    public function ConvertNil(last: CommandQueueNil): CommandQueueBase := new SimpleAsyncQueueArrayNil(body, last);
    public function Convert<T>(last: CommandQueue<T>): CommandQueueBase := new SimpleAsyncQueueArray<T>(body, last);
  end;
  
  QueueArrayUtils = static class
    
    public static function FlattenQueueArray<T>(inp: sequence of CommandQueueBase): ValueTuple<List<CommandQueueBase>,CommandQueueBase>; where T: ISimpleQueueArray;
    begin
      var res := new QueueArrayFlattener<T>;
      res.ProcessSeq(inp);
      var last_ind := res.qs.Count-1;
      var last := res.qs[last_ind];
      res.qs.RemoveAt(last_ind);
      Result := ValueTuple.Create(res.qs,last);
    end;
    
    public static function ConstructSync(inp: sequence of CommandQueueBase): CommandQueueBase;
    begin
      var (body,last) := FlattenQueueArray&<ISimpleSyncQueueArray>(inp);
      Result := if body.Count=0 then last else last.ConvertTyped(new QueueArraySyncConstructor(body.ToArray));
    end;
    public static function ConstructSyncNil(inp: sequence of CommandQueueBase) := CommandQueueNil ( ConstructSync(inp) );
    public static function ConstructSync<T>(inp: sequence of CommandQueueBase) := CommandQueue&<T>( ConstructSync(inp) );
    
    public static function ConstructAsync(inp: sequence of CommandQueueBase): CommandQueueBase;
    begin
      var (body,last) := FlattenQueueArray&<ISimpleAsyncQueueArray>(inp);
      Result := if body.Count=0 then last else last.ConvertTyped(new QueueArrayAsyncConstructor(body.ToArray));
    end;
    public static function ConstructAsyncNil(inp: sequence of CommandQueueBase) := CommandQueueNil ( ConstructAsync(inp) );
    public static function ConstructAsync<T>(inp: sequence of CommandQueueBase) := CommandQueue&<T>( ConstructAsync(inp) );
    
  end;
  
{$endregion Utils}

static function CommandQueueNil.operator+(q1: CommandQueueBase; q2: CommandQueueNil) := QueueArrayUtils. ConstructSyncNil(|q1, q2|);
static function CommandQueueNil.operator*(q1: CommandQueueBase; q2: CommandQueueNil) := QueueArrayUtils.ConstructAsyncNil(|q1, q2|);

static function CommandQueue<T>.operator+(q1: CommandQueueBase; q2: CommandQueue<T>) := QueueArrayUtils. ConstructSync&<T>(|q1, q2|);
static function CommandQueue<T>.operator*(q1: CommandQueueBase; q2: CommandQueue<T>) := QueueArrayUtils.ConstructAsync&<T>(|q1, q2|);

{$endregion +/*}

{$region Multiusable}

type
  MultiusableCommandQueueHubCommon<TQ> = abstract class(IMultiusableCommandQueueHub)
  where TQ: CommandQueueBase;
    public q: TQ;
    public constructor(q: TQ) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>) :=
    if prev_hubs.Add(self) then q.RegisterWaitables(g, prev_hubs);
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    function Invoke<TLData,TR>(g: CLTaskGlobalData; l: TLData; invoke_q: (CLTaskGlobalData,TLData)->TR): TR; where TLData: ICLTaskLocalData; where TR: QueueResBase;
    begin
      var prev_ev := l.PrevEv;
      
      var res_data: MultiuseableResultData;
      // Потоко-безопасно, потому что все .Invoke выполняются синхронно
      //TODO А что будет когда .ThenIf и т.п.
      if g.mu_res.TryGetValue(self, res_data) then
      begin
        g.curr_err_handler := new CLTaskErrHandlerMultiusableRepeater(g.curr_err_handler, res_data.err_handler);
        Result := TR( res_data.qres );
      end else
      begin
        var prev_err_handler := g.curr_err_handler;
        g.curr_err_handler := new CLTaskErrHandlerEmpty;
        
        l.PrevEv := EventList.Empty;
        Result := invoke_q(g, l);
        // QueueResFunc shouldn't have it's .GetRes be called twice
        Result := TR(Result.ThenInvokeIfDelegateRes(g, l.NeedPtrQr));
        Result.can_set_ev := false;
        var q_err_handler := g.curr_err_handler;
        
        g.curr_err_handler := new CLTaskErrHandlerMultiusableRepeater(prev_err_handler, q_err_handler);
        g.mu_res[self] := new MultiuseableResultData(Result, q_err_handler);
      end;
      
      var res_ev := Result.ev;
      res_ev.Retain({$ifdef EventDebug}$'for all mu branches'{$endif});
      if prev_ev.count<>0 then
        Result := TR(Result.TrySetEvBase(res_ev+prev_ev));
    end;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>);
    begin
      sb += ' => ';
      if q.ToStringHeader(sb, index) then
        delayed.Add(q);
      sb += #10;
    end;
    
  end;
  
  MultiusableCommandQueueHubNil = sealed class(MultiusableCommandQueueHubCommon< CommandQueueNil >)
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil) := Invoke(g, l, q.Invoke);
    
    public function MakeNode: CommandQueueNil;
    
  end;
  MultiusableCommandQueueNodeNil = sealed class(CommandQueueNil)
    public hub: MultiusableCommandQueueHubNil;
    public constructor(hub: MultiusableCommandQueueHubNil) := self.hub := hub;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := hub.RegisterWaitables(g, prev_hubs);
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueResNil; override := hub.Invoke(g, l);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    hub.ToString(sb, tabs, index, delayed);
    
  end;
  
  MultiusableCommandQueueHub<T> = sealed class(MultiusableCommandQueueHubCommon< CommandQueue<T> >)
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData) := Invoke(g, l, q.Invoke);
    
    public function MakeNode: CommandQueue<T>;
    
  end;
  MultiusableCommandQueueNode<T> = sealed class(CommandQueue<T>)
    public hub: MultiusableCommandQueueHub<T>;
    public constructor(hub: MultiusableCommandQueueHub<T>) := self.hub := hub;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := hub.RegisterWaitables(g, prev_hubs);
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override :=
    // Additional pointer shouldn't be created for just 1 mu user
    hub.Invoke(g, l.WithPtrNeed(false));
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    hub.ToString(sb, tabs, index, delayed);
    
  end;
  
function MultiusableCommandQueueHubNil.MakeNode := new MultiusableCommandQueueNodeNil(self);
function MultiusableCommandQueueHub<T>.MakeNode := new MultiusableCommandQueueNode<T>(self);

function CommandQueueNil.Multiusable := (new MultiusableCommandQueueHubNil(self)).MakeNode;
function CommandQueue<T>.Multiusable := (new MultiusableCommandQueueHub<T>(self)).MakeNode;

{$endregion Multiusable}

{$region Wait}

{$region Def}
//TODO Куча дублей кода, особенно в Combination
//TODO data ничего не делает, кроме как для WaitDebug, потому что state хранится в sub_info
// - Лучше передавать self.GetHashCode
//TODO Отписка никогда не происходит - пока не сделал, чтоб перепродумывать как обрабатывать всё при циклах

{$region Base}

type
  WaitMarker = abstract partial class
    
    public procedure InitInnerHandles(g: CLTaskGlobalData); abstract;
    
    public function MakeWaitEv(g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; abstract;
    
  end;
  
{$endregion Base}

{$region Outer}

type
  /// wait_handler, который можно встроить в очередь как есть
  WaitHandlerOuter = abstract class
    public uev: UserEvent;
    private state := 0;
    
    public constructor(g: CLTaskGlobalData; l: CLTaskLocalDataNil);
    begin
      
      uev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'Wait result'{$endif});
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Created outer with prev_ev=[ {l.prev_ev.evs?.JoinToString} ], res_ev={uev}');
      {$endif WaitDebug}
      EventList.AttachCallback(true, self.uev, ()->System.GC.KeepAlive(self){$ifdef EventDebug}, $'KeepAlive(WaitHandlerOuter)'{$endif});
      
      var err_handler := g.curr_err_handler;
      l.prev_ev.MultiAttachCallback(false, ()->
      begin
        if err_handler.HadError(true) then
        begin
          {$ifdef WaitDebug}
          WaitDebug.RegisterAction(self, $'Aborted');
          {$endif WaitDebug}
          uev.SetComplete;
        end else
        begin
          {$ifdef WaitDebug}
          WaitDebug.RegisterAction(self, $'Got prev_ev boost');
          {$endif WaitDebug}
          self.IncState;
        end;
      end{$ifdef EventDebug}, $'KeepAlive(handler[{self.GetHashCode}])'{$endif});
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function TryConsume: boolean; abstract;
    
    protected function IncState: boolean;
    begin
      var new_state := Interlocked.Increment(self.state);
      
      {$ifdef DEBUG}
      if not new_state.InRange(1,2) then raise new OpenCLABCInternalException($'WaitHandlerOuter.state={new_state}');
      {$endif DEBUG}
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Advanced to state {new_state}');
      {$endif WaitDebug}
      
      Result := (new_state=2) and TryConsume;
    end;
    protected procedure DecState;
    begin
      {$ifdef DEBUG}
      var new_state :=
      {$endif DEBUG}
      Interlocked.Decrement(self.state);
      
      {$ifdef DEBUG}
      if not new_state.InRange(0,1) then
        raise new OpenCLABCInternalException($'WaitHandlerOuter.state={new_state}');
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Gone back to state {new_state}');
      {$endif WaitDebug}
      
      {$endif DEBUG}
      
    end;
    
  end;
  
{$endregion Outer}

{$region Direct}

type
  IWaitHandlerSub = interface
    
    // Возвращает true, если активацию успешно съели
    function HandleChildInc(data: integer): boolean;
    procedure HandleChildDec(data: integer);
    
  end;
  
  WaitHandlerDirectSubInfo = class
    public threshold, data: integer;
    public state := new InterlockedBoolean;
    public constructor(threshold, data: integer);
    begin
      self.threshold := threshold;
      self.data := data;
    end;
    public constructor := raise new OpenCLABCInternalException;
  end;
  /// Напрямую хранит активации конкретного CLTaskGlobalData
  WaitHandlerDirect = sealed class
    private subs := new ConcurrentDictionary<IWaitHandlerSub, WaitHandlerDirectSubInfo>;
    private activations := 0;
    private reserved := 0;
    
    public procedure Subscribe(sub: IWaitHandlerSub; info: WaitHandlerDirectSubInfo);
    begin
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Got new sub {sub.GetHashCode}');
      {$endif WaitDebug}
      
      if not subs.TryAdd(sub, info) then
      begin
        {$ifdef DEBUG}
        raise new OpenCLABCInternalException($'Sub added twice');
        {$endif DEBUG}
      end else
      if activations>=info.threshold then
        if info.state.TrySet(true) then
        begin
          {$ifdef WaitDebug}
          WaitDebug.RegisterAction(self, $'Add immidiatly inced sub {sub.GetHashCode}');
          {$endif WaitDebug}
          // Может выполняться одновременно с AddActivation, в таком случае 
          sub.HandleChildInc(info.data);
        end;
    end;
    
    public procedure AddActivation;
    begin
      {$ifdef WaitDebug}
      var new_act :=
      {$endif WaitDebug}
      Interlocked.Increment(activations);
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Got activation =>{new_act}');
      {$endif WaitDebug}
      
      foreach var kvp in subs do
        // activations может изменится, если .HandleChildInc из
        // .AddActivation другого хэндлера или .Subscribe затронет self.activations
        // Поэтому результат Interlocked.Increment использовать нельзя
        if activations>=kvp.Value.threshold then
          if kvp.Value.state.TrySet(true) and kvp.Key.HandleChildInc(kvp.Value.data) then
          begin
            {$ifdef WaitDebug}
            WaitDebug.RegisterAction(self, $'Sub {kvp.Key.GetHashCode} consumed activation =>{activations}');
            {$endif WaitDebug}
            // Если активацию съели - нет смысла продолжать
            break;
          end;
    end;
    
    public function TryReserve(c: integer): boolean;
    begin
      var n_reserved := Interlocked.Add(reserved, c);
      Result := n_reserved<=activations;
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Tried to reserve {c}=>{n_reserved}: {Result}');
      {$endif WaitDebug}
      
      // Надо делать там, где было вызвано TryReserve
      // Потому что TryReserve не последняя проверка, есть ещё uev.SetStatus
//      if not Result then ReleaseReserve(c);
    end;
    public procedure ReleaseReserve(c: integer) :=
    if Interlocked.Add(reserved, -c)<0 then
    begin
      {$ifdef DEBUG}
      raise new OpenCLABCInternalException($'reserved={reserved}');
      {$endif DEBUG}
    end else
    begin
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Released reserve {c}=>{reserved}');
      {$endif WaitDebug}
    end;
    
    public procedure Comsume(c: integer);
    begin
      var new_act := Interlocked.Add(activations, -c);
      var new_res := Interlocked.Add(reserved, -c);
      {$ifdef DEBUG}
      if (new_act<0) or (new_res<0) then
        raise new OpenCLABCInternalException($'new_act={new_act}, new_res={new_res}');
      {$endif DEBUG}
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Sub consumed {c}, new_act={new_act}, new_res={new_res}');
      {$endif WaitDebug}
      
      foreach var kvp in subs do
        if activations<kvp.Value.threshold then
          if kvp.Value.state.TrySet(false) then
            kvp.Key.HandleChildDec(kvp.Value.data);
    end;
    
  end;
  /// Обёртка WaitHandlerDirect, которая является WaitHandlerOuter
  WaitHandlerDirectWrap = sealed class(WaitHandlerOuter, IWaitHandlerSub)
    private source: WaitHandlerDirect;
    
    public constructor(g: CLTaskGlobalData; l: CLTaskLocalDataNil; source: WaitHandlerDirect);
    begin
      inherited Create(g, l);
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'This is DirectWrap for {source.GetHashCode}');
      {$endif WaitDebug}
      self.source := source;
      source.Subscribe(self, new WaitHandlerDirectSubInfo(1,0));
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    public function IWaitHandlerSub.HandleChildInc(data: integer) := self.IncState;
    public procedure IWaitHandlerSub.HandleChildDec(data: integer) := self.DecState;
    
    protected function TryConsume: boolean; override;
    begin
      Result := source.TryReserve(1) and self.uev.SetComplete;
      if not Result then source.ReleaseReserve(1);
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Tryed reserving {1} in source[{source.GetHashCode}]: {Result}');
      {$endif WaitDebug}
      
      if Result then source.Comsume(1);
    end;
    
  end;
  
  /// Маркер, не ссылающийся на другие маркеры
  WaitMarkerDirect = abstract class(WaitMarker)
    private handlers := new ConcurrentDictionary<CLTaskGlobalData, WaitHandlerDirect>;
    
    public procedure InitInnerHandles(g: CLTaskGlobalData); override :=
    handlers.GetOrAdd(g, g->
    begin
      Result := new WaitHandlerDirect;
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(Result, $'Created for {self.GetType.Name}[{self.GetHashCode}]');
      {$endif WaitDebug}
    end);
    
    public function MakeWaitEv(g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override :=
    WaitHandlerDirectWrap.Create(g, l, handlers[g]).uev;
    
    public procedure SendSignal; override :=
    foreach var h in handlers.Values do
      h.AddActivation;
    
  end;
  
{$endregion Direct}

{$region Combination}

{$region Base}

type
  WaitMarkerCombination<TChild> = abstract class(WaitMarker)
  where TChild: WaitMarker;
    private children: array of TChild;
    
    public constructor(children: array of TChild) := self.children := children;
    public constructor := raise new OpenCLABCInternalException;
    
    public procedure InitInnerHandles(g: CLTaskGlobalData); override :=
    foreach var child in children do child.InitInnerHandles(g);
    
    {$region Disabled override's}
    
    private function ConvertToQBase: CommandQueueBase; override;
    begin
      Result := nil;
      raise new System.InvalidProgramException($'Преобразовывать комбинацию маркеров в очередь нельзя. Возможно вы забыли написать WaitFor?');
    end;
    
    public procedure SendSignal; override :=
    raise new System.InvalidProgramException($'Err:WaitMarkerCombination.SendSignal');
    
    {$endregion Disabled override's}
    
  end;
  
{$endregion Base}

{$region All}

type
  WaitHandlerAllInner = sealed class(IWaitHandlerSub)
    private sources: array of WaitHandlerDirect;
    private ref_counts: array of integer;
    private done_c := 0;
    
    private sub: IWaitHandlerSub;
    private sub_data: integer;
    
    public constructor(sources: array of WaitHandlerDirect; ref_counts: array of integer; sub: IWaitHandlerSub; sub_data: integer);
    begin
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Created AllInner for: {sources.Select(s->s.GetHashCode).JoinToString}');
      {$endif WaitDebug}
      self.sources := sources;
      for var i := 0 to sources.Length-1 do
        sources[i].Subscribe(self, new WaitHandlerDirectSubInfo(ref_counts[i], i));
      self.ref_counts := ref_counts;
      self.sub := sub;
      self.sub_data := sub_data;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    public function IWaitHandlerSub.HandleChildInc(data: integer): boolean;
    begin
      var new_done_c := Interlocked.Increment(done_c);
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Got activation from {sources[data].GetHashCode}, new_done_c={new_done_c}/{sources.Length}');
      {$endif WaitDebug}
      
      Result := (new_done_c=sources.Length) and sub.HandleChildInc(sub_data);
    end;
    public procedure IWaitHandlerSub.HandleChildDec(data: integer);
    begin
      var prev_done_c := Interlocked.Decrement(done_c)+1;
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Got deactivation from {sources[data].GetHashCode}, new_done_c={prev_done_c-1}/{sources.Length}');
      {$endif WaitDebug}
      
      if prev_done_c=sources.Length then sub.HandleChildDec(sub_data);
    end;
    
    public function TryConsume(uev: UserEvent): boolean;
    begin
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Trying to reserve');
      {$endif WaitDebug}
      Result := false;
      for var i := 0 to sources.Length-1 do
      begin
        if sources[i].TryReserve(ref_counts[i]) then continue;
        for var prev_i := 0 to i do
          sources[i].ReleaseReserve(ref_counts[i]);
        exit;
      end;
      Result := uev.SetComplete;
      if Result then
      begin
        {$ifdef WaitDebug}
        WaitDebug.RegisterAction(self, $'Consuming');
        {$endif WaitDebug}
        for var i := 0 to sources.Length-1 do
          sources[i].Comsume(ref_counts[i]);
      end else
      begin
        {$ifdef WaitDebug}
        WaitDebug.RegisterAction(self, $'Abort consume');
        {$endif WaitDebug}
        for var i := 0 to sources.Length-1 do
          sources[i].ReleaseReserve(ref_counts[i]);
      end;
    end;
    
  end;
  WaitHandlerAllOuter = sealed class(WaitHandlerOuter, IWaitHandlerSub)
    private sources: array of WaitHandlerDirect;
    private ref_counts: array of integer;
    private done_c := 0;
    
    public constructor(g: CLTaskGlobalData; l: CLTaskLocalDataNil; sources: array of WaitHandlerDirect; ref_counts: array of integer);
    begin
      inherited Create(g, l);
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'This is AllOuter for: {sources.Select(s->s.GetHashCode).JoinToString}');
      {$endif WaitDebug}
      self.sources := sources;
      for var i := 0 to sources.Length-1 do
        sources[i].Subscribe(self, new WaitHandlerDirectSubInfo(ref_counts[i], i));
      self.ref_counts := ref_counts;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    public function IWaitHandlerSub.HandleChildInc(data: integer): boolean;
    begin
      var new_done_c := Interlocked.Increment(done_c);
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Got activation from {sources[data].GetHashCode}, new_done_c={new_done_c}/{sources.Length}');
      {$endif WaitDebug}
      
      Result := (new_done_c=sources.Length) and self.IncState;
    end;
    public procedure IWaitHandlerSub.HandleChildDec(data: integer);
    begin
      var prev_done_c := Interlocked.Decrement(done_c)+1;
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Got deactivation from {sources[data].GetHashCode}, new_done_c={prev_done_c-1}/{sources.Length}');
      {$endif WaitDebug}
      
      if prev_done_c=sources.Length then self.DecState;
    end;
    
    protected function TryConsume: boolean; override;
    begin
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Trying to reserve');
      {$endif WaitDebug}
      Result := false;
      for var i := 0 to sources.Length-1 do
      begin
        if sources[i].TryReserve(ref_counts[i]) then continue;
        for var prev_i := 0 to i do
          sources[i].ReleaseReserve(ref_counts[i]);
        exit;
      end;
      Result := uev.SetComplete;
      if Result then
      begin
        {$ifdef WaitDebug}
        WaitDebug.RegisterAction(self, $'Consuming');
        {$endif WaitDebug}
        for var i := 0 to sources.Length-1 do
          sources[i].Comsume(ref_counts[i]);
      end else
      begin
        {$ifdef WaitDebug}
        WaitDebug.RegisterAction(self, $'Abort consume');
        {$endif WaitDebug}
        for var i := 0 to sources.Length-1 do
          sources[i].ReleaseReserve(ref_counts[i]);
      end;
    end;
    
  end;
  
  WaitMarkerAll = sealed partial class(WaitMarkerCombination<WaitMarkerDirect>)
    private ref_counts: array of integer;
    
    public constructor(children: Dictionary<WaitMarkerDirect, integer>);
    begin
      inherited Create(children.Keys.ToArray);
      self.ref_counts := self.children.ConvertAll(key->children[key]);
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      foreach var i in Range(0,children.Length-1).OrderByDescending(i->ref_counts[i]) do
      begin
        children[i].ToString(sb, tabs, index, delayed);
        if ref_counts[i]<>1 then
        begin
          sb.Length -= 1;
          sb += ' * ';
          sb.Append(ref_counts[i]);
          sb += #10;
        end;
      end;
    end;
    
    public function MakeWaitEv(g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override :=
    WaitHandlerAllOuter.Create(g, l, children.ConvertAll(m->m.handlers[g]), ref_counts).uev;
    
    private function GetChildrenArr: array of WaitMarkerDirect;
    begin
      Result := new WaitMarkerDirect[ref_counts.Sum];
      var res_ind := 0;
      for var i := 0 to children.Length-1 do
        loop ref_counts[i] do
        begin
          Result[res_ind] := children[i];
          res_ind += 1;
        end;
    end;
    
  end;
  
{$endregion All}

{$region Any}

type
  WaitHandlerAnyOuter = sealed class(WaitHandlerOuter, IWaitHandlerSub)
    private sources: array of WaitHandlerAllInner;
    
    private done_c := 0;
    
    public constructor(g: CLTaskGlobalData; l: CLTaskLocalDataNil; markers: array of WaitMarkerAll);
    begin
      inherited Create(g, l);
      self.sources := new WaitHandlerAllInner[markers.Length];
      for var i := 0 to markers.Length-1 do
        self.sources[i] := new WaitHandlerAllInner(markers[i].children.ConvertAll(m->m.handlers[g]), markers[i].ref_counts, self, i);
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'This is AnyOuter for: {sources.Select(s->s.GetHashCode).JoinToString}');
      {$endif WaitDebug}
    end;
    public constructor := raise new OpenCLABCInternalException;
    
    public function IWaitHandlerSub.HandleChildInc(data: integer): boolean;
    begin
      var new_done_c := Interlocked.Increment(done_c);
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Got activation from {sources[data].GetHashCode}, new_done_c={new_done_c}/{sources.Length}');
      {$endif WaitDebug}
      
      Result := (new_done_c=1) and self.IncState;
    end;
    public procedure IWaitHandlerSub.HandleChildDec(data: integer);
    begin
      var prev_done_c := Interlocked.Decrement(done_c)+1;
      
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Got deactivation from {sources[data].GetHashCode}, new_done_c={prev_done_c-1}/{sources.Length}');
      {$endif WaitDebug}
      
      if prev_done_c=1 then self.DecState;
    end;
    
    protected function TryConsume: boolean; override;
    begin
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Trying to consume');
      {$endif WaitDebug}
      Result := false;
      for var i := 0 to sources.Length-1 do
        if sources[i].TryConsume(uev) then
        begin
          Result := true;
          break;
        end;
    end;
    
  end;
  
  WaitMarkerAny = sealed partial class(WaitMarkerCombination<WaitMarkerAll>)
    
    public constructor(sources: array of WaitMarkerAll) := inherited Create(sources);
    private constructor := raise new OpenCLABCInternalException;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      foreach var child in children do
        child.ToString(sb, tabs, index, delayed);
    end;
    
    public function MakeWaitEv(g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override :=
    WaitHandlerAnyOuter.Create(g, l, children).uev;
    
  end;
  
{$endregion Any}

{$region public}

type
  WaitMarkerAllFast = sealed class
    private children: Dictionary<WaitMarkerDirect, integer>;
    
    public constructor(c: integer) :=
    children := new Dictionary<WaitMarkerDirect, integer>(c);
    public constructor(m: WaitMarkerDirect);
    begin
      Create(1);
      self.children.Add(m, 1);
    end;
    public constructor(m: WaitMarkerAll);
    begin
      Create(m.children.Length);
      for var i := 0 to m.children.Length-1 do
        self.children.Add(m.children[i], m.ref_counts[i]);
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    public static function operator in(what, in_what: WaitMarkerAllFast): boolean;
    begin
      Result := false;
      
      if what.children.Count>in_what.children.Count then
        exit;
      foreach var kvp in what.children do
        if in_what.children.Get(kvp.Key) < kvp.Value then
          exit;
      
      Result := true;
    end;
    
    public static function operator+(c1, c2: WaitMarkerAllFast): WaitMarkerAllFast;
    begin
      Result := new WaitMarkerAllFast(c1.children.Count+c2.children.Count);
      foreach var kvp in c1.children do
        Result.children.Add(kvp.Key, kvp.Value);
      foreach var kvp in c2.children do
        Result.children[kvp.Key] := Result.children.Get(kvp.Key) + kvp.Value;
    end;
    
    public static procedure TryAdd(lst: List<WaitMarkerAllFast>; c: WaitMarkerAllFast);
    begin
      
      for var i := 0 to lst.Count-1 do
      begin
        var c0 := lst[i];
        
        if c0 in c then
          lst[i] := c else
        if c in c0 then
          {nothing} else
          continue;
        
        exit;
      end;
      
      lst += c;
    end;
    
    public static function MarkerFromLst(lst: IList<WaitMarkerAllFast>): WaitMarker;
    begin
      if lst.Count>1 then
      begin
        var res := new WaitMarkerAll[lst.Count];
        for var i := 0 to res.Length-1 do
          res[i] := new WaitMarkerAll(lst[i].children);
        Result := new WaitMarkerAny(res);
      end else
      case lst[0].children.Values.Sum of
        0: raise new System.ArgumentException($'Количество комбинируемых маркеров должно быть положительным');
        1: Result := lst[0].children.Keys.Single;
        else Result := new WaitMarkerAll(lst[0].children);
      end;
    end;
    
  end;
  
function WaitAll(sub_markers: sequence of WaitMarker): WaitMarker;
begin
  var prev := |new WaitMarkerAllFast(0)| as IList<WaitMarkerAllFast>;
  var next := new List<WaitMarkerAllFast>;
  
  foreach var m in sub_markers do
  begin
    
    if m is WaitMarkerAny(var ma) then
    begin
      foreach var child in ma.children do
      begin
        var c2 := new WaitMarkerAllFast(child);
        foreach var c1 in prev do
          WaitMarkerAllFast.TryAdd(next, c1+c2);
      end;
    end else
    begin
      var c2 := if m is WaitMarkerDirect(var md) then
        new WaitMarkerAllFast(md) else
        new WaitMarkerAllFast(WaitMarkerAll(m));
      foreach var c1 in prev do
        next += c1+c2;
    end;
    
    prev := next.ToArray;
    next.Clear;
  end;
  
  Result := WaitMarkerAllFast.MarkerFromLst(prev);
end;
function WaitAll(params sub_markers: array of WaitMarker) := WaitAll(sub_markers.AsEnumerable);

function WaitAny(sub_markers: sequence of WaitMarker): WaitMarker;
begin
  var res := new List<WaitMarkerAllFast>;
  foreach var m in sub_markers do
    if m is WaitMarkerAny(var ma) then
    begin
      foreach var child in ma.children do
        WaitMarkerAllFast.TryAdd(res, new WaitMarkerAllFast(child));
    end else
    begin
      var c := if m is WaitMarkerDirect(var md) then
        new WaitMarkerAllFast(md) else
        new WaitMarkerAllFast(WaitMarkerAll(m));
      WaitMarkerAllFast.TryAdd(res, c);
    end;
  Result := WaitMarkerAllFast.MarkerFromLst(res);
end;
function WaitAny(params sub_markers: array of WaitMarker) := WaitAny(sub_markers.AsEnumerable);

static function WaitMarker.operator and(m1, m2: WaitMarker) := WaitAll(|m1, m2|);
static function WaitMarker.operator or(m1, m2: WaitMarker) := WaitAny(|m1, m2|);

{$endregion public}

{$endregion Combination}

{$endregion Def}

{$region WaitMarkerDummy}

type
  WaitMarkerDummyExecutor = sealed class(CommandQueueNil)
    private m: WaitMarkerDirect;
    
    public constructor(m: WaitMarkerDirect) := self.m := m;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueResNil; override;
    begin
      var err_handler := g.curr_err_handler;
      l.prev_ev.MultiAttachCallback(true, ()->if not err_handler.HadError(true) then m.SendSignal{$ifdef EventDebug}, $'SendSignal'{$endif});
      Result := new QueueResConstNil(l.prev_ev);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      m.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  WaitMarkerDummy = sealed class(WaitMarkerDirect)
    private executor: WaitMarkerDummyExecutor;
    public constructor := executor := new WaitMarkerDummyExecutor(self);
    
    private function ConvertToQBase: CommandQueueBase; override := executor;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override := sb += #10;
    
  end;
  
static function WaitMarker.Create := new WaitMarkerDummy;

{$endregion WaitMarkerDummy}

{$region ThenMarkerSignal}

type
  DetachedMarkerSignalWrapCommon<TQ> = abstract class(WaitMarkerDirect)
  where TQ: CommandQueueBase;
    protected org: TQ;
    
    public constructor(org: TQ) := self.org := org;
    private constructor := raise new OpenCLABCInternalException;
    
    private function ConvertToQBase: CommandQueueBase; override := org;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      org.ToStringHeader(sb, index);
      sb += #10;
      
    end;
    
  end;
  
  DetachedMarkerSignalCommon<TQ> = record
  where TQ: CommandQueueBase;
    public q: TQ;
    public wrap: DetachedMarkerSignalWrapCommon<TQ>;
    public signal_in_finally: boolean;
    
    public procedure Init(q: TQ; wrap: DetachedMarkerSignalWrapCommon<TQ>; signal_in_finally: boolean);
    begin
      self.q := q;
      self.wrap := wrap;
      self.signal_in_finally := signal_in_finally;
    end;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>) := q.RegisterWaitables(g, prev_hubs);
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    function Invoke<TLData,TR>(g: CLTaskGlobalData; l: TLData; invoke_q: (CLTaskGlobalData,TLData)->TR): TR; where TLData: ICLTaskLocalData; where TR: QueueResBase;
    begin
      Result := invoke_q(g, l);
      var err_handler := g.curr_err_handler;
      var callback: ()->();
      if signal_in_finally then
        callback := wrap.SendSignal else
        callback := ()->if not err_handler.HadError(true) then wrap.SendSignal;
      Result.ev.MultiAttachCallback(true, callback{$ifdef EventDebug}, $'ExecuteMWHandlers'{$endif});
    end;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>);
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      wrap.ToStringHeader(sb, index);
      sb += #10;
      
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  DetachedMarkerSignalWrapperNil = sealed class(DetachedMarkerSignalWrapCommon<CommandQueueNil>)
    
  end;
  DetachedMarkerSignalNil = sealed partial class(CommandQueueNil)
    data: DetachedMarkerSignalCommon<CommandQueueNil>;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := data.RegisterWaitables(g, prev_hubs);
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueResNil; override := data.Invoke(g, l, data.q.Invoke);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    data.ToString(sb, tabs, index, delayed);
    
  end;
  
  DetachedMarkerSignalWrapper<T> = sealed class(DetachedMarkerSignalWrapCommon<CommandQueue<T>>)
    
  end;
  DetachedMarkerSignal<T> = sealed partial class(CommandQueue<T>)
    data: DetachedMarkerSignalCommon<CommandQueue<T>>;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := data.RegisterWaitables(g, prev_hubs);
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override := data.Invoke(g, l, data.q.Invoke);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    data.ToString(sb, tabs, index, delayed);
    
  end;
  
function DetachedMarkerSignalNil.get_signal_in_finally := data.signal_in_finally;
function DetachedMarkerSignal<T>.get_signal_in_finally := data.signal_in_finally;

constructor DetachedMarkerSignalNil.Create(q: CommandQueueNil; signal_in_finally: boolean) :=
data.Init(q, new DetachedMarkerSignalWrapperNil(self), signal_in_finally);
constructor DetachedMarkerSignal<T>.Create(q: CommandQueue<T>; signal_in_finally: boolean) :=
data.Init(q, new DetachedMarkerSignalWrapper<T>(self), signal_in_finally);

static function DetachedMarkerSignalNil.operator implicit(dms: DetachedMarkerSignalNil) := dms.data.wrap;
static function DetachedMarkerSignal<T>.operator implicit(dms: DetachedMarkerSignal<T>) := dms.data.wrap;

{$endregion ThenMarkerSignal}

{$region WaitFor}

type
  CommandQueueWaitFor = sealed class(CommandQueueNil)
    public marker: WaitMarker;
    public constructor(marker: WaitMarker) := self.marker := marker;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueResNil; override :=
    new QueueResConstNil(marker.MakeWaitEv(g,l));
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    marker.InitInnerHandles(g);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      marker.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
function WaitFor(marker: WaitMarker) := new CommandQueueWaitFor(marker);

{$endregion WaitFor}

{$region ThenWaitFor}

type
  CommandQueueThenBaseWaitFor<T> = abstract class(CommandQueue<T>)
    public q: CommandQueue<T>;
    public marker: WaitMarker;
    
    public constructor(q: CommandQueue<T>; marker: WaitMarker);
    begin
      self.q := q;
      self.marker := marker;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      q.RegisterWaitables(g, prev_hubs);
      marker.InitInnerHandles(g);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
      marker.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  CommandQueueThenWaitFor<T> = sealed class(CommandQueueThenBaseWaitFor<T>)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      Result := q.Invoke(g, l);
      l.prev_ev := Result.ev;
      Result := Result.TrySetEv( marker.MakeWaitEv(g, CLTaskLocalDataNil(l)) );
    end;
    
  end;
  CommandQueueThenFinallyWaitFor<T> = sealed class(CommandQueueThenBaseWaitFor<T>)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      var origin_err_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      Result := q.Invoke(g, l);
      var q_err_handler := g.curr_err_handler;
      
      l.prev_ev := Result.ev;
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      Result := Result.TrySetEv( marker.MakeWaitEv(g, CLTaskLocalDataNil(l)) );
      var m_err_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(origin_err_handler, |q_err_handler, m_err_handler|);
    end;
    
  end;
  
function CommandQueue<T>.ThenWaitFor(marker: WaitMarker) := new CommandQueueThenWaitFor<T>(self, marker);
function CommandQueue<T>.ThenFinallyWaitFor(marker: WaitMarker) := new CommandQueueThenFinallyWaitFor<T>(self, marker);

{$endregion ThenWaitFor}

{$endregion Wait}

{$region Finally+Handle}

{$region Finally}

type
  CommandQueueTryFinallyCommon<TQ> = record
  where TQ: CommandQueueBase;
    public try_do: CommandQueueBase;
    public do_finally: TQ;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>);
    begin
      try_do.RegisterWaitables(g, prev_hubs);
      do_finally.RegisterWaitables(g, prev_hubs);
    end;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    function Invoke<TLData,TR>(g: CLTaskGlobalData; l: TLData; invoke_finally: (CLTaskGlobalData,TLData)->TR): TR; where TLData: ICLTaskLocalData; where TR: QueueResBase;
    begin
      var origin_err_handler := g.curr_err_handler;
      
      {$region try_do}
      var mid_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'mid_ev for {self.GetType}'{$endif});
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      var try_ev := try_do.InvokeBase(g, l.WithPtrNeed(false)).ThenInvokeIfDelegateRes(g, false).ev;
      var try_handler := g.curr_err_handler;
      
      try_ev.MultiAttachCallback(false, ()->mid_ev.SetComplete(){$ifdef EventDebug}, $'Set mid_ev {mid_ev}'{$endif});
      
      {$endregion try_do}
      
      {$region do_finally}
      l.PrevEv := mid_ev;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      Result := invoke_finally(g, l);
      var fin_handler := g.curr_err_handler;
      
      {$endregion do_finally}
      
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(origin_err_handler, |try_handler, fin_handler|);
    end;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>);
    begin
      sb += #10;
      try_do.ToString(sb, tabs, index, delayed);
      do_finally.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  CommandQueueTryFinallyNil = sealed class(CommandQueueNil)
    private data := new CommandQueueTryFinallyCommon< CommandQueueNil >;
    
    private constructor(try_do: CommandQueueBase; do_finally: CommandQueueNil);
    begin
      data.try_do := try_do;
      data.do_finally := do_finally;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    data.RegisterWaitables(g, prev_hubs);
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueResNil; override := data.Invoke(g, l, data.do_finally.Invoke);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    data.ToString(sb, tabs, index, delayed);
    
  end;
  CommandQueueTryFinally<T> = sealed class(CommandQueue<T>)
    private data := new CommandQueueTryFinallyCommon< CommandQueue<T> >;
    
    private constructor(try_do: CommandQueueBase; do_finally: CommandQueue<T>);
    begin
      data.try_do := try_do;
      data.do_finally := do_finally;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    data.RegisterWaitables(g, prev_hubs);
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override := data.Invoke(g, l, data.do_finally.Invoke);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    data.ToString(sb, tabs, index, delayed);
    
  end;
  
static function CommandQueueNil.operator>=(try_do: CommandQueueBase; do_finally: CommandQueueNil) :=
new CommandQueueTryFinallyNil(try_do, do_finally);
static function CommandQueue<T>.operator>=(try_do: CommandQueueBase; do_finally: CommandQueue<T>) :=
new CommandQueueTryFinally<T>(try_do, do_finally);

{$endregion Finally}

{$region Non-Finally}

type
  
  CommandQueueHandleWithoutRes = sealed class(CommandQueueNil)
    private q: CommandQueueBase;
    private handler: Exception->boolean;
    
    public constructor(q: CommandQueueBase; handler: Exception->boolean);
    begin
      self.q := q;
      self.handler := handler;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueResNil; override;
    begin
      var origin_err_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      var q_ev := q.InvokeBase(g, l.WithPtrNeed(false)).ThenInvokeIfDelegateRes(g, false).ev;
      var q_err_handler := g.curr_err_handler;
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(origin_err_handler, |q_err_handler|);
      
      var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for {self.GetType}'{$endif});
      q_ev.MultiAttachCallback(false, ()->
      begin
        q_err_handler.TryRemoveErrors(handler);
        res_ev.SetComplete;
      end{$ifdef EventDebug}, $'Set res_ev {res_ev}'{$endif});
      
      Result := new QueueResConstNil(res_ev);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      q.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, handler);
      sb += #10;
      
    end;
    
  end;
  
  CommandQueueHandleDefaultRes<T> = sealed class(CommandQueue<T>)
    private q: CommandQueue<T>;
    private handler: Exception->boolean;
    private def: T;
    
    public constructor(q: CommandQueue<T>; handler: Exception->boolean; def: T);
    begin
      self.q := q;
      self.handler := handler;
      self.def := def;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      var origin_err_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      var prev_qr := q.Invoke(g, l.WithPtrNeed(false));
      var q_err_handler := g.curr_err_handler;
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(origin_err_handler, |q_err_handler|);
      
      var res := QueueRes&<T>.MakeNewDelayedOrPtr(l.need_ptr_qr);
      var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for {self.GetType}'{$endif});
      res.ev := res_ev;
      
      prev_qr.ev.MultiAttachCallback(false, ()->
      begin
        if not q_err_handler.HadError(true) then
          res.SetRes(prev_qr.GetRes) else
        begin
          q_err_handler.TryRemoveErrors(handler);
          if not q_err_handler.HadError(true) then
            res.SetRes(def);
        end;
        res_ev.SetComplete;
      end{$ifdef EventDebug}, $'Set res_ev {res_ev}'{$endif});
      
      Result := res;
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ' => ';
      sb.Append(def);
      sb += #10;
      
      q.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, handler);
      sb += #10;
      
    end;
    
  end;
  
  CommandQueueHandleReplaceRes<T> = sealed class(CommandQueue<T>)
    private q: CommandQueue<T>;
    private handler: List<Exception> -> T;
    
    public constructor(q: CommandQueue<T>; handler: List<Exception> -> T);
    begin
      self.q := q;
      self.handler := handler;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      var origin_err_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      var prev_qr := q.Invoke(g, l.WithPtrNeed(false));
      var q_err_handler := new CLTaskErrHandlerThief(g.curr_err_handler);
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(origin_err_handler, new CLTaskErrHandler[](q_err_handler));
      
      var res := QueueRes&<T>.MakeNewDelayedOrPtr(l.need_ptr_qr);
      var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for {self.GetType}'{$endif});
      res.ev := res_ev;
      
      prev_qr.ev.MultiAttachCallback(false, ()->
      begin
        if not q_err_handler.HadError(true) then
          res.SetRes(prev_qr.GetRes) else
        begin
          q_err_handler.StealPrevErrors;
          var err_lst := q_err_handler.get_local_err_lst;
          var handler_res := handler(err_lst);
          if err_lst.Count=0 then res.SetRes(handler_res);
        end;
        res_ev.SetComplete;
      end{$ifdef EventDebug}, $'Set res_ev {res_ev}'{$endif});
      
      Result := res;
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      q.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, handler);
      sb += #10;
      
    end;
    
  end;
  
function CommandQueueBase.HandleWithoutRes(handler: Exception->boolean) :=
new CommandQueueHandleWithoutRes(self, handler);

function CommandQueue<T>.HandleDefaultRes(handler: Exception->boolean; def: T): CommandQueue<T> :=
new CommandQueueHandleDefaultRes<T>(self, handler, def);

function CommandQueue<T>.HandleReplaceRes(handler: List<Exception> -> T) :=
new CommandQueueHandleReplaceRes<T>(self, handler);

{$endregion Non-Finally}

{$endregion Finally+Handle}

{$endregion Queue converter's}

{$region KernelArg}

{$region Base}

type
  ISetableKernelArg = interface
    procedure SetArg(k: cl_kernel; ind: UInt32);
  end;
  KernelArg = abstract partial class
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<ISetableKernelArg>; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); abstract;
    
  end;
  
{$endregion Base}

{$region Const}

{$region Base}

type
  ConstKernelArg = abstract class(KernelArg, ISetableKernelArg)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<ISetableKernelArg>; override :=
    new QueueResConst<ISetableKernelArg>(self, EventList.Empty);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    public procedure SetArg(k: cl_kernel; ind: UInt32); abstract;
    
  end;
  
{$endregion Base}

{$region CLArray}

type
  KernelArgCLArray<T> = sealed class(ConstKernelArg)
  where T: record;
    private a: CLArray<T>;
    
    public constructor(a: CLArray<T>) := self.a := a;
    private constructor := raise new OpenCLABCInternalException;
    
    public procedure SetArg(k: cl_kernel; ind: UInt32); override :=
    OpenCLABCInternalException.RaiseIfError( cl.SetKernelArg(k, ind, new UIntPtr(cl_mem.Size), a.ntv) );
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ' => ';
      sb.Append(a);
      sb += #10;
    end;
    
  end;
  
static function KernelArg.FromCLArray<T>(a: CLArray<T>): KernelArg; where T: record;
begin Result := new KernelArgCLArray<T>(a); end;

{$endregion CLArray}

{$region MemorySegment}

type
  KernelArgMemorySegment = sealed class(ConstKernelArg)
    private mem: MemorySegment;
    
    public constructor(mem: MemorySegment) := self.mem := mem;
    private constructor := raise new OpenCLABCInternalException;
    
    public procedure SetArg(k: cl_kernel; ind: UInt32); override :=
   OpenCLABCInternalException.RaiseIfError( cl.SetKernelArg(k, ind, new UIntPtr(cl_mem.Size), mem.ntv) );
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ' => ';
      sb.Append(mem);
      sb += #10;
    end;
    
  end;
  
static function KernelArg.FromMemorySegment(mem: MemorySegment) := new KernelArgMemorySegment(mem);

{$endregion MemorySegment}

{$region Ptr}

type
  KernelArgData = sealed class(ConstKernelArg)
    private ptr: IntPtr;
    private sz: UIntPtr;
    
    public constructor(ptr: IntPtr; sz: UIntPtr);
    begin
      self.ptr := ptr;
      self.sz := sz;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    public procedure SetArg(k: cl_kernel; ind: UInt32); override :=
    OpenCLABCInternalException.RaiseIfError( cl.SetKernelArg(k, ind, sz, pointer(ptr)) );
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ' => ';
      sb.Append(ptr);
      sb += '[';
      sb.Append(sz);
      sb += ']'#10;
    end;
    
  end;
  
static function KernelArg.FromData(ptr: IntPtr; sz: UIntPtr) := new KernelArgData(ptr, sz);

static function KernelArg.FromValueData<TRecord>(ptr: ^TRecord): KernelArg;
begin
  BlittableHelper.RaiseIfBad(typeof(TRecord), 'передавать в качестве параметров kernel''а');
  Result := KernelArg.FromData(new IntPtr(ptr), new UIntPtr(Marshal.SizeOf&<TRecord>));
end;

{$endregion Ptr}

{$region Value}

type
  KernelArgValue<TRecord> = sealed class(ConstKernelArg)
  where TRecord: record;
    private val: ^TRecord := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>));
    
    static constructor :=
    BlittableHelper.RaiseIfBad(typeof(TRecord), 'передавать в качестве параметров kernel''а');
    
    public constructor(val: TRecord) := self.val^ := val;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure Finalize; override :=
    Marshal.FreeHGlobal(new IntPtr(val));
    
    public procedure SetArg(k: cl_kernel; ind: UInt32); override :=
    OpenCLABCInternalException.RaiseIfError( cl.SetKernelArg(k, ind, new UIntPtr(Marshal.SizeOf&<TRecord>), pointer(self.val)) ); 
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ' => ';
      sb.Append(val^);
      sb += #10;
    end;
    
  end;
  
static function KernelArg.FromValue<TRecord>(val: TRecord) := new KernelArgValue<TRecord>(val);

{$endregion Value}

{$region NativeValue}

type
  KernelArgNativeValue<TRecord> = sealed class(ConstKernelArg)
  where TRecord: record;
    private val: NativeValue<TRecord>;
    
    public constructor(val: NativeValue<TRecord>) := self.val := val;
    private constructor := raise new OpenCLABCInternalException;
    
    public procedure SetArg(k: cl_kernel; ind: UInt32); override :=
    OpenCLABCInternalException.RaiseIfError( cl.SetKernelArg(k, ind, new UIntPtr(Marshal.SizeOf&<TRecord>), self.val.Pointer) ); 
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ' => ';
      sb += val.ToString;
      sb += #10;
    end;
    
  end;
  
static function KernelArg.FromNativeValue<TRecord>(val: NativeValue<TRecord>): KernelArg; where TRecord: record;
begin
  Result := new KernelArgNativeValue<TRecord>(val);
end;

{$endregion NativeValue}

{$region Array}

type
  KernelArgArray<TRecord> = sealed class(ConstKernelArg)
  where TRecord: record;
    private hnd: GCHandle;
    private offset: integer;
    
    static constructor :=
    BlittableHelper.RaiseIfBad(typeof(TRecord), 'передавать в качестве параметров kernel''а');
    
    public constructor(a: array of TRecord; ind: integer);
    begin
      self.hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
      self.offset := Marshal.SizeOf&<TRecord> * ind;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure Finalize; override :=
    if hnd.IsAllocated then hnd.Free;
    
    public procedure SetArg(k: cl_kernel; ind: UInt32); override :=
    OpenCLABCInternalException.RaiseIfError( cl.SetKernelArg(k, ind, new UIntPtr(Marshal.SizeOf&<TRecord>), (hnd.AddrOfPinnedObject+offset).ToPointer) ); 
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ' => ';
      sb.Append(_ObjectToString(hnd.Target));
      if offset<>0 then
      begin
        sb += '+';
        sb.Append(offset);
        sb += 'b';
      end;
      sb += #10;
    end;
    
  end;
  
static function KernelArg.FromArray<TRecord>(a: array of TRecord; ind: integer) := new KernelArgArray<TRecord>(a, ind);

{$endregion Array}

{$endregion Const}

{$region Invokeable}

{$region Base}

type
  InvokeableKernelArg = abstract class(KernelArg) end;
  
{$endregion Base}

{$region CLArray}

type
  KernelArgCLArrayCQ<T> = sealed class(InvokeableKernelArg)
  where T: record;
    public q: CommandQueue<CLArray<T>>;
    public constructor(q: CommandQueue<CLArray<T>>) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<ISetableKernelArg>; override :=
    q.Invoke(g, l.WithPtrNeed(false)).LazyQuickTransform(a->new KernelArgCLArray<T>(a) as ISetableKernelArg);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
static function KernelArg.FromCLArrayCQ<T>(a_q: CommandQueue<CLArray<T>>): KernelArg; where T: record;
begin Result := new KernelArgCLArrayCQ<T>(a_q); end;

{$endregion CLArray}

{$region MemorySegment}

type
  KernelArgMemorySegmentCQ = sealed class(InvokeableKernelArg)
    public q: CommandQueue<MemorySegment>;
    public constructor(q: CommandQueue<MemorySegment>) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<ISetableKernelArg>; override :=
    q.Invoke(g, l.WithPtrNeed(false)).LazyQuickTransform(mem->new KernelArgMemorySegment(mem) as ISetableKernelArg);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
static function KernelArg.FromMemorySegmentCQ(mem_q: CommandQueue<MemorySegment>) :=
new KernelArgMemorySegmentCQ(mem_q);

{$endregion MemorySegment}

{$region Ptr}

type
  KernelArgDataCQ = sealed class(InvokeableKernelArg)
    public ptr_q: CommandQueue<IntPtr>;
    public sz_q: CommandQueue<UIntPtr>;
    public constructor(ptr_q: CommandQueue<IntPtr>; sz_q: CommandQueue<UIntPtr>);
    begin
      self.ptr_q := ptr_q;
      self.sz_q := sz_q;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<ISetableKernelArg>; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      var  sz_qr: QueueRes<UIntPtr>;
      g.ParallelInvoke(l.WithPtrNeed(false), true, 2, invoker->
      begin
        ptr_qr := invoker.InvokeBranch(ptr_q.Invoke);
         sz_qr := invoker.InvokeBranch( sz_q.Invoke);
      end);
      var res_ev := ptr_qr.ev+sz_qr.ev;
      //TODO #2604
      var b := (ptr_qr is QueueResConst<IntPtr>(var ptr_c_qr)) and (sz_qr is QueueResConst<UIntPtr>(var sz_c_qr));
      if b then
        Result := new QueueResConst<ISetableKernelArg>(new KernelArgData(ptr_c_qr.res, sz_c_qr.res), res_ev) else
        Result := new QueueResFunc<ISetableKernelArg>(()->new KernelArgData(ptr_qr.GetRes, sz_qr.GetRes), res_ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr_q.RegisterWaitables(g, prev_hubs);
       sz_q.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      ptr_q.ToString(sb, tabs, index, delayed);
       sz_q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
static function KernelArg.FromDataCQ(ptr_q: CommandQueue<IntPtr>; sz_q: CommandQueue<UIntPtr>) :=
new KernelArgDataCQ(ptr_q, sz_q);

{$endregion Ptr}

{$region Value}

type
  KernelArgPtrQr<TRecord> = sealed class(ConstKernelArg)
    public qr: QueueResDelayedPtr<TRecord>;
    
    public constructor(qr: QueueResDelayedPtr<TRecord>) := self.qr := qr;
    private constructor := raise new OpenCLABCInternalException;
    
    public procedure SetArg(k: cl_kernel; ind: UInt32); override :=
    OpenCLABCInternalException.RaiseIfError( cl.SetKernelArg(k, ind, new UIntPtr(Marshal.SizeOf&<TRecord>), qr.ptr) );
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    raise new System.NotSupportedException;
    
  end;
  
  KernelArgValueCQ<TRecord> = sealed class(InvokeableKernelArg)
  where TRecord: record;
    public q: CommandQueue<TRecord>;
    
    static constructor :=
    BlittableHelper.RaiseIfBad(typeof(TRecord), 'передавать в качестве параметров kernel''а');
    
    public constructor(q: CommandQueue<TRecord>) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<ISetableKernelArg>; override;
    begin
      var prev_qr := q.Invoke(g, l.WithPtrNeed(true));
      if prev_qr is QueueResDelayedPtr<TRecord>(var ptr_qr) then
        Result := new QueueResConst<ISetableKernelArg>(new KernelArgPtrQr<TRecord>(ptr_qr), ptr_qr.ev) else
        Result := prev_qr.LazyQuickTransform(val->new KernelArgValue<TRecord>(val) as ISetableKernelArg);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
static function KernelArg.FromValueCQ<TRecord>(valq: CommandQueue<TRecord>) :=
new KernelArgValueCQ<TRecord>(valq);

{$endregion Value}

{$region NativeValue}

type
  KernelArgNativeValueCQ<TRecord> = sealed class(InvokeableKernelArg)
  where TRecord: record;
    public q: CommandQueue<NativeValue<TRecord>>;
    
    public constructor(q: CommandQueue<NativeValue<TRecord>>) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<ISetableKernelArg>; override :=
    q.Invoke(g, l.WithPtrNeed(false)).LazyQuickTransform(nv->new KernelArgNativeValue<TRecord>(nv) as ISetableKernelArg);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
static function KernelArg.FromNativeValueCQ<TRecord>(valq: CommandQueue<NativeValue<TRecord>>): KernelArg; where TRecord: record;
begin
  Result := new KernelArgNativeValueCQ<TRecord>(valq);
end;

{$endregion NativeValue}

{$region Array}

type
  KernelArgArrayCQ<TRecord> = sealed class(InvokeableKernelArg)
  where TRecord: record;
    public a_q: CommandQueue<array of TRecord>;
    public ind_q: CommandQueue<integer>;
    
    static constructor :=
    BlittableHelper.RaiseIfBad(typeof(TRecord), 'передавать в качестве параметров kernel''а');
    
    public constructor(a_q: CommandQueue<array of TRecord>; ind_q: CommandQueue<integer>);
    begin
      self.  a_q :=   a_q;
      self.ind_q := ind_q;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<ISetableKernelArg>; override;
    begin
      var   a_qr: QueueRes<array of TRecord>;
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(l.WithPtrNeed(false), true, 2, invoker->
      begin
          a_qr := invoker.InvokeBranch(  a_q.Invoke);
        ind_qr := invoker.InvokeBranch(ind_q.Invoke);
      end);
      var res_ev := a_qr.ev+ind_qr.ev;
      //TODO #2604
      var b := (a_qr is QueueResConst<array of TRecord>(var a_c_qr)) and (ind_qr is QueueResConst<integer>(var ind_c_qr));
      if b then
        Result := new QueueResConst<ISetableKernelArg>(new KernelArgArray<TRecord>(a_c_qr.res, ind_c_qr.res), res_ev) else
        Result := new QueueResFunc<ISetableKernelArg>(()->new KernelArgArray<TRecord>(a_qr.GetRes, ind_qr.GetRes), res_ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
        a_q.RegisterWaitables(g, prev_hubs);
      ind_q.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
        a_q.ToString(sb, tabs, index, delayed);
      ind_q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
static function KernelArg.FromArrayCQ<TRecord>(a_q: CommandQueue<array of TRecord>; ind_q: CommandQueue<integer>) :=
new KernelArgArrayCQ<TRecord>(a_q, ind_q);

{$endregion Array}

{$endregion Invokeable}

{$endregion KernelArg}

{$region GPUCommand}

{$region Base}

type
  GPUCommandObjInvoker<T> = (CLTaskGlobalData,CLTaskLocalDataNil)->QueueRes<T>;
  
  GPUCommand<T> = abstract class
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); abstract;
    
    protected function InvokeObj  (o: T;                              g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; abstract;
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<T>; g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; abstract;
    
    protected function DisplayName: string; virtual := CommandQueueBase.DisplayNameForType(self.GetType);
    protected static procedure ToStringWriteDelegate(sb: StringBuilder; d: System.Delegate) := CommandQueueBase.ToStringWriteDelegate(sb,d);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); abstract;
    
    private procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>);
    begin
      sb.Append(#9, tabs);
      sb += DisplayName;
      self.ToStringImpl(sb, tabs+1, index, delayed);
    end;
    
  end;
  
  BasicGPUCommand<T> = abstract class(GPUCommand<T>)
    
    protected function DisplayName: string; override;
    begin
      Result := self.GetType.Name;
      Result := Result.Remove(Result.IndexOf('`'));
    end;
    
    public static function MakeQueue(q: CommandQueueBase): BasicGPUCommand<T>;
    
    public static function MakeBackgroundProc(p: T->()): BasicGPUCommand<T>;
    public static function MakeBackgroundProc(p: (T,Context)->()): BasicGPUCommand<T>;
    public static function MakeQuickProc(p: T->()): BasicGPUCommand<T>;
    public static function MakeQuickProc(p: (T,Context)->()): BasicGPUCommand<T>;
    
    public static function MakeWait(m: WaitMarker): BasicGPUCommand<T>;
    
  end;
  
{$endregion Base}

{$region Queue}

type
  QueueCommandCommon<TObj,TQ> = abstract class(BasicGPUCommand<TObj>)
  where TQ: CommandQueueBase;
    public q: TQ;
    
    public constructor(q: TQ) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; abstract;
    
    protected function InvokeObj  (o: TObj;                              g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override := Invoke(g, l);
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<TObj>; g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override := Invoke(g, l);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  QueueCommandNil<TObj> = sealed class(QueueCommandCommon<TObj,CommandQueueNil>)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override :=
    q.Invoke(g, l).ThenInvokeIfProcRes(g).ev;
    
  end;
  QueueCommand<TObj,TQRes> = sealed class(QueueCommandCommon<TObj,CommandQueue<TQRes>>)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override :=
    q.Invoke(g, l.WithPtrNeed(false)).ThenInvokeIfFuncRes(g,false).ev;
    
  end;
  
  QueueCommandFactory<TObj> = sealed class(ITypedCQConverter<BasicGPUCommand<TObj>>)
    
    public function ConvertNil(cq: CommandQueueNil): BasicGPUCommand<TObj> := new QueueCommandNil<TObj>(cq);
    public function Convert<T>(cq: CommandQueue<T>): BasicGPUCommand<TObj> :=
    if cq is ConstQueue<T> then nil else
    if cq is CastQueueBase<T>(var ccq) then
      ccq.SourceBase.ConvertTyped(self) else
      new QueueCommand<TObj,T>(cq);
    
  end;
  
static function BasicGPUCommand<T>.MakeQueue(q: CommandQueueBase) := q.ConvertTyped(new QueueCommandFactory<T>);

{$endregion Queue}

{$region Proc}

{$region Base}

type
  ProcCommandBase<T, TProc> = abstract class(BasicGPUCommand<T>)
  where TProc: Delegate;
    public p: TProc;
    
    public constructor(p: TProc) := self.p := p;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure ExecProc(o: T; c: Context); abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ': ';
      ToStringWriteDelegate(sb, p);
      sb += #10;
    end;
    
  end;
  
{$endregion Base}

type
  BackgroundProcCommandBase<T, TProc> = abstract class(ProcCommandBase<T, TProc>)
  where TProc: Delegate;
    
    protected function InvokeObj(o: T; g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override :=
    UserEvent.StartBackgroundWork(l.prev_ev, ()->ExecProc(o, g.c), g
      {$ifdef EventDebug}, $'const body of {self.GetType}'{$endif}
    );
    
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<T>; g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override;
    begin
      var o_q_res := o_invoke(g, l);
      Result := UserEvent.StartBackgroundWork(o_q_res.ev, ()->ExecProc(o_q_res.GetRes(), g.c), g
        {$ifdef EventDebug}, $'queue body of {self.GetType}'{$endif}
      );
    end;
    
  end;
  
  BackgroundProcCommand<T> = sealed class(BackgroundProcCommandBase<T, T->()>)
    
    protected procedure ExecProc(o: T; c: Context); override := p(o);
    
  end;
  BackgroundProcCommandC<T> = sealed class(BackgroundProcCommandBase<T, (T,Context)->()>)
    
    protected procedure ExecProc(o: T; c: Context); override := p(o, c);
    
  end;
  
static function BasicGPUCommand<T>.MakeBackgroundProc(p: T->()) := new BackgroundProcCommand<T>(p);
static function BasicGPUCommand<T>.MakeBackgroundProc(p: (T,Context)->()) := new BackgroundProcCommandC<T>(p);

type
  QuickProcCommandBase<T, TProc> = abstract class(ProcCommandBase<T, TProc>)
  where TProc: Delegate;
    
    protected function InvokeObj(o: T; g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override;
    begin
      var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for {self.GetType}'{$endif});
      
      var c := g.c;
      var err_handler := g.curr_err_handler;
      l.prev_ev.MultiAttachCallback(false, ()->
      begin
        if not err_handler.HadError(true) then
        try
          ExecProc(o, c);
        except
          on e: Exception do err_handler.AddErr(e);
        end;
        res_ev.SetComplete;
      end{$ifdef EventDebug}, $'const body of {self.GetType}'{$endif});
      
      Result := res_ev;
    end;
    
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<T>; g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override;
    begin
      var prev_qr := o_invoke(g, l);
      {$ifdef DEBUG}
      // prev_qr.GetRes could be called >1 time
      // But o_invoke wouldn't return QueueResFunc,
      // because multiusable uses .ThenInvokeIfDelegateRes
      if prev_qr is QueueResFunc<T> then raise new System.NotSupportedException;
      {$endif DEBUG}
      var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for {self.GetType}'{$endif});
      
      var c := g.c;
      var err_handler := g.curr_err_handler;
      prev_qr.ev.MultiAttachCallback(false, ()->
      begin
        if not err_handler.HadError(true) then
        try
          ExecProc(prev_qr.GetRes, c);
        except
          on e: Exception do err_handler.AddErr(e);
        end;
        res_ev.SetComplete;
      end{$ifdef EventDebug}, $'queue body of {self.GetType}'{$endif});
      
      Result := res_ev;
    end;
    
  end;
  
  QuickProcCommand<T> = sealed class(QuickProcCommandBase<T, T->()>)
    
    protected procedure ExecProc(o: T; c: Context); override := p(o);
    
  end;
  QuickProcCommandC<T> = sealed class(QuickProcCommandBase<T, (T,Context)->()>)
    
    protected procedure ExecProc(o: T; c: Context); override := p(o, c);
    
  end;
  
static function BasicGPUCommand<T>.MakeQuickProc(p: T->()) := new QuickProcCommand<T>(p);
static function BasicGPUCommand<T>.MakeQuickProc(p: (T,Context)->()) := new QuickProcCommandC<T>(p);

{$endregion Proc}

{$region Wait}

type
  WaitCommand<T> = sealed class(BasicGPUCommand<T>)
    public marker: WaitMarker;
    
    public constructor(marker: WaitMarker) := self.marker := marker;
    private constructor := raise new OpenCLABCInternalException;
    
    private function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil) := marker.MakeWaitEv(g, l);
    
    protected function InvokeObj  (o: T;                              g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override := Invoke(g, l);
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<T>; g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override := Invoke(g, l);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    marker.InitInnerHandles(g);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      marker.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
static function BasicGPUCommand<T>.MakeWait(m: WaitMarker) := new WaitCommand<T>(m);

{$endregion Wait}

{$endregion GPUCommand}

{$region GPUCommandContainer}

{$region Base}

type
  GPUCommandContainer<T> = abstract partial class
    private constructor := raise new OpenCLABCInternalException;
  end;
  GPUCommandContainerCore<T> = abstract class
    private cc: GPUCommandContainer<T>;
    protected constructor(cc: GPUCommandContainer<T>) := self.cc := cc;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<T>; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); abstract;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); abstract;
    private procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>);
    begin
      sb.Append(#9, tabs);
      
      var tn := self.GetType.Name;
      sb += tn.Remove(tn.IndexOf('`'));
      
      self.ToStringImpl(sb, tabs+1, index, delayed);
    end;
    
  end;
  
  GPUCommandContainer<T> = abstract partial class(CommandQueue<T>)
    protected core: GPUCommandContainerCore<T>;
    protected commands := new List<GPUCommand<T>>;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      {$ifdef DEBUG}
      l.CheckInvalidNeedPtrQr(self);
      {$endif DEBUG}
      Result := core.Invoke(g, CLTaskLocalDataNil(l));
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      core.RegisterWaitables(g, prev_hubs);
      foreach var comm in commands do comm.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      core.ToString(sb, tabs, index, delayed);
      foreach var comm in commands do
        comm.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
function AddCommand<TContainer, TRes>(cc: TContainer; comm: GPUCommand<TRes>): TContainer; where TContainer: GPUCommandContainer<TRes>;
begin
  cc.commands += comm;
  Result := cc;
end;

{$endregion Base}

{$region Core}

type
  CCCObj<T> = sealed class(GPUCommandContainerCore<T>)
    public o: T;
    
    public constructor(cc: GPUCommandContainer<T>; o: T);
    begin
      inherited Create(cc);
      self.o := o;
    end;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<T>; override;
    begin
      var o := self.o;
      
      foreach var comm in cc.commands do
        l.prev_ev := comm.InvokeObj(o, g, l);
      
      Result := new QueueResConst<T>(o, l.prev_ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ' => ';
      sb.Append(o);
      sb += #10;
    end;
    
  end;
  
  CCCQueue<T> = sealed class(GPUCommandContainerCore<T>)
    public hub: MultiusableCommandQueueHub<T>;
    
    public constructor(cc: GPUCommandContainer<T>; q: CommandQueue<T>);
    begin
      inherited Create(cc);
      self.hub := new MultiusableCommandQueueHub<T>(q);
    end;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueRes<T>; override;
    begin
      var invoke_plug: GPUCommandObjInvoker<T> := (g,l)->hub.MakeNode.Invoke(g,l.WithPtrNeed(false));
      
      foreach var comm in cc.commands do
        l.prev_ev := comm.InvokeQueue(invoke_plug, g, l);
      
      Result := invoke_plug(g, l);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    hub.q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      hub.q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  GPUCommandContainer<T> = abstract partial class
    
    protected constructor(o: T) :=
    self.core := new CCCObj<T>(self, o);
    
    protected constructor(q: CommandQueue<T>) :=
    self.core := new CCCQueue<T>(self, q);
    
  end;
  
{$endregion Core}

{$region Kernel}

type
  KernelCCQ = sealed partial class(GPUCommandContainer<Kernel>)
    
  end;
  
constructor KernelCCQ.Create(o: Kernel) := inherited;
constructor KernelCCQ.Create(q: CommandQueue<Kernel>) := inherited;
constructor KernelCCQ.Create := inherited;

{$region Special .Add's}

function KernelCCQ.AddQueue(q: CommandQueueBase): KernelCCQ;
begin
  Result := self;
  var comm := BasicGPUCommand&<Kernel>.MakeQueue(q);
  if comm<>nil then commands.Add(comm);
end;

function KernelCCQ.AddProc(p: Kernel->()) := AddCommand(self, BasicGPUCommand&<Kernel>.MakeBackgroundProc(p));
function KernelCCQ.AddProc(p: (Kernel, Context)->()) := AddCommand(self, BasicGPUCommand&<Kernel>.MakeBackgroundProc(p));
function KernelCCQ.AddQuickProc(p: Kernel->()) := AddCommand(self, BasicGPUCommand&<Kernel>.MakeQuickProc(p));
function KernelCCQ.AddQuickProc(p: (Kernel, Context)->()) := AddCommand(self, BasicGPUCommand&<Kernel>.MakeQuickProc(p));

function KernelCCQ.AddWait(marker: WaitMarker) := AddCommand(self, BasicGPUCommand&<Kernel>.MakeWait(marker));

{$endregion Special .Add's}

{$endregion Kernel}

{$region MemorySegment}

type
  MemorySegmentCCQ = sealed partial class(GPUCommandContainer<MemorySegment>)
    
  end;
  
static function KernelArg.operator implicit(mem_q: MemorySegmentCCQ): KernelArg := FromMemorySegmentCQ(mem_q);

constructor MemorySegmentCCQ.Create(o: MemorySegment) := inherited;
constructor MemorySegmentCCQ.Create(q: CommandQueue<MemorySegment>) := inherited;
constructor MemorySegmentCCQ.Create := inherited;

{$region Special .Add's}

function MemorySegmentCCQ.AddQueue(q: CommandQueueBase): MemorySegmentCCQ;
begin
  Result := self;
  var comm := BasicGPUCommand&<MemorySegment>.MakeQueue(q);
  if comm<>nil then commands.Add(comm);
end;

function MemorySegmentCCQ.AddProc(p: MemorySegment->()) := AddCommand(self, BasicGPUCommand&<MemorySegment>.MakeBackgroundProc(p));
function MemorySegmentCCQ.AddProc(p: (MemorySegment, Context)->()) := AddCommand(self, BasicGPUCommand&<MemorySegment>.MakeBackgroundProc(p));
function MemorySegmentCCQ.AddQuickProc(p: MemorySegment->()) := AddCommand(self, BasicGPUCommand&<MemorySegment>.MakeQuickProc(p));
function MemorySegmentCCQ.AddQuickProc(p: (MemorySegment, Context)->()) := AddCommand(self, BasicGPUCommand&<MemorySegment>.MakeQuickProc(p));

function MemorySegmentCCQ.AddWait(marker: WaitMarker) := AddCommand(self, BasicGPUCommand&<MemorySegment>.MakeWait(marker));

{$endregion Special .Add's}

{$endregion MemorySegment}

{$region CLArray}

type
  CLArrayCCQ<T> = sealed partial class(GPUCommandContainer<CLArray<T>>)
    
  end;
  
static function KernelArg.operator implicit<T>(a_q: CLArrayCCQ<T>): KernelArg; where T: record;
//TODO #2550
begin Result := FromCLArrayCQ(a_q as object as GPUCommandContainer<CLArray<T>>); end;

constructor CLArrayCCQ<T>.Create(o: CLArray<T>) := inherited;
constructor CLArrayCCQ<T>.Create(q: CommandQueue<CLArray<T>>) := inherited;
constructor CLArrayCCQ<T>.Create := inherited;

{$region Special .Add's}

function CLArrayCCQ<T>.AddQueue(q: CommandQueueBase): CLArrayCCQ<T>;
begin
  Result := self;
  var comm := BasicGPUCommand&<CLArray<T>>.MakeQueue(q);
  if comm<>nil then commands.Add(comm);
end;

function CLArrayCCQ<T>.AddProc(p: CLArray<T>->()) := AddCommand(self, BasicGPUCommand&<CLArray<T>>.MakeBackgroundProc(p));
function CLArrayCCQ<T>.AddProc(p: (CLArray<T>, Context)->()) := AddCommand(self, BasicGPUCommand&<CLArray<T>>.MakeBackgroundProc(p));
function CLArrayCCQ<T>.AddQuickProc(p: CLArray<T>->()) := AddCommand(self, BasicGPUCommand&<CLArray<T>>.MakeQuickProc(p));
function CLArrayCCQ<T>.AddQuickProc(p: (CLArray<T>, Context)->()) := AddCommand(self, BasicGPUCommand&<CLArray<T>>.MakeQuickProc(p));

function CLArrayCCQ<T>.AddWait(marker: WaitMarker) := AddCommand(self, BasicGPUCommand&<CLArray<T>>.MakeWait(marker));

{$endregion Special .Add's}

{$endregion CLArray}

{$endregion GPUCommandContainer}

{$region Enqueueable's}

{$region Core}

type
  EnqEvLst = sealed class
    private evs: array of EventList;
    private c1 := 0;
    private c2 := 0;
    {$ifdef DEBUG}
    private skipped := 0;
    {$endif DEBUG}
    
    public constructor(cap: integer) :=
    evs := new EventList[cap];
    private constructor := raise new OpenCLABCInternalException;
    
    public property Capacity: integer read evs.Length;
    
    public procedure AddL1(ev: EventList);
    begin
      {$ifdef DEBUG}
      if c1+c2+skipped = evs.Length then raise new OpenCLABCInternalException($'Not enough EnqEv capacity');
      {$endif DEBUG}
      if ev.count=0 then
        {$ifdef DEBUG}skipped += 1{$endif} else
      begin
        evs[c1] := ev;
        c1 += 1;
      end;
    end;
    public procedure AddL2(ev: EventList);
    begin
      {$ifdef DEBUG}
      if c1+c2+skipped = evs.Length then raise new OpenCLABCInternalException($'Not enough EnqEv capacity');
      {$endif DEBUG}
      if ev.count=0 then
        {$ifdef DEBUG}skipped += 1{$endif} else
      begin
        c2 += 1;
        evs[evs.Length-c2] := ev;
      end;
    end;
    
    public function MakeLists: ValueTuple<EventList, EventList>;
    begin
      {$ifdef DEBUG}
      if c1+c2+skipped <> evs.Length then raise new OpenCLABCInternalException($'Too much EnqEv capacity: {c1+c2}/{evs.Length} used');
      {$endif DEBUG}
      Result := ValueTuple.Create(
        EventList.Combine(new ArraySegment<EventList>(evs,0,c1)),
        EventList.Combine(new ArraySegment<EventList>(evs,evs.Length-c2,c2))
      );
    end;
    
  end;
  
  EnqueueableEnqFunc<TInvData> = function(cq: cl_command_queue; err_handler: CLTaskErrHandler; ev_l2: EventList; inv_data: TInvData): cl_event;
  IEnqueueable<TInvData> = interface
    
    function EnqEvCapacity: integer;
    
    function InvokeParams(g: CLTaskGlobalData; enq_evs: EnqEvLst): EnqueueableEnqFunc<TInvData>;
    
  end;
  
  EnqueueableCore = static class
    
    private static function MakeEvList(exp_size: integer; start_ev: EventList): List<EventList>;
    begin
      var need_start_ev := start_ev.count<>0;
      Result := new List<EventList>(exp_size + integer(need_start_ev));
      if need_start_ev then Result += start_ev;
    end;
    
    private static function ExecuteEnqFunc<TEnq, TInvData>(cq: cl_command_queue; q: TEnq; enq_f: EnqueueableEnqFunc<TInvData>; inv_data: TInvData; ev_l2: EventList; err_handler: CLTaskErrHandler): EventList; where TEnq: IEnqueueable<TInvData>;
    begin
      Result := ev_l2;
      try
        var enq_ev := enq_f(cq, err_handler, ev_l2, inv_data);
        {$ifdef EventDebug}
        EventDebug.RegisterEventRetain(enq_ev, $'Enq by {q.GetType}, waiting on [{ev_l2.evs?.JoinToString}]');
        {$endif EventDebug}
        // 1. ev_l2 can be released only after executing dependant command
        // 2. If event in ev_l2 would receive error, enq_ev would not give descriptive error
        Result := Result+enq_ev;
      except
        on e: Exception do err_handler.AddErr(e);
      end;
    end;
    
    public static function Invoke<TEnq, TInvData>(q: TEnq; inv_data: TInvData; g: CLTaskGlobalData; start_ev: EventList; start_ev_in_l1: boolean): EventList; where TEnq: IEnqueueable<TInvData>;
    begin
      var enq_evs := new EnqEvLst(q.EnqEvCapacity+1);
      if start_ev_in_l1 then
        enq_evs.AddL1(start_ev) else
        enq_evs.AddL2(start_ev);
      
      var pre_params_handler := g.curr_err_handler;
      var enq_f := q.InvokeParams(g, enq_evs);
      var (ev_l1, ev_l2) := enq_evs.MakeLists;
      
      if pre_params_handler.HadError(true) then
      begin
        Result := ev_l1+ev_l2;
        exit;
      end;
      
      // если enq_f асинхронное, чтоб следующая команда не записалась до его вызова - надо полностью забрать очередь
      var cq := g.GetCQ(ev_l1.count<>0);
      {$ifdef QueueDebug}
      QueueDebug.Add(cq, q.GetType.ToString);
      {$endif QueueDebug}
      
      if ev_l1.count=0 then
        Result := ExecuteEnqFunc(cq, q, enq_f, inv_data, ev_l2, g.curr_err_handler) else
      begin
        var res_ev := new UserEvent(g.cl_c
          {$ifdef EventDebug}, $'{q.GetType}, temp for nested AttachCallback: [{ev_l1.evs.JoinToString}], then [{ev_l2.evs?.JoinToString}]'{$endif}
        );
        
        var post_params_handler := g.curr_err_handler;
        ev_l1.MultiAttachCallback(false, ()->
        begin
          // Can't cache, ev_l2 wasn't completed yet
          if post_params_handler.HadError(false) then
          begin
            res_ev.SetComplete;
            g.free_cqs.Add(cq);
            exit;
          end;
          ExecuteEnqFunc(cq, q, enq_f, inv_data, ev_l2, post_params_handler)
          .MultiAttachCallback(false, ()->
          begin
            res_ev.SetComplete;
            g.free_cqs.Add(cq);
          end{$ifdef EventDebug}, $'propagating Enq ev of {q.GetType} to res_ev: {res_ev.uev}'{$endif});
        end{$ifdef EventDebug}, $'calling async Enq of {q.GetType}'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
  end;
  
{$endregion Core}

{$region GPUCommand}

type
  EnqueueableGPUCommandInvData<T> = record
    qr: QueueRes<T>;
  end;
  EnqueueableGPUCommand<T> = abstract class(GPUCommand<T>, IEnqueueable<EnqueueableGPUCommandInvData<T>>)
    
    public function EnqEvCapacity: integer; abstract;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (T, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; abstract;
    public function InvokeParams(g: CLTaskGlobalData; enq_evs: EnqEvLst): EnqueueableEnqFunc<EnqueueableGPUCommandInvData<T>>;
    begin
      var enq_f := InvokeParamsImpl(g, enq_evs);
      Result := (lcq, err_handler, ev, data)->enq_f(data.qr.GetRes, lcq, err_handler, ev);
    end;
    
    protected function Invoke(g: CLTaskGlobalData; prev_qr: QueueRes<T>; start_ev: EventList; start_ev_in_l1: boolean): EventList;
    begin
      var inv_data: EnqueueableGPUCommandInvData<T>;
      inv_data.qr  := prev_qr;
      
      Result := EnqueueableCore.Invoke(self, inv_data, g, start_ev, start_ev_in_l1);
    end;
    
    protected function InvokeObj(o: T; g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override :=
    Invoke(g, new QueueResConst<T>(o, EventList.Empty), l.prev_ev, false);
    
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<T>; g: CLTaskGlobalData; l: CLTaskLocalDataNil): EventList; override;
    begin
      var prev_qr := o_invoke(g, l);
      Result := Invoke(g, prev_qr, prev_qr.ev, not (prev_qr is IQueueResConst));
    end;
    
  end;
  
{$endregion GPUCommand}

{$region GetCommand}

type
  EnqueueableGetCommandInvData<TObj, TRes> = record
    prev_qr: QueueRes<TObj>;
    res_qr: QueueResDelayedBase<TRes>;
  end;
  EnqueueableGetCommand<TObj, TRes> = abstract class(CommandQueue<TRes>, IEnqueueable<EnqueueableGetCommandInvData<TObj, TRes>>)
    protected prev_commands: GPUCommandContainer<TObj>;
    
    public constructor(prev_commands: GPUCommandContainer<TObj>) :=
    self.prev_commands := prev_commands;
    
    public function EnqEvCapacity: integer; abstract;
    
    public function ForcePtrQr: boolean; virtual := false;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (TObj, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<TRes>)->cl_event; abstract;
    public function InvokeParams(g: CLTaskGlobalData; enq_evs: EnqEvLst): EnqueueableEnqFunc<EnqueueableGetCommandInvData<TObj, TRes>>;
    begin
      var enq_f := InvokeParamsImpl(g, enq_evs);
      Result := (lcq, err_handler, ev, data)->enq_f(data.prev_qr.GetRes, lcq, err_handler, ev, data.res_qr);
    end;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var prev_qr := prev_commands.Invoke(g, l.WithPtrNeed(false));
      
      var inv_data: EnqueueableGetCommandInvData<TObj, TRes>;
      inv_data.prev_qr  := prev_qr;
      inv_data.res_qr   := QueueResDelayedBase&<TRes>.MakeNewDelayedOrPtr(l.need_ptr_qr or ForcePtrQr);
      
      Result := inv_data.res_qr;
      Result.ev := EnqueueableCore.Invoke(self, inv_data, g, prev_qr.ev, not (prev_qr is IQueueResConst));
    end;
    
  end;
  
{$endregion GetCommand}

{$region Kernel}

{$region Implicit}

{$region 1#Exec}

function Kernel.Exec1(sz1: CommandQueue<integer>; params args: array of KernelArg): Kernel;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddExec1(sz1, args));
end;

function Kernel.Exec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): Kernel;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddExec2(sz1, sz2, args));
end;

function Kernel.Exec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): Kernel;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddExec3(sz1, sz2, sz3, args));
end;

function Kernel.Exec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): Kernel;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddExec(global_work_offset, global_work_size, local_work_size, args));
end;

{$endregion 1#Exec}

{$endregion Implicit}

{$region Explicit}

{$region 1#Exec}

{$region Exec1}

type
  KernelCommandExec1 = sealed class(EnqueueableGPUCommand<Kernel>)
    private  sz1: CommandQueue<integer>;
    private args: array of KernelArg;
    
    public function EnqEvCapacity: integer; override := 1 + args.Length;
    
    public constructor(sz1: CommandQueue<integer>; params args: array of KernelArg);
    begin
      self. sz1 :=  sz1;
      self.args := args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (Kernel, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var  sz1_qr: QueueRes<integer>;
      var args_qr: array of QueueRes<ISetableKernelArg>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create, true, enq_evs.Capacity-1, invoker->
      begin
         sz1_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->sz1.Invoke(g, l.WithPtrNeed(False))); if (sz1_qr is IQueueResConst) then enq_evs.AddL2(sz1_qr.ev) else enq_evs.AddL1(sz1_qr.ev);
        args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<QueueRes<ISetableKernelArg>>(temp1.Invoke); if (Result is IQueueResConst) then enq_evs.AddL2(Result.ev) else enq_evs.AddL1(Result.ev); end);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var  sz1 :=  sz1_qr.GetRes;
        var args := args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        var ntv := o.UseExclusiveNative(ntv->
        begin
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i);
          
          var ec := cl.EnqueueNDRangeKernel(
            cq, ntv, 1,
            nil,
            new UIntPtr[](new UIntPtr(sz1)),
            nil,
            evs.count, evs.evs, res_ev
          );
          OpenCLABCInternalException.RaiseIfError(ec);
          
          OpenCLABCInternalException.RaiseIfError( cl.RetainKernel(ntv) );
          Result := ntv;
        end);
        
        var args_hnd := GCHandle.Alloc(args);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          OpenCLABCInternalException.RaiseIfError( cl.ReleaseKernel(ntv) );
          args_hnd.Free;
        end{$ifdef EventDebug}, 'custom afterwork'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
       sz1.RegisterWaitables(g, prev_hubs);
      foreach var temp1 in args do temp1.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'sz1: ';
      sz1.ToString(sb, tabs, index, delayed, false);
      
      for var i := 0 to args.Length-1 do 
      begin
        sb.Append(#9, tabs);
        sb += 'args[';
        sb.Append(i);
        sb += ']: ';
        args[i].ToString(sb, tabs, index, delayed, false);
      end;
      
    end;
    
  end;
  
function KernelCCQ.AddExec1(sz1: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
begin
  Result := AddCommand(self, new KernelCommandExec1(sz1, args));
end;

{$endregion Exec1}

{$region Exec2}

type
  KernelCommandExec2 = sealed class(EnqueueableGPUCommand<Kernel>)
    private  sz1: CommandQueue<integer>;
    private  sz2: CommandQueue<integer>;
    private args: array of KernelArg;
    
    public function EnqEvCapacity: integer; override := 2 + args.Length;
    
    public constructor(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg);
    begin
      self. sz1 :=  sz1;
      self. sz2 :=  sz2;
      self.args := args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (Kernel, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var  sz1_qr: QueueRes<integer>;
      var  sz2_qr: QueueRes<integer>;
      var args_qr: array of QueueRes<ISetableKernelArg>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create, true, enq_evs.Capacity-1, invoker->
      begin
         sz1_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->sz1.Invoke(g, l.WithPtrNeed(False))); if (sz1_qr is IQueueResConst) then enq_evs.AddL2(sz1_qr.ev) else enq_evs.AddL1(sz1_qr.ev);
         sz2_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->sz2.Invoke(g, l.WithPtrNeed(False))); if (sz2_qr is IQueueResConst) then enq_evs.AddL2(sz2_qr.ev) else enq_evs.AddL1(sz2_qr.ev);
        args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<QueueRes<ISetableKernelArg>>(temp1.Invoke); if (Result is IQueueResConst) then enq_evs.AddL2(Result.ev) else enq_evs.AddL1(Result.ev); end);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var  sz1 :=  sz1_qr.GetRes;
        var  sz2 :=  sz2_qr.GetRes;
        var args := args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        var ntv := o.UseExclusiveNative(ntv->
        begin
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i);
          
          var ec := cl.EnqueueNDRangeKernel(
            cq, ntv, 2,
            nil,
            new UIntPtr[](new UIntPtr(sz1),new UIntPtr(sz2)),
            nil,
            evs.count, evs.evs, res_ev
          );
          OpenCLABCInternalException.RaiseIfError(ec);
          
          OpenCLABCInternalException.RaiseIfError( cl.RetainKernel(ntv) );
          Result := ntv;
        end);
        
        var args_hnd := GCHandle.Alloc(args);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          OpenCLABCInternalException.RaiseIfError( cl.ReleaseKernel(ntv) );
          args_hnd.Free;
        end{$ifdef EventDebug}, 'custom afterwork'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
       sz1.RegisterWaitables(g, prev_hubs);
       sz2.RegisterWaitables(g, prev_hubs);
      foreach var temp1 in args do temp1.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'sz1: ';
      sz1.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'sz2: ';
      sz2.ToString(sb, tabs, index, delayed, false);
      
      for var i := 0 to args.Length-1 do 
      begin
        sb.Append(#9, tabs);
        sb += 'args[';
        sb.Append(i);
        sb += ']: ';
        args[i].ToString(sb, tabs, index, delayed, false);
      end;
      
    end;
    
  end;
  
function KernelCCQ.AddExec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
begin
  Result := AddCommand(self, new KernelCommandExec2(sz1, sz2, args));
end;

{$endregion Exec2}

{$region Exec3}

type
  KernelCommandExec3 = sealed class(EnqueueableGPUCommand<Kernel>)
    private  sz1: CommandQueue<integer>;
    private  sz2: CommandQueue<integer>;
    private  sz3: CommandQueue<integer>;
    private args: array of KernelArg;
    
    public function EnqEvCapacity: integer; override := 3 + args.Length;
    
    public constructor(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg);
    begin
      self. sz1 :=  sz1;
      self. sz2 :=  sz2;
      self. sz3 :=  sz3;
      self.args := args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (Kernel, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var  sz1_qr: QueueRes<integer>;
      var  sz2_qr: QueueRes<integer>;
      var  sz3_qr: QueueRes<integer>;
      var args_qr: array of QueueRes<ISetableKernelArg>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create, true, enq_evs.Capacity-1, invoker->
      begin
         sz1_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->sz1.Invoke(g, l.WithPtrNeed(False))); if (sz1_qr is IQueueResConst) then enq_evs.AddL2(sz1_qr.ev) else enq_evs.AddL1(sz1_qr.ev);
         sz2_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->sz2.Invoke(g, l.WithPtrNeed(False))); if (sz2_qr is IQueueResConst) then enq_evs.AddL2(sz2_qr.ev) else enq_evs.AddL1(sz2_qr.ev);
         sz3_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->sz3.Invoke(g, l.WithPtrNeed(False))); if (sz3_qr is IQueueResConst) then enq_evs.AddL2(sz3_qr.ev) else enq_evs.AddL1(sz3_qr.ev);
        args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<QueueRes<ISetableKernelArg>>(temp1.Invoke); if (Result is IQueueResConst) then enq_evs.AddL2(Result.ev) else enq_evs.AddL1(Result.ev); end);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var  sz1 :=  sz1_qr.GetRes;
        var  sz2 :=  sz2_qr.GetRes;
        var  sz3 :=  sz3_qr.GetRes;
        var args := args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        var ntv := o.UseExclusiveNative(ntv->
        begin
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i);
          
          var ec := cl.EnqueueNDRangeKernel(
            cq, ntv, 3,
            nil,
            new UIntPtr[](new UIntPtr(sz1),new UIntPtr(sz2),new UIntPtr(sz3)),
            nil,
            evs.count, evs.evs, res_ev
          );
          OpenCLABCInternalException.RaiseIfError(ec);
          
          OpenCLABCInternalException.RaiseIfError( cl.RetainKernel(ntv) );
          Result := ntv;
        end);
        
        var args_hnd := GCHandle.Alloc(args);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          OpenCLABCInternalException.RaiseIfError( cl.ReleaseKernel(ntv) );
          args_hnd.Free;
        end{$ifdef EventDebug}, 'custom afterwork'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
       sz1.RegisterWaitables(g, prev_hubs);
       sz2.RegisterWaitables(g, prev_hubs);
       sz3.RegisterWaitables(g, prev_hubs);
      foreach var temp1 in args do temp1.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'sz1: ';
      sz1.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'sz2: ';
      sz2.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'sz3: ';
      sz3.ToString(sb, tabs, index, delayed, false);
      
      for var i := 0 to args.Length-1 do 
      begin
        sb.Append(#9, tabs);
        sb += 'args[';
        sb.Append(i);
        sb += ']: ';
        args[i].ToString(sb, tabs, index, delayed, false);
      end;
      
    end;
    
  end;
  
function KernelCCQ.AddExec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
begin
  Result := AddCommand(self, new KernelCommandExec3(sz1, sz2, sz3, args));
end;

{$endregion Exec3}

{$region Exec}

type
  KernelCommandExec = sealed class(EnqueueableGPUCommand<Kernel>)
    private global_work_offset: CommandQueue<array of UIntPtr>;
    private   global_work_size: CommandQueue<array of UIntPtr>;
    private    local_work_size: CommandQueue<array of UIntPtr>;
    private               args: array of KernelArg;
    
    public function EnqEvCapacity: integer; override := 3 + args.Length;
    
    public constructor(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg);
    begin
      self.global_work_offset := global_work_offset;
      self.  global_work_size :=   global_work_size;
      self.   local_work_size :=    local_work_size;
      self.              args :=               args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (Kernel, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var global_work_offset_qr: QueueRes<array of UIntPtr>;
      var   global_work_size_qr: QueueRes<array of UIntPtr>;
      var    local_work_size_qr: QueueRes<array of UIntPtr>;
      var               args_qr: array of QueueRes<ISetableKernelArg>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create, true, enq_evs.Capacity-1, invoker->
      begin
        global_work_offset_qr := invoker.InvokeBranch&<QueueRes<array of UIntPtr>>((g,l)->global_work_offset.Invoke(g, l.WithPtrNeed(False))); if (global_work_offset_qr is IQueueResConst) then enq_evs.AddL2(global_work_offset_qr.ev) else enq_evs.AddL1(global_work_offset_qr.ev);
          global_work_size_qr := invoker.InvokeBranch&<QueueRes<array of UIntPtr>>((g,l)->global_work_size.Invoke(g, l.WithPtrNeed(False))); if (global_work_size_qr is IQueueResConst) then enq_evs.AddL2(global_work_size_qr.ev) else enq_evs.AddL1(global_work_size_qr.ev);
           local_work_size_qr := invoker.InvokeBranch&<QueueRes<array of UIntPtr>>((g,l)->local_work_size.Invoke(g, l.WithPtrNeed(False))); if (local_work_size_qr is IQueueResConst) then enq_evs.AddL2(local_work_size_qr.ev) else enq_evs.AddL1(local_work_size_qr.ev);
                      args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<QueueRes<ISetableKernelArg>>(temp1.Invoke); if (Result is IQueueResConst) then enq_evs.AddL2(Result.ev) else enq_evs.AddL1(Result.ev); end);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var global_work_offset := global_work_offset_qr.GetRes;
        var   global_work_size :=   global_work_size_qr.GetRes;
        var    local_work_size :=    local_work_size_qr.GetRes;
        var               args :=               args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        var ntv := o.UseExclusiveNative(ntv->
        begin
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i);
          
          var ec := cl.EnqueueNDRangeKernel(
            cq, ntv, global_work_size.Length,
            global_work_offset,
            global_work_size,
            local_work_size,
            evs.count, evs.evs, res_ev
          );
          OpenCLABCInternalException.RaiseIfError(ec);
          
          OpenCLABCInternalException.RaiseIfError( cl.RetainKernel(ntv) );
          Result := ntv;
        end);
        
        var args_hnd := GCHandle.Alloc(args);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          OpenCLABCInternalException.RaiseIfError( cl.ReleaseKernel(ntv) );
          args_hnd.Free;
        end{$ifdef EventDebug}, 'custom afterwork'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      global_work_offset.RegisterWaitables(g, prev_hubs);
        global_work_size.RegisterWaitables(g, prev_hubs);
         local_work_size.RegisterWaitables(g, prev_hubs);
      foreach var temp1 in args do temp1.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'global_work_offset: ';
      global_work_offset.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'global_work_size: ';
      global_work_size.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'local_work_size: ';
      local_work_size.ToString(sb, tabs, index, delayed, false);
      
      for var i := 0 to args.Length-1 do 
      begin
        sb.Append(#9, tabs);
        sb += 'args[';
        sb.Append(i);
        sb += ']: ';
        args[i].ToString(sb, tabs, index, delayed, false);
      end;
      
    end;
    
  end;
  
function KernelCCQ.AddExec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): KernelCCQ;
begin
  Result := AddCommand(self, new KernelCommandExec(global_work_offset, global_work_size, local_work_size, args));
end;

{$endregion Exec}

{$endregion 1#Exec}

{$endregion Explicit}

{$endregion Kernel}

{$region MemorySegment}

{$region Implicit}

{$region 1#Write&Read}

function MemorySegment.WriteData(ptr: CommandQueue<IntPtr>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteData(ptr));
end;

function MemorySegment.WriteData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteData(ptr, mem_offset, len));
end;

function MemorySegment.ReadData(ptr: CommandQueue<IntPtr>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadData(ptr));
end;

function MemorySegment.ReadData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadData(ptr, mem_offset, len));
end;

function MemorySegment.WriteData(ptr: pointer): MemorySegment;
begin
  Result := WriteData(IntPtr(ptr));
end;

function MemorySegment.WriteData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegment;
begin
  Result := WriteData(IntPtr(ptr), mem_offset, len);
end;

function MemorySegment.ReadData(ptr: pointer): MemorySegment;
begin
  Result := ReadData(IntPtr(ptr));
end;

function MemorySegment.ReadData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegment;
begin
  Result := ReadData(IntPtr(ptr), mem_offset, len);
end;

function MemorySegment.WriteValue<TRecord>(val: TRecord): MemorySegment; where TRecord: record;
begin
  Result := WriteValue(val, 0);
end;

function MemorySegment.WriteValue<TRecord>(val: CommandQueue<TRecord>): MemorySegment; where TRecord: record;
begin
  Result := WriteValue(val, 0);
end;

function MemorySegment.WriteValue<TRecord>(val: NativeValue<TRecord>): MemorySegment; where TRecord: record;
begin
  Result := WriteValue(val, 0);
end;

function MemorySegment.WriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegment; where TRecord: record;
begin
  Result := WriteValue(val, 0);
end;

function MemorySegment.ReadValue<TRecord>(val: NativeValue<TRecord>): MemorySegment; where TRecord: record;
begin
  Result := WriteValue(val, 0);
end;

function MemorySegment.ReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegment; where TRecord: record;
begin
  Result := WriteValue(val, 0);
end;

function MemorySegment.WriteValue<TRecord>(val: TRecord; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteValue&<TRecord>(val, mem_offset));
end;

function MemorySegment.WriteValue<TRecord>(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteValue&<TRecord>(val, mem_offset));
end;

function MemorySegment.WriteValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteValue&<TRecord>(val, mem_offset));
end;

function MemorySegment.WriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteValue&<TRecord>(val, mem_offset));
end;

function MemorySegment.ReadValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadValue&<TRecord>(val, mem_offset));
end;

function MemorySegment.ReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadValue&<TRecord>(val, mem_offset));
end;

function MemorySegment.WriteArray1<TRecord>(a: array of TRecord): MemorySegment; where TRecord: record;
begin
  Result := WriteArray1(new ConstQueue<array of TRecord>(a));
end;

function MemorySegment.WriteArray2<TRecord>(a: array[,] of TRecord): MemorySegment; where TRecord: record;
begin
  Result := WriteArray2(new ConstQueue<array[,] of TRecord>(a));
end;

function MemorySegment.WriteArray3<TRecord>(a: array[,,] of TRecord): MemorySegment; where TRecord: record;
begin
  Result := WriteArray3(new ConstQueue<array[,,] of TRecord>(a));
end;

function MemorySegment.ReadArray1<TRecord>(a: array of TRecord): MemorySegment; where TRecord: record;
begin
  Result := ReadArray1(new ConstQueue<array of TRecord>(a));
end;

function MemorySegment.ReadArray2<TRecord>(a: array[,] of TRecord): MemorySegment; where TRecord: record;
begin
  Result := ReadArray2(new ConstQueue<array[,] of TRecord>(a));
end;

function MemorySegment.ReadArray3<TRecord>(a: array[,,] of TRecord): MemorySegment; where TRecord: record;
begin
  Result := ReadArray3(new ConstQueue<array[,,] of TRecord>(a));
end;

function MemorySegment.WriteArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := WriteArray1(new ConstQueue<array of TRecord>(a), a_offset, len, mem_offset);
end;

function MemorySegment.WriteArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := WriteArray2(new ConstQueue<array[,] of TRecord>(a), a_offset1,a_offset2, len, mem_offset);
end;

function MemorySegment.WriteArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := WriteArray3(new ConstQueue<array[,,] of TRecord>(a), a_offset1,a_offset2,a_offset3, len, mem_offset);
end;

function MemorySegment.ReadArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := ReadArray1(new ConstQueue<array of TRecord>(a), a_offset, len, mem_offset);
end;

function MemorySegment.ReadArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := ReadArray2(new ConstQueue<array[,] of TRecord>(a), a_offset1,a_offset2, len, mem_offset);
end;

function MemorySegment.ReadArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := ReadArray3(new ConstQueue<array[,,] of TRecord>(a), a_offset1,a_offset2,a_offset3, len, mem_offset);
end;

function MemorySegment.WriteArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteArray1&<TRecord>(a));
end;

function MemorySegment.WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteArray2&<TRecord>(a));
end;

function MemorySegment.WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteArray3&<TRecord>(a));
end;

function MemorySegment.ReadArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadArray1&<TRecord>(a));
end;

function MemorySegment.ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadArray2&<TRecord>(a));
end;

function MemorySegment.ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadArray3&<TRecord>(a));
end;

function MemorySegment.WriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteArray1&<TRecord>(a, a_offset, len, mem_offset));
end;

function MemorySegment.WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteArray2&<TRecord>(a, a_offset1, a_offset2, len, mem_offset));
end;

function MemorySegment.WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteArray3&<TRecord>(a, a_offset1, a_offset2, a_offset3, len, mem_offset));
end;

function MemorySegment.ReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadArray1&<TRecord>(a, a_offset, len, mem_offset));
end;

function MemorySegment.ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadArray2&<TRecord>(a, a_offset1, a_offset2, len, mem_offset));
end;

function MemorySegment.ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadArray3&<TRecord>(a, a_offset1, a_offset2, a_offset3, len, mem_offset));
end;

{$endregion 1#Write&Read}

{$region 2#Fill}

function MemorySegment.FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillData(ptr, pattern_len));
end;

function MemorySegment.FillData(ptr: CommandQueue<IntPtr>; pattern_len, mem_offset, len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillData(ptr, pattern_len, mem_offset, len));
end;

function MemorySegment.FillValue<TRecord>(val: TRecord): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val));
end;

function MemorySegment.FillValue<TRecord>(val: CommandQueue<TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val));
end;

function MemorySegment.FillValue<TRecord>(val: TRecord; mem_offset, len: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val, mem_offset, len));
end;

function MemorySegment.FillValue<TRecord>(val: CommandQueue<TRecord>; mem_offset, len: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val, mem_offset, len));
end;

function MemorySegment.FillArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillArray1&<TRecord>(a));
end;

function MemorySegment.FillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillArray2&<TRecord>(a));
end;

function MemorySegment.FillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillArray3&<TRecord>(a));
end;

function MemorySegment.FillArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillArray1&<TRecord>(a, a_offset, pattern_len, len, mem_offset));
end;

function MemorySegment.FillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillArray2&<TRecord>(a, a_offset1, a_offset2, pattern_len, len, mem_offset));
end;

function MemorySegment.FillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillArray3&<TRecord>(a, a_offset1, a_offset2, a_offset3, pattern_len, len, mem_offset));
end;

{$endregion 2#Fill}

{$region 3#Copy}

function MemorySegment.CopyTo(mem: CommandQueue<MemorySegment>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddCopyTo(mem));
end;

function MemorySegment.CopyTo(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddCopyTo(mem, from_pos, to_pos, len));
end;

function MemorySegment.CopyFrom(mem: CommandQueue<MemorySegment>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddCopyFrom(mem));
end;

function MemorySegment.CopyFrom(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddCopyFrom(mem, from_pos, to_pos, len));
end;

{$endregion 3#Copy}

{$region Get}

function MemorySegment.GetValue<TRecord>: TRecord; where TRecord: record;
begin
  Result := GetValue&<TRecord>(0);
end;

function MemorySegment.GetValue<TRecord>(mem_offset: CommandQueue<integer>): TRecord; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddGetValue&<TRecord>(mem_offset));
end;

function MemorySegment.GetArray1<TRecord>: array of TRecord; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddGetArray1&<TRecord>);
end;

function MemorySegment.GetArray1<TRecord>(len: CommandQueue<integer>): array of TRecord; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddGetArray1&<TRecord>(len));
end;

function MemorySegment.GetArray2<TRecord>(len1,len2: CommandQueue<integer>): array[,] of TRecord; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddGetArray2&<TRecord>(len1, len2));
end;

function MemorySegment.GetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): array[,,] of TRecord; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddGetArray3&<TRecord>(len1, len2, len3));
end;

{$endregion Get}

{$endregion Implicit}

{$region Explicit}

{$region 1#Write&Read}

{$region WriteDataAutoSize}

type
  MemorySegmentCommandWriteDataAutoSize = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private ptr: CommandQueue<IntPtr>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(ptr: CommandQueue<IntPtr>);
    begin
      self.ptr := ptr;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, o.Size,
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteData(ptr: CommandQueue<IntPtr>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteDataAutoSize(ptr));
end;

{$endregion WriteDataAutoSize}

{$region WriteData}

type
  MemorySegmentCommandWriteData = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private        ptr: CommandQueue<IntPtr>;
    private mem_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 3;
    
    public constructor(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>);
    begin
      self.       ptr :=        ptr;
      self.mem_offset := mem_offset;
      self.       len :=        len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        ptr_qr: QueueRes<IntPtr>;
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
               ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(       ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        ptr :=        ptr_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             ptr.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteData(ptr, mem_offset, len));
end;

{$endregion WriteData}

{$region ReadDataAutoSize}

type
  MemorySegmentCommandReadDataAutoSize = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private ptr: CommandQueue<IntPtr>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(ptr: CommandQueue<IntPtr>);
    begin
      self.ptr := ptr;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, o.Size,
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadData(ptr: CommandQueue<IntPtr>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadDataAutoSize(ptr));
end;

{$endregion ReadDataAutoSize}

{$region ReadData}

type
  MemorySegmentCommandReadData = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private        ptr: CommandQueue<IntPtr>;
    private mem_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 3;
    
    public constructor(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>);
    begin
      self.       ptr :=        ptr;
      self.mem_offset := mem_offset;
      self.       len :=        len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        ptr_qr: QueueRes<IntPtr>;
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
               ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(       ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        ptr :=        ptr_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             ptr.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadData(ptr, mem_offset, len));
end;

{$endregion ReadData}

{$region WriteDataAutoSize}

function MemorySegmentCCQ.AddWriteData(ptr: pointer): MemorySegmentCCQ;
begin
  Result := AddWriteData(IntPtr(ptr));
end;

{$endregion WriteDataAutoSize}

{$region WriteData}

function MemorySegmentCCQ.AddWriteData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := AddWriteData(IntPtr(ptr), mem_offset, len);
end;

{$endregion WriteData}

{$region ReadDataAutoSize}

function MemorySegmentCCQ.AddReadData(ptr: pointer): MemorySegmentCCQ;
begin
  Result := AddReadData(IntPtr(ptr));
end;

{$endregion ReadDataAutoSize}

{$region ReadData}

function MemorySegmentCCQ.AddReadData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := AddReadData(IntPtr(ptr), mem_offset, len);
end;

{$endregion ReadData}

{$region WriteValue}

function MemorySegmentCCQ.AddWriteValue<TRecord>(val: TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteValue(val, 0);
end;

{$endregion WriteValue}

{$region WriteValueQ}

function MemorySegmentCCQ.AddWriteValue<TRecord>(val: CommandQueue<TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteValue(val, 0);
end;

{$endregion WriteValueQ}

{$region WriteValueN}

function MemorySegmentCCQ.AddWriteValue<TRecord>(val: NativeValue<TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteValue(val, 0);
end;

{$endregion WriteValueN}

{$region WriteValueNQ}

function MemorySegmentCCQ.AddWriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteValue(val, 0);
end;

{$endregion WriteValueNQ}

{$region ReadValueN}

function MemorySegmentCCQ.AddReadValue<TRecord>(val: NativeValue<TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteValue(val, 0);
end;

{$endregion ReadValueN}

{$region ReadValueNQ}

function MemorySegmentCCQ.AddReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteValue(val, 0);
end;

{$endregion ReadValueNQ}

{$region WriteValue}

type
  MemorySegmentCommandWriteValue<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private        val: ^TRecord := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>));
    private mem_offset: CommandQueue<integer>;
    
    protected procedure Finalize; override;
    begin
      Marshal.FreeHGlobal(new IntPtr(val));
    end;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(val: TRecord; mem_offset: CommandQueue<integer>);
    begin
      self.       val^ :=        val;
      self.mem_offset  := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var mem_offset := mem_offset_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr(val),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val^);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteValue<TRecord>(val: TRecord; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteValue<TRecord>(val, mem_offset));
end;

{$endregion WriteValue}

{$region WriteValueQ}

type
  MemorySegmentCommandWriteValueQ<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private        val: CommandQueue<TRecord>;
    private mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 2;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>);
    begin
      self.       val :=        val;
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        val_qr: QueueRes<TRecord>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create, true, enq_evs.Capacity-1, invoker->
      begin
               val_qr := invoker.InvokeBranch&<QueueRes<TRecord>>((g,l)->val.Invoke(g, l.WithPtrNeed( True))); if (val_qr is IQueueResDelayedPtr) or (val_qr is IQueueResConst) then enq_evs.AddL2(val_qr.ev) else enq_evs.AddL1(val_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->mem_offset.Invoke(g, l.WithPtrNeed(False))); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        val :=        val_qr.ToPtr;
        var mem_offset := mem_offset_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr(val.GetPtr),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             val.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteValue<TRecord>(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteValueQ<TRecord>(val, mem_offset));
end;

{$endregion WriteValueQ}

{$region WriteValueN}

type
  MemorySegmentCommandWriteValueN<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private        val: NativeValue<TRecord>;
    private mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>);
    begin
      self.       val :=        val;
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var mem_offset := mem_offset_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteValueN<TRecord>(val, mem_offset));
end;

{$endregion WriteValueN}

{$region WriteValueNQ}

type
  MemorySegmentCommandWriteValueNQ<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private        val: CommandQueue<NativeValue<TRecord>>;
    private mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 2;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>);
    begin
      self.       val :=        val;
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        val_qr: QueueRes<NativeValue<TRecord>>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
               val_qr := invoker.InvokeBranch&<QueueRes<NativeValue<TRecord>>>(       val.Invoke); if (val_qr is IQueueResConst) then enq_evs.AddL2(val_qr.ev) else enq_evs.AddL1(val_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        val :=        val_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             val.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteValueNQ<TRecord>(val, mem_offset));
end;

{$endregion WriteValueNQ}

{$region ReadValueN}

type
  MemorySegmentCommandReadValueN<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private        val: NativeValue<TRecord>;
    private mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>);
    begin
      self.       val :=        val;
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var mem_offset := mem_offset_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadValueN<TRecord>(val, mem_offset));
end;

{$endregion ReadValueN}

{$region ReadValueNQ}

type
  MemorySegmentCommandReadValueNQ<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private        val: CommandQueue<NativeValue<TRecord>>;
    private mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 2;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>);
    begin
      self.       val :=        val;
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        val_qr: QueueRes<NativeValue<TRecord>>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
               val_qr := invoker.InvokeBranch&<QueueRes<NativeValue<TRecord>>>(       val.Invoke); if (val_qr is IQueueResConst) then enq_evs.AddL2(val_qr.ev) else enq_evs.AddL1(val_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        val :=        val_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             val.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadValueNQ<TRecord>(val, mem_offset));
end;

{$endregion ReadValueNQ}

{$region WriteArray1AutoSize}

function MemorySegmentCCQ.AddWriteArray1<TRecord>(a: array of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteArray1(new ConstQueue<array of TRecord>(a));
end;

{$endregion WriteArray1AutoSize}

{$region WriteArray2AutoSize}

function MemorySegmentCCQ.AddWriteArray2<TRecord>(a: array[,] of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteArray2(new ConstQueue<array[,] of TRecord>(a));
end;

{$endregion WriteArray2AutoSize}

{$region WriteArray3AutoSize}

function MemorySegmentCCQ.AddWriteArray3<TRecord>(a: array[,,] of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteArray3(new ConstQueue<array[,,] of TRecord>(a));
end;

{$endregion WriteArray3AutoSize}

{$region ReadArray1AutoSize}

function MemorySegmentCCQ.AddReadArray1<TRecord>(a: array of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddReadArray1(new ConstQueue<array of TRecord>(a));
end;

{$endregion ReadArray1AutoSize}

{$region ReadArray2AutoSize}

function MemorySegmentCCQ.AddReadArray2<TRecord>(a: array[,] of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddReadArray2(new ConstQueue<array[,] of TRecord>(a));
end;

{$endregion ReadArray2AutoSize}

{$region ReadArray3AutoSize}

function MemorySegmentCCQ.AddReadArray3<TRecord>(a: array[,,] of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddReadArray3(new ConstQueue<array[,,] of TRecord>(a));
end;

{$endregion ReadArray3AutoSize}

{$region WriteArray1}

function MemorySegmentCCQ.AddWriteArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteArray1(new ConstQueue<array of TRecord>(a), a_offset, len, mem_offset);
end;

{$endregion WriteArray1}

{$region WriteArray2}

function MemorySegmentCCQ.AddWriteArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteArray2(new ConstQueue<array[,] of TRecord>(a), a_offset1,a_offset2, len, mem_offset);
end;

{$endregion WriteArray2}

{$region WriteArray3}

function MemorySegmentCCQ.AddWriteArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddWriteArray3(new ConstQueue<array[,,] of TRecord>(a), a_offset1,a_offset2,a_offset3, len, mem_offset);
end;

{$endregion WriteArray3}

{$region ReadArray1}

function MemorySegmentCCQ.AddReadArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddReadArray1(new ConstQueue<array of TRecord>(a), a_offset, len, mem_offset);
end;

{$endregion ReadArray1}

{$region ReadArray2}

function MemorySegmentCCQ.AddReadArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddReadArray2(new ConstQueue<array[,] of TRecord>(a), a_offset1,a_offset2, len, mem_offset);
end;

{$endregion ReadArray2}

{$region ReadArray3}

function MemorySegmentCCQ.AddReadArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddReadArray3(new ConstQueue<array[,,] of TRecord>(a), a_offset1,a_offset2,a_offset3, len, mem_offset);
end;

{$endregion ReadArray3}

{$region WriteArray1AutoSize}

type
  MemorySegmentCommandWriteArray1AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array of TRecord>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of TRecord>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because {-rank-} block would be nested in {-AutoSize-}
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteArray1AutoSize<TRecord>(a));
end;

{$endregion WriteArray1AutoSize}

{$region WriteArray2AutoSize}

type
  MemorySegmentCommandWriteArray2AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,] of TRecord>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,] of TRecord>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because {-rank-} block would be nested in {-AutoSize-}
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0,0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteArray2AutoSize<TRecord>(a));
end;

{$endregion WriteArray2AutoSize}

{$region WriteArray3AutoSize}

type
  MemorySegmentCommandWriteArray3AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,,] of TRecord>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,,] of TRecord>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because {-rank-} block would be nested in {-AutoSize-}
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0,0,0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteArray3AutoSize<TRecord>(a));
end;

{$endregion WriteArray3AutoSize}

{$region ReadArray1AutoSize}

type
  MemorySegmentCommandReadArray1AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array of TRecord>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of TRecord>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because {-rank-} block would be nested in {-AutoSize-}
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadArray1AutoSize<TRecord>(a));
end;

{$endregion ReadArray1AutoSize}

{$region ReadArray2AutoSize}

type
  MemorySegmentCommandReadArray2AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,] of TRecord>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,] of TRecord>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because {-rank-} block would be nested in {-AutoSize-}
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0,0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadArray2AutoSize<TRecord>(a));
end;

{$endregion ReadArray2AutoSize}

{$region ReadArray3AutoSize}

type
  MemorySegmentCommandReadArray3AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,,] of TRecord>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,,] of TRecord>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because {-rank-} block would be nested in {-AutoSize-}
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0,0,0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadArray3AutoSize<TRecord>(a));
end;

{$endregion ReadArray3AutoSize}

{$region WriteArray1}

type
  MemorySegmentCommandWriteArray1<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private          a: CommandQueue<array of TRecord>;
    private   a_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    private mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 4;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>);
    begin
      self.         a :=          a;
      self.  a_offset :=   a_offset;
      self.       len :=        len;
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array of TRecord>;
      var   a_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(         a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
          a_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset.Invoke); if (a_offset_qr is IQueueResConst) then enq_evs.AddL2(a_offset_qr.ev) else enq_evs.AddL1(a_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var          a :=          a_qr.GetRes;
        var   a_offset :=   a_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.RegisterWaitables(g, prev_hubs);
        a_offset.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset: ';
      a_offset.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteArray1<TRecord>(a, a_offset, len, mem_offset));
end;

{$endregion WriteArray1}

{$region WriteArray2}

type
  MemorySegmentCommandWriteArray2<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private          a: CommandQueue<array[,] of TRecord>;
    private  a_offset1: CommandQueue<integer>;
    private  a_offset2: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    private mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 5;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>);
    begin
      self.         a :=          a;
      self. a_offset1 :=  a_offset1;
      self. a_offset2 :=  a_offset2;
      self.       len :=        len;
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array[,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(         a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
         a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset1.Invoke); if (a_offset1_qr is IQueueResConst) then enq_evs.AddL2(a_offset1_qr.ev) else enq_evs.AddL1(a_offset1_qr.ev);
         a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset2.Invoke); if (a_offset2_qr is IQueueResConst) then enq_evs.AddL2(a_offset2_qr.ev) else enq_evs.AddL1(a_offset2_qr.ev);
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var          a :=          a_qr.GetRes;
        var  a_offset1 :=  a_offset1_qr.GetRes;
        var  a_offset2 :=  a_offset2_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.RegisterWaitables(g, prev_hubs);
       a_offset1.RegisterWaitables(g, prev_hubs);
       a_offset2.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset1: ';
      a_offset1.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset2: ';
      a_offset2.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteArray2<TRecord>(a, a_offset1, a_offset2, len, mem_offset));
end;

{$endregion WriteArray2}

{$region WriteArray3}

type
  MemorySegmentCommandWriteArray3<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private          a: CommandQueue<array[,,] of TRecord>;
    private  a_offset1: CommandQueue<integer>;
    private  a_offset2: CommandQueue<integer>;
    private  a_offset3: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    private mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 6;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>);
    begin
      self.         a :=          a;
      self. a_offset1 :=  a_offset1;
      self. a_offset2 :=  a_offset2;
      self. a_offset3 :=  a_offset3;
      self.       len :=        len;
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array[,,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var  a_offset3_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(         a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
         a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset1.Invoke); if (a_offset1_qr is IQueueResConst) then enq_evs.AddL2(a_offset1_qr.ev) else enq_evs.AddL1(a_offset1_qr.ev);
         a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset2.Invoke); if (a_offset2_qr is IQueueResConst) then enq_evs.AddL2(a_offset2_qr.ev) else enq_evs.AddL1(a_offset2_qr.ev);
         a_offset3_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset3.Invoke); if (a_offset3_qr is IQueueResConst) then enq_evs.AddL2(a_offset3_qr.ev) else enq_evs.AddL1(a_offset3_qr.ev);
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var          a :=          a_qr.GetRes;
        var  a_offset1 :=  a_offset1_qr.GetRes;
        var  a_offset2 :=  a_offset2_qr.GetRes;
        var  a_offset3 :=  a_offset3_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2,a_offset3],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.RegisterWaitables(g, prev_hubs);
       a_offset1.RegisterWaitables(g, prev_hubs);
       a_offset2.RegisterWaitables(g, prev_hubs);
       a_offset3.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset1: ';
      a_offset1.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset2: ';
      a_offset2.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset3: ';
      a_offset3.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandWriteArray3<TRecord>(a, a_offset1, a_offset2, a_offset3, len, mem_offset));
end;

{$endregion WriteArray3}

{$region ReadArray1}

type
  MemorySegmentCommandReadArray1<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private          a: CommandQueue<array of TRecord>;
    private   a_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    private mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 4;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>);
    begin
      self.         a :=          a;
      self.  a_offset :=   a_offset;
      self.       len :=        len;
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array of TRecord>;
      var   a_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(         a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
          a_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset.Invoke); if (a_offset_qr is IQueueResConst) then enq_evs.AddL2(a_offset_qr.ev) else enq_evs.AddL1(a_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var          a :=          a_qr.GetRes;
        var   a_offset :=   a_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.RegisterWaitables(g, prev_hubs);
        a_offset.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset: ';
      a_offset.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadArray1<TRecord>(a, a_offset, len, mem_offset));
end;

{$endregion ReadArray1}

{$region ReadArray2}

type
  MemorySegmentCommandReadArray2<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private          a: CommandQueue<array[,] of TRecord>;
    private  a_offset1: CommandQueue<integer>;
    private  a_offset2: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    private mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 5;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>);
    begin
      self.         a :=          a;
      self. a_offset1 :=  a_offset1;
      self. a_offset2 :=  a_offset2;
      self.       len :=        len;
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array[,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(         a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
         a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset1.Invoke); if (a_offset1_qr is IQueueResConst) then enq_evs.AddL2(a_offset1_qr.ev) else enq_evs.AddL1(a_offset1_qr.ev);
         a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset2.Invoke); if (a_offset2_qr is IQueueResConst) then enq_evs.AddL2(a_offset2_qr.ev) else enq_evs.AddL1(a_offset2_qr.ev);
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var          a :=          a_qr.GetRes;
        var  a_offset1 :=  a_offset1_qr.GetRes;
        var  a_offset2 :=  a_offset2_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.RegisterWaitables(g, prev_hubs);
       a_offset1.RegisterWaitables(g, prev_hubs);
       a_offset2.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset1: ';
      a_offset1.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset2: ';
      a_offset2.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadArray2<TRecord>(a, a_offset1, a_offset2, len, mem_offset));
end;

{$endregion ReadArray2}

{$region ReadArray3}

type
  MemorySegmentCommandReadArray3<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private          a: CommandQueue<array[,,] of TRecord>;
    private  a_offset1: CommandQueue<integer>;
    private  a_offset2: CommandQueue<integer>;
    private  a_offset3: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    private mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 6;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>);
    begin
      self.         a :=          a;
      self. a_offset1 :=  a_offset1;
      self. a_offset2 :=  a_offset2;
      self. a_offset3 :=  a_offset3;
      self.       len :=        len;
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array[,,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var  a_offset3_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(         a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
         a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset1.Invoke); if (a_offset1_qr is IQueueResConst) then enq_evs.AddL2(a_offset1_qr.ev) else enq_evs.AddL1(a_offset1_qr.ev);
         a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset2.Invoke); if (a_offset2_qr is IQueueResConst) then enq_evs.AddL2(a_offset2_qr.ev) else enq_evs.AddL1(a_offset2_qr.ev);
         a_offset3_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset3.Invoke); if (a_offset3_qr is IQueueResConst) then enq_evs.AddL2(a_offset3_qr.ev) else enq_evs.AddL1(a_offset3_qr.ev);
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var          a :=          a_qr.GetRes;
        var  a_offset1 :=  a_offset1_qr.GetRes;
        var  a_offset2 :=  a_offset2_qr.GetRes;
        var  a_offset3 :=  a_offset3_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2,a_offset3],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.RegisterWaitables(g, prev_hubs);
       a_offset1.RegisterWaitables(g, prev_hubs);
       a_offset2.RegisterWaitables(g, prev_hubs);
       a_offset3.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset1: ';
      a_offset1.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset2: ';
      a_offset2.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset3: ';
      a_offset3.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadArray3<TRecord>(a, a_offset1, a_offset2, a_offset3, len, mem_offset));
end;

{$endregion ReadArray3}

{$endregion 1#Write&Read}

{$region 2#Fill}

{$region FillDataAutoSize}

type
  MemorySegmentCommandFillDataAutoSize = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private         ptr: CommandQueue<IntPtr>;
    private pattern_len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 2;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.pattern_len := pattern_len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(        ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.Invoke); if (pattern_len_qr is IQueueResConst) then enq_evs.AddL2(pattern_len_qr.ev) else enq_evs.AddL1(pattern_len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
              ptr.RegisterWaitables(g, prev_hubs);
      pattern_len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'pattern_len: ';
      pattern_len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillDataAutoSize(ptr, pattern_len));
end;

{$endregion FillDataAutoSize}

{$region FillData}

type
  MemorySegmentCommandFillData = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private         ptr: CommandQueue<IntPtr>;
    private pattern_len: CommandQueue<integer>;
    private  mem_offset: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 4;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len, mem_offset, len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.pattern_len := pattern_len;
      self. mem_offset :=  mem_offset;
      self.        len :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(        ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.Invoke); if (pattern_len_qr is IQueueResConst) then enq_evs.AddL2(pattern_len_qr.ev) else enq_evs.AddL1(pattern_len_qr.ev);
         mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>( mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var  mem_offset :=  mem_offset_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
              ptr.RegisterWaitables(g, prev_hubs);
      pattern_len.RegisterWaitables(g, prev_hubs);
       mem_offset.RegisterWaitables(g, prev_hubs);
              len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'pattern_len: ';
      pattern_len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillData(ptr, pattern_len, mem_offset, len));
end;

{$endregion FillData}

{$region FillValueAutoSize}

type
  MemorySegmentCommandFillValueAutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private val: ^TRecord := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>));
    
    protected procedure Finalize; override;
    begin
      Marshal.FreeHGlobal(new IntPtr(val));
    end;
    
    public function EnqEvCapacity: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(val: TRecord);
    begin
      self.val^ := val;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      
      Result := (o, cq, err_handler, evs)->
      begin
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val^);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillValue<TRecord>(val: TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillValueAutoSize<TRecord>(val));
end;

{$endregion FillValueAutoSize}

{$region FillValueAutoSizeQ}

type
  MemorySegmentCommandFillValueAutoSizeQ<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private val: CommandQueue<TRecord>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(val: CommandQueue<TRecord>);
    begin
      self.val := val;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var val_qr: QueueRes<TRecord>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(True), true, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueRes<TRecord>>(val.Invoke); if (val_qr is IQueueResDelayedPtr) or (val_qr is IQueueResConst) then enq_evs.AddL2(val_qr.ev) else enq_evs.AddL1(val_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var val := val_qr.ToPtr;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val.GetPtr), new UIntPtr(Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillValue<TRecord>(val: CommandQueue<TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillValueAutoSizeQ<TRecord>(val));
end;

{$endregion FillValueAutoSizeQ}

{$region FillValue}

type
  MemorySegmentCommandFillValue<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private        val: ^TRecord := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>));
    private mem_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    
    protected procedure Finalize; override;
    begin
      Marshal.FreeHGlobal(new IntPtr(val));
    end;
    
    public function EnqEvCapacity: integer; override := 2;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(val: TRecord; mem_offset, len: CommandQueue<integer>);
    begin
      self.       val^ :=        val;
      self.mem_offset  := mem_offset;
      self.       len  :=        len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var mem_offset := mem_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new UIntPtr(mem_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val^);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillValue<TRecord>(val: TRecord; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillValue<TRecord>(val, mem_offset, len));
end;

{$endregion FillValue}

{$region FillValueQ}

type
  MemorySegmentCommandFillValueQ<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private        val: CommandQueue<TRecord>;
    private mem_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 3;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(val: CommandQueue<TRecord>; mem_offset, len: CommandQueue<integer>);
    begin
      self.       val :=        val;
      self.mem_offset := mem_offset;
      self.       len :=        len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        val_qr: QueueRes<TRecord>;
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create, true, enq_evs.Capacity-1, invoker->
      begin
               val_qr := invoker.InvokeBranch&<QueueRes<TRecord>>((g,l)->val.Invoke(g, l.WithPtrNeed( True))); if (val_qr is IQueueResDelayedPtr) or (val_qr is IQueueResConst) then enq_evs.AddL2(val_qr.ev) else enq_evs.AddL1(val_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->mem_offset.Invoke(g, l.WithPtrNeed(False))); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->len.Invoke(g, l.WithPtrNeed(False))); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        val :=        val_qr.ToPtr;
        var mem_offset := mem_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val.GetPtr), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new UIntPtr(mem_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             val.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillValue<TRecord>(val: CommandQueue<TRecord>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillValueQ<TRecord>(val, mem_offset, len));
end;

{$endregion FillValueQ}

{$region FillArray1AutoSize}

type
  MemorySegmentCommandFillArray1AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array of TRecord>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of TRecord>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[0], new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillArray1AutoSize<TRecord>(a));
end;

{$endregion FillArray1AutoSize}

{$region FillArray2AutoSize}

type
  MemorySegmentCommandFillArray2AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,] of TRecord>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,] of TRecord>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[0,0], new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillArray2AutoSize<TRecord>(a));
end;

{$endregion FillArray2AutoSize}

{$region FillArray3AutoSize}

type
  MemorySegmentCommandFillArray3AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,,] of TRecord>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,,] of TRecord>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[0,0,0], new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillArray3AutoSize<TRecord>(a));
end;

{$endregion FillArray3AutoSize}

{$region FillArray1}

type
  MemorySegmentCommandFillArray1<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private           a: CommandQueue<array of TRecord>;
    private    a_offset: CommandQueue<integer>;
    private pattern_len: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    private  mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 5;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array of TRecord>; a_offset, pattern_len, len, mem_offset: CommandQueue<integer>);
    begin
      self.          a :=           a;
      self.   a_offset :=    a_offset;
      self.pattern_len := pattern_len;
      self.        len :=         len;
      self. mem_offset :=  mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var           a_qr: QueueRes<array of TRecord>;
      var    a_offset_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(          a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
           a_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(   a_offset.Invoke); if (a_offset_qr is IQueueResConst) then enq_evs.AddL2(a_offset_qr.ev) else enq_evs.AddL1(a_offset_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.Invoke); if (pattern_len_qr is IQueueResConst) then enq_evs.AddL2(pattern_len_qr.ev) else enq_evs.AddL1(pattern_len_qr.ev);
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
         mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>( mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var           a :=           a_qr.GetRes;
        var    a_offset :=    a_offset_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var  mem_offset :=  mem_offset_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset], new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
                a.RegisterWaitables(g, prev_hubs);
         a_offset.RegisterWaitables(g, prev_hubs);
      pattern_len.RegisterWaitables(g, prev_hubs);
              len.RegisterWaitables(g, prev_hubs);
       mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset: ';
      a_offset.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'pattern_len: ';
      pattern_len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillArray1<TRecord>(a, a_offset, pattern_len, len, mem_offset));
end;

{$endregion FillArray1}

{$region FillArray2}

type
  MemorySegmentCommandFillArray2<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private           a: CommandQueue<array[,] of TRecord>;
    private   a_offset1: CommandQueue<integer>;
    private   a_offset2: CommandQueue<integer>;
    private pattern_len: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    private  mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 6;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, pattern_len, len, mem_offset: CommandQueue<integer>);
    begin
      self.          a :=           a;
      self.  a_offset1 :=   a_offset1;
      self.  a_offset2 :=   a_offset2;
      self.pattern_len := pattern_len;
      self.        len :=         len;
      self. mem_offset :=  mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var           a_qr: QueueRes<array[,] of TRecord>;
      var   a_offset1_qr: QueueRes<integer>;
      var   a_offset2_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(          a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
          a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset1.Invoke); if (a_offset1_qr is IQueueResConst) then enq_evs.AddL2(a_offset1_qr.ev) else enq_evs.AddL1(a_offset1_qr.ev);
          a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset2.Invoke); if (a_offset2_qr is IQueueResConst) then enq_evs.AddL2(a_offset2_qr.ev) else enq_evs.AddL1(a_offset2_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.Invoke); if (pattern_len_qr is IQueueResConst) then enq_evs.AddL2(pattern_len_qr.ev) else enq_evs.AddL1(pattern_len_qr.ev);
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
         mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>( mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var           a :=           a_qr.GetRes;
        var   a_offset1 :=   a_offset1_qr.GetRes;
        var   a_offset2 :=   a_offset2_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var  mem_offset :=  mem_offset_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset1,a_offset2], new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
                a.RegisterWaitables(g, prev_hubs);
        a_offset1.RegisterWaitables(g, prev_hubs);
        a_offset2.RegisterWaitables(g, prev_hubs);
      pattern_len.RegisterWaitables(g, prev_hubs);
              len.RegisterWaitables(g, prev_hubs);
       mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset1: ';
      a_offset1.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset2: ';
      a_offset2.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'pattern_len: ';
      pattern_len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillArray2<TRecord>(a, a_offset1, a_offset2, pattern_len, len, mem_offset));
end;

{$endregion FillArray2}

{$region FillArray3}

type
  MemorySegmentCommandFillArray3<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private           a: CommandQueue<array[,,] of TRecord>;
    private   a_offset1: CommandQueue<integer>;
    private   a_offset2: CommandQueue<integer>;
    private   a_offset3: CommandQueue<integer>;
    private pattern_len: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    private  mem_offset: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 7;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, pattern_len, len, mem_offset: CommandQueue<integer>);
    begin
      self.          a :=           a;
      self.  a_offset1 :=   a_offset1;
      self.  a_offset2 :=   a_offset2;
      self.  a_offset3 :=   a_offset3;
      self.pattern_len := pattern_len;
      self.        len :=         len;
      self. mem_offset :=  mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var           a_qr: QueueRes<array[,,] of TRecord>;
      var   a_offset1_qr: QueueRes<integer>;
      var   a_offset2_qr: QueueRes<integer>;
      var   a_offset3_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(          a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
          a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset1.Invoke); if (a_offset1_qr is IQueueResConst) then enq_evs.AddL2(a_offset1_qr.ev) else enq_evs.AddL1(a_offset1_qr.ev);
          a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset2.Invoke); if (a_offset2_qr is IQueueResConst) then enq_evs.AddL2(a_offset2_qr.ev) else enq_evs.AddL1(a_offset2_qr.ev);
          a_offset3_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset3.Invoke); if (a_offset3_qr is IQueueResConst) then enq_evs.AddL2(a_offset3_qr.ev) else enq_evs.AddL1(a_offset3_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.Invoke); if (pattern_len_qr is IQueueResConst) then enq_evs.AddL2(pattern_len_qr.ev) else enq_evs.AddL1(pattern_len_qr.ev);
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
         mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>( mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var           a :=           a_qr.GetRes;
        var   a_offset1 :=   a_offset1_qr.GetRes;
        var   a_offset2 :=   a_offset2_qr.GetRes;
        var   a_offset3 :=   a_offset3_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var  mem_offset :=  mem_offset_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset1,a_offset2,a_offset3], new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
                a.RegisterWaitables(g, prev_hubs);
        a_offset1.RegisterWaitables(g, prev_hubs);
        a_offset2.RegisterWaitables(g, prev_hubs);
        a_offset3.RegisterWaitables(g, prev_hubs);
      pattern_len.RegisterWaitables(g, prev_hubs);
              len.RegisterWaitables(g, prev_hubs);
       mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset1: ';
      a_offset1.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset2: ';
      a_offset2.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset3: ';
      a_offset3.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'pattern_len: ';
      pattern_len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandFillArray3<TRecord>(a, a_offset1, a_offset2, a_offset3, pattern_len, len, mem_offset));
end;

{$endregion FillArray3}

{$endregion 2#Fill}

{$region 3#Copy}

{$region CopyToAutoSize}

type
  MemorySegmentCommandCopyToAutoSize = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private mem: CommandQueue<MemorySegment>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(mem: CommandQueue<MemorySegment>);
    begin
      self.mem := mem;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var mem_qr: QueueRes<MemorySegment>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        mem_qr := invoker.InvokeBranch&<QueueRes<MemorySegment>>(mem.Invoke); if (mem_qr is IQueueResConst) then enq_evs.AddL2(mem_qr.ev) else enq_evs.AddL1(mem_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var mem := mem_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, o.Native,mem.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          o.Size64<mem.Size64 ? o.Size : mem.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem: ';
      mem.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddCopyTo(mem: CommandQueue<MemorySegment>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandCopyToAutoSize(mem));
end;

{$endregion CopyToAutoSize}

{$region CopyTo}

type
  MemorySegmentCommandCopyTo = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private      mem: CommandQueue<MemorySegment>;
    private from_pos: CommandQueue<integer>;
    private   to_pos: CommandQueue<integer>;
    private      len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 4;
    
    public constructor(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>);
    begin
      self.     mem :=      mem;
      self.from_pos := from_pos;
      self.  to_pos :=   to_pos;
      self.     len :=      len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var      mem_qr: QueueRes<MemorySegment>;
      var from_pos_qr: QueueRes<integer>;
      var   to_pos_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
             mem_qr := invoker.InvokeBranch&<QueueRes<MemorySegment>>(     mem.Invoke); if (mem_qr is IQueueResConst) then enq_evs.AddL2(mem_qr.ev) else enq_evs.AddL1(mem_qr.ev);
        from_pos_qr := invoker.InvokeBranch&<QueueRes<integer>>(from_pos.Invoke); if (from_pos_qr is IQueueResConst) then enq_evs.AddL2(from_pos_qr.ev) else enq_evs.AddL1(from_pos_qr.ev);
          to_pos_qr := invoker.InvokeBranch&<QueueRes<integer>>(  to_pos.Invoke); if (to_pos_qr is IQueueResConst) then enq_evs.AddL2(to_pos_qr.ev) else enq_evs.AddL1(to_pos_qr.ev);
             len_qr := invoker.InvokeBranch&<QueueRes<integer>>(     len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var      mem :=      mem_qr.GetRes;
        var from_pos := from_pos_qr.GetRes;
        var   to_pos :=   to_pos_qr.GetRes;
        var      len :=      len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, o.Native,mem.Native,
          new UIntPtr(from_pos), new UIntPtr(to_pos),
          new UIntPtr(len),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
           mem.RegisterWaitables(g, prev_hubs);
      from_pos.RegisterWaitables(g, prev_hubs);
        to_pos.RegisterWaitables(g, prev_hubs);
           len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem: ';
      mem.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'from_pos: ';
      from_pos.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'to_pos: ';
      to_pos.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddCopyTo(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandCopyTo(mem, from_pos, to_pos, len));
end;

{$endregion CopyTo}

{$region CopyFromAutoSize}

type
  MemorySegmentCommandCopyFromAutoSize = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private mem: CommandQueue<MemorySegment>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(mem: CommandQueue<MemorySegment>);
    begin
      self.mem := mem;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var mem_qr: QueueRes<MemorySegment>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        mem_qr := invoker.InvokeBranch&<QueueRes<MemorySegment>>(mem.Invoke); if (mem_qr is IQueueResConst) then enq_evs.AddL2(mem_qr.ev) else enq_evs.AddL1(mem_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var mem := mem_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, mem.Native,o.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          o.Size64<mem.Size64 ? o.Size : mem.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem: ';
      mem.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddCopyFrom(mem: CommandQueue<MemorySegment>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandCopyFromAutoSize(mem));
end;

{$endregion CopyFromAutoSize}

{$region CopyFrom}

type
  MemorySegmentCommandCopyFrom = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private      mem: CommandQueue<MemorySegment>;
    private from_pos: CommandQueue<integer>;
    private   to_pos: CommandQueue<integer>;
    private      len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 4;
    
    public constructor(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>);
    begin
      self.     mem :=      mem;
      self.from_pos := from_pos;
      self.  to_pos :=   to_pos;
      self.     len :=      len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var      mem_qr: QueueRes<MemorySegment>;
      var from_pos_qr: QueueRes<integer>;
      var   to_pos_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
             mem_qr := invoker.InvokeBranch&<QueueRes<MemorySegment>>(     mem.Invoke); if (mem_qr is IQueueResConst) then enq_evs.AddL2(mem_qr.ev) else enq_evs.AddL1(mem_qr.ev);
        from_pos_qr := invoker.InvokeBranch&<QueueRes<integer>>(from_pos.Invoke); if (from_pos_qr is IQueueResConst) then enq_evs.AddL2(from_pos_qr.ev) else enq_evs.AddL1(from_pos_qr.ev);
          to_pos_qr := invoker.InvokeBranch&<QueueRes<integer>>(  to_pos.Invoke); if (to_pos_qr is IQueueResConst) then enq_evs.AddL2(to_pos_qr.ev) else enq_evs.AddL1(to_pos_qr.ev);
             len_qr := invoker.InvokeBranch&<QueueRes<integer>>(     len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var      mem :=      mem_qr.GetRes;
        var from_pos := from_pos_qr.GetRes;
        var   to_pos :=   to_pos_qr.GetRes;
        var      len :=      len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, mem.Native,o.Native,
          new UIntPtr(from_pos), new UIntPtr(to_pos),
          new UIntPtr(len),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
           mem.RegisterWaitables(g, prev_hubs);
      from_pos.RegisterWaitables(g, prev_hubs);
        to_pos.RegisterWaitables(g, prev_hubs);
           len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem: ';
      mem.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'from_pos: ';
      from_pos.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'to_pos: ';
      to_pos.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddCopyFrom(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandCopyFrom(mem, from_pos, to_pos, len));
end;

{$endregion CopyFrom}

{$endregion 3#Copy}

{$region Get}

{$region GetValue}

function MemorySegmentCCQ.AddGetValue<TRecord>: CommandQueue<TRecord>; where TRecord: record;
begin
  Result := AddGetValue&<TRecord>(0);
end;

{$endregion GetValue}

{$region GetValue}

type
  MemorySegmentCommandGetValue<TRecord> = sealed class(EnqueueableGetCommand<MemorySegment, TRecord>)
  where TRecord: record;
    private mem_offset: CommandQueue<integer>;
    
    public function ForcePtrQr: boolean; override := true;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(ccq: MemorySegmentCCQ; mem_offset: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.mem_offset := mem_offset;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<TRecord>)->cl_event; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.Invoke); if (mem_offset_qr is IQueueResConst) then enq_evs.AddL2(mem_offset_qr.ev) else enq_evs.AddL1(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var mem_offset := mem_offset_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr((own_qr as QueueResDelayedPtr<TRecord>).ptr),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        var own_qr_hnd := GCHandle.Alloc(own_qr);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          own_qr_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [own_qr]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddGetValue<TRecord>(mem_offset: CommandQueue<integer>): CommandQueue<TRecord>; where TRecord: record;
begin
  Result := new MemorySegmentCommandGetValue<TRecord>(self, mem_offset) as CommandQueue<TRecord>;
end;

{$endregion GetValue}

{$region GetArray1AutoSize}

type
  MemorySegmentCommandGetArray1AutoSize<TRecord> = sealed class(EnqueueableGetCommand<MemorySegment, array of TRecord>)
  where TRecord: record;
    
    public function EnqEvCapacity: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(ccq: MemorySegmentCCQ);
    begin
      inherited Create(ccq);
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array of TRecord>)->cl_event; override;
    begin
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var res := new TRecord[o.Size64 div Marshal.SizeOf&<TRecord>];;
        own_qr.SetRes(res);
        var res_hnd := GCHandle.Alloc(res, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(res.Length * Marshal.SizeOf&<TRecord>),
          res_hnd.AddrOfPinnedObject,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override := sb += #10;
    
  end;
  
function MemorySegmentCCQ.AddGetArray1<TRecord>: CommandQueue<array of TRecord>; where TRecord: record;
begin
  Result := new MemorySegmentCommandGetArray1AutoSize<TRecord>(self) as CommandQueue<array of TRecord>;
end;

{$endregion GetArray1AutoSize}

{$region GetArray1}

type
  MemorySegmentCommandGetArray1<TRecord> = sealed class(EnqueueableGetCommand<MemorySegment, array of TRecord>)
  where TRecord: record;
    private len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(ccq: MemorySegmentCCQ; len: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.len := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array of TRecord>)->cl_event; override;
    begin
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>(len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var len := len_qr.GetRes;
        var res := new TRecord[len];
        own_qr.SetRes(res);
        var res_hnd := GCHandle.Alloc(res, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(int64(len) * Marshal.SizeOf&<TRecord>),
          res[0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddGetArray1<TRecord>(len: CommandQueue<integer>): CommandQueue<array of TRecord>; where TRecord: record;
begin
  Result := new MemorySegmentCommandGetArray1<TRecord>(self, len) as CommandQueue<array of TRecord>;
end;

{$endregion GetArray1}

{$region GetArray2}

type
  MemorySegmentCommandGetArray2<TRecord> = sealed class(EnqueueableGetCommand<MemorySegment, array[,] of TRecord>)
  where TRecord: record;
    private len1: CommandQueue<integer>;
    private len2: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 2;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(ccq: MemorySegmentCCQ; len1,len2: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.len1 := len1;
      self.len2 := len2;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array[,] of TRecord>)->cl_event; override;
    begin
      var len1_qr: QueueRes<integer>;
      var len2_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        len1_qr := invoker.InvokeBranch&<QueueRes<integer>>(len1.Invoke); if (len1_qr is IQueueResConst) then enq_evs.AddL2(len1_qr.ev) else enq_evs.AddL1(len1_qr.ev);
        len2_qr := invoker.InvokeBranch&<QueueRes<integer>>(len2.Invoke); if (len2_qr is IQueueResConst) then enq_evs.AddL2(len2_qr.ev) else enq_evs.AddL1(len2_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var len1 := len1_qr.GetRes;
        var len2 := len2_qr.GetRes;
        var res := new TRecord[len1,len2];
        own_qr.SetRes(res);
        var res_hnd := GCHandle.Alloc(res, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(int64(len1)*len2 * Marshal.SizeOf&<TRecord>),
          res[0,0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      len1.RegisterWaitables(g, prev_hubs);
      len2.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'len1: ';
      len1.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len2: ';
      len2.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddGetArray2<TRecord>(len1,len2: CommandQueue<integer>): CommandQueue<array[,] of TRecord>; where TRecord: record;
begin
  Result := new MemorySegmentCommandGetArray2<TRecord>(self, len1, len2) as CommandQueue<array[,] of TRecord>;
end;

{$endregion GetArray2}

{$region GetArray3}

type
  MemorySegmentCommandGetArray3<TRecord> = sealed class(EnqueueableGetCommand<MemorySegment, array[,,] of TRecord>)
  where TRecord: record;
    private len1: CommandQueue<integer>;
    private len2: CommandQueue<integer>;
    private len3: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 3;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(ccq: MemorySegmentCCQ; len1,len2,len3: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.len1 := len1;
      self.len2 := len2;
      self.len3 := len3;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array[,,] of TRecord>)->cl_event; override;
    begin
      var len1_qr: QueueRes<integer>;
      var len2_qr: QueueRes<integer>;
      var len3_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        len1_qr := invoker.InvokeBranch&<QueueRes<integer>>(len1.Invoke); if (len1_qr is IQueueResConst) then enq_evs.AddL2(len1_qr.ev) else enq_evs.AddL1(len1_qr.ev);
        len2_qr := invoker.InvokeBranch&<QueueRes<integer>>(len2.Invoke); if (len2_qr is IQueueResConst) then enq_evs.AddL2(len2_qr.ev) else enq_evs.AddL1(len2_qr.ev);
        len3_qr := invoker.InvokeBranch&<QueueRes<integer>>(len3.Invoke); if (len3_qr is IQueueResConst) then enq_evs.AddL2(len3_qr.ev) else enq_evs.AddL1(len3_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var len1 := len1_qr.GetRes;
        var len2 := len2_qr.GetRes;
        var len3 := len3_qr.GetRes;
        var res := new TRecord[len1,len2,len3];
        own_qr.SetRes(res);
        var res_hnd := GCHandle.Alloc(res, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(int64(len1)*len2*len3 * Marshal.SizeOf&<TRecord>),
          res[0,0,0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      len1.RegisterWaitables(g, prev_hubs);
      len2.RegisterWaitables(g, prev_hubs);
      len3.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'len1: ';
      len1.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len2: ';
      len2.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len3: ';
      len3.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddGetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): CommandQueue<array[,,] of TRecord>; where TRecord: record;
begin
  Result := new MemorySegmentCommandGetArray3<TRecord>(self, len1, len2, len3) as CommandQueue<array[,,] of TRecord>;
end;

{$endregion GetArray3}

{$endregion Get}

{$endregion Explicit}

{$endregion MemorySegment}

{$region CLArray}

{$region Implicit}

{$region 1#Write&Read}

function CLArray<T>.WriteData(ptr: CommandQueue<IntPtr>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteData(ptr));
end;

function CLArray<T>.WriteData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteData(ptr, ind, len));
end;

function CLArray<T>.ReadData(ptr: CommandQueue<IntPtr>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadData(ptr));
end;

function CLArray<T>.ReadData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadData(ptr, ind, len));
end;

function CLArray<T>.WriteData(ptr: pointer): CLArray<T>;
begin
  Result := WriteData(IntPtr(ptr));
end;

function CLArray<T>.WriteData(ptr: pointer; ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := WriteData(IntPtr(ptr), ind, len);
end;

function CLArray<T>.ReadData(ptr: pointer): CLArray<T>;
begin
  Result := ReadData(IntPtr(ptr));
end;

function CLArray<T>.ReadData(ptr: pointer; ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := ReadData(IntPtr(ptr), ind, len);
end;

function CLArray<T>.WriteValue(val: &T; ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteValue(val, ind));
end;

function CLArray<T>.WriteValue(val: CommandQueue<&T>; ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteValue(val, ind));
end;

function CLArray<T>.WriteValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteValue(val, ind));
end;

function CLArray<T>.WriteValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteValue(val, ind));
end;

function CLArray<T>.ReadValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadValue(val, ind));
end;

function CLArray<T>.ReadValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadValue(val, ind));
end;

function CLArray<T>.WriteArray(a: CommandQueue<array of &T>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteArray(a));
end;

function CLArray<T>.WriteArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddWriteArray(a, ind, len, a_ind));
end;

function CLArray<T>.ReadArray(a: CommandQueue<array of &T>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadArray(a));
end;

function CLArray<T>.ReadArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddReadArray(a, ind, len, a_ind));
end;

{$endregion 1#Write&Read}

{$region 2#Fill}

function CLArray<T>.FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillData(ptr, pattern_len));
end;

function CLArray<T>.FillData(ptr: CommandQueue<IntPtr>; pattern_len, ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillData(ptr, pattern_len, ind, len));
end;

function CLArray<T>.FillData(ptr: pointer; pattern_len: CommandQueue<integer>): CLArray<T>;
begin
  Result := FillData(IntPtr(ptr), pattern_len);
end;

function CLArray<T>.FillData(ptr: pointer; pattern_len, ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := FillData(IntPtr(ptr), pattern_len, ind, len);
end;

function CLArray<T>.FillValue(val: &T): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillValue(val));
end;

function CLArray<T>.FillValue(val: CommandQueue<&T>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillValue(val));
end;

function CLArray<T>.FillValue(val: &T; ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillValue(val, ind, len));
end;

function CLArray<T>.FillValue(val: CommandQueue<&T>; ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillValue(val, ind, len));
end;

function CLArray<T>.FillArray(a: CommandQueue<array of &T>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillArray(a));
end;

function CLArray<T>.FillArray(a: CommandQueue<array of &T>; a_offset,pattern_len, ind,len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddFillArray(a, a_offset, pattern_len, ind, len));
end;

{$endregion 2#Fill}

{$region 3#Copy}

function CLArray<T>.CopyTo(a: CommandQueue<CLArray<T>>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddCopyTo(a));
end;

function CLArray<T>.CopyTo(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddCopyTo(a, from_ind, to_ind, len));
end;

function CLArray<T>.CopyFrom(a: CommandQueue<CLArray<T>>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddCopyFrom(a));
end;

function CLArray<T>.CopyFrom(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddCopyFrom(a, from_ind, to_ind, len));
end;

{$endregion 3#Copy}

{$region Get}

function CLArray<T>.GetValue(ind: CommandQueue<integer>): &T;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddGetValue(ind));
end;

function CLArray<T>.GetArray: array of &T;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddGetArray);
end;

function CLArray<T>.GetArray(ind, len: CommandQueue<integer>): array of &T;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.AddGetArray(ind, len));
end;

{$endregion Get}

{$endregion Implicit}

{$region Explicit}

{$region 1#Write&Read}

{$region WriteDataAutoSize}

type
  CLArrayCommandWriteDataAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private ptr: CommandQueue<IntPtr>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(ptr: CommandQueue<IntPtr>);
    begin
      self.ptr := ptr;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(o.ByteSize),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddWriteData(ptr: CommandQueue<IntPtr>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandWriteDataAutoSize<T>(ptr));
end;

{$endregion WriteDataAutoSize}

{$region WriteData}

type
  CLArrayCommandWriteData<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private ptr: CommandQueue<IntPtr>;
    private ind: CommandQueue<integer>;
    private len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 3;
    
    public constructor(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>);
    begin
      self.ptr := ptr;
      self.ind := ind;
      self.len := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>(len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var ind := ind_qr.GetRes;
        var len := len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
      ind.RegisterWaitables(g, prev_hubs);
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddWriteData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandWriteData<T>(ptr, ind, len));
end;

{$endregion WriteData}

{$region ReadDataAutoSize}

type
  CLArrayCommandReadDataAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private ptr: CommandQueue<IntPtr>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(ptr: CommandQueue<IntPtr>);
    begin
      self.ptr := ptr;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(o.ByteSize),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddReadData(ptr: CommandQueue<IntPtr>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandReadDataAutoSize<T>(ptr));
end;

{$endregion ReadDataAutoSize}

{$region ReadData}

type
  CLArrayCommandReadData<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private ptr: CommandQueue<IntPtr>;
    private ind: CommandQueue<integer>;
    private len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 3;
    
    public constructor(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>);
    begin
      self.ptr := ptr;
      self.ind := ind;
      self.len := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>(len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var ind := ind_qr.GetRes;
        var len := len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
      ind.RegisterWaitables(g, prev_hubs);
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddReadData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandReadData<T>(ptr, ind, len));
end;

{$endregion ReadData}

{$region WriteDataAutoSize}

function CLArrayCCQ<T>.AddWriteData(ptr: pointer): CLArrayCCQ<T>;
begin
  Result := AddWriteData(IntPtr(ptr));
end;

{$endregion WriteDataAutoSize}

{$region WriteData}

function CLArrayCCQ<T>.AddWriteData(ptr: pointer; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddWriteData(IntPtr(ptr), ind, len);
end;

{$endregion WriteData}

{$region ReadDataAutoSize}

function CLArrayCCQ<T>.AddReadData(ptr: pointer): CLArrayCCQ<T>;
begin
  Result := AddReadData(IntPtr(ptr));
end;

{$endregion ReadDataAutoSize}

{$region ReadData}

function CLArrayCCQ<T>.AddReadData(ptr: pointer; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddReadData(IntPtr(ptr), ind, len);
end;

{$endregion ReadData}

{$region WriteValue}

type
  CLArrayCommandWriteValue<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: ^&T := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<&T>));
    private ind: CommandQueue<integer>;
    
    protected procedure Finalize; override;
    begin
      Marshal.FreeHGlobal(new IntPtr(val));
    end;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(val: &T; ind: CommandQueue<integer>);
    begin
      self.val^ := val;
      self.ind  := ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ind := ind_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          new IntPtr(val),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val^);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddWriteValue(val: &T; ind: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandWriteValue<T>(val, ind));
end;

{$endregion WriteValue}

{$region WriteValueQ}

type
  CLArrayCommandWriteValueQ<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: CommandQueue<&T>;
    private ind: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 2;
    
    public constructor(val: CommandQueue<&T>; ind: CommandQueue<integer>);
    begin
      self.val := val;
      self.ind := ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var val_qr: QueueRes<&T>;
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create, true, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueRes<&T>>((g,l)->val.Invoke(g, l.WithPtrNeed( True))); if (val_qr is IQueueResDelayedPtr) or (val_qr is IQueueResConst) then enq_evs.AddL2(val_qr.ev) else enq_evs.AddL1(val_qr.ev);
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->ind.Invoke(g, l.WithPtrNeed(False))); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var val := val_qr.ToPtr;
        var ind := ind_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          new IntPtr(val.GetPtr),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.RegisterWaitables(g, prev_hubs);
      ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddWriteValue(val: CommandQueue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandWriteValueQ<T>(val, ind));
end;

{$endregion WriteValueQ}

{$region WriteValueN}

type
  CLArrayCommandWriteValueN<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: NativeValue<&T>;
    private ind: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(val: NativeValue<&T>; ind: CommandQueue<integer>);
    begin
      self.val := val;
      self.ind := ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ind := ind_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddWriteValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandWriteValueN<T>(val, ind));
end;

{$endregion WriteValueN}

{$region WriteValueNQ}

type
  CLArrayCommandWriteValueNQ<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: CommandQueue<NativeValue<&T>>;
    private ind: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 2;
    
    public constructor(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>);
    begin
      self.val := val;
      self.ind := ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var val_qr: QueueRes<NativeValue<&T>>;
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueRes<NativeValue<&T>>>(val.Invoke); if (val_qr is IQueueResConst) then enq_evs.AddL2(val_qr.ev) else enq_evs.AddL1(val_qr.ev);
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var val := val_qr.GetRes;
        var ind := ind_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.RegisterWaitables(g, prev_hubs);
      ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddWriteValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandWriteValueNQ<T>(val, ind));
end;

{$endregion WriteValueNQ}

{$region ReadValueN}

type
  CLArrayCommandReadValueN<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: NativeValue<&T>;
    private ind: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(val: NativeValue<&T>; ind: CommandQueue<integer>);
    begin
      self.val := val;
      self.ind := ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ind := ind_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddReadValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandReadValueN<T>(val, ind));
end;

{$endregion ReadValueN}

{$region ReadValueNQ}

type
  CLArrayCommandReadValueNQ<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: CommandQueue<NativeValue<&T>>;
    private ind: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 2;
    
    public constructor(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>);
    begin
      self.val := val;
      self.ind := ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var val_qr: QueueRes<NativeValue<&T>>;
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueRes<NativeValue<&T>>>(val.Invoke); if (val_qr is IQueueResConst) then enq_evs.AddL2(val_qr.ev) else enq_evs.AddL1(val_qr.ev);
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var val := val_qr.GetRes;
        var ind := ind_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.RegisterWaitables(g, prev_hubs);
      ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddReadValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandReadValueNQ<T>(val, ind));
end;

{$endregion ReadValueNQ}

{$region WriteArrayAutoSize}

type
  CLArrayCommandWriteArrayAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private a: CommandQueue<array of &T>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(a: CommandQueue<array of &T>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of &T>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<T>),
          a[0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddWriteArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandWriteArrayAutoSize<T>(a));
end;

{$endregion WriteArrayAutoSize}

{$region WriteArray}

type
  CLArrayCommandWriteArray<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private     a: CommandQueue<array of &T>;
    private   ind: CommandQueue<integer>;
    private   len: CommandQueue<integer>;
    private a_ind: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 4;
    
    public constructor(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>);
    begin
      self.    a :=     a;
      self.  ind :=   ind;
      self.  len :=   len;
      self.a_ind := a_ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var     a_qr: QueueRes<array of &T>;
      var   ind_qr: QueueRes<integer>;
      var   len_qr: QueueRes<integer>;
      var a_ind_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
            a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(    a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
          ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(  ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
          len_qr := invoker.InvokeBranch&<QueueRes<integer>>(  len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
        a_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(a_ind.Invoke); if (a_ind_qr is IQueueResConst) then enq_evs.AddL2(a_ind_qr.ev) else enq_evs.AddL1(a_ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var     a :=     a_qr.GetRes;
        var   ind :=   ind_qr.GetRes;
        var   len :=   len_qr.GetRes;
        var a_ind := a_ind_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind*Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          a[a_ind],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
          a.RegisterWaitables(g, prev_hubs);
        ind.RegisterWaitables(g, prev_hubs);
        len.RegisterWaitables(g, prev_hubs);
      a_ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_ind: ';
      a_ind.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddWriteArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandWriteArray<T>(a, ind, len, a_ind));
end;

{$endregion WriteArray}

{$region ReadArrayAutoSize}

type
  CLArrayCommandReadArrayAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private a: CommandQueue<array of &T>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(a: CommandQueue<array of &T>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of &T>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<T>),
          a[0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddReadArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandReadArrayAutoSize<T>(a));
end;

{$endregion ReadArrayAutoSize}

{$region ReadArray}

type
  CLArrayCommandReadArray<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private     a: CommandQueue<array of &T>;
    private   ind: CommandQueue<integer>;
    private   len: CommandQueue<integer>;
    private a_ind: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 4;
    
    public constructor(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>);
    begin
      self.    a :=     a;
      self.  ind :=   ind;
      self.  len :=   len;
      self.a_ind := a_ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var     a_qr: QueueRes<array of &T>;
      var   ind_qr: QueueRes<integer>;
      var   len_qr: QueueRes<integer>;
      var a_ind_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
            a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(    a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
          ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(  ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
          len_qr := invoker.InvokeBranch&<QueueRes<integer>>(  len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
        a_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(a_ind.Invoke); if (a_ind_qr is IQueueResConst) then enq_evs.AddL2(a_ind_qr.ev) else enq_evs.AddL1(a_ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var     a :=     a_qr.GetRes;
        var   ind :=   ind_qr.GetRes;
        var   len :=   len_qr.GetRes;
        var a_ind := a_ind_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind*Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          a[a_ind],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
          a.RegisterWaitables(g, prev_hubs);
        ind.RegisterWaitables(g, prev_hubs);
        len.RegisterWaitables(g, prev_hubs);
      a_ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_ind: ';
      a_ind.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddReadArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandReadArray<T>(a, ind, len, a_ind));
end;

{$endregion ReadArray}

{$endregion 1#Write&Read}

{$region 2#Fill}

{$region FillDataAutoSize}

type
  CLArrayCommandFillDataAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private         ptr: CommandQueue<IntPtr>;
    private pattern_len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 2;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.pattern_len := pattern_len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(        ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.Invoke); if (pattern_len_qr is IQueueResConst) then enq_evs.AddL2(pattern_len_qr.ev) else enq_evs.AddL1(pattern_len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len*Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
              ptr.RegisterWaitables(g, prev_hubs);
      pattern_len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'pattern_len: ';
      pattern_len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandFillDataAutoSize<T>(ptr, pattern_len));
end;

{$endregion FillDataAutoSize}

{$region FillData}

type
  CLArrayCommandFillData<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private         ptr: CommandQueue<IntPtr>;
    private pattern_len: CommandQueue<integer>;
    private         ind: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 4;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len, ind, len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.pattern_len := pattern_len;
      self.        ind :=         ind;
      self.        len :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      var         ind_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(        ptr.Invoke); if (ptr_qr is IQueueResConst) then enq_evs.AddL2(ptr_qr.ev) else enq_evs.AddL1(ptr_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.Invoke); if (pattern_len_qr is IQueueResConst) then enq_evs.AddL2(pattern_len_qr.ev) else enq_evs.AddL1(pattern_len_qr.ev);
                ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(        ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var         ind :=         ind_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len*Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
              ptr.RegisterWaitables(g, prev_hubs);
      pattern_len.RegisterWaitables(g, prev_hubs);
              ind.RegisterWaitables(g, prev_hubs);
              len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'pattern_len: ';
      pattern_len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandFillData<T>(ptr, pattern_len, ind, len));
end;

{$endregion FillData}

{$region FillDataAutoSize}

function CLArrayCCQ<T>.AddFillData(ptr: pointer; pattern_len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddFillData(IntPtr(ptr), pattern_len);
end;

{$endregion FillDataAutoSize}

{$region FillData}

function CLArrayCCQ<T>.AddFillData(ptr: pointer; pattern_len, ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddFillData(IntPtr(ptr), pattern_len, ind, len);
end;

{$endregion FillData}

{$region FillValueAutoSize}

type
  CLArrayCommandFillValueAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: ^&T := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<&T>));
    
    protected procedure Finalize; override;
    begin
      Marshal.FreeHGlobal(new IntPtr(val));
    end;
    
    public function EnqEvCapacity: integer; override := 0;
    
    public constructor(val: &T);
    begin
      self.val^ := val;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      
      Result := (o, cq, err_handler, evs)->
      begin
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val^);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddFillValue(val: &T): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandFillValueAutoSize<T>(val));
end;

{$endregion FillValueAutoSize}

{$region FillValueAutoSizeQ}

type
  CLArrayCommandFillValueAutoSizeQ<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: CommandQueue<&T>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(val: CommandQueue<&T>);
    begin
      self.val := val;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var val_qr: QueueRes<&T>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(True), true, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueRes<&T>>(val.Invoke); if (val_qr is IQueueResDelayedPtr) or (val_qr is IQueueResConst) then enq_evs.AddL2(val_qr.ev) else enq_evs.AddL1(val_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var val := val_qr.ToPtr;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val.GetPtr), new UIntPtr(Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddFillValue(val: CommandQueue<&T>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandFillValueAutoSizeQ<T>(val));
end;

{$endregion FillValueAutoSizeQ}

{$region FillValue}

type
  CLArrayCommandFillValue<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: ^&T := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<&T>));
    private ind: CommandQueue<integer>;
    private len: CommandQueue<integer>;
    
    protected procedure Finalize; override;
    begin
      Marshal.FreeHGlobal(new IntPtr(val));
    end;
    
    public function EnqEvCapacity: integer; override := 2;
    
    public constructor(val: &T; ind, len: CommandQueue<integer>);
    begin
      self.val^ := val;
      self.ind  := ind;
      self.len  := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>(len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ind := ind_qr.GetRes;
        var len := len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.RegisterWaitables(g, prev_hubs);
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val^);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddFillValue(val: &T; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandFillValue<T>(val, ind, len));
end;

{$endregion FillValue}

{$region FillValueQ}

type
  CLArrayCommandFillValueQ<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: CommandQueue<&T>;
    private ind: CommandQueue<integer>;
    private len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 3;
    
    public constructor(val: CommandQueue<&T>; ind, len: CommandQueue<integer>);
    begin
      self.val := val;
      self.ind := ind;
      self.len := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var val_qr: QueueRes<&T>;
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create, true, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueRes<&T>>((g,l)->val.Invoke(g, l.WithPtrNeed( True))); if (val_qr is IQueueResDelayedPtr) or (val_qr is IQueueResConst) then enq_evs.AddL2(val_qr.ev) else enq_evs.AddL1(val_qr.ev);
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->ind.Invoke(g, l.WithPtrNeed(False))); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>((g,l)->len.Invoke(g, l.WithPtrNeed(False))); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var val := val_qr.ToPtr;
        var ind := ind_qr.GetRes;
        var len := len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val.GetPtr), new UIntPtr(Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.RegisterWaitables(g, prev_hubs);
      ind.RegisterWaitables(g, prev_hubs);
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddFillValue(val: CommandQueue<&T>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandFillValueQ<T>(val, ind, len));
end;

{$endregion FillValueQ}

{$region FillArrayAutoSize}

type
  CLArrayCommandFillArrayAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private a: CommandQueue<array of &T>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(a: CommandQueue<array of &T>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of &T>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[0], new UIntPtr(a.Length*Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddFillArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandFillArrayAutoSize<T>(a));
end;

{$endregion FillArrayAutoSize}

{$region FillArray}

type
  CLArrayCommandFillArray<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private           a: CommandQueue<array of &T>;
    private    a_offset: CommandQueue<integer>;
    private pattern_len: CommandQueue<integer>;
    private         ind: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 5;
    
    public constructor(a: CommandQueue<array of &T>; a_offset,pattern_len, ind,len: CommandQueue<integer>);
    begin
      self.          a :=           a;
      self.   a_offset :=    a_offset;
      self.pattern_len := pattern_len;
      self.        ind :=         ind;
      self.        len :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var           a_qr: QueueRes<array of &T>;
      var    a_offset_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         ind_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(          a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
           a_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(   a_offset.Invoke); if (a_offset_qr is IQueueResConst) then enq_evs.AddL2(a_offset_qr.ev) else enq_evs.AddL1(a_offset_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.Invoke); if (pattern_len_qr is IQueueResConst) then enq_evs.AddL2(pattern_len_qr.ev) else enq_evs.AddL1(pattern_len_qr.ev);
                ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(        ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var           a :=           a_qr.GetRes;
        var    a_offset :=    a_offset_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var         ind :=         ind_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset], new UIntPtr(pattern_len*Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
                a.RegisterWaitables(g, prev_hubs);
         a_offset.RegisterWaitables(g, prev_hubs);
      pattern_len.RegisterWaitables(g, prev_hubs);
              ind.RegisterWaitables(g, prev_hubs);
              len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'a_offset: ';
      a_offset.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'pattern_len: ';
      pattern_len.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddFillArray(a: CommandQueue<array of &T>; a_offset,pattern_len, ind,len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandFillArray<T>(a, a_offset, pattern_len, ind, len));
end;

{$endregion FillArray}

{$endregion 2#Fill}

{$region 3#Copy}

{$region CopyToAutoSize}

type
  CLArrayCommandCopyToAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private a: CommandQueue<CLArray<T>>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(a: CommandQueue<CLArray<T>>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<CLArray<T>>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<CLArray<T>>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, o.Native,a.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          new UIntPtr(Min(o.ByteSize, a.ByteSize)),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddCopyTo(a: CommandQueue<CLArray<T>>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandCopyToAutoSize<T>(a));
end;

{$endregion CopyToAutoSize}

{$region CopyTo}

type
  CLArrayCommandCopyTo<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private        a: CommandQueue<CLArray<T>>;
    private from_ind: CommandQueue<integer>;
    private   to_ind: CommandQueue<integer>;
    private      len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 4;
    
    public constructor(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>);
    begin
      self.       a :=        a;
      self.from_ind := from_ind;
      self.  to_ind :=   to_ind;
      self.     len :=      len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        a_qr: QueueRes<CLArray<T>>;
      var from_ind_qr: QueueRes<integer>;
      var   to_ind_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
               a_qr := invoker.InvokeBranch&<QueueRes<CLArray<T>>>(       a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
        from_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(from_ind.Invoke); if (from_ind_qr is IQueueResConst) then enq_evs.AddL2(from_ind_qr.ev) else enq_evs.AddL1(from_ind_qr.ev);
          to_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(  to_ind.Invoke); if (to_ind_qr is IQueueResConst) then enq_evs.AddL2(to_ind_qr.ev) else enq_evs.AddL1(to_ind_qr.ev);
             len_qr := invoker.InvokeBranch&<QueueRes<integer>>(     len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        a :=        a_qr.GetRes;
        var from_ind := from_ind_qr.GetRes;
        var   to_ind :=   to_ind_qr.GetRes;
        var      len :=      len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, o.Native,a.Native,
          new UIntPtr(from_ind*Marshal.SizeOf&<T>), new UIntPtr(to_ind*Marshal.SizeOf&<T>),
          new UIntPtr(len*Marshal.SizeOf&<T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             a.RegisterWaitables(g, prev_hubs);
      from_ind.RegisterWaitables(g, prev_hubs);
        to_ind.RegisterWaitables(g, prev_hubs);
           len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'from_ind: ';
      from_ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'to_ind: ';
      to_ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddCopyTo(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandCopyTo<T>(a, from_ind, to_ind, len));
end;

{$endregion CopyTo}

{$region CopyFromAutoSize}

type
  CLArrayCommandCopyFromAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private a: CommandQueue<CLArray<T>>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(a: CommandQueue<CLArray<T>>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<CLArray<T>>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<CLArray<T>>>(a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, a.Native,o.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          new UIntPtr(Min(o.ByteSize, a.ByteSize)),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddCopyFrom(a: CommandQueue<CLArray<T>>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandCopyFromAutoSize<T>(a));
end;

{$endregion CopyFromAutoSize}

{$region CopyFrom}

type
  CLArrayCommandCopyFrom<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private        a: CommandQueue<CLArray<T>>;
    private from_ind: CommandQueue<integer>;
    private   to_ind: CommandQueue<integer>;
    private      len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 4;
    
    public constructor(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>);
    begin
      self.       a :=        a;
      self.from_ind := from_ind;
      self.  to_ind :=   to_ind;
      self.     len :=      len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        a_qr: QueueRes<CLArray<T>>;
      var from_ind_qr: QueueRes<integer>;
      var   to_ind_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
               a_qr := invoker.InvokeBranch&<QueueRes<CLArray<T>>>(       a.Invoke); if (a_qr is IQueueResConst) then enq_evs.AddL2(a_qr.ev) else enq_evs.AddL1(a_qr.ev);
        from_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(from_ind.Invoke); if (from_ind_qr is IQueueResConst) then enq_evs.AddL2(from_ind_qr.ev) else enq_evs.AddL1(from_ind_qr.ev);
          to_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(  to_ind.Invoke); if (to_ind_qr is IQueueResConst) then enq_evs.AddL2(to_ind_qr.ev) else enq_evs.AddL1(to_ind_qr.ev);
             len_qr := invoker.InvokeBranch&<QueueRes<integer>>(     len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        a :=        a_qr.GetRes;
        var from_ind := from_ind_qr.GetRes;
        var   to_ind :=   to_ind_qr.GetRes;
        var      len :=      len_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, a.Native,o.Native,
          new UIntPtr(from_ind*Marshal.SizeOf&<T>), new UIntPtr(to_ind*Marshal.SizeOf&<T>),
          new UIntPtr(len*Marshal.SizeOf&<T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             a.RegisterWaitables(g, prev_hubs);
      from_ind.RegisterWaitables(g, prev_hubs);
        to_ind.RegisterWaitables(g, prev_hubs);
           len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'from_ind: ';
      from_ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'to_ind: ';
      to_ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddCopyFrom(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandCopyFrom<T>(a, from_ind, to_ind, len));
end;

{$endregion CopyFrom}

{$endregion 3#Copy}

{$region Get}

{$region GetValue}

type
  CLArrayCommandGetValue<T> = sealed class(EnqueueableGetCommand<CLArray<T>, &T>)
  where T: record;
    private ind: CommandQueue<integer>;
    
    public function ForcePtrQr: boolean; override := true;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(ccq: CLArrayCCQ<T>; ind: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.ind := ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<&T>)->cl_event; override;
    begin
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var ind := ind_qr.GetRes;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(int64(ind) * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          new IntPtr((own_qr as QueueResDelayedPtr<&T>).ptr),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        var own_qr_hnd := GCHandle.Alloc(own_qr);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          own_qr_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [own_qr]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddGetValue(ind: CommandQueue<integer>): CommandQueue<&T>;
begin
  Result := new CLArrayCommandGetValue<T>(self, ind) as CommandQueue<&T>;
end;

{$endregion GetValue}

{$region GetArrayAutoSize}

type
  CLArrayCommandGetArrayAutoSize<T> = sealed class(EnqueueableGetCommand<CLArray<T>, array of &T>)
  where T: record;
    
    public function EnqEvCapacity: integer; override := 0;
    
    public constructor(ccq: CLArrayCCQ<T>);
    begin
      inherited Create(ccq);
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array of &T>)->cl_event; override;
    begin
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var res := new T[o.Length];
        own_qr.SetRes(res);
        var res_hnd := GCHandle.Alloc(res, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          res_hnd.AddrOfPinnedObject,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override := sb += #10;
    
  end;
  
function CLArrayCCQ<T>.AddGetArray: CommandQueue<array of &T>;
begin
  Result := new CLArrayCommandGetArrayAutoSize<T>(self) as CommandQueue<array of &T>;
end;

{$endregion GetArrayAutoSize}

{$region GetArray}

type
  CLArrayCommandGetArray<T> = sealed class(EnqueueableGetCommand<CLArray<T>, array of &T>)
  where T: record;
    private ind: CommandQueue<integer>;
    private len: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 2;
    
    public constructor(ccq: CLArrayCCQ<T>; ind, len: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.ind := ind;
      self.len := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array of &T>)->cl_event; override;
    begin
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(CLTaskLocalDataNil.Create.WithPtrNeed(False), true, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.Invoke); if (ind_qr is IQueueResConst) then enq_evs.AddL2(ind_qr.ev) else enq_evs.AddL1(ind_qr.ev);
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>(len.Invoke); if (len_qr is IQueueResConst) then enq_evs.AddL2(len_qr.ev) else enq_evs.AddL1(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var ind := ind_qr.GetRes;
        var len := len_qr.GetRes;
        var res := new T[len];
        own_qr.SetRes(res);
        var res_hnd := GCHandle.Alloc(res, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(int64(ind) * Marshal.SizeOf&<T>), new UIntPtr(int64(len) * Marshal.SizeOf&<T>),
          res_hnd.AddrOfPinnedObject,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.RegisterWaitables(g, prev_hubs);
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddGetArray(ind, len: CommandQueue<integer>): CommandQueue<array of &T>;
begin
  Result := new CLArrayCommandGetArray<T>(self, ind, len) as CommandQueue<array of &T>;
end;

{$endregion GetArray}

{$endregion Get}

{$endregion Explicit}

{$endregion CLArray}

{$endregion Enqueueable's}

{$region Global subprograms}

{$region HFQ/HPQ}

{$region Common}

type
  CommandQueueHostCommon<TDelegate> = record
  where TDelegate: Delegate;
    private d: TDelegate;
    
    public procedure ToString(sb: StringBuilder);
    begin
      sb += ': ';
      CommandQueueBase.ToStringWriteDelegate(sb, d);
      sb += #10;
    end;
    
  end;
  
{$endregion Common}

{$region Backgound}

{$region Func}

type
  CommandQueueHostBackgoundFuncBase<T, TFunc> = abstract class(CommandQueue<T>)
  where TFunc: Delegate;
    private data: CommandQueueHostCommon<TFunc>;
    
    public constructor(f: TFunc) := data.d := f;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function ExecFunc(c: Context): T; abstract;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      var c := g.c;
      
      var qr := QueueRes&<T>.MakeNewDelayedOrPtr(l.need_ptr_qr);
      qr.ev := UserEvent.StartBackgroundWork(l.prev_ev, ()->qr.SetRes( ExecFunc(c) ), g
        {$ifdef EventDebug}, $'body of {self.GetType}'{$endif}
      );
      
      Result := qr;
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override := data.ToString(sb);
    
  end;
  
  CommandQueueHostBackgoundFunc<T> = sealed class(CommandQueueHostBackgoundFuncBase<T, ()->T>)
    
    protected function ExecFunc(c: Context): T; override := data.d();
    
  end;
  CommandQueueHostBackgoundFuncC<T> = sealed class(CommandQueueHostBackgoundFuncBase<T, Context->T>)
    
    protected function ExecFunc(c: Context): T; override := data.d(c);
    
  end;
  
function HFQ<T>(f: ()->T) :=
new CommandQueueHostBackgoundFunc<T>(f);
function HFQ<T>(f: Context->T) :=
new CommandQueueHostBackgoundFuncC<T>(f);

{$endregion Func}

{$region Proc}

type
  CommandQueueHostBackgoundProcBase<TProc> = abstract class(CommandQueueNil)
  where TProc: Delegate;
    private data: CommandQueueHostCommon<TProc>;
    
    public constructor(p: TProc) := data.d := p;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure ExecProc(c: Context); abstract;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueResNil; override;
    begin
      var c := g.c;
      
      var res_ev :=  UserEvent.StartBackgroundWork(l.prev_ev, ()->ExecProc(c), g
        {$ifdef EventDebug}, $'body of {self.GetType}'{$endif}
      );
      
      Result := new QueueResConstNil(res_ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override := data.ToString(sb);
    
  end;
  
  CommandQueueHostBackgoundProc = sealed class(CommandQueueHostBackgoundProcBase<()->()>)
    
    protected procedure ExecProc(c: Context); override := data.d();
    
  end;
  CommandQueueHostBackgoundProcC = sealed class(CommandQueueHostBackgoundProcBase<Context->()>)
    
    protected procedure ExecProc(c: Context); override := data.d(c);
    
  end;
  
function HPQ(p: ()->()) :=
new CommandQueueHostBackgoundProc(p);
function HPQ(p: Context->()) :=
new CommandQueueHostBackgoundProcC(p);

{$endregion Proc}

{$endregion Backgound}

{$region Quick}

{$region Func}

type
  CommandQueueHostQuickFuncBase<T, TFunc> = abstract class(CommandQueue<T>)
  where TFunc: Delegate;
    private data: CommandQueueHostCommon<TFunc>;
    
    public constructor(f: TFunc) := data.d := f;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function ExecFunc(c: Context): T; abstract;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      if l.prev_ev.count=0 then
        Result := new QueueResConst<T>(ExecFunc(g.c), EventList.Empty) else
      if l.need_ptr_qr then
      begin
        var c := g.c;
        var res := new QueueResDelayedPtr<T>;
        var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev of {self.GetType}'{$endif});
        var err_handler := g.curr_err_handler;
        l.prev_ev.MultiAttachCallback(false, ()->
        begin
          if not err_handler.HadError(true) then
          try
            res.SetRes(ExecFunc(c));
          except
            on e: Exception do err_handler.AddErr(e);
          end;
          res_ev.SetComplete;
        end{$ifdef EventDebug}, $'body of {self.GetType}'{$endif});
        res.ev := res_ev;
        Result := res;
      end else
      begin
        var c := g.c;
        Result := new QueueResFunc<T>(()->ExecFunc(c), l.prev_ev);
      end;
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override := data.ToString(sb);
    
  end;
  
  CommandQueueHostQuickFunc<T> = sealed class(CommandQueueHostQuickFuncBase<T, ()->T>)
    
    protected function ExecFunc(c: Context): T; override := data.d();
    
  end;
  CommandQueueHostQuickFuncC<T> = sealed class(CommandQueueHostQuickFuncBase<T, Context->T>)
    
    protected function ExecFunc(c: Context): T; override := data.d(c);
    
  end;
  
function HFQQ<T>(f: ()->T) :=
new CommandQueueHostQuickFunc<T>(f);
function HFQQ<T>(f: Context->T) :=
new CommandQueueHostQuickFuncC<T>(f);

{$endregion Func}

{$region Proc}

type
  CommandQueueHostQuickProcBase<TProc> = abstract class(CommandQueueNil)
  where TProc: Delegate;
    private data: CommandQueueHostCommon<TProc>;
    
    public constructor(p: TProc) := data.d := p;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure ExecProc(c: Context); abstract;
    
    private procedure InvokeProc(err_handler: CLTaskErrHandler; c: Context) :=
    if not err_handler.HadError(true) then
    try
      ExecProc(c);
    except
      on e: Exception do err_handler.AddErr(e);
    end;
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalDataNil): QueueResNil; override;
    begin
      var c := g.c;
      var err_handler := g.curr_err_handler;
      Result := new QueueResProcNil(()->self.InvokeProc(err_handler, c), l.prev_ev); 
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override := data.ToString(sb);
    
  end;
  
  CommandQueueHostQuickProc = sealed class(CommandQueueHostQuickProcBase<()->()>)
    
    protected procedure ExecProc(c: Context); override := data.d();
    
  end;
  CommandQueueHostQuickProcC = sealed class(CommandQueueHostQuickProcBase<Context->()>)
    
    protected procedure ExecProc(c: Context); override := data.d(c);
    
  end;
  
function HPQQ(p: ()->()) :=
new CommandQueueHostQuickProc(p);
function HPQQ(p: Context->()) :=
new CommandQueueHostQuickProcC(p);

{$endregion Proc}

{$endregion Quick}

{$endregion HFQ/HPQ}

{$region CombineQueue's}

{$region Sync}

{$region NonConv}

function CombineSyncQueueBase(params qs: array of CommandQueueBase) := QueueArrayUtils.ConstructSync(qs);
function CombineSyncQueueBase(qs: sequence of CommandQueueBase) := QueueArrayUtils.ConstructSync(qs);

function CombineSyncQueueNil(params qs: array of CommandQueueNil) := QueueArrayUtils.ConstructSyncNil(qs.Cast&<CommandQueueBase>);
function CombineSyncQueueNil(qs: sequence of CommandQueueNil) := QueueArrayUtils.ConstructSyncNil(qs.Cast&<CommandQueueBase>);

function CombineSyncQueue<T>(params qs: array of CommandQueue<T>) := QueueArrayUtils.ConstructSync&<T>(qs.Cast&<CommandQueueBase>);
function CombineSyncQueue<T>(qs: sequence of CommandQueue<T>) := QueueArrayUtils.ConstructSync&<T>(qs.Cast&<CommandQueueBase>);

function CombineSyncQueueNil(qs: sequence of CommandQueueBase; last: CommandQueueNil) := QueueArrayUtils.ConstructSyncNil(qs.Append&<CommandQueueBase>(last));

function CombineSyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>) := QueueArrayUtils.ConstructSync&<T>(qs.Append&<CommandQueueBase>(last));

{$endregion NonConv}

{$region Conv}

{$region NonContext}

function CombineConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>) := new BackgroundConvSyncQueueArray<TInp, TRes>(qs.ToArray, conv);
function CombineConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>) := new BackgroundConvSyncQueueArray<TInp, TRes>(qs.ToArray, conv);

function CombineConvSyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new BackgroundConvSyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineConvSyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new BackgroundConvSyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineConvSyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new BackgroundConvSyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineConvSyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new BackgroundConvSyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineConvSyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new BackgroundConvSyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineConvSyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new BackgroundConvSyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>) := new QuickConvSyncQueueArray<TInp, TRes>(qs.ToArray, conv);
function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>) := new QuickConvSyncQueueArray<TInp, TRes>(qs.ToArray, conv);

function CombineQuickConvSyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new QuickConvSyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineQuickConvSyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new QuickConvSyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineQuickConvSyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new QuickConvSyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineQuickConvSyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new QuickConvSyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineQuickConvSyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new QuickConvSyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineQuickConvSyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new QuickConvSyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

{$endregion NonContext}

{$region Context}

function CombineConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>) := new BackgroundConvSyncQueueArrayC<TInp, TRes>(qs.ToArray, conv);
function CombineConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>) := new BackgroundConvSyncQueueArrayC<TInp, TRes>(qs.ToArray, conv);

function CombineConvSyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new BackgroundConvSyncQueueArray2C<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineConvSyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new BackgroundConvSyncQueueArray3C<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineConvSyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new BackgroundConvSyncQueueArray4C<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineConvSyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new BackgroundConvSyncQueueArray5C<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineConvSyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new BackgroundConvSyncQueueArray6C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineConvSyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new BackgroundConvSyncQueueArray7C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>) := new QuickConvSyncQueueArrayC<TInp, TRes>(qs.ToArray, conv);
function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>) := new QuickConvSyncQueueArrayC<TInp, TRes>(qs.ToArray, conv);

function CombineQuickConvSyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new QuickConvSyncQueueArray2C<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineQuickConvSyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new QuickConvSyncQueueArray3C<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineQuickConvSyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new QuickConvSyncQueueArray4C<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineQuickConvSyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new QuickConvSyncQueueArray5C<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineQuickConvSyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new QuickConvSyncQueueArray6C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineQuickConvSyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new QuickConvSyncQueueArray7C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

{$endregion Context}

{$endregion Conv}

{$endregion Sync}

{$region Async}

{$region NonConv}

function CombineAsyncQueueBase(params qs: array of CommandQueueBase) := QueueArrayUtils.ConstructAsync(qs);
function CombineAsyncQueueBase(qs: sequence of CommandQueueBase) := QueueArrayUtils.ConstructAsync(qs);

function CombineAsyncQueueNil(params qs: array of CommandQueueNil) := QueueArrayUtils.ConstructAsyncNil(qs.Cast&<CommandQueueBase>);
function CombineAsyncQueueNil(qs: sequence of CommandQueueNil) := QueueArrayUtils.ConstructAsyncNil(qs.Cast&<CommandQueueBase>);

function CombineAsyncQueue<T>(params qs: array of CommandQueue<T>) := QueueArrayUtils.ConstructAsync&<T>(qs.Cast&<CommandQueueBase>);
function CombineAsyncQueue<T>(qs: sequence of CommandQueue<T>) := QueueArrayUtils.ConstructAsync&<T>(qs.Cast&<CommandQueueBase>);

function CombineAsyncQueueNil(qs: sequence of CommandQueueBase; last: CommandQueueNil) := QueueArrayUtils.ConstructAsyncNil(qs.Append&<CommandQueueBase>(last));

function CombineAsyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>) := QueueArrayUtils.ConstructAsync&<T>(qs.Append&<CommandQueueBase>(last));

{$endregion NonConv}

{$region Conv}

{$region NonContext}

function CombineConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>) := new BackgroundConvAsyncQueueArray<TInp, TRes>(qs.ToArray, conv);
function CombineConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>) := new BackgroundConvAsyncQueueArray<TInp, TRes>(qs.ToArray, conv);

function CombineConvAsyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new BackgroundConvAsyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineConvAsyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new BackgroundConvAsyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineConvAsyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new BackgroundConvAsyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineConvAsyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new BackgroundConvAsyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineConvAsyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new BackgroundConvAsyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineConvAsyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new BackgroundConvAsyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>) := new QuickConvAsyncQueueArray<TInp, TRes>(qs.ToArray, conv);
function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>) := new QuickConvAsyncQueueArray<TInp, TRes>(qs.ToArray, conv);

function CombineQuickConvAsyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new QuickConvAsyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineQuickConvAsyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new QuickConvAsyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineQuickConvAsyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new QuickConvAsyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineQuickConvAsyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new QuickConvAsyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineQuickConvAsyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new QuickConvAsyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineQuickConvAsyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new QuickConvAsyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

{$endregion NonContext}

{$region Context}

function CombineConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>) := new BackgroundConvAsyncQueueArrayC<TInp, TRes>(qs.ToArray, conv);
function CombineConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>) := new BackgroundConvAsyncQueueArrayC<TInp, TRes>(qs.ToArray, conv);

function CombineConvAsyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new BackgroundConvAsyncQueueArray2C<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineConvAsyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new BackgroundConvAsyncQueueArray3C<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineConvAsyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new BackgroundConvAsyncQueueArray4C<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineConvAsyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new BackgroundConvAsyncQueueArray5C<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineConvAsyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new BackgroundConvAsyncQueueArray6C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineConvAsyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new BackgroundConvAsyncQueueArray7C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>) := new QuickConvAsyncQueueArrayC<TInp, TRes>(qs.ToArray, conv);
function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>) := new QuickConvAsyncQueueArrayC<TInp, TRes>(qs.ToArray, conv);

function CombineQuickConvAsyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new QuickConvAsyncQueueArray2C<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineQuickConvAsyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new QuickConvAsyncQueueArray3C<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineQuickConvAsyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new QuickConvAsyncQueueArray4C<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineQuickConvAsyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new QuickConvAsyncQueueArray5C<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineQuickConvAsyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new QuickConvAsyncQueueArray6C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineQuickConvAsyncQueueN7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new QuickConvAsyncQueueArray7C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

{$endregion Context}

{$endregion Conv}

{$endregion Async}

{$endregion CombineQueue's}

{$endregion Global subprograms}

initialization
finalization
  {$ifdef EventDebug}
  EventDebug.AssertDone;
  {$endif EventDebug}
  {$ifdef WaitDebug}
  foreach var whd: WaitHandlerDirect in WaitDebug.WaitActions.Keys.OfType&<WaitHandlerDirect> do
    if whd.reserved<>0 then
      raise new OpenCLABCInternalException($'WaitHandler.reserved in finalization was <>0');
  {$endif WaitDebug}
end.