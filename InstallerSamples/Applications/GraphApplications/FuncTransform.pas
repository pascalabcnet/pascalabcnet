uses
  GraphWPF;

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
  Window.Clear;
  DrawGraph(f, -boundx, boundx, -boundy, boundy); 
  Window.Title := Format('mx={0:f2}  my={1:f2}  dx={2:f2}  dy={3:f2}', mx, my, dx, dy);
end;

var ArrowKeys := HSet(Key.Left, Key.Right, Key.Up, Key.Down, Key.Home, Key.&End, Key.PageUp, Key.PageDown);

procedure KeyDown(k: Key);
begin
  var g := Transform(f);
  case k of
    Key.Left:     my -= h;
    Key.Right:    my += h;
    Key.Up:       mx -= h;
    Key.Down:     mx += h;
    Key.Home:     dx += h;
    Key.PageUp:   dx -= h;
    Key.PageDown: dy += h;
    Key.End:      dy -= h;
  end;
  if k in ArrowKeys then
    DrawGraphic(g);
end;

begin
  DrawGraphic(Transform(f));
  OnKeyDown := KeyDown;
end.