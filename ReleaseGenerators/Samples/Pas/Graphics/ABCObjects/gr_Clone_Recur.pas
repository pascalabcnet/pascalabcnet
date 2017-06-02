// Клонирование графических объектов. 
// Контейнер графических объектов. Вложенные контейнеры
uses GraphABC,ABCObjects;

/// Создание четырех графических объектов из одного
procedure Four(var g: ObjectABC);
begin
  var w := 8*g.Width div 7;
  var f := ContainerABC.Create(0,0);
  f.Add(g);
  g := g.Clone;;
  g.moveon(w,0);
  g := g.Clone;
  g.moveon(0,w);
  g := g.Clone;
  g.moveon(-w,0);
  g := f;
end;

begin
  LockDrawingObjects;
  SetWindowSize(630,630);
  
  var g: ObjectABC := new SquareABC(0,0,14,clYellow);
  Four(g);
  Four(g);
  Four(g);
  Four(g);
  Four(g);
  UnLockDrawingObjects;
end.
