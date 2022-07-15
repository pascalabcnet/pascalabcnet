// Базовый модуль-заглушка. Переопределяется в каталогах решаемых заданий
unit Tasks;

uses LightPT;

procedure CheckTaskT(name: string);
begin
  TaskResult := NotUnderControl;
  WriteInfoToDatabase(name, TaskResult);
end;

initialization
  CheckTask := CheckTaskT;
finalization
end.