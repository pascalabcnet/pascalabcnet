uses NumLibABC;

// ������ � ����������
begin
   var u:=(new Polynom(2, -6, 0, 3.8, 0, 1)).Value(-7.16); // �������� � �����
   Println(u);
   
   var t:=new Polynom(1, -7, 12, -3, -2);
   var (p,q):=(t.PInt, t.PDif); // ��������� � �����������
   p.PrintlnBeauty; q.PrintlnBeauty;
end.
