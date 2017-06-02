// Пример из пакета KuMir/PMir
program Dragon;

uses GraphABC,Utils;

var
  x := 200;
  y := 150;
  dx := 0;
  dy := -4;
  turn: array [1..1000] of Boolean;

begin
  SetWindowSize(790,500);
  Window.Title := 'Кривая Дракона';
  var f := True;
  for var a := 1 to 64 do
  begin
    turn[2*a-1] := f;
    f := not f;
    turn[2*a] := turn[a];
  end;
  var b := 0;
  var d := 1;
  f := false;
  MoveTo(x,y);
  
  for var a:=1 to 128 do
  begin
    var t: integer;
    LockDrawing;
    for var i:=1 to 127*4 do
    begin
      b += d; 
      x += dx; 
      y += dy;
      LineTo(x,y);
      if f and not turn[b] or not f and turn[b] then
      begin
        t := dy;
        dy := -dx;
      end
      else
      begin
        t := -dy;
        dy := dx;
      end;
      dx := t;
    end;
    b += d; 
    x += dx; 
    y += dy;
    LineTo(x,y);
    d := -d;
    f := not f;
    if turn[a] then
    begin
      t := dy;
      dy := -dx;
    end
    else
    begin
      t := -dy;
      dy := dx;
    end;
    dx := t;
    UnLockDrawing;
  end;
  write('Время работы = ',Milliseconds/1000,' с');
end.
