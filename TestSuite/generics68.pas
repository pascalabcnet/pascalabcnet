var i: integer;
type
  I1 = interface
    procedure p1;
  end;
  
  // Обязательно класс, не запись
  t2 = class(I1)
    public procedure p1;
    begin
      i := 1;
    end;
  end;
  
// Обязательно I1(o), (o as I1) работает без проблем
procedure p0<T>(o: T) := I1(o).p1;

begin
  p0(new t2);
  assert(i = 1);
end.