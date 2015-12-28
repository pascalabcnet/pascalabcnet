// Клонирование графических объектов
uses ABCObjects,GraphABC;

var bt: TextABC;
  
begin
  var x := 224;
  bt := new TextABC(60,110,110,'Hello!',RGB(x,x,x));
  while x>32 do
  begin
    Sleep(40);
    x -= 32;
    bt := bt.Clone;
    bt.Color := RGB(x,x,x);
    bt.MoveOn(7,7);
  end;
end.
