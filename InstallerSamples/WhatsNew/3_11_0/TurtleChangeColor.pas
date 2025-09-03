uses Turtle,GraphWPF;

begin
  SetWidth(5);
  ToPoint(-10,0);
  loop 70 do
  begin
    SetColor(RandomColor);
    Down;
    Forw(5);
    Up;
    Back(5);
    TurnRight(90);
    Forw(0.3);
    TurnLeft(90);
  end;
end.