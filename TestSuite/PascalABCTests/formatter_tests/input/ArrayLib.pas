unit ArrayLib;

/// Вывод массива 
procedure WriteArray<T>(a: array of T); 
begin
  for var i:=0 to a.Length-1 do
    write(a[i],' ');
  writeln;
end;

procedure WriteArray(a: array of real); 
begin
  foreach x: real in a do
    write(x:0:1,'  ');
  writeln;
end;

/// Создание массива и заполнение его случайными числами
procedure CreateRandom(var a: array of integer; n: integer);
begin
  a := new integer[n];
  for var i:=0 to n-1 do
    a[i] := random(100);
end;

procedure CreateRandom(var a: array of real; n: integer);
begin
  a := new real[n];
  for var i:=0 to n-1 do
    a[i] := random*10;
end;

end.