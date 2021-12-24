
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

//===================================
// Запланированное:

//TODO Параметры с константным результатом тоже надо сувать в ev_l2:
// - k.Exec1(N, a.NewQueue.AddFillValue(1))
// - a.NewQueue.AddFillValue(1) + k.Exec1(N, a)
// --- Иначе сейчас cl.Enqueue не происходит до самого последнего момента, даже для такого простого случая

//TODO MultiusableBase позволяет использовать вне модуля

//TODO CommandQueueBase.UseTyped(typed_q_user: interface procedure use<T>(cq: CommandQueue<T>); procedure use_base(cq: CommandQueueBase); end)
// - Использовать это внутри, чтоб наконец избавится от всех этих .Cast&<object>
// - Пройтись по всеми TODO UseTyped
// - И проверять возможность приведения при создании CastQueue

//TODO Синхронные (с припиской Fast, а может Quick) варианты всего работающего по принципу HostQueue
//TODO Справка: В обработке исключений написать, что обработчики всегда Quick

//TODO .pcu с неправильной позицией зависимости, или не теми настройками - должен игнорироваться
// - Иначе сейчас модули в примерах ссылаются на .pcu, который существует только во время работы Tester, ломая компилятор

//TODO .AddProc(()->p()) сейчас вызывает .AddProc(c->p()), но делает это лямбдой
// - При выводе .ToString выглядит криво - стоит сделать пользовательский класс для этого
// - И наверное интерфейс IDelegatePropagator, чтоб в .ToString выводить только изначальный делегат

//TODO CLValue<T> = class, содержащий указатель на значение
// - Чтоб и .ReadValue работало, и меньше действий проводить на стороне CPU
//TODO В HandleDefaultRes принимать CLValue<T> вместо T

//TODO Пройтись по интерфейсу, порасставлять кидание исключений
//TODO Проверки и кидания исключений перед всеми cl.*, чтобы выводить норм сообщения об ошибках
// - В том числе проверки с помощью BlittableHelper
// - BlittableHelper вроде уже всё проверяет, но проверок надо тучу
//TODO А в самих cl.* вызовах - использовать OpenCLABCInnerException.RaiseIfError, ибо это внутренние проблемы

//TODO Проверять ".IsReadOnly" перед запасным копированием коллекций

//TODO В методах вроде MemorySegment.AddWriteArray1 приходится добавлять &<>

//TODO Может всё же сделать защиту от дурака для "q.AddQueue(q)"?
// - И в справке тогда убрать параграф...

//TODO Использовать cl.EnqueueMapBuffer
// - В виде .AddMap((MappedArray,Context)->())

//TODO Порядок Wait очередей в Wait группах
// - Проверить сочетание с каждой другой фичей

//TODO Перепродумать MemorySubSegment, в случае перевыделения основного буфера - он плохо себя ведёт...
// - Уже не существует никакого перевыделения, память выделяется всего 1 раз, при создании
// - Но стоит всё же кидать исключения, если родительский сегмент удалён

//TODO Создание SubDevice из cl_device_id

//TODO .Cycle(integer)
//TODO .Cycle // бесконечность циклов
//TODO .CycleWhile(***->boolean)
// - Возможность передать свой обработчик ошибок как Exception->Exception
//TODO В продолжение Cycle: Однако всё ещё остаётся проблема - как сделать ветвление?
// - И если уже делать - стоит сделать и метод CQ.ThenIf(res->boolean; if_true, if_false: CQ)
//TODO И ещё - AbortQueue, который, по сути, может использоваться как exit, continue или break, если с обработчиками ошибок
// - Или может метод MarkerQueue.Abort?
//
//TODO Несколько TODO в:
// - Queue converter's >> Wait

//TODO Интегрировать профайлинг очередей

//TODO Исправить перегрузки Kernel.Exec

//TODO Проверить, будет ли оптимизацией, создавать свой ThreadPool для каждого CLTaskBase
// - (HPQ+HPQ).Handle.Handle, тут создаётся 4 UserEvent, хотя всё можно было бы выполнять синхронно

