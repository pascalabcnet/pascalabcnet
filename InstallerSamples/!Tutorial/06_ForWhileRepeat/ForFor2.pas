// Вложенные циклы for
const n = 6;

begin
  for var i:=1 to n do
  begin
    for var j:=1 to i do
      Write('*');
    Writeln;  
  end;
  Writeln;  
  for var i:=1 to n do
  begin
    for var j:=1 to 3*n do
      if Odd(i+j) then 
        Write('*')
      else Write(' ');  
    Writeln;  
  end;  
end.