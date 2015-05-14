// Рекурсивное рисование двоичного дерева поиска
unit u_RandomSearchTree;

uses GraphABC;

type
  PNode=^TNode;
  TNode=record
    data: integer;
    left,right: PNode;
  end;
  
function CreateNode(d: integer; l,r: PNode): PNode;
begin
  New(Result);
  Result^.data:=d;
  Result^.left:=l;
  Result^.right:=r;
end;

procedure Add(var t: PNode; d: integer);
begin
  if t=nil then
    t:=CreateNode(d,nil,nil)
  else if t^.data>d then
    Add(t^.left,d)
  else if t^.data<d then
    Add(t^.right,d)
end;

const dy=60;

procedure DrawTree(t: PNode; x,y,dx: integer);
begin
  if t=nil then exit;
  Line(x,y+16,x-dx,y+dy);
  Line(x,y+16,x+dx,y+dy);
  TextOut(x-3,y,IntToStr(t^.data));
  DrawTree(t^.left,x-dx,y+dy,dx div 2);
  DrawTree(t^.right,x+dx,y+dy,dx div 2);
end;

var
  t: PNode;
  i: integer;
  
begin
  for i:=1 to 30 do
    Add(t,Random(100));
  SetWindowCaption('Двоичное дерево поиска');
  DrawTree(t,WindowWidth div 2,10,WindowWidth div 4);
end.