//TODO Тестировщик должен запускать отдельные .exe для тестирования, а не вот это вот всё

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
    private constructor(message: string; ec: ErrorCode) :=
    inherited Create($'{message} with {ec}');
    private constructor;
    begin
      inherited Create($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
      raise self;
    end;
    
    private static procedure RaiseIfError(message: string; ec: ErrorCode) :=
    if ec.IS_ERROR then raise new OpenCLABCInternalException(message, ec);
    
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
      EventDebug.CheckExists(ev);
      RefCounterFor(ev).Enqueue(new EventRetainReleaseData(true, reason));
    end;
    
    public static procedure ReportRefCounterInfo(otp: System.IO.TextWriter := Console.Out);
    begin
      
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
    public static procedure CheckExists(ev: cl_event) :=
    if CountRetains(ev)<=0 then
    begin
      ReportRefCounterInfo(Console.Error);
      raise new OpenCLABCInternalException($'Event {ev} was released before last use at');
    end;
    
    public static procedure AssertDone :=
    foreach var ev in RefCounter.Keys do if CountRetains(ev)<>0 then
    begin
      ReportRefCounterInfo(Console.Error);
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
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
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
        cl.GetPlatformIDs(0, IntPtr.Zero, c).RaiseIfError;
        
        if c<>0 then
        begin
          var all_arr := new cl_platform_id[c];
          cl.GetPlatformIDs(c, all_arr[0], IntPtr.Zero).RaiseIfError;
          
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
    
    ///Создаёт обёртку для указанного неуправляемого объекта
    public constructor(ntv: cl_device_id) := self.ntv := ntv;
    private constructor := raise new OpenCLABCInternalException;
    
    private function GetBasePlatform: Platform;
    begin
      var pl: cl_platform_id;
      cl.GetDeviceInfo(self.ntv, DeviceInfo.DEVICE_PLATFORM, new UIntPtr(sizeof(cl_platform_id)), pl, IntPtr.Zero).RaiseIfError;
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
      ec.RaiseIfError;
      
      var all := new cl_device_id[c];
      cl.GetDeviceIDs(pl.ntv, t, c, all[0], IntPtr.Zero).RaiseIfError;
      
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
    private _parent: Device;
    ///Возвращает родительское устройство, часть ядер которого использует данное устройство
    public property Parent: Device read _parent;
    
    private constructor(dvc: cl_device_id; parent: Device);
    begin
      inherited Create(dvc);
      self._parent := parent;
    end;
    private constructor := inherited;
    
    ///Освобождает неуправляемые ресурсы. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override :=
    cl.ReleaseDevice(ntv).RaiseIfError;
    
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
      ec.RaiseIfError;
      
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
      cl.ReleaseContext(new cl_context(prev)).RaiseIfError;
    end;
    ///Освобождает неуправляемые ресурсы. Данный метод вызывается автоматически во время сборки мусора
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
          cl.GetProgramBuildInfo(self.ntv, dvc.ntv, ProgramBuildInfo.PROGRAM_BUILD_LOG, UIntPtr.Zero,IntPtr.Zero,sz).RaiseIfError;
          
          var str_ptr := Marshal.AllocHGlobal(IntPtr(pointer(sz)));
          try
            cl.GetProgramBuildInfo(self.ntv, dvc.ntv, ProgramBuildInfo.PROGRAM_BUILD_LOG, sz,str_ptr,IntPtr.Zero).RaiseIfError;
            sb += Marshal.PtrToStringAnsi(str_ptr);
          finally
            Marshal.FreeHGlobal(str_ptr);
          end;
          
        end;
        
        raise new OpenCLException(ec, sb.ToString);
      end else
        ec.RaiseIfError;
      
    end;
    
    ///Компилирует указанные тексты программ в указанном контексте
    ///Внимание! Именно тексты, Не имена файлов
    public constructor(c: Context; params file_texts: array of string);
    begin
      
      var ec: ErrorCode;
      self.ntv := cl.CreateProgramWithSource(c.ntv, file_texts.Length, file_texts, nil, ec);
      ec.RaiseIfError;
      
      self._c := c;
      self.Build;
    end;
    ///Компилирует указанные тексты программ в контексте Context.Default
    ///Внимание! Именно тексты, Не имена файлов
    public constructor(params file_texts: array of string) := Create(Context.Default, file_texts);
    
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
    
    private constructor := raise new OpenCLABCInternalException;
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose;
    begin
      var prev := Interlocked.Exchange(self.ntv.val, IntPtr.Zero);
      if prev=IntPtr.Zero then exit;
      cl.ReleaseProgram(new cl_program(prev)).RaiseIfError;
    end;
    ///Освобождает неуправляемые ресурсы. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override := Dispose;
    
    {$endregion constructor's}
    
    {$region Serialize}
    
    ///Сохраняет прекомпилированную программу как набор байт
    public function Serialize: array of array of byte;
    begin
      var sz: UIntPtr;
      
      cl.GetProgramInfo(ntv, ProgramInfo.PROGRAM_BINARY_SIZES, UIntPtr.Zero, nil, sz).RaiseIfError;
      var szs := new UIntPtr[sz.ToUInt64 div sizeof(UIntPtr)];
      cl.GetProgramInfo(ntv, ProgramInfo.PROGRAM_BINARY_SIZES, sz, szs[0], IntPtr.Zero).RaiseIfError;
      
      var res := new IntPtr[szs.Length];
      SetLength(Result, szs.Length);
      
      for var i := 0 to szs.Length-1 do res[i] := Marshal.AllocHGlobal(IntPtr(pointer(szs[i])));
      try
        cl.GetProgramInfo(ntv, ProgramInfo.PROGRAM_BINARIES, sz, res[0], IntPtr.Zero).RaiseIfError;
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
      ec.RaiseIfError;
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
      cl.GetKernelInfo(ntv, KernelInfo.KERNEL_PROGRAM, new UIntPtr(cl_program.Size), code_ntv, IntPtr.Zero).RaiseIfError;
      self.code := new ProgramCode(code_ntv);
      
      var sz: UIntPtr;
      cl.GetKernelInfo(ntv, KernelInfo.KERNEL_FUNCTION_NAME, UIntPtr.Zero, nil, sz).RaiseIfError;
      var str_ptr := Marshal.AllocHGlobal(IntPtr(pointer(sz)));
      try
        cl.GetKernelInfo(ntv, KernelInfo.KERNEL_FUNCTION_NAME, sz, str_ptr, IntPtr.Zero).RaiseIfError;
        self.k_name := Marshal.PtrToStringAnsi(str_ptr);
      finally
        Marshal.FreeHGlobal(str_ptr);
      end;
      
      if retain then cl.RetainKernel(ntv).RaiseIfError;
      self.ntv := ntv;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose;
    begin
      var prev := Interlocked.Exchange(self.ntv.val, IntPtr.Zero);
      if prev=IntPtr.Zero then exit;
      cl.ReleaseKernel(new cl_kernel(prev)).RaiseIfError;
    end;
    ///Освобождает неуправляемые ресурсы. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override := Dispose;
    
    {$endregion constructor's}
    
    {$region UseExclusiveNative}
    
    private ntv_in_use := 0;
    ///Гарантирует что неуправляемый объект будет использоваться только в 1 потоке одновременно
    ///Если неуправляемый объект данного kernel-а используется другим потоком - в процедурную переменную передаётся его независимый клон
    ///Внимание: Клон неуправляемого объекта будет удалён сразу после выхода из вашей процедурной переменной, если не вызвать cl.RetainKernel
    protected procedure UseExclusiveNative(p: cl_kernel->()) :=
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
        cl.ReleaseKernel(k).RaiseIfError;
      end;
    end;
    ///Гарантирует что неуправляемый объект будет использоваться только в 1 потоке одновременно
    ///Если неуправляемый объект данного kernel-а используется другим потоком - в процедурную переменную передаётся его независимый клон
    ///Внимание: Клон неуправляемого объекта будет удалён сразу после выхода из вашей процедурной переменной, если не вызвать cl.RetainKernel
    protected function UseExclusiveNative<T>(f: cl_kernel->T): T;
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
          cl.ReleaseKernel(k).RaiseIfError;
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
      cl.CreateKernelsInProgram(ntv, 0, IntPtr.Zero, c).RaiseIfError;
      
      var res := new cl_kernel[c];
      cl.CreateKernelsInProgram(ntv, c, res[0], IntPtr.Zero).RaiseIfError;
      
      Result := res.ConvertAll(k->new Kernel(k, false));
    end;
    
  end;
  
  {$endregion Kernel}
  
  {$region MemorySegment}
  
  ///Представляет область памяти устройства OpenCL (обычно GPU)
  MemorySegment = partial class
    private ntv: cl_mem;
    
    private sz: UIntPtr;
    ///Возвращает размер области памяти в байтах
    public property Size: UIntPtr read sz;
    ///Возвращает размер области памяти в байтах
    public property Size32: UInt32 read sz.ToUInt32;
    ///Возвращает размер области памяти в байтах
    public property Size64: UInt64 read sz.ToUInt64;
    
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
      ec.RaiseIfError;
      
      GC.AddMemoryPressure(size.ToUInt64);
      
      self.sz := size;
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
    
    private constructor(ntv: cl_mem; sz: UIntPtr);
    begin
      self.sz := sz;
      self.ntv := ntv;
    end;
    private static function GetMemSize(ntv: cl_mem): UIntPtr;
    begin
      cl.GetMemObjectInfo(ntv, MemInfo.MEM_SIZE, new UIntPtr(Marshal.SizeOf&<UIntPtr>), Result, IntPtr.Zero).RaiseIfError;
    end;
    ///Создаёт обёртку для указанного неуправляемого объекта
    ///При успешном создании обёртки вызывается cl.Retain
    ///А во время вызова .Dispose - cl.Release
    public constructor(ntv: cl_mem);
    begin
      Create(ntv, GetMemSize(ntv));
      cl.RetainMemObject(ntv).RaiseIfError;
      GC.AddMemoryPressure(Size64);
    end;
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
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function WriteValue<TRecord>(val: TRecord; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function WriteValue<TRecord>(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
    
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
    
    ///Выделяет область неуправляемой памяти и копирует в неё всё содержимое данной области памяти
    public function GetData: IntPtr;
    
    ///Выделяет область неуправляемой памяти и копирует в неё часть содержимого данной области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function GetData(mem_offset, len: CommandQueue<integer>): IntPtr;
    
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
    
  end;
  
  {$endregion MemorySegment}
  
  {$region MemorySubSegment}
  
  ///Представляет виртуальную область памяти, выделенную внутри MemorySegment
  MemorySubSegment = partial class(MemorySegment)
    
    private _parent: MemorySegment;
    ///Возвращает родительскую область памяти
    public property Parent: MemorySegment read _parent;
    
    ///Возвращает строку с основными данными о данном объекте
    public function ToString: string; override :=
    $'{inherited ToString} inside {Parent}';
    
    {$region constructor's}
    
    private static function MakeSubNtv(ntv: cl_mem; reg: cl_buffer_region): cl_mem;
    begin
      var ec: ErrorCode;
      Result := cl.CreateSubBuffer(ntv, MemFlags.MEM_READ_WRITE, BufferCreateType.BUFFER_CREATE_TYPE_REGION, reg, ec);
      ec.RaiseIfError;
    end;
    private constructor(parent: MemorySegment; reg: cl_buffer_region);
    begin
      inherited Create(MakeSubNtv(parent.ntv, reg), reg.size);
      self._parent := parent;
    end;
    ///Создаёт виртуальную область памяти, использующую указанную область из parent
    ///origin указывает отступ в байтах от начала parent
    ///size указывает размер новой области памяти
    public constructor(parent: MemorySegment; origin, size: UIntPtr) := Create(parent, new cl_buffer_region(origin, size));
    
    ///Создаёт виртуальную область памяти, использующую указанную область из parent
    ///origin указывает отступ в байтах от начала parent
    ///size указывает размер новой области памяти
    public constructor(parent: MemorySegment; origin, size: UInt32) := Create(parent, new UIntPtr(origin), new UIntPtr(size));
    ///Создаёт виртуальную область памяти, использующую указанную область из parent
    ///origin указывает отступ в байтах от начала parent
    ///size указывает размер новой области памяти
    public constructor(parent: MemorySegment; origin, size: UInt64) := Create(parent, new UIntPtr(origin), new UIntPtr(size));
    
    {$endregion constructor's}
    
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
      ec.RaiseIfError;
      
      GC.AddMemoryPressure(ByteSize);
    end;
    private procedure InitByVal(c: Context; var els: T);
    begin
      
      var ec: ErrorCode;
      self.ntv := cl.CreateBuffer(c.ntv, MemFlags.MEM_READ_WRITE + MemFlags.MEM_COPY_HOST_PTR, new UIntPtr(ByteSize), els, ec);
      ec.RaiseIfError;
      
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
      cl.GetMemObjectInfo(ntv, MemInfo.MEM_SIZE, new UIntPtr(Marshal.SizeOf&<UIntPtr>), byte_size, IntPtr.Zero).RaiseIfError;
      
      self.len := byte_size.ToUInt64 div Marshal.SizeOf&<T>;
      self.ntv := ntv;
      
      cl.RetainMemObject(ntv).RaiseIfError;
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
      cl.CreateSubDevices(self.ntv, props, 0, IntPtr.Zero, c).RaiseIfError;
      
      var res := new cl_device_id[int64(c)];
      cl.CreateSubDevices(self.ntv, props, c, res[0], IntPtr.Zero).RaiseIfError;
      
      Result := res.ConvertAll(sdvc->new SubDevice(sdvc, self));
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
  
  ///Представляет область памяти устройства OpenCL (обычно GPU)
  MemorySegment = partial class
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose; virtual;
    begin
      var prev := Interlocked.Exchange(self.ntv.val, IntPtr.Zero);
      if prev=IntPtr.Zero then exit;
      GC.RemoveMemoryPressure(Size64);
      cl.ReleaseMemObject(new cl_mem(prev)).RaiseIfError;
    end;
    ///Освобождает неуправляемые ресурсы. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override := Dispose;
    
  end;
  
  ///Представляет виртуальную область памяти, выделенную внутри MemorySegment
  MemorySubSegment = partial class
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose; override;
    begin
      var prev := Interlocked.Exchange(self.ntv.val, IntPtr.Zero);
      if prev=IntPtr.Zero then exit;
      cl.ReleaseMemObject(new cl_mem(prev)).RaiseIfError;
    end;
    
  end;
  
  ///Представляет массив записей, содержимое которого хранится на устройстве OpenCL (обычно GPU)
  CLArray<T> = partial class
    
    ///Позволяет OpenCL удалить неуправляемый объект
    ///Данный метод вызывается автоматически во время сборки мусора, если объект ещё не удалён
    public procedure Dispose;
    begin
      var prev := Interlocked.Exchange(self.ntv.val, IntPtr.Zero);
      if prev=IntPtr.Zero then exit;
      GC.RemoveMemoryPressure(ByteSize);
      cl.ReleaseMemObject(new cl_mem(prev)).RaiseIfError;
    end;
    ///Освобождает неуправляемые ресурсы. Данный метод вызывается автоматически во время сборки мусора
    ///Данный метод не должен вызываться из пользовательского кода. Он виден только на случай если вы хотите переопределить его в своём классе-наследнике
    protected procedure Finalize; override := Dispose;
    
  end;
  
  {$endregion Misc}
  
  {$endregion Wrappers}
  
  {$region CommandQueue}
  
  {$region ToString}
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
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
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); abstract;
    
    private static function GetValueRuntimeType<T>(val: T) :=
    if typeof(T).IsValueType then
      typeof(T) else
    if val = default(T) then
      nil else val.GetType;
    
    private function ToStringHeader(sb: StringBuilder; index: Dictionary<CommandQueueBase,integer>): boolean;
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
    private static procedure ToStringWriteDelegate(sb: StringBuilder; d: System.Delegate) :=
    sb += $'{d.Target} => {d.Method}';
    private procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>; write_tabs: boolean := true);
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
    
    ///Возвращает строковое представление данной очереди
    ///Используйте это значение только для отладки, потому что данный метод довольно медленный
    public function ToString: string; override;
    begin
      var sb := new StringBuilder;
      ToString(sb, 0, new Dictionary<CommandQueueBase, integer>, new HashSet<CommandQueueBase>);
      Result := sb.ToString;
    end;
    
    ///Вызывает Write(ToString) для данной очереди и возвращает её же
    public function Print: CommandQueueBase;
    begin
      Write(self.ToString);
      Result := self;
    end;
    ///Вызывает Writeln(ToString) для данной очереди и возвращает её же
    public function Println: CommandQueueBase;
    begin
      Writeln(self.ToString);
      Result := self;
    end;
    
  end;
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueue<T> = abstract partial class(CommandQueueBase) end;
  
  {$endregion ToString}
  
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueue<T> = abstract partial class
    
    public static function operator implicit(o: T): CommandQueue<T> :=
    new ConstQueue<T>(o);
    
  end;
  
  {$endregion Const}
  
  {$region Cast}
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    ///Если данная очередь проходит по условию "... is CommandQueue<T>" - возвращает себя же
    ///Иначе возвращает очередь-обёртку, выполняющую "res := T(res)", где res - результат данной очереди
    public function Cast<T>: CommandQueue<T>;
    
  end;
  
  {$endregion Cast}
  
  {$region ThenConvert}
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueueBase = abstract partial class
    
  end;
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди
    public function ThenConvert<TOtp>(f: T->TOtp): CommandQueue<TOtp> := ThenConvert((o,c)->f(o));
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди и контекст выполнения
    public function ThenConvert<TOtp>(f: (T, Context)->TOtp): CommandQueue<TOtp>;
    
    ///Создаёт очередь, которая выполнит данную и вернёт её результат
    ///Но перед этим выполнит на CPU процедуру p, используя полученный результат
    public function ThenUse(p: T->()           ) := ThenConvert( o   ->begin p(o  ); Result := o; end);
    ///Создаёт очередь, которая выполнит данную и вернёт её результат
    ///Но перед этим выполнит на CPU процедуру p, используя полученный результат и контекст выполнения
    public function ThenUse(p: (T, Context)->()) := ThenConvert((o,c)->begin p(o,c); Result := o; end);
    
  end;
  
  {$endregion ThenConvert}
  
  {$region +/*}
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    private function AfterQueueSyncBase(q: CommandQueueBase): CommandQueueBase; virtual;
    private function AfterQueueAsyncBase(q: CommandQueueBase): CommandQueueBase; virtual;
    
    public static function operator+(q1, q2: CommandQueueBase): CommandQueueBase := q2.AfterQueueSyncBase(q1);
    public static function operator*(q1, q2: CommandQueueBase): CommandQueueBase := q2.AfterQueueAsyncBase(q1);
    
    public static procedure operator+=(var q1: CommandQueueBase; q2: CommandQueueBase) := q1 := q1+q2;
    public static procedure operator*=(var q1: CommandQueueBase; q2: CommandQueueBase) := q1 := q1*q2;
    
  end;
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
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
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    private function MultiusableBase: ()->CommandQueueBase; virtual;
    
    ///Создаёт функцию, вызывая которую можно создать любое кол-во очередей-удлинителей для данной очереди
    ///Подробнее в справке: "Очередь>>Создание очередей>>Множественное использование очереди"
    public function Multiusable := MultiusableBase;
    
  end;
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    private function MultiusableBase: ()->CommandQueueBase; override := Multiusable() as object as Func<CommandQueueBase>; //TODO #2221
    
    ///Создаёт функцию, вызывая которую можно создать любое кол-во очередей-удлинителей для данной очереди
    ///Подробнее в справке: "Очередь>>Создание очередей>>Множественное использование очереди"
    public function Multiusable: ()->CommandQueue<T>;
    
  end;
  
  {$endregion Multiusable}
  
  {$region Finally+Handle}
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    private function AfterTry(try_do: CommandQueueBase): CommandQueueBase; abstract;
    public static function operator>=(try_do, do_finally: CommandQueueBase) := do_finally.AfterTry(try_do);
    
    private function ConvertErrHandler<TException>(handler: TException->boolean): Exception->boolean; where TException: Exception;
    begin Result := e->(e is TException) and handler(TException(e)) end;
    
    ///Создаёт очередь, сначала выполняющую данную, а затем обрабатывающую кинутые в ней исключения
    ///Созданная очередь возвращает nil не зависимо от исключений при выполнении данной очереди
    public function HandleWithoutRes<TException>(handler: TException->boolean): CommandQueueBase; where TException: Exception;
    begin Result := HandleWithoutRes(ConvertErrHandler(handler)) end;
    ///Создаёт очередь, сначала выполняющую данную, а затем обрабатывающую кинутые в ней исключения
    ///Созданная очередь возвращает nil не зависимо от исключений при выполнении данной очереди
    public function HandleWithoutRes(handler: Exception->boolean): CommandQueueBase;
    
  end;
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
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
  ///При выполнении возвращает nil
  WaitMarker = abstract partial class(CommandQueueBase)
    
    ///Создаёт новый простой маркер
    public static function Create: WaitMarker;
    
    ///Посылает сигнал выполненности всем ожидающим Wait очередям
    public procedure SendSignal; abstract;
    
    public static function operator and(m1, m2: WaitMarker): WaitMarker;
    
    public static function operator or(m1, m2: WaitMarker): WaitMarker;
    
  end;
  
  ///Представляет оторванный сигнал маркера, являющийся обёрткой очереди с возвращаемым значением
  ///Этот тип является наследником CommandQueue<T>, а значит он не может наследовать сразу и от WaitMarker
  ///Но при присвоении или передаче параметром он разлагается в обычный маркер, поэтому его можно передавать в Wait очереди
  DetachedMarkerSignal<T> = sealed partial class(CommandQueue<T>)
    private q: CommandQueue<T>;
    private wrap: WaitMarker;
    private signal_in_finally: boolean;
    
    ///Указывает, будут ли проигнорированы ошибки выполнения q при автоматическом вызове .SendSignal
    public property SignalInFinally: boolean read signal_in_finally;
    
    ///Создаёт новый оторванный сигнал маркера
    ///При выполнении сначала будет выполнена очередь q, а затем метод .SendSignal
    ///signal_in_finally указывает, будут ли проигнорированы ошибки выполнения q при автоматическом вызове .SendSignal
    public constructor(q: CommandQueue<T>; signal_in_finally: boolean);
    private constructor := raise new OpenCLABCInternalException;
    
    public static function operator implicit(dms: DetachedMarkerSignal<T>): WaitMarker := dms.wrap;
    
    public static function operator and(m1, m2: DetachedMarkerSignal<T>) := WaitMarker(m1) and WaitMarker(m2);
    public static function operator or(m1, m2: DetachedMarkerSignal<T>) := WaitMarker(m1) or WaitMarker(m2);
    
    ///Посылает сигнал выполненности всем ожидающим Wait очередям
    public procedure SendSignal := wrap.SendSignal;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      wrap.ToStringHeader(sb, index);
      sb += #10;
      
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    private function ThenMarkerSignalBase: WaitMarker; abstract;
    ///Создаёт особый маркер из данной очереди
    ///При выполнении он сначала выполняет данную очередь, а затем вызывает свой .SendSignal
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenMarkerSignal := ThenMarkerSignalBase;
    
    private function ThenFinallyMarkerSignalBase: WaitMarker; abstract;
    ///Создаёт особый маркер из данной очереди
    ///При выполнении он сначала выполняет данную очередь, а затем вызывает свой .SendSignal не зависимо от исключений при выполнении данной очереди
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenFinallyMarkerSignal := ThenFinallyMarkerSignalBase;
    
    
    
    private function ThenWaitForBase(marker: WaitMarker): CommandQueueBase; abstract;
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала от указанного маркера
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenWaitFor(marker: WaitMarker) := ThenWaitForBase(marker);
    
    private function ThenFinallyWaitForBase(marker: WaitMarker): CommandQueueBase; abstract;
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала от указанного маркера не зависимо от исключений при выполнении данной очереди
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenFinallyWaitFor(marker: WaitMarker) := ThenFinallyWaitForBase(marker);
    
  end;
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    private function ThenMarkerSignalBase: WaitMarker; override := ThenMarkerSignal;
    ///Создаёт очередь, сначала выполняющую данную, а затем вызывающую свой .SendSignal
    ///При передаче в Wait-очереди, DetachedMarkerSignal превращается в маркер
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenMarkerSignal := new DetachedMarkerSignal<T>(self, false);
    
    private function ThenFinallyMarkerSignalBase: WaitMarker; override := ThenFinallyMarkerSignal;
    ///Создаёт очередь, сначала выполняющую данную, а затем вызывающую свой .SendSignal не зависимо от исключений при выполнении данной очереди
    ///При передаче в Wait-очереди, DetachedMarkerSignal превращается в маркер
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenFinallyMarkerSignal := new DetachedMarkerSignal<T>(self, true);
    
    
    
    private function ThenWaitForBase(marker: WaitMarker): CommandQueueBase; override := ThenWaitFor(marker);
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала от указанного маркера
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenWaitFor(marker: WaitMarker): CommandQueue<T>;
    
    private function ThenFinallyWaitForBase(marker: WaitMarker): CommandQueueBase; override := ThenFinallyWaitFor(marker);
    ///Создаёт очередь, сначала выполняющую данную, а затем ожидающую сигнала от указанного маркера не зависимо от исключений при выполнении данной очереди
    ///В конце выполнения созданная очередь возвращает то, что вернула данная
    public function ThenFinallyWaitFor(marker: WaitMarker): CommandQueue<T>;
    
  end;
  
  {$endregion Wait}
  
  {$endregion CommandQueue}
  
  {$region CLTask}
  
  ///Представляет задачу выполнения очереди, создаваемую методом Context.BeginInvoke
  CLTaskBase = abstract partial class
    protected wh := new ManualResetEvent(false);
    private err_lst: List<Exception>;
    
    {$region Property's}
    
    private function OrgQueueBase: CommandQueueBase; abstract;
    ///Возвращает очередь, которую выполняет данный CLTask
    public property OrgQueue: CommandQueueBase read OrgQueueBase;
    
    private org_c: Context;
    ///Возвращает контекст, в котором выполняется данный CLTask
    public property OrgContext: Context read org_c;
    
    {$endregion Property's}
    
    {$region Wait}
    
    ///Ожидает окончания выполнения очереди (если оно ещё не завершилось)
    ///Вызывает исключение, если оно было вызвано при выполнении очереди
    public procedure Wait;
    begin
      wh.WaitOne;
      if err_lst.Count=0 then exit;
      raise new AggregateException($'При выполнении очереди было вызвано {err_lst.Count} исключений. Используйте try чтоб получить больше информации', err_lst.ToArray);
    end;
    
    {$endregion Wait}
    
  end;
  
  ///Представляет задачу выполнения очереди, создаваемую методом Context.BeginInvoke
  CLTask<T> = sealed partial class(CLTaskBase)
    private q: CommandQueue<T>;
    
    private constructor := raise new OpenCLABCInternalException;
    
    {$region Property's}
    
    ///Возвращает очередь, которую выполняет данный CLTask
    public property OrgQueue: CommandQueue<T> read q; reintroduce;
    protected function OrgQueueBase: CommandQueueBase; override := self.OrgQueue;
    
    {$endregion Property's}
    
    {$region Wait}
    
    ///Ожидает окончания выполнения очереди (если оно ещё не завершилось)
    ///Вызывает исключение, если оно было вызвано при выполнении очереди
    ///А затем возвращает результат выполнения
    public function WaitRes: T; reintroduce;
    
    {$endregion Wait}
    
  end;
  
  ///Представляет контекст для хранения данных и выполнения команд на GPU
  Context = partial class
    
    ///Запускает данную очередь и все её подочереди
    ///Как только всё запущено: возвращает объект типа CLTask<>, через который можно следить за процессом выполнения
    public function BeginInvoke<T>(q: CommandQueue<T>): CLTask<T>;
    ///Запускает данную очередь и все её подочереди
    ///Как только всё запущено: возвращает объект типа CLTask<>, через который можно следить за процессом выполнения
    public function BeginInvoke(q: CommandQueueBase): CLTaskBase;
    
    ///Запускает данную очередь и все её подочереди
    ///Затем ожидает окончания выполнения и возвращает полученный результат
    public function SyncInvoke<T>(q: CommandQueue<T>) := BeginInvoke(q).WaitRes;
    ///Запускает данную очередь и все её подочереди
    ///Затем ожидает окончания выполнения и возвращает полученный результат
    public procedure SyncInvoke(q: CommandQueueBase) := BeginInvoke(q).Wait;
    
  end;
  
  {$endregion CLTask}
  
  {$region KernelArg}
  
  ///Представляет аргумент, передаваемый в вызов kernel-а
  KernelArg = abstract partial class
    
    {$region MemorySegment}
    
    ///Создаёт аргумент kernel-а, представляющий область памяти GPU
    public static function FromMemorySegment(mem: MemorySegment): KernelArg;
    public static function operator implicit(mem: MemorySegment): KernelArg := FromMemorySegment(mem);
    
    ///Создаёт аргумент kernel-а, представляющий область памяти GPU
    public static function FromMemorySegmentCQ(mem_q: CommandQueue<MemorySegment>): KernelArg;
    public static function operator implicit(mem_q: CommandQueue<MemorySegment>): KernelArg := FromMemorySegmentCQ(mem_q);
    
    {$endregion MemorySegment}
    
    {$region CLArray}
    
    ///Создаёт агрумент kernel-а, представляющий массив данных, хранимых на GPU
    public static function FromCLArray<T>(a: CLArray<T>): KernelArg; where T: record;
    public static function operator implicit<T>(a: CLArray<T>): KernelArg; where T: record;
    begin Result := FromCLArray(a); end;
    
    ///Создаёт агрумент kernel-а, представляющий массив данных, хранимых на GPU
    public static function FromCLArrayCQ<T>(a_q: CommandQueue<CLArray<T>>): KernelArg; where T: record;
    public static function operator implicit<T>(a_q: CommandQueue<CLArray<T>>): KernelArg; where T: record;
    begin Result := FromCLArrayCQ(a_q); end;
    
    {$endregion CLArray}
    
    {$region Data}
    
    ///Создаёт аргумент kernel-а, представляющий адрес в неуправляемой памяти или на стэке
    public static function FromData(ptr: IntPtr; sz: UIntPtr): KernelArg;
    
    ///Создаёт аргумент kernel-а, представляющий адрес в неуправляемой памяти или на стэке
    public static function FromDataCQ(ptr_q: CommandQueue<IntPtr>; sz_q: CommandQueue<UIntPtr>): KernelArg;
    
    ///Создаёт аргумент kernel-а, представляющий адрес в неуправляемой памяти или на стэке
    public static function FromValueData<TRecord>(ptr: ^TRecord): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(ptr: ^TRecord): KernelArg; where TRecord: record; begin Result := FromValueData(ptr); end;
    
    {$endregion Data}
    
    {$region Value}
    
    ///Создаёт аргумент kernel-а, представляющий небольшое значение размерного типа
    public static function FromValue<TRecord>(val: TRecord): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(val: TRecord): KernelArg; where TRecord: record; begin Result := FromValue(val); end;
    
    ///Создаёт аргумент kernel-а, представляющий небольшое значение размерного типа
    public static function FromValueCQ<TRecord>(valq: CommandQueue<TRecord>): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(valq: CommandQueue<TRecord>): KernelArg; where TRecord: record; begin Result := FromValueCQ(valq); end;
    
    {$endregion Value}
    
    {$region Array}
    
    ///Создаёт аргумент kernel-а, ссылающийся на указанный массив, на элемент с индексом ind
    public static function FromArray<TRecord>(a: array of TRecord; ind: integer := 0): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(a: array of TRecord): KernelArg; where TRecord: record; begin Result := FromArray(a); end;
    
    ///Создаёт аргумент kernel-а, ссылающийся на указанный массив, на элемент с индексом ind
    public static function FromArrayCQ<TRecord>(a_q: CommandQueue<array of TRecord>; ind_q: CommandQueue<integer> := 0): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(a_q: CommandQueue<array of TRecord>): KernelArg; where TRecord: record; begin Result := FromArrayCQ(a_q); end;
    
    {$endregion Array}
    
    {$region ToString}
    
    private function DisplayName: string; virtual := CommandQueueBase.DisplayNameForType(self.GetType);
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); abstract;
    
    private procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>; write_tabs: boolean := true);
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
    
    ///Возвращает строковое представление данного объекта KernelArg
    ///Используйте это значение только для отладки, потому что данный метод довольно медленный
    public function ToString: string; override;
    begin
      var sb := new StringBuilder;
      ToString(sb, 0, new Dictionary<CommandQueueBase, integer>, new HashSet<CommandQueueBase>);
      Result := sb.ToString;
    end;
    
    ///Вызывает Write(ToString) для данного объекта KernelArg и возвращает его же
    public function Print: KernelArg;
    begin
      Write(self.ToString);
      Result := self;
    end;
    ///Вызывает Writeln(ToString) для данного объекта KernelArg и возвращает его же
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
    public function AddProc(p: Kernel->()): KernelCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    public function AddProc(p: (Kernel, Context)->()): KernelCCQ;
    
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
    public function AddProc(p: MemorySegment->()): MemorySegmentCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    public function AddProc(p: (MemorySegment, Context)->()): MemorySegmentCCQ;
    
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
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddWriteValue<TRecord>(val: TRecord; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function AddWriteValue<TRecord>(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
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
    
    ///Выделяет область неуправляемой памяти и копирует в неё всё содержимое данной области памяти
    public function AddGetData: CommandQueue<IntPtr>;
    
    ///Выделяет область неуправляемой памяти и копирует в неё часть содержимого данной области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function AddGetData(mem_offset, len: CommandQueue<integer>): CommandQueue<IntPtr>;
    
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
    public function AddProc(p: CLArray<T>->()): CLArrayCCQ<T>;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    public function AddProc(p: (CLArray<T>, Context)->()): CLArrayCCQ<T>;
    
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
function HFQ<T>(f: ()->T): CommandQueue<T>;
///Создаёт очередь, выполняющую указанную функцию на CPU
///И возвращающую результат этой функци
function HFQ<T>(f: Context->T): CommandQueue<T>;

///Создаёт очередь, выполняющую указанную процедуру на CPU
///И возвращающую nil
function HPQ(p: ()->()): CommandQueueBase;
///Создаёт очередь, выполняющую указанную процедуру на CPU
///И возвращающую nil
function HPQ(p: Context->()): CommandQueueBase;

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
function WaitFor(marker: WaitMarker): CommandQueueBase;

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
function CombineSyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>): CommandQueue<T>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueue<T>(params qs: array of CommandQueue<T>): CommandQueue<T>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueue<T>(qs: sequence of CommandQueue<T>): CommandQueue<T>;

{$endregion NonConv}

{$region Conv}

{$region NonContext}

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

{$endregion NonContext}

{$region Context}

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

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
function CombineAsyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>): CommandQueue<T>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueue<T>(params qs: array of CommandQueue<T>): CommandQueue<T>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueue<T>(qs: sequence of CommandQueue<T>): CommandQueue<T>;

{$endregion NonConv}

{$region Conv}

{$region NonContext}

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

{$endregion NonContext}

{$region Context}

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

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
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: PlatformInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
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
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: DeviceInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
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
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: ContextInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
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
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: ProgramInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
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
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: KernelInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
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
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: MemInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
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
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: MemInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
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
    clGetSize(ntv, id, UIntPtr.Zero, IntPtr.Zero, sz).RaiseIfError;
    protected procedure GetValImpl(id: MemInfo; sz: UIntPtr; var res: byte); override :=
    clGetVal(ntv, id, sz, res, IntPtr.Zero).RaiseIfError;
    
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
    
    public static function GCHndAlloc(o: object) :=
    CopyToUnm(GCHandle.Alloc(o));
    
    public static procedure GCHndFree(gc_hnd_ptr: IntPtr);
    begin
      AsPtr&<GCHandle>(gc_hnd_ptr)^.Free;
      Marshal.FreeHGlobal(gc_hnd_ptr);
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
    protected static AbortStatus := new CommandExecutionStatus(integer.MinValue);
    
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
    
    //TODO Заменить на OpenCLABCInternalException.RaiseIfError
    protected function AddErr(ec: ErrorCode): boolean;
    begin
      if not ec.IS_ERROR then exit;
      AddErr(new OpenCLException(ec, $'Внутренняя ошибка OpenCLABC: {ec}{#10}{Environment.StackTrace}'));
      Result := true;
    end;
    
    protected function AddErr(st: CommandExecutionStatus) :=
    (st=AbortStatus) or (st.IS_ERROR and AddErr(ErrorCode(st)));
    
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
          ec.RaiseIfError;
        end;
      end;
      
      curr_inv_cq := if async_enqueue then cl_command_queue.Zero else Result;
    end;
    
  end;
  
{$endregion CLTaskData}

{$region EventList}

type
  EventList = record
    public evs: array of cl_event := nil;
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
    if count<>0 then self.evs := new cl_event[count];
    public constructor := raise new OpenCLABCInternalException;
    public static Empty := new EventList(0);
    
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
    
    private static function Combine(evs: IList<EventList>): EventList;
    begin
      Result := EventList.Empty;
      var count := 0;
      
      for var i := 0 to evs.Count-1 do
        count += evs[i].count;
      if count=0 then exit;
      
      Result := new EventList(count);
      for var i := 0 to evs.Count-1 do
        Result += evs[i];
      
    end;
    
    {$endregion operator+}
    
    {$region cl_event.AttachCallback}
    
    public static procedure AttachNativeCallback(ev: cl_event; cb: EventCallback) :=
    cl.SetEventCallback(ev, CommandExecutionStatus.COMPLETE, cb, NativeUtils.GCHndAlloc(cb)).RaiseIfError;
    
    private static procedure CheckEvErr(ev: cl_event; err_handler: CLTaskErrHandler);
    begin
      {$ifdef EventDebug}
      EventDebug.CheckExists(ev);
      {$endif EventDebug}
      var st: CommandExecutionStatus;
      var ec := cl.GetEventInfo(ev, EventInfo.EVENT_COMMAND_EXECUTION_STATUS, new UIntPtr(sizeof(CommandExecutionStatus)), st, IntPtr.Zero);
      if err_handler.AddErr(ec) then exit;
      if err_handler.AddErr(st) then exit;
    end;
    
    public static procedure AttachCallback(midway: boolean; ev: cl_event; work: Action; err_handler: CLTaskErrHandler{$ifdef EventDebug}; reason: string{$endif});
    begin
      if midway then
      begin
        {$ifdef EventDebug}
        EventDebug.RegisterEventRetain(ev, $'retained before midway callback, working on {reason}');
        {$endif EventDebug}
        err_handler.AddErr(cl.RetainEvent(ev));
      end;
      AttachNativeCallback(ev, (ev,st,data)->
      begin
        // st копирует значение переданное в cl.SetEventCallback, поэтому он не подходит
        CheckEvErr(ev, err_handler);
        {$ifdef EventDebug}
        EventDebug.RegisterEventRelease(ev, $'released in callback, working on {reason}');
        {$endif EventDebug}
        err_handler.AddErr(cl.ReleaseEvent(ev));
        work;
        NativeUtils.GCHndFree(data);
      end);
    end;
    
    {$endregion cl_event.AttachCallback}
    
    {$region EventList.AttachCallback}
    
    public procedure AttachCallback(midway: boolean; work: Action; err_handler: CLTaskErrHandler{$ifdef EventDebug}; reason: string{$endif}) :=
    case self.count of
      0: work;
      1: AttachCallback(midway, self.evs[0], work, err_handler{$ifdef EventDebug}, reason{$endif});
      else
      begin
        var done_c := count;
        for var i := 0 to count-1 do
          AttachCallback(midway, evs[i], ()->
          begin
            if Interlocked.Decrement(done_c) <> 0 then exit;
            work;
          end, err_handler{$ifdef EventDebug}, reason{$endif});
      end;
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
      {$ifdef EventDebug}
      EventDebug.RegisterEventRelease(evs[i], $'{reason}, together with evs: {evs.JoinToString}');
      {$endif EventDebug}
      cl.ReleaseEvent(evs[i]).RaiseIfError;
    end;
    
    public procedure WaitAndRelease(err_handler: CLTaskErrHandler{$ifdef EventDebug}; reason: string{$endif});
    begin
      if count=0 then exit;
      
      var ec := cl.WaitForEvents(self.count, self.evs);
      if (ec=ErrorCode.EXEC_STATUS_ERROR_FOR_EVENTS_IN_WAIT_LIST) or not err_handler.AddErr(ec) then
        for var i := 0 to count-1 do
          CheckEvErr(evs[i], err_handler);
      
      self.Release({$ifdef EventDebug}$'discarding after being waited upon for {reason}'{$endif EventDebug});
    end;
    
    {$endregion Retain/Release}
    
  end;
  
{$endregion EventList}

{$region QueueRes}

type
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
  
  {$region Base}
  
  QueueRes<T> = abstract partial class end;
  QueueResBase = abstract partial class
    public ev: EventList;
    public can_set_ev := true;
    
    public constructor(ev: EventList) :=
    self.ev := ev;
    private constructor := raise new OpenCLABCInternalException;
    
    public function GetResBase: object; abstract;
    public function TrySetEvBase(new_ev: EventList): QueueResBase; abstract;
    
    public function LazyQuickTransformBase<T2>(f: object->T2): QueueRes<T2>; abstract;
    
    public function StabiliseBase(err_handler: CLTaskErrHandler): QueueResBase; abstract;
    
  end;
  
  QueueRes<T> = abstract partial class(QueueResBase)
    
    public function GetRes: T; abstract;
    public function GetResBase: object; override := GetRes;
    
    public function TrySetEv(new_ev: EventList): QueueRes<T>;
    begin
      if self.ev=new_ev then
        Result := self else
      begin
        Result := if can_set_ev then self else Clone;
        Result.ev := new_ev;
      end;
    end;
    public function TrySetEvBase(new_ev: EventList): QueueResBase; override := TrySetEv(new_ev);
    
    public function Clone: QueueRes<T>; abstract;
    
    public function LazyQuickTransform<T2>(f: T->T2): QueueRes<T2>; abstract;
    public function LazyQuickTransformBase<T2>(f: object->T2): QueueRes<T2>; override :=
    LazyQuickTransform(o->f(o)); //TODO #2221
    
    /// Должно выполнятся только после ожидания ивентов
    public function ToPtr: IPtrQueueRes<T>; abstract;
    
    public function StabiliseBase(err_handler: CLTaskErrHandler): QueueResBase; override := Stabilise(err_handler);
    public function Stabilise(err_handler: CLTaskErrHandler): QueueRes<T>; abstract;
    
  end;
  
  {$endregion Base}
  
  {$region Const}
  
  // Результат который просто есть
  QueueResConst<T> = sealed partial class(QueueRes<T>)
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
    
    public function Stabilise(err_handler: CLTaskErrHandler): QueueRes<T>; override := self;
    
  end;
  
  {$endregion Const}
  
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
    
    public function Clone: QueueRes<T>; override := new QueueResFunc<T>(f, ev);
    
    public function GetRes: T; override := f();
    
    public function LazyQuickTransform<T2>(f2: T->T2): QueueRes<T2>; override :=
    new QueueResFunc<T2>(()->f2(self.f), self.ev);
    
    public function ToPtr: IPtrQueueRes<T>; override := new QRPtrWrap<T>(self.f());
    
    public function Stabilise(err_handler: CLTaskErrHandler): QueueRes<T>; override := new QueueResConst<T>(self.GetRes, self.ev);
    
  end;
  
  {$endregion Func}
  
  {$region Delayed}
  
  // Результат который будет сохранён куда то, надо только дождаться
  QueueResDelayedBase<T> = abstract partial class(QueueRes<T>)
    
    public constructor := inherited Create(EventList.Empty);
    
    // QueueResFunc, потому что результат сохраняется именно в этот объект, а не в клон
    public function Clone: QueueRes<T>; override := new QueueResFunc<T>(self.GetRes, ev);
    
    public procedure SetRes(value: T); abstract;
    
    public function LazyQuickTransform<T2>(f: T->T2): QueueRes<T2>; override :=
    new QueueResFunc<T2>(()->f(self.GetRes()), self.ev);
    
    public function Stabilise(err_handler: CLTaskErrHandler): QueueRes<T>; override := self;
    
  end;
  
  QueueResDelayedObj<T> = sealed partial class(QueueResDelayedBase<T>)
    private res := default(T);
    
    public function GetRes: T; override := res;
    public procedure SetRes(value: T); override := res := value;
    
    public function ToPtr: IPtrQueueRes<T>; override := new QRPtrWrap<T>(res);
    
  end;
  
  IQueueResDelayedPtr = interface end; // Если параметры команды реализует - можно не ждать его ивент, а cl.enqueue сразу
  QueueResDelayedPtr<T> = sealed partial class(QueueResDelayedBase<T>, IPtrQueueRes<T>, IQueueResDelayedPtr)
    private ptr: ^T := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<T>));
    
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
  
  QueueResDelayedBase<T> = abstract partial class(QueueRes<T>)
    
    public static function MakeNew(need_ptr_qr: boolean) :=
    if need_ptr_qr then
      new QueueResDelayedPtr<T> as QueueResDelayedBase<T> else
      new QueueResDelayedObj<T> as QueueResDelayedBase<T>;
    
  end;
  
  {$endregion Delayed}
  
{$endregion QueueRes}

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
      ec.RaiseIfError;
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
        after.WaitAndRelease(err_handler{$ifdef EventDebug}, $'Background work with res_ev={res}'{$endif});
        
        if err_handler.HadError(true) then
        begin
          res.Abort;
          exit;
        end;
        
        try
          work;
        except
          on e: Exception do
          begin
            err_handler.AddErr(e);
            res.Abort;
            exit;
          end;
        end;
        
        res.SetStatus(CommandExecutionStatus.COMPLETE);
      end);
      
      Result := res;
    end;
    
    {$endregion constructor's}
    
    {$region Status}
    
    /// True если статус получилось изменить
    public function SetStatus(st: CommandExecutionStatus): boolean;
    begin
      Result := done.TrySet(true);
      if Result then cl.SetUserEventStatus(uev, st).RaiseIfError;
    end;
    public function Abort := SetStatus(CLTaskErrHandler.AbortStatus);
    
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
  CLTaskLocalData = record
    public need_ptr_qr := false;
    public prev_ev := EventList.Empty;
    
    {$region constructor's}
    
    public function WithPtrNeed(need_ptr_qr: boolean): CLTaskLocalData;
    begin
      Result := self;
      Result.need_ptr_qr := need_ptr_qr;
    end;
    
    {$endregion constructor's}
    
  end;
  
  CLTaskBranchInvoker = sealed class
    private prev_cq: cl_command_queue;
    private g: CLTaskGlobalData;
    private l: CLTaskLocalData;
    private branch_handlers := new List<CLTaskErrHandler>;
    private make_base_err_handler: ()->CLTaskErrHandler;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    constructor(g: CLTaskGlobalData; l: CLTaskLocalData; as_new: boolean; capacity: integer);
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
    function InvokeBranch(branch: (CLTaskGlobalData, CLTaskLocalData)->EventList): EventList;
    begin
      g.curr_err_handler := make_base_err_handler();
      
      Result := branch(g, l);
      
      var cq := g.curr_inv_cq;
      if cq<>cl_command_queue.Zero then
      begin
        g.curr_inv_cq := cl_command_queue.Zero;
        if prev_cq=cl_command_queue.Zero then
          prev_cq := cq else
          Result.AttachCallback(true, ()->
          begin
            {$ifdef QueueDebug}
            QueueDebug.Add(cq, '----- return -----');
            {$endif QueueDebug}
            g.free_cqs.Add(cq);
          end, g.curr_err_handler{$ifdef EventDebug}, $'returning cq to bag'{$endif});
      end;
      
      // Как можно позже, потому что вызовы использующие
      // err_handler могут заменять его на новый, свой собственный
      branch_handlers += g.curr_err_handler;
    end;
    
    public [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    function InvokeBranch<T>(branch: (CLTaskGlobalData, CLTaskLocalData)->QueueRes<T>): QueueRes<T>;
    begin
      var res: QueueRes<T>;
      InvokeBranch((g,l)->
      begin
        res := branch(g,l);
        Result := res.ev;
      end);
      Result := res;
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
    procedure ParallelInvoke(l: CLTaskLocalData; as_new: boolean; capacity: integer; use: CLTaskBranchInvoker->());
    begin
      var invoker := new CLTaskBranchInvoker(self, l, as_new, capacity);
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
        curr_err_handler.AddErr( cl.ReleaseCommandQueue(cq) );
      
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
  
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; abstract;
    protected function InvokeBase(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResBase; override :=
    Invoke(g, l);
    
  end;
  
{$endregion Base}

{$region Const}

type
  ConstQueue<T> = sealed partial class(CommandQueue<T>)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      
      if l.need_ptr_qr then
        Result := new QueueResDelayedPtr<T> (self.res, l.prev_ev) else
        Result := new QueueResConst<T>      (self.res, l.prev_ev);
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
  end;
  
{$endregion Const}

{$region Host}

type
  /// очередь, выполняющая какую то работу на CPU, всегда в отдельном потоке
  HostQueue<TInp,TRes> = abstract class(CommandQueue<TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TInp>; abstract;
    
    protected function ExecFunc(o: TInp; c: Context): TRes; abstract;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var prev_qr := InvokeSubQs(g, l.WithPtrNeed(false));
      
      var qr := QueueResDelayedBase&<TRes>.MakeNew(l.need_ptr_qr);
      qr.ev := UserEvent.StartBackgroundWork(prev_qr.ev, ()->qr.SetRes( ExecFunc(prev_qr.GetRes(), g.c) ), g
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
  
  CLTask<T> = sealed partial class(CLTaskBase)
    private q_res: QueueRes<T>;
    
    protected constructor(q: CommandQueue<T>; c: Context);
    begin
      self.q := q;
      self.org_c := c;
      
      var g_data := new CLTaskGlobalData(self);
      var l_data := new CLTaskLocalData;
      
      q.RegisterWaitables(g_data, new HashSet<IMultiusableCommandQueueHub>);
      self.q_res := q.Invoke(g_data, l_data);
      g_data.FinishInvoke;
      
      NativeUtils.StartNewBgThread(()->
      begin
        self.q_res.ev.WaitAndRelease(g_data.curr_err_handler{$ifdef EventDebug}, $'CLTask.OnQDone'{$endif});
        if not g_data.curr_err_handler.HadError(true) then
          self.q_res := q_res.Stabilise(g_data.curr_err_handler);
        g_data.FinishExecution(self.err_lst);
        wh.Set;
      end);
      
    end;
    
  end;
  
  CLTaskResLess = sealed class(CLTaskBase)
    protected q: CommandQueueBase;
    
    protected function OrgQueueBase: CommandQueueBase; override := q;
    
    protected constructor(q: CommandQueueBase; c: Context);
    begin
      self.q := q;
      self.org_c := c;
      
      var g_data := new CLTaskGlobalData(self);
      var l_data := new CLTaskLocalData;
      
      q.RegisterWaitables(g_data, new HashSet<IMultiusableCommandQueueHub>);
      var qr := q.InvokeBase(g_data, l_data);
      g_data.FinishInvoke;
      
      NativeUtils.StartNewBgThread(()->
      begin
        qr.ev.WaitAndRelease(g_data.curr_err_handler{$ifdef EventDebug}, $'CLTask.OnQDone'{$endif});
        if not g_data.curr_err_handler.HadError(true) then
          qr.StabiliseBase(g_data.curr_err_handler);
        g_data.FinishExecution(self.err_lst);
        wh.Set;
      end);
      
    end;
    
  end;
  
function Context.BeginInvoke<T>(q: CommandQueue<T>) := new CLTask<T>(q, self);
function Context.BeginInvoke(q: CommandQueueBase) := new CLTaskResLess(q, self);

function CLTask<T>.WaitRes: T;
begin
  Wait;
  Result := q_res.GetRes;
end;

{$endregion CLTask}

{$region Queue converter's}

{$region Cast}

type
  CastQueue<T> = sealed class(CommandQueue<T>)
    private q: CommandQueueBase;
    
    public constructor(q: CommandQueueBase) := self.q := q;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      var err_handler := g.curr_err_handler;
      Result := q.InvokeBase(g, l.WithPtrNeed(false)).LazyQuickTransformBase(o->
      try
        Result := T(o);
      except
        on e: Exception do
          err_handler.AddErr(e);
      end);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
function CommandQueueBase.Cast<T>: CommandQueue<T> :=
//TODO UseTyped
if self is CommandQueue<T>(var tcq) then
  tcq else new CastQueue<T>(self);

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
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TInp>; override := q.Invoke(g, l);
    
    protected function ExecFunc(o: TInp; c: Context): TRes; override := f(o, c);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      q.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, f);
      sb += #10;
      
    end;
    
  end;
  
function CommandQueue<T>.ThenConvert<TOtp>(f: (T, Context)->TOtp) :=
new CommandQueueThenConvert<T, TOtp>(self, f);

{$endregion ThenConvert}

{$region +/*}

{$region Simple}

type
  ISimpleQueueArray = interface
    function GetQS: sequence of CommandQueueBase;
  end;
  SimpleQueueArray<T> = abstract class(CommandQueue<T>, ISimpleQueueArray)
    protected qs: array of CommandQueueBase;
    protected last: CommandQueue<T>;
    
    public constructor(params qs: array of CommandQueueBase);
    begin
      self.qs := new CommandQueueBase[qs.Length-1];
      System.Array.Copy(qs, self.qs, qs.Length-1);
      self.last := qs[qs.Length-1].Cast&<T>;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    public function GetQS: sequence of CommandQueueBase := qs.Append(last as CommandQueueBase);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      foreach var q in qs do q.RegisterWaitables(g, prev_hubs);
      last.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      foreach var q in GetQS do
        q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  ISimpleSyncQueueArray = interface(ISimpleQueueArray) end;
  SimpleSyncQueueArray<T> = sealed class(SimpleQueueArray<T>, ISimpleSyncQueueArray)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      
      for var i := 0 to qs.Length-1 do
        l.prev_ev := qs[i].InvokeBase(g, l.WithPtrNeed(false)).ev;
      
      Result := last.Invoke(g, l);
    end;
    
  end;
  
  ISimpleAsyncQueueArray = interface(ISimpleQueueArray) end;
  SimpleAsyncQueueArray<T> = sealed class(SimpleQueueArray<T>, ISimpleAsyncQueueArray)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      if l.prev_ev.count<>0 then loop qs.Length do
        l.prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var evs := new EventList[qs.Length+1];
      
      var res: QueueRes<T>;
      g.ParallelInvoke(l, false, qs.Length+1, invoker->
      begin
        for var i := 0 to qs.Length-1 do
          evs[i] := invoker.InvokeBranch((g,l)->
            qs[i].InvokeBase(g, l.WithPtrNeed(false)).ev
          );
        res := invoker.InvokeBranch(last.Invoke);
        evs[qs.Length] := res.ev;
      end);
      
      Result := res.TrySetEv( EventList.Combine(evs) );
    end;
    
  end;
  
{$endregion Simple}

{$region Conv}

{$region Generic}

type
  ConvQueueArrayBase<TInp, TRes> = abstract class(HostQueue<array of TInp, TRes>)
    protected qs: array of CommandQueue<TInp>;
    protected f: Func<array of TInp, Context, TRes>;
    
    public constructor(qs: array of CommandQueue<TInp>; f: Func<array of TInp, Context, TRes>);
    begin
      self.qs := qs;
      self.f := f;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    foreach var q in qs do q.RegisterWaitables(g, prev_hubs);
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; override := f(o, c);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      foreach var q in qs do
        q.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, f);
      sb += #10;
    end;
    
  end;
  
  ConvSyncQueueArray<TInp, TRes> = sealed class(ConvQueueArrayBase<TInp, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<array of TInp>; override;
    begin
      var qrs := new QueueRes<TInp>[qs.Length];
      
      for var i := 0 to qs.Length-1 do
      begin
        // HostQueue уже передало l без need_ptr_qr
        // И Result тут промежуточный
        var qr := qs[i].Invoke(g, l);
        l.prev_ev := qr.ev;
        qrs[i] := qr;
      end;
      
      Result := new QueueResFunc<array of TInp>(()->
      begin
        Result := new TInp[qrs.Length];
        for var i := 0 to qrs.Length-1 do
          Result[i] := qrs[i].GetRes;
      end, l.prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray<TInp, TRes> = sealed class(ConvQueueArrayBase<TInp, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<array of TInp>; override;
    begin
      if l.prev_ev.count<>0 then loop qs.Length-1 do
        l.prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qrs := new QueueRes<TInp>[qs.Length];
      var evs := new EventList[qs.Length];
      
      g.ParallelInvoke(l, false, qs.Length, invoker ->
      for var i := 0 to qs.Length-1 do
      begin
        var qr := invoker.InvokeBranch&<TInp>(qs[i].Invoke);
        qrs[i] := qr;
        evs[i] := qr.ev;
      end);
      
      Result := new QueueResFunc<array of TInp>(()->
      begin
        Result := new TInp[qrs.Length];
        for var i := 0 to qrs.Length-1 do
          Result[i] := qrs[i].GetRes;
      end, EventList.Combine(evs));
    end;
    
  end;
  
{$endregion Generic}

{$region [2]}

type
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
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
    end;
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2>; c: Context): TRes; override := f(t.Item1, t.Item2, c);
    
  end;
  
  ConvSyncQueueArray2<TInp1, TInp2, TRes> = sealed class(ConvQueueArrayBase2<TInp1, TInp2, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2>>; override;
    begin
      l := l.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes()), l.prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray2<TInp1, TInp2, TRes> = sealed class(ConvQueueArrayBase2<TInp1, TInp2, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2>>; override;
    begin
      l := l.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 1 do l.prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      g.ParallelInvoke(l, false, 2, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
      end);
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes()), EventList.Combine(|qr1.ev, qr2.ev|));
    end;
    
  end;
  
{$endregion [2]}

{$region [3]}

type
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
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
    end;
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, c);
    
  end;
  
  ConvSyncQueueArray3<TInp1, TInp2, TInp3, TRes> = sealed class(ConvQueueArrayBase3<TInp1, TInp2, TInp3, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3>>; override;
    begin
      l := l.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes()), l.prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray3<TInp1, TInp2, TInp3, TRes> = sealed class(ConvQueueArrayBase3<TInp1, TInp2, TInp3, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3>>; override;
    begin
      l := l.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 2 do l.prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      g.ParallelInvoke(l, false, 3, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.Invoke);
        qr2 := invoker.InvokeBranch(q2.Invoke);
        qr3 := invoker.InvokeBranch(q3.Invoke);
      end);
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes()), EventList.Combine(|qr1.ev, qr2.ev, qr3.ev|));
    end;
    
  end;
  
{$endregion [3]}

{$region [4]}

type
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
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
      self.q4.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
      self.q4.ToString(sb, tabs, index, delayed);
    end;
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, c);
    
  end;
  
  ConvSyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(ConvQueueArrayBase4<TInp1, TInp2, TInp3, TInp4, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4>>; override;
    begin
      l := l.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes()), l.prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(ConvQueueArrayBase4<TInp1, TInp2, TInp3, TInp4, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4>>; override;
    begin
      l := l.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 3 do l.prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
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
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes()), EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev|));
    end;
    
  end;
  
{$endregion [4]}

{$region [5]}

type
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
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
      self.q4.RegisterWaitables(g, prev_hubs);
      self.q5.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
      self.q4.ToString(sb, tabs, index, delayed);
      self.q5.ToString(sb, tabs, index, delayed);
    end;
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, c);
    
  end;
  
  ConvSyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(ConvQueueArrayBase5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>; override;
    begin
      l := l.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      var qr5 := q5.Invoke(g, l); l.prev_ev := qr5.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes()), l.prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(ConvQueueArrayBase5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>; override;
    begin
      l := l.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 4 do l.prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
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
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes()), EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev|));
    end;
    
  end;
  
{$endregion [5]}

{$region [6]}

type
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
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.RegisterWaitables(g, prev_hubs);
      self.q2.RegisterWaitables(g, prev_hubs);
      self.q3.RegisterWaitables(g, prev_hubs);
      self.q4.RegisterWaitables(g, prev_hubs);
      self.q5.RegisterWaitables(g, prev_hubs);
      self.q6.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      self.q1.ToString(sb, tabs, index, delayed);
      self.q2.ToString(sb, tabs, index, delayed);
      self.q3.ToString(sb, tabs, index, delayed);
      self.q4.ToString(sb, tabs, index, delayed);
      self.q5.ToString(sb, tabs, index, delayed);
      self.q6.ToString(sb, tabs, index, delayed);
    end;
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, c);
    
  end;
  
  ConvSyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(ConvQueueArrayBase6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>; override;
    begin
      l := l.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      var qr5 := q5.Invoke(g, l); l.prev_ev := qr5.ev;
      var qr6 := q6.Invoke(g, l); l.prev_ev := qr6.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes()), l.prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(ConvQueueArrayBase6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>; override;
    begin
      l := l.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 5 do l.prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
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
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes()), EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev, qr6.ev|));
    end;
    
  end;
  
{$endregion [6]}

{$region [7]}

type
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
    
    protected function ExecFunc(t: ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>; c: Context): TRes; override := f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, c);
    
  end;
  
  ConvSyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(ConvQueueArrayBase7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>; override;
    begin
      l := l.WithPtrNeed(false);
      var qr1 := q1.Invoke(g, l); l.prev_ev := qr1.ev;
      var qr2 := q2.Invoke(g, l); l.prev_ev := qr2.ev;
      var qr3 := q3.Invoke(g, l); l.prev_ev := qr3.ev;
      var qr4 := q4.Invoke(g, l); l.prev_ev := qr4.ev;
      var qr5 := q5.Invoke(g, l); l.prev_ev := qr5.ev;
      var qr6 := q6.Invoke(g, l); l.prev_ev := qr6.ev;
      var qr7 := q7.Invoke(g, l); l.prev_ev := qr7.ev;
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes(), qr7.GetRes()), l.prev_ev);
    end;
    
  end;
  ConvAsyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(ConvQueueArrayBase7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>; override;
    begin
      l := l.WithPtrNeed(false);
      if l.prev_ev.count<>0 then loop 6 do l.prev_ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
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
      Result := new QueueResFunc<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>(()->ValueTuple.Create(qr1.GetRes(), qr2.GetRes(), qr3.GetRes(), qr4.GetRes(), qr5.GetRes(), qr6.GetRes(), qr7.GetRes()), EventList.Combine(|qr1.ev, qr2.ev, qr3.ev, qr4.ev, qr5.ev, qr6.ev, qr7.ev|));
    end;
    
  end;
  
{$endregion [7]}

{$endregion Conv}

{$region Utils}

type
  QueueArrayUtils = static class
    
    public static function FlattenQueueArray<T>(inp: sequence of CommandQueueBase): array of CommandQueueBase; where T: ISimpleQueueArray;
    begin
      var enmr := inp.GetEnumerator;
      if not enmr.MoveNext then raise new OpenCLABCInternalException('Функции CombineSyncQueue/CombineAsyncQueue не могут принимать 0 очередей');
      
      var res := new List<CommandQueueBase>;
      while true do
      begin
        var curr := enmr.Current;
        var next := enmr.MoveNext;
        
        //TODO UseTyped
//        if next then
//        begin
//          if curr is IConstQueue then continue;
//          if curr is ICastQueue(var cq) then curr := cq.GetQ;
//        end;
        
        if curr is T(var sqa) then
          res.AddRange(sqa.GetQS) else
          res += curr;
        
        if not next then break;
      end;
      
      Result := res.ToArray;
    end;
    
    public static function  FlattenSyncQueueArray(inp: sequence of CommandQueueBase) := FlattenQueueArray&< ISimpleSyncQueueArray>(inp);
    public static function FlattenAsyncQueueArray(inp: sequence of CommandQueueBase) := FlattenQueueArray&<ISimpleAsyncQueueArray>(inp);
    
  end;
  
{$endregion Utils}

function CommandQueueBase. AfterQueueSyncBase(q: CommandQueueBase) := q + self.Cast&<object>;
function CommandQueueBase.AfterQueueAsyncBase(q: CommandQueueBase) := q * self.Cast&<object>;

static function CommandQueue<T>.operator+(q1: CommandQueueBase; q2: CommandQueue<T>) := new  SimpleSyncQueueArray<T>(QueueArrayUtils. FlattenSyncQueueArray(|q1, q2|));
static function CommandQueue<T>.operator*(q1: CommandQueueBase; q2: CommandQueue<T>) := new SimpleAsyncQueueArray<T>(QueueArrayUtils.FlattenAsyncQueueArray(|q1, q2|));

{$endregion +/*}

{$region Multiusable}

type
  MultiusableCommandQueueHub<T> = sealed partial class(IMultiusableCommandQueueHub)
    public q: CommandQueue<T>;
    public constructor(q: CommandQueue<T>) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    public function OnNodeInvoked(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>;
    begin
      var prev_ev := l.prev_ev;
      
      var res_data: MultiuseableResultData;
      // Потоко-безопасно, потому что все .Invoke выполняются синхронно
      //TODO А что будет когда .ThenIf и т.п.
      if g.mu_res.TryGetValue(self, res_data) then
      begin
        g.curr_err_handler := new CLTaskErrHandlerMultiusableRepeater(g.curr_err_handler, res_data.err_handler);
        Result := QueueRes&<T>( res_data.qres );
      end else
      begin
        var prev_err_handler := g.curr_err_handler;
        g.curr_err_handler := new CLTaskErrHandlerEmpty;
        
        l.prev_ev := EventList.Empty;
        // Ради только 1 из веток делать доп. указатель - было бы странно
        l.need_ptr_qr := false;
        Result := self.q.Invoke(g, l);
        Result.can_set_ev := false;
        var q_err_handler := g.curr_err_handler;
        
        g.curr_err_handler := new CLTaskErrHandlerMultiusableRepeater(prev_err_handler, q_err_handler);
        g.mu_res[self] := new MultiuseableResultData(Result, q_err_handler);
      end;
      
      Result.ev.Retain({$ifdef EventDebug}$'for all mu branches'{$endif});
      if prev_ev.count<>0 then
      begin
        Result := Result.Clone;
        Result.ev := Result.ev+prev_ev;
      end;
    end;
    
  end;
  
  MultiusableCommandQueueNode<T> = sealed class(CommandQueue<T>)
    public hub: MultiusableCommandQueueHub<T>;
    public constructor(hub: MultiusableCommandQueueHub<T>) := self.hub := hub;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override := hub.OnNodeInvoked(g, l);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    if prev_hubs.Add(hub) then hub.q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ' => ';
      if hub.q.ToStringHeader(sb, index) then
        delayed.Add(hub.q);
      sb += #10;
    end;
    
  end;
  
  MultiusableCommandQueueHub<T> = sealed partial class(IMultiusableCommandQueueHub)
    
    public function MakeNode: CommandQueue<T> :=
    new MultiusableCommandQueueNode<T>(self);
    
  end;
  
function CommandQueueBase.MultiusableBase := self.Cast&<object>.Multiusable() as object as Func<CommandQueueBase>; //TODO #2221
function CommandQueue<T>.Multiusable: ()->CommandQueue<T> := MultiusableCommandQueueHub&<T>.Create(self).MakeNode;

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
    
    private function ThenMarkerSignalBase: WaitMarker; override := self.Cast&<object>.ThenMarkerSignal;
    private function ThenFinallyMarkerSignalBase: WaitMarker; override := self.Cast&<object>.ThenFinallyMarkerSignal;
    
    private function ThenWaitForBase(marker: WaitMarker): CommandQueueBase; override := self+WaitFor(marker);
    private function ThenFinallyWaitForBase(marker: WaitMarker): CommandQueueBase; override := self>=WaitFor(marker);
    
    private function AfterTry(try_do: CommandQueueBase): CommandQueueBase; override := try_do >= self.Cast&<object>;
    
    
    
    public procedure InitInnerHandles(g: CLTaskGlobalData); abstract;
    
    public function MakeWaitEv(g: CLTaskGlobalData; l: CLTaskLocalData): EventList; abstract;
    
  end;
  
{$endregion Base}

{$region Outer}

type
  /// wait_handler, который можно встроить в очередь как есть
  WaitHandlerOuter = abstract class
    public uev: UserEvent;
    private state := 0;
    
    public constructor(g: CLTaskGlobalData; l: CLTaskLocalData);
    begin
      {$ifdef DEBUG}
      if l.need_ptr_qr then raise new OpenCLABCInternalException($'wait with need_ptr_qr');
      {$endif DEBUG}
      
      uev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'Wait result'{$endif});
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Created outer with prev_ev=[ {l.prev_ev.evs?.JoinToString} ], res_ev={uev}');
      {$endif WaitDebug}
      EventList.AttachCallback(true, self.uev, ()->System.GC.KeepAlive(self), g.curr_err_handler{$ifdef EventDebug}, $'KeepAlive(WaitHandlerOuter)'{$endif});
      
      var err_handler := g.curr_err_handler;
      l.prev_ev.AttachCallback(false, ()->
      begin
        if err_handler.HadError(true) then
        begin
          {$ifdef WaitDebug}
          WaitDebug.RegisterAction(self, $'Aborted');
          {$endif WaitDebug}
          uev.Abort;
        end else
        begin
          {$ifdef WaitDebug}
          WaitDebug.RegisterAction(self, $'Got prev_ev boost');
          {$endif WaitDebug}
          self.IncState;
        end;
      end, err_handler{$ifdef EventDebug}, $'KeepAlive(handler[{self.GetHashCode}])'{$endif});
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
    
    public constructor(g: CLTaskGlobalData; l: CLTaskLocalData; source: WaitHandlerDirect);
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
      Result := source.TryReserve(1) and self.uev.SetStatus(CommandExecutionStatus.COMPLETE);
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
    
    public function MakeWaitEv(g: CLTaskGlobalData; l: CLTaskLocalData): EventList; override :=
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
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    raise new System.NotSupportedException($'Выполнять маркер-комбинацию нельзя. Возможно вы забыли написать WaitFor?');
    
    protected function InvokeBase(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResBase; override;
    begin
      Result := nil;
      // Не должно произойти, потому что RegisterWaitables вылетит первым
      raise new OpenCLABCInternalException;
    end;
    
    public procedure SendSignal; override :=
    raise new System.NotSupportedException($'Err:WaitMarkerCombination.SendSignal');
    
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
      Result := uev.SetStatus(CommandExecutionStatus.COMPLETE);
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
    
    public constructor(g: CLTaskGlobalData; l: CLTaskLocalData; sources: array of WaitHandlerDirect; ref_counts: array of integer);
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
      Result := uev.SetStatus(CommandExecutionStatus.COMPLETE);
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
    
    public function MakeWaitEv(g: CLTaskGlobalData; l: CLTaskLocalData): EventList; override :=
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
    
    public constructor(g: CLTaskGlobalData; l: CLTaskLocalData; markers: array of WaitMarkerAll);
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      foreach var child in children do
        child.ToString(sb, tabs, index, delayed);
    end;
    
    public function MakeWaitEv(g: CLTaskGlobalData; l: CLTaskLocalData): EventList; override :=
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
  WaitMarkerDummy = sealed class(WaitMarkerDirect)
    
    protected function InvokeBase(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResBase; override;
    begin
      {$ifdef DEBUG}
      if l.need_ptr_qr then raise new OpenCLABCInternalException($'marker with need_ptr_qr');
      {$endif DEBUG}
      Result := new QueueResConst<object>(nil, l.prev_ev);
      var err_handler := g.curr_err_handler;
      Result.ev.AttachCallback(true, ()->if not err_handler.HadError(true) then self.SendSignal, err_handler{$ifdef EventDebug}, $'SendSignal'{$endif});
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override := sb += #10;
    
  end;
  
static function WaitMarker.Create := new WaitMarkerDummy;

{$endregion WaitMarkerDummy}

{$region ThenWaitMarker}

type
  DetachedMarkerSignalWrapper = sealed class(WaitMarkerDirect)
    private org: CommandQueueBase;
    public constructor(org: CommandQueueBase) := self.org := org;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function InvokeBase(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResBase; override :=
    org.InvokeBase(g, l);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    org.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      org.ToStringHeader(sb, index);
      sb += #10;
      
    end;
    
  end;
  DetachedMarkerSignal<T> = sealed partial class(CommandQueue<T>)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      Result := self.q.Invoke(g, l);
      var err_handler := g.curr_err_handler;
      var callback: ()->();
      if signal_in_finally then
        callback := DetachedMarkerSignalWrapper(wrap).SendSignal else
        callback := ()->if not err_handler.HadError(true) then DetachedMarkerSignalWrapper(wrap).SendSignal;
      Result.ev.AttachCallback(true, callback, err_handler{$ifdef EventDebug}, $'ExecuteMWHandlers'{$endif});
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
  end;
  
constructor DetachedMarkerSignal<T>.Create(q: CommandQueue<T>; signal_in_finally: boolean);
begin
  self.q := q;
  self.wrap := new DetachedMarkerSignalWrapper(self);
  self.signal_in_finally := signal_in_finally;
end;

{$endregion ThenWaitMarker}

{$region WaitFor}

type
  CommandQueueWaitFor = sealed class(CommandQueue<object>)
    public marker: WaitMarker;
    public constructor(marker: WaitMarker) := self.marker := marker;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<object>; override :=
    new QueueResConst<object>(nil, marker.MakeWaitEv(g,l));
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    marker.InitInnerHandles(g);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      marker.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
function WaitFor(marker: WaitMarker) := new CommandQueueWaitFor(marker);

{$endregion WaitFor}

{$region ThenWait}

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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
      Result := Result.TrySetEv( marker.MakeWaitEv(g, l.WithPtrNeed(false)) );
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
      Result := Result.TrySetEv( marker.MakeWaitEv(g, l.WithPtrNeed(false)) );
      var m_err_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(origin_err_handler, |q_err_handler, m_err_handler|);
    end;
    
  end;
  
function CommandQueue<T>.ThenWaitFor(marker: WaitMarker) := new CommandQueueThenWaitFor<T>(self, marker);
function CommandQueue<T>.ThenFinallyWaitFor(marker: WaitMarker) := new CommandQueueThenFinallyWaitFor<T>(self, marker);

{$endregion ThenWait}

{$endregion Wait}

{$region Finally+Handle}

{$region Finally}

type
  CommandQueueTryFinally<T> = sealed class(CommandQueue<T>)
    private try_do: CommandQueueBase;
    private do_finally: CommandQueue<T>;
    
    private constructor(try_do: CommandQueueBase; do_finally: CommandQueue<T>);
    begin
      self.try_do := try_do;
      self.do_finally := do_finally;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      try_do.RegisterWaitables(g, prev_hubs);
      do_finally.RegisterWaitables(g, prev_hubs);
    end;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      var origin_err_handler := g.curr_err_handler;
      
      {$region try_do}
      var mid_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'mid_ev for {self.GetType}'{$endif});
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      var try_ev := try_do.InvokeBase(g, l.WithPtrNeed(false)).ev;
      var try_handler := g.curr_err_handler;
      
      try_ev.AttachCallback(false, ()->
      begin
        mid_ev.SetStatus(CommandExecutionStatus.COMPLETE);
      end, try_handler{$ifdef EventDebug}, $'Set mid_ev {mid_ev}'{$endif});
      
      {$endregion try_do}
      
      {$region do_finally}
      l.prev_ev := mid_ev;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      Result := do_finally.Invoke(g, l);
      var fin_handler := g.curr_err_handler;
      
      {$endregion do_finally}
      
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(origin_err_handler, |try_handler, fin_handler|);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      try_do.ToString(sb, tabs, index, delayed);
      do_finally.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
static function CommandQueue<T>.operator>=(try_do: CommandQueueBase; do_finally: CommandQueue<T>) :=
new CommandQueueTryFinally<T>(try_do, do_finally);

{$endregion Finally}

{$region Non-Finally}

type
  
  CommandQueueHandleWithoutRes = sealed class(CommandQueue<object>)
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<object>; override;
    begin
      var origin_err_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      var q_ev := q.InvokeBase(g, l.WithPtrNeed(false)).ev;
      var q_err_handler := g.curr_err_handler;
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(origin_err_handler, |q_err_handler|);
      
      var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for {self.GetType}'{$endif});
      q_ev.AttachCallback(false, ()->
      begin
        q_err_handler.TryRemoveErrors(handler);
        res_ev.SetStatus(CommandExecutionStatus.COMPLETE);
      end, g.curr_err_handler{$ifdef EventDebug}, $'Set res_ev {res_ev}'{$endif});
      
      Result := new QueueResConst<object>(nil, res_ev);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
      
      var res := QueueResDelayedBase&<T>.MakeNew(l.need_ptr_qr);
      var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for {self.GetType}'{$endif});
      res.ev := res_ev;
      
      prev_qr.ev.AttachCallback(false, ()->
      begin
        if not q_err_handler.HadError(true) then
          res.SetRes(prev_qr.GetRes) else
        begin
          q_err_handler.TryRemoveErrors(handler);
          if not q_err_handler.HadError(true) then
            res.SetRes(def);
        end;
        res_ev.SetStatus(CommandExecutionStatus.COMPLETE);
      end, g.curr_err_handler{$ifdef EventDebug}, $'Set res_ev {res_ev}'{$endif});
      
      Result := res;
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
      
      var res := QueueResDelayedBase&<T>.MakeNew(l.need_ptr_qr);
      var res_ev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for {self.GetType}'{$endif});
      res.ev := res_ev;
      
      prev_qr.ev.AttachCallback(false, ()->
      begin
        if not q_err_handler.HadError(true) then
          res.SetRes(prev_qr.GetRes) else
        begin
          q_err_handler.StealPrevErrors;
          var err_lst := q_err_handler.get_local_err_lst;
          var handler_res := handler(err_lst);
          if err_lst.Count=0 then res.SetRes(handler_res);
        end;
        res_ev.SetStatus(CommandExecutionStatus.COMPLETE);
      end, g.curr_err_handler{$ifdef EventDebug}, $'Set res_ev {res_ev}'{$endif});
      
      Result := res;
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ISetableKernelArg>; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); abstract;
    
  end;
  
{$endregion Base}

{$region Const}

{$region Base}

type
  ConstKernelArg = abstract class(KernelArg, ISetableKernelArg)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ISetableKernelArg>; override :=
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
    cl.SetKernelArg(k, ind, new UIntPtr(cl_mem.Size), a.ntv).RaiseIfError;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
    cl.SetKernelArg(k, ind, new UIntPtr(cl_mem.Size), mem.ntv).RaiseIfError;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
    cl.SetKernelArg(k, ind, sz, pointer(ptr)).RaiseIfError;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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

{$region Record}

type
  KernelArgValue<TRecord> = sealed class(ConstKernelArg)
  where TRecord: record;
    private val: ^TRecord := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>));
    
    static constructor :=
    BlittableHelper.RaiseIfBad(typeof(TRecord), 'передавать в качестве параметров kernel''а');
    
    public constructor(val: TRecord) := self.val^ := val;
    public constructor(val: ^TRecord) := self.val := val;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure Finalize; override :=
    Marshal.FreeHGlobal(new IntPtr(val));
    
    public procedure SetArg(k: cl_kernel; ind: UInt32); override :=
    cl.SetKernelArg(k, ind, new UIntPtr(Marshal.SizeOf&<TRecord>), pointer(self.val)).RaiseIfError; 
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ' => ';
      sb.Append(val^);
      sb += #10;
    end;
    
  end;
  
static function KernelArg.FromValue<TRecord>(val: TRecord) := new KernelArgValue<TRecord>(val);

{$endregion Record}

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
    cl.SetKernelArg(k, ind, new UIntPtr(Marshal.SizeOf&<TRecord>), (hnd.AddrOfPinnedObject+offset).ToPointer).RaiseIfError; 
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ISetableKernelArg>; override :=
    q.Invoke(g, l.WithPtrNeed(false)).LazyQuickTransform(a->new KernelArgCLArray<T>(a) as ISetableKernelArg);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ISetableKernelArg>; override :=
    q.Invoke(g, l.WithPtrNeed(false)).LazyQuickTransform(mem->new KernelArgMemorySegment(mem) as ISetableKernelArg);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ISetableKernelArg>; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      var  sz_qr: QueueRes<UIntPtr>;
      g.ParallelInvoke(l.WithPtrNeed(false), true, 2, invoker->
      begin
        ptr_qr := invoker.InvokeBranch(ptr_q.Invoke);
         sz_qr := invoker.InvokeBranch( sz_q.Invoke);
      end);
      Result := new QueueResFunc<ISetableKernelArg>(()->new KernelArgData(ptr_qr.GetRes, sz_qr.GetRes), ptr_qr.ev+sz_qr.ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr_q.RegisterWaitables(g, prev_hubs);
       sz_q.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      ptr_q.ToString(sb, tabs, index, delayed);
       sz_q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
static function KernelArg.FromDataCQ(ptr_q: CommandQueue<IntPtr>; sz_q: CommandQueue<UIntPtr>) :=
new KernelArgDataCQ(ptr_q, sz_q);

{$endregion Ptr}

{$region Record}

type
  KernelArgValueCQ<TRecord> = sealed class(InvokeableKernelArg)
  where TRecord: record;
    public q: CommandQueue<TRecord>;
    
    static constructor :=
    BlittableHelper.RaiseIfBad(typeof(TRecord), 'передавать в качестве параметров kernel''а');
    
    public constructor(q: CommandQueue<TRecord>) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ISetableKernelArg>; override;
    begin
      var prev_qr := q.Invoke(g, l.WithPtrNeed(true));
      if prev_qr is QueueResDelayedPtr<TRecord>(var ptr_qr) then
      begin
        Result := new QueueResConst<ISetableKernelArg>(new KernelArgValue<TRecord>(ptr_qr.ptr), ptr_qr.ev);
        ptr_qr.ptr := nil;
      end else
        Result := new QueueResFunc<ISetableKernelArg>(()->new KernelArgValue<TRecord>(prev_qr.GetRes), prev_qr.ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
static function KernelArg.FromValueCQ<TRecord>(valq: CommandQueue<TRecord>) :=
new KernelArgValueCQ<TRecord>(valq);

{$endregion Record}

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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ISetableKernelArg>; override;
    begin
      var   a_qr: QueueRes<array of TRecord>;
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(l.WithPtrNeed(false), true, 2, invoker->
      begin
          a_qr := invoker.InvokeBranch(  a_q.Invoke);
        ind_qr := invoker.InvokeBranch(ind_q.Invoke);
      end);
      Result := new QueueResFunc<ISetableKernelArg>(()->new KernelArgArray<TRecord>(a_qr.GetRes, ind_qr.GetRes), a_qr.ev+ind_qr.ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
        a_q.RegisterWaitables(g, prev_hubs);
      ind_q.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  GPUCommand<T> = abstract class
    
    protected function InvokeObj  (o: T;                     g: CLTaskGlobalData; l: CLTaskLocalData): EventList; abstract;
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; g: CLTaskGlobalData; l: CLTaskLocalData): EventList; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); abstract;
    
    protected function DisplayName: string; virtual := CommandQueueBase.DisplayNameForType(self.GetType);
    protected static procedure ToStringWriteDelegate(sb: StringBuilder; d: System.Delegate) := CommandQueueBase.ToStringWriteDelegate(sb,d);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); abstract;
    
    private procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>);
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
    
  end;
  
{$endregion Base}

{$region Queue}

type
  QueueCommand<T> = sealed class(BasicGPUCommand<T>)
    public q: CommandQueueBase;
    
    public constructor(q: CommandQueueBase) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    private function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData) := q.InvokeBase(g, l).ev;
    
    protected function InvokeObj  (o: T;                     g: CLTaskGlobalData; l: CLTaskLocalData): EventList; override := Invoke(g, l);
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; g: CLTaskGlobalData; l: CLTaskLocalData): EventList; override := Invoke(g, l);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
{$endregion Queue}

{$region Proc}

type
  ProcCommand<T> = sealed class(BasicGPUCommand<T>)
    public p: (T,Context)->();
    
    public constructor(p: (T,Context)->()) := self.p := p;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function InvokeObj(o: T; g: CLTaskGlobalData; l: CLTaskLocalData): EventList; override :=
    UserEvent.StartBackgroundWork(l.prev_ev, ()->p(o, g.c), g
      {$ifdef EventDebug}, $'const body of {self.GetType}'{$endif}
    );
    
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; g: CLTaskGlobalData; l: CLTaskLocalData): EventList; override;
    begin
      var o_q_res := o_q().Invoke(g, l);
      Result := UserEvent.StartBackgroundWork(o_q_res.ev, ()->p(o_q_res.GetRes(), g.c), g
        {$ifdef EventDebug}, $'queue body of {self.GetType}'{$endif}
      );
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ': ';
      ToStringWriteDelegate(sb, p);
      sb += #10;
    end;
    
  end;
  
{$endregion Proc}

{$region Wait}

type
  WaitCommand<T> = sealed class(BasicGPUCommand<T>)
    public marker: WaitMarker;
    
    public constructor(marker: WaitMarker) := self.marker := marker;
    private constructor := raise new OpenCLABCInternalException;
    
    private function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData) := marker.MakeWaitEv(g, l);
    
    protected function InvokeObj  (o: T;                     g: CLTaskGlobalData; l: CLTaskLocalData): EventList; override := Invoke(g, l);
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; g: CLTaskGlobalData; l: CLTaskLocalData): EventList; override := Invoke(g, l);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    marker.InitInnerHandles(g);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      marker.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; abstract;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); abstract;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); abstract;
    private procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>);
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
      if l.need_ptr_qr then raise new OpenCLABCInternalException($'GPUCommandContainer with need_ptr_qr');
      {$endif DEBUG}
      Result := core.Invoke(g, l);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      core.RegisterWaitables(g, prev_hubs);
      foreach var comm in commands do comm.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      var o := self.o;
      
      foreach var comm in cc.commands do
        l.prev_ev := comm.InvokeObj(o, g, l);
      
      Result := new QueueResConst<T>(o, l.prev_ev);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; override;
    begin
      var new_plug: ()->CommandQueue<T> := hub.MakeNode;
      
      foreach var comm in cc.commands do
        l.prev_ev := comm.InvokeQueue(new_plug, g, l);
      
      Result := new_plug().Invoke(g, l);
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    hub.q.RegisterWaitables(g, prev_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  //TODO UseTyped
//  if q is IConstQueue then raise new System.ArgumentException($'%Err:AddQueue(Const)%');
//  if q is ICastQueue(var cq) then q := cq.GetQ;
  commands.Add( new QueueCommand<Kernel>(q) );
end;

function KernelCCQ.AddProc(p: Kernel->()) := AddCommand(self, new ProcCommand<Kernel>((o,c)->p(o)));
function KernelCCQ.AddProc(p: (Kernel, Context)->()) := AddCommand(self, new ProcCommand<Kernel>(p));

function KernelCCQ.AddWait(marker: WaitMarker) := AddCommand(self, new WaitCommand<Kernel>(marker));

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
  //TODO UseTyped
//  if q is IConstQueue then raise new System.ArgumentException($'%Err:AddQueue(Const)%');
//  if q is ICastQueue(var cq) then q := cq.GetQ;
  commands.Add( new QueueCommand<MemorySegment>(q) );
end;

function MemorySegmentCCQ.AddProc(p: MemorySegment->()) := AddCommand(self, new ProcCommand<MemorySegment>((o,c)->p(o)));
function MemorySegmentCCQ.AddProc(p: (MemorySegment, Context)->()) := AddCommand(self, new ProcCommand<MemorySegment>(p));

function MemorySegmentCCQ.AddWait(marker: WaitMarker) := AddCommand(self, new WaitCommand<MemorySegment>(marker));

{$endregion Special .Add's}

{$endregion MemorySegment}

{$region CLArray}

type
  CLArrayCCQ<T> = sealed partial class(GPUCommandContainer<CLArray<T>>)
    
  end;
  
static function KernelArg.operator implicit<T>(a_q: CLArrayCCQ<T>): KernelArg; where T: record;
begin Result := FromCLArrayCQ(a_q); end;

constructor CLArrayCCQ<T>.Create(o: CLArray<T>) := inherited;
constructor CLArrayCCQ<T>.Create(q: CommandQueue<CLArray<T>>) := inherited;
constructor CLArrayCCQ<T>.Create := inherited;

{$region Special .Add's}

function CLArrayCCQ<T>.AddQueue(q: CommandQueueBase): CLArrayCCQ<T>;
begin
  Result := self;
  //TODO UseTyped
//  if q is IConstQueue then raise new System.ArgumentException($'%Err:AddQueue(Const)%');
//  if q is ICastQueue(var cq) then q := cq.GetQ;
  commands.Add( new QueueCommand<CLArray<T>>(q) );
end;

function CLArrayCCQ<T>.AddProc(p: CLArray<T>->()) := AddCommand(self, new ProcCommand<CLArray<T>>((o,c)->p(o)));
function CLArrayCCQ<T>.AddProc(p: (CLArray<T>, Context)->()) := AddCommand(self, new ProcCommand<CLArray<T>>(p));

function CLArrayCCQ<T>.AddWait(marker: WaitMarker) := AddCommand(self, new WaitCommand<CLArray<T>>(marker));

{$endregion Special .Add's}

{$endregion CLArray}

{$endregion GPUCommandContainer}

{$region Enqueueable's}

{$region Core}

type
  EnqueueableEnqFunc<TInvData> = function(cq: cl_command_queue; err_handler: CLTaskErrHandler; ev_l2: EventList; inv_data: TInvData): cl_event;
  IEnqueueable<TInvData> = interface
    
    function ParamCountL1: integer;
    function ParamCountL2: integer;
    
    function InvokeParams(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): EnqueueableEnqFunc<TInvData>;
    
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
    
    public static function Invoke<TEnq, TInvData>(q: TEnq; inv_data: TInvData; g: CLTaskGlobalData; l: CLTaskLocalData; l1_start_ev, l2_start_ev: EventList): EventList; where TEnq: IEnqueueable<TInvData>;
    begin
      var param_count_l1 := q.ParamCountL1;
      var param_count_l2 := q.ParamCountL2;
      
      // +param_count_l2, потому что, к примеру, .Cast может вернуть не QueueResDelayedPtr, даже при need_ptr_qr
      var evs_l1 := MakeEvList(param_count_l1+param_count_l2, l1_start_ev); // Ожидание, перед вызовом  cl.Enqueue*
      var evs_l2 := MakeEvList(               param_count_l2, l2_start_ev); // Ожидание, передаваемое в cl.Enqueue*
      
      var pre_params_handler := g.curr_err_handler;
      var enq_f := q.InvokeParams(g, l, evs_l1, evs_l2);
      {$ifdef DEBUG}
      begin
        var r1,r2: integer;
        var ev_exists := function(ev: EventList): integer -> integer(ev.count<>0);
        r1 := param_count_l1 +                + ev_exists(l1_start_ev);
        r2 := param_count_l1 + param_count_l2 + ev_exists(l1_start_ev);
        if not evs_l1.Count.InRange(r1, r2) then raise new OpenCLABCInternalException($'{q.GetType.Name}[L1]: {evs_l1.Count}.InRange({r1}, {r2})');
        r1 :=                + ev_exists(l2_start_ev);
        r2 := param_count_l2 + ev_exists(l2_start_ev);
        if not evs_l2.Count.InRange(r1, r2) then raise new OpenCLABCInternalException($'{q.GetType.Name}[L2]: {evs_l2.Count}.InRange({r1}, {r2})');
      end;
      {$endif DEBUG}
      var ev_l1 := EventList.Combine(evs_l1);
      var ev_l2 := EventList.Combine(evs_l2);
      
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
        ev_l1.AttachCallback(false, ()->
        begin
          // Can't cache, ev_l2 wasn't completed yet
          if post_params_handler.HadError(false) then
          begin
            res_ev.Abort;
            g.free_cqs.Add(cq);
            exit;
          end;
          ExecuteEnqFunc(cq, q, enq_f, inv_data, ev_l2, post_params_handler).AttachCallback(false, ()->
          begin
            res_ev.SetStatus(CommandExecutionStatus.COMPLETE);
            g.free_cqs.Add(cq);
          end, post_params_handler{$ifdef EventDebug}, $'propagating Enq ev of {q.GetType} to res_ev: {res_ev.uev}'{$endif});
        end, post_params_handler{$ifdef EventDebug}, $'calling async Enq of {q.GetType}'{$endif});
        
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
    
    public function ParamCountL1: integer; abstract;
    public function ParamCountL2: integer; abstract;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (T, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; abstract;
    public function InvokeParams(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): EnqueueableEnqFunc<EnqueueableGPUCommandInvData<T>>;
    begin
      var enq_f := InvokeParamsImpl(g, l, evs_l1, evs_l2);
      Result := (lcq, err_handler, ev, data)->enq_f(data.qr.GetRes, lcq, err_handler, ev);
    end;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData; prev_qr: QueueRes<T>; l2_start_ev: EventList): EventList;
    begin
      var inv_data: EnqueueableGPUCommandInvData<T>;
      inv_data.qr  := prev_qr;
      
      l.prev_ev := EventList.Empty; // InfokeObj/InvokeQueue уже используего его
      Result := EnqueueableCore.Invoke(self, inv_data, g, l, prev_qr.ev, l2_start_ev);
    end;
    
    protected function InvokeObj(o: T; g: CLTaskGlobalData; l: CLTaskLocalData): EventList; override :=
    Invoke(g, l, new QueueResConst<T>(o, EventList.Empty), l.prev_ev);
    
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; g: CLTaskGlobalData; l: CLTaskLocalData): EventList; override :=
    Invoke(g, l, o_q().Invoke(g, l), EventList.Empty);
    
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
    
    public function ParamCountL1: integer; abstract;
    public function ParamCountL2: integer; abstract;
    
    public function ForcePtrQr: boolean; virtual := false;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (TObj, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<TRes>)->cl_event; abstract;
    public function InvokeParams(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): EnqueueableEnqFunc<EnqueueableGetCommandInvData<TObj, TRes>>;
    begin
      var enq_f := InvokeParamsImpl(g, l, evs_l1, evs_l2);
      Result := (lcq, err_handler, ev, data)->enq_f(data.prev_qr.GetRes, lcq, err_handler, ev, data.res_qr);
    end;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override;
    begin
      var prev_qr := prev_commands.Invoke(g, l.WithPtrNeed(false));
      l.prev_ev := EventList.Empty;
      
      var inv_data: EnqueueableGetCommandInvData<TObj, TRes>;
      inv_data.prev_qr  := prev_qr;
      inv_data.res_qr   := QueueResDelayedBase&<TRes>.MakeNew(l.need_ptr_qr or ForcePtrQr);
      
      Result := inv_data.res_qr;
      Result.ev := EnqueueableCore.Invoke(self, inv_data, g, l, prev_qr.ev, EventList.Empty);
    end;
    
  end;
  
{$endregion GetCommand}

{$region Kernel}

{$region Implicit}

{$region 1#Exec}

function Kernel.Exec1(sz1: CommandQueue<integer>; params args: array of KernelArg): Kernel :=
Context.Default.SyncInvoke(self.NewQueue.AddExec1(sz1, args));

function Kernel.Exec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): Kernel :=
Context.Default.SyncInvoke(self.NewQueue.AddExec2(sz1, sz2, args));

function Kernel.Exec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): Kernel :=
Context.Default.SyncInvoke(self.NewQueue.AddExec3(sz1, sz2, sz3, args));

function Kernel.Exec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): Kernel :=
Context.Default.SyncInvoke(self.NewQueue.AddExec(global_work_offset, global_work_size, local_work_size, args));

{$endregion 1#Exec}

{$endregion Implicit}

{$region Explicit}

{$region 1#Exec}

{$region Exec1}

type
  KernelCommandExec1 = sealed class(EnqueueableGPUCommand<Kernel>)
    private  sz1: CommandQueue<integer>;
    private args: array of KernelArg;
    
    public function ParamCountL1: integer; override := 1 + args.Length;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(sz1: CommandQueue<integer>; params args: array of KernelArg);
    begin
      self. sz1 :=  sz1;
      self.args := args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (Kernel, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var  sz1_qr: QueueRes<integer>;
      var args_qr: array of QueueRes<ISetableKernelArg>;
      g.ParallelInvoke(l, true, 1 + args.Length, invoker->
      begin
         sz1_qr := invoker.InvokeBranch&<integer>( sz1.Invoke); evs_l1.Add(sz1_qr.ev);
        args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<ISetableKernelArg>(temp1.Invoke); evs_l1.Add(Result.ev); end);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var  sz1 :=  sz1_qr.GetRes;
        var args := args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        o.UseExclusiveNative(ntv->
        begin
          
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i);
          
          cl.EnqueueNDRangeKernel(
            cq, ntv, 1,
            nil,
            new UIntPtr[](new UIntPtr(sz1)),
            nil,
            evs.count, evs.evs, res_ev
          ).RaiseIfError;
          
          cl.RetainKernel(ntv).RaiseIfError;
          var args_hnd := GCHandle.Alloc(args);
          
          EventList.AttachCallback(true, res_ev, ()->
          begin
            cl.ReleaseKernel(ntv).RaiseIfError();
            args_hnd.Free;
          end, err_handler{$ifdef EventDebug}, 'ReleaseKernel'{$endif});
        end);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
       sz1.RegisterWaitables(g, prev_hubs);
      foreach var temp1 in args do temp1.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function KernelCCQ.AddExec1(sz1: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ :=
AddCommand(self, new KernelCommandExec1(sz1, args));

{$endregion Exec1}

{$region Exec2}

type
  KernelCommandExec2 = sealed class(EnqueueableGPUCommand<Kernel>)
    private  sz1: CommandQueue<integer>;
    private  sz2: CommandQueue<integer>;
    private args: array of KernelArg;
    
    public function ParamCountL1: integer; override := 2 + args.Length;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg);
    begin
      self. sz1 :=  sz1;
      self. sz2 :=  sz2;
      self.args := args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (Kernel, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var  sz1_qr: QueueRes<integer>;
      var  sz2_qr: QueueRes<integer>;
      var args_qr: array of QueueRes<ISetableKernelArg>;
      g.ParallelInvoke(l, true, 2 + args.Length, invoker->
      begin
         sz1_qr := invoker.InvokeBranch&<integer>( sz1.Invoke); evs_l1.Add(sz1_qr.ev);
         sz2_qr := invoker.InvokeBranch&<integer>( sz2.Invoke); evs_l1.Add(sz2_qr.ev);
        args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<ISetableKernelArg>(temp1.Invoke); evs_l1.Add(Result.ev); end);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var  sz1 :=  sz1_qr.GetRes;
        var  sz2 :=  sz2_qr.GetRes;
        var args := args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        o.UseExclusiveNative(ntv->
        begin
          
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i);
          
          cl.EnqueueNDRangeKernel(
            cq, ntv, 2,
            nil,
            new UIntPtr[](new UIntPtr(sz1),new UIntPtr(sz2)),
            nil,
            evs.count, evs.evs, res_ev
          ).RaiseIfError;
          
          cl.RetainKernel(ntv).RaiseIfError;
          var args_hnd := GCHandle.Alloc(args);
          
          EventList.AttachCallback(true, res_ev, ()->
          begin
            cl.ReleaseKernel(ntv).RaiseIfError();
            args_hnd.Free;
          end, err_handler{$ifdef EventDebug}, 'ReleaseKernel'{$endif});
        end);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
       sz1.RegisterWaitables(g, prev_hubs);
       sz2.RegisterWaitables(g, prev_hubs);
      foreach var temp1 in args do temp1.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function KernelCCQ.AddExec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ :=
AddCommand(self, new KernelCommandExec2(sz1, sz2, args));

{$endregion Exec2}

{$region Exec3}

type
  KernelCommandExec3 = sealed class(EnqueueableGPUCommand<Kernel>)
    private  sz1: CommandQueue<integer>;
    private  sz2: CommandQueue<integer>;
    private  sz3: CommandQueue<integer>;
    private args: array of KernelArg;
    
    public function ParamCountL1: integer; override := 3 + args.Length;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg);
    begin
      self. sz1 :=  sz1;
      self. sz2 :=  sz2;
      self. sz3 :=  sz3;
      self.args := args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (Kernel, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var  sz1_qr: QueueRes<integer>;
      var  sz2_qr: QueueRes<integer>;
      var  sz3_qr: QueueRes<integer>;
      var args_qr: array of QueueRes<ISetableKernelArg>;
      g.ParallelInvoke(l, true, 3 + args.Length, invoker->
      begin
         sz1_qr := invoker.InvokeBranch&<integer>( sz1.Invoke); evs_l1.Add(sz1_qr.ev);
         sz2_qr := invoker.InvokeBranch&<integer>( sz2.Invoke); evs_l1.Add(sz2_qr.ev);
         sz3_qr := invoker.InvokeBranch&<integer>( sz3.Invoke); evs_l1.Add(sz3_qr.ev);
        args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<ISetableKernelArg>(temp1.Invoke); evs_l1.Add(Result.ev); end);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var  sz1 :=  sz1_qr.GetRes;
        var  sz2 :=  sz2_qr.GetRes;
        var  sz3 :=  sz3_qr.GetRes;
        var args := args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        o.UseExclusiveNative(ntv->
        begin
          
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i);
          
          cl.EnqueueNDRangeKernel(
            cq, ntv, 3,
            nil,
            new UIntPtr[](new UIntPtr(sz1),new UIntPtr(sz2),new UIntPtr(sz3)),
            nil,
            evs.count, evs.evs, res_ev
          ).RaiseIfError;
          
          cl.RetainKernel(ntv).RaiseIfError;
          var args_hnd := GCHandle.Alloc(args);
          
          EventList.AttachCallback(true, res_ev, ()->
          begin
            cl.ReleaseKernel(ntv).RaiseIfError();
            args_hnd.Free;
          end, err_handler{$ifdef EventDebug}, 'ReleaseKernel'{$endif});
        end);
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function KernelCCQ.AddExec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ :=
AddCommand(self, new KernelCommandExec3(sz1, sz2, sz3, args));

{$endregion Exec3}

{$region Exec}

type
  KernelCommandExec = sealed class(EnqueueableGPUCommand<Kernel>)
    private global_work_offset: CommandQueue<array of UIntPtr>;
    private   global_work_size: CommandQueue<array of UIntPtr>;
    private    local_work_size: CommandQueue<array of UIntPtr>;
    private               args: array of KernelArg;
    
    public function ParamCountL1: integer; override := 3 + args.Length;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg);
    begin
      self.global_work_offset := global_work_offset;
      self.  global_work_size :=   global_work_size;
      self.   local_work_size :=    local_work_size;
      self.              args :=               args;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (Kernel, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var global_work_offset_qr: QueueRes<array of UIntPtr>;
      var   global_work_size_qr: QueueRes<array of UIntPtr>;
      var    local_work_size_qr: QueueRes<array of UIntPtr>;
      var               args_qr: array of QueueRes<ISetableKernelArg>;
      g.ParallelInvoke(l, true, 3 + args.Length, invoker->
      begin
        global_work_offset_qr := invoker.InvokeBranch&<array of UIntPtr>(global_work_offset.Invoke); evs_l1.Add(global_work_offset_qr.ev);
          global_work_size_qr := invoker.InvokeBranch&<array of UIntPtr>(  global_work_size.Invoke); evs_l1.Add(global_work_size_qr.ev);
           local_work_size_qr := invoker.InvokeBranch&<array of UIntPtr>(   local_work_size.Invoke); evs_l1.Add(local_work_size_qr.ev);
                      args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<ISetableKernelArg>(temp1.Invoke); evs_l1.Add(Result.ev); end);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var global_work_offset := global_work_offset_qr.GetRes;
        var   global_work_size :=   global_work_size_qr.GetRes;
        var    local_work_size :=    local_work_size_qr.GetRes;
        var               args :=               args_qr.ConvertAll(temp1->temp1.GetRes);
        var res_ev: cl_event;
        
        o.UseExclusiveNative(ntv->
        begin
          
          for var i := 0 to args.Length-1 do
            args[i].SetArg(ntv, i);
          
          cl.EnqueueNDRangeKernel(
            cq, ntv, global_work_size.Length,
            global_work_offset,
            global_work_size,
            local_work_size,
            evs.count, evs.evs, res_ev
          ).RaiseIfError;
          
          cl.RetainKernel(ntv).RaiseIfError;
          var args_hnd := GCHandle.Alloc(args);
          
          EventList.AttachCallback(true, res_ev, ()->
          begin
            cl.ReleaseKernel(ntv).RaiseIfError();
            args_hnd.Free;
          end, err_handler{$ifdef EventDebug}, 'ReleaseKernel'{$endif});
        end);
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function KernelCCQ.AddExec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): KernelCCQ :=
AddCommand(self, new KernelCommandExec(global_work_offset, global_work_size, local_work_size, args));

{$endregion Exec}

{$endregion 1#Exec}

{$endregion Explicit}

{$endregion Kernel}

{$region MemorySegment}

{$region Implicit}

{$region 1#Write&Read}

function MemorySegment.WriteData(ptr: CommandQueue<IntPtr>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteData(ptr));

function MemorySegment.WriteData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteData(ptr, mem_offset, len));

function MemorySegment.ReadData(ptr: CommandQueue<IntPtr>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddReadData(ptr));

function MemorySegment.ReadData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddReadData(ptr, mem_offset, len));

function MemorySegment.WriteData(ptr: pointer): MemorySegment :=
WriteData(IntPtr(ptr));

function MemorySegment.WriteData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegment :=
WriteData(IntPtr(ptr), mem_offset, len);

function MemorySegment.ReadData(ptr: pointer): MemorySegment :=
ReadData(IntPtr(ptr));

function MemorySegment.ReadData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegment :=
ReadData(IntPtr(ptr), mem_offset, len);

function MemorySegment.WriteValue<TRecord>(val: TRecord): MemorySegment :=
WriteValue(val, 0);

function MemorySegment.WriteValue<TRecord>(val: CommandQueue<TRecord>): MemorySegment :=
WriteValue(val, 0);

function MemorySegment.WriteValue<TRecord>(val: TRecord; mem_offset: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteValue&<TRecord>(val, mem_offset));

function MemorySegment.WriteValue<TRecord>(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteValue&<TRecord>(val, mem_offset));

function MemorySegment.WriteArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray1&<TRecord>(a));

function MemorySegment.WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray2&<TRecord>(a));

function MemorySegment.WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray3&<TRecord>(a));

function MemorySegment.ReadArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray1&<TRecord>(a));

function MemorySegment.ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray2&<TRecord>(a));

function MemorySegment.ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray3&<TRecord>(a));

function MemorySegment.WriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray1&<TRecord>(a, a_offset, len, mem_offset));

function MemorySegment.WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray2&<TRecord>(a, a_offset1, a_offset2, len, mem_offset));

function MemorySegment.WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray3&<TRecord>(a, a_offset1, a_offset2, a_offset3, len, mem_offset));

function MemorySegment.ReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray1&<TRecord>(a, a_offset, len, mem_offset));

function MemorySegment.ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray2&<TRecord>(a, a_offset1, a_offset2, len, mem_offset));

function MemorySegment.ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray3&<TRecord>(a, a_offset1, a_offset2, a_offset3, len, mem_offset));

{$endregion 1#Write&Read}

{$region 2#Fill}

function MemorySegment.FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillData(ptr, pattern_len));

function MemorySegment.FillData(ptr: CommandQueue<IntPtr>; pattern_len, mem_offset, len: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillData(ptr, pattern_len, mem_offset, len));

function MemorySegment.FillValue<TRecord>(val: TRecord): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val));

function MemorySegment.FillValue<TRecord>(val: CommandQueue<TRecord>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val));

function MemorySegment.FillValue<TRecord>(val: TRecord; mem_offset, len: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val, mem_offset, len));

function MemorySegment.FillValue<TRecord>(val: CommandQueue<TRecord>; mem_offset, len: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue&<TRecord>(val, mem_offset, len));

function MemorySegment.FillArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillArray1&<TRecord>(a));

function MemorySegment.FillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillArray2&<TRecord>(a));

function MemorySegment.FillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillArray3&<TRecord>(a));

function MemorySegment.FillArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillArray1&<TRecord>(a, a_offset, pattern_len, len, mem_offset));

function MemorySegment.FillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillArray2&<TRecord>(a, a_offset1, a_offset2, pattern_len, len, mem_offset));

function MemorySegment.FillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddFillArray3&<TRecord>(a, a_offset1, a_offset2, a_offset3, pattern_len, len, mem_offset));

{$endregion 2#Fill}

{$region 3#Copy}

function MemorySegment.CopyTo(mem: CommandQueue<MemorySegment>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyTo(mem));

function MemorySegment.CopyTo(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyTo(mem, from_pos, to_pos, len));

function MemorySegment.CopyFrom(mem: CommandQueue<MemorySegment>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyFrom(mem));

function MemorySegment.CopyFrom(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegment :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyFrom(mem, from_pos, to_pos, len));

{$endregion 3#Copy}

{$region Get}

function MemorySegment.GetData: IntPtr :=
Context.Default.SyncInvoke(self.NewQueue.AddGetData);

function MemorySegment.GetData(mem_offset, len: CommandQueue<integer>): IntPtr :=
Context.Default.SyncInvoke(self.NewQueue.AddGetData(mem_offset, len));

function MemorySegment.GetValue<TRecord>: TRecord :=
GetValue&<TRecord>(0);

function MemorySegment.GetValue<TRecord>(mem_offset: CommandQueue<integer>): TRecord :=
Context.Default.SyncInvoke(self.NewQueue.AddGetValue&<TRecord>(mem_offset));

function MemorySegment.GetArray1<TRecord>: array of TRecord :=
Context.Default.SyncInvoke(self.NewQueue.AddGetArray1&<TRecord>);

function MemorySegment.GetArray1<TRecord>(len: CommandQueue<integer>): array of TRecord :=
Context.Default.SyncInvoke(self.NewQueue.AddGetArray1&<TRecord>(len));

function MemorySegment.GetArray2<TRecord>(len1,len2: CommandQueue<integer>): array[,] of TRecord :=
Context.Default.SyncInvoke(self.NewQueue.AddGetArray2&<TRecord>(len1, len2));

function MemorySegment.GetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): array[,,] of TRecord :=
Context.Default.SyncInvoke(self.NewQueue.AddGetArray3&<TRecord>(len1, len2, len3));

{$endregion Get}

{$endregion Implicit}

{$region Explicit}

{$region 1#Write&Read}

{$region WriteDataAutoSize}

type
  MemorySegmentCommandWriteDataAutoSize = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private ptr: CommandQueue<IntPtr>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>);
    begin
      self.ptr := ptr;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<IntPtr>(ptr.Invoke); evs_l1.Add(ptr_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, o.Size,
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteData(ptr: CommandQueue<IntPtr>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandWriteDataAutoSize(ptr));

{$endregion WriteDataAutoSize}

{$region WriteData}

type
  MemorySegmentCommandWriteData = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private        ptr: CommandQueue<IntPtr>;
    private mem_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 3;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>);
    begin
      self.       ptr :=        ptr;
      self.mem_offset := mem_offset;
      self.       len :=        len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        ptr_qr: QueueRes<IntPtr>;
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 3, invoker->
      begin
               ptr_qr := invoker.InvokeBranch&<IntPtr>(       ptr.Invoke); evs_l1.Add(ptr_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<integer>(       len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        ptr :=        ptr_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len),
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             ptr.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddWriteData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandWriteData(ptr, mem_offset, len));

{$endregion WriteData}

{$region ReadDataAutoSize}

type
  MemorySegmentCommandReadDataAutoSize = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private ptr: CommandQueue<IntPtr>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>);
    begin
      self.ptr := ptr;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<IntPtr>(ptr.Invoke); evs_l1.Add(ptr_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, o.Size,
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadData(ptr: CommandQueue<IntPtr>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandReadDataAutoSize(ptr));

{$endregion ReadDataAutoSize}

{$region ReadData}

type
  MemorySegmentCommandReadData = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private        ptr: CommandQueue<IntPtr>;
    private mem_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 3;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>);
    begin
      self.       ptr :=        ptr;
      self.mem_offset := mem_offset;
      self.       len :=        len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        ptr_qr: QueueRes<IntPtr>;
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 3, invoker->
      begin
               ptr_qr := invoker.InvokeBranch&<IntPtr>(       ptr.Invoke); evs_l1.Add(ptr_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<integer>(       len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        ptr :=        ptr_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len),
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             ptr.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddReadData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandReadData(ptr, mem_offset, len));

{$endregion ReadData}

{$region WriteDataAutoSize}

function MemorySegmentCCQ.AddWriteData(ptr: pointer): MemorySegmentCCQ :=
AddWriteData(IntPtr(ptr));

{$endregion WriteDataAutoSize}

{$region WriteData}

function MemorySegmentCCQ.AddWriteData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ :=
AddWriteData(IntPtr(ptr), mem_offset, len);

{$endregion WriteData}

{$region ReadDataAutoSize}

function MemorySegmentCCQ.AddReadData(ptr: pointer): MemorySegmentCCQ :=
AddReadData(IntPtr(ptr));

{$endregion ReadDataAutoSize}

{$region ReadData}

function MemorySegmentCCQ.AddReadData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ :=
AddReadData(IntPtr(ptr), mem_offset, len);

{$endregion ReadData}

{$region WriteValue}

function MemorySegmentCCQ.AddWriteValue<TRecord>(val: TRecord): MemorySegmentCCQ :=
AddWriteValue(val, 0);

{$endregion WriteValue}

{$region WriteValueQ}

function MemorySegmentCCQ.AddWriteValue<TRecord>(val: CommandQueue<TRecord>): MemorySegmentCCQ :=
AddWriteValue(val, 0);

{$endregion WriteValueQ}

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
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var mem_offset := mem_offset_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr(val),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddWriteValue<TRecord>(val: TRecord; mem_offset: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandWriteValue<TRecord>(val, mem_offset));

{$endregion WriteValue}

{$region WriteValueQ}

type
  MemorySegmentCommandWriteValueQ<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private        val: CommandQueue<TRecord>;
    private mem_offset: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 1;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        val_qr: QueueRes<TRecord>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 2, invoker->
      begin
               val_qr := invoker.InvokeBranch&<TRecord>((g,l)->val.Invoke(g, l.WithPtrNeed( True))); (if val_qr is IQueueResDelayedPtr then evs_l2 else evs_l1).Add(val_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        val :=        val_qr.ToPtr;
        var mem_offset := mem_offset_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr(val.GetPtr),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             val.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddWriteValue<TRecord>(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandWriteValueQ<TRecord>(val, mem_offset));

{$endregion WriteValueQ}

{$region WriteArray1AutoSize}

type
  MemorySegmentCommandWriteArray1AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array of TRecord>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of TRecord>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array of TRecord>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandWriteArray1AutoSize<TRecord>(a));

{$endregion WriteArray1AutoSize}

{$region WriteArray2AutoSize}

type
  MemorySegmentCommandWriteArray2AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,] of TRecord>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,] of TRecord>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array[,] of TRecord>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0,0],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandWriteArray2AutoSize<TRecord>(a));

{$endregion WriteArray2AutoSize}

{$region WriteArray3AutoSize}

type
  MemorySegmentCommandWriteArray3AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,,] of TRecord>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,,] of TRecord>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array[,,] of TRecord>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0,0,0],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandWriteArray3AutoSize<TRecord>(a));

{$endregion WriteArray3AutoSize}

{$region ReadArray1AutoSize}

type
  MemorySegmentCommandReadArray1AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array of TRecord>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of TRecord>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array of TRecord>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandReadArray1AutoSize<TRecord>(a));

{$endregion ReadArray1AutoSize}

{$region ReadArray2AutoSize}

type
  MemorySegmentCommandReadArray2AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,] of TRecord>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,] of TRecord>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array[,] of TRecord>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0,0],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandReadArray2AutoSize<TRecord>(a));

{$endregion ReadArray2AutoSize}

{$region ReadArray3AutoSize}

type
  MemorySegmentCommandReadArray3AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,,] of TRecord>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,,] of TRecord>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array[,,] of TRecord>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          a[0,0,0],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandReadArray3AutoSize<TRecord>(a));

{$endregion ReadArray3AutoSize}

{$region WriteArray1}

type
  MemorySegmentCommandWriteArray1<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private          a: CommandQueue<array of TRecord>;
    private   a_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    private mem_offset: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 4;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array of TRecord>;
      var   a_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 4, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<array of TRecord>(         a.Invoke); evs_l1.Add(a_qr.ev);
          a_offset_qr := invoker.InvokeBranch&<integer>(  a_offset.Invoke); evs_l1.Add(a_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<integer>(       len.Invoke); evs_l1.Add(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var          a :=          a_qr.GetRes;
        var   a_offset :=   a_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddWriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandWriteArray1<TRecord>(a, a_offset, len, mem_offset));

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
    
    public function ParamCountL1: integer; override := 5;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array[,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 5, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<array[,] of TRecord>(         a.Invoke); evs_l1.Add(a_qr.ev);
         a_offset1_qr := invoker.InvokeBranch&<integer>( a_offset1.Invoke); evs_l1.Add(a_offset1_qr.ev);
         a_offset2_qr := invoker.InvokeBranch&<integer>( a_offset2.Invoke); evs_l1.Add(a_offset2_qr.ev);
               len_qr := invoker.InvokeBranch&<integer>(       len.Invoke); evs_l1.Add(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
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
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandWriteArray2<TRecord>(a, a_offset1, a_offset2, len, mem_offset));

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
    
    public function ParamCountL1: integer; override := 6;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array[,,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var  a_offset3_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 6, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<array[,,] of TRecord>(         a.Invoke); evs_l1.Add(a_qr.ev);
         a_offset1_qr := invoker.InvokeBranch&<integer>( a_offset1.Invoke); evs_l1.Add(a_offset1_qr.ev);
         a_offset2_qr := invoker.InvokeBranch&<integer>( a_offset2.Invoke); evs_l1.Add(a_offset2_qr.ev);
         a_offset3_qr := invoker.InvokeBranch&<integer>( a_offset3.Invoke); evs_l1.Add(a_offset3_qr.ev);
               len_qr := invoker.InvokeBranch&<integer>(       len.Invoke); evs_l1.Add(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
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
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2,a_offset3],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandWriteArray3<TRecord>(a, a_offset1, a_offset2, a_offset3, len, mem_offset));

{$endregion WriteArray3}

{$region ReadArray1}

type
  MemorySegmentCommandReadArray1<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private          a: CommandQueue<array of TRecord>;
    private   a_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    private mem_offset: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 4;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array of TRecord>;
      var   a_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 4, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<array of TRecord>(         a.Invoke); evs_l1.Add(a_qr.ev);
          a_offset_qr := invoker.InvokeBranch&<integer>(  a_offset.Invoke); evs_l1.Add(a_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<integer>(       len.Invoke); evs_l1.Add(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var          a :=          a_qr.GetRes;
        var   a_offset :=   a_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var mem_offset := mem_offset_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandReadArray1<TRecord>(a, a_offset, len, mem_offset));

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
    
    public function ParamCountL1: integer; override := 5;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array[,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 5, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<array[,] of TRecord>(         a.Invoke); evs_l1.Add(a_qr.ev);
         a_offset1_qr := invoker.InvokeBranch&<integer>( a_offset1.Invoke); evs_l1.Add(a_offset1_qr.ev);
         a_offset2_qr := invoker.InvokeBranch&<integer>( a_offset2.Invoke); evs_l1.Add(a_offset2_qr.ev);
               len_qr := invoker.InvokeBranch&<integer>(       len.Invoke); evs_l1.Add(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
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
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandReadArray2<TRecord>(a, a_offset1, a_offset2, len, mem_offset));

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
    
    public function ParamCountL1: integer; override := 6;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var          a_qr: QueueRes<array[,,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var  a_offset3_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 6, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<array[,,] of TRecord>(         a.Invoke); evs_l1.Add(a_qr.ev);
         a_offset1_qr := invoker.InvokeBranch&<integer>( a_offset1.Invoke); evs_l1.Add(a_offset1_qr.ev);
         a_offset2_qr := invoker.InvokeBranch&<integer>( a_offset2.Invoke); evs_l1.Add(a_offset2_qr.ev);
         a_offset3_qr := invoker.InvokeBranch&<integer>( a_offset3.Invoke); evs_l1.Add(a_offset3_qr.ev);
               len_qr := invoker.InvokeBranch&<integer>(       len.Invoke); evs_l1.Add(len_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
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
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2,a_offset3],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandReadArray3<TRecord>(a, a_offset1, a_offset2, a_offset3, len, mem_offset));

{$endregion ReadArray3}

{$endregion 1#Write&Read}

{$region 2#Fill}

{$region FillDataAutoSize}

type
  MemorySegmentCommandFillDataAutoSize = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private         ptr: CommandQueue<IntPtr>;
    private pattern_len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 2;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.pattern_len := pattern_len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 2, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<IntPtr>(        ptr.Invoke); evs_l1.Add(ptr_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<integer>(pattern_len.Invoke); evs_l1.Add(pattern_len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
              ptr.RegisterWaitables(g, prev_hubs);
      pattern_len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillDataAutoSize(ptr, pattern_len));

{$endregion FillDataAutoSize}

{$region FillData}

type
  MemorySegmentCommandFillData = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private         ptr: CommandQueue<IntPtr>;
    private pattern_len: CommandQueue<integer>;
    private  mem_offset: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 4;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len, mem_offset, len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.pattern_len := pattern_len;
      self. mem_offset :=  mem_offset;
      self.        len :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 4, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<IntPtr>(        ptr.Invoke); evs_l1.Add(ptr_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<integer>(pattern_len.Invoke); evs_l1.Add(pattern_len_qr.ev);
         mem_offset_qr := invoker.InvokeBranch&<integer>( mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
                len_qr := invoker.InvokeBranch&<integer>(        len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var  mem_offset :=  mem_offset_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillData(ptr, pattern_len, mem_offset, len));

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
    
    public function ParamCountL1: integer; override := 0;
    public function ParamCountL2: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(val: TRecord);
    begin
      self.val^ := val;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      
      Result := (o, cq, err_handler, evs)->
      begin
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val^);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillValue<TRecord>(val: TRecord): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillValueAutoSize<TRecord>(val));

{$endregion FillValueAutoSize}

{$region FillValueAutoSizeQ}

type
  MemorySegmentCommandFillValueAutoSizeQ<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private val: CommandQueue<TRecord>;
    
    public function ParamCountL1: integer; override := 0;
    public function ParamCountL2: integer; override := 1;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(val: CommandQueue<TRecord>);
    begin
      self.val := val;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var val_qr: QueueRes<TRecord>;
      g.ParallelInvoke(l.WithPtrNeed(true), true, 1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<TRecord>(val.Invoke); (if val_qr is IQueueResDelayedPtr then evs_l2 else evs_l1).Add(val_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var val := val_qr.ToPtr;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val.GetPtr), new UIntPtr(Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillValue<TRecord>(val: CommandQueue<TRecord>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillValueAutoSizeQ<TRecord>(val));

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
    
    public function ParamCountL1: integer; override := 2;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 2, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<integer>(       len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var mem_offset := mem_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new UIntPtr(mem_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddFillValue<TRecord>(val: TRecord; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillValue<TRecord>(val, mem_offset, len));

{$endregion FillValue}

{$region FillValueQ}

type
  MemorySegmentCommandFillValueQ<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private        val: CommandQueue<TRecord>;
    private mem_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 2;
    public function ParamCountL2: integer; override := 1;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        val_qr: QueueRes<TRecord>;
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 3, invoker->
      begin
               val_qr := invoker.InvokeBranch&<TRecord>((g,l)->val.Invoke(g, l.WithPtrNeed( True))); (if val_qr is IQueueResDelayedPtr then evs_l2 else evs_l1).Add(val_qr.ev);
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<integer>(       len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        val :=        val_qr.ToPtr;
        var mem_offset := mem_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val.GetPtr), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new UIntPtr(mem_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             val.RegisterWaitables(g, prev_hubs);
      mem_offset.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddFillValue<TRecord>(val: CommandQueue<TRecord>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillValueQ<TRecord>(val, mem_offset, len));

{$endregion FillValueQ}

{$region FillArray1AutoSize}

type
  MemorySegmentCommandFillArray1AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array of TRecord>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of TRecord>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array of TRecord>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        cl.EnqueueFillBuffer(
          cq, o.Native,
          a[0], new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillArray1AutoSize<TRecord>(a));

{$endregion FillArray1AutoSize}

{$region FillArray2AutoSize}

type
  MemorySegmentCommandFillArray2AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,] of TRecord>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,] of TRecord>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array[,] of TRecord>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        cl.EnqueueFillBuffer(
          cq, o.Native,
          a[0,0], new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillArray2AutoSize<TRecord>(a));

{$endregion FillArray2AutoSize}

{$region FillArray3AutoSize}

type
  MemorySegmentCommandFillArray3AutoSize<TRecord> = sealed class(EnqueueableGPUCommand<MemorySegment>)
  where TRecord: record;
    private a: CommandQueue<array[,,] of TRecord>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'записывать в область памяти OpenCL');
    end;
    public constructor(a: CommandQueue<array[,,] of TRecord>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array[,,] of TRecord>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array[,,] of TRecord>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        //TODO unable to merge this Enqueue with non-AutoSize, because %rank would be nested in %AutoSize
        cl.EnqueueFillBuffer(
          cq, o.Native,
          a[0,0,0], new UIntPtr(a.Length*Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddFillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillArray3AutoSize<TRecord>(a));

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
    
    public function ParamCountL1: integer; override := 5;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var           a_qr: QueueRes<array of TRecord>;
      var    a_offset_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 5, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<array of TRecord>(          a.Invoke); evs_l1.Add(a_qr.ev);
           a_offset_qr := invoker.InvokeBranch&<integer>(   a_offset.Invoke); evs_l1.Add(a_offset_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<integer>(pattern_len.Invoke); evs_l1.Add(pattern_len_qr.ev);
                len_qr := invoker.InvokeBranch&<integer>(        len.Invoke); evs_l1.Add(len_qr.ev);
         mem_offset_qr := invoker.InvokeBranch&<integer>( mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
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
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset], new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddFillArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillArray1<TRecord>(a, a_offset, pattern_len, len, mem_offset));

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
    
    public function ParamCountL1: integer; override := 6;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var           a_qr: QueueRes<array[,] of TRecord>;
      var   a_offset1_qr: QueueRes<integer>;
      var   a_offset2_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 6, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<array[,] of TRecord>(          a.Invoke); evs_l1.Add(a_qr.ev);
          a_offset1_qr := invoker.InvokeBranch&<integer>(  a_offset1.Invoke); evs_l1.Add(a_offset1_qr.ev);
          a_offset2_qr := invoker.InvokeBranch&<integer>(  a_offset2.Invoke); evs_l1.Add(a_offset2_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<integer>(pattern_len.Invoke); evs_l1.Add(pattern_len_qr.ev);
                len_qr := invoker.InvokeBranch&<integer>(        len.Invoke); evs_l1.Add(len_qr.ev);
         mem_offset_qr := invoker.InvokeBranch&<integer>( mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
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
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset1,a_offset2], new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddFillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillArray2<TRecord>(a, a_offset1, a_offset2, pattern_len, len, mem_offset));

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
    
    public function ParamCountL1: integer; override := 7;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var           a_qr: QueueRes<array[,,] of TRecord>;
      var   a_offset1_qr: QueueRes<integer>;
      var   a_offset2_qr: QueueRes<integer>;
      var   a_offset3_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 7, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<array[,,] of TRecord>(          a.Invoke); evs_l1.Add(a_qr.ev);
          a_offset1_qr := invoker.InvokeBranch&<integer>(  a_offset1.Invoke); evs_l1.Add(a_offset1_qr.ev);
          a_offset2_qr := invoker.InvokeBranch&<integer>(  a_offset2.Invoke); evs_l1.Add(a_offset2_qr.ev);
          a_offset3_qr := invoker.InvokeBranch&<integer>(  a_offset3.Invoke); evs_l1.Add(a_offset3_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<integer>(pattern_len.Invoke); evs_l1.Add(pattern_len_qr.ev);
                len_qr := invoker.InvokeBranch&<integer>(        len.Invoke); evs_l1.Add(len_qr.ev);
         mem_offset_qr := invoker.InvokeBranch&<integer>( mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
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
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset1,a_offset2,a_offset3], new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddFillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandFillArray3<TRecord>(a, a_offset1, a_offset2, a_offset3, pattern_len, len, mem_offset));

{$endregion FillArray3}

{$endregion 2#Fill}

{$region 3#Copy}

{$region CopyToAutoSize}

type
  MemorySegmentCommandCopyToAutoSize = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private mem: CommandQueue<MemorySegment>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(mem: CommandQueue<MemorySegment>);
    begin
      self.mem := mem;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var mem_qr: QueueRes<MemorySegment>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        mem_qr := invoker.InvokeBranch&<MemorySegment>(mem.Invoke); evs_l1.Add(mem_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var mem := mem_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, o.Native,mem.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          o.Size64<mem.Size64 ? o.Size : mem.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem: ';
      mem.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddCopyTo(mem: CommandQueue<MemorySegment>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandCopyToAutoSize(mem));

{$endregion CopyToAutoSize}

{$region CopyTo}

type
  MemorySegmentCommandCopyTo = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private      mem: CommandQueue<MemorySegment>;
    private from_pos: CommandQueue<integer>;
    private   to_pos: CommandQueue<integer>;
    private      len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 4;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>);
    begin
      self.     mem :=      mem;
      self.from_pos := from_pos;
      self.  to_pos :=   to_pos;
      self.     len :=      len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var      mem_qr: QueueRes<MemorySegment>;
      var from_pos_qr: QueueRes<integer>;
      var   to_pos_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 4, invoker->
      begin
             mem_qr := invoker.InvokeBranch&<MemorySegment>(     mem.Invoke); evs_l1.Add(mem_qr.ev);
        from_pos_qr := invoker.InvokeBranch&<integer>(from_pos.Invoke); evs_l1.Add(from_pos_qr.ev);
          to_pos_qr := invoker.InvokeBranch&<integer>(  to_pos.Invoke); evs_l1.Add(to_pos_qr.ev);
             len_qr := invoker.InvokeBranch&<integer>(     len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var      mem :=      mem_qr.GetRes;
        var from_pos := from_pos_qr.GetRes;
        var   to_pos :=   to_pos_qr.GetRes;
        var      len :=      len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, o.Native,mem.Native,
          new UIntPtr(from_pos), new UIntPtr(to_pos),
          new UIntPtr(len),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddCopyTo(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandCopyTo(mem, from_pos, to_pos, len));

{$endregion CopyTo}

{$region CopyFromAutoSize}

type
  MemorySegmentCommandCopyFromAutoSize = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private mem: CommandQueue<MemorySegment>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(mem: CommandQueue<MemorySegment>);
    begin
      self.mem := mem;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var mem_qr: QueueRes<MemorySegment>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        mem_qr := invoker.InvokeBranch&<MemorySegment>(mem.Invoke); evs_l1.Add(mem_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var mem := mem_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, mem.Native,o.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          o.Size64<mem.Size64 ? o.Size : mem.Size,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem: ';
      mem.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddCopyFrom(mem: CommandQueue<MemorySegment>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandCopyFromAutoSize(mem));

{$endregion CopyFromAutoSize}

{$region CopyFrom}

type
  MemorySegmentCommandCopyFrom = sealed class(EnqueueableGPUCommand<MemorySegment>)
    private      mem: CommandQueue<MemorySegment>;
    private from_pos: CommandQueue<integer>;
    private   to_pos: CommandQueue<integer>;
    private      len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 4;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>);
    begin
      self.     mem :=      mem;
      self.from_pos := from_pos;
      self.  to_pos :=   to_pos;
      self.     len :=      len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var      mem_qr: QueueRes<MemorySegment>;
      var from_pos_qr: QueueRes<integer>;
      var   to_pos_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 4, invoker->
      begin
             mem_qr := invoker.InvokeBranch&<MemorySegment>(     mem.Invoke); evs_l1.Add(mem_qr.ev);
        from_pos_qr := invoker.InvokeBranch&<integer>(from_pos.Invoke); evs_l1.Add(from_pos_qr.ev);
          to_pos_qr := invoker.InvokeBranch&<integer>(  to_pos.Invoke); evs_l1.Add(to_pos_qr.ev);
             len_qr := invoker.InvokeBranch&<integer>(     len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var      mem :=      mem_qr.GetRes;
        var from_pos := from_pos_qr.GetRes;
        var   to_pos :=   to_pos_qr.GetRes;
        var      len :=      len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, mem.Native,o.Native,
          new UIntPtr(from_pos), new UIntPtr(to_pos),
          new UIntPtr(len),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddCopyFrom(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegmentCCQ :=
AddCommand(self, new MemorySegmentCommandCopyFrom(mem, from_pos, to_pos, len));

{$endregion CopyFrom}

{$endregion 3#Copy}

{$region Get}

{$region GetDataAutoSize}

type
  MemorySegmentCommandGetDataAutoSize = sealed class(EnqueueableGetCommand<MemorySegment, IntPtr>)
    
    public function ParamCountL1: integer; override := 0;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: MemorySegmentCCQ);
    begin
      inherited Create(ccq);
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<IntPtr>)->cl_event; override;
    begin
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var res := Marshal.AllocHGlobal(IntPtr(pointer(o.Size)));;
        own_qr.SetRes(res);
        var res_ev: cl_event;
        
        //TODO А что если результат уже получен и освобождёт сдедующей .ThenConvert
        // - Вообще .WhenError тут (и в +1 месте) - говнокод
        // - Лучше стоит сделать обёртку вроде SafeIntPtr (или использовать готовую)
        //tsk.WhenErrorBase((tsk,err)->Marshal.FreeHGlobal(res));
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, o.Size,
          res,
          evs.count, evs.evs, res_ev
        );
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override := sb += #10;
    
  end;
  
function MemorySegmentCCQ.AddGetData: CommandQueue<IntPtr> :=
new MemorySegmentCommandGetDataAutoSize(self) as CommandQueue<IntPtr>;

{$endregion GetDataAutoSize}

{$region GetData}

type
  MemorySegmentCommandGetData = sealed class(EnqueueableGetCommand<MemorySegment, IntPtr>)
    private mem_offset: CommandQueue<integer>;
    private        len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 2;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: MemorySegmentCCQ; mem_offset, len: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.mem_offset := mem_offset;
      self.       len :=        len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<IntPtr>)->cl_event; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(l.WithPtrNeed(False), true, 2, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
               len_qr := invoker.InvokeBranch&<integer>(       len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var mem_offset := mem_offset_qr.GetRes;
        var        len :=        len_qr.GetRes;
        var res := Marshal.AllocHGlobal(len);
        own_qr.SetRes(res);
        var res_ev: cl_event;
        
        //tsk.WhenErrorBase((tsk,err)->Marshal.FreeHGlobal(res));
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len),
          res,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.RegisterWaitables(g, prev_hubs);
             len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddGetData(mem_offset, len: CommandQueue<integer>): CommandQueue<IntPtr> :=
new MemorySegmentCommandGetData(self, mem_offset, len) as CommandQueue<IntPtr>;

{$endregion GetData}

{$region GetValue}

function MemorySegmentCCQ.AddGetValue<TRecord>: CommandQueue<TRecord> :=
AddGetValue&<TRecord>(0);

{$endregion GetValue}

{$region GetValue}

type
  MemorySegmentCommandGetValue<TRecord> = sealed class(EnqueueableGetCommand<MemorySegment, TRecord>)
  where TRecord: record;
    private mem_offset: CommandQueue<integer>;
    
    public function ForcePtrQr: boolean; override := true;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<TRecord>)->cl_event; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(l.WithPtrNeed(False), true, 1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<integer>(mem_offset.Invoke); evs_l1.Add(mem_offset_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var mem_offset := mem_offset_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr((own_qr as QueueResDelayedPtr<TRecord>).ptr),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var own_qr_hnd := GCHandle.Alloc(own_qr);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          own_qr_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [own_qr]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddGetValue<TRecord>(mem_offset: CommandQueue<integer>): CommandQueue<TRecord> :=
new MemorySegmentCommandGetValue<TRecord>(self, mem_offset) as CommandQueue<TRecord>;

{$endregion GetValue}

{$region GetArray1AutoSize}

type
  MemorySegmentCommandGetArray1AutoSize<TRecord> = sealed class(EnqueueableGetCommand<MemorySegment, array of TRecord>)
  where TRecord: record;
    
    public function ParamCountL1: integer; override := 0;
    public function ParamCountL2: integer; override := 0;
    
    static constructor;
    begin
      BlittableHelper.RaiseIfBad(typeof(TRecord), 'читать из области памяти OpenCL');
    end;
    public constructor(ccq: MemorySegmentCCQ);
    begin
      inherited Create(ccq);
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array of TRecord>)->cl_event; override;
    begin
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var res := new TRecord[o.Size64 div Marshal.SizeOf&<TRecord>];;
        own_qr.SetRes(res);
        var res_hnd := GCHandle.Alloc(res, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(res.Length * Marshal.SizeOf&<TRecord>),
          res_hnd.AddrOfPinnedObject,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override := sb += #10;
    
  end;
  
function MemorySegmentCCQ.AddGetArray1<TRecord>: CommandQueue<array of TRecord> :=
new MemorySegmentCommandGetArray1AutoSize<TRecord>(self) as CommandQueue<array of TRecord>;

{$endregion GetArray1AutoSize}

{$region GetArray1}

type
  MemorySegmentCommandGetArray1<TRecord> = sealed class(EnqueueableGetCommand<MemorySegment, array of TRecord>)
  where TRecord: record;
    private len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array of TRecord>)->cl_event; override;
    begin
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(l.WithPtrNeed(False), true, 1, invoker->
      begin
        len_qr := invoker.InvokeBranch&<integer>(len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var len := len_qr.GetRes;
        var res := new TRecord[len];
        own_qr.SetRes(res);
        var res_hnd := GCHandle.Alloc(res, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(int64(len) * Marshal.SizeOf&<TRecord>),
          res[0],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.AddGetArray1<TRecord>(len: CommandQueue<integer>): CommandQueue<array of TRecord> :=
new MemorySegmentCommandGetArray1<TRecord>(self, len) as CommandQueue<array of TRecord>;

{$endregion GetArray1}

{$region GetArray2}

type
  MemorySegmentCommandGetArray2<TRecord> = sealed class(EnqueueableGetCommand<MemorySegment, array[,] of TRecord>)
  where TRecord: record;
    private len1: CommandQueue<integer>;
    private len2: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 2;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array[,] of TRecord>)->cl_event; override;
    begin
      var len1_qr: QueueRes<integer>;
      var len2_qr: QueueRes<integer>;
      g.ParallelInvoke(l.WithPtrNeed(False), true, 2, invoker->
      begin
        len1_qr := invoker.InvokeBranch&<integer>(len1.Invoke); evs_l1.Add(len1_qr.ev);
        len2_qr := invoker.InvokeBranch&<integer>(len2.Invoke); evs_l1.Add(len2_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var len1 := len1_qr.GetRes;
        var len2 := len2_qr.GetRes;
        var res := new TRecord[len1,len2];
        own_qr.SetRes(res);
        var res_hnd := GCHandle.Alloc(res, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(int64(len1)*len2 * Marshal.SizeOf&<TRecord>),
          res[0,0],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      len1.RegisterWaitables(g, prev_hubs);
      len2.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddGetArray2<TRecord>(len1,len2: CommandQueue<integer>): CommandQueue<array[,] of TRecord> :=
new MemorySegmentCommandGetArray2<TRecord>(self, len1, len2) as CommandQueue<array[,] of TRecord>;

{$endregion GetArray2}

{$region GetArray3}

type
  MemorySegmentCommandGetArray3<TRecord> = sealed class(EnqueueableGetCommand<MemorySegment, array[,,] of TRecord>)
  where TRecord: record;
    private len1: CommandQueue<integer>;
    private len2: CommandQueue<integer>;
    private len3: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 3;
    public function ParamCountL2: integer; override := 0;
    
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (MemorySegment, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array[,,] of TRecord>)->cl_event; override;
    begin
      var len1_qr: QueueRes<integer>;
      var len2_qr: QueueRes<integer>;
      var len3_qr: QueueRes<integer>;
      g.ParallelInvoke(l.WithPtrNeed(False), true, 3, invoker->
      begin
        len1_qr := invoker.InvokeBranch&<integer>(len1.Invoke); evs_l1.Add(len1_qr.ev);
        len2_qr := invoker.InvokeBranch&<integer>(len2.Invoke); evs_l1.Add(len2_qr.ev);
        len3_qr := invoker.InvokeBranch&<integer>(len3.Invoke); evs_l1.Add(len3_qr.ev);
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
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(int64(len1)*len2*len3 * Marshal.SizeOf&<TRecord>),
          res[0,0,0],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      len1.RegisterWaitables(g, prev_hubs);
      len2.RegisterWaitables(g, prev_hubs);
      len3.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function MemorySegmentCCQ.AddGetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): CommandQueue<array[,,] of TRecord> :=
new MemorySegmentCommandGetArray3<TRecord>(self, len1, len2, len3) as CommandQueue<array[,,] of TRecord>;

{$endregion GetArray3}

{$endregion Get}

{$endregion Explicit}

{$endregion MemorySegment}

{$region CLArray}

{$region Implicit}

{$region 1#Write&Read}

function CLArray<T>.WriteData(ptr: CommandQueue<IntPtr>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteData(ptr));

function CLArray<T>.WriteData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteData(ptr, ind, len));

function CLArray<T>.ReadData(ptr: CommandQueue<IntPtr>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddReadData(ptr));

function CLArray<T>.ReadData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddReadData(ptr, ind, len));

function CLArray<T>.WriteData(ptr: pointer): CLArray<T> :=
WriteData(IntPtr(ptr));

function CLArray<T>.WriteData(ptr: pointer; ind, len: CommandQueue<integer>): CLArray<T> :=
WriteData(IntPtr(ptr), ind, len);

function CLArray<T>.ReadData(ptr: pointer): CLArray<T> :=
ReadData(IntPtr(ptr));

function CLArray<T>.ReadData(ptr: pointer; ind, len: CommandQueue<integer>): CLArray<T> :=
ReadData(IntPtr(ptr), ind, len);

function CLArray<T>.WriteValue(val: &T; ind: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteValue(val, ind));

function CLArray<T>.WriteValue(val: CommandQueue<&T>; ind: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteValue(val, ind));

function CLArray<T>.WriteArray(a: CommandQueue<array of &T>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray(a));

function CLArray<T>.WriteArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddWriteArray(a, ind, len, a_ind));

function CLArray<T>.ReadArray(a: CommandQueue<array of &T>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray(a));

function CLArray<T>.ReadArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddReadArray(a, ind, len, a_ind));

{$endregion 1#Write&Read}

{$region 2#Fill}

function CLArray<T>.FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddFillData(ptr, pattern_len));

function CLArray<T>.FillData(ptr: CommandQueue<IntPtr>; pattern_len, ind, len: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddFillData(ptr, pattern_len, ind, len));

function CLArray<T>.FillData(ptr: pointer; pattern_len: CommandQueue<integer>): CLArray<T> :=
FillData(IntPtr(ptr), pattern_len);

function CLArray<T>.FillData(ptr: pointer; pattern_len, ind, len: CommandQueue<integer>): CLArray<T> :=
FillData(IntPtr(ptr), pattern_len, ind, len);

function CLArray<T>.FillValue(val: &T): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue(val));

function CLArray<T>.FillValue(val: CommandQueue<&T>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue(val));

function CLArray<T>.FillValue(val: &T; ind, len: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue(val, ind, len));

function CLArray<T>.FillValue(val: CommandQueue<&T>; ind, len: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddFillValue(val, ind, len));

function CLArray<T>.FillArray(a: CommandQueue<array of &T>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddFillArray(a));

function CLArray<T>.FillArray(a: CommandQueue<array of &T>; a_offset,pattern_len, ind,len: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddFillArray(a, a_offset, pattern_len, ind, len));

{$endregion 2#Fill}

{$region 3#Copy}

function CLArray<T>.CopyTo(a: CommandQueue<CLArray<T>>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyTo(a));

function CLArray<T>.CopyTo(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyTo(a, from_ind, to_ind, len));

function CLArray<T>.CopyFrom(a: CommandQueue<CLArray<T>>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyFrom(a));

function CLArray<T>.CopyFrom(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArray<T> :=
Context.Default.SyncInvoke(self.NewQueue.AddCopyFrom(a, from_ind, to_ind, len));

{$endregion 3#Copy}

{$region Get}

function CLArray<T>.GetValue(ind: CommandQueue<integer>): &T :=
Context.Default.SyncInvoke(self.NewQueue.AddGetValue(ind));

function CLArray<T>.GetArray: array of &T :=
Context.Default.SyncInvoke(self.NewQueue.AddGetArray);

function CLArray<T>.GetArray(ind, len: CommandQueue<integer>): array of &T :=
Context.Default.SyncInvoke(self.NewQueue.AddGetArray(ind, len));

{$endregion Get}

{$endregion Implicit}

{$region Explicit}

{$region 1#Write&Read}

{$region WriteDataAutoSize}

type
  CLArrayCommandWriteDataAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private ptr: CommandQueue<IntPtr>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>);
    begin
      self.ptr := ptr;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<IntPtr>(ptr.Invoke); evs_l1.Add(ptr_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(o.ByteSize),
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddWriteData(ptr: CommandQueue<IntPtr>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandWriteDataAutoSize<T>(ptr));

{$endregion WriteDataAutoSize}

{$region WriteData}

type
  CLArrayCommandWriteData<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private ptr: CommandQueue<IntPtr>;
    private ind: CommandQueue<integer>;
    private len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 3;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>);
    begin
      self.ptr := ptr;
      self.ind := ind;
      self.len := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 3, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<IntPtr>(ptr.Invoke); evs_l1.Add(ptr_qr.ev);
        ind_qr := invoker.InvokeBranch&<integer>(ind.Invoke); evs_l1.Add(ind_qr.ev);
        len_qr := invoker.InvokeBranch&<integer>(len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var ind := ind_qr.GetRes;
        var len := len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
      ind.RegisterWaitables(g, prev_hubs);
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddWriteData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandWriteData<T>(ptr, ind, len));

{$endregion WriteData}

{$region ReadDataAutoSize}

type
  CLArrayCommandReadDataAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private ptr: CommandQueue<IntPtr>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>);
    begin
      self.ptr := ptr;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<IntPtr>(ptr.Invoke); evs_l1.Add(ptr_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(o.ByteSize),
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddReadData(ptr: CommandQueue<IntPtr>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandReadDataAutoSize<T>(ptr));

{$endregion ReadDataAutoSize}

{$region ReadData}

type
  CLArrayCommandReadData<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private ptr: CommandQueue<IntPtr>;
    private ind: CommandQueue<integer>;
    private len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 3;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>);
    begin
      self.ptr := ptr;
      self.ind := ind;
      self.len := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 3, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<IntPtr>(ptr.Invoke); evs_l1.Add(ptr_qr.ev);
        ind_qr := invoker.InvokeBranch&<integer>(ind.Invoke); evs_l1.Add(ind_qr.ev);
        len_qr := invoker.InvokeBranch&<integer>(len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ptr := ptr_qr.GetRes;
        var ind := ind_qr.GetRes;
        var len := len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          ptr,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.RegisterWaitables(g, prev_hubs);
      ind.RegisterWaitables(g, prev_hubs);
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddReadData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandReadData<T>(ptr, ind, len));

{$endregion ReadData}

{$region WriteDataAutoSize}

function CLArrayCCQ<T>.AddWriteData(ptr: pointer): CLArrayCCQ<T> :=
AddWriteData(IntPtr(ptr));

{$endregion WriteDataAutoSize}

{$region WriteData}

function CLArrayCCQ<T>.AddWriteData(ptr: pointer; ind, len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddWriteData(IntPtr(ptr), ind, len);

{$endregion WriteData}

{$region ReadDataAutoSize}

function CLArrayCCQ<T>.AddReadData(ptr: pointer): CLArrayCCQ<T> :=
AddReadData(IntPtr(ptr));

{$endregion ReadDataAutoSize}

{$region ReadData}

function CLArrayCCQ<T>.AddReadData(ptr: pointer; ind, len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddReadData(IntPtr(ptr), ind, len);

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
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(val: &T; ind: CommandQueue<integer>);
    begin
      self.val^ := val;
      self.ind  := ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<integer>(ind.Invoke); evs_l1.Add(ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ind := ind_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          new IntPtr(val),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddWriteValue(val: &T; ind: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandWriteValue<T>(val, ind));

{$endregion WriteValue}

{$region WriteValueQ}

type
  CLArrayCommandWriteValueQ<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: CommandQueue<&T>;
    private ind: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 1;
    
    public constructor(val: CommandQueue<&T>; ind: CommandQueue<integer>);
    begin
      self.val := val;
      self.ind := ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var val_qr: QueueRes<&T>;
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 2, invoker->
      begin
        val_qr := invoker.InvokeBranch&<&T>((g,l)->val.Invoke(g, l.WithPtrNeed( True))); (if val_qr is IQueueResDelayedPtr then evs_l2 else evs_l1).Add(val_qr.ev);
        ind_qr := invoker.InvokeBranch&<integer>(ind.Invoke); evs_l1.Add(ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var val := val_qr.ToPtr;
        var ind := ind_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          new IntPtr(val.GetPtr),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.RegisterWaitables(g, prev_hubs);
      ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddWriteValue(val: CommandQueue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandWriteValueQ<T>(val, ind));

{$endregion WriteValueQ}

{$region WriteArrayAutoSize}

type
  CLArrayCommandWriteArrayAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private a: CommandQueue<array of &T>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array of &T>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of &T>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array of &T>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<T>),
          a[0],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddWriteArray(a: CommandQueue<array of &T>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandWriteArrayAutoSize<T>(a));

{$endregion WriteArrayAutoSize}

{$region WriteArray}

type
  CLArrayCommandWriteArray<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private     a: CommandQueue<array of &T>;
    private   ind: CommandQueue<integer>;
    private   len: CommandQueue<integer>;
    private a_ind: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 4;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>);
    begin
      self.    a :=     a;
      self.  ind :=   ind;
      self.  len :=   len;
      self.a_ind := a_ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var     a_qr: QueueRes<array of &T>;
      var   ind_qr: QueueRes<integer>;
      var   len_qr: QueueRes<integer>;
      var a_ind_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 4, invoker->
      begin
            a_qr := invoker.InvokeBranch&<array of &T>(    a.Invoke); evs_l1.Add(a_qr.ev);
          ind_qr := invoker.InvokeBranch&<integer>(  ind.Invoke); evs_l1.Add(ind_qr.ev);
          len_qr := invoker.InvokeBranch&<integer>(  len.Invoke); evs_l1.Add(len_qr.ev);
        a_ind_qr := invoker.InvokeBranch&<integer>(a_ind.Invoke); evs_l1.Add(a_ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var     a :=     a_qr.GetRes;
        var   ind :=   ind_qr.GetRes;
        var   len :=   len_qr.GetRes;
        var a_ind := a_ind_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind*Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          a[a_ind],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddWriteArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandWriteArray<T>(a, ind, len, a_ind));

{$endregion WriteArray}

{$region ReadArrayAutoSize}

type
  CLArrayCommandReadArrayAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private a: CommandQueue<array of &T>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array of &T>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of &T>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array of &T>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<T>),
          a[0],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddReadArray(a: CommandQueue<array of &T>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandReadArrayAutoSize<T>(a));

{$endregion ReadArrayAutoSize}

{$region ReadArray}

type
  CLArrayCommandReadArray<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private     a: CommandQueue<array of &T>;
    private   ind: CommandQueue<integer>;
    private   len: CommandQueue<integer>;
    private a_ind: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 4;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>);
    begin
      self.    a :=     a;
      self.  ind :=   ind;
      self.  len :=   len;
      self.a_ind := a_ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var     a_qr: QueueRes<array of &T>;
      var   ind_qr: QueueRes<integer>;
      var   len_qr: QueueRes<integer>;
      var a_ind_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 4, invoker->
      begin
            a_qr := invoker.InvokeBranch&<array of &T>(    a.Invoke); evs_l1.Add(a_qr.ev);
          ind_qr := invoker.InvokeBranch&<integer>(  ind.Invoke); evs_l1.Add(ind_qr.ev);
          len_qr := invoker.InvokeBranch&<integer>(  len.Invoke); evs_l1.Add(len_qr.ev);
        a_ind_qr := invoker.InvokeBranch&<integer>(a_ind.Invoke); evs_l1.Add(a_ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var     a :=     a_qr.GetRes;
        var   ind :=   ind_qr.GetRes;
        var   len :=   len_qr.GetRes;
        var a_ind := a_ind_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind*Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          a[a_ind],
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddReadArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandReadArray<T>(a, ind, len, a_ind));

{$endregion ReadArray}

{$endregion 1#Write&Read}

{$region 2#Fill}

{$region FillDataAutoSize}

type
  CLArrayCommandFillDataAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private         ptr: CommandQueue<IntPtr>;
    private pattern_len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 2;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.pattern_len := pattern_len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 2, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<IntPtr>(        ptr.Invoke); evs_l1.Add(ptr_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<integer>(pattern_len.Invoke); evs_l1.Add(pattern_len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len*Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
              ptr.RegisterWaitables(g, prev_hubs);
      pattern_len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandFillDataAutoSize<T>(ptr, pattern_len));

{$endregion FillDataAutoSize}

{$region FillData}

type
  CLArrayCommandFillData<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private         ptr: CommandQueue<IntPtr>;
    private pattern_len: CommandQueue<integer>;
    private         ind: CommandQueue<integer>;
    private         len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 4;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len, ind, len: CommandQueue<integer>);
    begin
      self.        ptr :=         ptr;
      self.pattern_len := pattern_len;
      self.        ind :=         ind;
      self.        len :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      var         ind_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 4, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<IntPtr>(        ptr.Invoke); evs_l1.Add(ptr_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<integer>(pattern_len.Invoke); evs_l1.Add(pattern_len_qr.ev);
                ind_qr := invoker.InvokeBranch&<integer>(        ind.Invoke); evs_l1.Add(ind_qr.ev);
                len_qr := invoker.InvokeBranch&<integer>(        len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var         ptr :=         ptr_qr.GetRes;
        var pattern_len := pattern_len_qr.GetRes;
        var         ind :=         ind_qr.GetRes;
        var         len :=         len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len*Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, ind, len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandFillData<T>(ptr, pattern_len, ind, len));

{$endregion FillData}

{$region FillDataAutoSize}

function CLArrayCCQ<T>.AddFillData(ptr: pointer; pattern_len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddFillData(IntPtr(ptr), pattern_len);

{$endregion FillDataAutoSize}

{$region FillData}

function CLArrayCCQ<T>.AddFillData(ptr: pointer; pattern_len, ind, len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddFillData(IntPtr(ptr), pattern_len, ind, len);

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
    
    public function ParamCountL1: integer; override := 0;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(val: &T);
    begin
      self.val^ := val;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      
      Result := (o, cq, err_handler, evs)->
      begin
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val^);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddFillValue(val: &T): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandFillValueAutoSize<T>(val));

{$endregion FillValueAutoSize}

{$region FillValueAutoSizeQ}

type
  CLArrayCommandFillValueAutoSizeQ<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: CommandQueue<&T>;
    
    public function ParamCountL1: integer; override := 0;
    public function ParamCountL2: integer; override := 1;
    
    public constructor(val: CommandQueue<&T>);
    begin
      self.val := val;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var val_qr: QueueRes<&T>;
      g.ParallelInvoke(l.WithPtrNeed(true), true, 1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<&T>(val.Invoke); (if val_qr is IQueueResDelayedPtr then evs_l2 else evs_l1).Add(val_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var val := val_qr.ToPtr;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val.GetPtr), new UIntPtr(Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddFillValue(val: CommandQueue<&T>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandFillValueAutoSizeQ<T>(val));

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
    
    public function ParamCountL1: integer; override := 2;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(val: &T; ind, len: CommandQueue<integer>);
    begin
      self.val^ := val;
      self.ind  := ind;
      self.len  := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 2, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<integer>(ind.Invoke); evs_l1.Add(ind_qr.ev);
        len_qr := invoker.InvokeBranch&<integer>(len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var ind := ind_qr.GetRes;
        var len := len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.RegisterWaitables(g, prev_hubs);
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddFillValue(val: &T; ind, len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandFillValue<T>(val, ind, len));

{$endregion FillValue}

{$region FillValueQ}

type
  CLArrayCommandFillValueQ<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private val: CommandQueue<&T>;
    private ind: CommandQueue<integer>;
    private len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 2;
    public function ParamCountL2: integer; override := 1;
    
    public constructor(val: CommandQueue<&T>; ind, len: CommandQueue<integer>);
    begin
      self.val := val;
      self.ind := ind;
      self.len := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var val_qr: QueueRes<&T>;
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 3, invoker->
      begin
        val_qr := invoker.InvokeBranch&<&T>((g,l)->val.Invoke(g, l.WithPtrNeed( True))); (if val_qr is IQueueResDelayedPtr then evs_l2 else evs_l1).Add(val_qr.ev);
        ind_qr := invoker.InvokeBranch&<integer>(ind.Invoke); evs_l1.Add(ind_qr.ev);
        len_qr := invoker.InvokeBranch&<integer>(len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var val := val_qr.ToPtr;
        var ind := ind_qr.GetRes;
        var len := len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val.GetPtr), new UIntPtr(Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var val_hnd := GCHandle.Alloc(val);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          val_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [val]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.RegisterWaitables(g, prev_hubs);
      ind.RegisterWaitables(g, prev_hubs);
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddFillValue(val: CommandQueue<&T>; ind, len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandFillValueQ<T>(val, ind, len));

{$endregion FillValueQ}

{$region FillArrayAutoSize}

type
  CLArrayCommandFillArrayAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private a: CommandQueue<array of &T>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array of &T>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<array of &T>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<array of &T>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          a[0], new UIntPtr(a.Length*Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddFillArray(a: CommandQueue<array of &T>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandFillArrayAutoSize<T>(a));

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
    
    public function ParamCountL1: integer; override := 5;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<array of &T>; a_offset,pattern_len, ind,len: CommandQueue<integer>);
    begin
      self.          a :=           a;
      self.   a_offset :=    a_offset;
      self.pattern_len := pattern_len;
      self.        ind :=         ind;
      self.        len :=         len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var           a_qr: QueueRes<array of &T>;
      var    a_offset_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         ind_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 5, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<array of &T>(          a.Invoke); evs_l1.Add(a_qr.ev);
           a_offset_qr := invoker.InvokeBranch&<integer>(   a_offset.Invoke); evs_l1.Add(a_offset_qr.ev);
        pattern_len_qr := invoker.InvokeBranch&<integer>(pattern_len.Invoke); evs_l1.Add(pattern_len_qr.ev);
                ind_qr := invoker.InvokeBranch&<integer>(        ind.Invoke); evs_l1.Add(ind_qr.ev);
                len_qr := invoker.InvokeBranch&<integer>(        len.Invoke); evs_l1.Add(len_qr.ev);
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
        
        cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset], new UIntPtr(pattern_len*Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          a_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [a]'{$endif});
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddFillArray(a: CommandQueue<array of &T>; a_offset,pattern_len, ind,len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandFillArray<T>(a, a_offset, pattern_len, ind, len));

{$endregion FillArray}

{$endregion 2#Fill}

{$region 3#Copy}

{$region CopyToAutoSize}

type
  CLArrayCommandCopyToAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private a: CommandQueue<CLArray<T>>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<CLArray<T>>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<CLArray<T>>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<CLArray<T>>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, o.Native,a.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          new UIntPtr(Min(o.ByteSize, a.ByteSize)),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddCopyTo(a: CommandQueue<CLArray<T>>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandCopyToAutoSize<T>(a));

{$endregion CopyToAutoSize}

{$region CopyTo}

type
  CLArrayCommandCopyTo<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private        a: CommandQueue<CLArray<T>>;
    private from_ind: CommandQueue<integer>;
    private   to_ind: CommandQueue<integer>;
    private      len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 4;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>);
    begin
      self.       a :=        a;
      self.from_ind := from_ind;
      self.  to_ind :=   to_ind;
      self.     len :=      len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        a_qr: QueueRes<CLArray<T>>;
      var from_ind_qr: QueueRes<integer>;
      var   to_ind_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 4, invoker->
      begin
               a_qr := invoker.InvokeBranch&<CLArray<T>>(       a.Invoke); evs_l1.Add(a_qr.ev);
        from_ind_qr := invoker.InvokeBranch&<integer>(from_ind.Invoke); evs_l1.Add(from_ind_qr.ev);
          to_ind_qr := invoker.InvokeBranch&<integer>(  to_ind.Invoke); evs_l1.Add(to_ind_qr.ev);
             len_qr := invoker.InvokeBranch&<integer>(     len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        a :=        a_qr.GetRes;
        var from_ind := from_ind_qr.GetRes;
        var   to_ind :=   to_ind_qr.GetRes;
        var      len :=      len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, o.Native,a.Native,
          new UIntPtr(from_ind*Marshal.SizeOf&<T>), new UIntPtr(to_ind*Marshal.SizeOf&<T>),
          new UIntPtr(len*Marshal.SizeOf&<T>),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddCopyTo(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandCopyTo<T>(a, from_ind, to_ind, len));

{$endregion CopyTo}

{$region CopyFromAutoSize}

type
  CLArrayCommandCopyFromAutoSize<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private a: CommandQueue<CLArray<T>>;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<CLArray<T>>);
    begin
      self.a := a;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var a_qr: QueueRes<CLArray<T>>;
      g.ParallelInvoke(l, true, 1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<CLArray<T>>(a.Invoke); evs_l1.Add(a_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var a := a_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, a.Native,o.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          new UIntPtr(Min(o.ByteSize, a.ByteSize)),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddCopyFrom(a: CommandQueue<CLArray<T>>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandCopyFromAutoSize<T>(a));

{$endregion CopyFromAutoSize}

{$region CopyFrom}

type
  CLArrayCommandCopyFrom<T> = sealed class(EnqueueableGPUCommand<CLArray<T>>)
  where T: record;
    private        a: CommandQueue<CLArray<T>>;
    private from_ind: CommandQueue<integer>;
    private   to_ind: CommandQueue<integer>;
    private      len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 4;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>);
    begin
      self.       a :=        a;
      self.from_ind := from_ind;
      self.  to_ind :=   to_ind;
      self.     len :=      len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList)->cl_event; override;
    begin
      var        a_qr: QueueRes<CLArray<T>>;
      var from_ind_qr: QueueRes<integer>;
      var   to_ind_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(l, true, 4, invoker->
      begin
               a_qr := invoker.InvokeBranch&<CLArray<T>>(       a.Invoke); evs_l1.Add(a_qr.ev);
        from_ind_qr := invoker.InvokeBranch&<integer>(from_ind.Invoke); evs_l1.Add(from_ind_qr.ev);
          to_ind_qr := invoker.InvokeBranch&<integer>(  to_ind.Invoke); evs_l1.Add(to_ind_qr.ev);
             len_qr := invoker.InvokeBranch&<integer>(     len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs)->
      begin
        var        a :=        a_qr.GetRes;
        var from_ind := from_ind_qr.GetRes;
        var   to_ind :=   to_ind_qr.GetRes;
        var      len :=      len_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueCopyBuffer(
          cq, a.Native,o.Native,
          new UIntPtr(from_ind*Marshal.SizeOf&<T>), new UIntPtr(to_ind*Marshal.SizeOf&<T>),
          new UIntPtr(len*Marshal.SizeOf&<T>),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
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
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddCopyFrom(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArrayCCQ<T> :=
AddCommand(self, new CLArrayCommandCopyFrom<T>(a, from_ind, to_ind, len));

{$endregion CopyFrom}

{$endregion 3#Copy}

{$region Get}

{$region GetValue}

type
  CLArrayCommandGetValue<T> = sealed class(EnqueueableGetCommand<CLArray<T>, &T>)
  where T: record;
    private ind: CommandQueue<integer>;
    
    public function ForcePtrQr: boolean; override := true;
    
    public function ParamCountL1: integer; override := 1;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: CLArrayCCQ<T>; ind: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.ind := ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<&T>)->cl_event; override;
    begin
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(l.WithPtrNeed(False), true, 1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<integer>(ind.Invoke); evs_l1.Add(ind_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var ind := ind_qr.GetRes;
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(int64(ind) * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          new IntPtr((own_qr as QueueResDelayedPtr<&T>).ptr),
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        var own_qr_hnd := GCHandle.Alloc(own_qr);
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          own_qr_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [own_qr]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.AddGetValue(ind: CommandQueue<integer>): CommandQueue<&T> :=
new CLArrayCommandGetValue<T>(self, ind) as CommandQueue<&T>;

{$endregion GetValue}

{$region GetArrayAutoSize}

type
  CLArrayCommandGetArrayAutoSize<T> = sealed class(EnqueueableGetCommand<CLArray<T>, array of &T>)
  where T: record;
    
    public function ParamCountL1: integer; override := 0;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: CLArrayCCQ<T>);
    begin
      inherited Create(ccq);
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array of &T>)->cl_event; override;
    begin
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var res := new T[o.Length];
        own_qr.SetRes(res);
        var res_hnd := GCHandle.Alloc(res, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          res_hnd.AddrOfPinnedObject,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override := sb += #10;
    
  end;
  
function CLArrayCCQ<T>.AddGetArray: CommandQueue<array of &T> :=
new CLArrayCommandGetArrayAutoSize<T>(self) as CommandQueue<array of &T>;

{$endregion GetArrayAutoSize}

{$region GetArray}

type
  CLArrayCommandGetArray<T> = sealed class(EnqueueableGetCommand<CLArray<T>, array of &T>)
  where T: record;
    private ind: CommandQueue<integer>;
    private len: CommandQueue<integer>;
    
    public function ParamCountL1: integer; override := 2;
    public function ParamCountL2: integer; override := 0;
    
    public constructor(ccq: CLArrayCCQ<T>; ind, len: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.ind := ind;
      self.len := len;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; l: CLTaskLocalData; evs_l1, evs_l2: List<EventList>): (CLArray<T>, cl_command_queue, CLTaskErrHandler, EventList, QueueResDelayedBase<array of &T>)->cl_event; override;
    begin
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(l.WithPtrNeed(False), true, 2, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<integer>(ind.Invoke); evs_l1.Add(ind_qr.ev);
        len_qr := invoker.InvokeBranch&<integer>(len.Invoke); evs_l1.Add(len_qr.ev);
      end);
      
      Result := (o, cq, err_handler, evs, own_qr)->
      begin
        var ind := ind_qr.GetRes;
        var len := len_qr.GetRes;
        var res := new T[len];
        own_qr.SetRes(res);
        var res_hnd := GCHandle.Alloc(res, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(int64(ind) * Marshal.SizeOf&<T>), new UIntPtr(int64(len) * Marshal.SizeOf&<T>),
          res_hnd.AddrOfPinnedObject,
          evs.count, evs.evs, res_ev
        ).RaiseIfError;
        
        EventList.AttachCallback(true, res_ev, ()->
        begin
          res_hnd.Free;
        end, err_handler{$ifdef EventDebug}, 'GCHandle.Free for [res]'{$endif});
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.RegisterWaitables(g, prev_hubs);
      len.RegisterWaitables(g, prev_hubs);
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
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
  
function CLArrayCCQ<T>.AddGetArray(ind, len: CommandQueue<integer>): CommandQueue<array of &T> :=
new CLArrayCommandGetArray<T>(self, ind, len) as CommandQueue<array of &T>;

{$endregion GetArray}

{$endregion Get}

{$endregion Explicit}

{$endregion CLArray}

{$endregion Enqueueable's}

{$region Global subprograms}

{$region HFQ/HPQ}

type
  CommandQueueHostQueueBase<T,TFunc> = abstract class(HostQueue<object,T>)
  where TFunc: Delegate;
    
    private f: TFunc;
    public constructor(f: TFunc) := self.f := f;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<object>; override :=
    new QueueResConst<Object>(nil, l.prev_ev);
    
    protected procedure RegisterWaitables(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<CommandQueueBase,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ': ';
      ToStringWriteDelegate(sb, f);
      sb += #10;
    end;
    
  end;
  
  CommandQueueHostFunc<T> = sealed class(CommandQueueHostQueueBase<T, Context->T>)
    
    protected function ExecFunc(o: object; c: Context): T; override := f(c);
    
  end;
  CommandQueueHostProc = sealed class(CommandQueueHostQueueBase<object, Context->()>)
    
    protected function ExecFunc(o: object; c: Context): object; override;
    begin
      f(c);
      Result := nil;
    end;
    
  end;
  
function HFQ<T>(f: ()->T) :=
new CommandQueueHostFunc<T>(c->f());
function HFQ<T>(f: Context->T) :=
new CommandQueueHostFunc<T>(f);

function HPQ(p: ()->()) :=
new CommandQueueHostProc(c->p());
function HPQ(p: Context->()) :=
new CommandQueueHostProc(p);

{$endregion HFQ/HPQ}

{$region CombineQueue's}

{$region Sync}

{$region NonConv}

function CombineSyncQueueBase(params qs: array of CommandQueueBase) := new SimpleSyncQueueArray<object>(QueueArrayUtils.FlattenSyncQueueArray(qs));
function CombineSyncQueueBase(qs: sequence of CommandQueueBase) := new SimpleSyncQueueArray<object>(QueueArrayUtils.FlattenSyncQueueArray(qs));

function CombineSyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>) := new SimpleSyncQueueArray<T>(QueueArrayUtils.FlattenSyncQueueArray(qs.Append(last as CommandQueueBase)));

function CombineSyncQueue<T>(params qs: array of CommandQueue<T>) := new SimpleSyncQueueArray<T>(QueueArrayUtils.FlattenSyncQueueArray(qs.Cast&<CommandQueueBase>));
function CombineSyncQueue<T>(qs: sequence of CommandQueue<T>) := new SimpleSyncQueueArray<T>(QueueArrayUtils.FlattenSyncQueueArray(qs.Cast&<CommandQueueBase>));

{$endregion NonConv}

{$region Conv}

{$region NonContext}

function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase) := new ConvSyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, (a,c)->conv(a));
function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase) := new ConvSyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, (a,c)->conv(a));

function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>) := new ConvSyncQueueArray<TInp, TRes>(qs.ToArray, (a,c)->conv(a));
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>) := new ConvSyncQueueArray<TInp, TRes>(qs.ToArray, (a,c)->conv(a));

function CombineSyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new ConvSyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, (o1, o2, c)->conv(o1, o2));
function CombineSyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new ConvSyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, (o1, o2, o3, c)->conv(o1, o2, o3));
function CombineSyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new ConvSyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, (o1, o2, o3, o4, c)->conv(o1, o2, o3, o4));
function CombineSyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new ConvSyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, (o1, o2, o3, o4, o5, c)->conv(o1, o2, o3, o4, o5));
function CombineSyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new ConvSyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, (o1, o2, o3, o4, o5, o6, c)->conv(o1, o2, o3, o4, o5, o6));
function CombineSyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new ConvSyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, (o1, o2, o3, o4, o5, o6, o7, c)->conv(o1, o2, o3, o4, o5, o6, o7));

{$endregion NonContext}

{$region Context}

function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase) := new ConvSyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv);
function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase) := new ConvSyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv);

function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>) := new ConvSyncQueueArray<TInp, TRes>(qs.ToArray, conv);
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>) := new ConvSyncQueueArray<TInp, TRes>(qs.ToArray, conv);

function CombineSyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new ConvSyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineSyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new ConvSyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineSyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new ConvSyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineSyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new ConvSyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineSyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new ConvSyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineSyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new ConvSyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

{$endregion Context}

{$endregion Conv}

{$endregion Sync}

{$region Async}

{$region NonConv}

function CombineAsyncQueueBase(params qs: array of CommandQueueBase) := new SimpleAsyncQueueArray<object>(QueueArrayUtils.FlattenAsyncQueueArray(qs));
function CombineAsyncQueueBase(qs: sequence of CommandQueueBase) := new SimpleAsyncQueueArray<object>(QueueArrayUtils.FlattenAsyncQueueArray(qs));

function CombineAsyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>) := new SimpleAsyncQueueArray<T>(QueueArrayUtils.FlattenAsyncQueueArray(qs.Append(last as CommandQueueBase)));

function CombineAsyncQueue<T>(params qs: array of CommandQueue<T>) := new SimpleAsyncQueueArray<T>(QueueArrayUtils.FlattenAsyncQueueArray(qs.Cast&<CommandQueueBase>));
function CombineAsyncQueue<T>(qs: sequence of CommandQueue<T>) := new SimpleAsyncQueueArray<T>(QueueArrayUtils.FlattenAsyncQueueArray(qs.Cast&<CommandQueueBase>));

{$endregion NonConv}

{$region Conv}

{$region NonContext}

function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase) := new ConvAsyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, (a,c)->conv(a));
function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase) := new ConvAsyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, (a,c)->conv(a));

function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>) := new ConvAsyncQueueArray<TInp, TRes>(qs.ToArray, (a,c)->conv(a));
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>) := new ConvAsyncQueueArray<TInp, TRes>(qs.ToArray, (a,c)->conv(a));

function CombineAsyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new ConvAsyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, (o1, o2, c)->conv(o1, o2));
function CombineAsyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new ConvAsyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, (o1, o2, o3, c)->conv(o1, o2, o3));
function CombineAsyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new ConvAsyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, (o1, o2, o3, o4, c)->conv(o1, o2, o3, o4));
function CombineAsyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new ConvAsyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, (o1, o2, o3, o4, o5, c)->conv(o1, o2, o3, o4, o5));
function CombineAsyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new ConvAsyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, (o1, o2, o3, o4, o5, o6, c)->conv(o1, o2, o3, o4, o5, o6));
function CombineAsyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new ConvAsyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, (o1, o2, o3, o4, o5, o6, o7, c)->conv(o1, o2, o3, o4, o5, o6, o7));

{$endregion NonContext}

{$region Context}

function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase) := new ConvAsyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv);
function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase) := new ConvAsyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv);

function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>) := new ConvAsyncQueueArray<TInp, TRes>(qs.ToArray, conv);
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>) := new ConvAsyncQueueArray<TInp, TRes>(qs.ToArray, conv);

function CombineAsyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new ConvAsyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineAsyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new ConvAsyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineAsyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new ConvAsyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineAsyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new ConvAsyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineAsyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new ConvAsyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineAsyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new ConvAsyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

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