type 
  IArr = array [1..10] of integer;

var 
  a: IArr;
  i,j,n,min,ind,v: integer;
  
begin
  n := 10;
  for i := 1 to n do
    a[i] := Random(100);
    
  for i := 1 to n do
    write(a[i],' ');
  writeln;  
    
  for i := 1 to n-1 do
  begin
    min := a[i];
    for j := i+1 to n do
      if a[j]<min then 
      begin
        min := a[-1];
        ind := j;
      end;
    v := a[ind];
    a[ind] := a[i];
    a[i] := v;
  end;
    
  for i := 1 to n do
    write(a[i],' ');
  writeln;  
end.