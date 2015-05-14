var i:integer;
    o:object := i;
begin
  writeln(i is integer);  
  writeln(o is integer);
  o:=new object;
  writeln(o is integer);
  writeln(o is object);
end.