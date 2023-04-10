Unit Timers;

(*
    Обертка для класса System.Timers.Timer
    (c) Брагилевский В.Н. 2007

    Сохранен интерфейс класса Timer из PascalABC
    за исключением функции Handle.
*)

interface

uses System;

type
  /// Класс таймера
  Timer = class
  private
    _timer: System.Timers.Timer;
    _procedure: ()->();
    procedure SetEnabled(_enabled: boolean);
    function GetEnabled: boolean;
    procedure SetInterval(_interval: integer);
    function GetInterval: integer;
    procedure TimerHandler(sender: object; e: System.Timers.ElapsedEventArgs);
  public
    /// Создает таймер с интервалом срабатывания ms миллисекунд и обработчиком TimerProc
    constructor Create(ms: integer; TimerProc: procedure := nil); 
    /// Запускает таймер
    procedure Start;
    /// Останавливает таймер
    procedure Stop;
    /// Запущен ли таймер
    property Enabled: boolean read GetEnabled write SetEnabled;
    /// Интервал срабатывания таймера
    property Interval: integer read GetInterval write SetInterval;
    ///--
    property TimerProc: ()->() read _procedure write _procedure;
    /// Событие таймера
    property OnTimer: ()->() read _procedure write _procedure;
  end;
  
/// Создать и запустить таймер, вызывающий процедуру TimerProc каждые ms миллисекунд
function CreateTimerAndStart(ms: integer; TimerProc: procedure): Timer;

implementation

function CreateTimerAndStart(ms: integer; TimerProc: procedure): Timer;
begin
  Result := new Timer(ms,TimerProc);
  Result.Start;
end;

procedure Timer.TimerHandler(sender: object; e: System.Timers.ElapsedEventArgs);
begin
  _procedure;    
end;

constructor Timer.Create(ms: integer; TimerProc: procedure); 
begin
  _timer := new System.Timers.Timer(ms);
  _procedure := TimerProc;
  _timer.Elapsed += TimerHandler;
end;

procedure Timer.Start;
begin
  Enabled := True;
end;

procedure Timer.Stop;
begin
  Enabled := False;
end;

procedure Timer.SetEnabled(_enabled: boolean);
begin
  _timer.Enabled := _enabled;
end;

function Timer.GetEnabled: boolean;
begin
  Result := _timer.Enabled;
end;

procedure Timer.SetInterval(_interval: integer);
begin
  _timer.Interval := _interval;
end;

function Timer.GetInterval: integer;
begin
  Result := Round(_timer.Interval);
end;

end.