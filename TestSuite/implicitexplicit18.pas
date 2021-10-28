type
  TRec = record
    i: integer;
    constructor(i: integer);
    begin
      self.i := i;
    end;
  end;
  t1 = class
    i, j: integer;
    static function operator implicit<T>(a: array of T): t1; where T: record;
    begin
      var obj := new t1;
      obj.i := TRec(object(a[0])).i;
      obj.j := TRec(object(a[1])).i;
      Result := obj;
    end;
    
  end;
  
begin
  var a := Arr(new TRec(1),new TRec(2));
  var b: t1 := a;
  assert(b.i = 1);
  assert(b.j = 2);
end.