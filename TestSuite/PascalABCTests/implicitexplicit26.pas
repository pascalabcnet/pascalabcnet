var i: integer;
type
  t1 = class
    
    static function operator implicit<T>(val: T): t1;
    begin
      i := 1;
    end;
    
    static function operator implicit(val: byte): t1; 
    begin
      i := 2;
    end;
    
    static function operator implicit(val: integer): t1; 
    begin
      i := 3;
    end;
    
  end;
  
begin
  var o: t1 := 0;
  assert(i = 3);
  o := 2.3;
  assert(i = 1);
  o := byte(1);
  assert(i = 2);
end.