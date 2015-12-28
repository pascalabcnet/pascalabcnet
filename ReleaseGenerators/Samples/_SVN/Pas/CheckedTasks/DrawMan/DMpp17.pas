// Выполнение задания pp17 для Чертежника
uses Drawman;

procedure Spir(n: integer);
begin
  PenDown;
  var i := n;
  while i>=1 do
  begin
    OnVector(0,i);
    OnVector(i,0);
    i -= 1;
    OnVector(0,-i);
    OnVector(-i,0);
    i -= 1;
  end;
  PenUp;
end;

begin
  Task('pp17');
  ToPoint(2,2);
  Spir(10);
  ToPoint(16,2);
  Spir(8);
  ToPoint(0,0);
end.
