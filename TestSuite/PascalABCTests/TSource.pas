// #2068
type
  t1<T> = class
    procedure p1<T2>(f: Action<T>); begin end;
  end;

procedure proc2<TSource>;
begin
  var o := new t1<TSource>;
  o.p1&<string>(b->
  begin
    var a: TSource := b; 
  end);
end;

begin 
  Assert(1=1);
end.