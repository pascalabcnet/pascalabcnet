unit RobTasks;

interface

uses RobotTaskMaker;

implementation

procedure FirstRob;
begin
  TaskText('Задание myrob1. Закрасить помеченные клетки');
  Field(10,6);
  HorizontalWall(0,3,4);
  VerticalWall(4,3,2);
  RobotBegin(1,4);
  VerticalWall(5,1,5);
  HorizontalWall(5,1,4);
  RobotEnd(6,2);
  Tag(6,2);
end;

procedure SecondRob;
var n,i: integer;
begin
  TaskText('Задание myrob2. Закрасить клетки под закрашенными');
  n:=Random(4)+7;
  Field(n,4);
  RobotBeginEnd(1,3,n,3);
  MarkPainted(n,2);
  Tag(n,3);
  for i:=2 to n-1 do
    if Random(3)=1 then
    begin
      MarkPainted(i,2);
      Tag(i,3);
    end;
end;

begin
  RegisterTask('myrob1',FirstRob);
  RegisterTask('myrob2',SecondRob);
  RegisterGroup('myrob', 'Мои задания для Робота', 'RobTasks', 2);
end.

