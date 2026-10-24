uses Drawman, DrawManField, GraphABC, System.Windows.Forms, System.Threading;

var startButton, stepButton: Button;
var stepLabel: &Label;
var completed := new ManualResetEvent(False);

procedure FindControls(parent: Control);
begin
  foreach var child: Control in parent.Controls do
  begin
    if child is Button then
    begin
      if child.Text.StartsWith('Пуск (') then startButton := Button(child);
      if child.Text.StartsWith('Шаг (') then stepButton := Button(child);
    end;
    if (child is &Label) and child.Text.StartsWith('Шаг: ') then
      stepLabel := &Label(child);
    FindControls(child);
  end;
end;

procedure UI(action: procedure) := GraphABC.MainForm.Invoke(action);

procedure Require(value: boolean; message: string);
begin
  if not value then raise new System.Exception(message);
end;

function Count: integer;
begin
  var n := 0;
  UI(procedure -> n := integer.Parse(stepLabel.Text.Substring(5)));
  Result := n;
end;

procedure WaitCount(n: integer);
begin
  var timer := System.Diagnostics.Stopwatch.StartNew;
  while (Count < n) and (timer.ElapsedMilliseconds < 5000) do Thread.Sleep(10);
  Require(Count = n, 'unexpected command count');
end;

procedure Drive;
begin
  try
    var ready := False;
    var timer := System.Diagnostics.Stopwatch.StartNew;
    while not ready and (timer.ElapsedMilliseconds < 10000) do
    begin
      UI(procedure ->
      begin
        FindControls(GraphABC.MainForm);
        ready := (stepButton <> nil) and stepButton.Enabled;
      end);
      Thread.Sleep(10);
    end;
    Require(ready, 'initial pause did not become ready');
    var initial := Count;
    Thread.Sleep(150);
    Require(Count = initial, 'program moved before Start');
    if System.Environment.GetCommandLineArgs.Last = 'close' then
    begin
      System.Console.WriteLine('PASS Drawman UI: close while paused');
      UI(procedure -> GraphABC.MainForm.Close);
      raise new System.Exception('closing the window did not terminate the program');
    end;
    for var i := 1 to 4 do
    begin
      UI(procedure -> stepButton.PerformClick);
      WaitCount(initial + i);
      Thread.Sleep(150);
      Require(Count = initial + i, 'Step executed more than one command');
    end;
    UI(procedure -> startButton.PerformClick);
    timer.Restart;
    while (Count < initial + 5) and (timer.ElapsedMilliseconds < 5000) do Thread.Sleep(10);
    Require(Count >= initial + 5, 'Start did not resume');
    UI(procedure -> startButton.PerformClick);
    Thread.Sleep(150); // an operation already in progress may finish
    var paused := Count;
    Thread.Sleep(200);
    Require(Count = paused, 'Stop did not pause');
    UI(procedure -> stepButton.PerformClick);
    WaitCount(paused + 1);
    Thread.Sleep(150);
    Require(Count = paused + 1, 'Step after Stop executed twice');
    UI(procedure -> startButton.PerformClick);
    Require(completed.WaitOne(10000), 'program did not reach the error test');
    // The next field starts paused. One step attempts to leave its bounds.
    ready := False;
    timer.Restart;
    while not ready and (timer.ElapsedMilliseconds < 5000) do
    begin
      UI(procedure -> ready := stepButton.Enabled);
      Thread.Sleep(10);
    end;
    Require(ready, 'second field did not pause');
    UI(procedure -> stepButton.PerformClick);
    var failed := False;
    timer.Restart;
    while not failed and (timer.ElapsedMilliseconds < 5000) do
    begin
      UI(procedure -> failed := not startButton.Enabled and not stepButton.Enabled);
      Thread.Sleep(10);
    end;
    Require(failed, 'bounds error did not disable execution');
    System.Console.WriteLine('PASS Drawman UI: initial pause, four commands stepped, run, pause, step after pause, resume, bounds error');
    System.Environment.Exit(0);
  except
    on e: System.Exception do
    begin
      System.Console.WriteLine(e.ToString);
      System.Environment.Exit(1);
    end;
  end;
end;

begin
  var driver := new Thread(Drive);
  driver.IsBackground := True;
  driver.Start;
  Field(5,5);
  Speed(6);
  PenDown;
  ToPoint(1,1);
  OnVector(1,0);
  PenUp;
  loop 30 do
  begin
    ToPoint(1,1);
    OnVector(1,0);
  end;
  completed.Set;
  Field(1,1);
  ToPoint(100,100);
  raise new System.Exception('bounds error allowed program to continue');
end.
