/// Модуль упрощенной структуры
unit MyUnit; // имя модуля должно совпадать с именем файла

const Size = 100;

type IntArr = array [1..Size] of integer;

var Delimiter: string := ' ';

// Документирующие комментарии отображаются при наведении на имя курсора мыши
/// Заполняет массив случайными числами
procedure FillArr(var a: IntArr; n: integer);
begin
  for var i:=1 to n do
    a[i] := Random(100);
end;

/// Выводит массив
procedure WriteArr(const a: IntArr; n: integer);
begin
  for var i:=1 to n do
    write(a[i],Delimiter);
  writeln;  
end;

/// Возвращает минимальный элемент в массиве
function Min(const a: IntArr; n: integer): integer;
begin
  Result := a[1];
  for var i:=1 to n do
    if Result>a[i] then 
      Result := a[i];
end;

end.
