// Поиск значения в случайной матрице. Использование оператора goto
const 
  sz = 10;
  m = 4;
  n = 5;

type Matrix = array [1..sz,1..sz] of integer;

label 1;

var matr: Matrix;

begin
  // Заполнение матрицы m x n случайными числами
  for var i := 1 to m do
  for var j := 1 to n do
    matr[i,j] := Random(100);
  
  // Вывод матрицы 
  for var i := 1 to m do
  begin
    for var j := 1 to n do
      write(matr[i,j]:3);
    writeln;
  end;  
  
  // Есть ли в матрице элемент 5?
  var found5 := False;
  for var i := 1 to m do
  for var j := 1 to n do
    if matr[i,j]=5 then
    begin
      found5 := True;
      goto 1; // Ай как нехорошо! Но это лучший способ выхода из двух вложенных циклов сразу
    end;
1: 
  if found5 then 
    writeln('Элемент 5 найден')
  else writeln('Элемент 5 не найден')  
end.