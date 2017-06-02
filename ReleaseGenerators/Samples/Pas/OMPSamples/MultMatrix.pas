uses Arrays;

procedure ParallelMult(a,b,c: array [,] of real; n: integer);
begin
  {$omp parallel for }
  for var i:=0 to n-1 do
    for var j:=0 to n-1 do
    begin  
       c[i,j]:=0;
       for var l:=0 to n-1 do
          c[i,j]:=c[i,j]+a[i,l]*b[l,j];
    end;
end;

procedure Mult(a,b,c: array [,] of real; n: integer);
begin
  for var i:=0 to n-1 do
    for var j:=0 to n-1 do
    begin  
       c[i,j]:=0;
       for var l:=0 to n-1 do
          c[i,j]:=c[i,j]+a[i,l]*b[l,j];
    end;
end;

const n = 400;

begin
  var a := Arrays.CreateRandomRealMatrix(n,n);
  var b := Arrays.CreateRandomRealMatrix(n,n);
  var c := new real[n,n];
  ParallelMult(a,b,c,n);
  writeln('Параллельное перемножение матриц: ',Milliseconds,' миллисекунд');
  var d := Milliseconds;
  Mult(a,b,c,n);
  writeln('Последовательное перемножение матриц: ',Milliseconds-d,' миллисекунд');
end.