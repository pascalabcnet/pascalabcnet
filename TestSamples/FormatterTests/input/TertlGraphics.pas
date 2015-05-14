uses GraphABC, System.Collections.Generic;

const size=10;


type
  State = class    
    x,y,a: real;
    constructor(x,y,a: real);
    begin
      self.x := x;
      self.y := y;
      self.a := a;
    end;
  end;

function LSystem(axiom,newf,newx,newy,newb: string; level:integer): string;
//axiom - слово инициализаци
//newf  - порождающее правило
//newb  - порождающее правило
//level - число итераций
var T,W:string;
begin
  W := axiom;
  while level>0 do begin
    foreach c:char in W do
    case c of
      '+','-','[',']': T := T + c;
      'F': T := T + newf;
      'X': T := T + newx;
      'Y': T := T + newy;
      'b': T := T + newb;
      else raise new Exception('Неожиданный символ '+c);
    end;
    level := level - 1;
  end;
  result := T;
end;


procedure TertlGraphics(prog: string; x0,y0,a,b:real);
var x,y:real;
    StateStack: Queue<State>; 
begin
  StateStack := new Queue<State>;
  foreach c:char in prog do begin
    case c of
      '+': a := a + b;
      '-': a := a - b;
      'F','X','Y': begin
             x := x0 + cos(a)*size; 
             y := y0 + sin(a)*size;
             Line(Round(x0), Round(y0), Round(x), Round(y), clGreen);
           end;
      'b': begin
             x0 := x0 + cos(a)*size;
             y0 := y0 + sin(a)*size;
           end;
      '[': StateStack.Enqueue(new State(x0,y0,a));
      ']': begin
             x0 := StateStack.Peek.x;
             y0 := StateStack.Peek.y;
             a := StateStack.Peek.a;
             StateStack.Dequeue;
           end;
      else raise new Exception('Неожиданный символ '+c);
    end;
  end;
end;

begin
  InitWindow(200,100,640,480,clBlack);
  TertlGraphics(LSystem('FX','F','X+YF+','-FX-Y','',100),100,100,0,pi/2);
end.