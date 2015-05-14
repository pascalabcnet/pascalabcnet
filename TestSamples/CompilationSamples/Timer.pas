uses System;

var Timer:System.Timers.Timer;
    x:integer;
    exit:boolean;

procedure OnTimer1(sender:object; e:System.Timers.ElapsedEventArgs);
var i:integer;
begin
  writeln('xsxx');
  i:=i div i;
end;

begin
  writeln('1');
  Exit:=false;
  writeln('1');
  Timer:=new System.Timers.Timer(1000);
  writeln('1');
  Timer.Elapsed+=OnTimer1;
  writeln('1');
  Timer.Start;
  writeln('1');
  while not exit do 
    Sleep(Round(Timer.Interval));
end.