type
  t1 = class
    i: integer;
    procedure p1<T>(x: T); virtual := exit;
    function f<T>: T; virtual := default(T);
  end;
  
  t2 = class(t1)
    procedure p1<TT>(x: TT); override;
    begin
      i := 1;
    end;
    function f<T1>: T1; override;
    begin
      Result := default(T1);
      i := 2;
    end;
  end;
  
begin
  var o: t1 := new t2;
  o.p1&<integer>(2);
  assert(o.i = 1);
  assert(o.f&<integer> = 0);
  assert(o.i = 2);
end.