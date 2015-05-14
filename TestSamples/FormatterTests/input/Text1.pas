uses PT4;

var f:text;
    filename:string;
    n,k,i,j:integer;
begin
  Task('Text1');
  read(filename);read(n);read(k);
  AssignFile(f,filename);
  Rewrite(f);
  for i:=1 to n do begin
    for j:=1 to k do
      write(f,'*');
    writeln(f);  
  end;    
  CloseFile(f);
end.

