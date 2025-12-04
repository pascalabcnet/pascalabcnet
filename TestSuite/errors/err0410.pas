type
  node = class
    i: integer;
    function f1: node := new node;
  end;
  
begin
  var n1 := new node;
  var n2: node;
  with n1 do
  begin
    // Обязательно вложенный with
    n2 := f1;
    with n2 do
    begin
      i := 2;
    end;
    // Обязательно вызвать метод после вложенного with
    f1;
    i := 1;
  end;
  f1;
  assert(n1.i = 1);
  assert(n2.i = 2);
end.