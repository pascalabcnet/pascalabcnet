uses Robot,RobotTaskMaker,RobotField;

procedure Rob2;
   const h=10;//высота поля
         w=15;//ширина поля
         step=1;//отступ помеченных клеток от края поля
   var a,i: byte;
begin
  TaskText('Закрасить клетки вокруг прямоугольника');
  Field(w,h);
  a:=Random(h div 3)+step+1;//отступ прямоуголька от края поля
  HorizontalWall(a,a,w-a*2);
  HorizontalWall(a,h-a,w-a*2);
  VerticalWall(a,a,h-a*2);
  VerticalWall(w-a,a,h-a*2);
  RobotBeginEnd(a,a,a,a);
  for i:=a to w-a+1 do Tag(i,a);
  for i:=a to w-a+1 do Tag(i,h-a+1);
  for i:=a+1 to h-a do Tag(a,i);
  for i:=a+1 to h-a do Tag(w-a+1,i);

 
  CorrectFieldBounds;
  SetTaskCall;
  Stop;
end;
begin
    rob2;
end. 