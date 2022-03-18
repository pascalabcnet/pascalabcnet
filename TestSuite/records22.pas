type
  r1 = record
    val: integer;
    static function operator=(o1: r1; o2: integer) := o1.val = o2;
    static function operator<>(o1: r1; o2: integer) := o1.val <> o2;
  end;
  
begin
  var o: r1;
  assert(o = o);
  assert(o = 0);
  assert(o <> 2);
  o.val := 2;
  assert(o = 2);
end.