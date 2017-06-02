// Класс штампа ряда прямоугольников
uses GraphABC;

type 
  RectangleStamp = auto class
    x,y,w,h: integer;
    procedure Stamp;
    begin
      Rectangle(x,y,x+w,y+h);
    end;
  end;
  
  RowRectanglesStamp = auto class
    x,y,w,h,n: integer;
    procedure Stamp;
    begin
      var r := new RectangleStamp(x,y,w,h);
      r.Stamp;
      for var i:=1 to n-1 do
      begin
        r.x += r.w + 5;
        r.Stamp;
      end;
    end;
  end;  

const n=8;

begin
  var r := new RowRectanglesStamp(30,30,50,50,n);
  r.Stamp;
  for var i:=1 to n-1 do
  begin
    r.y += r.h + 5;
    r.Stamp;
  end;
end. 