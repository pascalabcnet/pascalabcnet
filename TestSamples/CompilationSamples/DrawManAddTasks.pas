/// Дополнительный модуль для исполнителя Чертежник с заданиями
unit DrawManAddTasks;
// После компиляции pcu-файл модуля желательно разместить в папке PascalABC.NET\bin\Lib

uses DrawMan,DMTaskMaker;

procedure my1;
begin
  TaskText('Задание my1. Начертите, используя циклы');
  Field(14,14);
  DoToPoint(1,7);
  var k := 1;
  DoPenDown;
  for var i:=1 to 12 do
  begin
    DoOnVector(1,i*k);
    k:=-k;
  end;
  DoPenUp;
end;

begin
  RegisterTask('my1',my1);
end.
