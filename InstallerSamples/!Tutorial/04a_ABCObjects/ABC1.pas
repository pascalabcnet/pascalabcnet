// Модуль ABCObjects. Изменение свойств объекта
uses ABCObjects,GraphABC;

const delay = 300;

procedure Pause;
begin
  Sleep(delay);
end;

var 
  r: RectangleABC;
  z: StarABC;

begin
  Window.Title := 'ABCObjects: свойства графических объектов';
  z := new StarABC(Window.Center.X,Window.Center.Y,70,30,6,Color.Green);
  r := new RectangleABC(100,100,200,100,Color.Gold);
  Pause;
  r.Center := Window.Center;
  Pause;
  r.Height := 70;
  Pause;
  r.Width := 220;
  Pause;
  z.Radius := 150;
  Pause;
  z.Color := Color.LightCoral;
  Pause;
  z.Count := 5;
  Pause;
 
  r.Text := 'PascalABC.NET';
  r.Color := Color.Gainsboro;
  Pause;
  r.BorderWidth := 3;
  r.BorderColor := Color.Blue;
  Pause;
  r.Center := Window.Center;
end.
