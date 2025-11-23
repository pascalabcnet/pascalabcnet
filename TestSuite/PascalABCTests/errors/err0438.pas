//!Нельзя преобразовать тип t0<t0<string>> к t1
var i: integer;
type
  t0<T> = class end;
  t2<T> = class end;
  t1 = class
    
    static function operator implicit<T>(a: t0<t0<T>>): t1; where T: record;
    begin
      i := 1;
    end;
   
  end;
  
begin
  var a: t0<t0<string>>;
  var k: t1 := a;
end.