uses ABCObjects,GraphABC;

var star := new StarABC(320,240,50,20,5,Color.Green);

procedure KeyDown(key: integer);
begin
  case key of
vk_left:  star.Left -= 5;
vk_right: star.Left += 5;
vk_up:    star.Top -= 5;
vk_down:  star.Top += 5;
  end;
end;

begin
  OnKeyDown := KeyDown;
end.