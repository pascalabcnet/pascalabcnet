/// Модуль упрощенной структуры
unit MyUnit; // имя модуля должно совпадать с именем файла

// Документирующие комментарии отображаются при наведении на имя курсора мыши
/// Возвращает максимальный элемент в массиве
function Max(a: array of integer): integer;
begin
  Result := integer.MinValue;
  foreach x: integer in a do
    if x>Result then 
      Result := x;
end;

end.
