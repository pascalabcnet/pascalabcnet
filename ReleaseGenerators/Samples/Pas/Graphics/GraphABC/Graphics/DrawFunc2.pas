uses GraphABC;

function System.Drawing.Rectangle.Scale(m: real): System.Drawing.Rectangle;
begin
  Result := Self;
  Result.Width := Trunc(Result.Width * m);
  Result.Height := Trunc(Result.Height * m)
end;

function System.Drawing.Rectangle.Move(dx,dy: integer): System.Drawing.Rectangle;
begin
  Result := Self;
  Result.X := Result.X + dx;
  Result.Y := Result.Y + dy;
end;

begin
  var r := ClientRectangle;
  r := r.Scale(0.5);
  var r1 := r.Move(r.Width,0);
  var r2 := r.Move(0,r.Height);
  var r3 := r.Move(r.Width,r.Height);
  Draw(x->x*sin(x),-20,20,r);
  Draw(sin,r1);
  Draw(cos,r2);
  Draw(exp,20,10,r3);
end.