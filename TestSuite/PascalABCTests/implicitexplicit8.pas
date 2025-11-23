var o: System.Type;
type
  t1 = class
    static function operator implicit<T>(val: T): t1;
    begin
      o := typeof(T);
    end;
  end;
  
  t2<T> = class end;
  
begin
  var a: t1 := new t2<integer>;
  assert(o = typeof(t2<integer>));
end.