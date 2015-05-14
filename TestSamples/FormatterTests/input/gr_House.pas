uses GraphABC,ABCObjects,ABCHouse,Events;

var
  f: ContainerABC;
  h: HouseABC;
  g: ObjectABC;
  d: DoorABC;

begin
  CenterWindow;
  SetSmoothingOff;
  h := new HouseABC(100,70,255,150,clGray);
  h.Wall.BorderWidth := 3;
  h.Window.Color := clYellow;
  h.Door.Color := clBlue;
  h.Window.BorderWidth:=2;
  h.Door.BorderWidth:=2;
  h.Window.Width2:=5;
  h.moveon(10,0);
  h.Redraw;
  f := ContainerABC.Create(100,270);
  f.Add(RectangleABC.Create(20,0,300,80,clBrown));
  g := h.Door.Clone;
  g.moveon(0,0);
  g.Owner:=f;
 
end.
