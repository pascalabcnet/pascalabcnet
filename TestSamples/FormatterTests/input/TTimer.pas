//Тест интерфейсов и делегатов

uses
  System, System.Timers;

type
  IStarter = interface
    procedure Start;
    function get_Enabled: boolean;
    procedure set_Enabled(value: boolean);
    property Enabled: boolean read get_Enabled write set_Enabled;
  end;
  
  MyTimer = class(Timer, IStarter)
  public
    class procedure OnTimedEvent(source: object; e: ElapsedEventArgs);
    begin
      Console.WriteLine('Hello World!');
    end;
  end;
  
var
  aTimer: MyTimer;

begin
  aTimer := new MyTimer;

  // Hook up the Elapsed event for the timer.
  aTimer.Elapsed += MyTimer.OnTimedEvent;

  // Set the Interval to 1 second (1000 milliseconds).
  aTimer.Interval := 1000;

  writeln(IStarter(aTimer).Enabled);
  IStarter(aTimer).Enabled := true;
  writeln(IStarter(aTimer).Enabled);

  Console.WriteLine('Press the Enter key to exit the program.');
  Console.ReadLine();

  // Keep the timer alive until the end of Main.
  GC.KeepAlive(aTimer);
end.
