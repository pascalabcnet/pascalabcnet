// Базовый модуль-заглушка. Переопределяется в каталогах решаемых заданий
unit Tasks;

uses LightPT;

function CheckTaskT(name: string): TaskStatus;
begin
  Result := AbsentTask;
  WriteInfoToDatabase(name, result);
end;

initialization
  CheckTask := CheckTaskT;
finalization
end.