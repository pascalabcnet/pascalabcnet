var i: integer;

type
  MyClass<T> = record
    static function operator implicit(a: array of T): MyClass<T>; 
    begin 
      Inc(i);
    end;
    static function operator=(first, second: MyClass<T>) := True;
  end;
  ott = (one, two, three);
  stype = MyClass<ott>;
 
  
begin
  var g: array of ott := |one,three|;
  var s: MyClass<ott> := new MyClass<ott>;
  var b := (g = s);
  assert(i = 1);
 // ppp(g,s);
end.