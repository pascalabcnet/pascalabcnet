// демонстрация работы параллельного for
//заполнение массива
procedure FillRandArr(A: array of real);
begin
  Randomize;
  for var i := 0 to A.Length - 1 do
    a[i] := Random(1000);
end;
 // вывод массива
procedure printArr(A: array of real);
begin
  for var i := 0 to A.Length - 1 do
    writeln(a[i]);
end;
// Последовательное вычисление квадратов синусов
procedure SqrSinArr(A: array of real; var C: array of real);
begin
  
  for var i := 0 to A.Length - 1 do
    C[i] := sqr(sin(A[i]));
end;
// Параллельное вычисление квадратов синусов
procedure SqrSinArrParallel(A: array of real; var C: array of real);
begin
  
  {$omp parallel for}
  for var i := 0 to A.Length - 1 do
    C[i] := sqr(sin(A[i]));
end;

begin
  var A: array of real;
  var C: array of real;
  SetLength(A, 10000000);    
  SetLength(C, A.Length);
  FillRandArr(A);    
  
  var  m0 := Milliseconds;
  SqrSinArrParallel(A, C);
  
  writeln('Параллельное выполнение: ', Milliseconds - m0, 'ms');
  
  m0 := Milliseconds;
  SqrSinArr(A, C);
  writeln('Последовательное выполнение: ', Milliseconds - m0, 'ms');
  
end.