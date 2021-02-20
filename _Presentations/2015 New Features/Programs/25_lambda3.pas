procedure PrintTable(f: RealFunc; a,b: real; n: integer);
begin
  var h := (b-a)/n;
  var x := a;
  for var i:=0 to n do
  begin
    writeln(x:3:1,f(x):7:2);
    x += h;       
  end;
end;

begin
  PrintTable(sqr,0,5,10); 
end.