{$reference l1.dll}

type
  c0 = class end;
  
  // обязательно использовать кастомный тип в качестве типоаргумента
  // с ns.c2<object> не проявляется
  c3 = class(ns.c2<c0>)
  procedure test;
  begin
    f1();
  end;
  end;

begin
  var obj := new c3;
  
end.