
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

//TODO Справка:

//===================================
// Запланированное:

//TODO cl.WaitForEvents тратит время процессора??? Почему?
//TODO Вроде как реализация может не отсылать комманды некоторое время без cl.Finish (а может cl.Flush?)
//TODO Интегрировать профайлинг очередей
// - И в том числе профайлинг отдельных ивентов

//TODO err_handler: can_cache может лучше заменить на can_cache_from?
// - Сейчас, вроде, can_cache:=false только в одном месте: и там кешировать нельзя НЕ все ноды
// - А нет, вроде can_cache=false вообще только на 1 уровень распространяется, а предыдущие хендлеры всё равно можно кешировать
// - В таком случае параметр лучше вообще убрать, и сделать отдельную HadError, не устанавливающую кеш
// - Ещё перепроверить, перед тем как делать

//TODO KernelArg.FromDataCQ(mu().ThenQConv(data->data.ptr), mu().ThenQConv(data->data.size))
// - Как то корявенько, когда надо из 1 значения сделать FromDataCQ
// - Может альтернативный вариант с передачей какой-то записи?
// - И наверное не только тут пригодится - к примеру SubMemorySegment
// - В то же время если надо из 2 очередей (ptr и size) его сделать Combine.Conv[Sync/Async].N2
// - То есть пользователь сам решает асинхронные ли ветки

//TODO KernelArg.FromArray принимает индекс но не длину
// - А FromCLArray вообще не может ссылаться на диапазон в массиве

//TODO Пройтись по интерфейсу, порасставлять кидание исключений
//TODO Проверки и кидания исключений перед всеми cl.*, чтобы выводить норм сообщения об ошибках
//TODO Попробовать получать информацию о параметрах Kernel'а и выдавать адекватные ошибки, если передают что-то не то
// - Адрес RAM всегда превращается в read-only копию на стороне OpenCL-C?

//TODO Использовать cl.EnqueueMapBuffer
// - В виде .AddMap((MappedArray,Context)->())
// - Недодел своей ветке

//TODO .pcu с неправильной позицией зависимости, или не теми настройками - должен игнорироваться
// - Иначе сейчас модули в примерах ссылаются на .pcu, который существует только во время работы Tester, ломая компилятор

//TODO Порядок Wait очередей в Wait группах
// - Проверить сочетание с каждой другой фичей

//TODO .Cycle(integer)
//TODO .Cycle // бесконечность циклов
//TODO .CycleWhile(***->boolean)
//TODO В продолжение Cycle: Однако всё ещё остаётся проблема - как сделать ветвление?
// - И если уже делать - стоит сделать и метод CQ.ThenIf(res->boolean; if_true, if_false: CQ)
//TODO И ещё - AbortQueue, который, по сути, может использоваться как exit, continue или break, если с обработчиками ошибок
// - Или может метод MarkerQueue.Abort?
//TODO .DelayInit, чтобы ветки .ThenIf можно было не инициализировать заранее
// - Тогда .ThenIf на много проще реализовать - через особый err_handler, который говорит что ошибки были, без собственно ошибок
//TODO CCQ.ThenIf(cond, command, nil)
// - Подумать как можно сделать это красивее, чем через MU

//TODO Несколько TODO в:
// - Queue converter's >> Wait

//TODO CLArray2 и CLArray3?
// - Основная проблема использовать только CLArray<> сейчас - через него не прочитаешь/не запишешь многомерные массивы из RAM
// - Вообще на стороне OpenCL запутывает, по строкам или по столбцам передался массив?
// - С этой стороны, лучше иметь только одномерный CLArray, ради безопасности
// - По хорошему, в коде использующем OpenCLABC, надо объявляться MatrixByRows/MatrixByCols и т.п.
// - Но это будет объёмно, а ради простых примеров...

//TODO Исправить перегрузки Kernel.Exec

//TODO Проверить, будет ли оптимизацией, создавать свой ThreadPool для каждого CLTaskBase
// - (HPQ+HPQ).Handle.Handle, тут создаётся 4 UserEvent, хотя всё можно было бы выполнять синхронно

//TODO А что если передавать в делегаты QueueRes'ов Context и возможно err_handler
// - Надо будет сразу сравнить скорость

//TODO CombineUse? А то CombineConv есть, а Use нету...

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

