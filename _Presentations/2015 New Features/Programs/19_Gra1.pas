uses GraphABC;

begin
  Window.Title := 'Прозрачность и рисунок';
  Brush.Color := Color.BlueViolet;
  Rectangle(50,50,200,150);
  Brush.Color := ARGB(128,50,200,0);
  Circle(200,130,50);
  
  Draw('Dog.png',300,50,0.5)
end.