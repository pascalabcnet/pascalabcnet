// ������������� ������������� �������. 
// ��������� SetLength ��������� ������ ��� ������������ ������
// �������� foreach (������ �������� ������ �� ������)
var a: array of integer;

begin
  var n := 20;
  SetLength(a,n);
  for var i:=0 to a.Length-1 do
    a[i] := Random(1,99);
  writeln('���������� ���������� ������������� ������� �����: ');  
  foreach var x in a do
    Print(x);
end.