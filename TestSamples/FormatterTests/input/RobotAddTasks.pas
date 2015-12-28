unit RobotAddTasks;

uses Robot,RobotTaskMaker;

procedure my1;
begin
  TaskText('Задание my1. Закрасить помеченные клетки');
  Field(15,11);
  RobotBeginEnd(1,1,15,11);
  Tag(8,6);
  HorizontalWall(3,5,5);
  HorizontalWall(3,6,5);
  VerticalWall(8,5,1);
end;

begin
  RegisterTask('my1',my1);
end.
