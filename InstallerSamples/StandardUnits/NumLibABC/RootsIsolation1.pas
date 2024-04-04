uses NumLibABC;

// �������� ������ ��������� y(x)=0 �� �������� ���������
// ��������� �������
begin
  var f:real->real:=t->sin(t)/(1+Sqr(Exp(t)))-0.1;
  var (a,b,h):=(-10,5,0.5);
  var oL:=new RootsIsolation(f,a,b,h);
  Println(oL.Value)
end.