//TODO Вообще CommandQueue это CommandTree - и уже давно
// - Но чтобы исправить это название - придётся вывернуть на изнанку абсолютно всё в модуле, включая кодо-генерируемую часть и справку
// - И совместимость со старым модулем будет нереально сделать
// - Поэтому пока что лучше не делать

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
uses System.Runtime.CompilerServices;
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
    inherited Create(OpenCLException.Create(ec).Message);
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
      otp.WriteLine(System.Environment.StackTrace);
      
      foreach var kvp in RefCounter do
      begin
        otp.WriteLine($'Logging state change of {kvp.Key}:');
        var c := 0;
        foreach var act in kvp.Value do
        begin
          c += if act.is_release then -1 else +1;
          otp.WriteLine($'{c,3} | {act}');
        end;
        otp.WriteLine('-'*30);
      end;
      
      otp.WriteLine('='*40);
      otp.Flush;
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
    
    public static procedure FinallyReport;
    begin
      foreach var ev in RefCounter.Keys do if CountRetains(ev)<>0 then
      begin
        ReportRefCounterInfo(Console.Error);
        Sleep(1000);
        raise new OpenCLABCInternalException(ev.ToString);
      end;
      var total_ev_count := RefCounter.Values.Sum(q->q.Select(act->act.is_release ? -1 : +1).PartialSum.CountOf(0));
      $'[EventDebug]: {total_ev_count} event''s created'.Println;
    end;
    
    {$endregion Retain/Release}
    
  end;
  
  {$endif EventDebug}{$endregion EventDebug}
  
  {$region QueueDebug}{$ifdef QueueDebug}
  
  QueueDebug = static class
    
    private static QueueUses := new ConcurrentDictionary<cl_command_queue, ConcurrentQueue<string>>;
    private static function QueueUsesFor(cq: cl_command_queue) := QueueUses.GetOrAdd(cq, cq->new ConcurrentQueue<string>);
    private static procedure Add(cq: cl_command_queue; use: string) := QueueUsesFor(cq).Enqueue(use);
    
    public static procedure ReportQueueUses(otp: System.IO.TextWriter := Console.Out) :=
    lock otp do
    begin
      otp.WriteLine(System.Environment.StackTrace);
      
      foreach var kvp in QueueUses do
      begin
        otp.WriteLine($'Logging uses of {kvp.Key}:');
        foreach var use in kvp.Value do
          otp.WriteLine(use);
        otp.WriteLine('-'*30);
      end;
      
      otp.WriteLine('='*40);
      otp.Flush;
    end;
    
    public static procedure FinallyReport;
    begin
      var total_q_count := QueueUses.Keys.Sum(q->
      begin
        Result := 0;
        var last_return := false;
        foreach var use in QueueUses[q] do
        begin
          last_return := ('- return -' in use) or ('- last q -' in use);
          Result += ord(last_return);
        end;
        if last_return then exit;
        ReportQueueUses(Console.Error);
        Sleep(1000);
        raise new OpenCLABCInternalException(q.ToString);
      end);
      $'[QueueDebug]: {total_q_count} queue''s created'.Println;
    end;
    
  end;
  
  {$endif}{$endregion QueueDebug}
  
  {$region WaitDebug}{$ifdef WaitDebug}
  
  WaitDebug = static class
    
    private static WaitActions := new ConcurrentDictionary<object, ConcurrentQueue<string>>;
    
    private static procedure RegisterAction(handler: object; act: string) :=
    WaitActions.GetOrAdd(handler, hc->new System.Collections.Concurrent.ConcurrentQueue<string>).Enqueue(act);
    
    public static procedure ReportWaitActions(otp: System.IO.TextWriter := Console.Out) :=
    lock otp do
    begin
      otp.WriteLine(System.Environment.StackTrace);
      
      foreach var kvp in WaitActions do
      begin
        otp.WriteLine($'Logging actions of handler[{kvp.Key.GetHashCode}]:');
        foreach var act in kvp.Value do
          otp.WriteLine(act);
        otp.WriteLine('-'*30);
      end;
      
      otp.WriteLine('='*40);
      otp.Flush;
    end;
    
    public static procedure FinallyReport;
    begin
      $'[WaitDebug]: {WaitActions.Count} wait handler''s created'.Println;
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
    protected [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    protected [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    
    public static function operator=(wr1, wr2: Platform): boolean :=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    public static function operator<>(wr1, wr2: Platform): boolean := false=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    
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
    
    public static function operator=(wr1, wr2: Device): boolean :=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    public static function operator<>(wr1, wr2: Device): boolean := false=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    
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
    
    public static function operator=(wr1, wr2: Context): boolean :=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    public static function operator<>(wr1, wr2: Context): boolean := false=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    
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
    
    public static function operator=(wr1, wr2: ProgramCode): boolean :=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    public static function operator<>(wr1, wr2: ProgramCode): boolean := false=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    
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
    
    public static function operator=(wr1, wr2: Kernel): boolean :=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    public static function operator<>(wr1, wr2: Kernel): boolean := false=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    
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
    
    public static function operator=(wr1, wr2: MemorySegment): boolean :=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    public static function operator<>(wr1, wr2: MemorySegment): boolean := false=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    
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
    
    public static function operator=(wr1, wr2: CLArray<T>): boolean :=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    public static function operator<>(wr1, wr2: CLArray<T>): boolean := false=
    if ReferenceEquals(wr1,nil) then ReferenceEquals(wr2,nil) else not ReferenceEquals(wr2,nil) and (wr1.ntv = wr2.ntv);
    
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
        Result := Result.Remove(ind) + '<' + t.GenericTypeArguments.Select(TypeToTypeName).JoinToString(', ') + '>';
      end;
      
    end;
    private function DisplayName: string; virtual := DisplayNameForType(self.GetType);
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); abstract;
    
    private static function GetValueRuntimeType<T>(val: T) :=
    if typeof(T).IsValueType then
      typeof(T) else
    if val = default(T) then
      nil else val.GetType;
    private static procedure ToStringRuntimeValue<T>(sb: StringBuilder; val: T);
    begin
      var rt := GetValueRuntimeType(val);
      if typeof(T) <> rt then
      begin
        if rt<>nil then sb.Append(TypeToTypeName(rt));
        sb += '{ ';
      end;
      if rt<>nil then
        sb.Append(_ObjectToString(val)) else
        sb += 'nil';
      if typeof(T) <> rt then
        sb += ' }';
    end;
    
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
    const lambda='lambda';
    const sugar_begin='<>';
    const par_separator='; ';
    begin
      if d.Target<>nil then
      begin
        sb += _ObjectToString(d.Target);
        sb += ' => ';
      end;
      var mi := d.Method;
      var rt := mi.ReturnType;
      if rt=typeof(Void) then rt := nil;
      
      sb += if rt=nil then 'procedure' else 'function';
      sb += ' ';
      begin
        var name := mi.Name;
        if name.StartsWith(sugar_begin) then
          name := if lambda in name then
            lambda else name.Substring(sugar_begin.Length);
        sb += name;
      end;
      
      var pars := mi.GetParameters;
      if pars.Length<>0 then
      begin
        sb += '(';
        foreach var par in pars do
        begin
          var name := par.Name;
          if name.StartsWith(sugar_begin) then
            name := name.Substring(sugar_begin.Length);
          sb += name;
          sb += ': ';
          sb += TypeToTypeName(par.ParameterType);
          sb += par_separator;
        end;
        sb.Length -= par_separator.Length;
        sb += ')';
      end;
      
      if rt<>nil then
      begin
        sb += ': ';
        sb += TypeToTypeName(rt);
      end;
      
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
  ///Константные очереди ничего не выполняют и возвращают заданное при создании значение
  ConstQueue<T> = sealed partial class(CommandQueue<T>)
    private res: T;
    
    ///Создаёт новую константную очередь, возвращающую указанное значения
    public constructor(o: T) := self.res := o;
    private constructor := raise new OpenCLABCInternalException;
    
    ///Значение, которого возвращает данная константная очередь
    public property Value: T read res write res;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ': ';
      ToStringRuntimeValue(sb, self.res);
      sb += #10;
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
  
  {$region Parameter}
  
  ///Представляет установщик очереди-параметра ParameterQueue<T>, созданный методом .NewSetter
  ///Если передать этот установщик в метод запуска очереди,
  ///при выполнении параметр будет иметь указанное значение
  ParameterQueueSetter = sealed partial class
    private val: object;
    
    private constructor := raise new OpenCLABCInternalException;
    
  end;
  ///Представляет очередь-параметр
  ///Очереди-параметры ничего не выполняют, но возвращает установленное при запуске очереди значение
  ParameterQueue<T> = sealed partial class(CommandQueue<T>)
    private _name: string;
    private def: T;
    private def_is_set: boolean;
    
    ///Создаёт новую очередь-параметр
    ///name указывает имя параметра
    public constructor(name: string);
    begin
      self._name := name;
      self.def_is_set := false;
    end;
    ///Создаёт новую очередь-параметр
    ///name указывает имя параметра
    ///Указанное значение будет использоваться если при запуске очереди значение параметра небыло установлено
    public constructor(name: string; def: T);
    begin
      self.name := name;
      self.def := def;
      self.def_is_set := true;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    ///Имя параметра
    public property Name: string read _name write _name;
    ///Указывает, установлено ли значение, которое будет использоваться если при запуске очереди значение параметра небыло установлено
    public property DefaultDefined: boolean read def_is_set;
    
    private function GetDefault: T;
    begin
      if not def_is_set then
        raise new InvalidOperationException($'Значение параметра {name} небыло установлено');
      Result := self.def;
    end;
    ///Значение, которое будет использоваться если при запуске очереди значение параметра небыло установлено
    public property &Default: T read def write
    begin
      self.def := value;
      self.def_is_set := true;
    end;
    
    ///Создаёт установщик данного параметра
    ///Если передать этот установщик в метод запуска очереди,
    ///при выполнении параметр будет иметь указанное значение
    public function NewSetter(val: T): ParameterQueueSetter;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += '["';
      sb += Name;
      sb += '"]: Default=';
      ToStringRuntimeValue(sb, self.def);
      sb += #10;
    end;
    
  end;
  
  {$endregion Parameter}
  
  {$region Cast}
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    ///Если данная очередь проходит по условию "... is CommandQueue<T>" - возвращает себя же
    ///Иначе возвращает очередь-обёртку, выполняющую "res := T(res)", где res - результат данной очереди
    public function Cast<T>: CommandQueue<T>;
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает nil
  CommandQueueNil = abstract partial class(CommandQueueBase)
    
    ///Создаёт константную очередь, выполняющую данную и возвращающую T(nil)
    ///Для этого T должен быть ссылочным
    public function Cast<T>: CommandQueue<T>; where T: class;
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает значение типа T
  CommandQueue<T> = abstract partial class(CommandQueueBase) end;
  
  {$endregion Cast}
  
  {$region DiscardResult}
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  CommandQueueBase = abstract partial class
    
    private function DiscardResultBase: CommandQueueNil; abstract;
    ///Возвращает данную очередь но без результата
    public function DiscardResult := DiscardResultBase;
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает nil
  CommandQueueNil = abstract partial class(CommandQueueBase)
    
    private function DiscardResultBase: CommandQueueNil; override := DiscardResult;
    ///Возвращает данную очередь
    public function DiscardResult := self;
    
  end;
  
  ///Представляет очередь команд, в основном выполняемых на GPU
  ///Такая очередь всегда возвращает значение типа T
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    private function DiscardResultBase: CommandQueueNil; override := DiscardResult;
    ///Возвращает данную очередь но без результата
    public function DiscardResult: CommandQueueNil;
    
  end;
  
  {$endregion DiscardResult}
  
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
    ///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickConvert<TOtp>(f: T->TOtp): CommandQueue<TOtp>;
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди и контекст выполнения
    ///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickConvert<TOtp>(f: (T, Context)->TOtp): CommandQueue<TOtp>;
    
    ///Создаёт очередь, которая выполнит данную и вернёт её результат
    ///Но перед этим выполнит на CPU процедуру p, используя полученный результат
    ///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickUse(p: T->()           ): CommandQueue<T>;
    ///Создаёт очередь, которая выполнит данную и вернёт её результат
    ///Но перед этим выполнит на CPU процедуру p, используя полученный результат и контекст выполнения
    ///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
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
    public function BeginInvoke(q: CommandQueueBase; params parameters: array of ParameterQueueSetter): CLTaskBase;
    ///Запускает данную очередь и все её подочереди
    ///Как только всё запущено: возвращает CLTask, через который можно следить за процессом выполнения
    public function BeginInvoke(q: CommandQueueNil; params parameters: array of ParameterQueueSetter): CLTaskNil;
    ///Запускает данную очередь и все её подочереди
    ///Как только всё запущено: возвращает CLTask, через который можно следить за процессом выполнения
    public function BeginInvoke<T>(q: CommandQueue<T>; params parameters: array of ParameterQueueSetter): CLTask<T>;
    
    ///Запускает данную очередь и все её подочереди
    ///Затем ожидает окончания выполнения
    public procedure SyncInvoke(q: CommandQueueBase; params parameters: array of ParameterQueueSetter) := BeginInvoke(q, parameters).Wait;
    ///Запускает данную очередь и все её подочереди
    ///Затем ожидает окончания выполнения
    public procedure SyncInvoke(q: CommandQueueNil; params parameters: array of ParameterQueueSetter) := BeginInvoke(q, parameters).Wait;
    ///Запускает данную очередь и все её подочереди
    ///Затем ожидает окончания выполнения и возвращает полученный результат
    public function SyncInvoke<T>(q: CommandQueue<T>; params parameters: array of ParameterQueueSetter) := BeginInvoke(q, parameters).WaitRes;
    
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
    public static function operator implicit(mem_q: ConstQueue<MemorySegment>): KernelArg := FromMemorySegmentCQ(mem_q);
    public static function operator implicit(mem_q: ParameterQueue<MemorySegment>): KernelArg := FromMemorySegmentCQ(mem_q);
    
    {$endregion MemorySegment}
    
    {$region CLArray}
    
    ///Создаёт аргумент kernel'а, представляющий массив данных, хранимых на GPU
    public static function FromCLArray<T>(a: CLArray<T>): KernelArg; where T: record;
    public static function operator implicit<T>(a: CLArray<T>): KernelArg; where T: record; begin Result := FromCLArray(a); end;
    
    ///Создаёт аргумент kernel'а, представляющий массив данных, хранимых на GPU
    public static function FromCLArrayCQ<T>(a_q: CommandQueue<CLArray<T>>): KernelArg; where T: record;
    public static function operator implicit<T>(a_q: CommandQueue<CLArray<T>>): KernelArg; where T: record; begin Result := FromCLArrayCQ(a_q); end;
    public static function operator implicit<T>(a_q: ConstQueue<CLArray<T>>): KernelArg; where T: record; begin Result := FromCLArrayCQ(a_q); end;
    public static function operator implicit<T>(a_q: ParameterQueue<CLArray<T>>): KernelArg; where T: record; begin Result := FromCLArrayCQ(a_q); end;
    
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
    public static function operator implicit<TRecord>(valq: ConstQueue<TRecord>): KernelArg; where TRecord: record; begin Result := FromValueCQ(valq); end;
    public static function operator implicit<TRecord>(valq: ParameterQueue<TRecord>): KernelArg; where TRecord: record; begin Result := FromValueCQ(valq); end;
    
    {$endregion Value}
    
    {$region NativeValue}
    
    ///Создаёт аргумент kernel'а, ссылающийся на неуправляемое значение
    public static function FromNativeValue<TRecord>(val: NativeValue<TRecord>): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(val: NativeValue<TRecord>): KernelArg; where TRecord: record; begin Result := FromNativeValue(val); end;
    
    ///Создаёт аргумент kernel'а, ссылающийся на неуправляемое значение
    public static function FromNativeValueCQ<TRecord>(valq: CommandQueue<NativeValue<TRecord>>): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(valq: CommandQueue<NativeValue<TRecord>>): KernelArg; where TRecord: record; begin Result := FromNativeValueCQ(valq); end;
    public static function operator implicit<TRecord>(valq: ConstQueue<NativeValue<TRecord>>): KernelArg; where TRecord: record; begin Result := FromNativeValueCQ(valq); end;
    public static function operator implicit<TRecord>(valq: ParameterQueue<NativeValue<TRecord>>): KernelArg; where TRecord: record; begin Result := FromNativeValueCQ(valq); end;
    
    {$endregion NativeValue}
    
    {$region Array}
    
    ///Создаёт аргумент kernel'а, ссылающийся на указанный массив, на элемент с индексом ind
    public static function FromArray<TRecord>(a: array of TRecord; ind: integer := 0): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(a: array of TRecord): KernelArg; where TRecord: record; begin Result := FromArray(a); end;
    
    ///Создаёт аргумент kernel'а, ссылающийся на указанный массив, на элемент с индексом ind
    public static function FromArrayCQ<TRecord>(a_q: CommandQueue<array of TRecord>; ind_q: CommandQueue<integer> := 0): KernelArg; where TRecord: record;
    public static function operator implicit<TRecord>(a_q: CommandQueue<array of TRecord>): KernelArg; where TRecord: record; begin Result := FromArrayCQ(a_q); end;
    public static function operator implicit<TRecord>(a_q: ConstQueue<array of TRecord>): KernelArg; where TRecord: record; begin Result := FromArrayCQ(a_q); end;
    public static function operator implicit<TRecord>(a_q: ParameterQueue<array of TRecord>): KernelArg; where TRecord: record; begin Result := FromArrayCQ(a_q); end;
    
    {$endregion Array}
    
    {$region ToString}
    
    private function DisplayName: string; virtual := CommandQueueBase.DisplayNameForType(self.GetType);
    private static procedure ToStringRuntimeValue<T>(sb: StringBuilder; val: T) := CommandQueueBase.ToStringRuntimeValue(sb, val);
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
    public function ThenQueue(q: CommandQueueBase): KernelCCQ;
    
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".ThenQuick..."
    public function ThenProc(p: Kernel->()): KernelCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".ThenQuick..."
    public function ThenProc(p: (Kernel, Context)->()): KernelCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickProc(p: Kernel->()): KernelCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickProc(p: (Kernel, Context)->()): KernelCCQ;
    
    ///Добавляет ожидание сигнала выполненности от заданного маркера
    public function ThenWait(marker: WaitMarker): KernelCCQ;
    
    {$endregion Special .Add's}
    
    {$region 1#Exec}
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function ThenExec1(sz1: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function ThenExec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
    
    ///Выполняет kernel с указанным кол-вом ядер и передаёт в него указанные аргументы
    public function ThenExec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
    
    ///Выполняет kernel с расширенным набором параметров
    ///Данная перегрузка используется в первую очередь для тонких оптимизаций
    ///Если она вам понадобилась по другой причина - пожалуйста, напишите в issue
    public function ThenExec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): KernelCCQ;
    
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
    public function ThenQueue(q: CommandQueueBase): MemorySegmentCCQ;
    
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".ThenQuick..."
    public function ThenProc(p: MemorySegment->()): MemorySegmentCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".ThenQuick..."
    public function ThenProc(p: (MemorySegment, Context)->()): MemorySegmentCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickProc(p: MemorySegment->()): MemorySegmentCCQ;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickProc(p: (MemorySegment, Context)->()): MemorySegmentCCQ;
    
    ///Добавляет ожидание сигнала выполненности от заданного маркера
    public function ThenWait(marker: WaitMarker): MemorySegmentCCQ;
    
    {$endregion Special .Add's}
    
    {$region 1#Write&Read}
    
    ///Заполняет всю область памяти данными, находящимися по указанному адресу в RAM
    public function ThenWriteData(ptr: CommandQueue<IntPtr>): MemorySegmentCCQ;
    
    ///Заполняет часть области памяти данными, находящимися по указанному адресу в RAM
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function ThenWriteData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Читает всё содержимое области памяти в RAM, по указанному адресу
    public function ThenReadData(ptr: CommandQueue<IntPtr>): MemorySegmentCCQ;
    
    ///Читает часть содержимого области памяти в RAM, по указанному адресу
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function ThenReadData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Заполняет всю область памяти данными, находящимися по указанному адресу в RAM
    public function ThenWriteData(ptr: pointer): MemorySegmentCCQ;
    
    ///Заполняет часть области памяти данными, находящимися по указанному адресу в RAM
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function ThenWriteData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Читает всё содержимое области памяти в RAM, по указанному адресу
    public function ThenReadData(ptr: pointer): MemorySegmentCCQ;
    
    ///Читает часть содержимого области памяти в RAM, по указанному адресу
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function ThenReadData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function ThenWriteValue<TRecord>(val: TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function ThenWriteValue<TRecord>(val: CommandQueue<TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function ThenWriteValue<TRecord>(val: NativeValue<TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в начало области памяти
    public function ThenWriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает значение размерного типа из начала области памяти в указанное значение
    public function ThenReadValue<TRecord>(val: NativeValue<TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает значение размерного типа из начала области памяти в указанное значение
    public function ThenReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenWriteValue<TRecord>(val: TRecord; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenWriteValue<TRecord>(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenWriteValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанное значение размерного типа в области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenWriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает значение размерного типа из области памяти в указанное значение
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenReadValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает значение размерного типа из области памяти в указанное значение
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function ThenWriteArray1<TRecord>(a: array of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function ThenWriteArray2<TRecord>(a: array[,] of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function ThenWriteArray3<TRecord>(a: array[,,] of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ThenReadArray1<TRecord>(a: array of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ThenReadArray2<TRecord>(a: array[,] of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ThenReadArray3<TRecord>(a: array[,,] of TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenWriteArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ThenWriteArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ThenWriteArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenReadArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ThenReadArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ThenReadArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function ThenWriteArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function ThenWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает весь массив в начало области памяти
    public function ThenWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ThenReadArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ThenReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает из области памяти достаточно байт чтоб заполнить весь массив
    public function ThenReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenWriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ThenWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Записывает указанный участок массива в область памяти
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ThenWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ThenReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Читает данные из области памяти в указанный участок массива
    ///a_offset(-ы) указывают индекс в массиве
    ///len указывает кол-во задействованных элементов массива
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ThenReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    {$endregion 1#Write&Read}
    
    {$region 2#Fill}
    
    ///Читает pattern_len байт из RAM по указанному адресу и заполняет их копиями всю область памяти
    public function ThenFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Читает pattern_len байт из RAM по указанному адресу и заполняет их копиями часть области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function ThenFillData(ptr: CommandQueue<IntPtr>; pattern_len, mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Заполняет всю область памяти копиями указанного значения размерного типа
    public function ThenFillValue<TRecord>(val: TRecord): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного значения размерного типа
    public function ThenFillValue<TRecord>(val: CommandQueue<TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет часть области памяти копиями указанного значения размерного типа
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function ThenFillValue<TRecord>(val: TRecord; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет часть области памяти копиями указанного значения размерного типа
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///len указывает кол-во задействованных в операции байт
    public function ThenFillValue<TRecord>(val: CommandQueue<TRecord>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного массива
    public function ThenFillArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного массива
    public function ThenFillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет всю область памяти копиями указанного массива
    public function ThenFillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет часть области памяти копиями части указанного массива
    ///a_offset(-ы) указывают индекс в массиве
    ///pattern_len указывает кол-во задействованных элементов массива
    ///len указывает кол-во задействованных в операции байт
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenFillArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет часть области памяти копиями части указанного массива
    ///a_offset(-ы) указывают индекс в массиве
    ///pattern_len указывает кол-во задействованных элементов массива
    ///len указывает кол-во задействованных в операции байт
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ThenFillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    ///Заполняет часть области памяти копиями части указанного массива
    ///a_offset(-ы) указывают индекс в массиве
    ///pattern_len указывает кол-во задействованных элементов массива
    ///len указывает кол-во задействованных в операции байт
    ///mem_offset указывает отступ от начала области памяти, в байтах
    ///
    ///ВНИМАНИЕ! У многомерных массивов элементы распологаются так же как у одномерных, разделение на строки виртуально
    ///Это значит что, к примеру, чтение 4 элементов 2-х мерного массива начиная с индекса [0,1]
    ///прочитает элементы [0,1], [0,2], [1,0], [1,1]. Для чтения частей из нескольких строк массива - делайте несколько операций чтения, по 1 на строку
    public function ThenFillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
    
    {$endregion 2#Fill}
    
    {$region 3#Copy}
    
    ///Копирует данные из текущей области памяти в mem
    ///Если области памяти имеют разный размер - в качестве объёма данных берётся размер меньшей области
    public function ThenCopyTo(mem: CommandQueue<MemorySegment>): MemorySegmentCCQ;
    
    ///Копирует данные из текущей области памяти в mem
    ///from_pos указывает отступ в байтах от начала области памяти, из которой копируют
    ///to_pos указывает отступ в байтах от начала области памяти, в которую копируют
    ///len указывает кол-во копируемых байт
    public function ThenCopyTo(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    ///Копирует данные из mem в текущюу область памяти
    ///Если области памяти имеют разный размер - в качестве объёма данных берётся размер меньшей области
    public function ThenCopyFrom(mem: CommandQueue<MemorySegment>): MemorySegmentCCQ;
    
    ///Копирует данные из mem в текущюу область памяти
    ///from_pos указывает отступ в байтах от начала области памяти, из которой копируют
    ///to_pos указывает отступ в байтах от начала области памяти, в которую копируют
    ///len указывает кол-во копируемых байт
    public function ThenCopyFrom(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegmentCCQ;
    
    {$endregion 3#Copy}
    
    {$region Get}
    
    ///Читает значение указанного размерного типа из начала области памяти
    public function ThenGetValue<TRecord>: CommandQueue<TRecord>; where TRecord: record;
    
    ///Читает значение указанного размерного типа из области памяти
    ///mem_offset указывает отступ от начала области памяти, в байтах
    public function ThenGetValue<TRecord>(mem_offset: CommandQueue<integer>): CommandQueue<TRecord>; where TRecord: record;
    
    ///Читает массив максимального размера, на сколько хватит байт данной области памяти
    public function ThenGetArray1<TRecord>: CommandQueue<array of TRecord>; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое данный области памяти
    public function ThenGetArray1<TRecord>(len: CommandQueue<integer>): CommandQueue<array of TRecord>; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое данный области памяти
    public function ThenGetArray2<TRecord>(len1,len2: CommandQueue<integer>): CommandQueue<array[,] of TRecord>; where TRecord: record;
    
    ///Создаёт массив с указанным кол-вом элементов и копирует в него содержимое данный области памяти
    public function ThenGetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): CommandQueue<array[,,] of TRecord>; where TRecord: record;
    
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
    public function ThenQueue(q: CommandQueueBase): CLArrayCCQ<T>;
    
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".ThenQuick..."
    public function ThenProc(p: CLArray<T>->()): CLArrayCCQ<T>;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат выполняется в отдельном потоке выполнения (Thread)
    ///Если делегат выполняется быстро и выделение нового потока излишне - используйте соответствующую функцию, начинающуюся на ".ThenQuick..."
    public function ThenProc(p: (CLArray<T>, Context)->()): CLArrayCCQ<T>;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickProc(p: CLArray<T>->()): CLArrayCCQ<T>;
    ///Добавляет выполнение процедуры на CPU в список обычных команд для GPU
    ///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
    ///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
    ///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
    ///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
    ///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
    public function ThenQuickProc(p: (CLArray<T>, Context)->()): CLArrayCCQ<T>;
    
    ///Добавляет ожидание сигнала выполненности от заданного маркера
    public function ThenWait(marker: WaitMarker): CLArrayCCQ<T>;
    
    {$endregion Special .Add's}
    
    {$region 1#Write&Read}
    
    ///Заполняет весь данный объект CLArray<T> данными, находящимися по указанному адресу в RAM
    public function ThenWriteData(ptr: CommandQueue<IntPtr>): CLArrayCCQ<T>;
    
    ///Заполняет len элементов начиная с индекса ind данного объекта CLArray<T> данными, находящимися по указанному адресу в RAM
    public function ThenWriteData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Читает всё содержимое данного объекта CLArray<T> в RAM, по указанному адресу
    public function ThenReadData(ptr: CommandQueue<IntPtr>): CLArrayCCQ<T>;
    
    ///Читает len элементов начиная с индекса ind данного объекта CLArray<T> в RAM, по указанному адресу
    public function ThenReadData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Заполняет весь данный объект CLArray<T> данными, находящимися по указанному адресу в RAM
    public function ThenWriteData(ptr: pointer): CLArrayCCQ<T>;
    
    ///Заполняет len элементов начиная с индекса ind данного объекта CLArray<T> данными, находящимися по указанному адресу в RAM
    public function ThenWriteData(ptr: pointer; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Читает всё содержимое данного объекта CLArray<T> в RAM, по указанному адресу
    public function ThenReadData(ptr: pointer): CLArrayCCQ<T>;
    
    ///Читает len элементов начиная с индекса ind данного объекта CLArray<T> в RAM, по указанному адресу
    public function ThenReadData(ptr: pointer; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Записывает указанное значение по индексу ind
    public function ThenWriteValue(val: &T; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Записывает указанное значение по индексу ind
    public function ThenWriteValue(val: CommandQueue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Записывает указанное значение по индексу ind
    public function ThenWriteValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Записывает указанное значение по индексу ind
    public function ThenWriteValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Читает значение по индексу ind в указанное значение
    public function ThenReadValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Читает значение по индексу ind в указанное значение
    public function ThenReadValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Записывает весь указанный массив в начало данного объекта CLArray<T>
    public function ThenWriteArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
    
    ///Записывает len элементов массива a, начиная с индекса a_ind, в данный объект CLArray<T> по индексу ind
    public function ThenWriteArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Читает начало данного объекта CLArray<T> в указанный массив
    public function ThenReadArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
    
    ///Читает len элементов данного объекта CLArray<T>, начиная с индекса ind, в массив a по индексу a_ind
    public function ThenReadArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArrayCCQ<T>;
    
    {$endregion 1#Write&Read}
    
    {$region 2#Fill}
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями весь данный объект CLArray<T>
    public function ThenFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями len элементов начиная с индекса ind данного объекта CLArray<T>
    public function ThenFillData(ptr: CommandQueue<IntPtr>; pattern_len, ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями весь данный объект CLArray<T>
    public function ThenFillData(ptr: pointer; pattern_len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Чиатет pattern_len элементов из RAM по указанному адресу и заполняет их копиями len элементов начиная с индекса ind данного объекта CLArray<T>
    public function ThenFillData(ptr: pointer; pattern_len, ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Заполняет весь массив копиями указанного значения
    public function ThenFillValue(val: &T): CLArrayCCQ<T>;
    
    ///Заполняет весь массив копиями указанного значения
    public function ThenFillValue(val: CommandQueue<&T>): CLArrayCCQ<T>;
    
    ///Заполняет len элементов начиная с индекса ind копиями указанного значения
    public function ThenFillValue(val: &T; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Заполняет len элементов начиная с индекса ind копиями указанного значения
    public function ThenFillValue(val: CommandQueue<&T>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Заполняет данный объект CLArray<T> компиями указанного массива
    public function ThenFillArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
    
    ///Заполняет len элементов начиная с индекса ind компиями части указанного массива
    ///Из указанного массива берётся pattern_len элементов, начиная с индекса a_offset
    public function ThenFillArray(a: CommandQueue<array of &T>; a_offset,pattern_len, ind,len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    {$endregion 2#Fill}
    
    {$region 3#Copy}
    
    ///Копирует данные из текущего массива в a
    ///Если у массивов разный размер - копируется кол-во элементов меньшего массива
    public function ThenCopyTo(a: CommandQueue<CLArray<T>>): CLArrayCCQ<T>;
    
    ///Копирует данные из текущего массива в a
    ///from_ind указывает индекс в массиве, из которого копируют
    ///to_ind указывает индекс в массиве, в который копируют
    ///len указывает кол-во копируемых элементов
    public function ThenCopyTo(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    ///Копирует данные из a в текущий массив
    ///Если у массивов разный размер - копируется кол-во элементов меньшего массива
    public function ThenCopyFrom(a: CommandQueue<CLArray<T>>): CLArrayCCQ<T>;
    
    ///Копирует данные из a в текущий массив
    ///from_ind указывает индекс в массиве, из которого копируют
    ///to_ind указывает индекс в массиве, в который копируют
    ///len указывает кол-во копируемых элементов
    public function ThenCopyFrom(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
    
    {$endregion 3#Copy}
    
    {$region Get}
    
    ///Читает элемент по указанному индексу
    public function ThenGetValue(ind: CommandQueue<integer>): CommandQueue<&T>;
    
    ///Читает весь CLArray<T> как обычный массив array of T
    public function ThenGetArray: CommandQueue<array of &T>;
    
    ///Читает len элементов начиная с индекса ind из CLArray<T> как обычный массив array of T
    public function ThenGetArray(ind, len: CommandQueue<integer>): CommandQueue<array of &T>;
    
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

{$region ConstQueue}

///Создаёт константную очередь с указанным результатом
function CQ<T>(o: T): CommandQueue<T>;

{$endregion ConstQueue}

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
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function HFQQ<T>(f: ()->T): CommandQueue<T>;
///Создаёт очередь, выполняющую указанную функцию на CPU
///И возвращающую результат этой функци
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
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
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function HPQQ(p: ()->()): CommandQueueNil;
///Создаёт очередь, выполняющую указанную процедуру на CPU
///И возвращающую nil
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
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
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
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
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvSyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
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
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
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
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
///Так же в делегате не желательно использовать долго выполняющиеся алгоритмы, особенно ввод с клавиатуры
///Если эти ограничения не подходят, используйте соответствующую функцию, без "Quick" в названии
function CombineQuickConvAsyncQueueN6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
///Переданный делегат старается выполняется в одном из уже существующих потоков выполнения, но так чтобы не нарушить порядок выполнения дерева очередей
///Из делегата категорически нельзя вызывать функции модуля OpenCL блокирующие выполнение, к примеру "cl.WaitForEvents", "clFinish" и блокирующий "cl.EnqueueReadBuffer"
///Подробнее - читайте в документации библиотеки OpenCL про функцию "clSetEventCallback"
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
    inherited Create(t=blame ? $'Значения типа {TypeToTypeName(t)} нельзя {source_name}' : $'Значения типа {TypeToTypeName(t)} нельзя {source_name}, потому что он содержит тип {TypeToTypeName(blame)}' );
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
      
    end;
    
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
      // Instead CLTaskErrHandlerBranchCombinator handles origin
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
  /// Repeats first handler, but also steals errors from second, if first is OK
  CLTaskErrHandlerThiefRepeater = sealed class(CLTaskErrHandlerThiefBase)
    private prev_handler: CLTaskErrHandler;
    
    public constructor(prev_handler, victim: CLTaskErrHandler);
    begin
      inherited Create(victim);
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
      if CanSteal then StealPrevErrors;
    end;
    
    protected procedure FillErrLstWithPrev(origin_cache: HashSet<CLTaskErrHandler>; lst: List<Exception>); override;
    begin
      var prev_c := lst.Count;
      prev_handler.FillErrLst(lst);
      if prev_c=lst.Count then StealPrevErrors;
    end;
    
  end;
  
{$endregion CLTaskErrHandler}

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
    public all_evs: sequence of cl_event;
    {$endif EventDebug}
    
    public constructor(work: Action; left_c: integer{$ifdef EventDebug}; reason: string; all_evs: sequence of cl_event{$endif});
    begin
      self.work := work;
      self.left_c := left_c;
      {$ifdef EventDebug}
      self.reason := reason;
      self.all_evs := all_evs;
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
    
    public static procedure AttachCallback(ev: cl_event; work: Action{$ifdef EventDebug}; reason: string{$endif});
    begin
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
      EventDebug.RegisterEventRelease(ev, $'released in multi-callback, working on {cb_data.reason}, together with evs: {cb_data.all_evs.JoinToString}');
      {$endif EventDebug}
      OpenCLABCInternalException.RaiseIfError(cl.ReleaseEvent(ev));
      if Interlocked.Decrement(cb_data.left_c) <> 0 then exit;
      hnd.Free;
      cb_data.work();
    end;
    private static multi_attachable_callback: EventCallback := InvokeMultiAttachedCallback;
    
    public procedure MultiAttachCallback(work: Action{$ifdef EventDebug}; reason: string{$endif}) :=
    case self.count of
      0: work;
      1: AttachCallback(self.evs[0], work{$ifdef EventDebug}, reason{$endif});
      else
      begin
        var cb_data := new MultiAttachCallbackData(work, self.count{$ifdef EventDebug}, reason, evs.Take(count){$endif});
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
      EventDebug.RegisterEventRetain(evs[i], $'{reason}, together with evs: {evs.Take(count).JoinToString}');
      {$endif EventDebug}
      OpenCLABCInternalException.RaiseIfError( cl.RetainEvent(evs[i]) );
    end;
    
    public procedure Release({$ifdef EventDebug}reason: string{$endif}) :=
    for var i := 0 to count-1 do
    begin
      {$ifdef EventDebug}
      EventDebug.RegisterEventRelease(evs[i], $'{reason}, together with evs: {evs.Take(count).JoinToString}');
      {$endif EventDebug}
      OpenCLABCInternalException.RaiseIfError( cl.ReleaseEvent(evs[i]) );
    end;
    
    // cl.WaitForEvents uses processor time to wait
    // so if we need to wait it's better to use ManualResetEvent
    public function ToMRE({$ifdef EventDebug}reason: string{$endif}): ManualResetEvent;
    begin
      Result := nil;
      if self.count=0 then exit;
      Result := new ManualResetEvent(false);
      var mre := Result;
      self.MultiAttachCallback(()->mre.Set(){$ifdef EventDebug}, $'setting mre for {reason}'{$endif});
    end;
    
    {$endregion Retain/Release}
    
  end;
  
{$endregion EventList}

{$region CLTaskGlobalData}

type
  IParameterQueue = interface
    
    property Name: string read;
    
  end;
  ParameterQueueSetter = sealed partial class
    par_q: IParameterQueue;
    
    public constructor(par_q: IParameterQueue; val: object);
    begin
      self.par_q := par_q;
      self.val := val;
    end;
    
    public property Name: string read par_q.Name;
    
  end;
  ParameterQueue<T> = sealed partial class(CommandQueue<T>, IParameterQueue)
    
  end;
  
//TODO #????
function ParameterQueue<T>.NewSetter(val: T) := new ParameterQueueSetter(self as object as IParameterQueue, val);

type
  CLTaskParameterData = record
    val: object;
    state: (TPS_Empty, TPS_Default, TPS_Set);
    
    public constructor :=
    self.state := TPS_Empty;
    public constructor(def: object);
    begin
      self.val := def;
      self.state := TPS_Default;
    end;
    
    public function &Set(name: string; val: object): CLTaskParameterData;
    begin
      if self.state=TPS_Set then
        raise new ArgumentException($'Значение параметра {name} установлено больше одного раза');
      Result.val := val;
      Result.state := TPS_Set;
    end;
    
    public procedure TestSet(name: string) :=
    if self.state=TPS_Empty then
      raise new ArgumentException($'Значение параметра {name} небыло установлено');
    
  end;
  
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
    
    public procedure ReturnCQ(cq: cl_command_queue);
    begin
      free_cqs.Add(cq);
      {$ifdef QueueDebug}
      QueueDebug.Add(cq, '----- return -----');
      {$endif QueueDebug}
    end;
    
    public parameters := new Dictionary<IParameterQueue, CLTaskParameterData>;
    public procedure ApplyParameters(pars: array of ParameterQueueSetter);
    begin
      foreach var par in pars do
      begin
        if not parameters.ContainsKey(par.par_q) then
          raise new ArgumentException($'Параметр {par.Name} не используется');
        parameters[par.par_q] := parameters[par.par_q].Set(par.Name, par.val);
      end;
      foreach var kvp in self.parameters do
        kvp.Value.TestSet(kvp.Key.Name);
    end;
    
  end;
  
{$endregion CLTaskGlobalData}

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
    
    public static function StartBackgroundWork(after: EventList; work: Action; c: cl_context{$ifdef EventDebug}; reason: string{$endif}): UserEvent;
    begin
      var res := new UserEvent(c
        {$ifdef EventDebug}, $'BackgroundWork, executing {reason}, after waiting on: {after.evs?.JoinToString}'{$endif}
      );
      
      var mre := after.ToMRE({$ifdef EventDebug}$'Background work with res_ev={res}'{$endif});
      NativeUtils.StartNewBgThread(()->
      begin
        if mre<>nil then mre.WaitOne;
        
        try
          work;
        finally
          res.SetComplete;
        end;
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

{$region QueueResAction}

type
  QueueResAction = Context->();
  QueueResSetter<T> = Context->T;
  
  QueueResActionUtils = static class
    
    static function HandlerWrap(err_handler: CLTaskErrHandler; d: QueueResAction): QueueResAction := c->
    if not err_handler.HadError(true) then
    try
      d(c);
    except
      on e: Exception do err_handler.AddErr(e);
    end;
    static function HandlerWrap<T>(err_handler: CLTaskErrHandler; d: QueueResSetter<T>): QueueResSetter<T> := c->
    if not err_handler.HadError(true) then
    try
      Result := d(c);
    except
      on e: Exception do err_handler.AddErr(e);
    end;
    static function HandlerWrapStrip<T>(err_handler: CLTaskErrHandler; d: QueueResSetter<T>): QueueResAction := c->
    if not err_handler.HadError(true) then
    try
      d(c);
    except
      on e: Exception do err_handler.AddErr(e);
    end;
    
    static function HandlerWrap<T>(err_handler: CLTaskErrHandler; d: (T,Context)->()): (T,Context)->() := (o,c)->
    if not err_handler.HadError(true) then
    try
      d(o,c);
    except
      on e: Exception do err_handler.AddErr(e);
    end;
    static function HandlerWrap<T,TRes>(err_handler: CLTaskErrHandler; d: (T,Context)->TRes): (T,Context)->TRes := (o,c)->
    if not err_handler.HadError(true) then
    try
      Result := d(o,c);
    except
      on e: Exception do err_handler.AddErr(e);
    end;
    static function HandlerWrapStrip<T,TRes>(err_handler: CLTaskErrHandler; d: (T,Context)->TRes): (T,Context)->() := (o,c)->
    if not err_handler.HadError(true) then
    try
      d(o,c);
    except
      on e: Exception do err_handler.AddErr(e);
    end;
    
  end;
  
  [StructLayout(LayoutKind.Auto)]
  QueueResComplDelegateData = record
    private call_list: array of QueueResAction := nil;
    private count := 0;
    
    private const initial_cap = 4;
    
    public constructor := exit;
    public constructor(d: QueueResAction);
    begin
      call_list := new QueueResAction[initial_cap];
      call_list[0] := d;
      count := 1;
    end;
    
    public procedure AddAction(d: QueueResAction);
    begin
      if call_list=nil then
        call_list := new QueueResAction[initial_cap] else
      if count=call_list.Length then
        System.Array.Resize(call_list, call_list.Length * 4);
      call_list[count] := d;
      count += 1;
    end;
    
    {$ifdef DEBUG}
    private was_invoked := false;
    {$endif DEBUG}
    public procedure Invoke(c: Context);
    begin
      {$ifdef DEBUG}
      if was_invoked then raise new System.InvalidProgramException($'{self.GetType}: {System.Environment.StackTrace}');
      was_invoked := true;
      {$endif DEBUG}
      for var i := 0 to count-1 do
        call_list[i](c);
    end;
    
  end;
  
{$endregion QueueResAction}

{$region CLTaskLocalData}

type
  [StructLayout(LayoutKind.Auto)]
  CLTaskLocalData = record
    public prev_delegate := default(QueueResComplDelegateData);
    public prev_ev := EventList.Empty;
    
    public constructor := exit;
    public constructor(ev: EventList) := self.prev_ev := ev;
    
    public function ShouldInstaCallAction: boolean;
    begin
      // Only const can have not events
      Result := prev_ev.count=0;
      {$ifdef DEBUG}
      if Result and (prev_delegate.count<>0) then raise new OpenCLABCInternalException($'Broken Quick.Invoke detected');
      {$endif DEBUG}
    end;
    
  end;
  
{$endregion CLTaskLocalData}

{$region QueueRes}

type
  {$region Base}
  
  IQueueRes = interface
    
    property ResEv: EventList read;
    
    procedure AddAction(d: QueueResAction);
    property HasActions: boolean read;
    procedure InvokeActions(c: Context);
    function ShouldInstaCallAction: boolean;
    
  end;
  
  IQueueResBaseFactory<TR> = interface
    where TR: IQueueRes;
    
    function MakeDelayed(l: CLTaskLocalData): TR;
    function MakeDelayed(make_l: TR->CLTaskLocalData): TR;
    
  end;
  
  [StructLayout(LayoutKind.Auto)]
  QueueResData = record
    private complition_delegate  := default(QueueResComplDelegateData);
    private ev                   := EventList.Empty;
    
    public static function operator implicit(base: QueueResData): CLTaskLocalData;
    begin
      Result := new CLTaskLocalData(base.ev);
      Result.prev_delegate := base.complition_delegate;
    end;
    
    public property ResEv: EventList read ev;
    
    public procedure AddAction(d: QueueResAction);
    begin
      {$ifdef DEBUG}
      if ShouldInstaCallAction then raise new OpenCLABCInternalException($'Broken Quick.Invoke detected');
      {$endif DEBUG}
      complition_delegate.AddAction(d);
    end;
    
    public procedure InvokeActions(c: Context) := complition_delegate.Invoke(c);
    
    public function ShouldInstaCallAction := CLTaskLocalData(self).ShouldInstaCallAction;
    
    protected procedure Finalize; override;
    begin
      {$ifdef DEBUG}
      if not complition_delegate.was_invoked then raise new System.InvalidProgramException($'{self.GetType}');
      {$endif DEBUG}
    end;
    
  end;
  
  {$endregion Base}
  
  {$region Nil}
  
  [StructLayout(LayoutKind.Auto)]
  QueueResNil = record(IQueueRes)
    private base: QueueResData;
    
    public constructor(l: CLTaskLocalData);
    begin
      base.ev := l.prev_ev;
      base.complition_delegate := l.prev_delegate;
    end;
    public constructor := raise new OpenCLABCInternalException;
    
    public property ResEv: EventList read base.ResEv;
    
    public procedure AddAction(d: QueueResAction) := base.AddAction(d);
    public property HasActions: boolean read base.complition_delegate.count<>0;
    public procedure InvokeActions(c: Context) := base.InvokeActions(c);
    public function ShouldInstaCallAction := base.ShouldInstaCallAction;
    
  end;
  QueueResNilFactory = record(IQueueResBaseFactory<QueueResNil>)
    
    function MakeDelayed(l: CLTaskLocalData) := new QueueResNil(l);
    function MakeDelayed(make_l: QueueResNil->CLTaskLocalData) := new QueueResNil(make_l(default(QueueResNil)));
    
  end;
  
  {$endregion Nil}
  
  {$region <T>}
  
  {$region General}
  
  QueueResT = abstract partial class(IQueueRes)
    private base: QueueResData;
    private res_const: boolean; // Whether res can be read before event completes
    {$ifdef DEBUG}
    private res_setter_exists := false;
    {$endif DEBUG}
    
    public property ResEv: EventList read; abstract;
    
    public procedure AddAction(d: QueueResAction) := base.AddAction(d);
    public property HasActions: boolean read base.complition_delegate.count<>0;
    public procedure InvokeActions(c: Context) := base.InvokeActions(c);
    public function ShouldInstaCallAction := base.ShouldInstaCallAction;
    
    public function TakeBaseOut: QueueResData;
    begin
      Result := self.base;
      self.base := default(QueueResData);
    end;
    
  end;
  
  QueueRes<T> = abstract partial class(QueueResT) end;
  IQueueResFactory<T,TR> = interface(IQueueResBaseFactory<TR>)
    where TR: QueueRes<T>;
    
    function MakeConst(l: CLTaskLocalData; res: T): TR;
    
  end;
  
  QueueRes<T> = abstract partial class(QueueResT)
    
    protected procedure InitDelayed(l: CLTaskLocalData);
    begin
      {$ifdef DEBUG}
      if l.prev_ev.count=0 then raise new OpenCLABCInternalException($'Delayed QueueRes, but it is not delayed');
      {$endif DEBUG}
      base.ev := l.prev_ev;
      base.complition_delegate := l.prev_delegate;
    end;
    
    protected procedure InitConst(l: CLTaskLocalData; res: T);
    begin
      base.ev := l.prev_ev;
      base.complition_delegate := l.prev_delegate;
      SetResDirect(res);
      res_const := true;
    end;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function WrapResult<TR>(factory: IQueueResFactory<T,TR>; new_ev: EventList): TR; where TR: QueueRes<T>;
    begin
      // It is expected that new_ev would have actions already attached
      var l := new CLTaskLocalData(new_ev);
      if res_const then
        Result := factory.MakeConst(l, self.GetResDirect) else
      begin
        Result := factory.MakeDelayed(l);
        Result.AddResSetter(c->self.GetResDirect);
      end;
    end;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function TransformResult<T2,TR>(factory: IQueueResFactory<T2,TR>; c: Context; can_insta_call: boolean; transform: (T,Context)->T2): TR; where TR: QueueRes<T2>;
    begin
      var res_l := CLTaskLocalData(self.TakeBaseOut);
      
      if res_l.ShouldInstaCallAction or (res_const and can_insta_call) then
        Result := factory.MakeConst(res_l, transform(self.GetResDirect, c)) else
      begin
        Result := factory.MakeDelayed(res_l);
        Result.AddResSetter(c->transform(self.GetResDirect, c));
      end;
      
    end;
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function TransformResultErrWrap<T2,TR>(factory: IQueueResFactory<T2,TR>; g: CLTaskGlobalData; can_insta_call: boolean; transform: (T,Context)->T2): TR; where TR: QueueRes<T2>;
    begin
      //TODO #2644: &<>
      transform := QueueResActionUtils.HandlerWrap&<T,T2>(g.curr_err_handler, transform);
      Result := TransformResult(factory, g.c, can_insta_call, transform);
    end;
    
    public property ResEv: EventList read base.ResEv; override;
    
    public property IsConst: boolean read res_const;
    
    public procedure AddResSetter(d: Context->T);
    begin
      {$ifdef DEBUG}
      if res_const then raise new OpenCLABCInternalException($'Result setter action on const qr');
      if res_setter_exists then raise new OpenCLABCInternalException($'Multiple result setter actions');
      res_setter_exists := true;
      {$endif DEBUG}
      AddAction(c->self.SetRes(d(c)));
    end;
    public procedure SetRes(res: T);
    begin
      {$ifdef DEBUG}
      if res_const then raise new OpenCLABCInternalException($'');
      {$endif DEBUG}
      SetResDirect(res);
    end;
    protected procedure SetResDirect(res: T); abstract;
    public function GetRes(c: Context): T;
    begin
      InvokeActions(c);
      Result := GetResDirect;
    end;
    protected function GetResDirect: T; abstract;
    
  end;
  
  {$endregion General}
  
  {$region Val}
  
  QueueResVal<T> = sealed class(QueueRes<T>)
    private res: T;
    
    public constructor(l: CLTaskLocalData) := InitDelayed(l);
    public constructor(make_l: QueueResVal<T>->CLTaskLocalData) := InitDelayed(make_l(self));
    
    public constructor(l: CLTaskLocalData; res: T) := InitConst(l, res);
    public constructor(make_l: QueueResVal<T>->CLTaskLocalData; res: T) := InitConst(make_l(self), res);
    
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure SetResDirect(res: T); override := self.res := res;
    protected function GetResDirect: T; override := self.res;
    
  end;
  QueueResValFactory<T> = record(IQueueResFactory<T, QueueResVal<T>>)
    
    public function MakeConst(l: CLTaskLocalData; res: T) := new QueueResVal<T>(l, res);
    
    public function MakeDelayed(l: CLTaskLocalData) := new QueueResVal<T>(l);
    public function MakeDelayed(make_l: QueueResVal<T>->CLTaskLocalData) := new QueueResVal<T>(make_l);
    
  end;
  QueueRes<T> = abstract partial class(QueueResT)
    public static val_factory := new QueueResValFactory<T>;
  end;
  
  {$endregion Val}
  
  {$region Ptr}
  
  QueueResPtr<T> = sealed class(QueueRes<T>)
    private res: ^T := pointer(Marshal.AllocHGlobal(Marshal.SizeOf&<T>));
    
    static constructor := BlittableHelper.RaiseIfBad(typeof(T), 'использовать в некоторой внутренней ситуации (напишите об этом в issue)');
    
    public constructor(l: CLTaskLocalData) := InitDelayed(l);
    public constructor(make_l: QueueResPtr<T>->CLTaskLocalData) := InitDelayed(make_l(self));
    
    public constructor(l: CLTaskLocalData; res: T) := InitConst(l, res);
    public constructor(make_l: QueueResPtr<T>->CLTaskLocalData; res: T) := InitConst(make_l(self), res);
    
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure Finalize; override;
    begin
      Marshal.FreeHGlobal(new IntPtr(res));
      inherited;
    end;
    
    protected procedure SetResDirect(res: T); override := self.res^ := res;
    protected function GetResDirect: T; override := self.res^;
    
  end;
  QueueResPtrFactory<T> = record(IQueueResFactory<T, QueueResPtr<T>>)
    
    public function MakeConst(l: CLTaskLocalData; res: T) := new QueueResPtr<T>(l, res);
    
    public function MakeDelayed(l: CLTaskLocalData) := new QueueResPtr<T>(l);
    public function MakeDelayed(make_l: QueueResPtr<T>->CLTaskLocalData) := new QueueResPtr<T>(make_l);
    
  end;
  QueueRes<T> = abstract partial class(QueueResT)
    public static ptr_factory := new QueueResPtrFactory<T>;
  end;
  
  {$endregion Ptr}
  
  {$endregion <T>}
  
//TODO #????
procedure TODO := exit;

[MethodImpl(MethodImplOptions.AggressiveInlining)]
function AttachInvokeActions(self: IQueueRes; g: CLTaskGlobalData): EventList; extensionmethod;
begin
  if not self.HasActions then
  begin
    Result := self.ResEv;
    exit;
  end else
  {$ifdef DEBUG}
  if self.ShouldInstaCallAction then // auto raise
  {$endif DEBUG}
    ;
  
  var uev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'res_ev for {self.GetType}.ThenAttachInvokeActions, after [{self.ResEv.evs?.JoinToString}]'{$endif});
  Result := uev;
  
  var c := g.c;
  self.ResEv.MultiAttachCallback(()->
  begin
    self.InvokeActions(c);
    uev.SetComplete;
  end{$ifdef EventDebug}, $'body of {self.GetType}.ThenAttachInvokeActions with res_ev={uev}'{$endif});
  
end;
//TODO #????
function AttachInvokeActions<T>(self: QueueRes<T>; g: CLTaskGlobalData); extensionmethod := (self as IQueueRes).AttachInvokeActions(g);

{$endregion QueueRes}

{$region MultiusableBase}

type
  IMultiusableCommandQueueHub = interface end;
  [StructLayout(LayoutKind.Auto)]
  MultiuseableResultData = record
    public qres: IQueueRes;
    public ev: EventList;
    public err_handler: CLTaskErrHandler;
    
    public constructor(qres: IQueueRes; ev: EventList; err_handler: CLTaskErrHandler);
    begin
      self.qres := qres;
      self.ev := ev;
      self.err_handler := err_handler;
    end;
    
  end;
  
{$endregion MultiusableBase}

{$region CLTaskGlobalData/BkanchInvoker}

type
  CLTaskBranchInvoker = sealed class
    private g: CLTaskGlobalData;
    private prev_ev: EventList?;
    private prev_cq := cl_command_queue.Zero;
    private branch_handlers := new List<CLTaskErrHandler>;
    private make_base_err_handler: ()->CLTaskErrHandler;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    constructor(g: CLTaskGlobalData; prev_ev: EventList?; capacity: integer);
    begin
      self.g := g;
      self.prev_ev := prev_ev;
      
      if g.curr_inv_cq<>cl_command_queue.Zero then
      begin
        {$ifdef DEBUG}
        if g.outer_cq<>cl_command_queue.Zero then raise new OpenCLABCInternalException($'outer_cq should be taken when curr_inv_cq is not Zero');
        {$endif DEBUG}
        
        // Make outer only if ParallelInvoke is said to wait for event of current cq
        // Otherwise command parameters would be added to outer cq, causing them to wait anyway
        if prev_ev<>nil then
        begin
          {$ifdef DEBUG}
          if prev_ev.Value.count=0 then raise new OpenCLABCInternalException($'prev_ev should not be Zero when curr_inv_cq is not Zero');
          {$endif DEBUG}
          g.outer_cq := g.curr_inv_cq;
        end else
          self.prev_cq := g.curr_inv_cq;
        
        g.curr_inv_cq := cl_command_queue.Zero;
      end;
      
      self.branch_handlers.Capacity := capacity;
      if prev_ev=nil then
        self.make_base_err_handler := ()->new CLTaskErrHandlerEmpty else
      begin
        var origin_handler := g.curr_err_handler;
        self.make_base_err_handler := ()->new CLTaskErrHandlerBranchBase(origin_handler);
      end;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function InvokeBranch<TR>(branch: (CLTaskGlobalData, CLTaskLocalData)->TR): TR; where TR: IQueueRes;
    begin
      g.curr_err_handler := make_base_err_handler();
      var l := if self.prev_ev=nil then
        new CLTaskLocalData else
        new CLTaskLocalData(self.prev_ev.Value);
      
      Result := branch(g, l);
      
      var cq := g.curr_inv_cq;
      if cq<>cl_command_queue.Zero then
      begin
        g.curr_inv_cq := cl_command_queue.Zero;
        if prev_cq=cl_command_queue.Zero then
          prev_cq := cq else
          Result.AddAction(c->self.g.ReturnCQ(cq));
      end;
      
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
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    procedure ParallelInvoke(l: CLTaskLocalData?; capacity: integer; use: Action<CLTaskBranchInvoker>);
    begin
      var prev_ev := default(EventList?);
      if l<>nil then
      begin
        var ev := QueueResNil.Create(l.Value).AttachInvokeActions(self);
        if ev.count<>0 then loop capacity-1 do
          ev.Retain({$ifdef EventDebug}$'for all async branches'{$endif});
        prev_ev := ev;
      end;
      
      var invoker := new CLTaskBranchInvoker(self, prev_ev, capacity);
      var origin_handler := self.curr_err_handler;
      
      use(invoker);
      
      {$ifdef DEBUG}
      if invoker.branch_handlers.Count<>capacity then
        raise new OpenCLABCInternalException($'{invoker.branch_handlers.Count} <> {capacity}');
      {$endif DEBUG}
      self.curr_err_handler := new CLTaskErrHandlerBranchCombinator(origin_handler, invoker.branch_handlers.ToArray);
      
      self.curr_inv_cq := invoker.prev_cq;
      if outer_cq<>cl_command_queue.Zero then self.GetCQ(false);
    end;
    
    public procedure FinishInvoke;
    begin
      
      // mu выполняют лишний .Retain, чтобы ивент не удалился пока очередь ещё запускается
      foreach var mrd in mu_res.Values do
        mrd.ev.Release({$ifdef EventDebug}$'excessive mu ev'{$endif});
      mu_res := nil;
      
    end;
    
    public procedure FinishExecution(var err_lst: List<Exception>);
    begin
      
      if curr_inv_cq<>cl_command_queue.Zero then
        ReturnCQ(curr_inv_cq);
      
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); abstract;
    
    protected static qr_nil_factory := new QueueResNilFactory;
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; abstract;
    
  end;
  
  CommandQueueNil = abstract partial class(CommandQueueBase)
    
  end;
  
  CommandQueue<T> = abstract partial class(CommandQueueBase)
    
    protected static qr_val_factory := QueueRes&<T>.val_factory;
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; abstract;
    
    protected static qr_ptr_factory := QueueRes&<T>.ptr_factory;
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; abstract;
    
    protected function InvokeToAny(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<T>; virtual := InvokeToVal(g, l);
    
  end;
  
  CommandQueueInvoker<TR> = (CLTaskGlobalData,CLTaskLocalData)->TR;
  
{$endregion Base}

{$region Const}

type
  ConstQueue<T> = sealed partial class(CommandQueue<T>)
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;    override := new QueueResNil(l);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := new QueueResVal<T>(l, self.res);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := new QueueResPtr<T>(l, self.res);
    
  end;
  
{$endregion Const}

{$region Parameter}

type
  ParameterQueue<T> = sealed partial class(CommandQueue<T>, IParameterQueue)
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      //TODO #????
      if g.parameters.ContainsKey(self as object as IParameterQueue) then exit;
      //TODO #????
      g.parameters[self as object as IParameterQueue] := if self.def_is_set then
        new CLTaskParameterData(self.def) else
        new CLTaskParameterData;
    end;
    
    private function GetParVal(g: CLTaskGlobalData) :=
    //TODO #????
    T(g.parameters[self as object as IParameterQueue].val);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;    override := new QueueResNil(l);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := new QueueResVal<T>(l, self.GetParVal(g));
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := new QueueResPtr<T>(l, self.GetParVal(g));
    
  end;
  
{$endregion Parameter}

{$endregion CommandQueue}

{$region CLTask}

type
  CLTaskBase = abstract partial class
    
  end;
  
  CLTaskNil = sealed partial class(CLTaskBase)
    
    private constructor(q: CommandQueueNil; c: Context; pars: array of ParameterQueueSetter);
    begin
      self.q := q;
      self.org_c := c;
      
      var g := new CLTaskGlobalData(self);
      
      q.InitBeforeInvoke(g, new HashSet<IMultiusableCommandQueueHub>);
      g.ApplyParameters(pars);
      var qr := q.InvokeToNil(g, new CLTaskLocalData);
      g.FinishInvoke;
      
      var mre := qr.ResEv.ToMRE({$ifdef EventDebug}$'CLTaskNil.FinishExecution'{$endif});
      NativeUtils.StartNewBgThread(()->
      begin
        if mre<>nil then mre.WaitOne;
        qr.InvokeActions(self.org_c);
        g.FinishExecution(self.err_lst);
        self.wh.Set;
      end);
      
    end;
    
  end;
  CLTask<T> = sealed partial class(CLTaskBase)
    private res: T;
    
    private constructor(q: CommandQueue<T>; c: Context; pars: array of ParameterQueueSetter);
    begin
      self.q := q;
      self.org_c := c;
      
      var g := new CLTaskGlobalData(self);
      
      q.InitBeforeInvoke(g, new HashSet<IMultiusableCommandQueueHub>);
      g.ApplyParameters(pars);
      var qr := q.InvokeToAny(g, new CLTaskLocalData);
      g.FinishInvoke;
      
      var mre := qr.ResEv.ToMRE({$ifdef EventDebug}$'CLTask<{typeof(T)}>.FinishExecution'{$endif});
      NativeUtils.StartNewBgThread(()->
      begin
        if mre<>nil then mre.WaitOne;
        self.res := qr.GetRes(self.org_c);
        g.FinishExecution(self.err_lst);
        self.wh.Set;
      end);
      
    end;
    
  end;
  
  CLTaskFactory = record(ITypedCQConverter<CLTaskBase>)
    private c: Context;
    private pars: array of ParameterQueueSetter;
    public constructor(c: Context; pars: array of ParameterQueueSetter);
    begin
      self.c := c;
      self.pars := pars;
    end;
    public constructor := raise new OpenCLABCInternalException;
    
    public function ConvertNil(cq: CommandQueueNil): CLTaskBase := new CLTaskNil(cq, c, pars);
    public function Convert<T>(cq: CommandQueue<T>): CLTaskBase := new CLTask<T>(cq, c, pars);
    
  end;
  
function Context.BeginInvoke(q: CommandQueueBase; params parameters: array of ParameterQueueSetter) := q.ConvertTyped(new CLTaskFactory(self, parameters));
function Context.BeginInvoke(q: CommandQueueNil; params parameters: array of ParameterQueueSetter) := new CLTaskNil(q, self, parameters);
function Context.BeginInvoke<T>(q: CommandQueue<T>; params parameters: array of ParameterQueueSetter) := new CLTask<T>(q, self, parameters);

function CLTask<T>.WaitRes: T;
begin
  Wait;
  Result := self.res;
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
        raise new System.InvalidCastException($'.Cast не может преобразовывать nil в {TypeToTypeName(typeof(T))}');
    end;
    public constructor(q: CommandQueueNil) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := q.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := q.InvokeToNil(g, l);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := new QueueResVal<T>(q.InvokeToNil(g, l).base, nil_val);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := new QueueResPtr<T>(q.InvokeToNil(g, l).base, nil_val);
    
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
        raise new System.InvalidCastException($'.Cast не может преобразовывать {TypeToTypeName(typeof(TInp))} в {TypeToTypeName(typeof(TRes))}');
      end;
    end;
    public constructor(q: CommandQueue<TInp>) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    public property SourceBase: CommandQueueBase read q as CommandQueueBase; override;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := q.InvokeToNil(g, l);
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResFactory<TRes,TR>): TR; where TR: QueueRes<TRes>;
    begin
      var prev_qr := q.InvokeToAny(g,l);
      Result := prev_qr.TransformResultErrWrap(qr_factory, g, true, (o,c)->TRes(object(o)));
    end;
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke(g, l, qr_val_factory);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke(g, l, qr_ptr_factory);
    
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

function CommandQueueNil.Cast<T> := new TypedNilQueue<T>(self);

{$endregion Cast}

{$region DiscardResult}

type
  CommandQueueDiscardResult<T> = sealed class(CommandQueueNil)
    private q: CommandQueue<T>;
    
    public constructor(q: CommandQueue<T>) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := q.InvokeToNil(g, l);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
function CommandQueue<T>.DiscardResult :=
new CommandQueueDiscardResult<T>(self);

{$endregion DiscardResult}

{$region BackgroundConvertQueue}

type
  BackgroundConvertQueue<TInp,TRes> = abstract class(CommandQueue<TRes>)
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TInp>; abstract;
    
    protected function ExecFunc(o: TInp; c: Context): TRes; abstract;
    
    private function MakeNilBody    (prev_qr: QueueRes<TInp>; err_handler: CLTaskErrHandler; c: Context; own_qr: QueueResNil): Action := ()->
    begin
      var inp := prev_qr.GetRes(c);
      if err_handler.HadError(true) then exit;
      try
        ExecFunc(inp, c);
      except
        on e: Exception do err_handler.AddErr(e);
      end;
    end;
    private function MakeResBody<TR>(prev_qr: QueueRes<TInp>; err_handler: CLTaskErrHandler; c: Context; own_qr: TR): Action; where TR: QueueRes<TRes>;
    begin
      Result := ()->
      begin
        var inp := prev_qr.GetRes(c);
        if err_handler.HadError(true) then exit;
        var res: TRes;
        try
          res := ExecFunc(inp, c);
        except
          on e: Exception do err_handler.AddErr(e);
        end;
        own_qr.SetRes(res);
      end;
    end;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResBaseFactory<TR>; make_body: (QueueRes<TInp>,CLTaskErrHandler,Context,TR)->Action): TR; where TR: IQueueRes;
    begin
      var prev_qr := InvokeSubQs(g, l);
      
      Result := qr_factory.MakeDelayed(qr->new CLTaskLocalData(UserEvent.StartBackgroundWork(
        prev_qr.ResEv, make_body(prev_qr, g.curr_err_handler, g.c, qr), g.cl_c
        {$ifdef EventDebug}, $'body of {self.GetType}'{$endif}
      )));
      
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, MakeNilBody);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke(g, l, qr_val_factory, MakeResBody&<QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke(g, l, qr_ptr_factory, MakeResBody&<QueueResPtr<TRes>>);
    
  end;
  
{$endregion BackgroundConvertQueue}

{$region ThenBackgroundConvert}

type
  CommandQueueThenBackgroundConvertBase<TInp, TRes, TFunc> = abstract class(BackgroundConvertQueue<TInp, TRes>)
  where TFunc: Delegate;
    private q: CommandQueue<TInp>;
    private f: TFunc;
    
    public constructor(q: CommandQueue<TInp>; f: TFunc);
    begin
      self.q := q;
      self.f := f;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TInp>; override := q.InvokeToAny(g, l);
    
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

{$endregion ThenBackgroundConvert}

{$region ThenBackgroundUse}

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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.InitBeforeInvoke(g, inited_hubs);
    
    protected procedure ExecProc(o: T; c: Context); abstract;
    
    private function MakeNilBody    (prev_qr: QueueRes<T>; err_handler: CLTaskErrHandler; c: Context): Action := ()->
    begin
      var res := prev_qr.GetRes(c);
      if err_handler.HadError(true) then exit;
      try
        ExecProc(res, c);
      except
        on e: Exception do err_handler.AddErr(e);
      end;
    end;
    private function MakeResBody<TR>(prev_qr: QueueRes<T>; err_handler: CLTaskErrHandler; c: Context; own_qr: TR): Action; where TR: QueueRes<T>;
    begin
      Result := ()->
      begin
        var res := prev_qr.GetRes(c);
        if err_handler.HadError(true) then exit;
        try
          ExecProc(res, c);
        except
          on e: Exception do err_handler.AddErr(e);
        end;
        own_qr.SetRes(res);
      end;
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var prev_qr := q.InvokeToAny(g, l);
      
      Result := new QueueResNil(new CLTaskLocalData(UserEvent.StartBackgroundWork(
        prev_qr.ResEv, MakeNilBody(prev_qr, g.curr_err_handler, g.c), g.cl_c
        {$ifdef EventDebug}, $'nil body of {self.GetType}'{$endif}
      )));
      
    end;
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResFactory<T,TR>): TR; where TR: QueueRes<T>;
    begin
      var prev_qr := q.InvokeToAny(g, l);
      
      Result := if prev_qr.IsConst then
        qr_factory.MakeConst(
          new CLTaskLocalData(UserEvent.StartBackgroundWork(
            prev_qr.ResEv, MakeNilBody(prev_qr, g.curr_err_handler, g.c), g.cl_c
            {$ifdef EventDebug}, $'const body of {self.GetType}'{$endif}
          )), prev_qr.GetResDirect
        ) else
        qr_factory.MakeDelayed(
          qr->new CLTaskLocalData(UserEvent.StartBackgroundWork(
            prev_qr.ResEv, MakeResBody(prev_qr, g.curr_err_handler, g.c, qr), g.cl_c
            {$ifdef EventDebug}, $'delayed body of {self.GetType}'{$endif}
          ))
        );
      
    end;
    
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := Invoke(g, l, qr_val_factory);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := Invoke(g, l, qr_ptr_factory);
    
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

{$endregion ThenBackgroundUse}

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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := q.InitBeforeInvoke(g, inited_hubs);
    
    protected function ExecFunc(o: TInp; c: Context): TRes; abstract;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var prev_qr := q.InvokeToAny(g, l);
      Result := new QueueResNil(prev_qr.TakeBaseOut);
      
      var d := QueueResActionUtils.HandlerWrapStrip(g.curr_err_handler, ExecFunc);
      if Result.ShouldInstaCallAction then
        d(prev_qr.GetResDirect, g.c) else
        Result.AddAction(c->d(prev_qr.GetResDirect,c));
      
    end;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResFactory<TRes,TR>): TR; where TR: QueueRes<TRes>;
    begin
      var prev_qr := q.InvokeToAny(g, l);
      Result := prev_qr.TransformResultErrWrap(qr_factory, g, false, ExecFunc);
    end;
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke(g, l, qr_val_factory);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke(g, l, qr_ptr_factory);
    
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := q.InitBeforeInvoke(g, inited_hubs);
    
    protected procedure ExecProc(o: T; c: Context); abstract;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function AddUse<TR1, TR2>(prev_qr: TR1; own_qr: TR2; g: CLTaskGlobalData): TR2; where TR1: QueueRes<T>; where TR2: IQueueRes;
    begin
      Result := own_qr;
      
      var d := QueueResActionUtils.HandlerWrap(g.curr_err_handler, ExecProc);
      if Result.ShouldInstaCallAction then
        d(prev_qr.GetResDirect, g.c) else
        Result.AddAction(c->d(prev_qr.GetResDirect, c));
      
    end;
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function AddUse<TR>(qr: TR; g: CLTaskGlobalData): TR; where TR: QueueRes<T>;
    begin
      Result := AddUse(qr,qr, g);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var prev_qr := q.InvokeToAny(g, l);
      Result := AddUse(prev_qr, new QueueResNil(prev_qr.TakeBaseOut), g);
    end;
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := AddUse(q.InvokeToVal(g, l), g);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := AddUse(q.InvokeToPtr(g, l), g);
    
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
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function GetQS: sequence of CommandQueueBase := qs.Append&<CommandQueueBase>(last);
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>);
    begin
      foreach var q in qs do q.InitBeforeInvoke(g, inited_hubs);
      last.InitBeforeInvoke(g, inited_hubs);
    end;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function InvokeSync<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; invoke_last: CommandQueueInvoker<TR>): TR; where TR: IQueueRes;
    begin
      for var i := 0 to qs.Length-1 do
        l := qs[i].InvokeToNil(g, l).base;
      
      Result := invoke_last(g, l);
    end;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function InvokeAsync<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; invoke_last: CommandQueueInvoker<TR>): ValueTuple<TR, EventList>; where TR: IQueueRes;
    begin
      var evs := new EventList[qs.Length+1];
      
      var res: TR;
      g.ParallelInvoke(l, qs.Length+1, invoker->
      begin
        for var i := 0 to qs.Length-1 do
          //TODO #2610
          evs[i] := invoker.InvokeBranch&<IQueueRes>((g,l)->
            qs[i].InvokeToNil(g, l)
          ).AttachInvokeActions(g);
        var l_res := invoker.InvokeBranch(invoke_last);
        res := l_res;
        evs[qs.Length] := l_res.AttachInvokeActions(g);
      end);
      
      Result := ValueTuple.Create(res, EventList.Combine(evs));
    end;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    data.InitBeforeInvoke(g, inited_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    data.ToString(sb, tabs, index, delayed);
    
  end;
  
  SimpleSyncQueueArrayNil = sealed class(SimpleQueueArrayNil, ISimpleSyncQueueArray)
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override :=
    data.InvokeSync(g, l, data.last.InvokeToNil);
  end;
  SimpleAsyncQueueArrayNil = sealed class(SimpleQueueArrayNil, ISimpleAsyncQueueArray)
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override :=
    new QueueResNil(new CLTaskLocalData( data.InvokeAsync(g, l, data.last.InvokeToNil).Item2 ));
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    data.InitBeforeInvoke(g, inited_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    data.ToString(sb, tabs, index, delayed);
    
  end;
  
  SimpleSyncQueueArray<T> = sealed class(SimpleQueueArray<T>, ISimpleSyncQueueArray)
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;    override := data.InvokeSync(g, l, data.last.InvokeToNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := data.InvokeSync(g, l, data.last.InvokeToVal);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := data.InvokeSync(g, l, data.last.InvokeToPtr);
    
  end;
  SimpleAsyncQueueArray<T> = sealed class(SimpleQueueArray<T>, ISimpleAsyncQueueArray)
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override :=
    new QueueResNil(new CLTaskLocalData( data.InvokeAsync(g, l, data.last.InvokeToNil).Item2 ));
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResFactory<T,TR>): TR; where TR: QueueRes<T>;
    begin
      var (prev_qr, ev) := data.InvokeAsync(g, l, data.last.InvokeToAny);
      Result := prev_qr.WrapResult(qr_factory, ev);
    end;
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := Invoke(g, l, qr_val_factory);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := Invoke(g, l, qr_ptr_factory);
    
  end;
  
{$endregion Simple}

{$region Conv}

{$region Generic}

type
  
  {$region Background}
  
  {$region Base}
  
  BackgroundConvQueueArrayBase<TInp, TRes, TFunc> = abstract class(BackgroundConvertQueue<array of TInp, TRes>)
  where TFunc: Delegate;
    protected qs: array of CommandQueue<TInp>;
    protected f: TFunc;
    
    public constructor(qs: array of CommandQueue<TInp>; f: TFunc);
    begin
      self.qs := qs;
      self.f := f;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    foreach var q in qs do q.InitBeforeInvoke(g, inited_hubs);
    
    protected [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function CombineQRs(qrs: array of QueueRes<TInp>; l: CLTaskLocalData): QueueResVal<array of TInp>;
    begin
      if qrs.All(qr->qr.IsConst) then
      begin
        var res := qrs.ConvertAll(qr->qr.GetResDirect);
        Result := new QueueResVal<array of TInp>(l, res);
      end else
      begin
        Result := new QueueResVal<array of TInp>(l);
        Result.AddResSetter(c->qrs.ConvertAll(qr->qr.GetResDirect));
      end;
    end;
    
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<array of TInp>; override;
    begin
      var qrs := new QueueRes<TInp>[qs.Length];
      
      for var i := 0 to qs.Length-1 do
      begin
        var qr := qs[i].InvokeToAny(g, l);
        l := qr.TakeBaseOut;
        qrs[i] := qr;
      end;
      
      Result := CombineQRs(qrs, l);
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<array of TInp>; override;
    begin
      var qrs := new QueueRes<TInp>[qs.Length];
      var evs := new EventList[qs.Length];
      
      g.ParallelInvoke(l, qs.Length, invoker->
      for var i := 0 to qs.Length-1 do
      begin
        var qr := invoker.InvokeBranch(qs[i].InvokeToAny);
        qrs[i] := qr;
        evs[i] := qr.AttachInvokeActions(g);
      end);
      
      var res_ev := EventList.Combine(evs);
      Result := CombineQRs(qrs, new CLTaskLocalData(res_ev));
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    foreach var q in qs do q.InitBeforeInvoke(g, inited_hubs);
    
    protected function ExecFunc(o: array of TInp; c: Context): TRes; abstract;
    
    protected [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function CombineQRsNil(qrs: array of QueueRes<TInp>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;
    begin
      Result := new QueueResNil(l);
      var d := QueueResActionUtils.HandlerWrapStrip(g.curr_err_handler, ExecFunc);
      if l.ShouldInstaCallAction then
        d(qrs.ConvertAll(qr->qr.GetResDirect), g.c) else
        Result.AddAction(c->d(qrs.ConvertAll(qr->qr.GetResDirect), c));
    end;
    
    protected [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function CombineQRsRes<TF,TR>(qrs: array of QueueRes<TInp>; g: CLTaskGlobalData; l: CLTaskLocalData): TR; where TF: IQueueResFactory<TRes,TR>, constructor; where TR: QueueRes<TRes>;
    begin
      //TODO #2644: &<>
      var d := QueueResActionUtils.HandlerWrap&<array of TInp, TRes>(g.curr_err_handler, ExecFunc);
      if l.ShouldInstaCallAction then
      begin
        var res := d(qrs.ConvertAll(qr->qr.GetResDirect), g.c);
        Result := TF.Create.MakeConst(l, res);
      end else
      begin
        Result := TF.Create.MakeDelayed(l);
        Result.AddResSetter(c->d(qrs.ConvertAll(qr->qr.GetResDirect), c));
      end;
    end;
    
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
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<array of QueueRes<TInp>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qrs := new QueueRes<TInp>[qs.Length];
      
      for var i := 0 to qs.Length-1 do
      begin
        var qr := qs[i].InvokeToAny(g, l);
        l := qr.TakeBaseOut;
        qrs[i] := qr;
      end;
      
      Result := CombineQRs(qrs, g, l);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    //TODO #????
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
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
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<array of QueueRes<TInp>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qrs := new QueueRes<TInp>[qs.Length];
      var evs := new EventList[qs.Length];
      
      g.ParallelInvoke(l, qs.Length, invoker->
      for var i := 0 to qs.Length-1 do
      begin
        var qr := invoker.InvokeBranch(qs[i].InvokeToAny);
        evs[i] := qr.AttachInvokeActions(g);
        qrs[i] := qr;
      end);
      
      var res_ev := EventList.Combine(evs);
      Result := CombineQRs(qrs, g, new CLTaskLocalData(res_ev));
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    //TODO #????
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
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
  BackgroundConvQueueArray2Base<TInp1, TInp2, TRes, TFunc> = abstract class(BackgroundConvertQueue<ValueTuple<TInp1, TInp2>, TRes>)
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; l: CLTaskLocalData): QueueResVal<ValueTuple<TInp1, TInp2>>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res := ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect);
        Result := new QueueResVal<ValueTuple<TInp1, TInp2>>(l, res);
      end else
      begin
        Result := new QueueResVal<ValueTuple<TInp1, TInp2>>(l);
        Result.AddResSetter(c->ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect));
      end;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; c: Context): TRes; abstract;
    
    protected function CombineQRsNil(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;
    begin
      Result := new QueueResNil(l);
      if l.ShouldInstaCallAction then
      begin
        if not g.curr_err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
      end else
      begin
        var err_handler := g.curr_err_handler;
        Result.AddAction(c->
        if not err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
    end;
    
    protected function CombineQRsRes<TF,TR>(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; g: CLTaskGlobalData; l: CLTaskLocalData): TR; where TF: IQueueResFactory<TRes,TR>, constructor; where TR: QueueRes<TRes>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res: TRes;        if not g.curr_err_handler.HadError(true) then
        try
          res := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
        Result := TF.Create.MakeConst(l, res);
      end else
      begin
        Result := TF.Create.MakeDelayed(l);
        var err_handler := g.curr_err_handler;
        Result.AddResSetter(c->
        if not err_handler.HadError(true) then
        try
          Result := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2>>; override;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, l);
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2>>; override;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      g.ParallelInvoke(l, 2, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, new CLTaskLocalData(res_ev));
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
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, g, l);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
  end;
  
  QuickConvSyncQueueArray2<TInp1, TInp2, TRes> = sealed class(QuickConvSyncQueueArray2Base<TInp1, TInp2, TRes, (TInp1, TInp2)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; c: Context): TRes; override := f(o1, o2);
    
  end;
  QuickConvSyncQueueArray2C<TInp1, TInp2, TRes> = sealed class(QuickConvSyncQueueArray2Base<TInp1, TInp2, TRes, (TInp1, TInp2, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; c: Context): TRes; override := f(o1, o2, c);
    
  end;
  
  QuickConvAsyncQueueArray2Base<TInp1, TInp2, TRes, TFunc> = abstract class(QuickConvQueueArray2Base<TInp1, TInp2, TRes, TFunc>)
  where TFunc: Delegate;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      g.ParallelInvoke(l, 2, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, g, new CLTaskLocalData(res_ev));
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
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
  BackgroundConvQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc> = abstract class(BackgroundConvertQueue<ValueTuple<TInp1, TInp2, TInp3>, TRes>)
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
      self.q3.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; l: CLTaskLocalData): QueueResVal<ValueTuple<TInp1, TInp2, TInp3>>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res := ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect);
        Result := new QueueResVal<ValueTuple<TInp1, TInp2, TInp3>>(l, res);
      end else
      begin
        Result := new QueueResVal<ValueTuple<TInp1, TInp2, TInp3>>(l);
        Result.AddResSetter(c->ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect));
      end;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
      self.q3.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; c: Context): TRes; abstract;
    
    protected function CombineQRsNil(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;
    begin
      Result := new QueueResNil(l);
      if l.ShouldInstaCallAction then
      begin
        if not g.curr_err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
      end else
      begin
        var err_handler := g.curr_err_handler;
        Result.AddAction(c->
        if not err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
    end;
    
    protected function CombineQRsRes<TF,TR>(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; g: CLTaskGlobalData; l: CLTaskLocalData): TR; where TF: IQueueResFactory<TRes,TR>, constructor; where TR: QueueRes<TRes>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res: TRes;        if not g.curr_err_handler.HadError(true) then
        try
          res := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
        Result := TF.Create.MakeConst(l, res);
      end else
      begin
        Result := TF.Create.MakeDelayed(l);
        var err_handler := g.curr_err_handler;
        Result.AddResSetter(c->
        if not err_handler.HadError(true) then
        try
          Result := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3>>; override;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      var qr3 := q3.InvokeToAny(g, l); l := qr3.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, qr3, l);
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3>>; override;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      g.ParallelInvoke(l, 3, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
        qr3 := invoker.InvokeBranch(q3.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g), qr3.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, qr3, new CLTaskLocalData(res_ev));
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
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, QueueRes<TInp3>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      var qr3 := q3.InvokeToAny(g, l); l := qr3.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, qr3, g, l);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
  end;
  
  QuickConvSyncQueueArray3<TInp1, TInp2, TInp3, TRes> = sealed class(QuickConvSyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, (TInp1, TInp2, TInp3)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; c: Context): TRes; override := f(o1, o2, o3);
    
  end;
  QuickConvSyncQueueArray3C<TInp1, TInp2, TInp3, TRes> = sealed class(QuickConvSyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, (TInp1, TInp2, TInp3, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; c: Context): TRes; override := f(o1, o2, o3, c);
    
  end;
  
  QuickConvAsyncQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc> = abstract class(QuickConvQueueArray3Base<TInp1, TInp2, TInp3, TRes, TFunc>)
  where TFunc: Delegate;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, QueueRes<TInp3>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      g.ParallelInvoke(l, 3, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
        qr3 := invoker.InvokeBranch(q3.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g), qr3.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, qr3, g, new CLTaskLocalData(res_ev));
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
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
  BackgroundConvQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc> = abstract class(BackgroundConvertQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4>, TRes>)
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
      self.q3.InitBeforeInvoke(g, prev_hubs);
      self.q4.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; l: CLTaskLocalData): QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4>>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res := ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect);
        Result := new QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4>>(l, res);
      end else
      begin
        Result := new QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4>>(l);
        Result.AddResSetter(c->ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect));
      end;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
      self.q3.InitBeforeInvoke(g, prev_hubs);
      self.q4.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; c: Context): TRes; abstract;
    
    protected function CombineQRsNil(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;
    begin
      Result := new QueueResNil(l);
      if l.ShouldInstaCallAction then
      begin
        if not g.curr_err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
      end else
      begin
        var err_handler := g.curr_err_handler;
        Result.AddAction(c->
        if not err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
    end;
    
    protected function CombineQRsRes<TF,TR>(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; g: CLTaskGlobalData; l: CLTaskLocalData): TR; where TF: IQueueResFactory<TRes,TR>, constructor; where TR: QueueRes<TRes>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res: TRes;        if not g.curr_err_handler.HadError(true) then
        try
          res := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
        Result := TF.Create.MakeConst(l, res);
      end else
      begin
        Result := TF.Create.MakeDelayed(l);
        var err_handler := g.curr_err_handler;
        Result.AddResSetter(c->
        if not err_handler.HadError(true) then
        try
          Result := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4>>; override;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      var qr3 := q3.InvokeToAny(g, l); l := qr3.TakeBaseOut;
      var qr4 := q4.InvokeToAny(g, l); l := qr4.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, qr3, qr4, l);
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4>>; override;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      g.ParallelInvoke(l, 4, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
        qr3 := invoker.InvokeBranch(q3.InvokeToAny);
        qr4 := invoker.InvokeBranch(q4.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g), qr3.AttachInvokeActions(g), qr4.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, new CLTaskLocalData(res_ev));
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
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, QueueRes<TInp3>, QueueRes<TInp4>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      var qr3 := q3.InvokeToAny(g, l); l := qr3.TakeBaseOut;
      var qr4 := q4.InvokeToAny(g, l); l := qr4.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, qr3, qr4, g, l);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
  end;
  
  QuickConvSyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(QuickConvSyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, (TInp1, TInp2, TInp3, TInp4)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; c: Context): TRes; override := f(o1, o2, o3, o4);
    
  end;
  QuickConvSyncQueueArray4C<TInp1, TInp2, TInp3, TInp4, TRes> = sealed class(QuickConvSyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, (TInp1, TInp2, TInp3, TInp4, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; c: Context): TRes; override := f(o1, o2, o3, o4, c);
    
  end;
  
  QuickConvAsyncQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc> = abstract class(QuickConvQueueArray4Base<TInp1, TInp2, TInp3, TInp4, TRes, TFunc>)
  where TFunc: Delegate;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, QueueRes<TInp3>, QueueRes<TInp4>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      g.ParallelInvoke(l, 4, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
        qr3 := invoker.InvokeBranch(q3.InvokeToAny);
        qr4 := invoker.InvokeBranch(q4.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g), qr3.AttachInvokeActions(g), qr4.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, g, new CLTaskLocalData(res_ev));
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
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
  BackgroundConvQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc> = abstract class(BackgroundConvertQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>, TRes>)
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
      self.q3.InitBeforeInvoke(g, prev_hubs);
      self.q4.InitBeforeInvoke(g, prev_hubs);
      self.q5.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; l: CLTaskLocalData): QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res := ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect);
        Result := new QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>(l, res);
      end else
      begin
        Result := new QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>(l);
        Result.AddResSetter(c->ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect));
      end;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
      self.q3.InitBeforeInvoke(g, prev_hubs);
      self.q4.InitBeforeInvoke(g, prev_hubs);
      self.q5.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; c: Context): TRes; abstract;
    
    protected function CombineQRsNil(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;
    begin
      Result := new QueueResNil(l);
      if l.ShouldInstaCallAction then
      begin
        if not g.curr_err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
      end else
      begin
        var err_handler := g.curr_err_handler;
        Result.AddAction(c->
        if not err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
    end;
    
    protected function CombineQRsRes<TF,TR>(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; g: CLTaskGlobalData; l: CLTaskLocalData): TR; where TF: IQueueResFactory<TRes,TR>, constructor; where TR: QueueRes<TRes>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res: TRes;        if not g.curr_err_handler.HadError(true) then
        try
          res := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
        Result := TF.Create.MakeConst(l, res);
      end else
      begin
        Result := TF.Create.MakeDelayed(l);
        var err_handler := g.curr_err_handler;
        Result.AddResSetter(c->
        if not err_handler.HadError(true) then
        try
          Result := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>; override;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      var qr3 := q3.InvokeToAny(g, l); l := qr3.TakeBaseOut;
      var qr4 := q4.InvokeToAny(g, l); l := qr4.TakeBaseOut;
      var qr5 := q5.InvokeToAny(g, l); l := qr5.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, l);
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5>>; override;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      g.ParallelInvoke(l, 5, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
        qr3 := invoker.InvokeBranch(q3.InvokeToAny);
        qr4 := invoker.InvokeBranch(q4.InvokeToAny);
        qr5 := invoker.InvokeBranch(q5.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g), qr3.AttachInvokeActions(g), qr4.AttachInvokeActions(g), qr5.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, new CLTaskLocalData(res_ev));
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
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, QueueRes<TInp3>, QueueRes<TInp4>, QueueRes<TInp5>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      var qr3 := q3.InvokeToAny(g, l); l := qr3.TakeBaseOut;
      var qr4 := q4.InvokeToAny(g, l); l := qr4.TakeBaseOut;
      var qr5 := q5.InvokeToAny(g, l); l := qr5.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, g, l);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
  end;
  
  QuickConvSyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(QuickConvSyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; c: Context): TRes; override := f(o1, o2, o3, o4, o5);
    
  end;
  QuickConvSyncQueueArray5C<TInp1, TInp2, TInp3, TInp4, TInp5, TRes> = sealed class(QuickConvSyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; c: Context): TRes; override := f(o1, o2, o3, o4, o5, c);
    
  end;
  
  QuickConvAsyncQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc> = abstract class(QuickConvQueueArray5Base<TInp1, TInp2, TInp3, TInp4, TInp5, TRes, TFunc>)
  where TFunc: Delegate;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, QueueRes<TInp3>, QueueRes<TInp4>, QueueRes<TInp5>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      g.ParallelInvoke(l, 5, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
        qr3 := invoker.InvokeBranch(q3.InvokeToAny);
        qr4 := invoker.InvokeBranch(q4.InvokeToAny);
        qr5 := invoker.InvokeBranch(q5.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g), qr3.AttachInvokeActions(g), qr4.AttachInvokeActions(g), qr5.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, g, new CLTaskLocalData(res_ev));
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
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
  BackgroundConvQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc> = abstract class(BackgroundConvertQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>, TRes>)
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
      self.q3.InitBeforeInvoke(g, prev_hubs);
      self.q4.InitBeforeInvoke(g, prev_hubs);
      self.q5.InitBeforeInvoke(g, prev_hubs);
      self.q6.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; qr6: QueueRes<TInp6>; l: CLTaskLocalData): QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res := ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect);
        Result := new QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>(l, res);
      end else
      begin
        Result := new QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>(l);
        Result.AddResSetter(c->ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect));
      end;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
      self.q3.InitBeforeInvoke(g, prev_hubs);
      self.q4.InitBeforeInvoke(g, prev_hubs);
      self.q5.InitBeforeInvoke(g, prev_hubs);
      self.q6.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; c: Context): TRes; abstract;
    
    protected function CombineQRsNil(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; qr6: QueueRes<TInp6>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;
    begin
      Result := new QueueResNil(l);
      if l.ShouldInstaCallAction then
      begin
        if not g.curr_err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
      end else
      begin
        var err_handler := g.curr_err_handler;
        Result.AddAction(c->
        if not err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
    end;
    
    protected function CombineQRsRes<TF,TR>(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; qr6: QueueRes<TInp6>; g: CLTaskGlobalData; l: CLTaskLocalData): TR; where TF: IQueueResFactory<TRes,TR>, constructor; where TR: QueueRes<TRes>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res: TRes;        if not g.curr_err_handler.HadError(true) then
        try
          res := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
        Result := TF.Create.MakeConst(l, res);
      end else
      begin
        Result := TF.Create.MakeDelayed(l);
        var err_handler := g.curr_err_handler;
        Result.AddResSetter(c->
        if not err_handler.HadError(true) then
        try
          Result := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>; override;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      var qr3 := q3.InvokeToAny(g, l); l := qr3.TakeBaseOut;
      var qr4 := q4.InvokeToAny(g, l); l := qr4.TakeBaseOut;
      var qr5 := q5.InvokeToAny(g, l); l := qr5.TakeBaseOut;
      var qr6 := q6.InvokeToAny(g, l); l := qr6.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, l);
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6>>; override;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      var qr6: QueueRes<TInp6>;
      g.ParallelInvoke(l, 6, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
        qr3 := invoker.InvokeBranch(q3.InvokeToAny);
        qr4 := invoker.InvokeBranch(q4.InvokeToAny);
        qr5 := invoker.InvokeBranch(q5.InvokeToAny);
        qr6 := invoker.InvokeBranch(q6.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g), qr3.AttachInvokeActions(g), qr4.AttachInvokeActions(g), qr5.AttachInvokeActions(g), qr6.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, new CLTaskLocalData(res_ev));
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
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, QueueRes<TInp3>, QueueRes<TInp4>, QueueRes<TInp5>, QueueRes<TInp6>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      var qr3 := q3.InvokeToAny(g, l); l := qr3.TakeBaseOut;
      var qr4 := q4.InvokeToAny(g, l); l := qr4.TakeBaseOut;
      var qr5 := q5.InvokeToAny(g, l); l := qr5.TakeBaseOut;
      var qr6 := q6.InvokeToAny(g, l); l := qr6.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, g, l);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
  end;
  
  QuickConvSyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(QuickConvSyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6);
    
  end;
  QuickConvSyncQueueArray6C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes> = sealed class(QuickConvSyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6, c);
    
  end;
  
  QuickConvAsyncQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc> = abstract class(QuickConvQueueArray6Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes, TFunc>)
  where TFunc: Delegate;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, QueueRes<TInp3>, QueueRes<TInp4>, QueueRes<TInp5>, QueueRes<TInp6>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      var qr6: QueueRes<TInp6>;
      g.ParallelInvoke(l, 6, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
        qr3 := invoker.InvokeBranch(q3.InvokeToAny);
        qr4 := invoker.InvokeBranch(q4.InvokeToAny);
        qr5 := invoker.InvokeBranch(q5.InvokeToAny);
        qr6 := invoker.InvokeBranch(q6.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g), qr3.AttachInvokeActions(g), qr4.AttachInvokeActions(g), qr5.AttachInvokeActions(g), qr6.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, g, new CLTaskLocalData(res_ev));
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
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
  BackgroundConvQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc> = abstract class(BackgroundConvertQueue<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>, TRes>)
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
      self.q3.InitBeforeInvoke(g, prev_hubs);
      self.q4.InitBeforeInvoke(g, prev_hubs);
      self.q5.InitBeforeInvoke(g, prev_hubs);
      self.q6.InitBeforeInvoke(g, prev_hubs);
      self.q7.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function CombineQRs(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; qr6: QueueRes<TInp6>; qr7: QueueRes<TInp7>; l: CLTaskLocalData): QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res := ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect, qr7.GetResDirect);
        Result := new QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>(l, res);
      end else
      begin
        Result := new QueueResVal<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>(l);
        Result.AddResSetter(c->ValueTuple.Create(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect, qr7.GetResDirect));
      end;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      self.q1.InitBeforeInvoke(g, prev_hubs);
      self.q2.InitBeforeInvoke(g, prev_hubs);
      self.q3.InitBeforeInvoke(g, prev_hubs);
      self.q4.InitBeforeInvoke(g, prev_hubs);
      self.q5.InitBeforeInvoke(g, prev_hubs);
      self.q6.InitBeforeInvoke(g, prev_hubs);
      self.q7.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; o7: TInp7; c: Context): TRes; abstract;
    
    protected function CombineQRsNil(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; qr6: QueueRes<TInp6>; qr7: QueueRes<TInp7>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;
    begin
      Result := new QueueResNil(l);
      if l.ShouldInstaCallAction then
      begin
        if not g.curr_err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect, qr7.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
      end else
      begin
        var err_handler := g.curr_err_handler;
        Result.AddAction(c->
        if not err_handler.HadError(true) then
        try
          ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect, qr7.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
    end;
    
    protected function CombineQRsRes<TF,TR>(qr1: QueueRes<TInp1>; qr2: QueueRes<TInp2>; qr3: QueueRes<TInp3>; qr4: QueueRes<TInp4>; qr5: QueueRes<TInp5>; qr6: QueueRes<TInp6>; qr7: QueueRes<TInp7>; g: CLTaskGlobalData; l: CLTaskLocalData): TR; where TF: IQueueResFactory<TRes,TR>, constructor; where TR: QueueRes<TRes>;
    begin
      if l.ShouldInstaCallAction then
      begin
        var res: TRes;        if not g.curr_err_handler.HadError(true) then
        try
          res := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect, qr7.GetResDirect, g.c);
        except
          on e: Exception do g.curr_err_handler.AddErr(e)
        end;
        Result := TF.Create.MakeConst(l, res);
      end else
      begin
        Result := TF.Create.MakeDelayed(l);
        var err_handler := g.curr_err_handler;
        Result.AddResSetter(c->
        if not err_handler.HadError(true) then
        try
          Result := ExecFunc(qr1.GetResDirect, qr2.GetResDirect, qr3.GetResDirect, qr4.GetResDirect, qr5.GetResDirect, qr6.GetResDirect, qr7.GetResDirect, c);
        except
          on e: Exception do err_handler.AddErr(e)
        end);
      end;
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>; override;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      var qr3 := q3.InvokeToAny(g, l); l := qr3.TakeBaseOut;
      var qr4 := q4.InvokeToAny(g, l); l := qr4.TakeBaseOut;
      var qr5 := q5.InvokeToAny(g, l); l := qr5.TakeBaseOut;
      var qr6 := q6.InvokeToAny(g, l); l := qr6.TakeBaseOut;
      var qr7 := q7.InvokeToAny(g, l); l := qr7.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, qr7, l);
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
    
    protected function InvokeSubQs(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<ValueTuple<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7>>; override;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      var qr6: QueueRes<TInp6>;
      var qr7: QueueRes<TInp7>;
      g.ParallelInvoke(l, 7, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
        qr3 := invoker.InvokeBranch(q3.InvokeToAny);
        qr4 := invoker.InvokeBranch(q4.InvokeToAny);
        qr5 := invoker.InvokeBranch(q5.InvokeToAny);
        qr6 := invoker.InvokeBranch(q6.InvokeToAny);
        qr7 := invoker.InvokeBranch(q7.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g), qr3.AttachInvokeActions(g), qr4.AttachInvokeActions(g), qr5.AttachInvokeActions(g), qr6.AttachInvokeActions(g), qr7.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, qr7, new CLTaskLocalData(res_ev));
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
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, QueueRes<TInp3>, QueueRes<TInp4>, QueueRes<TInp5>, QueueRes<TInp6>, QueueRes<TInp7>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1 := q1.InvokeToAny(g, l); l := qr1.TakeBaseOut;
      var qr2 := q2.InvokeToAny(g, l); l := qr2.TakeBaseOut;
      var qr3 := q3.InvokeToAny(g, l); l := qr3.TakeBaseOut;
      var qr4 := q4.InvokeToAny(g, l); l := qr4.TakeBaseOut;
      var qr5 := q5.InvokeToAny(g, l); l := qr5.TakeBaseOut;
      var qr6 := q6.InvokeToAny(g, l); l := qr6.TakeBaseOut;
      var qr7 := q7.InvokeToAny(g, l); l := qr7.TakeBaseOut;
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, qr7, g, l);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
  end;
  
  QuickConvSyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(QuickConvSyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; o7: TInp7; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6, o7);
    
  end;
  QuickConvSyncQueueArray7C<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes> = sealed class(QuickConvSyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, (TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context)->TRes>)
    
    protected function ExecFunc(o1: TInp1; o2: TInp2; o3: TInp3; o4: TInp4; o5: TInp5; o6: TInp6; o7: TInp7; c: Context): TRes; override := f(o1, o2, o3, o4, o5, o6, o7, c);
    
  end;
  
  QuickConvAsyncQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc> = abstract class(QuickConvQueueArray7Base<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes, TFunc>)
  where TFunc: Delegate;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TF,TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory_sample: TF; CombineQRs: Func<QueueRes<TInp1>, QueueRes<TInp2>, QueueRes<TInp3>, QueueRes<TInp4>, QueueRes<TInp5>, QueueRes<TInp6>, QueueRes<TInp7>, CLTaskGlobalData, CLTaskLocalData, TR>): TR; where TF: IQueueResBaseFactory<TR>, constructor; where TR: IQueueRes;
    begin
      var qr1: QueueRes<TInp1>;
      var qr2: QueueRes<TInp2>;
      var qr3: QueueRes<TInp3>;
      var qr4: QueueRes<TInp4>;
      var qr5: QueueRes<TInp5>;
      var qr6: QueueRes<TInp6>;
      var qr7: QueueRes<TInp7>;
      g.ParallelInvoke(l, 7, invoker->
      begin
        qr1 := invoker.InvokeBranch(q1.InvokeToAny);
        qr2 := invoker.InvokeBranch(q2.InvokeToAny);
        qr3 := invoker.InvokeBranch(q3.InvokeToAny);
        qr4 := invoker.InvokeBranch(q4.InvokeToAny);
        qr5 := invoker.InvokeBranch(q5.InvokeToAny);
        qr6 := invoker.InvokeBranch(q6.InvokeToAny);
        qr7 := invoker.InvokeBranch(q7.InvokeToAny);
      end);
      var res_ev := EventList.Combine(|qr1.AttachInvokeActions(g), qr2.AttachInvokeActions(g), qr3.AttachInvokeActions(g), qr4.AttachInvokeActions(g), qr5.AttachInvokeActions(g), qr6.AttachInvokeActions(g), qr7.AttachInvokeActions(g)|);
      Result := CombineQRs(qr1, qr2, qr3, qr4, qr5, qr6, qr7, g, new CLTaskLocalData(res_ev));
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;       override := Invoke(g, l, qr_nil_factory, CombineQRsNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke&<QueueResValFactory<TRes>,QueueResVal<TRes>>(g, l, qr_val_factory, CombineQRsRes&<QueueResValFactory<TRes>,QueueResVal<TRes>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>(g, l, qr_ptr_factory, CombineQRsRes&<QueueResPtrFactory<TRes>,QueueResPtr<TRes>>);
    
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
        if cq is ParameterQueue<T> then exit;
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
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>) :=
    if inited_hubs.Add(self) then q.InitBeforeInvoke(g, inited_hubs);
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; invoke_q: CommandQueueInvoker<TR>): ValueTuple<TR, EventList>; where TR: IQueueRes;
    begin
      var res_data: MultiuseableResultData;
      var qr: TR;
      
      // Потоко-безопасно, потому что все .Invoke выполняются синхронно
      //TODO А что будет когда .ThenIf и т.п.?
      if g.mu_res.TryGetValue(self, res_data) then
        qr := TR(res_data.qres) else
      begin
        var prev_err_handler := g.curr_err_handler;
        g.curr_err_handler := new CLTaskErrHandlerEmpty;
        
        qr := invoke_q(g, new CLTaskLocalData);
        var ev := qr.AttachInvokeActions(g);
        
        res_data := new MultiuseableResultData(qr, ev, g.curr_err_handler);
        g.mu_res[self] := res_data;
        
        g.curr_err_handler := prev_err_handler;
      end;
      g.curr_err_handler := new CLTaskErrHandlerThiefRepeater(g.curr_err_handler, res_data.err_handler);
      
      res_data.ev.Retain({$ifdef EventDebug}$'for all mu branches'{$endif});
      Result := ValueTuple.Create( qr, res_data.ev + QueueResNil.Create(l).AttachInvokeActions(g) );
    end;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    procedure ToString(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>);
    begin
      sb += ' => ';
      if q.ToStringHeader(sb, index) then
        delayed.Add(q);
      sb += #10;
    end;
    
  end;
  
  MultiusableCommandQueueHubNil = sealed partial class(MultiusableCommandQueueHubCommon< CommandQueueNil >) end;
  MultiusableCommandQueueNodeNil = sealed class(CommandQueueNil)
    public hub: MultiusableCommandQueueHubNil;
    
    public constructor(hub: MultiusableCommandQueueHubNil) := self.hub := hub;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := hub.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var (qr, ev) := hub.Invoke(g, l, hub.q.InvokeToNil);
      Result := new QueueResNil(new CLTaskLocalData(ev));
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    hub.ToString(sb, tabs, index, delayed);
    
  end;
  MultiusableCommandQueueHubNil = sealed partial class
    
    public function MakeNode: CommandQueueNil := new MultiusableCommandQueueNodeNil(self);
    
  end;
  
  MultiusableCommandQueueHub<T> = sealed partial class(MultiusableCommandQueueHubCommon< CommandQueue<T> >) end;
  MultiusableCommandQueueNode<T> = sealed class(CommandQueue<T>)
    public hub: MultiusableCommandQueueHub<T>;
    
    public constructor(hub: MultiusableCommandQueueHub<T>) := self.hub := hub;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := hub.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var (qr, ev) := hub.Invoke(g, l, hub.q.InvokeToAny);
      Result := new QueueResNil(new CLTaskLocalData(ev));
    end;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResFactory<T, TR>): TR; where TR: QueueRes<T>;
    begin
      var (qr, ev) := hub.Invoke(g, l, hub.q.InvokeToAny);
      Result := qr.WrapResult(qr_factory, ev);
    end;
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := Invoke(g, l, qr_val_factory);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := Invoke(g, l, qr_ptr_factory);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    hub.ToString(sb, tabs, index, delayed);
    
  end;
  MultiusableCommandQueueHub<T> = sealed partial class
    
    public function MakeNode: CommandQueue<T> := new MultiusableCommandQueueNode<T>(self);
    
  end;
  
function CommandQueueNil.Multiusable: ()->CommandQueueNil := (if self is MultiusableCommandQueueNodeNil(var mucqn) then mucqn.hub else new MultiusableCommandQueueHubNil(self)).MakeNode;
function CommandQueue<T>.Multiusable: ()->CommandQueue<T> := (if self is MultiusableCommandQueueNode<T>(var mucqn) then mucqn.hub else new MultiusableCommandQueueHub<T>(self)).MakeNode;

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
    
    public function MakeWaitEv(g: CLTaskGlobalData; prev_ev: EventList): EventList; abstract;
    public function MakeWaitEv(g: CLTaskGlobalData; l: CLTaskLocalData) := MakeWaitEv(g, QueueResNil.Create(l).AttachInvokeActions(g));
    
  end;
  
{$endregion Base}

{$region Outer}

type
  /// wait_handler, который можно встроить в очередь как есть
  WaitHandlerOuter = abstract class
    public uev: UserEvent;
    private state := 0;
    private gc_hnd: GCHandle;
    
    public constructor(g: CLTaskGlobalData; prev_ev: EventList);
    begin
      
      uev := new UserEvent(g.cl_c{$ifdef EventDebug}, $'Wait result'{$endif});
      {$ifdef WaitDebug}
      WaitDebug.RegisterAction(self, $'Created outer with prev_ev=[ {prev_ev.evs?.JoinToString} ], res_ev={uev}');
      {$endif WaitDebug}
      self.gc_hnd := GCHandle.Alloc(self);
      
      // Code of .ThenFinallyWaitFor expects
      // g.curr_err_handler to not change
      // and no new errors to be added
      var err_handler := g.curr_err_handler;
      prev_ev.MultiAttachCallback(()->
      begin
        if err_handler.HadError(true) then
        begin
          {$ifdef WaitDebug}
          WaitDebug.RegisterAction(self, $'Aborted');
          {$endif WaitDebug}
          uev.SetComplete;
          self.gc_hnd.Free;
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
      if Result then self.gc_hnd.Free;
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
    
    public constructor(g: CLTaskGlobalData; prev_ev: EventList; source: WaitHandlerDirect);
    begin
      inherited Create(g, prev_ev);
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
      WaitDebug.RegisterAction(self, $'Tried reserving {1} in source[{source.GetHashCode}]: {Result}');
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
    
    public function MakeWaitEv(g: CLTaskGlobalData; prev_ev: EventList): EventList; override :=
    WaitHandlerDirectWrap.Create(g, prev_ev, handlers[g]).uev;
    
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
  WaitHandlerAllInner<TSub> = sealed class(IWaitHandlerSub)
  where TSub: IWaitHandlerSub;
    private sources: array of WaitHandlerDirect;
    private ref_counts: array of integer;
    private done_c := 0;
    
    private sub: TSub;
    private sub_data: integer;
    
    public constructor(sources: array of WaitHandlerDirect; ref_counts: array of integer; sub: TSub; sub_data: integer);
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
    
    public constructor(g: CLTaskGlobalData; prev_ev: EventList; sources: array of WaitHandlerDirect; ref_counts: array of integer);
    begin
      inherited Create(g, prev_ev);
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
    
    public function MakeWaitEv(g: CLTaskGlobalData; prev_ev: EventList): EventList; override :=
    WaitHandlerAllOuter.Create(g, prev_ev, children.ConvertAll(m->m.handlers[g]), ref_counts).uev;
    
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
    private sources: array of WaitHandlerAllInner<WaitHandlerAnyOuter>;
    
    private done_c := 0;
    
    public constructor(g: CLTaskGlobalData; prev_ev: EventList; markers: array of WaitMarkerAll);
    begin
      inherited Create(g, prev_ev);
      self.sources := new WaitHandlerAllInner<WaitHandlerAnyOuter>[markers.Length];
      for var i := 0 to markers.Length-1 do
        self.sources[i] := new WaitHandlerAllInner<WaitHandlerAnyOuter>(markers[i].children.ConvertAll(m->m.handlers[g]), markers[i].ref_counts, self, i);
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
    
    public function MakeWaitEv(g: CLTaskGlobalData; prev_ev: EventList): EventList; override :=
    WaitHandlerAnyOuter.Create(g, prev_ev, children).uev;
    
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      Result := new QueueResNil(l);
      
      if Result.ShouldInstaCallAction then
      begin
        if not g.curr_err_handler.HadError(true) then
          m.SendSignal;
      end else
      begin
        var err_handler := g.curr_err_handler;
        Result.AddAction(c->if not err_handler.HadError(true) then m.SendSignal);
      end;
      
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
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>) := q.InitBeforeInvoke(g, inited_hubs);
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(prev_qr: TR; err_handler: CLTaskErrHandler): TR; where TR: IQueueRes;
    begin
      if prev_qr.ShouldInstaCallAction then
      begin
        if signal_in_finally or not err_handler.HadError(true) then
          wrap.SendSignal;
      end else
      if signal_in_finally then
        prev_qr.AddAction(c->wrap.SendSignal()) else
        prev_qr.AddAction(c->if not err_handler.HadError(true) then wrap.SendSignal);
      Result := prev_qr;
    end;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := data.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := data.Invoke(data.q.InvokeToNil(g,l), g.curr_err_handler);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    data.ToString(sb, tabs, index, delayed);
    
  end;
  
  DetachedMarkerSignalWrapper<T> = sealed class(DetachedMarkerSignalWrapCommon<CommandQueue<T>>)
    
  end;
  DetachedMarkerSignal<T> = sealed partial class(CommandQueue<T>)
    data: DetachedMarkerSignalCommon<CommandQueue<T>>;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := data.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := data.Invoke(data.q.InvokeToNil(g,l), g.curr_err_handler);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := data.Invoke(data.q.InvokeToVal(g,l), g.curr_err_handler);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := data.Invoke(data.q.InvokeToPtr(g,l), g.curr_err_handler);
    
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    marker.InitInnerHandles(g);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override :=
    new QueueResNil(new CLTaskLocalData( marker.MakeWaitEv(g,l) ));
    
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      q.InitBeforeInvoke(g, inited_hubs);
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
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var res_ev := marker.MakeWaitEv(g, q.InvokeToNil(g, l).base );
      Result := new QueueResNil(new CLTaskLocalData(res_ev));
    end;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(prev_qr: QueueRes<T>; g: CLTaskGlobalData; qr_factory: IQueueResFactory<T,TR>): TR; where TR: QueueRes<T>;
    begin
      var res_ev := marker.MakeWaitEv(g, prev_qr.AttachInvokeActions(g));
      Result := prev_qr.WrapResult(qr_factory, res_ev);
    end;
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := Invoke(q.InvokeToAny(g,l), g, qr_val_factory);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := Invoke(q.InvokeToAny(g,l), g, qr_ptr_factory);
    
  end;
  CommandQueueThenFinallyWaitFor<T> = sealed class(CommandQueueThenBaseWaitFor<T>)
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; invoke_q: CommandQueueInvoker<TR>): ValueTuple<TR, EventList>; where TR: IQueueRes;
    begin
      
      var pre_q_err_handler := g.curr_err_handler;
      var prev_qr := invoke_q(g, l);
      var post_q_err_handler := g.curr_err_handler;
      
      g.curr_err_handler := pre_q_err_handler;
      var res_ev := marker.MakeWaitEv(g, prev_qr.AttachInvokeActions(g));
      {$ifdef DEBUG}
      if g.curr_err_handler <> pre_q_err_handler then
        raise new OpenCLABCInternalException($'MakeWaitEv should not change g.curr_err_handler');
      // Otherwise, CLTaskErrHandlerBranchBase (like in >=) would be needed
      {$endif DEBUG}
      g.curr_err_handler := post_q_err_handler;
      
      Result := ValueTuple.Create(prev_qr, res_ev);
    end;
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResFactory<T,TR>): TR; where TR: QueueRes<T>;
    begin
      var (prev_qr, ev) := Invoke(g, l, q.InvokeToAny);
      Result := prev_qr.WrapResult(qr_factory, ev);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var (prev_qr, ev) := Invoke(g, l, q.InvokeToNil);
      Result := new QueueResNil(new CLTaskLocalData(ev));
    end;
    
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := Invoke(g, l, qr_val_factory);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := Invoke(g, l, qr_ptr_factory);
    
  end;
  
function CommandQueue<T>.ThenWaitFor(marker: WaitMarker) := new CommandQueueThenWaitFor<T>(self, marker);
function CommandQueue<T>.ThenFinallyWaitFor(marker: WaitMarker) := new CommandQueueThenFinallyWaitFor<T>(self, marker);

{$endregion ThenWaitFor}

{$endregion Wait}

{$region Finally}

type
  CommandQueueTryFinallyCommon<TQ> = record
  where TQ: CommandQueueBase;
    public try_do: CommandQueueBase;
    public do_finally: TQ;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>);
    begin
      try_do.InitBeforeInvoke(g, inited_hubs);
      do_finally.InitBeforeInvoke(g, inited_hubs);
    end;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; invoke_finally: CommandQueueInvoker<TR>): TR; where TR: IQueueRes;
    begin
      var origin_err_handler := g.curr_err_handler;
      
      {$region try_do}
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      l := try_do.InvokeToNil(g, l).base;
      var try_handler := g.curr_err_handler;
      
      {$endregion try_do}
      
      {$region do_finally}
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(origin_err_handler);
      Result := invoke_finally(g, l);
      var fin_handler := g.curr_err_handler;
      
      {$endregion do_finally}
      
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(origin_err_handler, |try_handler, fin_handler|);
    end;
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    data.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := data.Invoke(g, l, data.do_finally.InvokeToNil);
    
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    data.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;    override := data.Invoke(g, l, data.do_finally.InvokeToNil);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := data.Invoke(g, l, data.do_finally.InvokeToVal);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := data.Invoke(g, l, data.do_finally.InvokeToPtr);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override :=
    data.ToString(sb, tabs, index, delayed);
    
  end;
  
static function CommandQueueNil.operator>=(try_do: CommandQueueBase; do_finally: CommandQueueNil) :=
new CommandQueueTryFinallyNil(try_do, do_finally);
static function CommandQueue<T>.operator>=(try_do: CommandQueueBase; do_finally: CommandQueue<T>) :=
new CommandQueueTryFinally<T>(try_do, do_finally);

{$endregion Finally}

{$region Handle}

type
  
  CommandQueueHandleWithoutRes = sealed class(CommandQueueNil)
    private try_do: CommandQueueBase;
    private handler: Exception->boolean;
    
    public constructor(try_do: CommandQueueBase; handler: Exception->boolean);
    begin
      self.try_do := try_do;
      self.handler := handler;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    try_do.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var pre_inv_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(pre_inv_handler);
      Result := try_do.InvokeToNil(g, l);
      var post_inv_handler := g.curr_err_handler;
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(pre_inv_handler, |post_inv_handler|);
      
      Result.AddAction(c->
      try
        post_inv_handler.TryRemoveErrors(self.handler);
      except
        on e: Exception do post_inv_handler.AddErr(e);
      end);
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      try_do.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, handler);
      sb += #10;
      
    end;
    
  end;
  
  CommandQueueHandleDefaultRes<T> = sealed class(CommandQueue<T>)
    private try_do: CommandQueue<T>;
    private handler: Exception->boolean;
    private def: T;
    
    public constructor(try_do: CommandQueue<T>; handler: Exception->boolean; def: T);
    begin
      self.try_do := try_do;
      self.handler := handler;
      self.def := def;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    try_do.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var pre_inv_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(pre_inv_handler);
      Result := try_do.InvokeToNil(g, l);
      var post_inv_handler := g.curr_err_handler;
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(pre_inv_handler, |post_inv_handler|);
      
      Result.AddAction(c->
      try
        post_inv_handler.TryRemoveErrors(self.handler);
      except
        on e: Exception do post_inv_handler.AddErr(e);
      end);
      
    end;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResFactory<T,TR>): TR; where TR: QueueRes<T>;
    begin
      var pre_inv_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(pre_inv_handler);
      var prev_qr := try_do.InvokeToAny(g, l);
      var post_inv_handler := g.curr_err_handler;
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(pre_inv_handler, |post_inv_handler|);
      
      Result := prev_qr.TransformResult(qr_factory, g.c, true, (prev_res,c)->
      if not post_inv_handler.HadError(true) then
        Result := prev_res else
      try
        post_inv_handler.TryRemoveErrors(self.handler);
        Result := self.def;
      except
        on e: Exception do post_inv_handler.AddErr(e);
      end);
      
    end;
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := Invoke(g, l, qr_val_factory);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := Invoke(g, l, qr_ptr_factory);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ': ';
      ToStringRuntimeValue(sb, self.def);
      sb += #10;
      
      try_do.ToString(sb, tabs, index, delayed);
      
      sb.Append(#9, tabs);
      ToStringWriteDelegate(sb, handler);
      sb += #10;
      
    end;
    
  end;
  
  CommandQueueHandleReplaceRes<T> = sealed class(CommandQueue<T>)
    private try_do: CommandQueue<T>;
    private handler: List<Exception> -> T;
    
    public constructor(try_do: CommandQueue<T>; handler: List<Exception> -> T);
    begin
      self.try_do := try_do;
      self.handler := handler;
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    try_do.InitBeforeInvoke(g, inited_hubs);
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var pre_inv_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(pre_inv_handler);
      Result := try_do.InvokeToNil(g, l);
      var post_inv_handler := new CLTaskErrHandlerThief(g.curr_err_handler);
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(pre_inv_handler, new CLTaskErrHandler[](post_inv_handler));
      
      Result.AddAction(c->
      if post_inv_handler.HadError(true) then
      begin
        post_inv_handler.StealPrevErrors;
        try
          self.handler(post_inv_handler.get_local_err_lst);
        except
          on e: Exception do post_inv_handler.AddErr(e);
        end;
      end);
      
    end;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResFactory<T,TR>): TR; where TR: QueueRes<T>;
    begin
      var pre_inv_handler := g.curr_err_handler;
      
      g.curr_err_handler := new CLTaskErrHandlerBranchBase(pre_inv_handler);
      var prev_qr := try_do.InvokeToAny(g, l);
      var post_inv_handler := new CLTaskErrHandlerThief(g.curr_err_handler);
      g.curr_err_handler := new CLTaskErrHandlerBranchCombinator(pre_inv_handler, new CLTaskErrHandler[](post_inv_handler));
      
      Result := prev_qr.TransformResult(qr_factory, g.c, true, (prev_res,c)->
      if not post_inv_handler.HadError(true) then
        Result := prev_res else
      begin
        post_inv_handler.StealPrevErrors;
        try
          Result := self.handler(post_inv_handler.get_local_err_lst);
        except
          on e: Exception do post_inv_handler.AddErr(e);
        end;
      end);
      
    end;
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := Invoke(g, l, qr_val_factory);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := Invoke(g, l, qr_ptr_factory);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      try_do.ToString(sb, tabs, index, delayed);
      
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

{$endregion Handle}

{$endregion Queue converter's}

{$region KernelArg}

{$region Base}

type
  ISetableKernelArg = interface
    procedure SetArg(k: cl_kernel; ind: UInt32);
  end;
  KernelArg = abstract partial class
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<ISetableKernelArg>; abstract;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); abstract;
    
  end;
  
{$endregion Base}

{$region Const}

{$region Base}

type
  ConstKernelArg = abstract class(KernelArg, ISetableKernelArg)
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<ISetableKernelArg>; override :=
    new QueueResVal<ISetableKernelArg>(new CLTaskLocalData, self);
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
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
      sb += ': ';
      ToStringRuntimeValue(sb, self.a);
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
      sb += ': ';
      ToStringRuntimeValue(sb, self.mem);
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
      sb += ': ';
      ToStringRuntimeValue(sb, self.val^);
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
    private sz: UIntPtr;
    
    static constructor :=
    BlittableHelper.RaiseIfBad(typeof(TRecord), 'передавать в качестве параметров kernel''а');
    
    public constructor(a: array of TRecord; ind: integer);
    begin
      self.hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
      self.offset := Marshal.SizeOf&<TRecord> * ind;
      self.sz := new UIntPtr(Marshal.SizeOf&<TRecord> * (a.Length-ind));
    end;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure Finalize; override :=
    if hnd.IsAllocated then hnd.Free;
    
    public procedure SetArg(k: cl_kernel; ind: UInt32); override :=
    OpenCLABCInternalException.RaiseIfError( cl.SetKernelArg(k, ind, sz, (hnd.AddrOfPinnedObject+offset).ToPointer) ); 
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ': ';
      ToStringRuntimeValue(sb, hnd.Target as array of TRecord);
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
  InvokeableKernelArg = abstract class(KernelArg)
    protected static kqr_factory := new QueueResValFactory<ISetableKernelArg>;
  end;
  
{$endregion Base}

{$region CLArray}

type
  KernelArgCLArrayCQ<T> = sealed class(InvokeableKernelArg)
  where T: record;
    public q: CommandQueue<CLArray<T>>;
    public constructor(q: CommandQueue<CLArray<T>>) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<ISetableKernelArg>; override :=
    q.InvokeToAny(g, l).TransformResult(kqr_factory, g.c, true, (a,c)->new KernelArgCLArray<T>(a) as ISetableKernelArg);
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.InitBeforeInvoke(g, inited_hubs);
    
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<ISetableKernelArg>; override :=
    q.InvokeToAny(g, l).TransformResult(kqr_factory, g.c, true, (mem,c)->new KernelArgMemorySegment(mem) as ISetableKernelArg);
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.InitBeforeInvoke(g, inited_hubs);
    
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<ISetableKernelArg>; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      var  sz_qr: QueueRes<UIntPtr>;
      // as_new=false, because KernelArg should be invoked as new (independant of prev errors and events)
      g.ParallelInvoke(l, 2, invoker->
      begin
        ptr_qr := invoker.InvokeBranch(ptr_q.InvokeToAny);
         sz_qr := invoker.InvokeBranch( sz_q.InvokeToAny);
      end);
      var res_ev := ptr_qr.AttachInvokeActions(g) + sz_qr.AttachInvokeActions(g);
      var res_l := new CLTaskLocalData(res_ev);
      if ptr_qr.IsConst and sz_qr.IsConst then
        Result := new QueueResVal<ISetableKernelArg>(res_l, new KernelArgData(ptr_qr.GetResDirect, sz_qr.GetResDirect)) else
      begin
        Result := new QueueResVal<ISetableKernelArg>(res_l);
        Result.AddResSetter(c->new KernelArgData(ptr_qr.GetResDirect, sz_qr.GetResDirect));
      end;
    end;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr_q.InitBeforeInvoke(g, inited_hubs);
       sz_q.InitBeforeInvoke(g, inited_hubs);
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
    public qr: QueueResPtr<TRecord>;
    
    public constructor(qr: QueueResPtr<TRecord>) := self.qr := qr;
    private constructor := raise new OpenCLABCInternalException;
    
    public procedure SetArg(k: cl_kernel; ind: UInt32); override :=
    OpenCLABCInternalException.RaiseIfError( cl.SetKernelArg(k, ind, new UIntPtr(Marshal.SizeOf&<TRecord>), qr.res) );
    
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<ISetableKernelArg>; override;
    begin
      var prev_qr := q.InvokeToPtr(g, l);
      Result := new QueueResVal<ISetableKernelArg>(
        prev_qr.TakeBaseOut,
        new KernelArgPtrQr<TRecord>(prev_qr)
      );
    end;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.InitBeforeInvoke(g, inited_hubs);
    
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<ISetableKernelArg>; override :=
    q.InvokeToAny(g, l).TransformResult(kqr_factory, g.c, true, (nv,c)->new KernelArgNativeValue<TRecord>(nv) as ISetableKernelArg);
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.InitBeforeInvoke(g, inited_hubs);
    
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
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<ISetableKernelArg>; override;
    begin
      var   a_qr: QueueRes<array of TRecord>;
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(l, 2, invoker->
      begin
          a_qr := invoker.InvokeBranch(  a_q.InvokeToAny);
        ind_qr := invoker.InvokeBranch(ind_q.InvokeToAny);
      end);
      var res_ev := a_qr.AttachInvokeActions(g) + ind_qr.AttachInvokeActions(g);
      var res_l := new CLTaskLocalData(res_ev);
      if a_qr.IsConst and ind_qr.IsConst then
        Result := new QueueResVal<ISetableKernelArg>(res_l, new KernelArgArray<TRecord>(a_qr.GetResDirect, ind_qr.GetResDirect)) else
      begin
        Result := new QueueResVal<ISetableKernelArg>(res_l);
        Result.AddResSetter(c->new KernelArgArray<TRecord>(a_qr.GetResDirect, ind_qr.GetResDirect));
      end;
    end;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
        a_q.InitBeforeInvoke(g, inited_hubs);
      ind_q.InitBeforeInvoke(g, inited_hubs);
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
  GPUCommandObjInvoker<T> = CommandQueueInvoker<QueueResVal<T>>;
  
  GPUCommand<T> = abstract class
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); abstract;
    
    protected function InvokeObj  (o: T;                              g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; abstract;
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<T>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; abstract;
    
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
  QueueCommand<T> = sealed class(BasicGPUCommand<T>)
    public q: CommandQueueBase;
    
    public constructor(q: CommandQueueBase) := self.q := q;
    private constructor := raise new OpenCLABCInternalException;
    
    protected function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData) := q.InvokeToNil(g, l);
    
    protected function InvokeObj  (o: T;                              g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := Invoke(g, l);
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<T>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := Invoke(g, l);
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    q.InitBeforeInvoke(g, inited_hubs);
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  QueueCommandFactory<TObj> = sealed class(ITypedCQConverter<BasicGPUCommand<TObj>>)
    
    public function ConvertNil(cq: CommandQueueNil): BasicGPUCommand<TObj> := new QueueCommand<TObj>(cq);
    public function Convert<T>(cq: CommandQueue<T>): BasicGPUCommand<TObj> :=
    if cq is ConstQueue<T> then nil else
    if cq is ParameterQueue<T> then nil else
    if cq is CastQueueBase<T>(var ccq) then
      ccq.SourceBase.ConvertTyped(self) else
      new QueueCommand<TObj>(cq);
    
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
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
    
    protected function InvokeObj(o: T; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var prev_d := l.prev_delegate;
      var c := g.c;
      var err_handler := g.curr_err_handler;
      new QueueResNil(new CLTaskLocalData(
        UserEvent.StartBackgroundWork(l.prev_ev,
          ()->
          begin
            prev_d.Invoke(c);
            try
              ExecProc(o, c);
            except
              on e: Exception do err_handler.AddErr(e);
            end;
          end,
          g.cl_c{$ifdef EventDebug}, $'const body of {self.GetType}'{$endif}
        )
      ));
    end;
    
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<T>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var o_qr := o_invoke(g, l);
      var c := g.c;
      var err_handler := g.curr_err_handler;
      Result := new QueueResNil(new CLTaskLocalData(
        UserEvent.StartBackgroundWork(o_qr.ResEv,
          ()->
          begin
            var o := o_qr.GetRes(c);
            try
              ExecProc(o, c);
            except
              on e: Exception do err_handler.AddErr(e);
            end;
          end,
          g.cl_c{$ifdef EventDebug}, $'queue body of {self.GetType}'{$endif}
        )
      ));
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
    
    private function Invoke(prev_qr: QueueRes<T>; g: CLTaskGlobalData): QueueResNil;
    begin
      Result := new QueueResNil(prev_qr.TakeBaseOut);
      
      var d := QueueResActionUtils.HandlerWrap(g.curr_err_handler, ExecProc);
      if Result.ShouldInstaCallAction then
        d(prev_qr.GetResDirect, g.c) else
        Result.AddAction(c->d(prev_qr.GetResDirect, c));
      
    end;
    
    protected function InvokeObj  (o: T;                              g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := Invoke(new QueueResVal<T>(l, o), g);
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<T>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := Invoke(o_invoke(g, l), g);
    
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
    
    private function Invoke(g: CLTaskGlobalData; l: CLTaskLocalData) :=
    new QueueResNil(new CLTaskLocalData( marker.MakeWaitEv(g,l) ));
    
    protected function InvokeObj  (o: T;                              g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := Invoke(g, l);
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<T>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := Invoke(g, l);
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); abstract;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData; commands: List<GPUCommand<T>>): QueueResNil; abstract;
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData; commands: List<GPUCommand<T>>): QueueResVal<T>; abstract;
    
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
    // Not nil only when commands are nil
    private commands_in: GPUCommandContainer<T>;
    private old_command_count: integer;
    
    private procedure TakeCommandsBack;
    begin
      if commands_in=nil then exit;
      while commands_in.commands_in<>nil do
        commands_in := commands_in.commands_in;
      self.commands := new List<GPUCommand<T>>(old_command_count);
      for var i := 0 to old_command_count-1 do self.commands += commands_in.commands[i];
      commands_in := nil;
    end;
    
    public function Clone: GPUCommandContainer<T>; abstract;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      core.InitBeforeInvoke(g, inited_hubs);
      TakeCommandsBack;
      foreach var comm in self.commands do comm.InitBeforeInvoke(g, inited_hubs);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override    := core.InvokeToNil(g, l, self.commands);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := core.InvokeToVal(g, l, self.commands);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override;
    begin
      Result := nil;
      raise new OpenCLABCInternalException($'Err:Invoke:InvalidToPtr');
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      core.ToString(sb, tabs, index, delayed);
      TakeCommandsBack;
      foreach var comm in commands do
        comm.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
function AddCommand<TContainer, T>(cc: TContainer; comm: GPUCommand<T>): TContainer; where TContainer: GPUCommandContainer<T>;
begin
  cc.TakeCommandsBack;
  Result := TContainer(cc.Clone);
  cc.commands_in := Result;
  //TODO #????
  cc.old_command_count := (cc as GPUCommandContainer<T>).commands.Count;
  cc.commands := nil;
  Result.commands += comm;
end;

{$endregion Base}

{$region Core}

type
  CCCObj<T> = sealed class(GPUCommandContainerCore<T>)
    public o: T;
    
    public constructor(o: T) := self.o := o;
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; commands: List<GPUCommand<T>>; make_qr: (CLTaskLocalData, T)->TR): TR;
    begin
      var o := self.o;
      
      foreach var comm in commands do
        l := comm.InvokeObj(o, g, l).base;
      
      Result := make_qr(l, o);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData; commands: List<GPUCommand<T>>): QueueResNil; override    := Invoke(g, l, commands, (l,o)->new QueueResNil(l));
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData; commands: List<GPUCommand<T>>): QueueResVal<T>; override := Invoke(g, l, commands, (l,o)->new QueueResVal<T>(l,o));
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += ': ';
      CommandQueueBase.ToStringRuntimeValue(sb, self.o);
      sb += #10;
    end;
    
  end;
  
  CCCQueue<T> = sealed class(GPUCommandContainerCore<T>)
    public hub: MultiusableCommandQueueHub<T>;
    
    public constructor(q: CommandQueue<T>) := self.hub := new MultiusableCommandQueueHub<T>(q);
    private constructor := raise new OpenCLABCInternalException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override :=
    hub.q.InitBeforeInvoke(g, inited_hubs);
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; commands: List<GPUCommand<T>>; make_qr: (GPUCommandObjInvoker<T>,CLTaskGlobalData,CLTaskLocalData)->TR): TR;
    begin
      var invoke_plug: GPUCommandObjInvoker<T> := hub.MakeNode.InvokeToVal;
      
      foreach var comm in commands do
        l := comm.InvokeQueue(invoke_plug, g, l).base;
      
      Result := make_qr(invoke_plug, g, l);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData; commands: List<GPUCommand<T>>): QueueResNil;    override := Invoke(g, l, commands, (inv,g,l)->new QueueResNil(l));
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData; commands: List<GPUCommand<T>>): QueueResVal<T>; override := Invoke(g, l, commands, (inv,g,l)->inv(g,l));
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      hub.q.ToString(sb, tabs, index, delayed);
    end;
    
  end;
  
  GPUCommandContainer<T> = abstract partial class
    
    protected constructor(o: T) :=
    self.core := new CCCObj<T>(o);
    
    protected constructor(q: CommandQueue<T>) :=
    self.core := new CCCQueue<T>(q);
    
    protected constructor(ccq: GPUCommandContainer<T>);
    begin
      self.core := ccq.core;
      self.commands := ccq.commands;
    end;
    
  end;
  
{$endregion Core}

{$region Kernel}

type
  KernelCCQ = sealed partial class(GPUCommandContainer<Kernel>)
    
    private constructor(ccq: GPUCommandContainer<Kernel>) := inherited;
    public function Clone: GPUCommandContainer<Kernel>; override := new KernelCCQ(self);
    
  end;
  
constructor KernelCCQ.Create(o: Kernel) := inherited;
constructor KernelCCQ.Create(q: CommandQueue<Kernel>) := inherited;
constructor KernelCCQ.Create := inherited;

{$region Special .Add's}

function KernelCCQ.ThenQueue(q: CommandQueueBase): KernelCCQ;
begin
  var comm := BasicGPUCommand&<Kernel>.MakeQueue(q);
  Result := if comm=nil then self else AddCommand(self, comm);
end;

function KernelCCQ.ThenProc(p: Kernel->()) := AddCommand(self, BasicGPUCommand&<Kernel>.MakeBackgroundProc(p));
function KernelCCQ.ThenProc(p: (Kernel, Context)->()) := AddCommand(self, BasicGPUCommand&<Kernel>.MakeBackgroundProc(p));
function KernelCCQ.ThenQuickProc(p: Kernel->()) := AddCommand(self, BasicGPUCommand&<Kernel>.MakeQuickProc(p));
function KernelCCQ.ThenQuickProc(p: (Kernel, Context)->()) := AddCommand(self, BasicGPUCommand&<Kernel>.MakeQuickProc(p));

function KernelCCQ.ThenWait(marker: WaitMarker) := AddCommand(self, BasicGPUCommand&<Kernel>.MakeWait(marker));

{$endregion Special .Add's}

{$endregion Kernel}

{$region MemorySegment}

type
  MemorySegmentCCQ = sealed partial class(GPUCommandContainer<MemorySegment>)
    
    private constructor(ccq: GPUCommandContainer<MemorySegment>) := inherited;
    public function Clone: GPUCommandContainer<MemorySegment>; override := new MemorySegmentCCQ(self);
    
  end;
  
static function KernelArg.operator implicit(mem_q: MemorySegmentCCQ): KernelArg := FromMemorySegmentCQ(mem_q);

constructor MemorySegmentCCQ.Create(o: MemorySegment) := inherited;
constructor MemorySegmentCCQ.Create(q: CommandQueue<MemorySegment>) := inherited;
constructor MemorySegmentCCQ.Create := inherited;

{$region Special .Add's}

function MemorySegmentCCQ.ThenQueue(q: CommandQueueBase): MemorySegmentCCQ;
begin
  var comm := BasicGPUCommand&<MemorySegment>.MakeQueue(q);
  Result := if comm=nil then self else AddCommand(self, comm);
end;

function MemorySegmentCCQ.ThenProc(p: MemorySegment->()) := AddCommand(self, BasicGPUCommand&<MemorySegment>.MakeBackgroundProc(p));
function MemorySegmentCCQ.ThenProc(p: (MemorySegment, Context)->()) := AddCommand(self, BasicGPUCommand&<MemorySegment>.MakeBackgroundProc(p));
function MemorySegmentCCQ.ThenQuickProc(p: MemorySegment->()) := AddCommand(self, BasicGPUCommand&<MemorySegment>.MakeQuickProc(p));
function MemorySegmentCCQ.ThenQuickProc(p: (MemorySegment, Context)->()) := AddCommand(self, BasicGPUCommand&<MemorySegment>.MakeQuickProc(p));

function MemorySegmentCCQ.ThenWait(marker: WaitMarker) := AddCommand(self, BasicGPUCommand&<MemorySegment>.MakeWait(marker));

{$endregion Special .Add's}

{$endregion MemorySegment}

{$region CLArray}

type
  CLArrayCCQ<T> = sealed partial class(GPUCommandContainer<CLArray<T>>)
    
    private constructor(ccq: GPUCommandContainer<CLArray<T>>) := inherited;
    public function Clone: GPUCommandContainer<CLArray<T>>; override := new CLArrayCCQ<T>(self);
    
  end;
  
static function KernelArg.operator implicit<T>(a_q: CLArrayCCQ<T>): KernelArg; where T: record;
//TODO #2550
begin Result := FromCLArrayCQ(a_q as object as GPUCommandContainer<CLArray<T>>); end;

constructor CLArrayCCQ<T>.Create(o: CLArray<T>) := inherited;
constructor CLArrayCCQ<T>.Create(q: CommandQueue<CLArray<T>>) := inherited;
constructor CLArrayCCQ<T>.Create := inherited;

{$region Special .Add's}

function CLArrayCCQ<T>.ThenQueue(q: CommandQueueBase): CLArrayCCQ<T>;
begin
  var comm := BasicGPUCommand&<CLArray<T>>.MakeQueue(q);
  Result := if comm=nil then self else AddCommand(self, comm);
end;

function CLArrayCCQ<T>.ThenProc(p: CLArray<T>->()) := AddCommand(self, BasicGPUCommand&<CLArray<T>>.MakeBackgroundProc(p));
function CLArrayCCQ<T>.ThenProc(p: (CLArray<T>, Context)->()) := AddCommand(self, BasicGPUCommand&<CLArray<T>>.MakeBackgroundProc(p));
function CLArrayCCQ<T>.ThenQuickProc(p: CLArray<T>->()) := AddCommand(self, BasicGPUCommand&<CLArray<T>>.MakeQuickProc(p));
function CLArrayCCQ<T>.ThenQuickProc(p: (CLArray<T>, Context)->()) := AddCommand(self, BasicGPUCommand&<CLArray<T>>.MakeQuickProc(p));

function CLArrayCCQ<T>.ThenWait(marker: WaitMarker) := AddCommand(self, BasicGPUCommand&<CLArray<T>>.MakeWait(marker));

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
    
    private procedure CheckDone;
    begin
      {$ifdef DEBUG}
      if c1+c2+skipped <> evs.Length then raise new OpenCLABCInternalException($'Too much EnqEv capacity: {c1+c2+skipped}/{evs.Length} used');
      {$endif DEBUG}
    end;
    
    public function MakeLists: ValueTuple<EventList, EventList>;
    begin
      CheckDone;
      Result := ValueTuple.Create(
        EventList.Combine(new ArraySegment<EventList>(evs,0,c1)),
        EventList.Combine(new ArraySegment<EventList>(evs,evs.Length-c2,c2))
      );
    end;
    public function CombineAll: EventList;
    begin
      CheckDone;
      Result := EventList.Combine(evs);
    end;
    
  end;
  
  DirectEnqRes = ValueTuple<cl_event, QueueResAction>;
  EnqRes = ValueTuple<EventList, QueueResAction>;
  EnqueueableEnqFunc<TInvData> = function(inv_data: TInvData; c: Context; cq: cl_command_queue; ev_l2: EventList): DirectEnqRes;
  
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
    
    private static function ExecuteEnqFunc<TInvData>(
      inv_data: TInvData; c: Context; cq: cl_command_queue; ev_l2: EventList;
      enq_f: EnqueueableEnqFunc<TInvData>; err_handler: CLTaskErrHandler
      {$ifdef EventDebug}; q: object{$endif}): EnqRes;
    begin
      Result := new EnqRes(ev_l2, nil);
      try
        var (enq_ev, act) := enq_f(inv_data, c, cq, ev_l2);
        {$ifdef EventDebug}
        EventDebug.RegisterEventRetain(enq_ev, $'Enq by {q.GetType}, waiting on [{ev_l2.evs?.JoinToString}]');
        {$endif EventDebug}
        // 1. ev_l2 can be released only after executing dependant command
        // 2. If event in ev_l2 would receive error, enq_ev would not give descriptive error
        Result := new EnqRes(ev_l2+enq_ev, act);
      except
        on e: Exception do err_handler.AddErr(e);
      end;
    end;
    
    public static function Invoke<TEnq, TInvData>(q: TEnq; inv_data: TInvData; g: CLTaskGlobalData; start_ev: EventList; start_ev_in_l1: boolean): EnqRes; where TEnq: IEnqueueable<TInvData>;
    begin
      var enq_evs := new EnqEvLst(q.EnqEvCapacity+1);
      if start_ev_in_l1 then
        enq_evs.AddL1(start_ev) else
        enq_evs.AddL2(start_ev);
      
      var pre_params_handler := g.curr_err_handler;
      var enq_f := q.InvokeParams(g, enq_evs);
      // After InvokeParams, because parameters
      // should not care about prev events and errors
      if pre_params_handler.HadError(true) then
      begin
        Result := new EnqRes(enq_evs.CombineAll, nil);
        exit;
      end;
      
      var (ev_l1, ev_l2) := enq_evs.MakeLists;
      var need_async_inv := ev_l1.count<>0;
      
      var post_params_handler := g.curr_err_handler;
      // When inv is async, post_params_handler
      // could be appened later, until ev_l2 is completed
      if post_params_handler.HadError(not need_async_inv) then
      begin
        Result := new EnqRes(ev_l2, nil);
        exit;
      end;
      
      // When inv is async, cq needs to be secured for thread safety
      // Otherwise, next command can be written before current one
      var cq := g.GetCQ(need_async_inv);
      {$ifdef QueueDebug}
      QueueDebug.Add(cq, q.GetType.ToString);
      {$endif QueueDebug}
      
      if not need_async_inv then
        Result := ExecuteEnqFunc(inv_data, g.c, cq, ev_l2, enq_f, post_params_handler{$ifdef EventDebug}, q{$endif}) else
      begin
        var res_ev := new UserEvent(g.cl_c
          {$ifdef EventDebug}, $'{q.GetType}, temp for nested AttachCallback: [{ev_l1.evs.JoinToString}], then [{ev_l2.evs?.JoinToString}]'{$endif}
        );
        
        ev_l1.MultiAttachCallback(()->
        begin
          var (enq_ev, enq_act) := ExecuteEnqFunc(inv_data, g.c, cq, ev_l2, enq_f, post_params_handler{$ifdef EventDebug}, q{$endif});
          enq_ev.MultiAttachCallback(()->
          begin
            if enq_act<>nil then enq_act(g.c);
            g.ReturnCQ(cq);
            res_ev.SetComplete;
          end{$ifdef EventDebug}, $'propagating Enq ev of {q.GetType} to res_ev: {res_ev.uev}'{$endif});
        end{$ifdef EventDebug}, $'calling async Enq of {q.GetType}'{$endif});
        
        Result := new EnqRes(res_ev, nil);
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
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (T, cl_command_queue, EventList)->DirectEnqRes; abstract;
    public function InvokeParams(g: CLTaskGlobalData; enq_evs: EnqEvLst): EnqueueableEnqFunc<EnqueueableGPUCommandInvData<T>>;
    begin
      var enq_f := InvokeParamsImpl(g, enq_evs);
      Result := (data, c, lcq, ev)->enq_f(data.qr.GetRes(c), lcq, ev);
    end;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke(g: CLTaskGlobalData; prev_qr: QueueRes<T>; start_ev: EventList; start_ev_in_l1: boolean): QueueResNil;
    begin
      var inv_data: EnqueueableGPUCommandInvData<T>;
      inv_data.qr  := prev_qr;
      
      var (enq_ev, enq_act) := EnqueueableCore.Invoke(self, inv_data, g, start_ev, start_ev_in_l1);
      var res := new QueueResNil(new CLTaskLocalData(enq_ev));
      if enq_act<>nil then res.AddAction(enq_act);
      
      Result := res;
    end;
    
    protected function InvokeObj(o: T; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override :=
    Invoke(g, new QueueResVal<T>(new CLTaskLocalData, o), l.prev_ev, false);
    
    protected function InvokeQueue(o_invoke: GPUCommandObjInvoker<T>; g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      var prev_qr := o_invoke(g, l);
      Result := Invoke(g, prev_qr, prev_qr.ResEv, not prev_qr.IsConst);
    end;
    
  end;
  
{$endregion GPUCommand}

{$region GetCommand}

type
  EnqueueableGetCommandInvData<TObj, TRes> = record
    prev_qr: QueueRes<TObj>;
    res_qr: QueueRes<TRes>;
  end;
  EnqueueableGetCommand<TObj, TRes> = abstract class(CommandQueue<TRes>, IEnqueueable<EnqueueableGetCommandInvData<TObj, TRes>>)
    protected prev_commands: GPUCommandContainer<TObj>;
    
    public constructor(prev_commands: GPUCommandContainer<TObj>) :=
    self.prev_commands := prev_commands;
    
    public function EnqEvCapacity: integer; abstract;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (TObj, cl_command_queue, EventList, QueueRes<TRes>)->DirectEnqRes; abstract;
    public function InvokeParams(g: CLTaskGlobalData; enq_evs: EnqEvLst): EnqueueableEnqFunc<EnqueueableGetCommandInvData<TObj, TRes>>;
    begin
      var enq_f := InvokeParamsImpl(g, enq_evs);
      Result := (data, c, lcq, ev)->enq_f(data.prev_qr.GetRes(c), lcq, ev, data.res_qr);
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override := new QueueResNil(l);
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResFactory<TRes,TR>): TR; where TR: QueueRes<TRes>;
    begin
      Result := qr_factory.MakeDelayed(qr->
      begin
        var prev_qr := prev_commands.InvokeToVal(g, l);
        
        var inv_data: EnqueueableGetCommandInvData<TObj, TRes>;
        inv_data.prev_qr  := prev_qr;
        inv_data.res_qr   := qr;
        
        var (enq_ev, enq_act) := EnqueueableCore.Invoke(self, inv_data, g, prev_qr.ResEv, not prev_qr.IsConst);
        Result := new CLTaskLocalData(enq_ev);
        if enq_act<>nil then Result.prev_delegate.AddAction(enq_act);
      end);
    end;
    
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override := Invoke(g, l, qr_val_factory);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := Invoke(g, l, qr_ptr_factory);
    
  end;
  
  EnqueueableGetPtrCommand<TObj, TRes> = abstract class(EnqueueableGetCommand<TObj,TRes>)
    
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<TRes>; override;
    begin
      Result := nil;
      raise new OpenCLABCInternalException($'Err:Invoke:InvalidToVal');
    end;
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<TRes>; override := inherited InvokeToPtr(g, l);
    protected function InvokeToAny(g: CLTaskGlobalData; l: CLTaskLocalData): QueueRes<TRes>; override := InvokeToPtr(g,l);
    
  end;
  
{$endregion GetCommand}

{$region Kernel}

{$region Implicit}

{$region 1#Exec}

function Kernel.Exec1(sz1: CommandQueue<integer>; params args: array of KernelArg): Kernel;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenExec1(sz1, args));
end;

function Kernel.Exec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): Kernel;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenExec2(sz1, sz2, args));
end;

function Kernel.Exec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): Kernel;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenExec3(sz1, sz2, sz3, args));
end;

function Kernel.Exec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): Kernel;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenExec(global_work_offset, global_work_size, local_work_size, args));
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
       sz1.InitBeforeInvoke(g, prev_hubs);
      foreach var temp1 in args do temp1.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (Kernel, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var  sz1_qr: QueueRes<integer>;
      var args_qr: array of QueueResVal<ISetableKernelArg>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
         sz1_qr := invoker.InvokeBranch&<QueueRes<integer>>( sz1.InvokeToAny); if sz1_qr.IsConst then enq_evs.AddL2(sz1_qr.AttachInvokeActions(g)) else enq_evs.AddL1(sz1_qr.AttachInvokeActions(g));
        args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<QueueResVal<ISetableKernelArg>>(temp1.Invoke); if Result.IsConst then enq_evs.AddL2(Result.AttachInvokeActions(g)) else enq_evs.AddL1(Result.AttachInvokeActions(g)); end);
      end);
      
      Result := (o, cq, evs)->
      begin
        var  sz1 :=  sz1_qr.GetResDirect;
        var args := args_qr.ConvertAll(temp1->temp1.GetResDirect);
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          OpenCLABCInternalException.RaiseIfError( cl.ReleaseKernel(ntv) );
          GC.KeepAlive(args);
        end);
      end;
      
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
  
function KernelCCQ.ThenExec1(sz1: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
       sz1.InitBeforeInvoke(g, prev_hubs);
       sz2.InitBeforeInvoke(g, prev_hubs);
      foreach var temp1 in args do temp1.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (Kernel, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var  sz1_qr: QueueRes<integer>;
      var  sz2_qr: QueueRes<integer>;
      var args_qr: array of QueueResVal<ISetableKernelArg>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
         sz1_qr := invoker.InvokeBranch&<QueueRes<integer>>( sz1.InvokeToAny); if sz1_qr.IsConst then enq_evs.AddL2(sz1_qr.AttachInvokeActions(g)) else enq_evs.AddL1(sz1_qr.AttachInvokeActions(g));
         sz2_qr := invoker.InvokeBranch&<QueueRes<integer>>( sz2.InvokeToAny); if sz2_qr.IsConst then enq_evs.AddL2(sz2_qr.AttachInvokeActions(g)) else enq_evs.AddL1(sz2_qr.AttachInvokeActions(g));
        args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<QueueResVal<ISetableKernelArg>>(temp1.Invoke); if Result.IsConst then enq_evs.AddL2(Result.AttachInvokeActions(g)) else enq_evs.AddL1(Result.AttachInvokeActions(g)); end);
      end);
      
      Result := (o, cq, evs)->
      begin
        var  sz1 :=  sz1_qr.GetResDirect;
        var  sz2 :=  sz2_qr.GetResDirect;
        var args := args_qr.ConvertAll(temp1->temp1.GetResDirect);
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          OpenCLABCInternalException.RaiseIfError( cl.ReleaseKernel(ntv) );
          GC.KeepAlive(args);
        end);
      end;
      
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
  
function KernelCCQ.ThenExec2(sz1,sz2: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
       sz1.InitBeforeInvoke(g, prev_hubs);
       sz2.InitBeforeInvoke(g, prev_hubs);
       sz3.InitBeforeInvoke(g, prev_hubs);
      foreach var temp1 in args do temp1.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (Kernel, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var  sz1_qr: QueueRes<integer>;
      var  sz2_qr: QueueRes<integer>;
      var  sz3_qr: QueueRes<integer>;
      var args_qr: array of QueueResVal<ISetableKernelArg>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
         sz1_qr := invoker.InvokeBranch&<QueueRes<integer>>( sz1.InvokeToAny); if sz1_qr.IsConst then enq_evs.AddL2(sz1_qr.AttachInvokeActions(g)) else enq_evs.AddL1(sz1_qr.AttachInvokeActions(g));
         sz2_qr := invoker.InvokeBranch&<QueueRes<integer>>( sz2.InvokeToAny); if sz2_qr.IsConst then enq_evs.AddL2(sz2_qr.AttachInvokeActions(g)) else enq_evs.AddL1(sz2_qr.AttachInvokeActions(g));
         sz3_qr := invoker.InvokeBranch&<QueueRes<integer>>( sz3.InvokeToAny); if sz3_qr.IsConst then enq_evs.AddL2(sz3_qr.AttachInvokeActions(g)) else enq_evs.AddL1(sz3_qr.AttachInvokeActions(g));
        args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<QueueResVal<ISetableKernelArg>>(temp1.Invoke); if Result.IsConst then enq_evs.AddL2(Result.AttachInvokeActions(g)) else enq_evs.AddL1(Result.AttachInvokeActions(g)); end);
      end);
      
      Result := (o, cq, evs)->
      begin
        var  sz1 :=  sz1_qr.GetResDirect;
        var  sz2 :=  sz2_qr.GetResDirect;
        var  sz3 :=  sz3_qr.GetResDirect;
        var args := args_qr.ConvertAll(temp1->temp1.GetResDirect);
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          OpenCLABCInternalException.RaiseIfError( cl.ReleaseKernel(ntv) );
          GC.KeepAlive(args);
        end);
      end;
      
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
  
function KernelCCQ.ThenExec3(sz1,sz2,sz3: CommandQueue<integer>; params args: array of KernelArg): KernelCCQ;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      global_work_offset.InitBeforeInvoke(g, prev_hubs);
        global_work_size.InitBeforeInvoke(g, prev_hubs);
         local_work_size.InitBeforeInvoke(g, prev_hubs);
      foreach var temp1 in args do temp1.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (Kernel, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var global_work_offset_qr: QueueRes<array of UIntPtr>;
      var   global_work_size_qr: QueueRes<array of UIntPtr>;
      var    local_work_size_qr: QueueRes<array of UIntPtr>;
      var               args_qr: array of QueueResVal<ISetableKernelArg>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        global_work_offset_qr := invoker.InvokeBranch&<QueueRes<array of UIntPtr>>(global_work_offset.InvokeToAny); if global_work_offset_qr.IsConst then enq_evs.AddL2(global_work_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(global_work_offset_qr.AttachInvokeActions(g));
          global_work_size_qr := invoker.InvokeBranch&<QueueRes<array of UIntPtr>>(  global_work_size.InvokeToAny); if global_work_size_qr.IsConst then enq_evs.AddL2(global_work_size_qr.AttachInvokeActions(g)) else enq_evs.AddL1(global_work_size_qr.AttachInvokeActions(g));
           local_work_size_qr := invoker.InvokeBranch&<QueueRes<array of UIntPtr>>(   local_work_size.InvokeToAny); if local_work_size_qr.IsConst then enq_evs.AddL2(local_work_size_qr.AttachInvokeActions(g)) else enq_evs.AddL1(local_work_size_qr.AttachInvokeActions(g));
                      args_qr := args.ConvertAll(temp1->begin Result := invoker.InvokeBranch&<QueueResVal<ISetableKernelArg>>(temp1.Invoke); if Result.IsConst then enq_evs.AddL2(Result.AttachInvokeActions(g)) else enq_evs.AddL1(Result.AttachInvokeActions(g)); end);
      end);
      
      Result := (o, cq, evs)->
      begin
        var global_work_offset := global_work_offset_qr.GetResDirect;
        var   global_work_size :=   global_work_size_qr.GetResDirect;
        var    local_work_size :=    local_work_size_qr.GetResDirect;
        var               args :=               args_qr.ConvertAll(temp1->temp1.GetResDirect);
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          OpenCLABCInternalException.RaiseIfError( cl.ReleaseKernel(ntv) );
          GC.KeepAlive(args);
        end);
      end;
      
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
  
function KernelCCQ.ThenExec(global_work_offset, global_work_size, local_work_size: CommandQueue<array of UIntPtr>; params args: array of KernelArg): KernelCCQ;
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
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteData(ptr));
end;

function MemorySegment.WriteData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteData(ptr, mem_offset, len));
end;

function MemorySegment.ReadData(ptr: CommandQueue<IntPtr>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadData(ptr));
end;

function MemorySegment.ReadData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadData(ptr, mem_offset, len));
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
  Result := ReadValue(val, 0);
end;

function MemorySegment.ReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegment; where TRecord: record;
begin
  Result := ReadValue(val, 0);
end;

function MemorySegment.WriteValue<TRecord>(val: TRecord; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteValue&<TRecord>(val, mem_offset));
end;

function MemorySegment.WriteValue<TRecord>(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteValue&<TRecord>(val, mem_offset));
end;

function MemorySegment.WriteValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteValue&<TRecord>(val, mem_offset));
end;

function MemorySegment.WriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteValue&<TRecord>(val, mem_offset));
end;

function MemorySegment.ReadValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadValue&<TRecord>(val, mem_offset));
end;

function MemorySegment.ReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadValue&<TRecord>(val, mem_offset));
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
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteArray1&<TRecord>(a));
end;

function MemorySegment.WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteArray2&<TRecord>(a));
end;

function MemorySegment.WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteArray3&<TRecord>(a));
end;

function MemorySegment.ReadArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadArray1&<TRecord>(a));
end;

function MemorySegment.ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadArray2&<TRecord>(a));
end;

function MemorySegment.ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadArray3&<TRecord>(a));
end;

