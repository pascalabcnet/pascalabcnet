// ������������� ������������� �������. 
var a: array of integer;

begin
  writeln('SetLength ��� ��������� ������ �� ������ ������ ��������:');
  SetLength(a,5);
  for var i:=0 to a.Length-1 do
    a[i] := i+1;
  a.Println;

  SetLength(a,10);
  a.Println;
end.