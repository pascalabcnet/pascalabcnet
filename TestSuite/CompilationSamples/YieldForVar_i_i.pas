function MatrMainDiag(a: array[,]of integer):sequence of integer;
begin
  for var i:=0 to 10 do 
    yield a[i,i]
end;

function MatrMainDiag1(a: array[,]of integer):sequence of integer;
begin
  for var i:=0 to 10 do 
    yield min(i,i)
end;

begin
  
end.