function MemorySegment.WriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteArray1&<TRecord>(a, a_offset, len, mem_offset));
end;

function MemorySegment.WriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteArray2&<TRecord>(a, a_offset1, a_offset2, len, mem_offset));
end;

function MemorySegment.WriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteArray3&<TRecord>(a, a_offset1, a_offset2, a_offset3, len, mem_offset));
end;

function MemorySegment.ReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadArray1&<TRecord>(a, a_offset, len, mem_offset));
end;

function MemorySegment.ReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadArray2&<TRecord>(a, a_offset1, a_offset2, len, mem_offset));
end;

function MemorySegment.ReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadArray3&<TRecord>(a, a_offset1, a_offset2, a_offset3, len, mem_offset));
end;

{$endregion 1#Write&Read}

{$region 2#Fill}

function MemorySegment.FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillData(ptr, pattern_len));
end;

function MemorySegment.FillData(ptr: CommandQueue<IntPtr>; pattern_len, mem_offset, len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillData(ptr, pattern_len, mem_offset, len));
end;

function MemorySegment.FillValue<TRecord>(val: TRecord): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillValue&<TRecord>(val));
end;

function MemorySegment.FillValue<TRecord>(val: CommandQueue<TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillValue&<TRecord>(val));
end;

function MemorySegment.FillValue<TRecord>(val: TRecord; mem_offset, len: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillValue&<TRecord>(val, mem_offset, len));
end;

