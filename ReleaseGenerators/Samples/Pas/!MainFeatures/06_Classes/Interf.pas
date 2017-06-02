// Иллюстрация использования интерфейсов
type
  IShape = interface
    procedure Draw;
    property X: integer read;
    property Y: integer read;
  end;

  ICloneable = interface
    function Clone: Object;
  end;

  Point = class(IShape,ICloneable)
  private
    xx,yy: integer;
  public
    constructor Create(x,y: integer);
    begin
      xx := x; yy := y;
    end;  
    procedure Draw; begin end;
    property X: integer read xx;
    property Y: integer read yy;
    function Clone: Object;
    begin
      Result := new Point(xx,yy);
    end;
  end;

var 
  p: Point := new Point(2,3);
  ish: IShape := p;
  icl: ICloneable := p;
  
begin
  Println(ish.X,ish.Y);
  var p1: Point := Point(icl.Clone);
  p := nil;
  Println(p1.X,p1.Y);
  Println(ish is Point);
  Println(ish is ICloneable); // Cross cast!
end.