uses NumLibABC;

// Экономизация полинома на интервале
begin
  var x:=ArrGen(8,-0.75,x->x+0.25);
  var p:=new Polynom(0,1,-1/2,1/3,-1/4,1/5);
  var r:=p.EconomSym(0.75, 0.05);
  Println(r.eps,r.n);
  r.PrintlnBeauty;
  for var i:=1 to x.Length do
    Write(r.Value(x[i-1]):0:3,' ')
end.
