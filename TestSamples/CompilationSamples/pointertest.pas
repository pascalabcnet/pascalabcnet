function f(p:pointer):pointer;
begin
  //var i:=10;
  //result:=@i;
  result:=p;
end;
begin
  var i:=1;
  writeln(@i);
  writeln(f(@i));
  var p:=@i;
  var ptr:pointer;
  ptr:=p;
  p:=pinteger(ptr);
  writeln(p^);
end.