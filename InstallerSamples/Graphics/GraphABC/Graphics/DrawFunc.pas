uses GraphABC;

begin
  Brush.Color := ARGB(0,0,0,0); // прозрачная кисть
  Draw(x->x*sin(x),-20,20);
  Draw(sin);
  Draw(cos);
  Draw(exp);
end.