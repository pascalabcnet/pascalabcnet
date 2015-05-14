uses System, System.Threading.Tasks, System.Collections.Generic;

type IntTask = Task<integer>;

function longoperation1: integer;
begin
  var i := 0;
  while i < 1000000 do
    Inc(i);
  Result := i;
end;

function longoperation2: integer;
begin
  var i := 0;
  while i < 3000000 do
    Inc(i);
  Result := i;
end;

begin
  {var task1 := IntTask.Factory.StartNew(()->longoperation1());
  var task2 := IntTask.Factory.StartNew(()->longoperation2());
  task1.Wait();
  task2.Wait();
  assert(task1.Result = 1000000);
  assert(task2.Result = 3000000);}
end.