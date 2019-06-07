
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

//ToDo CommandQueueHostFunc при создании из o: T - заполнять сразу res, а функция пусть будет nil
// - сразу минус костыли и + скорость выполнения

//ToDo копирование буферов
//ToDo создание под-буфера

//ToDo issue компилятора:
// - #1880
// - #1881
// - #1947
// - #1952
// - #1958
// - #1981

type
  
  {$region class pre def}
  
  Context = class;
  KernelArg = class;
  Kernel = class;
  ProgramCode = class;
  
  {$endregion class pre def}
  
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
    
    private костыль_поле_o: T; //ToDo #1881
    
    //ToDo #1881
//    public constructor(o: T) :=
//    self.f := ()->o;
    public constructor(o: T);
    begin
      self.костыль_поле_o := o;
      self.f := ()->self.костыль_поле_o;
    end;
    
    private костыль_для_prev_ev: cl_event; //ToDo #1881
    
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
    
    {$region }
    
    {$endregion }
    
    public procedure Finalize; override :=
    if self.val_ptr<>IntPtr.Zero then Marshal.FreeHGlobal(val_ptr);
    
  end;
  
  KernelArg = sealed class
    private memobj: cl_mem;
    private sz: UIntPtr;
    
    {$region constructor's}
    
    private constructor := raise new System.NotSupportedException;
    
    public constructor(size: UIntPtr) :=
    self.sz := size;
    
    public constructor(size: integer) :=
    Create(new UIntPtr(size));
    
    public constructor(size: int64) :=
    Create(new UIntPtr(size));
    
    protected procedure Init(c: Context);
    
    {$endregion constructor's}
    
    {$region property's}
    
    public property Size: UIntPtr read sz;
    public property Size32: UInt32 read sz.ToUInt32;
    public property Size64: UInt64 read sz.ToUInt64;
    
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
    
    {$region Non-Queue IO}
    
    public function GetValue<TRecord>: TRecord;
    where TRecord: record;
    
    public function GetArray<TArray>(params szs: array of integer): TArray;
    where TArray: &Array;
    
    {$endregion Non-Queue IO}
    
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
    
    public static property &Default: Context read _def_cont;
    
    public constructor := Create(DeviceTypeFlags.GPU);
    
    static constructor :=
    try
      
      var ec := cl.GetPlatformIDs(1,@_platform,nil);
      ec.RaiseIfError;
      
      _def_cont := new Context;
      
    except
      on e: Exception do
      begin
        {$reference PresentationFramework.dll}
        System.Windows.MessageBox.Show(e.ToString, 'Не удалось инициализировать OpenCL');
        Halt;
      end;
    end;
    
    public constructor(dt: DeviceTypeFlags);
    begin
      var ec: ErrorCode;
      
      cl.GetDeviceIDs(_platform, dt, 1, @_device, nil).RaiseIfError;
      
      _context := cl.CreateContext(nil, 1, @_device, nil, nil, @ec);
      ec.RaiseIfError;
      
    end;
    
    public function SyncInvoke<T>(q: CommandQueue<T>): T;
    begin
      var ec: ErrorCode;
      var cq := cl.CreateCommandQueue(_context, _device, CommandQueuePropertyFlags.QUEUE_OUT_OF_ORDER_EXEC_MODE_ENABLE, ec);
      ec.RaiseIfError;
      
      foreach var tsk in q.Invoke(self, cq, cl_event.Zero).ToList do tsk.Wait;
      if q.ev<>cl_event.Zero then cl.WaitForEvents(1, @q.ev).RaiseIfError;
      
      cl.ReleaseCommandQueue(cq).RaiseIfError;
      Result := q.res;
    end;
    
    public function BeginInvoke<T>(q: CommandQueue<T>): Task<T>;
//    begin ToDo #1947
//      Result := new Task<T>(()->self.SyncInvoke(q)); //ToDo #1952 //ToDo #1881
//      Result.Start;
//    end;
    
    public procedure Finalize; override :=
    cl.ReleaseContext(_context).RaiseIfError;
    
  end;
  
  {$endregion Context}
  
  {$region ProgramCode}
  
  ProgramCode = sealed class
    private _program: cl_program;
    private cntxt: Context;
    
    private constructor := exit;
    
    public constructor(c: Context; params files: array of string);
    begin
      var ec: ErrorCode;
      self.cntxt := c;
      
      self._program := cl.CreateProgramWithSource(c._context, files.Length, files, files.ConvertAll(s->new UIntPtr(s.Length)), ec);
      ec.RaiseIfError;
      
      cl.BuildProgram(self._program, 1, @c._device, nil,nil,nil).RaiseIfError;
      
    end;
    
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
  
///HostFuncQueue
///Создаёт новую CommandQueueHostFunc
function HFQ<T>(f: ()->T): CommandQueueHostFunc<T>;

implementation

{$region Костыль #1947, #1952}

type
  КостыльType1<T> = auto class //ToDo любая из: #1947, #1952
    
    this_par: Context;
    par1: CommandQueue<T>;
    
    function lambda1: T;
    begin
      Result := this_par.SyncInvoke(par1);
    end;
    
  end;

function Context.BeginInvoke<T>(q: CommandQueue<T>): Task<T>;
begin
  var k := new КостыльType1<T>(self,q);
  Result := Task&<T>.Run(k.lambda1);
end;

{$endregion Костыль#1947, #1952}

{$region CommandQueue}

{$region HostFunc}

function CommandQueueHostFunc<T>.Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task;
begin
  var ec: ErrorCode;
  
  ClearEvent;
  self.ev := cl.CreateUserEvent(c._context, ec);
  ec.RaiseIfError;
  
  костыль_для_prev_ev := prev_ev;
  yield Task.Run(()->
  begin
    if костыль_для_prev_ev<>cl_event.Zero then cl.WaitForEvents(1,@костыль_для_prev_ev).RaiseIfError;
    self.res := f();
    
    cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
  end);
  
end;

static function CommandQueue<T>.operator implicit(o: T): CommandQueue<T> :=
new CommandQueueHostFunc<T>(o);

{$endregion HostFunc}

{$region SyncList}

type
  CommandQueueSyncList<T> = sealed class(CommandQueue<T>)
    public lst := new List<CommandQueueBase>;
    
    private костыль_для_prev_ev: cl_event; //ToDo #1881
    
    protected function Invoke(c: Context; cq: cl_command_queue; prev_ev: cl_event): sequence of Task; override;
    begin
      var ec: ErrorCode;
      
      ClearEvent;
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      var task_lst := new List<Task>(lst.Count);
      foreach var sq in lst do
      begin
        yield sequence sq.Invoke(c, cq, prev_ev);
        prev_ev := sq.ev;
      end;
      
      костыль_для_prev_ev := prev_ev;
      yield Task.Run(()->
      begin
        if костыль_для_prev_ev<>cl_event.Zero then cl.WaitForEvents(1,@костыль_для_prev_ev).RaiseIfError;
        self.res := T(lst[lst.Count-1].GetRes);
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
      end);
      
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
      
      var task_lst := new List<Task>(lst.Count);
      foreach var sq in lst do yield sequence sq.Invoke(c, cq, prev_ev);
      
      var p: Action0 := ()-> //ToDo #1958
      begin
        var evs := lst.Select(q->q.ev).Where(ev->ev<>cl_event.Zero).ToArray;
        if evs.Length<>0 then cl.WaitForEvents(evs.Length,evs).RaiseIfError;
        self.res := T(lst[lst.Count-1].GetRes);
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
      end;
      
      yield Task.Run(p);
      
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
    
    private костыль_для_cq: cl_event; //ToDo #1881
    private костыль_для_ev_lst: List<cl_event>; //ToDo #1880
    
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
      
      костыль_для_cq := cq;
      костыль_для_ev_lst := ev_lst;
      yield Task.Run(()->
      begin
        if костыль_для_ev_lst.Count<>0 then cl.WaitForEvents(костыль_для_ev_lst.Count, костыль_для_ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueWriteBuffer(костыль_для_cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), ptr.res, 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueWriteBuffer(костыль_для_cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), ptr.res, 1,@prev.ev,@buff_ev).RaiseIfError;
        cl.WaitForEvents(1, @buff_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE);
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
    
    private костыль_для_cq: cl_event; //ToDo #1881
    private костыль_для_ev_lst: List<cl_event>; //ToDo #1880
    
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
      
      костыль_для_cq := cq;
      костыль_для_ev_lst := ev_lst;
      yield Task.Run(()->
      begin
        if a.ev<>cl_event.Zero then cl.WaitForEvents(1,@a.ev).RaiseIfError;
        var gchnd := GCHandle.Alloc(a.res, GCHandleType.Pinned);
        
        if костыль_для_ev_lst.Count<>0 then cl.WaitForEvents(костыль_для_ev_lst.Count, костыль_для_ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueWriteBuffer(костыль_для_cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), gchnd.AddrOfPinnedObject, 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueWriteBuffer(костыль_для_cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), gchnd.AddrOfPinnedObject, 1,@prev.ev,@buff_ev).RaiseIfError;
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
    
    private костыль_для_cq: cl_event; //ToDo #1881
    private костыль_для_ev_lst: List<cl_event>; //ToDo #1880
    
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
      
      костыль_для_cq := cq;
      костыль_для_ev_lst := ev_lst;
      yield Task.Run(()->
      begin
        if костыль_для_ev_lst.Count<>0 then cl.WaitForEvents(костыль_для_ev_lst.Count, костыль_для_ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueReadBuffer(костыль_для_cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), ptr.res, 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueReadBuffer(костыль_для_cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), ptr.res, 1,@prev.ev,@buff_ev).RaiseIfError;
        cl.WaitForEvents(1, @buff_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE);
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
    
    private костыль_для_cq: cl_event; //ToDo #1881
    private костыль_для_ev_lst: List<cl_event>; //ToDo #1880
    
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
      
      костыль_для_cq := cq;
      костыль_для_ev_lst := ev_lst;
      yield Task.Run(()->
      begin
        if a.ev<>cl_event.Zero then cl.WaitForEvents(1,@a.ev).RaiseIfError;
        var gchnd := GCHandle.Alloc(a.res, GCHandleType.Pinned);
        
        if костыль_для_ev_lst.Count<>0 then cl.WaitForEvents(костыль_для_ev_lst.Count, костыль_для_ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueReadBuffer(костыль_для_cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), gchnd.AddrOfPinnedObject, 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueReadBuffer(костыль_для_cq, org.memobj, 0, new UIntPtr(offset.res), new UIntPtr(len.res), gchnd.AddrOfPinnedObject, 1,@prev.ev,@buff_ev).RaiseIfError;
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
    
    private костыль_для_cq: cl_event; //ToDo #1881
    private костыль_для_ev_lst: List<cl_event>; //ToDo #1880
    
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
      
      костыль_для_cq := cq;
      костыль_для_ev_lst := ev_lst;
      yield Task.Run(()->
      begin
        if костыль_для_ev_lst.Count<>0 then cl.WaitForEvents(костыль_для_ev_lst.Count, костыль_для_ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueFillBuffer(костыль_для_cq, org.memobj, ptr.res,new UIntPtr(pattern_len.res), new UIntPtr(offset.res),new UIntPtr(len.res), 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueFillBuffer(костыль_для_cq, org.memobj, ptr.res,new UIntPtr(pattern_len.res), new UIntPtr(offset.res),new UIntPtr(len.res), 1,@prev.ev,@buff_ev).RaiseIfError;
        cl.WaitForEvents(1, @buff_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE);
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
    
    private костыль_для_cq: cl_event; //ToDo #1881
    private костыль_для_ev_lst: List<cl_event>; //ToDo #1880
    
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
      
      костыль_для_cq := cq;
      костыль_для_ev_lst := ev_lst;
      yield Task.Run(()->
      begin
        if a.ev<>cl_event.Zero then cl.WaitForEvents(1,@a.ev).RaiseIfError;
        var gchnd := GCHandle.Alloc(a.res, GCHandleType.Pinned);
        var pattern_sz := Marshal.SizeOf(a.res.GetType.GetElementType) * a.res.Length;
        
        if костыль_для_ev_lst.Count<>0 then cl.WaitForEvents(костыль_для_ev_lst.Count, костыль_для_ev_lst.ToArray).RaiseIfError;
        
        var buff_ev: cl_event;
        if prev.ev=cl_event.Zero then
          cl.EnqueueFillBuffer(костыль_для_cq, org.memobj, gchnd.AddrOfPinnedObject,new UIntPtr(pattern_sz), new UIntPtr(offset.res),new UIntPtr(len.res), 0,nil,@buff_ev).RaiseIfError else
          cl.EnqueueFillBuffer(костыль_для_cq, org.memobj, gchnd.AddrOfPinnedObject,new UIntPtr(pattern_sz), new UIntPtr(offset.res),new UIntPtr(len.res), 1,@prev.ev,@buff_ev).RaiseIfError;
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
    
    private костыль_для_c: Context; //ToDo #1881
    private костыль_для_cq: cl_event; //ToDo #1881
    private костыль_для_ev_lst: List<cl_event>; //ToDo #1880
    
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
      
      костыль_для_c := c;
      костыль_для_cq := cq;
      костыль_для_ev_lst := ev_lst;
      yield Task.Run(()->
      begin
        if костыль_для_ev_lst.Count<>0 then cl.WaitForEvents(костыль_для_ev_lst.Count,костыль_для_ev_lst.ToArray);
        
        for var i := 0 to args_q.Length-1 do
        begin
          if args_q[i].res.memobj=cl_mem.Zero then args_q[i].res.Init(костыль_для_c);
          cl.SetKernelArg(org._kernel, i, new UIntPtr(UIntPtr.Size), args_q[i].res.memobj).RaiseIfError;
        end;
        
        var kernel_ev: cl_event;
        cl.EnqueueNDRangeKernel(костыль_для_cq, org._kernel, work_szs.Length, nil,work_szs,nil, 0,nil,@kernel_ev).RaiseIfError; // prev.ev уже в ev_lst, тут проверять не надо
        cl.WaitForEvents(1,@kernel_ev).RaiseIfError;
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE);
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

{$region Misc implementation}

procedure KernelArg.Init(c: Context);
begin
  var ec: ErrorCode;
  self.memobj := cl.CreateBuffer(c._context, MemoryFlags.READ_WRITE, self.sz, IntPtr.Zero, ec);
  ec.RaiseIfError;
end;

function KernelArg.GetValue<TRecord>: TRecord;
begin
  Context.Default.SyncInvoke(
    self.NewQueue
    .ReadValue(Result) as CommandQueue<KernelArg> //ToDo #1981
  );
end;

function KernelArg.GetArray<TArray>(params szs: array of integer): TArray;
begin
  Result := TArray(System.Array.CreateInstance(
    typeof(TArray).GetElementType,
    szs
  ));
  
  Context.Default.SyncInvoke(
    self.NewQueue
    .ReadData(Result) as CommandQueue<KernelArg> //ToDo #1981
  );
  
end;

constructor Kernel.Create(prog: ProgramCode; name: string);
begin
  var ec: ErrorCode;
  
  self._kernel := cl.CreateKernel(prog._program, name, ec);
  ec.RaiseIfError;
  
end;

function HFQ<T>(f: ()->T) :=
new CommandQueueHostFunc<T>(f);

{$endregion Misc implementation}

end.