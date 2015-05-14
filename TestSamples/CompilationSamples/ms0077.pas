var 
  a,b: array ['1'..'5'] of integer;

begin
  a['1'] := 5;
  b := a;
  b['1'] := 6;
  write(a['1']);
  readln;
end.