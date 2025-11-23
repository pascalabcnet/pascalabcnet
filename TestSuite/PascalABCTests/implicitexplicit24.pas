var i: integer;
type
  t1 = class
    
    static function operator implicit<T>(o: T): t1;
    begin
      i := 1;
      Result := nil;
    end;
    
  end;
  
function f0: t1 := nil;
procedure p0(o: t1) := exit;

begin
  var o: t1 := f0;
  assert(i = 0);
end.