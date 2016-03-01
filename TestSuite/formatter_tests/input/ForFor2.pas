// Вложенные циклы for
const n = 6;

begin
  for var i:=1 to n do
  begin
    for var j:=1 to i do
      write('*');
    writeln;  
  end;
  writeln;  
  for var i:=1 to n do
  begin
    for var j:=1 to 3*n do
      if Odd(i+j) then 
        write('*')
      else write(' ');  
    writeln;  
  end;  
end.