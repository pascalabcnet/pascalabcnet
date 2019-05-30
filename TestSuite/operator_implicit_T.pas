type
  t1<T> = class
    static function operator implicit(o: T): t1<T>; begin Assert(2=1); Result := nil; end;
  end;
  t2<T> = class(t1<T>) end;

procedure p<T>(r: t1<T>);
begin
  r := new t2<T>;
end;

begin 
  p(new t1<integer>);
end.