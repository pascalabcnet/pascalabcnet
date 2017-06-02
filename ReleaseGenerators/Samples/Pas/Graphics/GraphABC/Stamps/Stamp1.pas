// Штампы - это классы графических фигур, хранящие их параметры
// В любой момент можно нарисовать графическую фигуру, вызвав метод Stamp.

// Класс штампа прямоугольника
uses GraphABC;

type 
  RectangleStamp = auto class
    x,y,w,h: integer;
    procedure Stamp;
    begin
      Rectangle(x,y,x+w,y+h);
    end;
  end;

begin
  var r := new RectangleStamp(30,30,50,50);
  r.Stamp;
  for var i:=1 to 10 do
  begin
    r.x := r.x + r.w +5;
    r.Stamp;
  end;
end. 