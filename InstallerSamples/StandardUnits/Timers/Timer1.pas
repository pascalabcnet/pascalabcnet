uses Timers;

procedure OnTimer1 := Write('!');

procedure OnTimer2 := Write('?');

begin
  var t1 := new Timer(200, OnTimer1);
  var t2 := new Timer(300, OnTimer2);
  t1.Start;
  t2.Start;
  Sleep(10000);
end.