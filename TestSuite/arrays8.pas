type TEnum = (one, two, three);
var arr: array[TEnum] of integer;
begin
  arr[three] := 12;
  assert(arr[three] = 12);
end.