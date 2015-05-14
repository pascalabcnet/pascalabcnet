type
  Matr = array [1..10,1..10] of integer;

procedure Transpose(const a: Matr; 
  m,n: integer; var b: Matr);
var i,j: integer;
begin
  for i:=1 to n do
  for j:=1 to m do
    b[i,j] := a[j,i]  
end;

var a,b: Matr;
    m,n,i,j: integer;
        
begin
  m := 3;
  n := 4;
  for i:=1 to 3 do
  for j:=1 to 4 do
    a[i,j] := Random(100);  

  Transpose(a,m,n,b);
  
  for i:=1 to n do
  begin
    for j:=1 to m do
      write(b[i,j],' ');  
    writeln;  
  end;
end.