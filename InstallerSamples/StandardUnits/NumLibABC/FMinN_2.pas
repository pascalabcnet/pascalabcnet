uses NumLibABC;

// ��������� ����� MKSearch
begin
  var f:function(x:array of real):real:= x->Power(x[0],4)+
      Power(x[1],4)-2*Sqr(x[0])+4*x[0]*x[1]-2*Sqr(x[1])+3;
  var a:=Arr(-20.0,-20.0);
  var b:=Arr(20.0,20.0);
  var y:real;
  var oL:=new FMinN(a,f);
  oL.MKSearch(a,b,y);
  Write('������ ����������: '); oL.x.Println;
  Writeln('�������� �������: ', y);
end.
