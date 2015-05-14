uses ABCObjects,GraphABC;

type TClass = class
end;

var
  bt: TextABC;
  x: integer;
  
begin
  x:=224;
  bt:=new TextABC(60,110,110,'Hello!',RGB(x,x,x));
  while x>32 do
  begin
    Sleep(40);
    Dec(x,32);
    bt:=TextABC(bt.Clone);
    bt.Color:=RGB(x,x,x);
    bt.MoveOn(7,7);
  end;
end.
