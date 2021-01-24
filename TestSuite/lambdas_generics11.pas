type
  t1<T> = class
    
    procedure p1<T2>(f: T->T2);
    begin
      assert(f(default(T)).ToString() = '22');
    end;
    
  end;

procedure p1<T>;
begin
  var o: t1<byte>;
  o.p1(b->'22'); // ok
end;

procedure p2<TSource>;
begin
  var o: t1<TSource> := new t1<TSource>;
  o.p1(b->'22');
end;

begin 
p2&<integer>;
end.