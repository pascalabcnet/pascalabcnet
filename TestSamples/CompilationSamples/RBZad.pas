unit RBZad;

uses uRobot;

procedure c1;
begin
  InitField(10,8,40,2,2);
  hw(0,1,5);
  tagrect(2,1,5,1);
//  tagPainted(4,4);
  DrawField;
  SetRobotCoords(2,2);
  drawRobot;
end;

begin
end.
