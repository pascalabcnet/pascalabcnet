type
  t1 = class
    i, j: integer;
    static function operator implicit<T>(a: array of T): t1; where T: record;
    begin
      var obj := new t1;
      obj.i := integer(object(a[0]));
      obj.j := integer(object(a[1]));
      Result := obj;
    end;
    
  end;
  
begin
  var a := Arr(1,2);
  var b: t1 := a;
  assert(b.i = 1);
  assert(b.j = 2);
end.