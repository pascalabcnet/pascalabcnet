//События, +=, -=, демонстрация работы таймера System.Timers.Timer
uses System;

var Timer:System.Timers.Timer;
    x:integer;
    exit:boolean;

procedure OnTimer2(sender:object; e:System.Timers.ElapsedEventArgs);
begin
  Writeln(x);
  x:=x+1;
  exit:=x>=10;
end;
procedure OnTimer1(sender:object; e:System.Timers.ElapsedEventArgs);
begin
  Writeln(e.SignalTime);
  x:=x+1;
  if x>=5 then begin
    Timer.Elapsed-=OnTimer1;
    Timer.Elapsed+=OnTimer2;
  end;
end;

begin
  Exit:=false;
  Timer:=System.Timers.Timer.Create(1000);
  Timer.Elapsed+=OnTimer1;
  Timer.Start;
  while not exit do 
    Sleep(Round(Timer.Interval));
end.