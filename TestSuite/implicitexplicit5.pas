type
  t1 = class
    i: object;
    constructor(i: object);
    begin
      self.i := i;
    end;
    static function operator implicit<T>(val: T): t1 := new t1(val);
  end;
  
begin
  var a: t1 := 5;
  assert(integer(a.i) = 5);
end.