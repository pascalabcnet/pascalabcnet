// Матрица как динамический массив массивов
type Matrix = array of array of integer;

function CreateMatrix(m,n: integer): Matrix;
begin
  SetLength(Result,m);
  for var i:=0 to m-1 do
    SetLength(Result[i],n);
end;

procedure FillMatrByRandom(matr: Matrix);
begin
  for var i:=0 to matr.Length-1 do
  for var j:=0 to matr[0].Length-1 do
    matr[i][j] := Random(100);  
end;

procedure WriteMatrix(matr: Matrix);
begin
  for var i := 0 to matr.Length-1 do
  begin
    for var j := 0 to matr[0].Length-1 do
      write(matr[i,j]:3);
    writeln;
  end;  
end;

var matr: Matrix;

begin
  var m := 5;
  var n := 7;
  matr := CreateMatrix(m,n);
  
  FillMatrByRandom(matr);
  WriteMatrix(matr);
end.