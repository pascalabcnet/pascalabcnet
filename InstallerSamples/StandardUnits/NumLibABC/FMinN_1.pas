uses NumLibABC;

// ����� �������� ������� ����-������
function f(x:array of real):real:=100*Sqr(x[1]-Sqr(x[0]))+Sqr(1-x[0]);

begin
  var xb:=Arr(-1.2,1.0);
  var oL:=new FMinN(xb,f);
  var r:=oL.HJ;
  Writeln('���������� ��������: ',oL.iter);
  Write('�������� ����������: '); r.Println;
  Writeln('���������� �������� �������: ',f(r));
end.
