// Оператор выбора. Случайная фигура
uses GraphABC;

begin
  SetWindowSize(400,300);
  var t := Random(1,5);
  case t of
    1: Circle(200,150,100);
    2: Rectangle(100,100,300,200);
    3: Ellipse(100,100,300,200);
    4: RoundRect(100,100,300,200,20,20);
    5: Line(100,100,300,200);
  end;
end.