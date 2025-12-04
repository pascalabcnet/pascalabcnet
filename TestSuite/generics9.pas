function f<T>(t1: T): array of T; 
  where T: System.IComparable<T>; forward;

function f<T>(t1: T): array of T; 
begin
  var t2: T;
  t2 := t1;
  Result := new T[1];
  Result[0] := t1;
end;

begin
  var arr := f(2);
  assert(arr[0] = 2);
end.