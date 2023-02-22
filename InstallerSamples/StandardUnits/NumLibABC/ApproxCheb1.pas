uses NumLibABC;

// ������������� ��������� ������� ���������� ��������
// �� ������ ���������� ��������� 
begin
  var e:=0.1;
  var x:=ArrGen(12,i->0.25*i-2); x.Println;
  var y:=x.Select(z->2*z-5*Sqr(z)+8*z*Sqr(z)).ToArray; y.Println;
  var oL:=new ApproxCheb(x,y,e);
  oL.f.Println;  // ������������������ ��������
  Println(oL.r,oL.tol);  // ������������ ������� �������� � ����������� �����������
  oL.MakeCoef; // ��������������� ������������
  oL.c.Println;
end.
