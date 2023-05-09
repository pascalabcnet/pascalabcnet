uses TurtleWPF;

begin
  Down;
  SetSpeed(11);
  SetColor(Colors.Red);
  for var i:=1 to 450 do
  begin
    SetColor(RGB(128+i,0,i));
    Forw(i);
    Turn(96);
  end;
end.
