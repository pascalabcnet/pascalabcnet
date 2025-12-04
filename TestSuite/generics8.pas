program generics8;
type TClass<T> = class
class function Test: integer;
begin
  Result := 2;
end;
class function Test2<T1>: T1;
begin
  Result := default(T1);
end;
end;

begin
  var c := System.Collections.Generic.Comparer&<integer>.Default;
  assert(System.Collections.Generic.Comparer&<integer>.Equals(nil, nil) = true);
  assert(TClass&<integer>.Test=2);
  assert(generics8.TClass&<integer>.Test=2);
  assert(generics8.TClass&<integer>.Test2&<integer> =0);
end.