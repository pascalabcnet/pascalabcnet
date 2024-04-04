function f1(x: integer) := 1;

type TFnc = integer -> integer;

procedure p(params arr: array of object);
begin
  assert(TFnc(arr[0])(2) = 1);
  assert(TFnc(arr[1])(2) = 1);
end;
begin
  p(f1, f1); 
end.