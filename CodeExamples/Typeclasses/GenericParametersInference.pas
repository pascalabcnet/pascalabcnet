﻿type  
  
  Eq[T] = typeclass
    function equal(x, y: T): boolean;
    function notequal(x, y: T): boolean;
  end;
  
  Eq[integer] = instance
    function equal(x, y: integer):boolean := x = y;
    function notequal(x, y: integer):boolean := x <> y;
  end;
  
function ArrayEq<T>(l1, l2: array of T): boolean; where Eq[T];
begin
  Result := true;
  for var i :=0 to l1.Length - 1 do
    if Eq&[T].notequal(l1[i], l2[i]) then
    begin
      Result := false;
      break;
    end;
end;

begin
  begin
    var l1 := Arr(1,3,5,7,8); 
    var l2 := Arr(1,3,5,7,8); 
  
    // При выводе шаблонных параметров функции,
    // сначала выводится тип фактических параметров,
    // а потом ищутся подходящие классы типов
    writeln(ArrayEq(l1, l2));
  end;
  
  begin
    var l1 := Arr(1,3,5,7,8); 
    var l2 := Arr(1,3,5,1,8); 
  
    writeln(ArrayEq(l1, l2));
  end;

end.