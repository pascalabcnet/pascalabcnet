uses NumLibABC;

// ������������ ��������� ������� ���������� ��������
begin
  var f:real->real:=x->(3*x-8)/(8*x-4.1);
  var pt:=PartitionPoints(1.0,10.0,18).Select(x->new Point(x,f(x))).ToArray;
  var oL:=new Spline(pt);
  var r:=oL.Value(4.8);
  Writeln('�������� ������������������ ������� ��� �=4.8: ',r);
  var (d1,d2):=oL.Diff(4.8);
  Writeln('�������� 1-� � 2-� ���������� � ���� �����: ',d1,' ',d2)
end.
