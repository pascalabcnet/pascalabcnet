Uses Timers;

var t1, t2: Timer;

procedure OnTimer1;
begin
  Write('!');
end;

procedure OnTimer2;
begin
  Write('?');
end;

begin
  t1 := new Timer(200, OnTimer1);
  t2 := new Timer(300, OnTimer2);
  t1.Start;
  t2.Start;
  Sleep(10000);
end.