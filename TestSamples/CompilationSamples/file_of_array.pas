type my = array[1..10] of integer;

var 
  f: file of my;
  m: my;

begin
  write(f,m);
  read(f,m);
  readln;
end.