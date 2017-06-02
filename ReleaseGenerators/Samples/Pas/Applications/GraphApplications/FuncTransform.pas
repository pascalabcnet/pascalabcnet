uses
  GraphABC;

var
  h := 0.01;
  mx := 2.0;
  my := 0.35;
  dy := 0.0;
  dx := 0.0;
  f: real -> real := x -> x * sin(x);

const
  boundx = 5;
  boundy = 3;

function Transform(f: real -> real): real -> real;
begin
  Result := x -> my * f(mx * (x + dx)) + dy;
end;

procedure DrawGraphic(f: real -> real);
begin
  Draw(f, -boundx, boundx, -boundy, boundy); 
  Window.Title := Format('mx={0:f2}  my={1:f2}  dx={2:f2}  dy={3:f2}', mx, my, dx, dy);
  Redraw;
end;

procedure KeyDown(key: integer);
const
  ArrowKeys: set of integer = [vk_Left, vk_Right, vk_Up, vk_Down, vk_Home, vk_End, vk_PageUp, vk_PageDown];
begin
  var g := Transform(f);
  case key of
    vk_Left:     my -= h;
    vk_Right:    my += h;
    vk_Up:       mx -= h;
    vk_Down:     mx += h;
    vk_Home:     dx += h;
    vk_PageUp:   dx -= h;
    vk_PageDown: dy += h;
    vk_End:      dy -= h;
  end;
  if key in ArrowKeys then
    DrawGraphic(g);
end;

begin
  DrawGraphic(Transform(f));
  LockDrawing;
  OnKeyDown := KeyDown;
end.