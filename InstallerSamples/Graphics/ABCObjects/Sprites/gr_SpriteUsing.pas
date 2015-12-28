// Переключение состояний спрайта щелчком мыши
uses GraphABC,ABCSprites,ABCObjects,Events;

var
  s: SpriteABC;
  t: TextABC;

procedure MyMouseDown(x,y,mb: integer);
begin
  if s.PtInside(x,y) then
  begin
    // Переход к следующему состоянию спрайта
    if s.State<s.StateCount then
      s.State := s.State + 1
    else s.State := 1;
    t.Text := 'Состояние спрайта: ' + s.StateName;
  end;
end;
  
begin
  Window.Title := 'Щелкните мышью на спрайте';
  SetWindowSize(400,300);
  CenterWindow;
  s := new SpriteABC(150,100,'spr.spinf');
  t := new TextABC(55,30,15,'Состояние спрайта: '+s.StateName,clRed);
  OnMouseDown := MyMouseDown;
end.