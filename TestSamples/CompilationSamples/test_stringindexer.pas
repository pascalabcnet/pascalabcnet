var s:string;
    c:char;
    i:integer;
begin
  s:='1234';
  c:=s[1];
  for i:=0 to s.length-1 do 
    writeln(s[i]);
  readln;
end.