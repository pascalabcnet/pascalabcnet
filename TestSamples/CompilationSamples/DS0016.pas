const
  s1='str'+'str';
  s2='str'+'c';
  s3='c'+'str';
  s4='c'+'c'+'c';
  
  //error = 'c'++'s';
            
var c:char;

begin
  writeln(s1,' ',s2,' ',s3,' ',s4);
  writeln('x'+'x',c+'x'+'c');
  c:='x';
  writeln(c+c+c+c+c+c+c+c+c+c+c+c+c+c+c+c+c+c);
  readln;
end.