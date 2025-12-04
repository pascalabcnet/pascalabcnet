var i: integer;
type
  t1<T> = class;
  
  t0 = class
    
    public function f1<T>: t1<T> := nil;
    
  end;
  t1<T> = class(t0) end;
  
procedure p1<T>(q: t1<T>);
begin
  Inc(i);
end;

procedure p1(q: t0) := p1(q.f1&<byte>);

begin
  p1(new t0);
  assert(i = 1);
end.