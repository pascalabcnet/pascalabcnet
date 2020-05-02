uses GraphWPF;

const vmax = 50;

function RandomReal(a,b: real): real := Random*(b-a)+a;

type 
  BallInfo = auto class
    x,y,r,vx,vy: real;
    c: Color;
    procedure Move(dt: real) := (x,y) := (x+vx*dt,y+vy*dt);
    procedure Draw := FillCircle(x,y,r,c);
    procedure CheckDirection;
    begin
      if not x.Between(r,Window.Width-r) then 
        vx := -vx;
      if not y.Between(r,Window.Height-r) then 
        vy := -vy;
    end;
    procedure Step(dt: real);
    begin
      Move(dt); 
      CheckDirection;
      Draw;
    end;
    class function CreateRandomBallArray(n: integer): array of BallInfo;
    begin
      var rr := 25;
      Result := ArrGen(n,i->new BallInfo(RandomReal(rr,Window.Width-rr),
        RandomReal(rr,Window.Height-rr),RandomReal(5,15),
        RandomReal(-vmax,vmax),RandomReal(-vmax,vmax),RandomColor));
    end;    
  end;

begin
  Window.Title := 'Отражение шариков. Анимация на основе кадра';
  
  var n := 1000;
  var a := BallInfo.CreateRandomBallArray(n);
  
  OnDrawFrame := dt ->
    foreach var ball in a do
      ball.Step(dt);
end.