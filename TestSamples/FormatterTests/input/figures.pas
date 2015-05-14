// Демонстрация полиморфизма. 
// Многократное вложение контейнеров друг в друга.
// Клонирование объектов.
uses GraphABC;

type
  Figure=class
    x,y: integer;
    constructor Create(xx,yy: integer);
    begin
      x:=xx;
      y:=yy;
    end;
    procedure Draw;virtual;
    begin
    end;
    procedure Show;
    begin
      SetPenColor(clBlack);
      Draw;
    end;
    procedure Destroy;
    begin
    end;
    procedure Hide;virtual;
    begin
      SetPenColor(clWhite);
      Draw;
    end;
    procedure SetCoords(xx,yy: integer);
    begin
      x:=xx;
      y:=yy;
    end;
    procedure MoveCoords(dx,dy: integer);
    begin
      x:=x+dx;
      y:=y+dy;
    end;
    procedure MoveTo(xx,yy: integer);
    begin
      Hide;
      SetCoords(xx,yy);
      Show;
    end;
    procedure MoveOn(dx,dy: integer);
    begin
      MoveTo(x+dx,y+dy);
    end;
    function Clone: Figure;virtual;
    begin
      Clone:=Figure.Create(x,y);
    end;
  end;
  
  PointG=class(Figure)
    constructor Create(x1, y1 : integer);
    begin
      x := x1; y := y1;
    end;
    procedure Draw;override;
    begin
      SetPixel(x,y,clBlack)
    end;
    function Clone: Figure;override;
    begin
      Clone:=PointG.Create(x,y);
    end;
  end;
  
  RectG=class(Figure)
    w,h: integer;
    constructor Create(x1,y1,x2,y2: integer);
    begin
      //inherited Create(x1,y1);
      x := x1; y := y1;
      w:=x2-x1;
      h:=y2-y1;
    end;
    procedure Draw;override;
    begin
      Rectangle(x,y,x+w,y+h);
    end;
    function Clone: Figure;override;
    begin
      Clone:=RectG.Create(x,y,x+w,y+h);
    end;
  end;
  
  CircleG=class(Figure)
    r: integer;
    constructor Create(x1,y1,rr: integer);
    begin
      //inherited Create(x,y);
      x := x1; y := y1;
      r:=rr;
    end;
    procedure Draw;override;
    begin
      Ellipse(x-r,y-r,x+r,y+r);
    end;
    function Clone: Figure;override;
    begin
      Clone:=CircleG.Create(x,y,r);
    end;
  end;

  FigureContainer=class(Figure)
    A: array [1..100] of Figure;
    sz: integer;
    constructor Create;
    begin
      //inherited Create(0,0);
      x := 0; y := 0;
	sz:=0;
    end;
    procedure Destroy;
    var i: integer;
    begin
      for i:=1 to sz do
        A[i].Destroy;
    end;
    procedure Add(f: Figure);
    begin
      sz := sz + 1;
      A[sz]:=f;
    end;
    procedure Draw;override;
    var i: integer;
    begin
      for i:=1 to sz do
      begin
        A[i].MoveCoords(x,y);
        A[i].Draw;
        A[i].MoveCoords(-x,-y);
      end;
    end;
    function Clone: Figure;override;
    var
      v: FigureContainer;
      i: integer;
    begin
      v:=FigureContainer.Create;
      v.SetCoords(x,y);
      for i:=1 to sz do
        v.Add(A[i].Clone);
      Clone:=v;
    end;
  end;

var
  FC: FigureContainer;
  Glaz: FigureContainer;
  FC1,Glaz1: Figure;
  
begin
  
  SetWindowSize(800,400);
 SetWindowCaption('Фигуры');

  Glaz:=FigureContainer.Create;
  Glaz.Add(RectG.Create(0,0,70,70));
  Glaz.Add(CircleG.Create(35,35,4));
  Glaz.Add(PointG.Create(35,35));
  Glaz.SetCoords(110,110);

  Glaz1:=Glaz.Clone;
  Glaz1.MoveCoords(120,0);

  FC:=FigureContainer.Create;
  FC.Add(CircleG.Create(200,200,200));
  FC.Add(Glaz);
  FC.Add(Glaz1);
  FC.Add(RectG.Create(120,280,290,304));
  FC.Show;

  FC1:=FC.Clone;
  FC1.MoveCoords(400,0);
  FC1.Show;

//  Glaz.Destroy; - не надо, это делают FC и FC1
//  Glaz1.Destroy;
  FC.Destroy;
  FC1.Destroy;
end.
