Uses Timers;

var t: Timer;

procedure OnTimer;
begin
 Writeln('!');
 t.Interval := t.Interval * 2;
end;

begin
  t:=Timer.Create(10, OnTimer);
  t.Start;
  while true do sleep(1000);
end.