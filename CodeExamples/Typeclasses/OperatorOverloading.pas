type  
  
  Eq[T] = typeclass
  
    // Здесь оператор определяется над типом T,
    // а не над типом Eq
    function operator=(x, y: T): boolean;
    function operator<>(x, y: T): boolean;
    begin
      // Этот оператор ищется в текущем классе типов,
      // т.к. по умолчанию у шаблонного типа нет операторов
      Result := not (x = y);
    end;
  end;
  
  Eq[integer] = instance
    // В реализации функций инстанса все операторы
    // ищутся снаружи, это означает, что в инстансе,
    // и только в инстансе, нельзя вызывать операторы
    // класса типа, т.к. они всё-равно будут искаться
    // только во внешнем контексте. Без этого ограничения
    // появляется рекурсивное зацикливание в следующих
    // определениях:
    function operator=(x, y: integer):boolean := x = y;
    // Засчёт реализации по умолчанию, эту функцию можно
    // не переопределять, при этом сначала вызовется дефолтная
    // реализация в классе типов, а затем определение
    // оператора = из текущего инстанса
    //function operator<>(x, y: integer): boolean := x <> y;
  end;
  
function ArrayEq<T>(l1, l2: array of T): boolean; where Eq[T];
begin
  Result := true;
  for var i :=0 to l1.Length - 1 do
    // Оператор <> ищется в классе типа Eq[T].
    // Так проверяется шаблон.
    if l1[i] <> l2[i] then
    begin
      Result := false;
      break;
    end;
end;

begin
  begin
    var l1 := Arr(1,3,5,7,8); 
    var l2 := Arr(1,3,5,7,8); 
  
    // При инстанцировании, если существует инстанс, то все операторы
    // ищутся в инстансе.
    writeln(ArrayEq&[integer](l1, l2));
  end;
  
  begin
    var l1 := Arr(1,3,5,7,8); 
    var l2 := Arr(1,3,5,1,8); 
  
    writeln(ArrayEq&[integer](l1, l2));
  end;

end.