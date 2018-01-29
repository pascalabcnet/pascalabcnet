uses GraphWPF;

procedure ВсеТочкиМногоугольника(x0,y0,r: real; n: integer);
begin
  var q := Partition(0,2*Pi,n).Select(a->Pnt(x0 + r * Cos(a), y0 - r * Sin(a)));
  q.Cartesian(q).ForEach(p->Line(p[0].x,p[0].y,p[1].x,p[1].y,RandomColor));
end;

begin
  Pen.Width := 0.5;
  ВсеТочкиМногоугольника(400,300,290,30)
end.
