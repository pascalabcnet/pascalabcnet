var i: integer;
type
  t1 = class
    
    // Обязательно шаблонный operator implicit
    static function operator implicit<T>(o: T): t1;
    begin
      i := 1;
      Result := nil;
    end;
    
  end;
  
function f0: t1 := nil;
procedure p0(o: t1) := exit;

begin
  // Обязательно вызвать f0 без скобок
  // Если поставить скобки - не воспроизводится
  p0(f0);
  assert(i = 0);
end.