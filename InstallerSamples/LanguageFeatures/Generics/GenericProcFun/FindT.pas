// Обобщенные функции
// Выведение типа T по типам параметров

function IndexOf<T>(a: array of T; val: T): integer;
begin
  Result := -1;
  for var i:=0 to a.Length-1 do
    if a[i]=val then
    begin
      Result := i;
      exit;
    end;
end;

var a := Arr('Ваня', 'Коля', 'Саша', 'Сережа');

begin
  var s := 'Сережа';
  writelnFormat('Индекс элемент со значением ''{0}'' равен {1}',s,IndexOf(a,s));
end.