function MemorySegment.FillValue<TRecord>(val: CommandQueue<TRecord>; mem_offset, len: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillValue&<TRecord>(val, mem_offset, len));
end;

function MemorySegment.FillArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillArray1&<TRecord>(a));
end;

function MemorySegment.FillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillArray2&<TRecord>(a));
end;

function MemorySegment.FillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillArray3&<TRecord>(a));
end;

function MemorySegment.FillArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillArray1&<TRecord>(a, a_offset, pattern_len, len, mem_offset));
end;

function MemorySegment.FillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillArray2&<TRecord>(a, a_offset1, a_offset2, pattern_len, len, mem_offset));
end;

function MemorySegment.FillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegment; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillArray3&<TRecord>(a, a_offset1, a_offset2, a_offset3, pattern_len, len, mem_offset));
end;

{$endregion 2#Fill}

{$region 3#Copy}

function MemorySegment.CopyTo(mem: CommandQueue<MemorySegment>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenCopyTo(mem));
end;

function MemorySegment.CopyTo(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenCopyTo(mem, from_pos, to_pos, len));
end;

function MemorySegment.CopyFrom(mem: CommandQueue<MemorySegment>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenCopyFrom(mem));
end;

function MemorySegment.CopyFrom(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegment;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenCopyFrom(mem, from_pos, to_pos, len));
end;

{$endregion 3#Copy}

{$region Get}

function MemorySegment.GetValue<TRecord>: TRecord; where TRecord: record;
begin
  Result := GetValue&<TRecord>(0);
end;

function MemorySegment.GetValue<TRecord>(mem_offset: CommandQueue<integer>): TRecord; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenGetValue&<TRecord>(mem_offset));
end;

function MemorySegment.GetArray1<TRecord>: array of TRecord; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenGetArray1&<TRecord>);
end;

function MemorySegment.GetArray1<TRecord>(len: CommandQueue<integer>): array of TRecord; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenGetArray1&<TRecord>(len));
end;

