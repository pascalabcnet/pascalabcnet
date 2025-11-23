// #2067
type
  t1<T> = class
    procedure p1<T2>(f: Func<T,T2>); begin end;
  end;

procedure p1<T>;
begin
  var o: t1<byte>;
  o.p1(b->''); // ok
end;

procedure p2<TSource>;
begin
  var o: t1<TSource>;
  o.p1((b: TSource): string ->''); 
end;

begin 
  Assert(1=1);
end.