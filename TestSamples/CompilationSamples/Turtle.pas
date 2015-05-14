unit Turtle;

interface 

uses GraphABC;

procedure PenUp;

procedure PenDown;

procedure ToPoint (x,y: real);

procedure OnVector (dx,dy: real);

procedure Forw (r: real);

procedure Turn (dphi: real);

procedure Center;

procedure Init;

function TurtleX: real;

function TurtleY: real;

function TurtlePhi: real;

procedure SetTurtleX(x: real);

procedure SetTurtleY(y: real);

procedure SetTurtlePhi(phi: real);

implementation

var
  _up: boolean;
  _xx,_yy,_phi: real;
                
procedure PenUp;
begin
  _up:=True;
end;

procedure PenDown;
begin
  _up:=False;
end;

procedure ToPoint (x,y: real);
begin
  if not _up then Line(Round(x),Round(y),Round(_xx),Round(_yy));
  _xx:=x;
  _yy:=y;
end;

procedure OnVector (dx,dy: real);
begin
  ToPoint(_xx+dx,_yy+dy);
end;

procedure Forw (r: real);
begin
  OnVector(r*cos(_phi*Pi/180),r*sin(_phi*Pi/180));
end;

procedure Turn (dphi: real);
begin
  _phi:=_phi-dphi;
end;

procedure Center;
begin
  ToPoint(WindowWidth div 2,WindowHeight div 2);
end;

procedure Init;
begin
  PenUp;
  Center;
end;

function TurtleX: real;
begin
  TurtleX:=_xx
end;

function TurtleY: real;
begin
  TurtleY:=_yy
end;

function TurtlePhi: real;
begin
  TurtlePhi:=_Phi
end;

procedure SetTurtleX(x: real);
begin
  _xx:=x
end;

procedure SetTurtleY(y: real);
begin
  _yy:=y
end;

procedure SetTurtlePhi(phi: real);
begin
  _phi:=phi
end;

end.
