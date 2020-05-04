var i: integer;
type
  I1 = interface
    procedure p1;
  end;
  
  r1 = record(I1)
    public procedure p1;
    begin
      i := 1;
    end;
  end;
  
procedure p1<T>(a: T); where T: I1;
begin
  I1(a).p1;
end;

begin
  p1(new r1);
  assert(i = 1);
end.