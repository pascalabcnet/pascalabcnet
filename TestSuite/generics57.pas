var i: integer;
type
  IInt = interface
    procedure p1(x: integer);
  end;
  
  t1 = class(IInt)
    public procedure p1(x: integer);
    begin
      i := x;
    end;
  end;
  
procedure p0<T>(a: T);
where T: IInt;
begin
  a.p1(1);
end;

begin
  p0(new t1);
  assert(i = 1);
end.