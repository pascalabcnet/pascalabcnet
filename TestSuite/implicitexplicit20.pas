var i: integer;
type
  TRec = record end;
  t0<T> = class end;
  t2<T> = class end;
  t1 = class
    
    static function operator implicit<T>(a: t0<t0<T>>): t1; where T: record;
    begin
      i := 1;
    end;
    static function operator implicit<T>(a: t2<t2<t2<T>>>): t1; where T: record;
    begin
      i := 2;
    end;
  end;
  
begin
  var a: t0<t0<TRec>>;
  var a2: t2<t2<t2<TRec>>>;
  var k: t1 := a;
  assert(i = 1);
  k := a2;
  assert(i = 2);
end.