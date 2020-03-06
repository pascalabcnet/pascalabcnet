
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
///Справка данного модуля находится в начале его исходника
///   Исходники можно открыть Ctrl-кликом на любом имени из модуля
///
unit OpenCLABC;

interface

uses OpenCL;

uses System;
uses System.Threading;
uses System.Threading.Tasks;
uses System.Runtime.InteropServices;
uses System.Runtime.CompilerServices;

{$region ToDo}

//===================================
// Обязательно сделать до следующего пула:

//ToDo Тесты всех фич модуля
//ToDo И в каждом сделать по несколько выполнений, на случай плавающий ошибок

//===================================
// Запланированное:

//ToDo Раздел справки про оптимизацию
// - почему 1 очередь быстрее 2 её кусков

//ToDo Очередь-обработчик ошибок
// - сделать легко, надо только вставить свой промежуточный CLTaskBase
// - единственное - для Wait очереди надо хранить так же оригинальный таск

//ToDo Если в предыдущей очереди исключение - остановить выполнение
// - это не критично, но иначе будет выводить кучу лишних ошибок

//ToDo cl.SetKernelArg из нескольких потоков одновременно - предусмотреть

//ToDo Синхронные (с припиской Fast) варианты всего работающего по принципу HostQueue
//ToDo и асинхронные умнее запускать - помнить значение, указывающее можно ли выполнить их синхронно

//ToDo исправить десериализацию ProgramCode

//ToDo когда partial классы начнут нормально себя вести - использовать их чтоб переместить все "__*" классы в implementation

//ToDo CommmandQueueBase.ToString для дебага
// - так же дублирующий protected метод (tabs: integer; index: Dictionary<CommandQueueBase,integer>)

//ToDo CommandQueue.Cycle(integer)
//ToDo CommandQueue.Cycle // бесконечность циклов
//ToDo CommandQueue.CycleWhile(***->boolean)
// - Возможность передать свой обработчик ошибок как Exception->Exception

//ToDo В продолжение Cycle: Однако всё ещё остаётся проблема - как сделать ветвление?
// - И если уже делать - стоит сделать и метод CQ.ThenIf(res->boolean; if_true, if_false: CQ)
// - Ивенты должны выполнится, иначе останутся GCHandle которые не освободили
// - Наверное надо что то типа cl.SetUserEventStatus(ErrorCode.MyCustomQueueCancelCode)
// - Протестировать как это работает с юзер-ивентами и с маркерами из таких юзер-ивентов
// - Вроде бы маркер и cl.WaitForEvents имеют свой код ошибки, когда 1 из ивентов неправильный

//ToDo Read/Write для массивов - надо бы иметь возможность указывать отступ в массиве

//ToDo Типы Device и Platform
//ToDo А связь с OpenCL.pas сделать всему (и буферам и кёрнелам), но более человеческую

//ToDo Сделать методы BufferCommandQueue.AddGet
// - они особенные, потому что возвращают не BufferCommandQueue, а каждый свою очередь
// - полезно, потому что SyncInvoke такой очереди будет возвращать полученное значение

//ToDo Интегрировать профайлинг очередей

//===================================
// Сделать когда-нибуть:

//ToDo У всего, у чего есть .Finalize - проверить чтобы было и .Dispose, если надо
// - и добавить в справку, про то что этот объект можно удалять
// - из .Dispose можно блокировать .Finalize

//ToDo Пройтись по всем функциям OpenCL, посмотреть функционал каких не доступен из OpenCLABC
// - у Kernel.Exec несколько параметров не используются. Стоит использовать

//===================================

//ToDo issue компилятора:
// - #1981
// - #2118
// - #2120
// - #2145
// - #2150
// - #2173

{$endregion ToDo}

{$region Debug}

{ $define DebugMode}
{$ifdef DebugMode}

{$endif DebugMode}

{$endregion Debug}

type
  
  {$region pre def}
  
  CommandQueueBase = class;
  CommandQueue<T> = class;
  
  CLTaskBase = class;
  CLTask<T> = class;
  
  Buffer = class;
  Kernel = class;
  
  Context = class;
  
  ProgramCode = class;
  
  DeviceType = OpenCL.DeviceType;
  
  {$endregion pre def}
  
  {$region hidden utils}
  
  __NativUtils = static class
    
    static function CopyToUnm<TRecord>(a: TRecord): IntPtr; where TRecord: record;
    begin
      Result := Marshal.AllocHGlobal(Marshal.SizeOf&<TRecord>);
      var res: ^TRecord := pointer(Result);
      res^ := a;
    end;
    
    static function AsPtr<T>(p: pointer): ^T := p;
    
    static function GCHndAlloc(o: object) :=
    CopyToUnm(GCHandle.Alloc(o));
    
    static procedure GCHndFree(gc_hnd_ptr: IntPtr);
    begin
      AsPtr&<GCHandle>(pointer(gc_hnd_ptr))^.Free;
      Marshal.FreeHGlobal(gc_hnd_ptr);
    end;
    
  end;
  
  __EventList = sealed class
    private evs: array of cl_event;
    private count := 0;
    
    public constructor := exit;
    
    public constructor(count: integer) :=
    self.evs := count=0 ? nil : new cl_event[count];
    
    public property Item[i: integer]: cl_event read evs[i]; default;
    
    public static function operator implicit(ev: cl_event): __EventList;
    begin
      if ev=cl_event.Zero then
        Result := new __EventList else
      begin
        Result := new __EventList(1);
        Result += ev;
      end;
    end;
    
    public constructor(params evs: array of cl_event);
    begin
      self.evs := evs;
      self.count := evs.Length;
    end;
    
    public static procedure operator+=(l: __EventList; ev: cl_event);
    begin
      l.evs[l.count] := ev;
      l.count += 1;
    end;
    
    public static procedure operator+=(l: __EventList; ev: __EventList);
    begin
      for var i := 0 to ev.count-1 do
        l += ev[i];
    end;
    
    public static function operator+(l1,l2: __EventList): __EventList;
    begin
      Result := new __EventList(l1.count+l2.count);
      Result += l1;
      Result += l2;
    end;
    
    public static function Combine(params evs: array of __EventList): __EventList;
    begin
      Result := new __EventList(evs.Sum(ev->ev.count));
      foreach var ev in evs do
        Result += ev;
    end;
    
    public procedure Retain :=
    for var i := 0 to count-1 do
      cl.RetainEvent(evs[i]).RaiseIfError;
    
    public procedure Release :=
    for var i := 0 to count-1 do
      cl.ReleaseEvent(evs[i]).RaiseIfError;
    
    public static procedure AttachCallback(ev: cl_event; cb: EventCallback);
    public static procedure AttachCallback(ev: cl_event; cb: EventCallback; tsk: CLTaskBase);
    
    ///cb должен иметь глобальный try и вызывать "state.RaiseIfError" и "__NativUtils.GCHndFree(data)",
    ///А "cl.ReleaseEvent" если и вызывать - то только на результате вызова AttachCallback
    public function AttachCallback(cb: EventCallback; c: Context; var cq: cl_command_queue): cl_event;
    
  end;
  
  __IQueueRes = interface
    
    function ResBase: object;
    function ResFBase: ()->object;
    function EvBase: __EventList;
    
    //ToDo #2150
    //TODo #2173
//    function ResFBase<T2>(f2: object->T2): ()->T2;
    function IsF: boolean;
    
    function GetBase: object;
    
    function WaitAndGetBase: object;
    
    function AttachCallbackBase(cb: EventCallback; c: Context; var cq: cl_command_queue): __IQueueRes;
    
  end;
  __QueueRes<T> = record(__IQueueRes)
    res: T;
    res_f: ()->T;
    ev: __EventList;
    
    function ResBase: object := self.res;
    function ResFBase: ()->object := ()->self.res_f();
    function EvBase: __EventList := self.ev;
    
