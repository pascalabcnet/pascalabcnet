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
    _procedure: procedure;
    procedure SetEnabled(_enabled: boolean);
    function GetEnabled: boolean;
    procedure SetInterval(_interval: integer);
    function GetInterval: integer;
    procedure OnTimer(sender: object; e: System.Timers.ElapsedEventArgs);
  public
    /// Создает таймер с интервалом срабатывания ms миллисекунд и обработчиком TimerProc
    constructor Create(ms: integer; TimerProc: procedure); 
    /// Запускает таймер
    procedure Start;
    /// Останавливает таймер
    procedure Stop;
    /// Запущен ли таймер
    property Enabled: boolean read GetEnabled write SetEnabled;
    /// Интервал срабатывания таймера
    property Interval: integer read GetInterval write SetInterval;
  end;

implementation

  procedure Timer.OnTimer(sender: object; e: System.Timers.ElapsedEventArgs);
  begin
    _procedure;    
  end;
  
  constructor Timer.Create(ms: integer; TimerProc: procedure); 
  begin
    _timer := new System.Timers.Timer(ms);
    _procedure := TimerProc;
    _timer.Elapsed += OnTimer;
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