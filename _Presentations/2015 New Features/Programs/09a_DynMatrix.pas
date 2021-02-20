function Transpose(a: array[,] of integer): 
  array[,] of integer;
begin
  var m := Length(a,0);
  var n := Length(a,1);
  Result := new integer[n,m];
  for var i:=0 to n-1 do
  for var j:=0 to m-1 do
    Result[i,j] := a[j,i]  
end;

begin
  var a := MatrixRandom(3,4);
  writeln(a);
  var b := Transpose(a);
  writeln(b);
end.