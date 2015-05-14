// Блуждание мячиков по полю
// Работа с модулем CRT, наследование, полиморфизм
         
uses CRT;

const Speed=0.0001;
      CallHide=true;
      TextBackgroundColor=Black;
      KRnd=0.1;
      Drndx=0.5;
	  Drndy=0.7;

      RandomAdd=true;
      NormalBalls=10;
      RandomBalls=0;
	  MaxBall=NormalBalls+RandomBalls;
      
type 
  Ball=class
  public
    x,y:real;
    xn,yn:integer;
    Color:integer;
    procedure Show;
    begin
      TextColor(Color);
      xn:=trunc(x);yn:=trunc(y);
      GotoXY(xn,yn);
      write('o');
    end;
    procedure Hide;
    begin
      GotoXY(trunc(X),trunc(Y));
      write(' ');
    end;
    procedure MoveTo(_x,_y:real);
    begin
      if (trunc(_x)<>xn)or(trunc(_y)<>yn) then begin
        if (CallHide)then Hide;
        x:=_x;y:=_y;
        Show;
      end else begin
        x:=_x;y:=_y;
      end;
    end;
    constructor Create(_x,_y:real;_color:integer);
    begin
      x:=_x;y:=_y;
      xn:=trunc(x);yn:=trunc(y);
      Color:=_color;
    end;
    procedure NextStep;virtual;
    begin
      xn:=-1;yn:=-1;
      Show;
    end;
  end;
  
  NormalBall=class(Ball)
  public
    dx,dy:real;
    constructor Create(_x,_y,_dx,_dy:real;_color:integer);
    begin
      inherited Create(_x, _y ,_color);
      dx:=_dx*Speed;dy:=_dy*Speed;
      xn:=trunc(x);yn:=trunc(y);
    end;
    procedure NextStep;override;
    begin
      if (x+dx<0)or(x+dx>WindowWidth-1) then dx:=-dx else dx:=dx+(DRndx-random)*krnd*Speed;
      if (y+dy<0)or(y+dy>WindowHeight-1)then dy:=-dy else dy:=dy+(DRndy-random)*krnd*Speed;
      MoveTo(x+dx,y+dy);
    end;
  end;
  
  RandomBall=class(NormalBall)
    d:real;
  public
    procedure SetRandomD(var _d:real);
    begin
      if _d=0 then _d:=1;
	 _d:=-sign(_d)*d*Random;
    end;
    constructor Create(_x,_y,_d:real;_color:integer);
    begin
      inherited Create(_x,_y,_d,0,_color);		
      dx:=0;
      dy:=0;
      d:=_d*Speed;
      SetRandomD(dx);
      SetRandomD(dy);
      xn:=trunc(x);yn:=trunc(y);
    end;
    procedure NextStep;override;
    begin
      if (x+dx<0)or(x+dx>WindowWidth-1) then 
        SetRandomD(dx);
      if (y+dy<0)or(y+dy>WindowHeight-1)then 
        SetRandomD(dy);
      MoveTo(x+dx,y+dy);
    end;
  end;

  BallsContainer=class
    Balls:array[0..maxball] of Ball;
  public
    Count:integer;
    procedure Add(b:Ball);
    begin
      Balls[Count]:=b;
      Count:=Count+1;
    end;
    procedure NextStep;				
    var i:integer;
        b:ball;
    begin
      for i:=0 to Count-1 do begin
        b:=Balls[i];b.NextStep;
        //Balls[i].NextStep;			
      end;
    end;
  end;

var Balls:BallsContainer;
    i:integer;

begin
  TextBackground(TextBackgroundColor);
  //setwindowsize(100,100);
  ClrScr;
  HideCursor;
  Balls:=new BallsContainer;
  if RandomAdd then begin
    for i:=1 to NormalBalls do
      Balls.Add(new NormalBall(WindowWidth/2,WindowHeight/2, 0.5-Random , 0.5-Random, Random(15)));
    for i:=1 to RandomBalls do
      Balls.Add(new RandomBall(WindowWidth/2,WindowHeight/2, Random , Random(15)));
  end else begin
    Balls.Add(new NormalBall(10,10, 0.5 , 0.1,LightRed));
    Balls.Add(new NormalBall(50,20,-0.2 ,-0.7,LightGreen));
    Balls.Add(new NormalBall(70,15, 0.4 ,-0.3,LightBlue));
    Balls.Add(new RandomBall(WindowWidth/2,WindowHeight/2,1,White));
  end;
  while (not KeyPressed()) do
    Balls.NextStep;{}
  ReadKey;
end.
