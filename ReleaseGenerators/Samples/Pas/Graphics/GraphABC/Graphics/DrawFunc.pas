uses GraphABC;

begin
  Brush.Color := ARGB(0,0,0,0); // ���������� �����
  Draw(x->x*sin(x),-20,20);
  Draw(sin);
  Draw(cos);
  Draw(exp);
end.