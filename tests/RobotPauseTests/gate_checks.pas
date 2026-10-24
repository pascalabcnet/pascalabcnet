var gate := new TRobotExecutionGate;
var reached := new System.Threading.AutoResetEvent(False);
var finished := new System.Threading.ManualResetEvent(False);

procedure Require(value: boolean; message: string);
begin
  if not value then
    raise new System.Exception(message);
end;

begin
  gate.Pause;
  Require(gate.IsPaused, 'pause state');
  var worker := new System.Threading.Thread(procedure ->
  begin
    for var i := 1 to 50 do
    begin
      gate.Wait(False); // sensors do not consume a step
      gate.Wait(False);
      gate.Wait(True); // one movement consumes one step
      reached.Set;
    end;
    finished.Set;
  end);
  worker.IsBackground := True;
  worker.Start;
  Require(not reached.WaitOne(100), 'initial pause must block');
  for var i := 1 to 50 do
  begin
    gate.Step;
    Require(reached.WaitOne(3000), 'step must wake worker');
    Require(not reached.WaitOne(20), 'one click must not execute two commands');
  end;
  Require(finished.WaitOne(3000), 'all steps finished');
  Require(worker.Join(3000), 'step worker exited');

  // A click arriving before the worker starts waiting must not be lost.
  gate.Step;
  gate.Wait(True);
  gate.Run;
  Require(not gate.IsPaused, 'run state');
  for var i := 1 to 100 do gate.Wait(True);
  gate.Pause;
  worker := new System.Threading.Thread(procedure ->
  begin
    gate.Wait(False);
    reached.Set;
  end);
  worker.IsBackground := True;
  worker.Start;
  Require(not reached.WaitOne(100), 'pause after run must block');
  gate.Run;
  Require(reached.WaitOne(3000), 'run must release paused worker');
  Require(worker.Join(3000), 'run worker exited');

  gate.Fail;
  gate.Step;
  gate.Run;
  worker := new System.Threading.Thread(procedure ->
  begin
    gate.Wait(True);
    reached.Set;
  end);
  worker.IsBackground := True;
  worker.Start;
  Require(not reached.WaitOne(200), 'error must not be resumed by run or step');
  Println('PASS modern robot gate: pause, sensors, 50 single steps, early click, run, terminal error');
end.
