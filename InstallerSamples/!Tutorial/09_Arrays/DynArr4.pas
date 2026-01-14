// Передача динамического массива в качестве параметра

procedure WriteArr(a: array of integer); 
// var указывать не нужно, т.к. динамический массив является ссылкой на реальный объект
begin
  foreach var x in a do
    Print(x);
  Println;
end;

procedure CreateRandomArr(var a: array of integer; n: integer); 
// var указывать нужно, т.к. в процедуре создается новый динамический массив 
begin
  a := new integer[n];
  for var i:=0 to a.Length-1 do
    a[i] := Random(10);
end;

begin
  Println('Случайный целый массив: ');
  var a: array of integer;
  CreateRandomArr(a,30);
  WriteArr(a);
end.