function MemorySegment.GetArray2<TRecord>(len1,len2: CommandQueue<integer>): array[,] of TRecord; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenGetArray2&<TRecord>(len1, len2));
end;

function MemorySegment.GetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): array[,,] of TRecord; where TRecord: record;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenGetArray3&<TRecord>(len1, len2, len3));
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var ptr := ptr_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, o.Size,
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenWriteData(ptr: CommandQueue<IntPtr>): MemorySegmentCCQ;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             ptr.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
             len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var        ptr_qr: QueueRes<IntPtr>;
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
               ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(       ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var        ptr :=        ptr_qr.GetResDirect;
        var mem_offset := mem_offset_qr.GetResDirect;
        var        len :=        len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenWriteData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var ptr := ptr_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, o.Size,
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenReadData(ptr: CommandQueue<IntPtr>): MemorySegmentCCQ;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             ptr.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
             len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var        ptr_qr: QueueRes<IntPtr>;
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
               ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(       ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var        ptr :=        ptr_qr.GetResDirect;
        var mem_offset := mem_offset_qr.GetResDirect;
        var        len :=        len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenReadData(ptr: CommandQueue<IntPtr>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadData(ptr, mem_offset, len));
end;

{$endregion ReadData}

{$region WriteDataAutoSize}

function MemorySegmentCCQ.ThenWriteData(ptr: pointer): MemorySegmentCCQ;
begin
  Result := ThenWriteData(IntPtr(ptr));
end;

{$endregion WriteDataAutoSize}

{$region WriteData}

function MemorySegmentCCQ.ThenWriteData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := ThenWriteData(IntPtr(ptr), mem_offset, len);
end;

{$endregion WriteData}

{$region ReadDataAutoSize}

function MemorySegmentCCQ.ThenReadData(ptr: pointer): MemorySegmentCCQ;
begin
  Result := ThenReadData(IntPtr(ptr));
end;

{$endregion ReadDataAutoSize}

{$region ReadData}

function MemorySegmentCCQ.ThenReadData(ptr: pointer; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := ThenReadData(IntPtr(ptr), mem_offset, len);
end;

{$endregion ReadData}

{$region WriteValue}

function MemorySegmentCCQ.ThenWriteValue<TRecord>(val: TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenWriteValue(val, 0);
end;

{$endregion WriteValue}

{$region WriteValueQ}

function MemorySegmentCCQ.ThenWriteValue<TRecord>(val: CommandQueue<TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenWriteValue(val, 0);
end;

{$endregion WriteValueQ}

{$region WriteValueN}

function MemorySegmentCCQ.ThenWriteValue<TRecord>(val: NativeValue<TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenWriteValue(val, 0);
end;

{$endregion WriteValueN}

{$region WriteValueNQ}

function MemorySegmentCCQ.ThenWriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenWriteValue(val, 0);
end;

{$endregion WriteValueNQ}

{$region ReadValueN}

function MemorySegmentCCQ.ThenReadValue<TRecord>(val: NativeValue<TRecord>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenReadValue(val, 0);
end;

{$endregion ReadValueN}

{$region ReadValueNQ}

function MemorySegmentCCQ.ThenReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenReadValue(val, 0);
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var mem_offset := mem_offset_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr(val),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenWriteValue<TRecord>(val: TRecord; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             val.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var        val_qr: QueueResPtr<TRecord>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
               val_qr := invoker.InvokeBranch&<QueueResPtr<TRecord>>(       val.InvokeToPtr); enq_evs.AddL2(val_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var        val :=        val_qr.res;
        var mem_offset := mem_offset_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr(val),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          GC.KeepAlive(val_qr);
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenWriteValue<TRecord>(val: CommandQueue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var mem_offset := mem_offset_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenWriteValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             val.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var        val_qr: QueueRes<NativeValue<TRecord>>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
               val_qr := invoker.InvokeBranch&<QueueRes<NativeValue<TRecord>>>(       val.InvokeToAny); if val_qr.IsConst then enq_evs.AddL2(val_qr.AttachInvokeActions(g)) else enq_evs.AddL1(val_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var        val :=        val_qr.GetResDirect;
        var mem_offset := mem_offset_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenWriteValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var mem_offset := mem_offset_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenReadValue<TRecord>(val: NativeValue<TRecord>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             val.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var        val_qr: QueueRes<NativeValue<TRecord>>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
               val_qr := invoker.InvokeBranch&<QueueRes<NativeValue<TRecord>>>(       val.InvokeToAny); if val_qr.IsConst then enq_evs.AddL2(val_qr.AttachInvokeActions(g)) else enq_evs.AddL1(val_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var        val :=        val_qr.GetResDirect;
        var mem_offset := mem_offset_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenReadValue<TRecord>(val: CommandQueue<NativeValue<TRecord>>; mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := AddCommand(self, new MemorySegmentCommandReadValueNQ<TRecord>(val, mem_offset));
end;

{$endregion ReadValueNQ}

{$region WriteArray1AutoSize}

function MemorySegmentCCQ.ThenWriteArray1<TRecord>(a: array of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenWriteArray1(new ConstQueue<array of TRecord>(a));
end;

{$endregion WriteArray1AutoSize}

{$region WriteArray2AutoSize}

function MemorySegmentCCQ.ThenWriteArray2<TRecord>(a: array[,] of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenWriteArray2(new ConstQueue<array[,] of TRecord>(a));
end;

{$endregion WriteArray2AutoSize}

{$region WriteArray3AutoSize}

function MemorySegmentCCQ.ThenWriteArray3<TRecord>(a: array[,,] of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenWriteArray3(new ConstQueue<array[,,] of TRecord>(a));
end;

{$endregion WriteArray3AutoSize}

{$region ReadArray1AutoSize}

function MemorySegmentCCQ.ThenReadArray1<TRecord>(a: array of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenReadArray1(new ConstQueue<array of TRecord>(a));
end;

{$endregion ReadArray1AutoSize}

{$region ReadArray2AutoSize}

function MemorySegmentCCQ.ThenReadArray2<TRecord>(a: array[,] of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenReadArray2(new ConstQueue<array[,] of TRecord>(a));
end;

{$endregion ReadArray2AutoSize}

{$region ReadArray3AutoSize}

function MemorySegmentCCQ.ThenReadArray3<TRecord>(a: array[,,] of TRecord): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenReadArray3(new ConstQueue<array[,,] of TRecord>(a));
end;

{$endregion ReadArray3AutoSize}

{$region WriteArray1}

function MemorySegmentCCQ.ThenWriteArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenWriteArray1(new ConstQueue<array of TRecord>(a), a_offset, len, mem_offset);
end;

{$endregion WriteArray1}

{$region WriteArray2}

function MemorySegmentCCQ.ThenWriteArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenWriteArray2(new ConstQueue<array[,] of TRecord>(a), a_offset1,a_offset2, len, mem_offset);
end;

{$endregion WriteArray2}

{$region WriteArray3}

function MemorySegmentCCQ.ThenWriteArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenWriteArray3(new ConstQueue<array[,,] of TRecord>(a), a_offset1,a_offset2,a_offset3, len, mem_offset);
end;

{$endregion WriteArray3}

{$region ReadArray1}

function MemorySegmentCCQ.ThenReadArray1<TRecord>(a: array of TRecord; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenReadArray1(new ConstQueue<array of TRecord>(a), a_offset, len, mem_offset);
end;

{$endregion ReadArray1}

{$region ReadArray2}

function MemorySegmentCCQ.ThenReadArray2<TRecord>(a: array[,] of TRecord; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenReadArray2(new ConstQueue<array[,] of TRecord>(a), a_offset1,a_offset2, len, mem_offset);
end;

{$endregion ReadArray2}

{$region ReadArray3}

function MemorySegmentCCQ.ThenReadArray3<TRecord>(a: array[,,] of TRecord; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
begin
  Result := ThenReadArray3(new ConstQueue<array[,,] of TRecord>(a), a_offset1,a_offset2,a_offset3, len, mem_offset);
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array of TRecord>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenWriteArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array[,] of TRecord>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array[,,] of TRecord>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array of TRecord>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenReadArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array[,] of TRecord>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array[,,] of TRecord>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.InitBeforeInvoke(g, prev_hubs);
        a_offset.InitBeforeInvoke(g, prev_hubs);
             len.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var          a_qr: QueueRes<array of TRecord>;
      var   a_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(         a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
          a_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset.InvokeToAny); if a_offset_qr.IsConst then enq_evs.AddL2(a_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset_qr.AttachInvokeActions(g));
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var          a :=          a_qr.GetResDirect;
        var   a_offset :=   a_offset_qr.GetResDirect;
        var        len :=        len_qr.GetResDirect;
        var mem_offset := mem_offset_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenWriteArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.InitBeforeInvoke(g, prev_hubs);
       a_offset1.InitBeforeInvoke(g, prev_hubs);
       a_offset2.InitBeforeInvoke(g, prev_hubs);
             len.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var          a_qr: QueueRes<array[,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(         a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
         a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset1.InvokeToAny); if a_offset1_qr.IsConst then enq_evs.AddL2(a_offset1_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset1_qr.AttachInvokeActions(g));
         a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset2.InvokeToAny); if a_offset2_qr.IsConst then enq_evs.AddL2(a_offset2_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset2_qr.AttachInvokeActions(g));
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var          a :=          a_qr.GetResDirect;
        var  a_offset1 :=  a_offset1_qr.GetResDirect;
        var  a_offset2 :=  a_offset2_qr.GetResDirect;
        var        len :=        len_qr.GetResDirect;
        var mem_offset := mem_offset_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenWriteArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.InitBeforeInvoke(g, prev_hubs);
       a_offset1.InitBeforeInvoke(g, prev_hubs);
       a_offset2.InitBeforeInvoke(g, prev_hubs);
       a_offset3.InitBeforeInvoke(g, prev_hubs);
             len.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var          a_qr: QueueRes<array[,,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var  a_offset3_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(         a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
         a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset1.InvokeToAny); if a_offset1_qr.IsConst then enq_evs.AddL2(a_offset1_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset1_qr.AttachInvokeActions(g));
         a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset2.InvokeToAny); if a_offset2_qr.IsConst then enq_evs.AddL2(a_offset2_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset2_qr.AttachInvokeActions(g));
         a_offset3_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset3.InvokeToAny); if a_offset3_qr.IsConst then enq_evs.AddL2(a_offset3_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset3_qr.AttachInvokeActions(g));
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var          a :=          a_qr.GetResDirect;
        var  a_offset1 :=  a_offset1_qr.GetResDirect;
        var  a_offset2 :=  a_offset2_qr.GetResDirect;
        var  a_offset3 :=  a_offset3_qr.GetResDirect;
        var        len :=        len_qr.GetResDirect;
        var mem_offset := mem_offset_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2,a_offset3],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenWriteArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.InitBeforeInvoke(g, prev_hubs);
        a_offset.InitBeforeInvoke(g, prev_hubs);
             len.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var          a_qr: QueueRes<array of TRecord>;
      var   a_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(         a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
          a_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset.InvokeToAny); if a_offset_qr.IsConst then enq_evs.AddL2(a_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset_qr.AttachInvokeActions(g));
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var          a :=          a_qr.GetResDirect;
        var   a_offset :=   a_offset_qr.GetResDirect;
        var        len :=        len_qr.GetResDirect;
        var mem_offset := mem_offset_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenReadArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.InitBeforeInvoke(g, prev_hubs);
       a_offset1.InitBeforeInvoke(g, prev_hubs);
       a_offset2.InitBeforeInvoke(g, prev_hubs);
             len.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var          a_qr: QueueRes<array[,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(         a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
         a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset1.InvokeToAny); if a_offset1_qr.IsConst then enq_evs.AddL2(a_offset1_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset1_qr.AttachInvokeActions(g));
         a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset2.InvokeToAny); if a_offset2_qr.IsConst then enq_evs.AddL2(a_offset2_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset2_qr.AttachInvokeActions(g));
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var          a :=          a_qr.GetResDirect;
        var  a_offset1 :=  a_offset1_qr.GetResDirect;
        var  a_offset2 :=  a_offset2_qr.GetResDirect;
        var        len :=        len_qr.GetResDirect;
        var mem_offset := mem_offset_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenReadArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
               a.InitBeforeInvoke(g, prev_hubs);
       a_offset1.InitBeforeInvoke(g, prev_hubs);
       a_offset2.InitBeforeInvoke(g, prev_hubs);
       a_offset3.InitBeforeInvoke(g, prev_hubs);
             len.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var          a_qr: QueueRes<array[,,] of TRecord>;
      var  a_offset1_qr: QueueRes<integer>;
      var  a_offset2_qr: QueueRes<integer>;
      var  a_offset3_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                 a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(         a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
         a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset1.InvokeToAny); if a_offset1_qr.IsConst then enq_evs.AddL2(a_offset1_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset1_qr.AttachInvokeActions(g));
         a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset2.InvokeToAny); if a_offset2_qr.IsConst then enq_evs.AddL2(a_offset2_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset2_qr.AttachInvokeActions(g));
         a_offset3_qr := invoker.InvokeBranch&<QueueRes<integer>>( a_offset3.InvokeToAny); if a_offset3_qr.IsConst then enq_evs.AddL2(a_offset3_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset3_qr.AttachInvokeActions(g));
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var          a :=          a_qr.GetResDirect;
        var  a_offset1 :=  a_offset1_qr.GetResDirect;
        var  a_offset2 :=  a_offset2_qr.GetResDirect;
        var  a_offset3 :=  a_offset3_qr.GetResDirect;
        var        len :=        len_qr.GetResDirect;
        var mem_offset := mem_offset_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          a[a_offset1,a_offset2,a_offset3],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenReadArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
              ptr.InitBeforeInvoke(g, prev_hubs);
      pattern_len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(        ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.InvokeToAny); if pattern_len_qr.IsConst then enq_evs.AddL2(pattern_len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(pattern_len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var         ptr :=         ptr_qr.GetResDirect;
        var pattern_len := pattern_len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): MemorySegmentCCQ;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
              ptr.InitBeforeInvoke(g, prev_hubs);
      pattern_len.InitBeforeInvoke(g, prev_hubs);
       mem_offset.InitBeforeInvoke(g, prev_hubs);
              len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(        ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.InvokeToAny); if pattern_len_qr.IsConst then enq_evs.AddL2(pattern_len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(pattern_len_qr.AttachInvokeActions(g));
         mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>( mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var         ptr :=         ptr_qr.GetResDirect;
        var pattern_len := pattern_len_qr.GetResDirect;
        var  mem_offset :=  mem_offset_qr.GetResDirect;
        var         len :=         len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenFillData(ptr: CommandQueue<IntPtr>; pattern_len, mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      
      Result := (o, cq, evs)->
      begin
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val^);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenFillValue<TRecord>(val: TRecord): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var val_qr: QueueResPtr<TRecord>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueResPtr<TRecord>>(val.InvokeToPtr); enq_evs.AddL2(val_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var val := val_qr.res;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<TRecord>),
          UIntPtr.Zero, o.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          GC.KeepAlive(val_qr);
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenFillValue<TRecord>(val: CommandQueue<TRecord>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem_offset.InitBeforeInvoke(g, prev_hubs);
             len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var mem_offset := mem_offset_qr.GetResDirect;
        var        len :=        len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new UIntPtr(mem_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenFillValue<TRecord>(val: TRecord; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             val.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
             len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var        val_qr: QueueResPtr<TRecord>;
      var mem_offset_qr: QueueRes<integer>;
      var        len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
               val_qr := invoker.InvokeBranch&<QueueResPtr<TRecord>>(       val.InvokeToPtr); enq_evs.AddL2(val_qr.AttachInvokeActions(g));
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
               len_qr := invoker.InvokeBranch&<QueueRes<integer>>(       len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var        val :=        val_qr.res;
        var mem_offset := mem_offset_qr.GetResDirect;
        var        len :=        len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new UIntPtr(mem_offset), new UIntPtr(len),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          GC.KeepAlive(val_qr);
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenFillValue<TRecord>(val: CommandQueue<TRecord>; mem_offset, len: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array of TRecord>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenFillArray1<TRecord>(a: CommandQueue<array of TRecord>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array[,] of TRecord>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenFillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array[,,] of TRecord>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenFillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
                a.InitBeforeInvoke(g, prev_hubs);
         a_offset.InitBeforeInvoke(g, prev_hubs);
      pattern_len.InitBeforeInvoke(g, prev_hubs);
              len.InitBeforeInvoke(g, prev_hubs);
       mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var           a_qr: QueueRes<array of TRecord>;
      var    a_offset_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<QueueRes<array of TRecord>>(          a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
           a_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(   a_offset.InvokeToAny); if a_offset_qr.IsConst then enq_evs.AddL2(a_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset_qr.AttachInvokeActions(g));
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.InvokeToAny); if pattern_len_qr.IsConst then enq_evs.AddL2(pattern_len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(pattern_len_qr.AttachInvokeActions(g));
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
         mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>( mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var           a :=           a_qr.GetResDirect;
        var    a_offset :=    a_offset_qr.GetResDirect;
        var pattern_len := pattern_len_qr.GetResDirect;
        var         len :=         len_qr.GetResDirect;
        var  mem_offset :=  mem_offset_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset], new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenFillArray1<TRecord>(a: CommandQueue<array of TRecord>; a_offset, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
                a.InitBeforeInvoke(g, prev_hubs);
        a_offset1.InitBeforeInvoke(g, prev_hubs);
        a_offset2.InitBeforeInvoke(g, prev_hubs);
      pattern_len.InitBeforeInvoke(g, prev_hubs);
              len.InitBeforeInvoke(g, prev_hubs);
       mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var           a_qr: QueueRes<array[,] of TRecord>;
      var   a_offset1_qr: QueueRes<integer>;
      var   a_offset2_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<QueueRes<array[,] of TRecord>>(          a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
          a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset1.InvokeToAny); if a_offset1_qr.IsConst then enq_evs.AddL2(a_offset1_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset1_qr.AttachInvokeActions(g));
          a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset2.InvokeToAny); if a_offset2_qr.IsConst then enq_evs.AddL2(a_offset2_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset2_qr.AttachInvokeActions(g));
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.InvokeToAny); if pattern_len_qr.IsConst then enq_evs.AddL2(pattern_len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(pattern_len_qr.AttachInvokeActions(g));
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
         mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>( mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var           a :=           a_qr.GetResDirect;
        var   a_offset1 :=   a_offset1_qr.GetResDirect;
        var   a_offset2 :=   a_offset2_qr.GetResDirect;
        var pattern_len := pattern_len_qr.GetResDirect;
        var         len :=         len_qr.GetResDirect;
        var  mem_offset :=  mem_offset_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset1,a_offset2], new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenFillArray2<TRecord>(a: CommandQueue<array[,] of TRecord>; a_offset1,a_offset2, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
                a.InitBeforeInvoke(g, prev_hubs);
        a_offset1.InitBeforeInvoke(g, prev_hubs);
        a_offset2.InitBeforeInvoke(g, prev_hubs);
        a_offset3.InitBeforeInvoke(g, prev_hubs);
      pattern_len.InitBeforeInvoke(g, prev_hubs);
              len.InitBeforeInvoke(g, prev_hubs);
       mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var           a_qr: QueueRes<array[,,] of TRecord>;
      var   a_offset1_qr: QueueRes<integer>;
      var   a_offset2_qr: QueueRes<integer>;
      var   a_offset3_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      var  mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<QueueRes<array[,,] of TRecord>>(          a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
          a_offset1_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset1.InvokeToAny); if a_offset1_qr.IsConst then enq_evs.AddL2(a_offset1_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset1_qr.AttachInvokeActions(g));
          a_offset2_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset2.InvokeToAny); if a_offset2_qr.IsConst then enq_evs.AddL2(a_offset2_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset2_qr.AttachInvokeActions(g));
          a_offset3_qr := invoker.InvokeBranch&<QueueRes<integer>>(  a_offset3.InvokeToAny); if a_offset3_qr.IsConst then enq_evs.AddL2(a_offset3_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset3_qr.AttachInvokeActions(g));
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.InvokeToAny); if pattern_len_qr.IsConst then enq_evs.AddL2(pattern_len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(pattern_len_qr.AttachInvokeActions(g));
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
         mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>( mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var           a :=           a_qr.GetResDirect;
        var   a_offset1 :=   a_offset1_qr.GetResDirect;
        var   a_offset2 :=   a_offset2_qr.GetResDirect;
        var   a_offset3 :=   a_offset3_qr.GetResDirect;
        var pattern_len := pattern_len_qr.GetResDirect;
        var         len :=         len_qr.GetResDirect;
        var  mem_offset :=  mem_offset_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset1,a_offset2,a_offset3], new UIntPtr(pattern_len),
          new UIntPtr(mem_offset), new UIntPtr(len*Marshal.SizeOf&<TRecord>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenFillArray3<TRecord>(a: CommandQueue<array[,,] of TRecord>; a_offset1,a_offset2,a_offset3, pattern_len, len, mem_offset: CommandQueue<integer>): MemorySegmentCCQ; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var mem_qr: QueueRes<MemorySegment>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        mem_qr := invoker.InvokeBranch&<QueueRes<MemorySegment>>(mem.InvokeToAny); if mem_qr.IsConst then enq_evs.AddL2(mem_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var mem := mem_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, o.Native,mem.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          o.Size64<mem.Size64 ? o.Size : mem.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem: ';
      mem.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenCopyTo(mem: CommandQueue<MemorySegment>): MemorySegmentCCQ;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
           mem.InitBeforeInvoke(g, prev_hubs);
      from_pos.InitBeforeInvoke(g, prev_hubs);
        to_pos.InitBeforeInvoke(g, prev_hubs);
           len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var      mem_qr: QueueRes<MemorySegment>;
      var from_pos_qr: QueueRes<integer>;
      var   to_pos_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
             mem_qr := invoker.InvokeBranch&<QueueRes<MemorySegment>>(     mem.InvokeToAny); if mem_qr.IsConst then enq_evs.AddL2(mem_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_qr.AttachInvokeActions(g));
        from_pos_qr := invoker.InvokeBranch&<QueueRes<integer>>(from_pos.InvokeToAny); if from_pos_qr.IsConst then enq_evs.AddL2(from_pos_qr.AttachInvokeActions(g)) else enq_evs.AddL1(from_pos_qr.AttachInvokeActions(g));
          to_pos_qr := invoker.InvokeBranch&<QueueRes<integer>>(  to_pos.InvokeToAny); if to_pos_qr.IsConst then enq_evs.AddL2(to_pos_qr.AttachInvokeActions(g)) else enq_evs.AddL1(to_pos_qr.AttachInvokeActions(g));
             len_qr := invoker.InvokeBranch&<QueueRes<integer>>(     len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var      mem :=      mem_qr.GetResDirect;
        var from_pos := from_pos_qr.GetResDirect;
        var   to_pos :=   to_pos_qr.GetResDirect;
        var      len :=      len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, o.Native,mem.Native,
          new UIntPtr(from_pos), new UIntPtr(to_pos),
          new UIntPtr(len),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenCopyTo(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegmentCCQ;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      mem.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var mem_qr: QueueRes<MemorySegment>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        mem_qr := invoker.InvokeBranch&<QueueRes<MemorySegment>>(mem.InvokeToAny); if mem_qr.IsConst then enq_evs.AddL2(mem_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var mem := mem_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, mem.Native,o.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          o.Size64<mem.Size64 ? o.Size : mem.Size,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem: ';
      mem.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenCopyFrom(mem: CommandQueue<MemorySegment>): MemorySegmentCCQ;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
           mem.InitBeforeInvoke(g, prev_hubs);
      from_pos.InitBeforeInvoke(g, prev_hubs);
        to_pos.InitBeforeInvoke(g, prev_hubs);
           len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var      mem_qr: QueueRes<MemorySegment>;
      var from_pos_qr: QueueRes<integer>;
      var   to_pos_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
             mem_qr := invoker.InvokeBranch&<QueueRes<MemorySegment>>(     mem.InvokeToAny); if mem_qr.IsConst then enq_evs.AddL2(mem_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_qr.AttachInvokeActions(g));
        from_pos_qr := invoker.InvokeBranch&<QueueRes<integer>>(from_pos.InvokeToAny); if from_pos_qr.IsConst then enq_evs.AddL2(from_pos_qr.AttachInvokeActions(g)) else enq_evs.AddL1(from_pos_qr.AttachInvokeActions(g));
          to_pos_qr := invoker.InvokeBranch&<QueueRes<integer>>(  to_pos.InvokeToAny); if to_pos_qr.IsConst then enq_evs.AddL2(to_pos_qr.AttachInvokeActions(g)) else enq_evs.AddL1(to_pos_qr.AttachInvokeActions(g));
             len_qr := invoker.InvokeBranch&<QueueRes<integer>>(     len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var      mem :=      mem_qr.GetResDirect;
        var from_pos := from_pos_qr.GetResDirect;
        var   to_pos :=   to_pos_qr.GetResDirect;
        var      len :=      len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, mem.Native,o.Native,
          new UIntPtr(from_pos), new UIntPtr(to_pos),
          new UIntPtr(len),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function MemorySegmentCCQ.ThenCopyFrom(mem: CommandQueue<MemorySegment>; from_pos, to_pos, len: CommandQueue<integer>): MemorySegmentCCQ;
begin
  Result := AddCommand(self, new MemorySegmentCommandCopyFrom(mem, from_pos, to_pos, len));
end;

{$endregion CopyFrom}

{$endregion 3#Copy}

{$region Get}

{$region GetValue}

function MemorySegmentCCQ.ThenGetValue<TRecord>: CommandQueue<TRecord>; where TRecord: record;
begin
  Result := ThenGetValue&<TRecord>(0);
end;

{$endregion GetValue}

{$region GetValue}

type
  MemorySegmentCommandGetValue<TRecord> = sealed class(EnqueueableGetPtrCommand<MemorySegment, TRecord>)
  where TRecord: record;
    private mem_offset: CommandQueue<integer>;
    
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      prev_commands.InitBeforeInvoke(g, prev_hubs);
      mem_offset.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList, QueueRes<TRecord>)->DirectEnqRes; override;
    begin
      var mem_offset_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        mem_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(mem_offset.InvokeToAny); if mem_offset_qr.IsConst then enq_evs.AddL2(mem_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(mem_offset_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs, own_qr)->
      begin
        var mem_offset := mem_offset_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(mem_offset), new UIntPtr(Marshal.SizeOf&<TRecord>),
          new IntPtr((own_qr as QueueResPtr<TRecord>).res),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          GC.KeepAlive(own_qr);
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'mem_offset: ';
      mem_offset.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenGetValue<TRecord>(mem_offset: CommandQueue<integer>): CommandQueue<TRecord>; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := prev_commands.InitBeforeInvoke(g, prev_hubs);
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList, QueueRes<array of TRecord>)->DirectEnqRes; override;
    begin
      
      Result := (o, cq, evs, own_qr)->
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          res_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override := sb += #10;
    
  end;
  
function MemorySegmentCCQ.ThenGetArray1<TRecord>: CommandQueue<array of TRecord>; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      prev_commands.InitBeforeInvoke(g, prev_hubs);
      len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList, QueueRes<array of TRecord>)->DirectEnqRes; override;
    begin
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>(len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs, own_qr)->
      begin
        var len := len_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          res_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'len: ';
      len.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function MemorySegmentCCQ.ThenGetArray1<TRecord>(len: CommandQueue<integer>): CommandQueue<array of TRecord>; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      prev_commands.InitBeforeInvoke(g, prev_hubs);
      len1.InitBeforeInvoke(g, prev_hubs);
      len2.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList, QueueRes<array[,] of TRecord>)->DirectEnqRes; override;
    begin
      var len1_qr: QueueRes<integer>;
      var len2_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        len1_qr := invoker.InvokeBranch&<QueueRes<integer>>(len1.InvokeToAny); if len1_qr.IsConst then enq_evs.AddL2(len1_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len1_qr.AttachInvokeActions(g));
        len2_qr := invoker.InvokeBranch&<QueueRes<integer>>(len2.InvokeToAny); if len2_qr.IsConst then enq_evs.AddL2(len2_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len2_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs, own_qr)->
      begin
        var len1 := len1_qr.GetResDirect;
        var len2 := len2_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          res_hnd.Free;
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenGetArray2<TRecord>(len1,len2: CommandQueue<integer>): CommandQueue<array[,] of TRecord>; where TRecord: record;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      prev_commands.InitBeforeInvoke(g, prev_hubs);
      len1.InitBeforeInvoke(g, prev_hubs);
      len2.InitBeforeInvoke(g, prev_hubs);
      len3.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (MemorySegment, cl_command_queue, EventList, QueueRes<array[,,] of TRecord>)->DirectEnqRes; override;
    begin
      var len1_qr: QueueRes<integer>;
      var len2_qr: QueueRes<integer>;
      var len3_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        len1_qr := invoker.InvokeBranch&<QueueRes<integer>>(len1.InvokeToAny); if len1_qr.IsConst then enq_evs.AddL2(len1_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len1_qr.AttachInvokeActions(g));
        len2_qr := invoker.InvokeBranch&<QueueRes<integer>>(len2.InvokeToAny); if len2_qr.IsConst then enq_evs.AddL2(len2_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len2_qr.AttachInvokeActions(g));
        len3_qr := invoker.InvokeBranch&<QueueRes<integer>>(len3.InvokeToAny); if len3_qr.IsConst then enq_evs.AddL2(len3_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len3_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs, own_qr)->
      begin
        var len1 := len1_qr.GetResDirect;
        var len2 := len2_qr.GetResDirect;
        var len3 := len3_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          res_hnd.Free;
        end);
      end;
      
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
  
function MemorySegmentCCQ.ThenGetArray3<TRecord>(len1,len2,len3: CommandQueue<integer>): CommandQueue<array[,,] of TRecord>; where TRecord: record;
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
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteData(ptr));
end;

function CLArray<T>.WriteData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteData(ptr, ind, len));
end;

function CLArray<T>.ReadData(ptr: CommandQueue<IntPtr>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadData(ptr));
end;

function CLArray<T>.ReadData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadData(ptr, ind, len));
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
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteValue(val, ind));
end;

function CLArray<T>.WriteValue(val: CommandQueue<&T>; ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteValue(val, ind));
end;

function CLArray<T>.WriteValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteValue(val, ind));
end;

function CLArray<T>.WriteValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteValue(val, ind));
end;

function CLArray<T>.ReadValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadValue(val, ind));
end;

function CLArray<T>.ReadValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadValue(val, ind));
end;

function CLArray<T>.WriteArray(a: CommandQueue<array of &T>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteArray(a));
end;

function CLArray<T>.WriteArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenWriteArray(a, ind, len, a_ind));
end;

function CLArray<T>.ReadArray(a: CommandQueue<array of &T>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadArray(a));
end;

function CLArray<T>.ReadArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenReadArray(a, ind, len, a_ind));
end;

{$endregion 1#Write&Read}

{$region 2#Fill}

function CLArray<T>.FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillData(ptr, pattern_len));
end;

function CLArray<T>.FillData(ptr: CommandQueue<IntPtr>; pattern_len, ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillData(ptr, pattern_len, ind, len));
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
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillValue(val));
end;

function CLArray<T>.FillValue(val: CommandQueue<&T>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillValue(val));
end;

function CLArray<T>.FillValue(val: &T; ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillValue(val, ind, len));
end;

function CLArray<T>.FillValue(val: CommandQueue<&T>; ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillValue(val, ind, len));
end;

function CLArray<T>.FillArray(a: CommandQueue<array of &T>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillArray(a));
end;

function CLArray<T>.FillArray(a: CommandQueue<array of &T>; a_offset,pattern_len, ind,len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenFillArray(a, a_offset, pattern_len, ind, len));
end;

{$endregion 2#Fill}

{$region 3#Copy}

function CLArray<T>.CopyTo(a: CommandQueue<CLArray<T>>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenCopyTo(a));
end;

function CLArray<T>.CopyTo(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenCopyTo(a, from_ind, to_ind, len));
end;

function CLArray<T>.CopyFrom(a: CommandQueue<CLArray<T>>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenCopyFrom(a));
end;

function CLArray<T>.CopyFrom(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArray<T>;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenCopyFrom(a, from_ind, to_ind, len));
end;

{$endregion 3#Copy}

{$region Get}

function CLArray<T>.GetValue(ind: CommandQueue<integer>): &T;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenGetValue(ind));
end;

function CLArray<T>.GetArray: array of &T;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenGetArray);
end;

function CLArray<T>.GetArray(ind, len: CommandQueue<integer>): array of &T;
begin
  Result := Context.Default.SyncInvoke(self.NewQueue.ThenGetArray(ind, len));
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var ptr := ptr_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(o.ByteSize),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.ThenWriteData(ptr: CommandQueue<IntPtr>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.InitBeforeInvoke(g, prev_hubs);
      ind.InitBeforeInvoke(g, prev_hubs);
      len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>(len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var ptr := ptr_qr.GetResDirect;
        var ind := ind_qr.GetResDirect;
        var len := len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenWriteData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var ptr := ptr_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(0), new UIntPtr(o.ByteSize),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ptr: ';
      ptr.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.ThenReadData(ptr: CommandQueue<IntPtr>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ptr.InitBeforeInvoke(g, prev_hubs);
      ind.InitBeforeInvoke(g, prev_hubs);
      len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var ptr_qr: QueueRes<IntPtr>;
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>(len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var ptr := ptr_qr.GetResDirect;
        var ind := ind_qr.GetResDirect;
        var len := len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenReadData(ptr: CommandQueue<IntPtr>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandReadData<T>(ptr, ind, len));
end;

{$endregion ReadData}

{$region WriteDataAutoSize}

function CLArrayCCQ<T>.ThenWriteData(ptr: pointer): CLArrayCCQ<T>;
begin
  Result := ThenWriteData(IntPtr(ptr));
end;

{$endregion WriteDataAutoSize}

{$region WriteData}

function CLArrayCCQ<T>.ThenWriteData(ptr: pointer; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := ThenWriteData(IntPtr(ptr), ind, len);
end;

{$endregion WriteData}

{$region ReadDataAutoSize}

function CLArrayCCQ<T>.ThenReadData(ptr: pointer): CLArrayCCQ<T>;
begin
  Result := ThenReadData(IntPtr(ptr));
end;

{$endregion ReadDataAutoSize}

{$region ReadData}

function CLArrayCCQ<T>.ThenReadData(ptr: pointer; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := ThenReadData(IntPtr(ptr), ind, len);
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var ind := ind_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          new IntPtr(val),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenWriteValue(val: &T; ind: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.InitBeforeInvoke(g, prev_hubs);
      ind.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var val_qr: QueueResPtr<&T>;
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueResPtr<&T>>(val.InvokeToPtr); enq_evs.AddL2(val_qr.AttachInvokeActions(g));
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var val := val_qr.res;
        var ind := ind_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          new IntPtr(val),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          GC.KeepAlive(val_qr);
        end);
      end;
      
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
  
function CLArrayCCQ<T>.ThenWriteValue(val: CommandQueue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var ind := ind_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenWriteValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.InitBeforeInvoke(g, prev_hubs);
      ind.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var val_qr: QueueRes<NativeValue<&T>>;
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueRes<NativeValue<&T>>>(val.InvokeToAny); if val_qr.IsConst then enq_evs.AddL2(val_qr.AttachInvokeActions(g)) else enq_evs.AddL1(val_qr.AttachInvokeActions(g));
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var val := val_qr.GetResDirect;
        var ind := ind_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenWriteValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var ind := ind_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenReadValue(val: NativeValue<&T>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.InitBeforeInvoke(g, prev_hubs);
      ind.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var val_qr: QueueRes<NativeValue<&T>>;
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueRes<NativeValue<&T>>>(val.InvokeToAny); if val_qr.IsConst then enq_evs.AddL2(val_qr.AttachInvokeActions(g)) else enq_evs.AddL1(val_qr.AttachInvokeActions(g));
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var val := val_qr.GetResDirect;
        var ind := ind_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          val.ptr,
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenReadValue(val: CommandQueue<NativeValue<&T>>; ind: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array of &T>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<T>),
          a[0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.ThenWriteArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
          a.InitBeforeInvoke(g, prev_hubs);
        ind.InitBeforeInvoke(g, prev_hubs);
        len.InitBeforeInvoke(g, prev_hubs);
      a_ind.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var     a_qr: QueueRes<array of &T>;
      var   ind_qr: QueueRes<integer>;
      var   len_qr: QueueRes<integer>;
      var a_ind_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
            a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(    a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
          ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(  ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
          len_qr := invoker.InvokeBranch&<QueueRes<integer>>(  len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
        a_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(a_ind.InvokeToAny); if a_ind_qr.IsConst then enq_evs.AddL2(a_ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_ind_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var     a :=     a_qr.GetResDirect;
        var   ind :=   ind_qr.GetResDirect;
        var   len :=   len_qr.GetResDirect;
        var a_ind := a_ind_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueWriteBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind*Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          a[a_ind],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function CLArrayCCQ<T>.ThenWriteArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array of &T>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          UIntPtr.Zero, new UIntPtr(a.Length*Marshal.SizeOf&<T>),
          a[0],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.ThenReadArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
          a.InitBeforeInvoke(g, prev_hubs);
        ind.InitBeforeInvoke(g, prev_hubs);
        len.InitBeforeInvoke(g, prev_hubs);
      a_ind.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var     a_qr: QueueRes<array of &T>;
      var   ind_qr: QueueRes<integer>;
      var   len_qr: QueueRes<integer>;
      var a_ind_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
            a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(    a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
          ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(  ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
          len_qr := invoker.InvokeBranch&<QueueRes<integer>>(  len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
        a_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(a_ind.InvokeToAny); if a_ind_qr.IsConst then enq_evs.AddL2(a_ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_ind_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var     a :=     a_qr.GetResDirect;
        var   ind :=   ind_qr.GetResDirect;
        var   len :=   len_qr.GetResDirect;
        var a_ind := a_ind_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(ind*Marshal.SizeOf&<T>), new UIntPtr(len*Marshal.SizeOf&<T>),
          a[a_ind],
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function CLArrayCCQ<T>.ThenReadArray(a: CommandQueue<array of &T>; ind, len, a_ind: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
              ptr.InitBeforeInvoke(g, prev_hubs);
      pattern_len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(        ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.InvokeToAny); if pattern_len_qr.IsConst then enq_evs.AddL2(pattern_len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(pattern_len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var         ptr :=         ptr_qr.GetResDirect;
        var pattern_len := pattern_len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len*Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
              ptr.InitBeforeInvoke(g, prev_hubs);
      pattern_len.InitBeforeInvoke(g, prev_hubs);
              ind.InitBeforeInvoke(g, prev_hubs);
              len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var         ptr_qr: QueueRes<IntPtr>;
      var pattern_len_qr: QueueRes<integer>;
      var         ind_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                ptr_qr := invoker.InvokeBranch&<QueueRes<IntPtr>>(        ptr.InvokeToAny); if ptr_qr.IsConst then enq_evs.AddL2(ptr_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ptr_qr.AttachInvokeActions(g));
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.InvokeToAny); if pattern_len_qr.IsConst then enq_evs.AddL2(pattern_len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(pattern_len_qr.AttachInvokeActions(g));
                ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(        ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var         ptr :=         ptr_qr.GetResDirect;
        var pattern_len := pattern_len_qr.GetResDirect;
        var         ind :=         ind_qr.GetResDirect;
        var         len :=         len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          ptr, new UIntPtr(pattern_len*Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenFillData(ptr: CommandQueue<IntPtr>; pattern_len, ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandFillData<T>(ptr, pattern_len, ind, len));
end;

{$endregion FillData}

{$region FillDataAutoSize}

function CLArrayCCQ<T>.ThenFillData(ptr: pointer; pattern_len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := ThenFillData(IntPtr(ptr), pattern_len);
end;

{$endregion FillDataAutoSize}

{$region FillData}

function CLArrayCCQ<T>.ThenFillData(ptr: pointer; pattern_len, ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := ThenFillData(IntPtr(ptr), pattern_len, ind, len);
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      
      Result := (o, cq, evs)->
      begin
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      sb.Append(val^);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.ThenFillValue(val: &T): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var val_qr: QueueResPtr<&T>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueResPtr<&T>>(val.InvokeToPtr); enq_evs.AddL2(val_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var val := val_qr.res;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          GC.KeepAlive(val_qr);
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'val: ';
      val.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.ThenFillValue(val: CommandQueue<&T>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      ind.InitBeforeInvoke(g, prev_hubs);
      len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>(len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var ind := ind_qr.GetResDirect;
        var len := len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenFillValue(val: &T; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      val.InitBeforeInvoke(g, prev_hubs);
      ind.InitBeforeInvoke(g, prev_hubs);
      len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var val_qr: QueueResPtr<&T>;
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        val_qr := invoker.InvokeBranch&<QueueResPtr<&T>>(val.InvokeToPtr); enq_evs.AddL2(val_qr.AttachInvokeActions(g));
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>(len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var val := val_qr.res;
        var ind := ind_qr.GetResDirect;
        var len := len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          new IntPtr(val), new UIntPtr(Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          GC.KeepAlive(val_qr);
        end);
      end;
      
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
  
function CLArrayCCQ<T>.ThenFillValue(val: CommandQueue<&T>; ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<array of &T>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[0], new UIntPtr(a.Length*Marshal.SizeOf&<&T>),
          UIntPtr.Zero, new UIntPtr(o.ByteSize),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.ThenFillArray(a: CommandQueue<array of &T>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
                a.InitBeforeInvoke(g, prev_hubs);
         a_offset.InitBeforeInvoke(g, prev_hubs);
      pattern_len.InitBeforeInvoke(g, prev_hubs);
              ind.InitBeforeInvoke(g, prev_hubs);
              len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var           a_qr: QueueRes<array of &T>;
      var    a_offset_qr: QueueRes<integer>;
      var pattern_len_qr: QueueRes<integer>;
      var         ind_qr: QueueRes<integer>;
      var         len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
                  a_qr := invoker.InvokeBranch&<QueueRes<array of &T>>(          a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
           a_offset_qr := invoker.InvokeBranch&<QueueRes<integer>>(   a_offset.InvokeToAny); if a_offset_qr.IsConst then enq_evs.AddL2(a_offset_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_offset_qr.AttachInvokeActions(g));
        pattern_len_qr := invoker.InvokeBranch&<QueueRes<integer>>(pattern_len.InvokeToAny); if pattern_len_qr.IsConst then enq_evs.AddL2(pattern_len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(pattern_len_qr.AttachInvokeActions(g));
                ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(        ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
                len_qr := invoker.InvokeBranch&<QueueRes<integer>>(        len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var           a :=           a_qr.GetResDirect;
        var    a_offset :=    a_offset_qr.GetResDirect;
        var pattern_len := pattern_len_qr.GetResDirect;
        var         ind :=         ind_qr.GetResDirect;
        var         len :=         len_qr.GetResDirect;
        var a_hnd := GCHandle.Alloc(a, GCHandleType.Pinned);
        
        var res_ev: cl_event;
        
        var ec := cl.EnqueueFillBuffer(
          cq, o.Native,
          a[a_offset], new UIntPtr(pattern_len*Marshal.SizeOf&<&T>),
          new UIntPtr(ind*Marshal.SizeOf&<&T>), new UIntPtr(len*Marshal.SizeOf&<&T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          a_hnd.Free;
        end);
      end;
      
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
  
function CLArrayCCQ<T>.ThenFillArray(a: CommandQueue<array of &T>; a_offset,pattern_len, ind,len: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<CLArray<T>>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<CLArray<T>>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, o.Native,a.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          new UIntPtr(Min(o.ByteSize, a.ByteSize)),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.ThenCopyTo(a: CommandQueue<CLArray<T>>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             a.InitBeforeInvoke(g, prev_hubs);
      from_ind.InitBeforeInvoke(g, prev_hubs);
        to_ind.InitBeforeInvoke(g, prev_hubs);
           len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var        a_qr: QueueRes<CLArray<T>>;
      var from_ind_qr: QueueRes<integer>;
      var   to_ind_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
               a_qr := invoker.InvokeBranch&<QueueRes<CLArray<T>>>(       a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
        from_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(from_ind.InvokeToAny); if from_ind_qr.IsConst then enq_evs.AddL2(from_ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(from_ind_qr.AttachInvokeActions(g));
          to_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(  to_ind.InvokeToAny); if to_ind_qr.IsConst then enq_evs.AddL2(to_ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(to_ind_qr.AttachInvokeActions(g));
             len_qr := invoker.InvokeBranch&<QueueRes<integer>>(     len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var        a :=        a_qr.GetResDirect;
        var from_ind := from_ind_qr.GetResDirect;
        var   to_ind :=   to_ind_qr.GetResDirect;
        var      len :=      len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, o.Native,a.Native,
          new UIntPtr(from_ind*Marshal.SizeOf&<T>), new UIntPtr(to_ind*Marshal.SizeOf&<T>),
          new UIntPtr(len*Marshal.SizeOf&<T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenCopyTo(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      a.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var a_qr: QueueRes<CLArray<T>>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        a_qr := invoker.InvokeBranch&<QueueRes<CLArray<T>>>(a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var a := a_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, a.Native,o.Native,
          UIntPtr.Zero, UIntPtr.Zero,
          new UIntPtr(Min(o.ByteSize, a.ByteSize)),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'a: ';
      a.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.ThenCopyFrom(a: CommandQueue<CLArray<T>>): CLArrayCCQ<T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
             a.InitBeforeInvoke(g, prev_hubs);
      from_ind.InitBeforeInvoke(g, prev_hubs);
        to_ind.InitBeforeInvoke(g, prev_hubs);
           len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList)->DirectEnqRes; override;
    begin
      var        a_qr: QueueRes<CLArray<T>>;
      var from_ind_qr: QueueRes<integer>;
      var   to_ind_qr: QueueRes<integer>;
      var      len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
               a_qr := invoker.InvokeBranch&<QueueRes<CLArray<T>>>(       a.InvokeToAny); if a_qr.IsConst then enq_evs.AddL2(a_qr.AttachInvokeActions(g)) else enq_evs.AddL1(a_qr.AttachInvokeActions(g));
        from_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(from_ind.InvokeToAny); if from_ind_qr.IsConst then enq_evs.AddL2(from_ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(from_ind_qr.AttachInvokeActions(g));
          to_ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(  to_ind.InvokeToAny); if to_ind_qr.IsConst then enq_evs.AddL2(to_ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(to_ind_qr.AttachInvokeActions(g));
             len_qr := invoker.InvokeBranch&<QueueRes<integer>>(     len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs)->
      begin
        var        a :=        a_qr.GetResDirect;
        var from_ind := from_ind_qr.GetResDirect;
        var   to_ind :=   to_ind_qr.GetResDirect;
        var      len :=      len_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueCopyBuffer(
          cq, a.Native,o.Native,
          new UIntPtr(from_ind*Marshal.SizeOf&<T>), new UIntPtr(to_ind*Marshal.SizeOf&<T>),
          new UIntPtr(len*Marshal.SizeOf&<T>),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, nil);
      end;
      
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
  
function CLArrayCCQ<T>.ThenCopyFrom(a: CommandQueue<CLArray<T>>; from_ind, to_ind, len: CommandQueue<integer>): CLArrayCCQ<T>;
begin
  Result := AddCommand(self, new CLArrayCommandCopyFrom<T>(a, from_ind, to_ind, len));
end;

{$endregion CopyFrom}

{$endregion 3#Copy}

{$region Get}

{$region GetValue}

type
  CLArrayCommandGetValue<T> = sealed class(EnqueueableGetPtrCommand<CLArray<T>, &T>)
  where T: record;
    private ind: CommandQueue<integer>;
    
    public function EnqEvCapacity: integer; override := 1;
    
    public constructor(ccq: CLArrayCCQ<T>; ind: CommandQueue<integer>);
    begin
      inherited Create(ccq);
      self.ind := ind;
    end;
    private constructor := raise new System.InvalidOperationException;
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      prev_commands.InitBeforeInvoke(g, prev_hubs);
      ind.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList, QueueRes<&T>)->DirectEnqRes; override;
    begin
      var ind_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs, own_qr)->
      begin
        var ind := ind_qr.GetResDirect;
        var res_ev: cl_event;
        
        var ec := cl.EnqueueReadBuffer(
          cq, o.Native, Bool.NON_BLOCKING,
          new UIntPtr(int64(ind) * Marshal.SizeOf&<T>), new UIntPtr(Marshal.SizeOf&<T>),
          new IntPtr((own_qr as QueueResPtr<&T>).res),
          evs.count, evs.evs, res_ev
        );
        OpenCLABCInternalException.RaiseIfError(ec);
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          GC.KeepAlive(own_qr);
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override;
    begin
      sb += #10;
      
      sb.Append(#9, tabs);
      sb += 'ind: ';
      ind.ToString(sb, tabs, index, delayed, false);
      
    end;
    
  end;
  
function CLArrayCCQ<T>.ThenGetValue(ind: CommandQueue<integer>): CommandQueue<&T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override := prev_commands.InitBeforeInvoke(g, prev_hubs);
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList, QueueRes<array of &T>)->DirectEnqRes; override;
    begin
      
      Result := (o, cq, evs, own_qr)->
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          res_hnd.Free;
        end);
      end;
      
    end;
    
    private procedure ToStringImpl(sb: StringBuilder; tabs: integer; index: Dictionary<object,integer>; delayed: HashSet<CommandQueueBase>); override := sb += #10;
    
  end;
  
function CLArrayCCQ<T>.ThenGetArray: CommandQueue<array of &T>;
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; prev_hubs: HashSet<IMultiusableCommandQueueHub>); override;
    begin
      prev_commands.InitBeforeInvoke(g, prev_hubs);
      ind.InitBeforeInvoke(g, prev_hubs);
      len.InitBeforeInvoke(g, prev_hubs);
    end;
    
    protected function InvokeParamsImpl(g: CLTaskGlobalData; enq_evs: EnqEvLst): (CLArray<T>, cl_command_queue, EventList, QueueRes<array of &T>)->DirectEnqRes; override;
    begin
      var ind_qr: QueueRes<integer>;
      var len_qr: QueueRes<integer>;
      g.ParallelInvoke(nil, enq_evs.Capacity-1, invoker->
      begin
        ind_qr := invoker.InvokeBranch&<QueueRes<integer>>(ind.InvokeToAny); if ind_qr.IsConst then enq_evs.AddL2(ind_qr.AttachInvokeActions(g)) else enq_evs.AddL1(ind_qr.AttachInvokeActions(g));
        len_qr := invoker.InvokeBranch&<QueueRes<integer>>(len.InvokeToAny); if len_qr.IsConst then enq_evs.AddL2(len_qr.AttachInvokeActions(g)) else enq_evs.AddL1(len_qr.AttachInvokeActions(g));
      end);
      
      Result := (o, cq, evs, own_qr)->
      begin
        var ind := ind_qr.GetResDirect;
        var len := len_qr.GetResDirect;
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
        
        Result := new DirectEnqRes(res_ev, c->
        begin
          res_hnd.Free;
        end);
      end;
      
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
  
function CLArrayCCQ<T>.ThenGetArray(ind, len: CommandQueue<integer>): CommandQueue<array of &T>;
begin
  Result := new CLArrayCommandGetArray<T>(self, ind, len) as CommandQueue<array of &T>;
end;

{$endregion GetArray}

{$endregion Get}

{$endregion Explicit}

{$endregion CLArray}

{$endregion Enqueueable's}

{$region Global subprograms}

{$region CQ}

function CQ<T>(o: T) := CommandQueue&<T>(o);

{$endregion CQ}

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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    protected function ExecFunc(c: Context): T; abstract;
    
    private function MakeNilBody    (prev_d: QueueResComplDelegateData; c: Context; err_handler: CLTaskErrHandler; own_qr: QueueResNil): Action := ()->
    begin
      prev_d.Invoke(c);
      if err_handler.HadError(true) then exit;
      try
        ExecFunc(c);
      except
        on e: Exception do err_handler.AddErr(e);
      end;
    end;
    private function MakeResBody<TR>(prev_d: QueueResComplDelegateData; c: Context; err_handler: CLTaskErrHandler; own_qr: TR): Action; where TR: QueueRes<T>;
    begin
      Result := ()->
      begin
        prev_d.Invoke(c);
        if err_handler.HadError(true) then exit;
        var res: T;
        try
          res := ExecFunc(c);
        except
          on e: Exception do err_handler.AddErr(e);
        end;
        own_qr.SetRes(res);
      end;
    end;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResBaseFactory<TR>; make_body: (QueueResComplDelegateData,Context,CLTaskErrHandler,TR)->Action): TR; where TR: IQueueRes;
    begin
      Result := qr_factory.MakeDelayed(qr->new CLTaskLocalData(
        UserEvent.StartBackgroundWork(l.prev_ev,
          make_body(l.prev_delegate, g.c, g.curr_err_handler, qr),
          g.cl_c{$ifdef EventDebug}, $'body of {self.GetType}'{$endif}
        )
      ));
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil;    override := Invoke(g, l, qr_nil_factory, MakeNilBody);
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := Invoke(g, l, qr_val_factory, MakeResBody&<QueueResVal<T>>);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := Invoke(g, l, qr_ptr_factory, MakeResBody&<QueueResPtr<T>>);
    
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    protected procedure ExecProc(c: Context); abstract;
    private function MakeBody(prev_d: QueueResComplDelegateData; err_handler: CLTaskErrHandler; c: Context): Action := ()->
    begin
      prev_d.Invoke(c);
      if err_handler.HadError(true) then exit;
      try
        ExecProc(c);
      except
        on e: Exception do err_handler.AddErr(e);
      end;
    end;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override :=
    new QueueResNil(new CLTaskLocalData(UserEvent.StartBackgroundWork(
      l.prev_ev, MakeBody(l.prev_delegate, g.curr_err_handler, g.c),
      g.cl_c{$ifdef EventDebug}, $'body of {self.GetType}'{$endif}
    )));
    
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    protected function ExecFunc(c: Context): T; abstract;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      Result := new QueueResNil(l);
      
      var d := QueueResActionUtils.HandlerWrapStrip(g.curr_err_handler, ExecFunc);
      if l.ShouldInstaCallAction then
        d(g.c) else
        Result.AddAction(d);
      
    end;
    
    private [MethodImpl(MethodImplOptions.AggressiveInlining)]
    function Invoke<TR>(g: CLTaskGlobalData; l: CLTaskLocalData; qr_factory: IQueueResFactory<T,TR>): TR; where TR: QueueRes<T>;
    begin
      
      var d := QueueResActionUtils.HandlerWrap(g.curr_err_handler, ExecFunc);
      if l.ShouldInstaCallAction then
        Result := qr_factory.MakeConst(l, d(g.c)) else
      begin
        Result := qr_factory.MakeDelayed(l);
        Result.AddResSetter(d);
      end;
      
    end;
    protected function InvokeToVal(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResVal<T>; override := Invoke(g, l, qr_val_factory);
    protected function InvokeToPtr(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResPtr<T>; override := Invoke(g, l, qr_ptr_factory);
    
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
    
    protected procedure InitBeforeInvoke(g: CLTaskGlobalData; inited_hubs: HashSet<IMultiusableCommandQueueHub>); override := exit;
    
    protected procedure ExecProc(c: Context); abstract;
    
    protected function InvokeToNil(g: CLTaskGlobalData; l: CLTaskLocalData): QueueResNil; override;
    begin
      Result := new QueueResNil(l);
      
      var d := QueueResActionUtils.HandlerWrap(g.curr_err_handler, ExecProc);
      if l.ShouldInstaCallAction then
        d(g.c) else
        Result.AddAction(d);
      
    end;
    
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
  EventDebug.FinallyReport;
  {$endif EventDebug}
  
  {$ifdef QueueDebug}
  QueueDebug.FinallyReport;
  {$endif QueueDebug}
  
  {$ifdef WaitDebug}
  foreach var whd: WaitHandlerDirect in WaitDebug.WaitActions.Keys.OfType&<WaitHandlerDirect> do
    if whd.reserved<>0 then
      raise new OpenCLABCInternalException($'WaitHandler.reserved in finalization was <>0');
  WaitDebug.FinallyReport;
  {$endif WaitDebug}
  
end.