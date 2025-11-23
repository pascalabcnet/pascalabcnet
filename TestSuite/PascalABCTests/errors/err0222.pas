type
  pair=record
  x,y:integer
  end;

begin
  var n:=10;
  var a:=new pair[n];
  for var i:=1 to n-1 do
    with a[i] do begin x:=i; y:=10*i end;
  Writeln(a);
  Writeln(a.Select(e->e.x+e.y).Sum)
end.