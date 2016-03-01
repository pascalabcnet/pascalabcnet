// Демонстрация использования случайных чисел для Чертежника. Лучи
uses DrawMan;

begin
  Field(30,22);
  ToPoint(15,11);
  PenDown;
  for var i:=1 to 100 do
  begin
    var dx := Random(-10,10);
    var dy := Random(-10,10);
    OnVector(dx,dy);
    OnVector(-dx,-dy);
  end;
  PenUp;
end.
