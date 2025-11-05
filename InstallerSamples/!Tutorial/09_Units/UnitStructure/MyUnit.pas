/// Модуль упрощенной структуры
unit MyUnit; // имя модуля должно совпадать с именем файла

const Size = 100;

type IntArr = array of integer;

var Delimiter: string := ' ';

// Документирующие комментарии отображаются при наведении на имя курсора мыши
/// Заполняет массив случайными числами
function FillArr(n: integer): IntArr;
begin
  Result := new integer[n];
  for var i:=0 to Result.Length-1 do
    Result[i] := Random(100);
end;

/// Возвращает минимальный элемент в массиве
function Min(a: IntArr): integer;
begin
  Result := a[0];
  for var i:=1 to a.Length-1 do
    if Result > a[i] then 
      Result := a[i];
end;

end.
