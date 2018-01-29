uses GraphWPF;

begin
  Window.Title := 'Система координат';
  Font.Size := 20;

  // SetMathematicCoords; // так тоже можно
  // SetMathematicCoords(-10,10); // так тоже можно
  SetMathematicCoords(-10,10,-9.2);
  DrawGrid;
 
  Print('Видимые координаты:',XMin,XMax,YMin,YMax);
  
  Polygon(Arr((-3,2),(2,1),(-2,-4)),ARGB(100,255,228,196));
  TextOut(-3,2,'A(-3,2)',Alignment.RightBottom);
  TextOut(2,1,'B(2,1)',Alignment.LeftBottom);
  TextOut(-2,-4,'C(-2,-4)',Alignment.CenterTop);
end.
