uses TurtleABC;

begin
  Mark;
  SetScale(4);
  SetOrigin(5,-35);
  //SetOrigin(-20,-50);
  Mark; 
  SetColor(Color.Blue);
  Down;
  
  loop 4 do
  begin
    Forw(20);
    Turn(90);
  end;
  
  Up;
  MoveTo(-30, 30);
  Down;
  SetColor(Color.Red);
  
  Mark;
  loop 3 do
  begin
    Forw(15);
    Turn(120);
  end;
  
  Up;
  MoveTo(30, 40);
  Down;
  SetColor(Color.Green);
  
  Mark;
  loop 20 do
  begin
    Forw(30);
    Turn(100);
  end;
  
  Up;
  MoveTo(-30, 90);
  Mark;
  Down;
  SetColor(Color.Orange);
  
  loop 5 do
  begin
    Forw(30);
    Turn(144);
  end;
  
  DrawCoordPoints;
end.