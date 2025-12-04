type
  t1 = class//не работает только в классе
    
    const c1 = 5;
    
    procedure p1;
    begin
      var arr1 := ArrGen(1, i -> c1);//В данной версии компилятора не поддерживается замыкание данного типа символов
      assert(arr1[0] = 5);
    end;
    
  end;

begin
  var t := new t1;
  t.p1;
end.