uses GraphWPF;

function RandomReal(a,b: real): real := Random*(b-a)+a;

type 
  BallInfo = auto class
    x,y,r,dx,dy: real;
    c: Color;
    procedure Move := (x,y) := (x+dx,y+dy);
    procedure Draw := FillCircle(x,y,r,c);
    procedure CheckDirection;
    begin
      if not x.Between(r,Window.Width-r) then 
        dx := -dx;
      if not y.Between(r,Window.Height-r) then 
        dy := -dy;
    end;
    procedure Step;
    begin
      Move; 
      CheckDirection;
      Draw;
    end;
    class function CreateRandomBallArray(n: integer): array of BallInfo;
    begin
      var rr := 20;
      Result := ArrGen(n,i->new BallInfo(RandomReal(rr,Window.Width-rr),
        RandomReal(rr,Window.Height-rr),RandomReal(5,15),
        RandomReal(-3,3),RandomReal(-3,3),RandomColor));
    end;    
  end;

begin
  Window.Title := 'Отражение шариков. Анимация на основе кадра';
  
  var n := 1000;
  var a := BallInfo.CreateRandomBallArray(n);
  
  BeginFrameBasedAnimation(()->
    foreach var ball in a do
      ball.Step
  );
  
  //BeginFrameBasedAnimation(()->a.ForEach(ball->ball.Step));
end.