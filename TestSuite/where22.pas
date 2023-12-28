type
  t1<TSelf> = interface
    where TSelf: t1<TSelf>;
    function f: integer;
  end;
  //Ошибка: Невозможно инстанцировать, так как тип TSelf не реализует интерфейс t1<TSelf>
  t2<TSelf> = interface(t1<TSelf>)
    where TSelf: t2<TSelf>;
    // А если так то работает:
//    where TSelf: t1<TSelf>, t2<TSelf>;
  end;
  
  TClass = class(t2<TClass>)
    public function f: integer;
    begin
      Result := 1;
    end;
  end;
begin 
  var obj := new TClass;
  assert(obj.f = 1);
end.