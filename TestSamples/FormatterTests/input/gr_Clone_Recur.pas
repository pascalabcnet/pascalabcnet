uses GraphABC,ABCObjects,Events;

var
  g: ObjectABC;

procedure Four(var g: ObjectABC);
var
  F: ContainerABC;
  w: integer;
begin
  w:=8*g.Width div 7;
  f:=ContainerABC.Create(0,0);
  f.Add(g);
  g:=g.Clone;;
  g.moveon(w,0);
  g:=g.Clone;
  g.moveon(0,w);
  g:=g.Clone;
  g.moveon(-w,0);
  g:=f;
end;

begin
  cls;
  LockDrawingObjects;
  SetWindowSize(630,630);
  g:=SquareABC.Create(0,0,14,clYellow);
//  g.Text:='3';

  Four(g);
  Four(g);
  Four(g);
  Four(g);
  Four(g);
  UnLockDrawingObjects;
end.
