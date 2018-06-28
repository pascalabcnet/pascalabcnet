// Undefined FileName(0) : Нельзя использовать override в коротких функциях без явно опредленного типа возвращаемого значения
// И выскакивает окно, тк Вы не определили Source Context

type 
  Less[T] = typeclass
    function operator<(x, y: T): boolean;
  end;
  Less[integer] = instance
    function operator<(x, y: integer) := x<y;
  end;

function MinIndex<T>(a: array of T): integer; where Less[T];
begin
  Result := -1;
  var min := a[0]; 
  for var i:=0 to a.Length - 1 do
    if a[i]<min then
    begin
      Result := i;
      min := a[i];
    end;
end;

begin
  var a := Arr(10,3,5,7,8); 
  MinIndex(a).Println;
end.