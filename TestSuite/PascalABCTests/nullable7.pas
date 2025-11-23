var i: integer;
type
  r1 = record
    static function operator=(a,b: r1): boolean;
    begin
      i := 1;
      Result := false;
    end;
    static function operator<>(a,b: r1) := false;
  end;
  
begin
  var n1, n2: r1?;
  assert(n1 = n2); // Должно быть true, без вызва r1.operator=
  n1 := new r1;
  assert(not (n1 <> n1));
  assert(not (n1 = n1));
  assert(i = 1);
end.