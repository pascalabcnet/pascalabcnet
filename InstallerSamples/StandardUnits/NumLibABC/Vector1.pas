uses NumLibABC;

// Векторная алгебра
begin
  // модуль вектора
  var a:=new Vector(3,-4,1);
  var b:=new Vector(-1,0,5);
  Writeln((2*a-b).ModV);
  
  // разложение по ортам 
  var p:=Arr(3.0,0.0,-4.0);
  a:=new Vector(p);
  a.Ort.Println;

  // направляющий косинус
  a:=new Vector(2,-1,1);
  b:=new Vector(2,3,6);
  Writeln(a*b/(a.ModV*b.ModV))

end.
