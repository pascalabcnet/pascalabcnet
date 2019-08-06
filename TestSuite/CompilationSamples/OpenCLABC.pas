﻿
//*****************************************************************************************************\\
// Copyright (©) Cergey Latchenko ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// This code is distributed under the Unlicense
// For details see LICENSE file or this:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\
// Copyright (©) Сергей Латченко ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// Этот код распространяется под Unlicense
// Для деталей смотрите в файл LICENSE или это:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\

///
/// Выскокоуровневая оболочка для модуля OpenCL
/// OpenCL и OpenCLABC можно использовать одновременно
/// Но контактировать они в основном не будут
///
/// Если чего то не хватает - писать как и для модуля OpenCL, сюда:
/// https://github.com/SunSerega/POCGL/issues
///
unit OpenCLABC;

interface

uses OpenCL;
uses System;
uses System.Threading.Tasks;
uses System.Runtime.InteropServices;

//ToDo клонирование очередей
// - для паралельного выполнения из разных потоков

//ToDo если контекст создан из cl_context - не удалять его

//ToDo issue компилятора:
// - #1952
// - #1981

type
  
  {$region misc class def}
  
  Context = class;
  KernelArg = class;
  Kernel = class;
  ProgramCode = class;
  DeviceTypeFlags = OpenCL.DeviceTypeFlags;
  
  {$endregion misc class def}
  
  {$region CommandQueue}
  
  CommandQueueBase = abstract class
    protected ev: cl_event;
    
    protected procedure ClearEvent :=
    if self.ev<>cl_event.Zero then cl.ReleaseEvent(self.ev).RaiseIfError;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; abstract;
    
    protected function GetRes: object; abstract;
    
  end;
  CommandQueue<T> = abstract class(CommandQueueBase)
    protected res: T;
    
    protected function GetRes: object; override := self.res;
    
    public static function operator+<T2>(q1: CommandQueue<T>; q2: CommandQueue<T2>): CommandQueue<T2>;
    
    public static function operator*<T2>(q1: CommandQueue<T>; q2: CommandQueue<T2>): CommandQueue<T2>;
    
    public static function operator implicit(o: T): CommandQueue<T>;
    
  end;
  
  CommandQueueHostFunc<T> = sealed class(CommandQueue<T>)
    private f: ()->T;
    
    public constructor(f: ()->T) :=
    self.f := f;
    
    public constructor(o: T);
    begin
      self.res := o;
      self.f := nil;
    end;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    
    public procedure Finalize; override :=
    ClearEvent;
    
  end;
  
  {$endregion CommandQueue}
  
  {$region KernelArg}
  
  KernelArgCommandQueue = abstract class(CommandQueue<KernelArg>)
    protected org: KernelArg;
    protected prev: KernelArgCommandQueue;
    private val_ptr: IntPtr;
    
    {$region constructor's}
    
    protected constructor(org: KernelArg);
    begin
      self.org := org;
      self.prev := nil;
      self.res := org;
    end;
    
    protected constructor(prev: KernelArgCommandQueue);
    begin
      self.org := prev.org;
      self.prev := prev;
      self.res := org;
    end;
    
    public static function Wrap(arg: KernelArg): KernelArgCommandQueue;
    
    {$endregion constructor's}
    
    {$region Write}
    
    public function WriteData(ptr: CommandQueue<IntPtr>): KernelArgCommandQueue;
    public function WriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): KernelArgCommandQueue;
    
    public function WriteData(ptr: pointer) := WriteData(IntPtr(ptr));
    public function WriteData(ptr: pointer; offset, len: CommandQueue<integer>) := WriteData(IntPtr(ptr), offset, len);
    
    public function WriteData(a: CommandQueue<&Array>): KernelArgCommandQueue;
    public function WriteData(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): KernelArgCommandQueue;
    
    public function WriteData(a: &Array) := WriteData(new CommandQueueHostFunc<&Array>(a));
    public function WriteData(a: &Array; offset, len: CommandQueue<integer>) := WriteData(new CommandQueueHostFunc&<&Array>(a), offset, len);
    
    public function WriteValue<TRecord>(val: TRecord; offset: CommandQueue<integer> := 0): KernelArgCommandQueue; where TRecord: record;
    begin
      var sz := Marshal.SizeOf&<TRecord>;
      var ptr := Marshal.AllocHGlobal(sz);
      var typed_ptr: ^TRecord := pointer(ptr);
      typed_ptr^ := val;
      Result := WriteData(ptr, offset, sz);
      Result.val_ptr := ptr;
    end;
    
    {$endregion Write}
    
    {$region Read}
    
    public function ReadData(ptr: CommandQueue<IntPtr>): KernelArgCommandQueue;
    public function ReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): KernelArgCommandQueue;
    
    public function ReadData(ptr: pointer) := ReadData(IntPtr(ptr));
    public function ReadData(ptr: pointer; offset, len: CommandQueue<integer>) := ReadData(IntPtr(ptr), offset, len);
    
    public function ReadData(a: CommandQueue<&Array>): KernelArgCommandQueue;
    public function ReadData(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): KernelArgCommandQueue;
    
    public function ReadData(a: &Array) := ReadData(new CommandQueueHostFunc<&Array>(a));
    public function ReadData(a: &Array; offset, len: CommandQueue<integer>) := ReadData(new CommandQueueHostFunc&<&Array>(a), offset, len);
    
    public function ReadValue<TRecord>(var val: TRecord; offset: CommandQueue<integer> := 0): KernelArgCommandQueue; where TRecord: record;
    begin
      Result := ReadData(@val, offset, Marshal.SizeOf&<TRecord>);
    end;
    
    {$endregion Read}
    
    {$region Fill}
    
    public function PatternFill(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): KernelArgCommandQueue;
    public function PatternFill(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>): KernelArgCommandQueue;
    
    public function PatternFill(ptr: pointer; pattern_len: CommandQueue<integer>) := PatternFill(IntPtr(ptr), pattern_len);
    public function PatternFill(ptr: pointer; pattern_len, offset, len: CommandQueue<integer>) := PatternFill(IntPtr(ptr), pattern_len, offset, len);
    
    public function PatternFill(a: CommandQueue<&Array>): KernelArgCommandQueue;
    public function PatternFill(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): KernelArgCommandQueue;
    
    public function PatternFill(a: &Array) := PatternFill(new CommandQueueHostFunc<&Array>(a));
    public function PatternFill(a: &Array; offset, len: CommandQueue<integer>) := PatternFill(new CommandQueueHostFunc&<&Array>(a), offset, len);
    
    public function PatternFill<TRecord>(val: TRecord): KernelArgCommandQueue; where TRecord: record;
    begin
      var sz := Marshal.SizeOf&<TRecord>;
      var ptr := Marshal.AllocHGlobal(sz);
      var typed_ptr: ^TRecord := pointer(ptr);
      typed_ptr^ := val;
      Result := PatternFill(ptr, sz);
      Result.val_ptr := ptr;
    end;
    
    public function PatternFill<TRecord>(val: TRecord; offset, len: CommandQueue<integer>): KernelArgCommandQueue; where TRecord: record;
    begin
      var sz := Marshal.SizeOf&<TRecord>;
      var ptr := Marshal.AllocHGlobal(sz);
      var typed_ptr: ^TRecord := pointer(ptr);
      typed_ptr^ := val;
      Result := PatternFill(ptr, sz, offset, len);
      Result.val_ptr := ptr;
    end;
    
    {$endregion Fill}
    
    {$region Copy}
    
    public function CopyFrom(arg: CommandQueue<KernelArg>; from, &to, len: CommandQueue<integer>): KernelArgCommandQueue;
    public function CopyTo  (arg: CommandQueue<KernelArg>; from, &to, len: CommandQueue<integer>): KernelArgCommandQueue;
    
    public function CopyFrom(arg: CommandQueue<KernelArg>): KernelArgCommandQueue;
    public function CopyTo  (arg: CommandQueue<KernelArg>): KernelArgCommandQueue;
    
    {$endregion Copy}
    
    public procedure Finalize; override :=
    if self.val_ptr<>IntPtr.Zero then Marshal.FreeHGlobal(val_ptr);
    
  end;
  
  KernelArg = sealed class
    private memobj: cl_mem;
    private sz: UIntPtr;
    private _parent: KernelArg;
    
    {$region constructor's}
    
    private constructor := raise new System.NotSupportedException;
    public constructor(size: UIntPtr) := self.sz := size;
    public constructor(size: integer) := Create(new UIntPtr(size));
    public constructor(size: int64)   := Create(new UIntPtr(size));
    
    public function SubBuff(offset, size: integer): KernelArg; 
    
    public procedure Init(c: Context);
    
    {$endregion constructor's}
    
    {$region property's}
    
    public property Size: UIntPtr read sz;
    public property Size32: UInt32 read sz.ToUInt32;
    public property Size64: UInt64 read sz.ToUInt64;
    
    public property Parent: KernelArg read _parent;
    
    {$endregion property's}
    
    {$region Queue's}
    
    public function NewQueue :=
    KernelArgCommandQueue.Wrap(self);
    
    public static function ValueQueue<TRecord>(val: TRecord): KernelArgCommandQueue; where TRecord: record;
    begin
      Result := 
        KernelArg.Create(Marshal.SizeOf&<TRecord>)
        .NewQueue.WriteValue(val);
    end;
    
    {$endregion Queue's}
    
    {$region Write}
    
    public function WriteData(ptr: CommandQueue<IntPtr>): KernelArg;
    public function WriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): KernelArg;
    
    public function WriteData(ptr: pointer) := WriteData(IntPtr(ptr));
    public function WriteData(ptr: pointer; offset, len: CommandQueue<integer>) := WriteData(IntPtr(ptr), offset, len);
    
    public function WriteData(a: CommandQueue<&Array>): KernelArg;
    public function WriteData(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): KernelArg;
    
    public function WriteData(a: &Array) := WriteData(new CommandQueueHostFunc<&Array>(a));
    public function WriteData(a: &Array; offset, len: CommandQueue<integer>) := WriteData(new CommandQueueHostFunc<&Array>(a), offset,len);
    
    public function WriteValue<TRecord>(val: TRecord; offset: CommandQueue<integer> := 0): KernelArg; where TRecord: record;
    begin
      Result := WriteData(@val, offset, Marshal.SizeOf&<TRecord>);
    end;
    
    {$endregion Write}
    
    {$region Read}
    
    public function ReadData(ptr: CommandQueue<IntPtr>): KernelArg;
    public function ReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): KernelArg;
    
    public function ReadData(ptr: pointer) := ReadData(IntPtr(ptr));
    public function ReadData(ptr: pointer; offset, len: CommandQueue<integer>) := ReadData(IntPtr(ptr), offset, len);
    
    public function ReadData(a: CommandQueue<&Array>): KernelArg;
    public function ReadData(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): KernelArg;
    
    public function ReadData(a: &Array) := ReadData(new CommandQueueHostFunc<&Array>(a));
    public function ReadData(a: &Array; offset, len: CommandQueue<integer>) := ReadData(new CommandQueueHostFunc<&Array>(a), offset,len);
    
    public function ReadValue<TRecord>(val: TRecord; offset: CommandQueue<integer> := 0): KernelArg; where TRecord: record;
    begin
      Result := ReadData(@val, offset, Marshal.SizeOf&<TRecord>);
    end;
    
    {$endregion Read}
    
    {$region Get}
    
    public function GetData(offset, len: CommandQueue<integer>): IntPtr;
    public function GetData := GetData(0,integer(self.Size32));
    
    public function GetArrayAt<TArray>(offset: CommandQueue<integer>; szs: CommandQueue<array of integer>): TArray; where TArray: &Array;
    public function GetArray<TArray>(szs: CommandQueue<array of integer>): TArray; where TArray: &Array; begin Result := GetArrayAt&<TArray>(0, szs); end;
    
    public function GetArrayAt<TArray>(offset: CommandQueue<integer>; params szs: array of integer): TArray; where TArray: &Array;
    begin Result := GetArrayAt&<TArray>(offset, new CommandQueueHostFunc<array of integer>(szs)); end;
    public function GetArray<TArray>(params szs: array of integer): TArray; where TArray: &Array;
    begin Result := GetArrayAt&<TArray>(0, new CommandQueueHostFunc<array of integer>(szs)); end;
    
    public function GetValueAt<TRecord>(offset: CommandQueue<integer>): TRecord; where TRecord: record;
    public function GetValue<TRecord>: TRecord; where TRecord: record; begin Result := GetValueAt&<TRecord>(0); end;
    
    {$endregion Get}
    
    {$region Fill}
    
    public function PatternFill(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): KernelArg;
    public function PatternFill(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>): KernelArg;
    
    public function PatternFill(ptr: pointer; pattern_len: CommandQueue<integer>) := PatternFill(IntPtr(ptr), pattern_len);
    public function PatternFill(ptr: pointer; pattern_len, offset, len: CommandQueue<integer>) := PatternFill(IntPtr(ptr), pattern_len, offset, len);
    
    public function PatternFill(a: CommandQueue<&Array>): KernelArg;
    public function PatternFill(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): KernelArg;
    
    public function PatternFill(a: &Array) := PatternFill(new CommandQueueHostFunc<&Array>(a));
    public function PatternFill(a: &Array; offset, len: CommandQueue<integer>) := PatternFill(new CommandQueueHostFunc<&Array>(a), offset,len);
    
    public function PatternFill<TRecord>(val: TRecord): KernelArg; where TRecord: record;
    begin
      Result := PatternFill(@val, Marshal.SizeOf&<TRecord>);
    end;
    
    public function PatternFill<TRecord>(val: TRecord; offset, len: CommandQueue<integer>): KernelArg; where TRecord: record;
    begin
      Result := PatternFill(@val, Marshal.SizeOf&<TRecord>, offset,len);
    end;
    
    {$endregion Fill}
    
    {$region Copy}
    
    public function CopyFrom(arg: KernelArg; from, &to, len: CommandQueue<integer>): KernelArg;
    public function CopyTo  (arg: KernelArg; from, &to, len: CommandQueue<integer>): KernelArg;
    
    public function CopyFrom(arg: KernelArg): KernelArg;
    public function CopyTo  (arg: KernelArg): KernelArg;
    
    {$endregion Copy}
    
    public procedure Finalize; override :=
    if self.memobj<>cl_mem.Zero then cl.ReleaseMemObject(self.memobj);
    
  end;
  
  {$endregion KernelArg}
  
  {$region Kernel}
  
  KernelCommandQueue = abstract class(CommandQueue<Kernel>)
    protected org: Kernel;
    protected prev: KernelCommandQueue;
    
    protected constructor(org: Kernel);
    begin
      self.org := org;
      self.prev := nil;
    end;
    
    protected constructor(prev: KernelCommandQueue);
    begin
      self.org := prev.org;
      self.prev := prev;
    end;
    
    public static function Wrap(arg: Kernel): KernelCommandQueue;
    
    
    
    public function Exec(work_szs: array of UIntPtr; params args: array of CommandQueue<KernelArg>): KernelCommandQueue;
    
    public function Exec(work_szs: array of integer; params args: array of CommandQueue<KernelArg>) :=
    Exec(work_szs.ConvertAll(sz->new UIntPtr(sz)), args);
    
    public function Exec(work_sz1: integer; params args: array of CommandQueue<KernelArg>) := Exec(new integer[](work_sz1), args);
    public function Exec(work_sz1, work_sz2: integer; params args: array of CommandQueue<KernelArg>) := Exec(new integer[](work_sz1, work_sz2), args);
    public function Exec(work_sz1, work_sz2, work_sz3: integer; params args: array of CommandQueue<KernelArg>) := Exec(new integer[](work_sz1, work_sz2, work_sz3), args);
    
  end;
  
  Kernel = sealed class
    private _kernel: cl_kernel;
    
    {$region constructor's}
    
    private constructor := raise new System.NotSupportedException;
    
    public constructor(prog: ProgramCode; name: string);
    
    {$endregion constructor's}
    
    {$region Queue's}
    
    public function NewQueue :=
    KernelCommandQueue.Wrap(self);
    
    public function Exec(work_szs: array of UIntPtr; params args: array of CommandQueue<KernelArg>): Kernel;
    
    public function Exec(work_szs: array of integer; params args: array of CommandQueue<KernelArg>) :=
    Exec(work_szs.ConvertAll(sz->new UIntPtr(sz)), args);
    
    public function Exec(work_sz1: integer; params args: array of CommandQueue<KernelArg>) := Exec(new integer[](work_sz1), args);
    public function Exec(work_sz1, work_sz2: integer; params args: array of CommandQueue<KernelArg>) := Exec(new integer[](work_sz1, work_sz2), args);
    public function Exec(work_sz1, work_sz2, work_sz3: integer; params args: array of CommandQueue<KernelArg>) := Exec(new integer[](work_sz1, work_sz2, work_sz3), args);
    
    {$endregion Queue's}
    
    public procedure Finalize; override :=
    cl.ReleaseKernel(self._kernel).RaiseIfError;
    
  end;
  
  {$endregion Kernel}
  
  {$region Context}
  
  Context = sealed class
    private static _platform: cl_platform_id;
    private static _def_cont: Context;
    
    private _device: cl_device_id;
    private _context: cl_context;
    
    public static property &Default: Context read _def_cont write _def_cont;
    
    static constructor :=
    try
      
      var ec := cl.GetPlatformIDs(1,@_platform,nil);
      ec.RaiseIfError;
      
      try
        _def_cont := new Context;
      except
        _def_cont := new Context(DeviceTypeFlags.All); // если нету GPU - попытаться хотя бы для чего то его инициализировать
      end;
      
    except
      on e: Exception do
      begin
        {$reference PresentationFramework.dll}
        System.Windows.MessageBox.Show(e.ToString, 'Не удалось инициализировать OpenCL');
        Halt;
      end;
    end;
    
    public constructor := Create(DeviceTypeFlags.GPU);
    
    public constructor(dt: DeviceTypeFlags);
    begin
      var ec: ErrorCode;
      
      cl.GetDeviceIDs(_platform, dt, 1, @_device, nil).RaiseIfError;
      
      _context := cl.CreateContext(nil, 1, @_device, nil, nil, @ec);
      ec.RaiseIfError;
      
    end;
    
    public constructor(context: cl_context);
    begin
      
      cl.GetContextInfo(context, ContextInfoType.CL_CONTEXT_DEVICES, new UIntPtr(IntPtr.Size), @_device, nil).RaiseIfError;
      
      _context := context;
    end;
    
    public constructor(context: cl_context; device: cl_device_id);
    begin
      _device := device;
      _context := context;
    end;
    
    public function BeginInvoke<T>(q: CommandQueue<T>): Task<T>;
    begin
      var ec: ErrorCode;
      var cq := cl.CreateCommandQueue(_context, _device, CommandQueuePropertyFlags.QUEUE_OUT_OF_ORDER_EXEC_MODE_ENABLE, ec);
      ec.RaiseIfError;
      
      var tasks := q.Invoke(self, cq, cl_event.Zero).ToArray;
      
      var костыль_для_Result: ()->T := ()-> //ToDo #1952
      begin
        Task.WaitAll(tasks);
        if q.ev<>cl_event.Zero then cl.WaitForEvents(1, @q.ev).RaiseIfError;
        
        cl.ReleaseCommandQueue(cq).RaiseIfError;
        Result := q.res;
      end;
      
      Result := Task.Run(костыль_для_Result);
    end;
    
    public function SyncInvoke<T>(q: CommandQueue<T>): T;
    begin
      var tsk := BeginInvoke(q);
      tsk.Wait; //ToDo плавающая ошибка - на этой строчке "System.Threading.Tasks.TaskCanceledException: Отменена задача."
      Result := tsk.Result;
      // может там Task&<T>.Run криво вызывается... посмотреть IL
    end;
    
    public procedure Finalize; override :=
    if _context <> cl_context.Zero then // если было исключение при инициализации
      cl.ReleaseContext(_context).RaiseIfError;
    
  end;
  
  {$endregion Context}
  
  {$region ProgramCode}
  
  ProgramCode = sealed class
    private _program: cl_program;
    private cntxt: Context;
    
    private constructor := exit;
    
    public constructor(c: Context; params files_texts: array of string);
    begin
      var ec: ErrorCode;
      self.cntxt := c;
      
      self._program := cl.CreateProgramWithSource(c._context, files_texts.Length, files_texts, files_texts.ConvertAll(s->new UIntPtr(s.Length)), ec);
      ec.RaiseIfError;
      
      cl.BuildProgram(self._program, 1, @c._device, nil,nil,nil).RaiseIfError;
      
    end;
    
    public constructor(params files_texts: array of string) :=
    Create(Context.Default, files_texts);
    
    public property KernelByName[kname: string]: Kernel read new Kernel(self, kname); default;
    
    public function GetAllKernels: Dictionary<string, Kernel>;
    begin
      
      var names_char_len: UIntPtr;
      cl.GetProgramInfo(_program, ProgramInfoType.NUM_KERNELS, new UIntPtr(UIntPtr.Size), @names_char_len, nil).RaiseIfError;
      
      var names_ptr := Marshal.AllocHGlobal(IntPtr(pointer(names_char_len))+1);
      cl.GetProgramInfo(_program, ProgramInfoType.KERNEL_NAMES, names_char_len, pointer(names_ptr), nil).RaiseIfError;
      
      var names := Marshal.PtrToStringAnsi(names_ptr).Split(';');
      Marshal.FreeHGlobal(names_ptr);
      
      Result := new Dictionary<string, Kernel>(names.Length);
      foreach var kname in names do
        Result[kname] := self[kname];
      
    end;
    
    public function Serialize: array of byte;
    begin
      var bytes_count: UIntPtr;
      cl.GetProgramInfo(_program, ProgramInfoType.BINARY_SIZES, new UIntPtr(UIntPtr.Size), @bytes_count, nil).RaiseIfError;
      
      var bytes_mem := Marshal.AllocHGlobal(IntPtr(pointer(bytes_count)));
      cl.GetProgramInfo(_program, ProgramInfoType.BINARIES, bytes_count, @bytes_mem, nil).RaiseIfError;
      
      Result := new byte[bytes_count.ToUInt64()];
      Marshal.Copy(bytes_mem,Result, 0,Result.Length);
      Marshal.FreeHGlobal(bytes_mem);
      
    end;
    
    public procedure SerializeTo(bw: System.IO.BinaryWriter);
    begin
      var bts := Serialize;
      bw.Write(bts.Length);
      bw.Write(bts);
    end;
    
    public procedure SerializeTo(str: System.IO.Stream) := SerializeTo(new System.IO.BinaryWriter(str));
    
    public static function Deserialize(c: Context; bin: array of byte): ProgramCode;
    begin
      var ec: ErrorCode;
      
      Result := new ProgramCode;
      Result.cntxt := c;
      
      var gchnd := GCHandle.Alloc(bin, GCHandleType.Pinned);
      var bin_mem: ^byte := pointer(gchnd.AddrOfPinnedObject);
      var bin_len := new UIntPtr(bin.Length);
      
      Result._program := cl.CreateProgramWithBinary(c._context,1,@c._device, @bin_len, @bin_mem, nil, @ec);
      ec.RaiseIfError;
      gchnd.Free;
      
    end;
    
    public static function DeserializeFrom(c: Context; br: System.IO.BinaryReader): ProgramCode;
    begin
      var bin_len := br.ReadInt32;
      var bin_arr := br.ReadBytes(bin_len);
      if bin_arr.Length<bin_len then raise new System.IO.EndOfStreamException;
      Result := Deserialize(c, bin_arr);
    end;
    
    public static function DeserializeFrom(c: Context; str: System.IO.Stream) :=
    DeserializeFrom(c, new System.IO.BinaryReader(str));
    
  end;
  
  {$endregion ProgramCode}
  
{$region Сахарные подпрограммы}

///HostFuncQueue
///Создаёт новую CommandQueueHostFunc
function HFQ<T>(f: ()->T): CommandQueueHostFunc<T>;

///HostFuncQueue
///Создаёт новую CommandQueueHostFunc
function HPQ(p: ()->()): CommandQueueHostFunc<object>;

{$endregion Сахарные подпрограммы}

implementation

{$region CommandQueue}

{$region HostFunc}

function CommandQueueHostFunc<T>.Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task;
begin
  var ec: ErrorCode;
  
  if (prev_ev<>cl_event.Zero) or (self.f <> nil) then
  begin
    
    ClearEvent;
    self.ev := cl.CreateUserEvent(c._context, ec);
    ec.RaiseIfError;
    
    yield Task.Run(()->
    begin
      if prev_ev<>cl_event.Zero then cl.WaitForEvents(1,@prev_ev).RaiseIfError;
      if self.f<>nil then self.res := self.f();
      
      cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
    end);
    
  end else
    self.ev := cl_event.Zero;
  
end;

static function CommandQueue<T>.operator implicit(o: T): CommandQueue<T> :=
new CommandQueueHostFunc<T>(o);

{$endregion HostFunc}

{$region SyncList}

type
  CommandQueueSyncList<T> = sealed class(CommandQueue<T>)
    public lst := new List<CommandQueueBase>;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      var ec: ErrorCode;
      
      foreach var sq in lst do
      begin
        yield sequence sq.Invoke(c, cq, prev_ev);
        prev_ev := sq.ev;
      end;
      
      if prev_ev<>cl_event.Zero then
      begin
        ClearEvent;
        self.ev := cl.CreateUserEvent(c._context, ec);
        ec.RaiseIfError;
        
        yield Task.Run(()->
        begin
          cl.WaitForEvents(1,@prev_ev).RaiseIfError;
          self.res := T(lst[lst.Count-1].GetRes);
          cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
        end);
      end else
      begin
        self.ev := cl_event.Zero;
        self.res := T(lst[lst.Count-1].GetRes);
      end;
      
    end;
    
    public procedure Finalize; override :=
    ClearEvent;
    
  end;

static function CommandQueue<T>.operator+<T2>(q1: CommandQueue<T>; q2: CommandQueue<T2>): CommandQueue<T2>;
begin
  var res: CommandQueueSyncList<T2>;
  if q2 is CommandQueueSyncList<T2>(var psl) then
    res := psl else
  begin
    res := new CommandQueueSyncList<T2>;
    res.lst += q2 as CommandQueueBase;
  end;
  
  if q1 is CommandQueueSyncList<T>(var psl) then
    res.lst.InsertRange(0, psl.lst) else
    res.lst.Insert(0, q1);
  
  Result := res;
end;

{$endregion SyncList}

{$region AsyncList}

type
  CommandQueueAsyncList<T> = sealed class(CommandQueue<T>)
    public lst := new List<CommandQueueBase>;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      var ec: ErrorCode;
      
      ClearEvent;
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      foreach var sq in lst do yield sequence sq.Invoke(c, cq, prev_ev);
      
      yield Task.Run(()->
      begin
        var evs := lst.Select(q->q.ev).Where(ev->ev<>cl_event.Zero).ToArray;
        if evs.Length<>0 then cl.WaitForEvents(evs.Length,evs).RaiseIfError;
        self.res := T(lst[lst.Count-1].GetRes);
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
      end);
      
    end;
    
    public procedure Finalize; override :=
    ClearEvent;
    
  end;

static function CommandQueue<T>.operator*<T2>(q1: CommandQueue<T>; q2: CommandQueue<T2>): CommandQueue<T2>;
begin
  var res: CommandQueueAsyncList<T2>;
  if q2 is CommandQueueAsyncList<T2>(var pasl) then
    res := pasl else
  begin
    res := new CommandQueueAsyncList<T2>;
    res.lst += q2 as CommandQueueBase;
  end;
  
  if q1 is CommandQueueAsyncList<T>(var pasl) then
    res.lst.InsertRange(0, pasl.lst) else
    res.lst.Insert(0, q1);
  
  Result := res;
end;

{$endregion AsyncList}

{$region KernelArg}

{$region Base}

type
  KernelArgQueueWrap = sealed class(KernelArgCommandQueue)
    
    public constructor(arg: KernelArg) :=
    inherited Create(arg);
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      self.ev := prev_ev;
      if org.memobj=cl_mem.Zero then org.Init(c);
      Result := System.Linq.Enumerable.Empty&<Task>;
    end;
    
  end;
  
static function KernelArgCommandQueue.Wrap(arg: KernelArg) :=
new KernelArgQueueWrap(arg);

{$endregion Base}

{$region WriteData}

type
  KernelArgQueueWriteData = sealed class(KernelArgCommandQueue)
    public ptr: CommandQueue<IntPtr>;
    public offset, len: CommandQueue<integer>;
    
    public constructor(arg: KernelArgCommandQueue; ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>);
    begin
      inherited Create(arg);
      self.ptr := ptr;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      var ec: ErrorCode;
      
      yield sequence prev  .Invoke(c, cq, prev_ev);
      
      var ev_lst := new List<cl_event>;
      yield sequence ptr   .Invoke(c, cq, cl_event.Zero); if ptr   .ev<>cl_event.Zero then ev_lst += ptr.ev;
      yield sequence offset.Invoke(c, cq, cl_event.Zero); if offset.ev<>cl_event.Zero then ev_lst += offset.ev;
      yield sequence len   .Invoke(c, cq, cl_event.Zero); if len   .ev<>cl_event.Zero then ev_lst += len.ev;
      
      ClearEvent;
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      yield Task.Run(()->
      begin
        if ev_lst.Count<>0 then cl.WaitForEvents(ev_lst.Count, ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueWriteBuffer(cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), ptr.res, 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueWriteBuffer(cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), ptr.res, 1,@prev.ev,@buff_ev).RaiseIfError;
        cl.WaitForEvents(1, @buff_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
      end);
      
    end;
    
    public procedure Finalize; override;
    begin
      inherited Finalize;
      ClearEvent;
    end;
    
  end;
  KernelArgQueueWriteArray = sealed class(KernelArgCommandQueue)
    public a: CommandQueue<&Array>;
    public offset, len: CommandQueue<integer>;
    
    public constructor(arg: KernelArgCommandQueue; a: CommandQueue<&Array>; offset, len: CommandQueue<integer>);
    begin
      inherited Create(arg);
      self.a := a;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      var ev_lst := new List<cl_event>;
      var ec: ErrorCode;
      
      yield sequence prev  .Invoke(c, cq,       prev_ev);
      
      yield sequence a     .Invoke(c, cq, cl_event.Zero);
      yield sequence offset.Invoke(c, cq, cl_event.Zero); if offset.ev<>cl_event.Zero then ev_lst += offset.ev;
      yield sequence len   .Invoke(c, cq, cl_event.Zero); if len   .ev<>cl_event.Zero then ev_lst += len.ev;
      
      ClearEvent;
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      yield Task.Run(()->
      begin
        if a.ev<>cl_event.Zero then cl.WaitForEvents(1,@a.ev).RaiseIfError;
        var gchnd := GCHandle.Alloc(a.res, GCHandleType.Pinned);
        
        if ev_lst.Count<>0 then cl.WaitForEvents(ev_lst.Count, ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueWriteBuffer(cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), gchnd.AddrOfPinnedObject, 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueWriteBuffer(cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), gchnd.AddrOfPinnedObject, 1,@prev.ev,@buff_ev).RaiseIfError;
        cl.WaitForEvents(1,@buff_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
        gchnd.Free;
      end);
      
    end;
    
    public procedure Finalize; override;
    begin
      inherited Finalize;
      ClearEvent;
    end;
    
  end;
  
function KernelArgCommandQueue.WriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
new KernelArgQueueWriteData(self,ptr,offset,len);

function KernelArgCommandQueue.WriteData(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
new KernelArgQueueWriteArray(self,a,offset,len);

function KernelArgCommandQueue.WriteData(ptr: CommandQueue<IntPtr>) := WriteData(ptr, 0,integer(org.sz.ToUInt32));
function KernelArgCommandQueue.WriteData(a: CommandQueue<&Array>) := WriteData(a, 0,integer(org.sz.ToUInt32));

{$endregion WriteData}

{$region ReadData}

type
  KernelArgQueueReadData = sealed class(KernelArgCommandQueue)
    public ptr: CommandQueue<IntPtr>;
    public offset, len: CommandQueue<integer>;
    
    public constructor(arg: KernelArgCommandQueue; ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>);
    begin
      inherited Create(arg);
      self.ptr := ptr;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      var ec: ErrorCode;
      
      yield sequence prev  .Invoke(c, cq, prev_ev);
      
      var ev_lst := new List<cl_event>;
      yield sequence ptr   .Invoke(c, cq, cl_event.Zero); if ptr   .ev<>cl_event.Zero then ev_lst += ptr.ev;
      yield sequence offset.Invoke(c, cq, cl_event.Zero); if offset.ev<>cl_event.Zero then ev_lst += offset.ev;
      yield sequence len   .Invoke(c, cq, cl_event.Zero); if len   .ev<>cl_event.Zero then ev_lst += len.ev;
      
      ClearEvent;
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      yield Task.Run(()->
      begin
        if ev_lst.Count<>0 then cl.WaitForEvents(ev_lst.Count, ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueReadBuffer(cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), ptr.res, 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueReadBuffer(cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), ptr.res, 1,@prev.ev,@buff_ev).RaiseIfError;
        cl.WaitForEvents(1, @buff_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
      end);
      
    end;
    
    public procedure Finalize; override;
    begin
      inherited Finalize;
      ClearEvent;
    end;
    
  end;
  KernelArgQueueReadArray = sealed class(KernelArgCommandQueue)
    public a: CommandQueue<&Array>;
    public offset, len: CommandQueue<integer>;
    
    public constructor(arg: KernelArgCommandQueue; a: CommandQueue<&Array>; offset, len: CommandQueue<integer>);
    begin
      inherited Create(arg);
      self.a := a;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      var ev_lst := new List<cl_event>;
      var ec: ErrorCode;
      
      yield sequence prev  .Invoke(c, cq,       prev_ev);
      
      yield sequence a     .Invoke(c, cq, cl_event.Zero);
      yield sequence offset.Invoke(c, cq, cl_event.Zero); if offset.ev<>cl_event.Zero then ev_lst += offset.ev;
      yield sequence len   .Invoke(c, cq, cl_event.Zero); if len   .ev<>cl_event.Zero then ev_lst += len.ev;
      
      ClearEvent;
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      yield Task.Run(()->
      begin
        if a.ev<>cl_event.Zero then cl.WaitForEvents(1,@a.ev).RaiseIfError;
        var gchnd := GCHandle.Alloc(a.res, GCHandleType.Pinned);
        
        if ev_lst.Count<>0 then cl.WaitForEvents(ev_lst.Count, ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueReadBuffer(cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), gchnd.AddrOfPinnedObject, 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueReadBuffer(cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), gchnd.AddrOfPinnedObject, 1,@prev.ev,@buff_ev).RaiseIfError;
        cl.WaitForEvents(1,@buff_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
        gchnd.Free;
      end);
      
    end;
    
    public procedure Finalize; override;
    begin
      inherited Finalize;
      ClearEvent;
    end;
    
  end;
  
function KernelArgCommandQueue.ReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
new KernelArgQueueReadData(self,ptr,offset,len);

function KernelArgCommandQueue.ReadData(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
new KernelArgQueueReadArray(self,a,offset,len);

function KernelArgCommandQueue.ReadData(ptr: CommandQueue<IntPtr>) := ReadData(ptr, 0,integer(org.sz.ToUInt32));
function KernelArgCommandQueue.ReadData(a: CommandQueue<&Array>) := ReadData(a, 0,integer(org.sz.ToUInt32));

{$endregion ReadData}

{$region PatternFill}

type
  KernelArgQueueDataFill = sealed class(KernelArgCommandQueue)
    public ptr: CommandQueue<IntPtr>;
    public pattern_len, offset, len: CommandQueue<integer>;
    
    public constructor(arg: KernelArgCommandQueue; ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>);
    begin
      inherited Create(arg);
      self.ptr := ptr;
      self.pattern_len := pattern_len;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      var ec: ErrorCode;
      
      yield sequence prev  .Invoke(c, cq, prev_ev);
      
      var ev_lst := new List<cl_event>;
      yield sequence ptr         .Invoke(c, cq, cl_event.Zero); if ptr         .ev<>cl_event.Zero then ev_lst += ptr.ev;
      yield sequence pattern_len .Invoke(c, cq, cl_event.Zero); if pattern_len .ev<>cl_event.Zero then ev_lst += pattern_len.ev;
      yield sequence offset      .Invoke(c, cq, cl_event.Zero); if offset      .ev<>cl_event.Zero then ev_lst += offset.ev;
      yield sequence len         .Invoke(c, cq, cl_event.Zero); if len         .ev<>cl_event.Zero then ev_lst += len.ev;
      
      ClearEvent;
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      yield Task.Run(()->
      begin
        if ev_lst.Count<>0 then cl.WaitForEvents(ev_lst.Count, ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueFillBuffer(cq, org.memobj, ptr.res,new UIntPtr(pattern_len.res), new UIntPtr(offset.res),new UIntPtr(len.res), 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueFillBuffer(cq, org.memobj, ptr.res,new UIntPtr(pattern_len.res), new UIntPtr(offset.res),new UIntPtr(len.res), 1,@prev.ev,@buff_ev).RaiseIfError;
        cl.WaitForEvents(1, @buff_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
      end);
      
    end;
    
    public procedure Finalize; override;
    begin
      inherited Finalize;
      ClearEvent;
    end;
    
  end;
  KernelArgQueueArrayFill = sealed class(KernelArgCommandQueue)
    public a: CommandQueue<&Array>;
    public offset, len: CommandQueue<integer>;
    
    public constructor(arg: KernelArgCommandQueue; a: CommandQueue<&Array>; offset, len: CommandQueue<integer>);
    begin
      inherited Create(arg);
      self.a := a;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      var ev_lst := new List<cl_event>;
      var ec: ErrorCode;
      
      yield sequence prev  .Invoke(c, cq,       prev_ev);
      
      yield sequence a     .Invoke(c, cq, cl_event.Zero);
      yield sequence offset.Invoke(c, cq, cl_event.Zero); if offset.ev<>cl_event.Zero then ev_lst += offset.ev;
      yield sequence len   .Invoke(c, cq, cl_event.Zero); if len   .ev<>cl_event.Zero then ev_lst += len.ev;
      
      ClearEvent;
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      yield Task.Run(()->
      begin
        if a.ev<>cl_event.Zero then cl.WaitForEvents(1,@a.ev).RaiseIfError;
        var gchnd := GCHandle.Alloc(a.res, GCHandleType.Pinned);
        var pattern_sz := Marshal.SizeOf(a.res.GetType.GetElementType) * a.res.Length;
        
        if ev_lst.Count<>0 then cl.WaitForEvents(ev_lst.Count, ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueFillBuffer(cq, org.memobj, gchnd.AddrOfPinnedObject,new UIntPtr(pattern_sz), new UIntPtr(offset.res),new UIntPtr(len.res), 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueFillBuffer(cq, org.memobj, gchnd.AddrOfPinnedObject,new UIntPtr(pattern_sz), new UIntPtr(offset.res),new UIntPtr(len.res), 1,@prev.ev,@buff_ev).RaiseIfError;
        cl.WaitForEvents(1,@buff_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
        gchnd.Free;
      end);
      
    end;
    
    public procedure Finalize; override;
    begin
      inherited Finalize;
      ClearEvent;
    end;
    
  end;
  
function KernelArgCommandQueue.PatternFill(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>) :=
new KernelArgQueueDataFill(self, ptr,pattern_len, offset,len);

function KernelArgCommandQueue.PatternFill(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
new KernelArgQueueArrayFill(self, a, offset,len);

function KernelArgCommandQueue.PatternFill(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>) := PatternFill(ptr,pattern_len, 0,integer(org.sz.ToUInt32));
function KernelArgCommandQueue.PatternFill(a: CommandQueue<&Array>) := PatternFill(a, 0,integer(org.sz.ToUInt32));

{$endregion PatternFill}

{$region Copy}

type
  KernelArgQueueCopy = sealed class(KernelArgCommandQueue)
    public f_arg, t_arg: CommandQueue<KernelArg>;
    public f_pos, t_pos, len: CommandQueue<integer>;
    
    public constructor(otp: KernelArgCommandQueue; f_arg, t_arg: CommandQueue<KernelArg>; f_pos, t_pos, len: CommandQueue<integer>);
    begin
      inherited Create(otp);
      self.f_arg := f_arg;
      self.t_arg := t_arg;
      self.f_pos := f_pos;
      self.t_pos := t_pos;
      self.len := len;
    end;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      var ec: ErrorCode;
      
      yield sequence prev  .Invoke(c, cq, prev_ev);
      
      var ev_lst := new List<cl_event>;
      yield sequence f_arg.Invoke(c, cq, cl_event.Zero); if f_arg.ev<>cl_event.Zero then ev_lst += f_arg.ev;
      yield sequence t_arg.Invoke(c, cq, cl_event.Zero); if t_arg.ev<>cl_event.Zero then ev_lst += t_arg.ev;
      yield sequence f_pos.Invoke(c, cq, cl_event.Zero); if f_pos.ev<>cl_event.Zero then ev_lst += f_pos.ev;
      yield sequence t_pos.Invoke(c, cq, cl_event.Zero); if t_pos.ev<>cl_event.Zero then ev_lst += t_pos.ev;
      yield sequence len  .Invoke(c, cq, cl_event.Zero); if len  .ev<>cl_event.Zero then ev_lst += len.ev;
      
      ClearEvent;
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      yield Task.Run(()->
      begin
        if ev_lst.Count<>0 then cl.WaitForEvents(ev_lst.Count, ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueCopyBuffer(cq, f_arg.res.memobj,t_arg.res.memobj, new UIntPtr(f_pos.res),new UIntPtr(t_pos.res), new UIntPtr(len.res), 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueCopyBuffer(cq, f_arg.res.memobj,t_arg.res.memobj, new UIntPtr(f_pos.res),new UIntPtr(t_pos.res), new UIntPtr(len.res), 1,@prev.ev,@buff_ev).RaiseIfError;
        cl.WaitForEvents(1, @buff_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
      end);
      
    end;
    
    public procedure Finalize; override;
    begin
      inherited Finalize;
      ClearEvent;
    end;
    
  end;

function KernelArgCommandQueue.CopyFrom(arg: CommandQueue<KernelArg>; from, &to, len: CommandQueue<integer>) :=
new KernelArgQueueCopy(self, arg,self as CommandQueue<KernelArg>, from,&to, len); //ToDo #1981

function KernelArgCommandQueue.CopyTo(arg: CommandQueue<KernelArg>; from, &to, len: CommandQueue<integer>) :=
new KernelArgQueueCopy(self, self as CommandQueue<KernelArg>,arg, &to,from, len); //ToDo #1981

function KernelArgCommandQueue.CopyFrom(arg: CommandQueue<KernelArg>) := CopyFrom(arg, 0,0, integer(self.org.Size32));
function KernelArgCommandQueue.CopyTo(arg: CommandQueue<KernelArg>) := CopyTo(arg, 0,0, integer(self.org.Size32));

{$endregion Copy}

{$endregion KernelArg}

{$region Kernel}

{$region Base}

type
  KernelQueueWrap = sealed class(KernelCommandQueue)
    
    public constructor(arg: Kernel);
    begin
      inherited Create(arg);
      self.res := org;
    end;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      self.ev := prev_ev;
      Result := System.Linq.Enumerable.Empty&<Task>;
    end;
    
  end;
  
static function KernelCommandQueue.Wrap(arg: Kernel) :=
new KernelQueueWrap(arg);

{$endregion Base}

{$region Exec}

type
  KernelQueueExec = sealed class(KernelCommandQueue)
    public args_q: array of CommandQueue<KernelArg>;
    public work_szs: array of UIntPtr;
    
    public constructor(k: KernelCommandQueue; work_szs: array of UIntPtr; args: array of CommandQueue<KernelArg>);
    begin
      inherited Create(k);
      self.work_szs := work_szs;
      self.args_q := args;
    end;
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      var ev_lst := new List<cl_event>;
      var ec: ErrorCode;
      
      yield sequence prev.Invoke(c, cq, prev_ev);
      if prev.ev<>cl_event.Zero then
        ev_lst += prev.ev;
      
      foreach var arg_q in args_q do
      begin
        yield sequence arg_q.Invoke(c, cq, cl_event.Zero);
        if arg_q.ev<>cl_event.Zero then
          ev_lst += arg_q.ev;
      end;
      
      ClearEvent;
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      yield Task.Run(()->
      begin
        if ev_lst.Count<>0 then cl.WaitForEvents(ev_lst.Count,ev_lst.ToArray);
        
        for var i := 0 to args_q.Length-1 do
        begin
          if args_q[i].res.memobj=cl_mem.Zero then args_q[i].res.Init(c);
          cl.SetKernelArg(org._kernel, i, new UIntPtr(UIntPtr.Size), args_q[i].res.memobj).RaiseIfError;
        end;
        
        var kernel_ev: cl_event;
        cl.EnqueueNDRangeKernel(cq, org._kernel, work_szs.Length, nil,work_szs,nil, 0,nil,@kernel_ev).RaiseIfError; // prev.ev уже в ev_lst, тут проверять не надо
        cl.WaitForEvents(1,@kernel_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
      end);
      
    end;
    
    public procedure Finalize; override :=
    ClearEvent;
    
  end;
  
function KernelCommandQueue.Exec(work_szs: array of UIntPtr; params args: array of CommandQueue<KernelArg>) :=
new KernelQueueExec(self, work_szs, args);

{$endregion Exec}

{$endregion Kernel}

{$endregion CommandQueue}

{$region Сахарные подпрограммы}

function HFQ<T>(f: ()->T) :=
new CommandQueueHostFunc<T>(f);

function HPQ(p: ()->()) :=
HFQ&<object>(
  ()->
  begin
    p();
    Result := nil;
  end
);

{$endregion Сахарные подпрограммы}

{$region KernelArg}

{$region constructor's}

procedure KernelArg.Init(c: Context);
begin
  var ec: ErrorCode;
  if self.memobj<>cl_mem.Zero then cl.ReleaseMemObject(self.memobj);
  self.memobj := cl.CreateBuffer(c._context, MemoryFlags.READ_WRITE, self.sz, IntPtr.Zero, ec);
  ec.RaiseIfError;
end;

function KernelArg.SubBuff(offset, size: integer): KernelArg;
begin
  if self.memobj=cl_mem.Zero then Init(Context.Default);
  
  Result := new KernelArg(size);
  Result._parent := self;
  
  var ec: ErrorCode;
  var reg := new cl_buffer_region(
    new UIntPtr( offset ),
    new UIntPtr( size )
  );
  Result.memobj := cl.CreateSubBuffer(self.memobj, MemoryFlags.READ_WRITE, BufferCreateType.REGION, pointer(@reg), ec);
  ec.RaiseIfError;
  
end;

{$endregion constructor's}

{$region Write}

function KernelArg.WriteData(ptr: CommandQueue<IntPtr>) :=
Context.Default.SyncInvoke(self.NewQueue.WriteData(ptr) as CommandQueue<KernelArg>);

function KernelArg.WriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(self.NewQueue.WriteData(ptr, offset,len) as CommandQueue<KernelArg>);

function KernelArg.WriteData(a: CommandQueue<&Array>) :=
Context.Default.SyncInvoke(self.NewQueue.WriteData(a) as CommandQueue<KernelArg>);

function KernelArg.WriteData(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(self.NewQueue.WriteData(a, offset,len) as CommandQueue<KernelArg>);

{$endregion Write}

{$region Read}

function KernelArg.ReadData(ptr: CommandQueue<IntPtr>) :=
Context.Default.SyncInvoke(self.NewQueue.ReadData(ptr) as CommandQueue<KernelArg>);

function KernelArg.ReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(self.NewQueue.ReadData(ptr, offset,len) as CommandQueue<KernelArg>);

function KernelArg.ReadData(a: CommandQueue<&Array>) :=
Context.Default.SyncInvoke(self.NewQueue.ReadData(a) as CommandQueue<KernelArg>);

function KernelArg.ReadData(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(self.NewQueue.ReadData(a, offset,len) as CommandQueue<KernelArg>);

{$endregion Read}

{$region Get}

function KernelArg.GetData(offset, len: CommandQueue<integer>): IntPtr;
begin
  var len_val := Context.Default.SyncInvoke(len);
  Result := Marshal.AllocHGlobal(len_val);
  Context.Default.SyncInvoke(
    self.NewQueue.ReadData(Result, offset,len_val) as CommandQueue<KernelArg>
  );
end;

function KernelArg.GetArrayAt<TArray>(offset: CommandQueue<integer>; szs: CommandQueue<array of integer>): TArray;
begin
  var el_t := typeof(TArray).GetElementType;
  
  var szs_val: array of integer := Context.Default.SyncInvoke(szs);
  Result := TArray(System.Array.CreateInstance(
    el_t,
    szs_val
  ));
  
  var res_len := Result.Length;
  
  Context.Default.SyncInvoke(
    self.NewQueue
    .ReadData(Result, offset, Marshal.SizeOf(el_t) * res_len) as CommandQueue<KernelArg> //ToDo #1981
  );
  
end;

function KernelArg.GetValueAt<TRecord>(offset: CommandQueue<integer>): TRecord;
begin
  Context.Default.SyncInvoke(
    self.NewQueue
    .ReadValue(Result, offset) as CommandQueue<KernelArg> //ToDo #1981
  );
end;

{$endregion Get}

{$region Fill}

function KernelArg.PatternFill(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(self.NewQueue.PatternFill(ptr,pattern_len) as CommandQueue<KernelArg>);

function KernelArg.PatternFill(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(self.NewQueue.PatternFill(ptr,pattern_len, offset,len) as CommandQueue<KernelArg>);

function KernelArg.PatternFill(a: CommandQueue<&Array>) :=
Context.Default.SyncInvoke(self.NewQueue.PatternFill(a) as CommandQueue<KernelArg>);

function KernelArg.PatternFill(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(self.NewQueue.PatternFill(a, offset,len) as CommandQueue<KernelArg>);

{$endregion Fill}

{$region Copy}

function KernelArg.CopyFrom(arg: KernelArg; from, &to, len: CommandQueue<integer>) := Context.Default.SyncInvoke(self.NewQueue.CopyFrom(arg, from,&to, len) as CommandQueue<KernelArg>);
function KernelArg.CopyTo  (arg: KernelArg; from, &to, len: CommandQueue<integer>) := Context.Default.SyncInvoke(self.NewQueue.CopyTo  (arg, from,&to, len) as CommandQueue<KernelArg>);

function KernelArg.CopyFrom(arg: KernelArg) := Context.Default.SyncInvoke(self.NewQueue.CopyFrom(arg) as CommandQueue<KernelArg>);
function KernelArg.CopyTo  (arg: KernelArg) := Context.Default.SyncInvoke(self.NewQueue.CopyTo  (arg) as CommandQueue<KernelArg>);

{$endregion Copy}

{$endregion KernelArg}

{$region Kernel}

constructor Kernel.Create(prog: ProgramCode; name: string);
begin
  var ec: ErrorCode;
  
  self._kernel := cl.CreateKernel(prog._program, name, ec);
  ec.RaiseIfError;
  
end;

function Kernel.Exec(work_szs: array of UIntPtr; params args: array of CommandQueue<KernelArg>) :=
Context.Default.SyncInvoke(self.NewQueue.Exec(work_szs, args) as CommandQueue<Kernel>);

{$endregion Kernel}

end.