uses NumLibABC;

// ���������� ���� ������ �������� � ��������������� ��������������
// ������� ������� �������� 
begin
  var p:=new Polynom(-609, -283 ,294, -38, -5,1);
  var oL:=new PolRt(p);
  if oL.ier=0 then oL.Value.Println
  else Writeln('������: ier=',oL.ier);
end.
