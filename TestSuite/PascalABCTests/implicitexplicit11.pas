var i: integer;
type
  t0 = class end;
  
  t1 = class
    
    static function operator implicit<T>(o: T): t1;
    where T: record;
    begin 
      i := 1;
    end;
    
    static function operator implicit(o: t0): t1;
    begin 
      i := 2;
    end;
    
  end;
  
procedure p1(q: t1) := exit;

begin
  p1(new t0);
  assert(i = 2);
end.