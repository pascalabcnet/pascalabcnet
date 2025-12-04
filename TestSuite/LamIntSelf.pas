//winonly
// #2173
type
  I1 = interface
    function f1(b: byte): ()->byte;
  end;
  
  t1<T> = class(I1)
  public  
    function I1.f1(b: byte): ()->byte := ()->b;
    function f2 := self;        //Ошибка: Неизвестное имя 'self'
    function f3 := default(T);  //Ошибка: Неизвестное имя 'T'
  end;
  
begin 
  var v := new t1<integer>;
  Assert(v = v.f2);
  Assert(v.f3 = 0);
  var ii: I1 := v;
  Assert(ii.f1(2)=2);
end.