type  
  
  Eq[T] = typeclass
    // Реализация функций по умолчанию
    function equal(x, y: T): boolean;
    begin
      Result := not notequal(x, y);
    end;
    
    function notequal(x, y: T): boolean;
    begin
      Result := not equal(x, y);
    end;
  end;
  
  Eq[integer] = instance
    // Теперь можно переопределить одну любую функцию
    function equal(x, y: integer):boolean := x = y;
    //function notequal(x, y: integer):boolean := x <> y;
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
  
    writeln(ArrayEq&[integer](l1, l2));
  end;
  
  begin
    var l1 := Arr(1,3,5,7,8); 
    var l2 := Arr(1,3,5,1,8); 
  
    writeln(ArrayEq&[integer](l1, l2));
  end;

end.