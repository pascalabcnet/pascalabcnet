// Базовый модуль-заглушка. Переопределяется в каталогах решаемых заданий
unit Tasks;

uses LightPT;

procedure CheckTaskT(name: string);
begin
  TaskResult := NotUnderControl;
  WriteInfoToLocalDatabase(name, TaskResult);
end;

initialization
  CheckTask := CheckTaskT;
finalization
end.