//    function __IQueueRes.ResFBase<T2>(f2: object->T2): ()->T2 := ()->f2(self.res_f());
    function IsF: boolean := self.res_f<>nil;
    
    function Get: T := res_f=nil ? res : res_f();
    function GetBase: object := Get();
    
    function WaitAndGet: T;
    begin
      
      if ev.count<>0 then
      begin
        cl.WaitForEvents(ev.count,ev.evs).RaiseIfError;
        ev.Release;
      end;
      
      Result := res_f=nil ? res : res_f();
    end;
    function WaitAndGetBase: object := WaitAndGet();
    
    function LazyQuickTransform<T2>(f: T->T2): __QueueRes<T2>;
    begin
      Result.ev := self.ev;
      
      if self.res_f<>nil then
      begin
        var f0 := self.res_f;
        Result.res_f := ()->f(f0());
      end else
      
      if self.ev.count=0 then
        Result.res := f(self.res) else
        
      begin
        var r0 := self.res;
        Result.res_f := ()->f(r0);
      end;
      
    end;
    
    function AttachCallback(cb: EventCallback; c: Context; var cq: cl_command_queue): __QueueRes<T>;
    begin
      Result.res    := self.res;
      Result.res_f  := self.res_f;
      Result.ev     := self.ev.AttachCallback(cb, c, cq);
    end;
    function AttachCallbackBase(cb: EventCallback; c: Context; var cq: cl_command_queue): __IQueueRes := AttachCallback(cb, c, cq);
    
  end;
  
  __MWEventContainer = sealed class // MW = Multi Wait
    curr_evs := new Queue<cl_event>;
    cached: integer;
  end;
  
  {$endregion hidden utils}
  
  {$region CommandQueue's}
  
  {$region CommandQueue}
  
  CommandQueueBase = abstract class
    
    {$region Queue converters}
    
    {$region ConstQueue}
    
    public static function operator implicit(o: object): CommandQueueBase;
    
    {$endregion ConstQueue}
    
    {$region Cast}
    
    public function Cast<T>: CommandQueue<T>;
    
    {$endregion Cast}
    
    {$region ThenConvert}
    
    //ToDo #2118
//    ///Создаёт очередь, которая выполнит данную
//    ///А затем выполнит на CPU функцию f, используя результат данной очереди
//    public function ThenConvert<T>(f: object->T): CommandQueue<T>;
//    ///Создаёт очередь, которая выполнит данную
//    ///А затем выполнит на CPU функцию f, используя результат данной очереди и контекст на котором её выполнили
//    public function ThenConvert<T>(f: (object,Context)->T): CommandQueue<T>;
    
    {$endregion ThenConvert}
    
    {$region [A]SyncQueue}
    
    public static function operator+(q1, q2: CommandQueueBase): CommandQueueBase;
    public static function operator+<T>(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T>;
    public static procedure operator+=(var q1: CommandQueueBase; q2: CommandQueueBase) := q1 := q1+q2;
    
    public static function operator*(q1, q2: CommandQueueBase): CommandQueueBase;
    public static function operator*<T>(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T>;
    public static procedure operator*=(var q1: CommandQueueBase; q2: CommandQueueBase) := q1 := q1*q2;
    
    {$endregion [A]SyncQueue}
    
    {$region Mutiusable}
    
    //ToDo #2120
//    ///Создаёт массив из n очередей, каждая из которых возвращает результат данной очереди
//    ///Каждую полученную очередь можно использовать одновременно с другими, но только в общей очереди
//    public function Multiusable(n: integer): array of CommandQueueBase;
//    
//    ///Создаёт функцию, создающую очередь, которая возвращает результат данной очереди
//    ///Каждую очередь, созданную полученной функцией, можно использовать одновременно с другими, но только в общей очереди
//    public function Multiusable: ()->CommandQueueBase;
    
    {$endregion Mutiusable}
    
    {$region ThenWait}
    
    public function ThenWaitFor(q: CommandQueueBase): CommandQueueBase := ThenWaitForAll(q);
    
    public function ThenWaitForAll(qs: sequence of CommandQueueBase): CommandQueueBase := CreateWaitWrapperBase(qs, true);
    public function ThenWaitForAll(params qs: array of CommandQueueBase) := ThenWaitForAll(qs.AsEnumerable);
    
    public function ThenWaitForAny(qs: sequence of CommandQueueBase): CommandQueueBase := CreateWaitWrapperBase(qs, false);
    public function ThenWaitForAny(params qs: array of CommandQueueBase) := ThenWaitForAny(qs.AsEnumerable);
    
    {$endregion ThenWait}
    
    {$endregion Queue converters}
    
    {$region Invoke}
    
    protected function InvokeBase(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __IQueueRes; abstract;
    
    protected function InvokeNewQBase(tsk: CLTaskBase; c: Context): __IQueueRes;
    
    {$endregion Invoke}
    
    {$region Utils}
    
    {$region MW}
    
    private waiters_c := 0;
    protected function IsWaitable := waiters_c<>0;
    protected procedure MakeWaitable := lock self do waiters_c += 1;
    protected procedure UnMakeWaitable := lock self do waiters_c -= 1;
    
    /// добавляет tsk в качестве ключа для всех ожидаемых очередей
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); abstract;
    
    private mw_evs := new Dictionary<CLTaskBase, __MWEventContainer>;
    protected procedure RegisterWaiterTask(tsk: CLTaskBase);
    
    protected function GetMWEvent(tsk: CLTaskBase; c: Context): cl_event;
    protected procedure SignalMWEvent(tsk: CLTaskBase);
    
    {$endregion MW}
    
    {$region Misc}
    
    protected static function CreateUserEvent(c: Context): cl_event;
    
    protected function CreateWaitWrapperBase(qs: sequence of CommandQueueBase; all: boolean): CommandQueueBase; abstract;
    
    {$endregion Misc}
    
    {$endregion Utils}
    
  end;
  CommandQueue<T> = abstract class(CommandQueueBase)
    
    {$region Queue converters}
    
    {$region ConstQueue}
    
    public static function operator implicit(o: T): CommandQueue<T>;
    
    {$endregion ConstQueue}
    
    {$region ThenConvert}
    
    public function ThenConvert<T2>(f: T->T2): CommandQueue<T2>;
    public function ThenConvert<T2>(f: (T,Context)->T2): CommandQueue<T2>;
    
    {$endregion ThenConvert}
    
    {$region [A]SyncQueue}
    
    public static procedure operator+=(var q1: CommandQueue<T>; q2: CommandQueue<T>) := q1 := q1+q2;
    public static procedure operator*=(var q1: CommandQueue<T>; q2: CommandQueue<T>) := q1 := q1*q2;
    
    {$endregion [A]SyncQueue}
    
    {$region Mutiusable}
    
    public function Multiusable: ()->CommandQueue<T>;
    
    {$endregion Mutiusable}
    
    {$region ThenWait}
    
    public function ThenWaitFor(q: CommandQueueBase): CommandQueue<T> := ThenWaitForAll(q);
    
    public function ThenWaitForAll(qs: sequence of CommandQueueBase): CommandQueue<T> := CreateWaitWrapper(qs, true);
    public function ThenWaitForAll(params qs: array of CommandQueueBase) := ThenWaitForAll(qs.AsEnumerable);
    
    public function ThenWaitForAny(qs: sequence of CommandQueueBase): CommandQueue<T> := CreateWaitWrapper(qs, false);
    public function ThenWaitForAny(params qs: array of CommandQueueBase) := ThenWaitForAny(qs.AsEnumerable);
    
    protected function CreateWaitWrapper(qs: sequence of CommandQueueBase; all: boolean): CommandQueue<T>;
    protected function CreateWaitWrapperBase(qs: sequence of CommandQueueBase; all: boolean): CommandQueueBase; override :=
    CreateWaitWrapper(qs, all);
    
    {$endregion ThenWait}
    
    {$endregion Queue converters}
    
    {$region Invoke}
    
    protected function Invoke(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; abstract;
    protected function InvokeBase(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __IQueueRes; override :=
    Invoke(tsk, c, cq, prev_ev);
    
    protected function InvokeNewQ(tsk: CLTaskBase; c: Context): __QueueRes<T>;
    
    {$endregion Invoke}
    
  end;
  
  {$endregion CommandQueue}
  
  {$region CLTask}
  
  CLTaskBase = abstract class
    protected err_lst := new List<Exception>;                 // Список исключений, вызванных при выполнении
    protected mu_res := new Dictionary<object, __IQueueRes>;  // Результаты хабов от .Multiusable
    
    protected wh := new ManualResetEvent(false);
    protected wh_lock := new object;
    
    {$region Utils}
    
    protected procedure AddErr(e: Exception) :=
    lock err_lst do err_lst += e;
    
    protected procedure AddErr(err: ErrorCode) :=
    if err.IS_ERROR then AddErr(new OpenCLException(err.ToString));
    
    protected procedure AddErr(st: CommandExecutionStatus) :=
    if st.IS_ERROR then AddErr(new OpenCLException(ErrorCode.Create(st).ToString));
    
    /// Возвращает True если очередь уже завершилась
    protected function AddEventHandler<T>(var ev: T; cb: T): boolean; where T: Delegate;
    begin
      lock wh_lock do
        Result := wh.WaitOne(0);
      if not Result then
        ev := Delegate.Combine(ev, cb) as T;
    end;
    
    {$endregion Utils}
    
    {$region def}
    
    protected function QBase: CommandQueueBase; abstract;
    protected function QResBase: object; abstract;
    
    {$endregion def}
    
    {$region events}
    
    private EvDone: Action<CLTaskBase>;
    private EvComplete: Action<CLTaskBase, object>;
    private EvError: Action<CLTaskBase, array of Exception>;
    
    public procedure WhenDone(cb: Action<CLTaskBase>) :=
    if AddEventHandler(EvDone, cb) then cb(self);
    
    public procedure WhenComplete(cb: Action<CLTaskBase, object>) :=
    if AddEventHandler(EvComplete, cb) then cb(self, QResBase);
    
    public procedure WhenError(cb: Action<CLTaskBase, array of Exception>) :=
    if AddEventHandler(EvError, cb) then lock err_lst do cb(self, err_lst.ToArray);
    
    {$endregion events}
    
    public property OrgQueue: CommandQueueBase read QBase;
    
    public procedure Wait;
    begin
      wh.WaitOne;
      lock err_lst do if err_lst.Count<>0 then raise new AggregateException(
        Format(
          '%task:errors%',
          err_lst
        ),
        err_lst
      );
    end;
    
    public function WaitRes: object;
    begin
      Wait;
      Result := QResBase;
    end;
    
  end;
  
  CLTask<T> = sealed class(CLTaskBase)
    private q: CommandQueue<T>;
    private q_res: T;
    
    {$region def}
    
    protected function QBase: CommandQueueBase; override := q;
    protected function QResBase: object; override := q_res;
    
    {$endregion def}
    
    {$region event's}
    
    private EvDone: Action<CLTask<T>>;
    private EvComplete: Action<CLTask<T>, T>;
    private EvError: Action<CLTask<T>, array of Exception>;
    
    public procedure WhenDone(cb: Action<CLTask<T>>); reintroduce :=
    if AddEventHandler(EvDone, cb) then cb(self);
    
    public procedure WhenComplete(cb: Action<CLTask<T>, T>); reintroduce :=
    if AddEventHandler(EvComplete, cb) then cb(self, q_res);
    
    public procedure WhenError(cb: Action<CLTask<T>, array of Exception>); reintroduce :=
    if AddEventHandler(EvError, cb) then lock err_lst do cb(self, err_lst.ToArray);
    
    {$endregion event's}
    
    protected constructor(q: CommandQueue<T>; c: Context);
    begin
      self.q := q;
      
      q.RegisterWaitables(self, new HashSet<object>);
      
      var cq := cl_command_queue.Zero;
      var res := q.Invoke(self, c, cq, new __EventList);
      
      // mu выполняют лишний .Retain, чтоб ивент не удалился пока очередь ещё запускается
      foreach var qr in mu_res.Values do
        qr.EvBase.Release;
      mu_res := nil;
      
      var ev := res.ev;
      
      if ev.count=0 then
      begin
        if cq<>cl_command_queue.Zero then raise new NotImplementedException; // не должно произойти никогда
        OnQDone( res.Get() );
      end else
        cl.ReleaseEvent(
          ev.AttachCallback((ev,st,data)->
          begin
            self.AddErr( st );
            
            if cq<>cl_command_queue.Zero then
              Task.Run(()->self.AddErr( cl.ReleaseCommandQueue(cq) ));
            
            OnQDone( res.Get() );
            
            __NativUtils.GCHndFree(data);
          end, c, cq)
        ).RaiseIfError;
      
    end;
    
    private procedure OnQDone(res: T) :=
    try
      self.q_res := res;
      
      var lb_EvDone:      Action<CLTaskBase>;
      var lb_EvComplete:  Action<CLTaskBase, object>;
      var lb_EvError:     Action<CLTaskBase, array of Exception>;
      
      var l_EvDone:       Action<CLTask<T>>;
      var l_EvComplete:   Action<CLTask<T>, T>;
      var l_EvError:      Action<CLTask<T>, array of Exception>;
      
      lock wh_lock do
      begin
        
        lb_EvDone     := inherited EvDone;
        lb_EvComplete := inherited EvComplete;
        lb_EvError    := inherited EvError;
        
        l_EvDone      := EvDone;
        l_EvComplete  := EvComplete;
        l_EvError     := EvError;
        
        wh.Set;
      end;
      
      try
        if lb_EvDone<>nil then lb_EvDone(self);
        if  l_EvDone<>nil then  l_EvDone(self);
      except
        on e: Exception do AddErr(e);
      end;
      
      try
        if lb_EvComplete<>nil then lb_EvComplete(self, res);
        if  l_EvComplete<>nil then  l_EvComplete(self, res);
      except
        on e: Exception do AddErr(e);
      end;
      
      if (lb_EvError<>nil) or (l_EvError<>nil) then
      begin
        var err_arr: array of Exception;
        lock err_lst do err_arr := err_lst.ToArray;
        
        if lb_EvError<>nil then lb_EvError(self, err_arr);
        if  l_EvError<>nil then  l_EvError(self, err_arr);
        
      end;
      
    except
      on e: Exception do
      begin
        AddErr(e);
        wh.Set;
      end;
    end;
    
    public property OrgQueue: CommandQueue<T> read q;
    
    public function WaitRes: T; reintroduce;
    begin
      Wait;
      Result := self.q_res;
    end;
    
  end;
  
  __CLTaskResLess = sealed class(CLTaskBase)
    private q: CommandQueueBase;
    private q_res: object;
    
    protected function QBase: CommandQueueBase; override := q;
    protected function QResBase: object; override := q_res;
    
    protected constructor(q: CommandQueueBase; c: Context);
    begin
      self.q := q;
      
      q.RegisterWaitables(self, new HashSet<object>);
      
      var cq := cl_command_queue.Zero;
      var res := q.InvokeBase(self, c, cq, new __EventList);
      
      // mu выполняют лишний .Retain, чтоб ивент не удалился пока очередь ещё запускается
      foreach var qr in mu_res.Values do
        qr.EvBase.Release;
      mu_res := nil;
      
      var ev := res.EvBase;
      
      if ev.count=0 then
      begin
        if cq<>cl_command_queue.Zero then raise new NotImplementedException; // не должно произойти никогда
        OnQDone( res.GetBase() );
      end else
        cl.ReleaseEvent(
          ev.AttachCallback((ev,st,data)->
          begin
            self.AddErr( st );
            
            if cq<>cl_command_queue.Zero then
              Task.Run(()->self.AddErr( cl.ReleaseCommandQueue(cq) ));
            
            OnQDone( res.GetBase() );
            
            __NativUtils.GCHndFree(data);
          end, c, cq)
        ).RaiseIfError;
      
    end;
    
    private procedure OnQDone(res: object) :=
    try
      self.q_res := res;
      
      var lb_EvDone:      Action<CLTaskBase>;
      var lb_EvComplete:  Action<CLTaskBase, object>;
      var lb_EvError:     Action<CLTaskBase, array of Exception>;
      
      lock wh_lock do
      begin
        
        lb_EvDone     := inherited EvDone;
        lb_EvComplete := inherited EvComplete;
        lb_EvError    := inherited EvError;
        
        wh.Set;
      end;
      
      try
        if lb_EvDone<>nil then lb_EvDone(self);
      except
        on e: Exception do AddErr(e);
      end;
      
      try
        if lb_EvComplete<>nil then lb_EvComplete(self, res);
      except
        on e: Exception do AddErr(e);
      end;
      
      if lb_EvError<>nil then
      begin
        var err_arr: array of Exception;
        lock err_lst do err_arr := err_lst.ToArray;
        
        lb_EvError(self, err_arr);
        
      end;
      
    except
      on e: Exception do
      begin
        AddErr(e);
        wh.Set;
      end;
    end;
    
  end;
  
  {$endregion CLTask}
  
  {$region ContainerQueue}
  
  // очередь, выполняющая незначитальный объём своей работы, но запускающая под-очереди
  __ContainerQueue<T> = abstract class(CommandQueue<T>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; abstract;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; override;
    begin
      Result := InvokeSubQs(tsk, c, cq, prev_ev);
      
      if self.IsWaitable then
        if Result.ev.count=0 then
          self.SignalMWEvent(tsk) else
          Result := Result.AttachCallback((ev,st,data)->
          begin
            tsk.AddErr( st );
            self.SignalMWEvent(tsk);
            __NativUtils.GCHndFree(data);
          end, c, cq);
      
    end;
    
  end;
  
  {$endregion ContainerQueue}
  
  {$region HostQueue}
  
  // очередь, выполняющая какую то работу на CPU, всегда в отдельном потоке
  __HostQueue<TInp,TRes> = abstract class(CommandQueue<TRes>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<TInp>; abstract;
    
    protected function ExecFunc(o: TInp; c: Context): TRes; abstract;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<TRes>; override;
    begin
      var prev_res := InvokeSubQs(tsk, c, cq, prev_ev);
      
      var uev := CreateUserEvent(c);
      Result.ev := uev;
      
      var res: TRes;
      Result.res_f := ()->res;
      
      Thread.Create(()->
      begin
        
        try
          res := ExecFunc(prev_res.WaitAndGet(), c);
          if self.IsWaitable then self.SignalMWEvent(tsk);
        except
          on e: Exception do tsk.AddErr(e);
        end;
        
        tsk.AddErr( cl.SetUserEventStatus(uev, CommandExecutionStatus.COMPLETE) );
      end).Start;
      
    end;
    
  end;
  
  {$endregion HostQueue}
  
  {$region ConstQueue}
  
  IConstQueue = interface
    function GetConstVal: Object;
  end;
  ConstQueue<T> = sealed class(CommandQueue<T>, IConstQueue)
    private res: T;
    
    public constructor(o: T) :=
    self.res := o;
    
    public function GetConstVal: object := self.res;
    public property Val: T read self.res;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; override;
    begin
      
      if self.IsWaitable then
      begin
        
        if prev_ev.count=0 then
          SignalMWEvent(tsk) else
          prev_ev := prev_ev.AttachCallback((ev,st,data)->
          begin
            tsk.AddErr( st );
            self.SignalMWEvent(tsk);
            __NativUtils.GCHndFree(data);
          end, c, cq);
        
      end;
      
      Result.ev := prev_ev;
      Result.res := self.res;
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override := exit;
    
  end;
  
  {$endregion ConstQueue}
  
  {$endregion CommandQueue's}
  
  {$region GPUCommand}
  
  __GPUCommand<T> = abstract class
    
    protected function InvokeObj(o: T; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __EventList; abstract;
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __EventList; abstract;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); abstract;
    
  end;
  
  __GPUCommandContainer<T> = class;
  __GPUCommandContainerBody<T> = abstract class
    private cc: __GPUCommandContainer<T>;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; abstract;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); abstract;
    
  end;
  
  __GPUCommandContainer<T> = abstract class(__ContainerQueue<T>)
    protected body: __GPUCommandContainerBody<T>;
    protected commands := new List<__GPUCommand<T>>;
    
    {$region def}
    
    protected procedure OnEarlyInit(c: Context); virtual := exit;
    
    {$endregion def}
    
    {$region Common}
    
    protected constructor(o: T);
    protected constructor(q: CommandQueue<T>);
    
    protected procedure InternalAddQueue(q: CommandQueueBase);
    
    protected procedure InternalAddProc(p: T->());
    protected procedure InternalAddProc(p: (T,Context)->());
    
    protected procedure InternalAddWaitAll(qs: sequence of CommandQueueBase);
    protected procedure InternalAddWaitAny(qs: sequence of CommandQueueBase);
    
    {$endregion Common}
    
    {$region sub implementation}
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; override :=
    body.Invoke(tsk, c, cq, prev_ev);
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      body.RegisterWaitables(tsk, prev_hubs);
      foreach var comm in commands do comm.RegisterWaitables(tsk, prev_hubs);
    end;
    
    {$endregion sub implementation}
    
    {$region reintroduce методы}
    
    //ToDo #2145
    public function Equals(obj: object): boolean; reintroduce := inherited Equals(obj);
    public function ToString: string; reintroduce := inherited ToString();
    public function GetType: System.Type; reintroduce := inherited GetType();
    public function GetHashCode: integer; reintroduce := inherited GetHashCode();
    
    {$endregion reintroduce методы}
    
  end;
  
  {$endregion GPUCommand}
  
  {$region Buffer}
  
  BufferCommandQueue = sealed class(__GPUCommandContainer<Buffer>)
    
    {$region constructor's}
    
    public constructor(b: Buffer) := inherited;
    public constructor(q: CommandQueue<Buffer>);
    
    {$endregion constructor's}
    
    {$region Utils}
    
    protected function AddCommand(comm: __GPUCommand<Buffer>): BufferCommandQueue;
    begin
      self.commands += comm;
      Result := self;
    end;
    
    protected function GetSizeQ: CommandQueue<integer>;
    
    {$endregion Utils}
    
    {$region Write}
    
    public function AddWriteData(ptr: CommandQueue<IntPtr>): BufferCommandQueue := AddWriteData(ptr, 0,GetSizeQ);
    public function AddWriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    public function AddWriteData(ptr: pointer) := AddWriteData(IntPtr(ptr));
    public function AddWriteData(ptr: pointer; offset, len: CommandQueue<integer>) := AddWriteData(IntPtr(ptr), offset, len);
    
    
    public function AddWriteArray(a: CommandQueue<&Array>): BufferCommandQueue := AddWriteArray(a, 0,GetSizeQ);
    public function AddWriteArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    public function AddWriteArray(a: &Array) := AddWriteArray(CommandQueue&<&Array>(a));
    public function AddWriteArray(a: &Array; offset, len: CommandQueue<integer>) := AddWriteArray(CommandQueue&<&Array>(a), offset, len);
    
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function AddWriteValue<TRecord>(val: TRecord; offset: CommandQueue<integer> := 0): BufferCommandQueue; where TRecord: record;
    
    public function AddWriteValue<TRecord>(val: CommandQueue<TRecord>; offset: CommandQueue<integer> := 0): BufferCommandQueue; where TRecord: record;
    
    {$endregion Write}
    
    {$region Read}
    
    public function AddReadData(ptr: CommandQueue<IntPtr>): BufferCommandQueue := AddReadData(ptr, 0,GetSizeQ);
    public function AddReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    public function AddReadData(ptr: pointer) := AddReadData(IntPtr(ptr));
    public function AddReadData(ptr: pointer; offset, len: CommandQueue<integer>) := AddReadData(IntPtr(ptr), offset, len);
    
    public function AddReadArray(a: CommandQueue<&Array>): BufferCommandQueue := AddReadArray(a, 0,GetSizeQ);
    public function AddReadArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    public function AddReadArray(a: &Array) := AddReadArray(CommandQueue&<&Array>(a));
    public function AddReadArray(a: &Array; offset, len: CommandQueue<integer>) := AddReadArray(CommandQueue&<&Array>(a), offset, len);
    
    public function AddReadValue<TRecord>(var val: TRecord; offset: CommandQueue<integer> := 0): BufferCommandQueue; where TRecord: record;
    begin
      Result := AddReadData(@val, offset, Marshal.SizeOf&<TRecord>);
    end;
    
    {$endregion Read}
    
    {$region Fill}
    
    public function AddFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): BufferCommandQueue := AddFillData(ptr,pattern_len, 0,GetSizeQ);
    public function AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    public function AddFillData(ptr: pointer; pattern_len: CommandQueue<integer>) := AddFillData(IntPtr(ptr), pattern_len);
    public function AddFillData(ptr: pointer; pattern_len, offset, len: CommandQueue<integer>) := AddFillData(IntPtr(ptr), pattern_len, offset, len);
    
    public function AddFillArray(a: CommandQueue<&Array>): BufferCommandQueue := AddFillArray(a, 0,GetSizeQ);
    public function AddFillArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    public function AddFillArray(a: &Array) := AddFillArray(CommandQueue&<&Array>(a));
    public function AddFillArray(a: &Array; offset, len: CommandQueue<integer>) := AddFillArray(CommandQueue&<&Array>(a), offset, len);
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function AddFillValue<TRecord>(val: TRecord): BufferCommandQueue; where TRecord: record;
    begin Result := AddFillValue(val, 0,GetSizeQ); end;
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function AddFillValue<TRecord>(val: TRecord; offset, len: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    public function AddFillValue<TRecord>(val: CommandQueue<TRecord>): BufferCommandQueue; where TRecord: record;
    begin Result := AddFillValue(val, 0,GetSizeQ); end;
    public function AddFillValue<TRecord>(val: CommandQueue<TRecord>; offset, len: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    {$endregion Fill}
    
    {$region Copy}
    
    public function AddCopyFrom(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>): BufferCommandQueue;
    public function AddCopyTo  (b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>): BufferCommandQueue;
    
    public function AddCopyFrom(b: CommandQueue<Buffer>) := AddCopyFrom(b, 0,0, GetSizeQ);
    public function AddCopyTo  (b: CommandQueue<Buffer>) := AddCopyTo  (b, 0,0, GetSizeQ);
    
    {$endregion Copy}
    
    {$region Non-command add's}
    
    public function AddQueue(q: CommandQueueBase): BufferCommandQueue;
    begin
      InternalAddQueue(q);
      Result := self;
    end;
    
    public function AddProc(p: (Buffer,Context)->()): BufferCommandQueue;
    begin
      InternalAddProc(p);
      Result := self;
    end;
    public function AddProc(p: Buffer->()): BufferCommandQueue;
    begin
      InternalAddProc(p);
      Result := self;
    end;
    
    public function AddWaitAll(qs: sequence of CommandQueueBase): BufferCommandQueue;
    begin
      InternalAddWaitAll(qs);
      Result := self;
    end;
    public function AddWaitAny(qs: sequence of CommandQueueBase): BufferCommandQueue;
    begin
      InternalAddWaitAny(qs);
      Result := self;
    end;
    public function AddWaitAll(params qs: array of CommandQueueBase) := AddWaitAll(qs.AsEnumerable);
    public function AddWaitAny(params qs: array of CommandQueueBase) := AddWaitAny(qs.AsEnumerable);
    public function AddWait(q: CommandQueueBase) := AddWaitAll(q);
    
    {$endregion Non-command add's}
    
  end;
  
  Buffer = sealed class(IDisposable)
    private memobj: cl_mem;
    private sz: UIntPtr;
    private _parent: Buffer;
    
    {$region constructor's}
    
    private constructor := raise new System.NotSupportedException;
    
    public constructor(size: UIntPtr) := self.sz := size;
    public constructor(size: integer) := Create(new UIntPtr(size));
    public constructor(size: int64)   := Create(new UIntPtr(size));
    
    public constructor(size: UIntPtr; c: Context);
    begin
      Create(size);
      Init(c);
    end;
    public constructor(size: integer; c: Context) := Create(new UIntPtr(size), c);
    public constructor(size: int64; c: Context)   := Create(new UIntPtr(size), c);
    
    public function SubBuff(offset, size: integer): Buffer; 
    
    public procedure Init(c: Context);
    
    public procedure Dispose :=
    if self.memobj<>cl_mem.Zero then
    begin
      GC.RemoveMemoryPressure(Size64);
      cl.ReleaseMemObject(memobj).RaiseIfError;
      memobj := cl_mem.Zero;
    end;
    
    protected procedure Finalize; override :=
    self.Dispose;
    
    {$endregion constructor's}
    
    {$region property's}
    
    public property Size: UIntPtr read sz;
    public property Size32: UInt32 read sz.ToUInt32;
    public property Size64: UInt64 read sz.ToUInt64;
    
    public property Parent: Buffer read _parent;
    
    {$endregion property's}
    
    {$region Queue's}
    
    public function NewQueue :=
    new BufferCommandQueue(self);
    
    {$endregion Queue's}
    
    {$region Write}
    
    public function WriteData(ptr: CommandQueue<IntPtr>): Buffer;
    public function WriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): Buffer;
    
    public function WriteData(ptr: pointer) := WriteData(IntPtr(ptr));
    public function WriteData(ptr: pointer; offset, len: CommandQueue<integer>) := WriteData(IntPtr(ptr), offset, len);
    
    
    public function WriteArray(a: CommandQueue<&Array>): Buffer;
    public function WriteArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): Buffer;
    
    public function WriteArray(a: &Array) := WriteArray(CommandQueue&<&Array>(a));
    public function WriteArray(a: &Array; offset, len: CommandQueue<integer>) := WriteArray(CommandQueue&<&Array>(a), offset, len);
    
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function WriteValue<TRecord>(val: TRecord; offset: CommandQueue<integer> := 0): Buffer; where TRecord: record;
    begin Result := WriteData(@val, offset, Marshal.SizeOf&<TRecord>); end;
    
    public function WriteValue<TRecord>(val: CommandQueue<TRecord>; offset: CommandQueue<integer> := 0): Buffer; where TRecord: record;
    
    {$endregion Write}
    
    {$region Read}
    
    public function ReadData(ptr: CommandQueue<IntPtr>): Buffer;
    public function ReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): Buffer;
    
    public function ReadData(ptr: pointer) := ReadData(IntPtr(ptr));
    public function ReadData(ptr: pointer; offset, len: CommandQueue<integer>) := ReadData(IntPtr(ptr), offset, len);
    
    public function ReadArray(a: CommandQueue<&Array>): Buffer;
    public function ReadArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): Buffer;
    
    public function ReadArray(a: &Array) := ReadArray(CommandQueue&<&Array>(a));
    public function ReadArray(a: &Array; offset, len: CommandQueue<integer>) := ReadArray(CommandQueue&<&Array>(a), offset, len);
    
    public function ReadValue<TRecord>(var val: TRecord; offset: CommandQueue<integer> := 0): Buffer; where TRecord: record;
    begin
      Result := ReadData(@val, offset, Marshal.SizeOf&<TRecord>);
    end;
    
    {$endregion Read}
    
    {$region Fill}
    
    public function FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): Buffer;
    public function FillData(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>): Buffer;
    
    public function FillData(ptr: pointer; pattern_len: CommandQueue<integer>) := FillData(IntPtr(ptr), pattern_len);
    public function FillData(ptr: pointer; pattern_len, offset, len: CommandQueue<integer>) := FillData(IntPtr(ptr), pattern_len, offset, len);
    
    public function FillArray(a: CommandQueue<&Array>): Buffer;
    public function FillArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): Buffer;
    
    public function FillArray(a: &Array) := FillArray(CommandQueue&<&Array>(a));
    public function FillArray(a: &Array; offset, len: CommandQueue<integer>) := FillArray(CommandQueue&<&Array>(a), offset, len);
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function FillValue<TRecord>(val: TRecord): Buffer; where TRecord: record;
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function FillValue<TRecord>(val: TRecord; offset, len: CommandQueue<integer>): Buffer; where TRecord: record;
    
    public function FillValue<TRecord>(val: CommandQueue<TRecord>): Buffer; where TRecord: record;
    public function FillValue<TRecord>(val: CommandQueue<TRecord>; offset, len: CommandQueue<integer>): Buffer; where TRecord: record;
    
    {$endregion Fill}
    
    {$region Copy}
    
    public function CopyFrom(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>): Buffer;
    public function CopyTo  (b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>): Buffer;
    
    public function CopyFrom(b: CommandQueue<Buffer>): Buffer;
    public function CopyTo  (b: CommandQueue<Buffer>): Buffer;
    
    {$endregion Copy}
    
    {$region Get}
    
    public function GetData(offset, len: CommandQueue<integer>): IntPtr;
    public function GetData := GetData(0,integer(self.Size32));
    
    
    
    public function GetArrayAt<TArray>(offset: CommandQueue<integer>; szs: CommandQueue<array of integer>): TArray; where TArray: &Array;
    public function GetArray<TArray>(szs: CommandQueue<array of integer>): TArray; where TArray: &Array;
    begin Result := GetArrayAt&<TArray>(0, szs); end;
    
    public function GetArrayAt<TArray>(offset: CommandQueue<integer>; params szs: array of CommandQueue<integer>): TArray; where TArray: &Array;
    public function GetArray<TArray>(params szs: array of integer): TArray; where TArray: &Array;
    begin Result := GetArrayAt&<TArray>(0, CommandQueue&<array of integer>(szs)); end;
    
    
    public function GetArray1At<TRecord>(offset, length: CommandQueue<integer>): array of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array of TRecord>(offset, length); end;
    public function GetArray1<TRecord>(length: CommandQueue<integer>): array of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array of TRecord>(0,length); end;
    
    public function GetArray1<TRecord>: array of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array of TRecord>(0, integer(sz.ToUInt32) div Marshal.SizeOf&<TRecord>); end;
    
    
    public function GetArray2At<TRecord>(offset, length1, length2: CommandQueue<integer>): array[,] of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array[,] of TRecord>(offset, length1, length2); end;
    public function GetArray2<TRecord>(length1, length2: CommandQueue<integer>): array[,] of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array[,] of TRecord>(0, length1, length2); end;
    
    
    public function GetArray3At<TRecord>(offset, length1, length2, length3: CommandQueue<integer>): array[,,] of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array[,,] of TRecord>(offset, length1, length2, length3); end;
    public function GetArray3<TRecord>(length1, length2, length3: CommandQueue<integer>): array[,,] of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array[,,] of TRecord>(0, length1, length2, length3); end;
    
    
    
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function GetValueAt<TRecord>(offset: CommandQueue<integer>): TRecord; where TRecord: record;
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function GetValue<TRecord>: TRecord; where TRecord: record;
    begin Result := GetValueAt&<TRecord>(0); end;
    
    {$endregion Get}
    
  end;
  
  {$endregion Buffer}
  
  {$region Kernel}
  
  KernelCommandQueue = sealed class(__GPUCommandContainer<Kernel>)
    
    {$region constructor's}
    
    public constructor(k: Kernel) := inherited;
    public constructor(q: CommandQueue<Kernel>) := inherited;
    
    {$endregion constructor's}
    
    {$region Utils}
    
    protected function AddCommand(comm: __GPUCommand<Kernel>): KernelCommandQueue;
    begin
      self.commands += comm;
      Result := self;
    end;
    
    {$endregion Utils}
    
    {$region Exec}
    
    public function AddExec(work_szs: array of UIntPtr; params args: array of CommandQueue<Buffer>): KernelCommandQueue;
    public function AddExec(work_szs: array of integer; params args: array of CommandQueue<Buffer>) :=
    AddExec(work_szs.ConvertAll(sz->new UIntPtr(sz)), args);
    
    public function AddExec1(work_sz1: UIntPtr; params args: array of CommandQueue<Buffer>) := AddExec(new UIntPtr[](work_sz1), args);
    public function AddExec1(work_sz1: integer; params args: array of CommandQueue<Buffer>) := AddExec1(new UIntPtr(work_sz1), args);
    
    public function AddExec2(work_sz1, work_sz2: UIntPtr; params args: array of CommandQueue<Buffer>) := AddExec(new UIntPtr[](work_sz1, work_sz2), args);
    public function AddExec2(work_sz1, work_sz2: integer; params args: array of CommandQueue<Buffer>) := AddExec2(new UIntPtr(work_sz1), new UIntPtr(work_sz2), args);
    
    public function AddExec3(work_sz1, work_sz2, work_sz3: UIntPtr; params args: array of CommandQueue<Buffer>) := AddExec(new UIntPtr[](work_sz1, work_sz2, work_sz3), args);
    public function AddExec3(work_sz1, work_sz2, work_sz3: integer; params args: array of CommandQueue<Buffer>) := AddExec3(new UIntPtr(work_sz1), new UIntPtr(work_sz2), new UIntPtr(work_sz3), args);
    
    
    public function AddExec(work_szs: array of CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>): KernelCommandQueue;
    public function AddExec(work_szs: array of CommandQueue<integer>; params args: array of CommandQueue<Buffer>) :=
    AddExec(work_szs.ConvertAll(sz_q->sz_q.ThenConvert(sz->new UIntPtr(sz))), args);
    
    public function AddExec1(work_sz1: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := AddExec(new CommandQueue<UIntPtr>[](work_sz1), args);
    public function AddExec1(work_sz1: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := AddExec1(work_sz1.ThenConvert(sz->new UIntPtr(sz)), args);
    
    public function AddExec2(work_sz1, work_sz2: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := AddExec(new CommandQueue<UIntPtr>[](work_sz1, work_sz2), args);
    public function AddExec2(work_sz1, work_sz2: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := AddExec2(work_sz1.ThenConvert(sz->new UIntPtr(sz)), work_sz2.ThenConvert(sz->new UIntPtr(sz)), args);
    
    public function AddExec3(work_sz1, work_sz2, work_sz3: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := AddExec(new CommandQueue<UIntPtr>[](work_sz1, work_sz2, work_sz3), args);
    public function AddExec3(work_sz1, work_sz2, work_sz3: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := AddExec3(work_sz1.ThenConvert(sz->new UIntPtr(sz)), work_sz2.ThenConvert(sz->new UIntPtr(sz)), work_sz3.ThenConvert(sz->new UIntPtr(sz)), args);
    
    
    public function AddExec(work_szs: CommandQueue<array of UIntPtr>; params args: array of CommandQueue<Buffer>): KernelCommandQueue;
    public function AddExec(work_szs: CommandQueue<array of integer>; params args: array of CommandQueue<Buffer>): KernelCommandQueue;
    
    {$endregion Exec}
    
    {$region Non-command add's}
    
    public function AddQueue(q: CommandQueueBase): KernelCommandQueue;
    begin
      InternalAddQueue(q);
      Result := self;
    end;
    
    public function AddProc(p: (Kernel,Context)->()): KernelCommandQueue;
    begin
      InternalAddProc(p);
      Result := self;
    end;
    public function AddProc(p: Kernel->()): KernelCommandQueue;
    begin
      InternalAddProc(p);
      Result := self;
    end;
    
    public function AddWaitAll(qs: sequence of CommandQueueBase): KernelCommandQueue;
    begin
      InternalAddWaitAll(qs);
      Result := self;
    end;
    public function AddWaitAny(qs: sequence of CommandQueueBase): KernelCommandQueue;
    begin
      InternalAddWaitAny(qs);
      Result := self;
    end;
    public function AddWaitAll(params qs: array of CommandQueueBase) := AddWaitAll(qs.AsEnumerable);
    public function AddWaitAny(params qs: array of CommandQueueBase) := AddWaitAny(qs.AsEnumerable);
    public function AddWait(q: CommandQueueBase) := AddWaitAll(q);
    
    {$endregion Non-command add's}
    
  end;
  
  Kernel = sealed class
    private _kernel: cl_kernel;
    
    {$region constructor's}
    
    private constructor := raise new System.NotSupportedException;
    
    public constructor(prog: ProgramCode; name: string);
    
    {$endregion constructor's}
    
    {$region Queue's}
    
    public function NewQueue :=
    new KernelCommandQueue(self);
    
    {$endregion Queue's}
    
    {$region Exec}
    
    public function Exec(work_szs: array of UIntPtr; params args: array of CommandQueue<Buffer>): Kernel;
    public function Exec(work_szs: array of integer; params args: array of CommandQueue<Buffer>) :=
    Exec(work_szs.ConvertAll(sz->new UIntPtr(sz)), args);
    
    public function Exec1(work_sz1: UIntPtr; params args: array of CommandQueue<Buffer>) := Exec(new UIntPtr[](work_sz1), args);
    public function Exec1(work_sz1: integer; params args: array of CommandQueue<Buffer>) := Exec1(new UIntPtr(work_sz1), args);
    
    public function Exec2(work_sz1, work_sz2: UIntPtr; params args: array of CommandQueue<Buffer>) := Exec(new UIntPtr[](work_sz1, work_sz2), args);
    public function Exec2(work_sz1, work_sz2: integer; params args: array of CommandQueue<Buffer>) := Exec2(new UIntPtr(work_sz1), new UIntPtr(work_sz2), args);
    
    public function Exec3(work_sz1, work_sz2, work_sz3: UIntPtr; params args: array of CommandQueue<Buffer>) := Exec(new UIntPtr[](work_sz1, work_sz2, work_sz3), args);
    public function Exec3(work_sz1, work_sz2, work_sz3: integer; params args: array of CommandQueue<Buffer>) := Exec3(new UIntPtr(work_sz1), new UIntPtr(work_sz2), new UIntPtr(work_sz3), args);
    
    
    public function Exec(work_szs: array of CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>): Kernel;
    public function Exec(work_szs: array of CommandQueue<integer>; params args: array of CommandQueue<Buffer>) :=
    Exec(work_szs.ConvertAll(sz_q->sz_q.ThenConvert(sz->new UIntPtr(sz))), args);
    
    public function Exec1(work_sz1: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := Exec(new CommandQueue<UIntPtr>[](work_sz1), args);
    public function Exec1(work_sz1: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := Exec1(work_sz1.ThenConvert(sz->new UIntPtr(sz)), args);
    
    public function Exec2(work_sz1, work_sz2: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := Exec(new CommandQueue<UIntPtr>[](work_sz1, work_sz2), args);
    public function Exec2(work_sz1, work_sz2: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := Exec2(work_sz1.ThenConvert(sz->new UIntPtr(sz)), work_sz2.ThenConvert(sz->new UIntPtr(sz)), args);
    
    public function Exec3(work_sz1, work_sz2, work_sz3: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := Exec(new CommandQueue<UIntPtr>[](work_sz1, work_sz2, work_sz3), args);
    public function Exec3(work_sz1, work_sz2, work_sz3: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := Exec3(work_sz1.ThenConvert(sz->new UIntPtr(sz)), work_sz2.ThenConvert(sz->new UIntPtr(sz)), work_sz3.ThenConvert(sz->new UIntPtr(sz)), args);
    
    
    public function Exec(work_szs: CommandQueue<array of UIntPtr>; params args: array of CommandQueue<Buffer>): Kernel;
    public function Exec(work_szs: CommandQueue<array of integer>; params args: array of CommandQueue<Buffer>): Kernel;
    
    {$endregion Exec}
    
    protected procedure Finalize; override :=
    cl.ReleaseKernel(self._kernel).RaiseIfError;
    
  end;
  
  {$endregion Kernel}
  
  {$region Context}
  
  Context = sealed class
    private static _def_cont: Context;
    
    private _device: cl_device_id;
    private _context: cl_context;
    private need_finnalize := false;
    
    public static property &Default: Context read _def_cont write _def_cont;
    
    static constructor :=
    try
      _def_cont := new Context;
    except
      try
        _def_cont := new Context(DeviceType.DEVICE_TYPE_ALL); // если нету GPU - попытаться хотя бы для чего то инициализировать
      except
        _def_cont := nil;
      end;
    end;
    
    public constructor := Create(DeviceType.DEVICE_TYPE_GPU);
    
    public constructor(dt: DeviceType);
    begin
      var ec: ErrorCode;
      
      var _platform: cl_platform_id;
      cl.GetPlatformIDs(1, _platform, IntPtr.Zero).RaiseIfError;
      
      cl.GetDeviceIDs(_platform, dt, 1, _device, IntPtr.Zero).RaiseIfError;
      
      _context := cl.CreateContext(IntPtr.Zero, 1, _device, nil, IntPtr.Zero, ec);
      ec.RaiseIfError;
      
      need_finnalize := true;
    end;
    
    public constructor(context: cl_context);
    begin
      
      cl.GetContextInfo(context, ContextInfo.CONTEXT_DEVICES, new UIntPtr(IntPtr.Size), _device, IntPtr.Zero).RaiseIfError;
      
      _context := context;
    end;
    
    public constructor(context: cl_context; device: cl_device_id);
    begin
      _device := device;
      _context := context;
    end;
    
    public function BeginInvoke<T>(q: CommandQueue<T>) := new CLTask<T>(q, self);
    public function BeginInvoke(q: CommandQueueBase): CLTaskBase := new __CLTaskResLess(q, self);
    
    public function SyncInvoke<T>(q: CommandQueue<T>) := BeginInvoke(q).WaitRes();
    public function SyncInvoke(q: CommandQueueBase) := BeginInvoke(q).WaitRes();
    
    protected procedure Finalize; override :=
    if need_finnalize then // если было исключение при инициализации или инициализация произошла из дескриптора
      cl.ReleaseContext(_context).RaiseIfError;
    
  end;
  
  {$endregion Context}
  
  {$region ProgramCode}
  
  ProgramCode = sealed class
    private _program: cl_program;
    
    {$region constructor's}
    
    private constructor := exit;
    
    public constructor(c: Context; params files_texts: array of string);
    begin
      var ec: ErrorCode;
      
      self._program := cl.CreateProgramWithSource(c._context, files_texts.Length, files_texts, files_texts.ConvertAll(s->new UIntPtr(s.Length)), ec);
      ec.RaiseIfError;
      
      cl.BuildProgram(self._program, 1,c._device, nil, nil,IntPtr.Zero).RaiseIfError;
      
    end;
    
    public constructor(params files_texts: array of string) :=
    Create(Context.Default, files_texts);
    
    {$endregion constructor's}
    
    {$region GetKernel}
    
    public property KernelByName[kname: string]: Kernel read new Kernel(self, kname); default;
    
    public function GetAllKernels: Dictionary<string, Kernel>;
    begin
      // Можно и "cl.CreateKernelsInProgram", но тогда имена придётся получать отдельно, а они нужны
      
      var names_char_len: UIntPtr;
      cl.GetProgramInfo(_program, ProgramInfo.PROGRAM_KERNEL_NAMES, UIntPtr.Zero,IntPtr.Zero, names_char_len).RaiseIfError;
      
      var names_ptr := Marshal.AllocHGlobal(IntPtr(pointer(names_char_len)));
      cl.GetProgramInfo(_program, ProgramInfo.PROGRAM_KERNEL_NAMES, names_char_len,names_ptr, IntPtr.Zero).RaiseIfError;
      
      var names := Marshal.PtrToStringAnsi(names_ptr).Split(';');
      Marshal.FreeHGlobal(names_ptr);
      
      Result := new Dictionary<string, Kernel>(names.Length);
      foreach var kname in names do
        Result[kname] := self[kname];
      
    end;
    
    {$endregion GetKernel}
    
    {$region Serialize}
    
    public function Serialize: array of byte;
    begin
      var bytes_count: UIntPtr;
      cl.GetProgramInfo(_program, ProgramInfo.PROGRAM_BINARY_SIZES, new UIntPtr(UIntPtr.Size),bytes_count, IntPtr.Zero).RaiseIfError;
      
      Result := new byte[bytes_count.ToUInt64];
      cl.GetProgramInfo(_program, ProgramInfo.PROGRAM_BINARIES, bytes_count,Result[0], IntPtr.Zero).RaiseIfError;
      
    end;
    
    public procedure SerializeTo(bw: System.IO.BinaryWriter);
    begin
      var bts := Serialize;
      bw.Write(bts.Length);
      bw.Write(bts);
    end;
    
    public procedure SerializeTo(str: System.IO.Stream) := SerializeTo(new System.IO.BinaryWriter(str));
    
    {$endregion Serialize}
    
    {$region Deserialize}
    
    public static function Deserialize(c: Context; bin: array of byte): ProgramCode;
    begin
      Result := new ProgramCode;
      var bin_len := new UIntPtr(bin.Length);
      
      var bin_arr: array of array of byte;
      SetLength(bin_arr,1);
      bin_arr[0] := bin;
      
      var ec: ErrorCode;
      Result._program := cl.CreateProgramWithBinary(c._context,1,c._device, bin_len,bin_arr, IntPtr.Zero,ec);
      ec.RaiseIfError;
      
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
    
    {$endregion Deserialize}
    
    protected procedure Finalize; override :=
    cl.ReleaseProgram(_program).RaiseIfError;
    
  end;
  
  {$endregion ProgramCode}
  
{$region Сахарные подпрограммы}

{$region HostExec}

function HFQ<T>(f: ()->T): CommandQueue<T>;
function HFQ<T>(f: Context->T): CommandQueue<T>;

function HPQ(p: ()->()): CommandQueueBase;
function HPQ(p: Context->()): CommandQueueBase;

{$endregion HostExec}

{$region CombineQueues}

{$region Sync}

{$region NonConv}

function CombineSyncQueueBase(qs: sequence of CommandQueueBase): CommandQueueBase;
function CombineSyncQueueBase(params qs: array of CommandQueueBase): CommandQueueBase;

function CombineSyncQueue<T>(qs: sequence of CommandQueueBase): CommandQueue<T>;
function CombineSyncQueue<T>(params qs: array of CommandQueueBase): CommandQueue<T>;

function CombineSyncQueue<T>(qs: sequence of CommandQueue<T>): CommandQueue<T>;
function CombineSyncQueue<T>(params qs: array of CommandQueue<T>): CommandQueue<T>;

{$endregion NonConv}

{$region Conv}

{$region NonContext}

function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;
function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;

function CombineSyncQueue<TInp,TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;
function CombineSyncQueue<TInp,TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;

{$endregion NonContext}

{$region Context}

function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;
function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;

function CombineSyncQueue<TInp,TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;
function CombineSyncQueue<TInp,TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;

{$endregion Context}

{$endregion Conv}

{$endregion Sync}

{$region Async}

{$region NonConv}

function CombineAsyncQueueBase(qs: sequence of CommandQueueBase): CommandQueueBase;
function CombineAsyncQueueBase(params qs: array of CommandQueueBase): CommandQueueBase;

function CombineAsyncQueue<T>(qs: sequence of CommandQueueBase): CommandQueue<T>;
function CombineAsyncQueue<T>(params qs: array of CommandQueueBase): CommandQueue<T>;

function CombineAsyncQueue<T>(qs: sequence of CommandQueue<T>): CommandQueue<T>;
function CombineAsyncQueue<T>(params qs: array of CommandQueue<T>): CommandQueue<T>;

{$endregion NonConv}

{$region Conv}

{$region NonContext}

function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;
function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;

function CombineAsyncQueue<TInp,TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;
function CombineAsyncQueue<TInp,TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;

{$endregion NonContext}

{$region Context}

function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;
function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;

function CombineAsyncQueue<TInp,TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;
function CombineAsyncQueue<TInp,TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;

{$endregion Context}

{$endregion Conv}

{$endregion Async}

{$endregion CombineQueues}

{$region Wait}

function WaitFor(q: CommandQueueBase): CommandQueueBase;

function WaitForAll(qs: sequence of CommandQueueBase): CommandQueueBase;
function WaitForAll(params qs: array of CommandQueueBase): CommandQueueBase;

function WaitForAny(qs: sequence of CommandQueueBase): CommandQueueBase;
function WaitForAny(params qs: array of CommandQueueBase): CommandQueueBase;

{$endregion Wait}

{$endregion Сахарные подпрограммы}

implementation

{$region Misc}

{$region CommandQueue}

function CommandQueueBase.InvokeNewQBase(tsk: CLTaskBase; c: Context): __IQueueRes;
begin
  var cq := cl_command_queue.Zero;
  Result := InvokeBase(tsk, c, cq, new __EventList);
  
  var CQFree: Action := ()->tsk.AddErr( cl.ReleaseCommandQueue(cq) );
  
  if Result.EvBase.count=0 then
    if cq<>cl_command_queue.Zero then Task.Run(CQFree) else
    Result := Result.AttachCallbackBase((ev,st,data)->
    begin
      tsk.AddErr( st );
      if cq<>cl_command_queue.Zero then Task.Run(CQFree);
      __NativUtils.GCHndFree(data);
    end, c, cq);
end;

function CommandQueue<T>.InvokeNewQ(tsk: CLTaskBase; c: Context): __QueueRes<T>;
begin
  var cq := cl_command_queue.Zero;
  Result := Invoke(tsk, c, cq, new __EventList);
  
  var CQFree: Action := ()->tsk.AddErr( cl.ReleaseCommandQueue(cq) );
  
  if Result.ev.count=0 then
    if cq<>cl_command_queue.Zero then Task.Run(CQFree) else
    Result := Result.AttachCallback((ev,st,data)->
    begin
      tsk.AddErr( st );
      if cq<>cl_command_queue.Zero then Task.Run(CQFree);
      __NativUtils.GCHndFree(data);
    end, c, cq);
end;

procedure CommandQueueBase.RegisterWaiterTask(tsk: CLTaskBase) :=
lock mw_evs do if not mw_evs.ContainsKey(tsk) then
begin
  mw_evs.Add(tsk, new __MWEventContainer);
  tsk.WhenDone(tsk->mw_evs.Remove(tsk));
end;

function CommandQueueBase.GetMWEvent(tsk: CLTaskBase; c: Context): cl_event;
begin
  var cont: __MWEventContainer;
  lock mw_evs do cont := mw_evs[tsk];
  
  lock cont do
    if cont.cached<>0 then
      cont.cached -= 1 else
    begin
      Result := CreateUserEvent(c);
      cont.curr_evs.Enqueue(Result);
    end;
  
end;

procedure CommandQueueBase.SignalMWEvent(tsk: CLTaskBase);
begin
  var conts: array of __MWEventContainer;
  lock mw_evs do conts := mw_evs.Values.ToArray;
  
  for var i := 0 to conts.Length-1 do
  begin
    var cont := conts[i];
    lock cont do
      if cont.curr_evs.Count=0 then
        cont.cached += 1 else
      begin
        var ev := cont.curr_evs.Dequeue;
        tsk.AddErr( cl.SetUserEventStatus(ev, CommandExecutionStatus.COMPLETE) );
      end;
  end;
  
end;

static function CommandQueueBase.CreateUserEvent(c: Context): cl_event;
begin
  var ec: ErrorCode;
  Result := cl.CreateUserEvent(c._context, ec);
  ec.RaiseIfError;
end;

{$endregion CommandQueue}

static procedure __EventList.AttachCallback(ev: cl_event; cb: EventCallback) :=
cl.SetEventCallback(ev, CommandExecutionStatus.COMPLETE, cb, __NativUtils.GCHndAlloc(cb)).RaiseIfError;
static procedure __EventList.AttachCallback(ev: cl_event; cb: EventCallback; tsk: CLTaskBase) :=
tsk.AddErr( cl.SetEventCallback(ev, CommandExecutionStatus.COMPLETE, cb, __NativUtils.GCHndAlloc(cb)) );

function __EventList.AttachCallback(cb: EventCallback; c: Context; var cq: cl_command_queue): cl_event;
begin
  
  var ev: cl_event;
  if self.count>1 then
  begin
    
    if cq=cl_command_queue.Zero then
    begin
      var ec: ErrorCode;
      cq := cl.CreateCommandQueueWithProperties(c._context,c._device, IntPtr.Zero, ec);
      ec.RaiseIfError;
    end;
    
    cl.EnqueueMarkerWithWaitList(cq, self.count, self.evs, ev).RaiseIfError;
    self.Release;
  end else
    ev := self[0];
  
  AttachCallback(ev, cb);
  Result := ev;
end;

function LazyQuickTransformBase<T2>(self: __IQueueRes; f: object->T2): __QueueRes<T2>; extensionmethod;
begin
  Result.ev := self.EvBase;
  
  if self.IsF then
    Result.res_f := ()->f( self.ResFBase()() ) else //ToDo #2150, #2173
  if self.EvBase.count=0 then
    Result.res   :=     f( self.ResBase()    ) else
    Result.res_f := ()->f( self.ResBase()    );
  
end;

{$endregion Misc}

{$region CommandQueue}

{$region ConstQueue}

static function CommandQueueBase.operator implicit(o: object): CommandQueueBase :=
new ConstQueue<object>(o);

static function CommandQueue<T>.operator implicit(o: T): CommandQueue<T> :=
new ConstQueue<T>(o);

{$endregion ConstQueue}

{$region Cast}

type
  CastQueue<T> = sealed class(__ContainerQueue<T>)
    private q: CommandQueueBase;
    
    public constructor(q: CommandQueueBase) := self.q := q;
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; override :=
    __QueueRes&<T>( q.InvokeBase(tsk, c, cq, prev_ev).LazyQuickTransformBase(o->T(o)) );
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override :=
    q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
function CommandQueueBase.Cast<T>: CommandQueue<T>;
begin
  Result := self as CommandQueue<T>;
  if Result=nil then Result := new CastQueue<T>(self);
end;

{$endregion Cast}

{$region HostFunc}

type
  CommandQueueHostFuncBase<T> = abstract class(__HostQueue<object,T>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<object>; override;
    begin
      Result.ev := prev_ev;
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override := exit;
    
  end;
  
  CommandQueueHostFunc<T> = sealed class(CommandQueueHostFuncBase<T>)
    private f: ()->T;
    
    public constructor(f: ()->T) :=
    self.f := f;
    
    protected function ExecFunc(o: object; c: Context): T; override := f();
    
  end;
  CommandQueueHostFuncC<T> = sealed class(CommandQueueHostFuncBase<T>)
    private f: Context->T;
    
    public constructor(f: Context->T) :=
    self.f := f;
    
    protected function ExecFunc(o: object; c: Context): T; override := f(c);
    
  end;
  
  CommandQueueHostProc = sealed class(CommandQueueHostFuncBase<object>)
    private p: ()->();
    
    public constructor(p: ()->()) :=
    self.p := p;
    
    protected function ExecFunc(o: object; c: Context): object; override;
    begin
      p();
      Result := nil;
    end;
    
  end;
  CommandQueueHostProcС = sealed class(CommandQueueHostFuncBase<object>)
    private p: Context->();
    
    public constructor(p: Context->()) :=
    self.p := p;
    
    protected function ExecFunc(o: object; c: Context): object; override;
    begin
      p(c);
      Result := nil;
    end;
    
  end;
  
function HFQ<T>(f: ()->T) :=
new CommandQueueHostFunc<T>(f);
function HFQ<T>(f: Context->T) :=
new CommandQueueHostFuncC<T>(f);

function HPQ(p: ()->()) :=
new CommandQueueHostProc(p);
function HPQ(p: Context->()) :=
new CommandQueueHostProcС(p);

{$endregion HostFunc}

{$region ThenConvert}

type
  CommandQueueThenConvertBase<TInp,TRes> = abstract class(__HostQueue<TInp, TRes>)
    q: CommandQueue<TInp>;
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<TInp>; override :=
    q.Invoke(tsk, c, cq, prev_ev);
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override :=
    q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
  CommandQueueThenConvert<TInp,TRes> = sealed class(CommandQueueThenConvertBase<TInp,TRes>)
    private f: TInp->TRes;
    
    constructor(q: CommandQueue<TInp>; f: TInp->TRes);
    begin
      self.q := q;
      self.f := f;
    end;
    
    protected function ExecFunc(o: TInp; c: Context): TRes; override := f(o);
    
  end;
  CommandQueueThenConvertC<TInp,TRes> = sealed class(CommandQueueThenConvertBase<TInp,TRes>)
    private f: (TInp,Context)->TRes;
    
    constructor(q: CommandQueue<TInp>; f: (TInp,Context)->TRes);
    begin
      self.q := q;
      self.f := f;
    end;
    
    protected function ExecFunc(o: TInp; c: Context): TRes; override := f(o, c);
    
  end;
  
//function CommandQueueBase.ThenConvert<T>(f: object->T) :=
//self.Cast&<object>.ThenConvert(f);

//function CommandQueueBase.ThenConvert<T>(f: (object,Context)->T) :=
//self.Cast&<object>.ThenConvert(f);

function CommandQueue<T>.ThenConvert<T2>(f: T->T2): CommandQueue<T2>;
begin
  var scq := self as ConstQueue<T>;
  if scq=nil then
    Result := new CommandQueueThenConvert<T,T2>(self, f) else
    Result := new CommandQueueHostFunc<T2>(()->f(scq.res));
end;

function CommandQueue<T>.ThenConvert<T2>(f: (T,Context)->T2): CommandQueue<T2>;
begin
  var scq := self as ConstQueue<T>;
  if scq=nil then
    Result := new CommandQueueThenConvertC<T,T2>(self, f) else
    Result := new CommandQueueHostFuncC<T2>(c->f(scq.res, c));
end;

{$endregion ThenConvert}

{$region Multiusable}

type
  MultiusableCommandQueueHub<T> = sealed class
    public q: CommandQueue<T>;
    public constructor(q: CommandQueue<T>) := self.q := q;
    
    public function OnNodeInvoked(tsk: CLTaskBase; c: Context): __QueueRes<T>;
    begin
      
      var res_o: __IQueueRes;
      if tsk.mu_res.TryGetValue(self, res_o) then
        Result := __QueueRes&<T>( res_o ) else
      begin
        Result := self.q.InvokeNewQ(tsk, c);
        tsk.mu_res.Add(self, Result);
      end;
      
      Result.ev.Retain;
    end;
    
    public function MakeNode: CommandQueue<T>;
    
  end;
  
  MultiusableCommandQueueNode<T> = sealed class(__ContainerQueue<T>)
    public hub: MultiusableCommandQueueHub<T>;
    public constructor(hub: MultiusableCommandQueueHub<T>) := self.hub := hub;
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; override;
    begin
      Result := hub.OnNodeInvoked(tsk, c);
      Result.ev := prev_ev + Result.ev;
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override :=
    if prev_hubs.Add(hub) then hub.q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
function MultiusableCommandQueueHub<T>.MakeNode :=
new MultiusableCommandQueueNode<T>(self);

function CommandQueue<T>.Multiusable: ()->CommandQueue<T>;
begin
  var hub := new MultiusableCommandQueueHub<T>(self);
  Result := hub.MakeNode;
end;

{$endregion Multiusable}

{$region Sync/Async Base}

type
  IQueueArray = interface
    function GetQS: sequence of CommandQueueBase;
  end;
  
  {$region Sync}
  
  SimpleQueueArray<T> = abstract class(__ContainerQueue<T>, IQueueArray)
    private qs: array of CommandQueueBase;
    
    public function GetQS: sequence of CommandQueueBase := qs;
    
    public constructor(qs: array of CommandQueueBase) := self.qs := qs;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override :=
    foreach var q in qs do q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
  {$endregion Sync}
  
  {$region Async}
  
  HQAExecutor<T> = abstract class //ToDo #2150
    
    /// синхронно или асинхронно запускает очереди qs и возвращает общий для них ивент в _prev_ev
    protected function WorkOn(qs: array of CommandQueue<T>; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var _prev_ev: __EventList): array of __QueueRes<T>; abstract;
    
  end;
  
  HQAExecutorSync<T> = sealed class(HQAExecutor<T>)
    
    protected function WorkOn(qs: array of CommandQueue<T>; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var _prev_ev: __EventList): array of __QueueRes<T>; override;
    begin
      var prev_ev := _prev_ev;
      Result := new __QueueRes<T>[qs.Length];
      
      for var i := 0 to qs.Length-1 do
      begin
        var r := qs[i].Invoke(tsk, c, cq, prev_ev);
        prev_ev := r.ev;
        Result[i] := r;
      end;
      
      _prev_ev := prev_ev;
    end;
    
  end;
  HQAExecutorAsync<T> = sealed class(HQAExecutor<T>)
    
    protected function WorkOn(qs: array of CommandQueue<T>; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var _prev_ev: __EventList): array of __QueueRes<T>; override;
    begin
      var prev_ev := _prev_ev;
      Result := new __QueueRes<T>[qs.Length];
      
      var evs := new __EventList[qs.Length];
      var count := 0;
      
      for var i := 0 to qs.Length-1 do
      begin
        var ncq := cl_command_queue.Zero;
        prev_ev.Retain;
        var res := qs[i].Invoke(tsk, c, ncq, prev_ev);
        Result[i].res := res.res;
        Result[i].res_f := res.res_f;
        
        var CQFree: Action := ()->tsk.AddErr( cl.ReleaseCommandQueue(ncq) );
        
        if res.ev.count=0 then
        begin
          if ncq<>cl_command_queue.Zero then
            Task.Run(CQFree);
        end else
          res := res.AttachCallback((_ev,_st,_data)->
          begin
            tsk.AddErr( _st );
            if ncq<>cl_command_queue.Zero then Task.Run(CQFree);
            __NativUtils.GCHndFree(_data);
          end, c, ncq);
        
        evs[i] := res.ev;
        count += res.ev.count;
      end;
      prev_ev.Release;
      
      prev_ev := new __EventList(count);
      foreach var ev in evs do
        prev_ev += ev;
      
      _prev_ev := prev_ev;
    end;
    
  end;
  
  HostQueueArrayBase<TInp,TRes> = abstract class(__HostQueue<array of TInp, TRes>, IQueueArray)
    private qs: array of CommandQueue<TInp>;
    private executor: HQAExecutor<TInp>;
    
    protected procedure InitExecutor(is_sync: boolean) :=
    self.executor := is_sync ? new HQAExecutorSync<TInp> as HQAExecutor<TInp> : new HQAExecutorAsync<TInp>;
    
    public function GetQS: sequence of CommandQueueBase := qs.Cast&<CommandQueueBase>;
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<array of TInp>; override;
    begin
      var res := executor.WorkOn(qs, tsk, c, cq, prev_ev);
      
      if res.Any(qr->qr.res_f<>nil) then
      begin
        var res_ref := res;
        
        Result.res_f := ()->
        begin
          var res := new TInp[res_ref.Length];
          for var i := 0 to res_ref.Length-1 do
            res[i] := res_ref[i].Get();
          Result := res;
        end;
        
      end else
      begin
        Result.res := new TInp[res.Length];
        for var i := 0 to res.Length-1 do
          Result.res[i] := res[i].res;
      end;
      
      Result.ev := prev_ev;
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override :=
    foreach var q in qs do q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
  HostQueueArray<TInp,TRes> = sealed class(HostQueueArrayBase<TInp,TRes>)
    private conv: Func<array of TInp, TRes>;
    
    protected constructor(qs: array of CommandQueue<TInp>; conv: Func<array of TInp, TRes>; is_sync: boolean);
    begin
      self.qs := qs;
      self.conv := conv;
      InitExecutor(is_sync);
    end;
    
    protected function ExecFunc(a: array of TInp; c: Context): TRes; override := conv(a);
    
  end;
  HostQueueArrayC<TInp,TRes> = sealed class(HostQueueArrayBase<TInp,TRes>)
    private conv: Func<array of TInp, Context, TRes>;
    
    protected constructor(qs: array of CommandQueue<TInp>; conv: Func<array of TInp, Context, TRes>; is_sync: boolean);
    begin
      self.qs := qs;
      self.conv := conv;
      InitExecutor(is_sync);
    end;
    
    protected function ExecFunc(a: array of TInp; c: Context): TRes; override := conv(a, c);
    
  end;
  
  {$endregion Async}
  
function FlattenQueueArray<T>(inp: sequence of CommandQueueBase): array of CommandQueueBase; where T: IQueueArray;
begin
  var enmr := inp.GetEnumerator;
  if not enmr.MoveNext then raise new InvalidOperationException('%FlattenQueueArray:inp empty%');
  
  var res := new List<CommandQueueBase>;
  while true do
  begin
    var curr := enmr.Current;
    var next := enmr.MoveNext;
    
    if not (curr is IConstQueue) or not next then
      if curr as object is T(var sqa) then //ToDo #2146
        res.AddRange(sqa.GetQS) else
        res += curr;
    
    if not next then break;
  end;
  
  Result := res.ToArray;
end;

{$endregion Sync/Async Base}

{$region SyncArray}

type
  CommandQueueSyncArray<T> = sealed class(SimpleQueueArray<T>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; override;
    begin
      
      for var i := 0 to qs.Length-2 do
        prev_ev := qs[i].InvokeBase(tsk, c, cq, prev_ev).EvBase;
      
      Result := (qs[qs.Length-1] as CommandQueue<T>).Invoke(tsk, c, cq, prev_ev);
    end;
    
  end;
  
static function CommandQueueBase.operator+(q1, q2: CommandQueueBase): CommandQueueBase :=
CombineSyncQueueBase(q1,q2);
static function CommandQueueBase.operator+<T>(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T> :=
CombineSyncQueue&<T>(q1,q2);

{$region NonConv}

function __CombineSyncQueue<T>(qss: sequence of CommandQueueBase): CommandQueue<T>;
begin
  var qs := FlattenQueueArray&<CommandQueueSyncArray<T>>(qss);
  qs[qs.Length-1] := qs[qs.Length-1].Cast&<T>;
  
  if qs.Length=1 then
    Result := qs[0] else
    Result := new CommandQueueSyncArray<T>(qs);
  
end;

function CombineSyncQueueBase(qs: sequence of CommandQueueBase) :=
__CombineSyncQueue&<object>(qs);

function CombineSyncQueueBase(params qs: array of CommandQueueBase) :=
__CombineSyncQueue&<object>(qs);

function CombineSyncQueue<T>(qs: sequence of CommandQueueBase) :=
__CombineSyncQueue&<T>(qs);

function CombineSyncQueue<T>(params qs: array of CommandQueueBase) :=
__CombineSyncQueue&<T>(qs);

function CombineSyncQueue<T>(qs: sequence of CommandQueue<T>) :=
__CombineSyncQueue&<T>(qs.Cast&<CommandQueueBase>);

function CombineSyncQueue<T>(params qs: array of CommandQueue<T>) :=
__CombineSyncQueue&<T>(qs.Cast&<CommandQueueBase>);

{$endregion NonConv}

{$region Conv}

{$region NoContext}

function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase) :=
new HostQueueArray<object,TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv, true);

function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase) :=
new HostQueueArray<object,TRes>(qs.ConvertAll(q->q.Cast&<object>), conv, true);

function CombineSyncQueue<TInp,TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>) :=
new HostQueueArray<TInp,TRes>(qs.ToArray, conv, true);

function CombineSyncQueue<TInp,TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>) :=
new HostQueueArray<TInp,TRes>(qs.ToArray, conv, true);

{$endregion NoContext}

{$region Context}

function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase) :=
new HostQueueArrayC<object,TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv, true);

function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase) :=
new HostQueueArrayC<object,TRes>(qs.ConvertAll(q->q.Cast&<object>), conv, true);

function CombineSyncQueue<TInp,TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>) :=
new HostQueueArrayC<TInp,TRes>(qs.ToArray, conv, true);

function CombineSyncQueue<TInp,TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>) :=
new HostQueueArrayC<TInp,TRes>(qs.ToArray, conv, true);

{$endregion Context}

{$endregion Conv}

{$endregion SyncArray}

{$region AsyncArray}

type
  CommandQueueAsyncArray<T> = sealed class(SimpleQueueArray<T>)
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; override;
    begin
      
      var evs := new __EventList[qs.Length-1];
      var count := 0;
      
      for var i := 0 to qs.Length-2 do
      begin
        var ncq := cl_command_queue.Zero;
        prev_ev.Retain;
        var ev := qs[i].InvokeBase(tsk, c, ncq, prev_ev).EvBase;
        
        var CQFree: Action := ()->tsk.AddErr( cl.ReleaseCommandQueue(ncq) );
        
        if ev.count=0 then
          if ncq<>cl_command_queue.Zero then Task.Run(CQFree) else
          ev := ev.AttachCallback((_ev,_st,_data)->
          begin
            tsk.AddErr( _st );
            if ncq<>cl_command_queue.Zero then Task.Run(CQFree);
            __NativUtils.GCHndFree(_data);
          end, c, ncq);
        
        evs[i] := ev;
        count += ev.count;
      end;
      
      // ничего страшного что 1 из веток использует внешний cq, пока только 1. Так даже эффективнее
      prev_ev.Retain;
      Result := (qs[qs.Length-1] as CommandQueue<T>).Invoke(tsk, c, cq, prev_ev);
      var res_ev := Result.ev;
      prev_ev.Release;
      
      Result.ev := new __EventList(count+res_ev.count);
      foreach var ev in evs do Result.ev += ev;
      Result.ev += res_ev;
      
    end;
    
  end;
  
static function CommandQueueBase.operator*(q1, q2: CommandQueueBase): CommandQueueBase :=
CombineAsyncQueueBase(q1,q2);
static function CommandQueueBase.operator*<T>(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T> :=
CombineAsyncQueue&<T>(q1,q2);

{$region NonConv}

function __CombineAsyncQueue<T>(qss: sequence of CommandQueueBase): CommandQueue<T>;
begin
  var qs := FlattenQueueArray&<CommandQueueAsyncArray<T>>(qss);
  qs[qs.Length-1] := qs[qs.Length-1].Cast&<T>;
  
  if qs.Length=1 then
    Result := qs[0] else
    Result := new CommandQueueAsyncArray<T>(qs);
  
end;

function CombineAsyncQueueBase(qs: sequence of CommandQueueBase) :=
__CombineAsyncQueue&<object>(qs);

function CombineAsyncQueueBase(params qs: array of CommandQueueBase) :=
__CombineAsyncQueue&<object>(qs);

function CombineAsyncQueue<T>(qs: sequence of CommandQueueBase) :=
__CombineAsyncQueue&<T>(qs);

function CombineAsyncQueue<T>(params qs: array of CommandQueueBase) :=
__CombineAsyncQueue&<T>(qs);

function CombineAsyncQueue<T>(qs: sequence of CommandQueue<T>) :=
__CombineAsyncQueue&<T>(qs.Cast&<CommandQueueBase>);

function CombineAsyncQueue<T>(params qs: array of CommandQueue<T>) :=
__CombineAsyncQueue&<T>(qs.Cast&<CommandQueueBase>);

{$endregion NonConv}

{$region Conv}

{$region NoContext}

function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase) :=
new HostQueueArray<object,TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv, false);

function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase) :=
new HostQueueArray<object,TRes>(qs.ConvertAll(q->q.Cast&<object>), conv, false);

function CombineAsyncQueue<TInp,TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>) :=
new HostQueueArray<TInp,TRes>(qs.ToArray, conv, false);

function CombineAsyncQueue<TInp,TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>) :=
new HostQueueArray<TInp,TRes>(qs.ToArray, conv, false);

{$endregion NoContext}

{$region Context}

function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase) :=
new HostQueueArrayC<object,TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv, false);

function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase) :=
new HostQueueArrayC<object,TRes>(qs.ConvertAll(q->q.Cast&<object>), conv, false);

function CombineAsyncQueue<TInp,TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>) :=
new HostQueueArrayC<TInp,TRes>(qs.ToArray, conv, false);

function CombineAsyncQueue<TInp,TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>) :=
new HostQueueArrayC<TInp,TRes>(qs.ToArray, conv, false);

{$endregion Context}

{$endregion Conv}

{$endregion AsyncArray}

{$region Wait}

type
  WCQWaiter = abstract class
    private waitables: array of CommandQueueBase;
    
    public constructor(waitables: array of CommandQueueBase);
    begin
      foreach var q in waitables do q.MakeWaitable;
      self.waitables := waitables;
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase) :=
    foreach var q in waitables do q.RegisterWaiterTask(tsk);
    
    public function GetWaitEv(tsk: CLTaskBase; c: Context): __EventList; abstract;
    
    protected procedure Finalize; override :=
    foreach var q in waitables do q.UnMakeWaitable;
    
  end;
  
  WCQWaiterAll = sealed class(WCQWaiter)
    
    public function GetWaitEv(tsk: CLTaskBase; c: Context): __EventList; override;
    begin
      Result := new __EventList(waitables.Length);
      foreach var q in waitables do
      begin
        var ev := q.GetMWEvent(tsk, c);
        if ev=cl_event.Zero then continue;
        Result += ev;
      end;
    end;
    
  end;
  WCQWaiterAny = sealed class(WCQWaiter)
    
    public function GetWaitEv(tsk: CLTaskBase; c: Context): __EventList; override;
    begin
      var uev := CommandQueueBase.CreateUserEvent(c);
      var done := false;
      var any_ev := false;
      var lo := new object;
      
      foreach var q in waitables do
      begin
        var ev := q.GetMWEvent(tsk, c);
        if ev=cl_event.Zero then continue;
        any_ev := true;
        
        __EventList.AttachCallback(ev, (ev,st,data)->
        begin
          tsk.AddErr( st );
          
          lock lo do
            if not done then
            begin
              tsk.AddErr( cl.SetUserEventStatus(uev, CommandExecutionStatus.COMPLETE) );
              done := true;
            end;
          
          __NativUtils.GCHndFree(data);
        end);
        
        cl.ReleaseEvent(ev).RaiseIfError;
      end;
      
      if any_ev then
        Result := uev else
      begin
        cl.ReleaseEvent(uev).RaiseIfError;
        Result := new __EventList;
      end;
    end;
    
  end;
  
  CommandQueueWaitFor = sealed class(__ContainerQueue<object>)
    public waiter: WCQWaiter;
    
    public constructor(waiter: WCQWaiter) :=
    self.waiter := waiter;
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<object>; override;
    begin
      Result.ev := waiter.GetWaitEv(tsk, c);
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override :=
    waiter.RegisterWaitables(tsk);
    
  end;
  CommandQueueThenWaitFor<T> = sealed class(__ContainerQueue<T>)
    public waiter: WCQWaiter;
    public q: CommandQueue<T>;
    
    public constructor(waiter: WCQWaiter; q: CommandQueue<T>);
    begin
      self.waiter := waiter;
      self.q := q;
    end;
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; override;
    begin
      Result := q.Invoke(tsk, c, cq, prev_ev);
      Result.ev := waiter.GetWaitEv(tsk, c) + Result.ev;
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      waiter.RegisterWaitables(tsk);
      q.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
function WaitFor(q: CommandQueueBase) := WaitForAll(q);

function WaitForAll(params qs: array of CommandQueueBase) := WaitForAll(qs.AsEnumerable);
function WaitForAll(qs: sequence of CommandQueueBase) :=
new CommandQueueWaitFor(
  new WCQWaiterAll(
    qs.ToArray
  )
);

function WaitForAny(params qs: array of CommandQueueBase) := WaitForAny(qs.AsEnumerable);
function WaitForAny(qs: sequence of CommandQueueBase) :=
new CommandQueueWaitFor(
  new WCQWaiterAny(
    qs.ToArray
  )
);

function CommandQueue<T>.CreateWaitWrapper(qs: sequence of CommandQueueBase; all: boolean): CommandQueue<T> :=
new CommandQueueThenWaitFor<T>(
  all?
    new WCQWaiterAll(qs.ToArray) as WCQWaiter :
    new WCQWaiterAny(qs.ToArray),
  self
);

{$endregion Wait}

{$region GPUCommand}

{$region GPUCommandContainer}

type
  __CCBObj<T> = sealed class(__GPUCommandContainerBody<T>)
    public o: T;
    
    public constructor(o: T; cc: __GPUCommandContainer<T>);
    begin
      self.o := o;
      self.cc := cc;
    end;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; override;
    begin
      var res := self.o;
      
      if res as object is Buffer(var b) then
        if b.memobj=cl_mem.Zero then
          b.Init(c);
      
      foreach var comm in cc.commands do
        prev_ev := comm.InvokeObj(res, tsk, c, cq, prev_ev);
      
      Result.ev := prev_ev;
      Result.res := res;
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override := exit;
    
  end;
  
  __CCBQueue<T> = sealed class(__GPUCommandContainerBody<T>)
    public hub: MultiusableCommandQueueHub<T>;
    
    public constructor(q: CommandQueue<T>; cc: __GPUCommandContainer<T>);
    begin
      self.hub := new MultiusableCommandQueueHub<T>(q);
      self.cc := cc;
    end;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __QueueRes<T>; override;
    begin
      var new_plug: ()->CommandQueue<T> := hub.MakeNode;
      
      foreach var comm in cc.commands do
        prev_ev := comm.InvokeQueue(new_plug, tsk, c, cq, prev_ev);
      
      Result := new_plug().Invoke(tsk, c, cq, prev_ev);
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override :=
    hub.q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
constructor __GPUCommandContainer<T>.Create(o: T) :=
self.body := new __CCBObj<T>(o, self);

constructor __GPUCommandContainer<T>.Create(q: CommandQueue<T>) :=
if q is ConstQueue<T>(var cq) then
  self.body := new __CCBObj<T>(cq.Val, self) else
  self.body := new __CCBQueue<T>(q, self);

function BufferCommandQueue.GetSizeQ: CommandQueue<integer>;
begin
  var ob := self.body as __CCBObj<Buffer>;
  if ob<>nil then
    Result := integer(ob.o.Size32) else
    Result := (self.body as __CCBQueue<Buffer>).hub.MakeNode.ThenConvert(b->integer(b.Size32));
end;

{$endregion GPUCommandContainer}

{$region QueueCommand}

type
  QueueCommand<T> = sealed class(__GPUCommand<T>)
    public q: CommandQueueBase;
    
    public constructor(q: CommandQueueBase) :=
    self.q := q;
    
    protected function InvokeObj(o: T; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __EventList; override :=
    q.InvokeBase(tsk, c, cq, prev_ev).EvBase;
    
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __EventList; override :=
    q.InvokeBase(tsk, c, cq, prev_ev).EvBase;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override :=
    q.RegisterWaitables(tsk, prev_hubs);
    
  end;
  
procedure __GPUCommandContainer<T>.InternalAddQueue(q: CommandQueueBase) :=
commands.Add( new QueueCommand<T>(q) );

{$endregion QueueCommand}

{$region ProcCommand}

type
  ProcCommandBase<T> = abstract class(__GPUCommand<T>)
    
    protected procedure ExecProc(c: Context; o: T); abstract;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_res: __QueueRes<T>): cl_event;
    begin
      var uev := CommandQueueBase.CreateUserEvent(c);
      
      Thread.Create(()->
      begin
        
        try
          self.ExecProc(c, prev_res.WaitAndGet);
        except
          on e: Exception do tsk.AddErr(e);
        end;
        
        tsk.AddErr( cl.SetUserEventStatus(uev, CommandExecutionStatus.COMPLETE) );
      end).Start;
      
      Result := uev;
    end;
    
    protected function InvokeObj(o: T; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __EventList; override;
    begin
      var prev_res: __QueueRes<T>;
      prev_res.res := o;
      prev_res.ev := prev_ev;
      Result := Invoke(tsk, c, cq, prev_res);
    end;
    
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __EventList; override :=
    Invoke(tsk, c, cq, o_q().Invoke(tsk, c, cq, prev_ev));
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override := exit;
    
  end;
  
  ProcCommand<T> = sealed class(ProcCommandBase<T>)
    public p: T->();
    
    public constructor(p: T->()) := self.p := p;
    
    protected procedure ExecProc(c: Context; o: T); override := p(o);
    
  end;
  ProcCommandC<T> = sealed class(ProcCommandBase<T>)
    public p: (T,Context)->();
    
    public constructor(p: (T,Context)->()) := self.p := p;
    
    protected procedure ExecProc(c: Context; o: T); override := p(o, c);
    
  end;
  
procedure __GPUCommandContainer<T>.InternalAddProc(p: T->()) :=
commands.Add( new ProcCommand<T>(p) );

procedure __GPUCommandContainer<T>.InternalAddProc(p: (T,Context)->()) :=
commands.Add( new ProcCommandC<T>(p) );

{$endregion ProcCommand}

{$region WaitCommand}

type
  WaitCommand<T> = sealed class(__GPUCommand<T>)
    public waiter: WCQWaiter;
    
    public constructor(waiter: WCQWaiter) :=
    self.waiter := waiter;
    
    protected function InvokeObj(o: T; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __EventList; override :=
    waiter.GetWaitEv(tsk, c) + prev_ev;
    
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __EventList; override :=
    waiter.GetWaitEv(tsk, c) + prev_ev;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override :=
    waiter.RegisterWaitables(tsk);
    
  end;
  
procedure __GPUCommandContainer<T>.InternalAddWaitAll(qs: sequence of CommandQueueBase) :=
commands.Add(new WaitCommand<T>( new WCQWaiterAll(qs.ToArray) ));

procedure __GPUCommandContainer<T>.InternalAddWaitAny(qs: sequence of CommandQueueBase) :=
commands.Add(new WaitCommand<T>( new WCQWaiterAny(qs.ToArray) ));

{$endregion WaitCommand}

{$region EnqueueableGPUCommand}

type
  EnqueueableGPUCommand<T> = abstract class(__GPUCommand<T>)
    protected allow_sync_enq := true;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (T, Context, cl_command_queue, __EventList)->cl_event; abstract;
    
    protected procedure FixCQ(c: Context; var cq: cl_command_queue) :=
    if cq=cl_command_queue.Zero then
    begin
      var ec: ErrorCode;
      cq := cl.CreateCommandQueueWithProperties(c._context,c._device, IntPtr.Zero, ec);
      ec.RaiseIfError;
    end;
    
    protected function Invoke(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList; o_res: __QueueRes<T>): __EventList;
    begin
      var enq_f := InvokeParams(tsk, c, cq, o_res.ev);
      
      if allow_sync_enq and (o_res.ev.count=0) then
        Result := enq_f(o_res.Get, c, cq, prev_ev) else
      begin
        var uev := CommandQueueBase.CreateUserEvent(c);
        
        // Асинхронное Enqueue, придётся пересоздать cq
        var lcq := cq;
        cq := cl_command_queue.Zero;
        
        if not allow_sync_enq then
          Thread.Create(()->
          begin
            
            try
              
              if prev_ev.count<>0 then
              begin
                cl.WaitForEvents(prev_ev.count,prev_ev.evs).RaiseIfError;
                prev_ev.Release;
              end;
              
              enq_f(o_res.WaitAndGet, c, lcq, nil);
            except
              on e: Exception do tsk.AddErr(e);
            end;
            
            tsk.AddErr( cl.SetUserEventStatus(uev,CommandExecutionStatus.COMPLETE) );
            tsk.AddErr( cl.ReleaseCommandQueue(lcq) );
          end).Start else
        begin
          
          var set_complete: EventCallback := (ev,st,data)->
          begin
            tsk.AddErr( st );
            
            Task.Run(()->tsk.AddErr( cl.ReleaseCommandQueue(lcq) ));
            
            tsk.AddErr( cl.SetUserEventStatus(uev, CommandExecutionStatus.COMPLETE) );
            
            __NativUtils.GCHndFree(data);
          end;
          
          cl.ReleaseEvent(o_res.ev.AttachCallback((ev,st,data)->
          begin
            tsk.AddErr( st );
            
            __EventList.AttachCallback(
              enq_f(o_res.Get, c, lcq, prev_ev),
              set_complete,
              tsk
            );
            
            __NativUtils.GCHndFree(data);
          end, c, lcq)).RaiseIfError;
          
        end;
        
        Result := uev;
      end;
      
    end;
    
    protected function InvokeObj(o: T; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __EventList; override;
    begin
      var o_res: __QueueRes<T>;
      o_res.res := o;
      o_res.ev := new __EventList;
      Result := Invoke(tsk, c, cq, prev_ev, o_res);
    end;
    
    protected function InvokeQueue(o_q: ()->CommandQueue<T>; tsk: CLTaskBase; c: Context; var cq: cl_command_queue; prev_ev: __EventList): __EventList; override :=
    Invoke(tsk, c, cq, new __EventList, o_q().Invoke(tsk, c, cq, prev_ev));
    
  end;
  
{$endregion DirectGPUCommandBase}

{$endregion GPUCommand}

{$region Buffer}

{$region BufferCommandQueue}

//ToDo попробовать исправить нормально
function костыль_BufferCommandQueue_Create(b: Buffer; c: Context): Buffer;
begin
  if b.memobj=cl_mem.Zero then b.Init(c);
  Result := b;
end;

constructor BufferCommandQueue.Create(q: CommandQueue<Buffer>) :=
inherited Create(q.ThenConvert(костыль_BufferCommandQueue_Create));

{$endregion BufferCommandQueue}

{$region Write}

type
  BufferCommandWriteData = sealed class(EnqueueableGPUCommand<Buffer>)
    public ptr_q: CommandQueue<IntPtr>;
    public offset_q, len_q: CommandQueue<integer>;
    
    public constructor(ptr_q: CommandQueue<IntPtr>; offset_q, len_q: CommandQueue<integer>);
    begin
      self.ptr_q    := ptr_q;
      self.offset_q := offset_q;
      self.len_q    := len_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var ptr     := ptr_q    .Invoke(tsk, c, cq, new __EventList);
      var offset  := offset_q .Invoke(tsk, c, cq, new __EventList);
      var len     := len_q    .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, ptr.ev, offset.ev, len.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          l_cq, b.memobj, Bool.NON_BLOCKING,
          new UIntPtr(offset.Get), new UIntPtr(len.Get),
          ptr.Get,
          prev_ev.count,prev_ev.evs,res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      ptr_q   .RegisterWaitables(tsk, prev_hubs);
      offset_q.RegisterWaitables(tsk, prev_hubs);
      len_q   .RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
  BufferCommandWriteArray = sealed class(EnqueueableGPUCommand<Buffer>)
    public a_q: CommandQueue<&Array>;
    public offset_q, len_q: CommandQueue<integer>;
    
    public constructor(a_q: CommandQueue<&Array>; offset_q, len_q: CommandQueue<integer>);
    begin
      self.allow_sync_enq := false;
      self.a_q      := a_q;
      self.offset_q := offset_q;
      self.len_q    := len_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var a       := a_q      .Invoke(tsk, c, cq, new __EventList);
      var offset  := offset_q .Invoke(tsk, c, cq, new __EventList);
      var len     := len_q    .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, a.ev, offset.ev, len.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        
        cl.EnqueueWriteBuffer(
          l_cq, b.memobj, Bool.BLOCKING,
          new UIntPtr(offset.WaitAndGet), new UIntPtr(len.WaitAndGet),
          Marshal.UnsafeAddrOfPinnedArrayElement(a.WaitAndGet,0),
          0,IntPtr.Zero,IntPtr.Zero
        ).RaiseIfError;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      a_q     .RegisterWaitables(tsk, prev_hubs);
      offset_q.RegisterWaitables(tsk, prev_hubs);
      len_q   .RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
  BufferCommandWriteValue<T> = sealed class(EnqueueableGPUCommand<Buffer>) where T: record;
    public val: IntPtr;
    public offset_q: CommandQueue<integer>;
    
    public constructor(val: T; offset_q: CommandQueue<integer>);
    begin
      self.val      := __NativUtils.CopyToUnm(val);
      self.offset_q := offset_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var offset  := offset_q .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, offset.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        var res_ev: cl_event;
        
        cl.EnqueueWriteBuffer(
          l_cq, b.memobj, Bool.NON_BLOCKING,
          new UIntPtr(offset.Get), new UIntPtr(Marshal.SizeOf&<T>),
          self.val,
          prev_ev.count,prev_ev.evs,res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      offset_q.RegisterWaitables(tsk, prev_hubs);
    end;
    
    protected procedure Finalize; override :=
    Marshal.FreeHGlobal(self.val);
    
  end;
  BufferCommandWriteValueQ<T> = sealed class(EnqueueableGPUCommand<Buffer>) where T: record;
    public val_q: CommandQueue<T>;
    public offset_q: CommandQueue<integer>;
    
    public constructor(val_q: CommandQueue<T>; offset_q: CommandQueue<integer>);
    begin
      self.val_q    := val_q;
      self.offset_q := offset_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var val     := val_q    .Invoke(tsk, c, cq, new __EventList);
      var offset  := offset_q .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, val.ev, offset.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        var res_ev: cl_event;
        var val_ptr := __NativUtils.CopyToUnm(val.Get());
        
        cl.EnqueueWriteBuffer(
          l_cq, b.memobj, Bool.NON_BLOCKING,
          new UIntPtr(offset.Get), new UIntPtr(Marshal.SizeOf&<T>),
          val_ptr,
          prev_ev.count,prev_ev.evs,res_ev
        ).RaiseIfError;
        
        __EventList.AttachCallback(res_ev, (ev,st,data)->
        begin
          tsk.AddErr(st);
          Marshal.FreeHGlobal(val_ptr);
          __NativUtils.GCHndFree(data);
        end);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      val_q   .RegisterWaitables(tsk, prev_hubs);
      offset_q.RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
  
function BufferCommandQueue.AddWriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandWriteData(ptr, offset, len));


function BufferCommandQueue.AddWriteArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandWriteArray(a, offset, len));


function BufferCommandQueue.AddWriteValue<TRecord>(val: TRecord; offset: CommandQueue<integer>) :=
AddCommand(new BufferCommandWriteValue<TRecord>(val, offset));

function BufferCommandQueue.AddWriteValue<TRecord>(val: CommandQueue<TRecord>; offset: CommandQueue<integer>) :=
AddCommand(new BufferCommandWriteValueQ<TRecord>(val, offset));

{$endregion Write}

{$region Read}

type
  BufferCommandReadData = sealed class(EnqueueableGPUCommand<Buffer>)
    public ptr_q: CommandQueue<IntPtr>;
    public offset_q, len_q: CommandQueue<integer>;
    
    public constructor(ptr_q: CommandQueue<IntPtr>; offset_q, len_q: CommandQueue<integer>);
    begin
      self.ptr_q    := ptr_q;
      self.offset_q := offset_q;
      self.len_q    := len_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var ptr     := ptr_q    .Invoke(tsk, c, cq, new __EventList);
      var offset  := offset_q .Invoke(tsk, c, cq, new __EventList);
      var len     := len_q    .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, ptr.ev, offset.ev, len.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        var res_ev: cl_event;
        
        cl.EnqueueReadBuffer(
          l_cq, b.memobj, Bool.NON_BLOCKING,
          new UIntPtr(offset.Get), new UIntPtr(len.Get),
          ptr.Get,
          prev_ev.count,prev_ev.evs,res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      ptr_q   .RegisterWaitables(tsk, prev_hubs);
      offset_q.RegisterWaitables(tsk, prev_hubs);
      len_q   .RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
  BufferCommandReadArray = sealed class(EnqueueableGPUCommand<Buffer>)
    public a_q: CommandQueue<&Array>;
    public offset_q, len_q: CommandQueue<integer>;
    
    public constructor(a_q: CommandQueue<&Array>; offset_q, len_q: CommandQueue<integer>);
    begin
      self.allow_sync_enq := false;
      self.a_q      := a_q;
      self.offset_q := offset_q;
      self.len_q    := len_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var a       := a_q      .Invoke(tsk, c, cq, new __EventList);
      var offset  := offset_q .Invoke(tsk, c, cq, new __EventList);
      var len     := len_q    .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, a.ev, offset.ev, len.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        
        cl.EnqueueReadBuffer(
          l_cq, b.memobj, Bool.BLOCKING,
          new UIntPtr(offset.WaitAndGet), new UIntPtr(len.WaitAndGet),
          Marshal.UnsafeAddrOfPinnedArrayElement(a.WaitAndGet,0),
          0,IntPtr.Zero,IntPtr.Zero
        ).RaiseIfError;
        
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      a_q     .RegisterWaitables(tsk, prev_hubs);
      offset_q.RegisterWaitables(tsk, prev_hubs);
      len_q   .RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
  
function BufferCommandQueue.AddReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandReadData(ptr, offset, len));


function BufferCommandQueue.AddReadArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandReadArray(a, offset, len));

{$endregion Read}

{$region Fill}

type
  BufferCommandDataFill = sealed class(EnqueueableGPUCommand<Buffer>)
    public ptr_q: CommandQueue<IntPtr>;
    public pattern_len_q, offset_q, len_q: CommandQueue<integer>;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len_q, offset_q, len_q: CommandQueue<integer>);
    begin
      self.ptr_q          := ptr_q;
      self.pattern_len_q  := pattern_len_q;
      self.offset_q       := offset_q;
      self.len_q          := len_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var ptr         := ptr_q        .Invoke(tsk, c, cq, new __EventList);
      var pattern_len := pattern_len_q.Invoke(tsk, c, cq, new __EventList);
      var offset      := offset_q     .Invoke(tsk, c, cq, new __EventList);
      var len         := len_q        .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, ptr.ev, pattern_len.ev, offset.ev, len.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          l_cq, b.memobj,
          ptr.Get, new UIntPtr(pattern_len.Get),
          new UIntPtr(offset.Get), new UIntPtr(len.Get),
          prev_ev.count, prev_ev.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      ptr_q         .RegisterWaitables(tsk, prev_hubs);
      pattern_len_q .RegisterWaitables(tsk, prev_hubs);
      offset_q      .RegisterWaitables(tsk, prev_hubs);
      len_q         .RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
  BufferCommandArrayFill = sealed class(EnqueueableGPUCommand<Buffer>)
    public a_q: CommandQueue<&Array>;
    public offset_q, len_q: CommandQueue<integer>;
    
    public constructor(a_q: CommandQueue<&Array>; offset_q, len_q: CommandQueue<integer>);
    begin
      self.allow_sync_enq := false;
      self.a_q      := a_q;
      self.offset_q := offset_q;
      self.len_q    := len_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var a       := a_q      .Invoke(tsk, c, cq, new __EventList);
      var offset  := offset_q .Invoke(tsk, c, cq, new __EventList);
      var len     := len_q    .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, a.ev, offset.ev, len.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        var res_ev: cl_event;
        var la := a.WaitAndGet;
        
        // Синхронного Fill нету, поэтому между cl.Enqueue и cl.WaitForEvents сборщик мусора может сломать указатель
        // Остаётся только закреплять, хоть так и не любой тип массива пропустит
        var a_hnd := GCHandle.Alloc(la, GCHandleType.Pinned);
        
        cl.EnqueueFillBuffer(
          l_cq, b.memobj,
          a_hnd.AddrOfPinnedObject, new UIntPtr(System.Buffer.ByteLength(la)),
          new UIntPtr(offset.WaitAndGet), new UIntPtr(len.WaitAndGet),
          prev_ev.count,prev_ev.evs,res_ev
        ).RaiseIfError;
        cl.WaitForEvents(1,res_ev).RaiseIfError;
        cl.ReleaseEvent(res_ev).RaiseIfError;
        
        a_hnd.Free;
        Result := cl_event.Zero;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      a_q     .RegisterWaitables(tsk, prev_hubs);
      offset_q.RegisterWaitables(tsk, prev_hubs);
      len_q   .RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
  BufferCommandValueFill<T> = sealed class(EnqueueableGPUCommand<Buffer>) where T: record;
    public val: IntPtr;
    public offset_q, len_q: CommandQueue<integer>;
    
    public constructor(val: T; offset_q, len_q: CommandQueue<integer>);
    begin
      self.val      := __NativUtils.CopyToUnm(val);
      self.offset_q := offset_q;
      self.len_q    := len_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var offset  := offset_q .Invoke(tsk, c, cq, new __EventList);
      var len     := len_q    .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, offset.ev, len.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        var res_ev: cl_event;
        
        cl.EnqueueFillBuffer(
          l_cq, b.memobj,
          val, new UIntPtr(Marshal.SizeOf&<T>),
          new UIntPtr(offset.Get), new UIntPtr(len.Get),
          prev_ev.count,prev_ev.evs,res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      offset_q.RegisterWaitables(tsk, prev_hubs);
      len_q   .RegisterWaitables(tsk, prev_hubs);
    end;
    
    protected procedure Finalize; override :=
    Marshal.FreeHGlobal(self.val);
    
  end;
  BufferCommandValueFillQ<T> = sealed class(EnqueueableGPUCommand<Buffer>) where T: record;
    public val_q: CommandQueue<T>;
    public offset_q, len_q: CommandQueue<integer>;
    
    public constructor(val_q: CommandQueue<T>; offset_q, len_q: CommandQueue<integer>);
    begin
      self.val_q    := val_q;
      self.offset_q := offset_q;
      self.len_q    := len_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var val     := val_q    .Invoke(tsk, c, cq, new __EventList);
      var offset  := offset_q .Invoke(tsk, c, cq, new __EventList);
      var len     := len_q    .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, val.ev, offset.ev, len.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        var res_ev: cl_event;
        var val_ptr := __NativUtils.CopyToUnm(val.Get());
        
        cl.EnqueueFillBuffer(
          l_cq, b.memobj,
          val_ptr, new UIntPtr(Marshal.SizeOf&<T>),
          new UIntPtr(offset.Get), new UIntPtr(len.Get),
          prev_ev.count,prev_ev.evs,res_ev
        ).RaiseIfError;
        
        __EventList.AttachCallback(res_ev, (ev,st,data)->
        begin
          tsk.AddErr(st);
          Marshal.FreeHGlobal(val_ptr);
          __NativUtils.GCHndFree(data);
        end);
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      val_q   .RegisterWaitables(tsk, prev_hubs);
      offset_q.RegisterWaitables(tsk, prev_hubs);
      len_q   .RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
  
function BufferCommandQueue.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandDataFill(ptr,pattern_len, offset,len));


function BufferCommandQueue.AddFillArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandArrayFill(a, offset,len));


function BufferCommandQueue.AddFillValue<TRecord>(val: TRecord; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandValueFill<TRecord>(val, offset, len));

function BufferCommandQueue.AddFillValue<TRecord>(val: CommandQueue<TRecord>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandValueFillQ<TRecord>(val, offset, len));

{$endregion Fill}

{$region Copy}

type
  BufferCommandCopyFrom = sealed class(EnqueueableGPUCommand<Buffer>)
    public buf_q: CommandQueue<Buffer>;
    public f_pos_q, t_pos_q, len_q: CommandQueue<integer>;
    
    public constructor(buf_q: CommandQueue<Buffer>; f_pos_q, t_pos_q, len_q: CommandQueue<integer>);
    begin
      self.buf_q    := buf_q;
      self.f_pos_q  := f_pos_q;
      self.t_pos_q  := t_pos_q;
      self.len_q    := len_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var buf   := buf_q  .Invoke(tsk, c, cq, new __EventList);
      var f_pos := f_pos_q.Invoke(tsk, c, cq, new __EventList);
      var t_pos := t_pos_q.Invoke(tsk, c, cq, new __EventList);
      var len   := len_q  .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, buf.ev, f_pos.ev, t_pos.ev, len.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        var res_ev: cl_event;
        
        var b2 := buf.Get;
        if b2.memobj=cl_mem.Zero then b2.Init(l_c);
        
        cl.EnqueueCopyBuffer(
          l_cq, b2.memobj,b.memobj,
          new UIntPtr(f_pos.Get), new UIntPtr(t_pos.Get),
          new UIntPtr(len.Get),
          prev_ev.count,prev_ev.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      buf_q   .RegisterWaitables(tsk, prev_hubs);
      f_pos_q .RegisterWaitables(tsk, prev_hubs);
      t_pos_q .RegisterWaitables(tsk, prev_hubs);
      len_q   .RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  BufferCommandCopyTo = sealed class(EnqueueableGPUCommand<Buffer>)
    public buf_q: CommandQueue<Buffer>;
    public f_pos_q, t_pos_q, len_q: CommandQueue<integer>;
    
    public constructor(buf_q: CommandQueue<Buffer>; f_pos_q, t_pos_q, len_q: CommandQueue<integer>);
    begin
      self.buf_q    := buf_q;
      self.f_pos_q  := f_pos_q;
      self.t_pos_q  := t_pos_q;
      self.len_q    := len_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Buffer, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var buf   := buf_q  .Invoke(tsk, c, cq, new __EventList);
      var f_pos := f_pos_q.Invoke(tsk, c, cq, new __EventList);
      var t_pos := t_pos_q.Invoke(tsk, c, cq, new __EventList);
      var len   := len_q  .Invoke(tsk, c, cq, new __EventList);
      ev_res := __EventList.Combine(ev_res, buf.ev, f_pos.ev, t_pos.ev, len.ev);
      
      FixCQ(c, cq);
      
      Result := (b, l_c, l_cq, prev_ev)->
      begin
        var res_ev: cl_event;
        
        var b2 := buf.Get;
        if b2.memobj=cl_mem.Zero then b2.Init(l_c);
        
        cl.EnqueueCopyBuffer(
          l_cq, b.memobj,b2.memobj,
          new UIntPtr(f_pos.Get), new UIntPtr(t_pos.Get),
          new UIntPtr(len.Get),
          prev_ev.count,prev_ev.evs, res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      buf_q   .RegisterWaitables(tsk, prev_hubs);
      f_pos_q .RegisterWaitables(tsk, prev_hubs);
      t_pos_q .RegisterWaitables(tsk, prev_hubs);
      len_q   .RegisterWaitables(tsk, prev_hubs);
    end;
    
  end;
  
function BufferCommandQueue.AddCopyFrom(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandCopyFrom(b, from,&to, len));

function BufferCommandQueue.AddCopyTo(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandCopyTo(b, &to,from, len));

{$endregion Copy}

{$endregion Buffer}

{$region Kernel}

{$region Exec}

type
  KernelCommandExec = sealed class(EnqueueableGPUCommand<Kernel>)
    public work_szs_q: CommandQueue<array of UIntPtr>;
    public args_q: array of CommandQueue<Buffer>;
    
    public constructor(work_szs_q: CommandQueue<array of UIntPtr>; args_q: array of CommandQueue<Buffer>);
    begin
      self.work_szs_q := work_szs_q;
      self.args_q     := args_q;
    end;
    
    protected function InvokeParams(tsk: CLTaskBase; c: Context; var cq: cl_command_queue; var ev_res: __EventList): (Kernel, Context, cl_command_queue, __EventList)->cl_event; override;
    begin
      var work_szs  := work_szs_q.Invoke(tsk, c, cq, new __EventList);
      var count := ev_res.count + work_szs.ev.count;
      
      var args := new __QueueRes<Buffer>[args_q.Length];
      for var i := 0 to args.Length-1 do
      begin
        var arg := args_q[i].Invoke(tsk, c, cq, new __EventList);
        count += arg.ev.count;
        args[i] := arg;
      end;
      
      var ev := new __EventList(count);
      ev += ev_res;
      ev += work_szs.ev;
      for var i := 0 to args.Length-1 do
        ev += args[i].ev;
      ev_res := ev;
      
      FixCQ(c, cq);
      
      Result := (k, l_c, l_cq, prev_ev)->
      begin
        var res_ev: cl_event;
        var work_szs_res := work_szs.Get;
        
        for var i := 0 to args.Length-1 do
        begin
          var b := args[i].Get;
          if b.memobj=cl_mem.Zero then b.Init(l_c);
          cl.SetKernelArg(k._kernel, i, new UIntPtr(UIntPtr.Size), b.memobj).RaiseIfError;
        end;
        
        cl.EnqueueNDRangeKernel(
          l_cq,k._kernel,
          work_szs_res.Length,
          nil,work_szs_res,nil,
          prev_ev.count,prev_ev.evs,res_ev
        ).RaiseIfError;
        
        Result := res_ev;
      end;
      
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<object>); override;
    begin
      work_szs_q.RegisterWaitables(tsk, prev_hubs);
      
      foreach var arg in args_q do 
        arg.RegisterWaitables(tsk, prev_hubs);
      
    end;
    
  end;
  
function KernelCommandQueue.AddExec(work_szs: array of UIntPtr; params args: array of CommandQueue<Buffer>) :=
AddCommand(new KernelCommandExec(work_szs, args));

function KernelCommandQueue.AddExec(work_szs: array of CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) :=
AddCommand(new KernelCommandExec(
  CombineAsyncQueue(a->a,work_szs),
  args
));

function KernelCommandQueue.AddExec(work_szs: CommandQueue<array of UIntPtr>; params args: array of CommandQueue<Buffer>) :=
AddCommand(new KernelCommandExec(
  work_szs,
  args
));

function KernelCommandQueue.AddExec(work_szs: CommandQueue<array of integer>; params args: array of CommandQueue<Buffer>) :=
AddCommand(new KernelCommandExec(
  work_szs.ThenConvert(a->a.ConvertAll(sz->new UIntPtr(sz))),
  args
));

{$endregion Exec}

{$endregion Kernel}

{$endregion CommandQueue}

{$region Неявные CommandQueue}

{$region Buffer}

{$region constructor's}

procedure Buffer.Init(c: Context) :=
lock self do
begin
  if self.memobj<>cl_mem.Zero then Dispose;
  GC.AddMemoryPressure(Size64);
  var ec: ErrorCode;
  self.memobj := cl.CreateBuffer(c._context, MemFlags.MEM_READ_WRITE, self.sz, IntPtr.Zero, ec);
  ec.RaiseIfError;
end;

function Buffer.SubBuff(offset, size: integer): Buffer;
begin
  if self.memobj=cl_mem.Zero then Init(Context.Default);
  
  Result := new Buffer(size);
  Result._parent := self;
  
  var ec: ErrorCode;
  var reg := new cl_buffer_region(
    new UIntPtr( offset ),
    new UIntPtr( size )
  );
  Result.memobj := cl.CreateSubBuffer(self.memobj, MemFlags.MEM_READ_WRITE, BufferCreateType.BUFFER_CREATE_TYPE_REGION,reg, ec);
  ec.RaiseIfError;
  
end;

{$endregion constructor's}

{$region Write}

function Buffer.WriteData(ptr: CommandQueue<IntPtr>) :=
Context.Default.SyncInvoke(NewQueue.AddWriteData(ptr) as CommandQueue<Buffer>);
function Buffer.WriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddWriteData(ptr, offset, len) as CommandQueue<Buffer>);

function Buffer.WriteArray(a: CommandQueue<&Array>) :=
Context.Default.SyncInvoke(NewQueue.AddWriteArray(a) as CommandQueue<Buffer>);
function Buffer.WriteArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddWriteArray(a, offset, len) as CommandQueue<Buffer>);

function Buffer.WriteValue<TRecord>(val: CommandQueue<TRecord>; offset: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddWriteValue(val, offset) as CommandQueue<Buffer>);

{$endregion Write}

{$region Read}

function Buffer.ReadData(ptr: CommandQueue<IntPtr>) :=
Context.Default.SyncInvoke(NewQueue.AddReadData(ptr) as CommandQueue<Buffer>);
function Buffer.ReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddReadData(ptr, offset, len) as CommandQueue<Buffer>);

function Buffer.ReadArray(a: CommandQueue<&Array>) :=
Context.Default.SyncInvoke(NewQueue.AddReadArray(a) as CommandQueue<Buffer>);
function Buffer.ReadArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddReadArray(a, offset, len) as CommandQueue<Buffer>);

{$endregion Read}

{$region PatternFill}

function Buffer.FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddFillData(ptr, pattern_len) as CommandQueue<Buffer>);
function Buffer.FillData(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddFillData(ptr, pattern_len, offset, len) as CommandQueue<Buffer>);

function Buffer.FillArray(a: CommandQueue<&Array>) :=
Context.Default.SyncInvoke(NewQueue.AddFillArray(a) as CommandQueue<Buffer>);
function Buffer.FillArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddFillArray(a, offset, len) as CommandQueue<Buffer>);

function Buffer.FillValue<TRecord>(val: TRecord) :=
Context.Default.SyncInvoke(NewQueue.AddFillValue(val) as CommandQueue<Buffer>);
function Buffer.FillValue<TRecord>(val: TRecord; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddFillValue(val, offset, len) as CommandQueue<Buffer>);
function Buffer.FillValue<TRecord>(val: CommandQueue<TRecord>) :=
Context.Default.SyncInvoke(NewQueue.AddFillValue(val) as CommandQueue<Buffer>);
function Buffer.FillValue<TRecord>(val: CommandQueue<TRecord>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddFillValue(val, offset, len) as CommandQueue<Buffer>);

{$endregion PatternFill}

{$region Copy}

function Buffer.CopyFrom(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddCopyFrom(b, from, &to, len) as CommandQueue<Buffer>);
function Buffer.CopyFrom(b: CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddCopyFrom(b) as CommandQueue<Buffer>);

function Buffer.CopyTo(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddCopyTo(b, from, &to, len) as CommandQueue<Buffer>);
function Buffer.CopyTo(b: CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddCopyTo(b) as CommandQueue<Buffer>);

{$endregion Copy}

{$region Get}

function Buffer.GetData(offset, len: CommandQueue<integer>): IntPtr;
begin
  var res: IntPtr;
  
  var Qs_len: ()->CommandQueue<integer>;
  if len is ConstQueue<integer> then
    Qs_len := ()->len else
  if len is MultiusableCommandQueueNode<integer>(var mcqn) then
    Qs_len := mcqn.hub.MakeNode else
    Qs_len := len.Multiusable;
  
  var Q_res := Qs_len().ThenConvert(len_val->
  begin
    Result := Marshal.AllocHGlobal(len_val);
    res := Result;
  end);
  
  Context.Default.SyncInvoke(
    self.NewQueue.AddReadData(Q_res, offset,Qs_len()) as CommandQueue<Buffer>
  );
  
  Result := res;
end;

function Buffer.GetArrayAt<TArray>(offset: CommandQueue<integer>; szs: CommandQueue<array of integer>): TArray;
begin
  var el_t := typeof(TArray).GetElementType;
  
  if szs is ConstQueue<array of integer>(var const_szs) then
  begin
    var res := System.Array.CreateInstance(
      el_t,
      const_szs.res
    );
    
    self.ReadArray(res, 0,Marshal.SizeOf(el_t)*res.Length);
    
    Result := TArray(res);
  end else
  begin
    var Qs_szs: ()->CommandQueue<array of integer>;
    if szs is ConstQueue<array of integer> then
      Qs_szs := ()->szs else
    if szs is MultiusableCommandQueueNode<array of integer>(var mcqn) then
      Qs_szs := mcqn.hub.MakeNode else
      Qs_szs := szs.Multiusable;
    
    var Qs_a := Qs_szs().ThenConvert(szs_val->
    System.Array.CreateInstance(
      el_t,
      szs_val
    )).Multiusable;
    
    var Q_a := Qs_a();
    var Q_a_len := Qs_szs().ThenConvert( szs_val -> Marshal.SizeOf(el_t)*szs_val.Aggregate((i1,i2)->i1*i2) );
    var Q_res := Qs_a().Cast&<TArray>;
    
    Result := Context.Default.SyncInvoke(
      self.NewQueue.AddReadArray(Q_a, offset, Q_a_len)
      as CommandQueue<Buffer> +
      Q_res
    );
  end;
  
end;

function Buffer.GetArrayAt<TArray>(offset: CommandQueue<integer>; params szs: array of CommandQueue<integer>) :=
szs.All(q->q is ConstQueue<integer>) ?
GetArrayAt&<TArray>(offset, CommandQueue&<array of integer>( szs.ConvertAll(q->ConstQueue&<integer>(q).res) )) :
GetArrayAt&<TArray>(offset, CombineAsyncQueue(a->a, szs));

function Buffer.GetValueAt<TRecord>(offset: CommandQueue<integer>): TRecord;
begin
  Context.Default.SyncInvoke(
    self.NewQueue
    .AddReadValue(Result, offset) as CommandQueue<Buffer>
  );
end;

{$endregion Get}

{$endregion Buffer}

{$region Kernel}

{$region constructor's}

constructor Kernel.Create(prog: ProgramCode; name: string);
begin
  var ec: ErrorCode;
  
  self._kernel := cl.CreateKernel(prog._program, name, ec);
  ec.RaiseIfError;
  
end;

{$endregion constructor's}

{$region Exec}

function Kernel.Exec(work_szs: array of UIntPtr; params args: array of CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddExec(work_szs, args) as CommandQueue<Kernel>);
function Kernel.Exec(work_szs: array of CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddExec(work_szs, args) as CommandQueue<Kernel>);
function Kernel.Exec(work_szs: CommandQueue<array of UIntPtr>; params args: array of CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddExec(work_szs, args) as CommandQueue<Kernel>);
function Kernel.Exec(work_szs: CommandQueue<array of integer>; params args: array of CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddExec(work_szs, args) as CommandQueue<Kernel>);

{$endregion Exec}

{$endregion Kernel}

{$endregion Неявные CommandQueue}

end.