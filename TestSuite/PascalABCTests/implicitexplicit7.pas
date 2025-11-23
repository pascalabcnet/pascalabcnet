type
  t1 = class
    static instance: t1;
    
    static function operator implicit<T>(val: ^T): t1;
    begin
      if instance = nil then
        instance := new t1;
      Result := instance;
    end;
  end;
  
procedure p1<T>;
begin
  var i := default(T);
  var a: t1 := @i;
  assert(a = t1.instance);
end;

begin
  var i := 5;
  var a: t1 := @i;
  assert(a = t1.instance);
  p1&<integer>;
end.