unit DMTasks;

interface

uses DMTaskMaker;

implementation

procedure FirstDM;
var i,a: integer;
begin
  TaskText('Задание mydm1. Начертите, используя цикл');
  Field(14,8);
  DoToPoint(7,7);
  a:=6;
  for i:=1 to 6 do
  begin
    DoPenDown;
    DoOnVector(a,-a);
    DoOnVector(-a,a);
    DoOnVector(-a,-a);
    DoOnVector(a,a);
    Dec(a);
    DoPenUp;
    DoOnVector(0,-1);
  end;
end;

procedure DoCross;
begin
  DoPenDown;
  DoOnVector(1,0); DoOnVector(0,-1);
  DoOnVector(1,0); DoOnVector(0,-1);
  DoOnVector(-1,0); DoOnVector(0,-1);
  DoOnVector(-1,0); DoOnVector(0,1);
  DoOnVector(-1,0); DoOnVector(0,1);
  DoOnVector(1,0); DoOnVector(0,1);
  DoPenUp;
end;

procedure SecondDM;
begin
  TaskText('Задание mydm2. Начертите, используя процедуру Cross');
  Field(18,12);
  DoToPoint(3,8);
  DoCross;
  DoToPoint(8,4);
  DoCross;
  DoToPoint(12,11);
  DoCross;
  DoToPoint(15,6);
  DoCross;
end;

begin
  RegisterGroup('mydm','Мои задания для Чертежника','DMTasks',2);
  RegisterTask('mydm1',FirstDM);
  RegisterTask('mydm2',SecondDM);
end.

