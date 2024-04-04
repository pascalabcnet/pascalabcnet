var i: integer;
type
  t1<T> = class
    
    static function operator implicit(o: T): t1<T>;
    begin
      i := 1;
    end;
    
  end;
  
procedure p1<T>;
begin
  var a: t1<T> := new t1<T>();
  //Ошибка: Возможны два преобразования типа: к типу t1<T> и к типу t1<T>
  var b := if false then a else nil;
  assert(i = 0);
  b := nil;
  assert(i = 0);
  var v: T;
  b := v;
  assert(i = 1);
end;

begin 
  p1&<integer>;
end.