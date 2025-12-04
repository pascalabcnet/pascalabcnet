type
  e1 = (
    ef1 = 2,
    ef2 = 4,
    ef3 = 8
  );

begin
  var a: array[e1] of byte;
  a[ef2] := 4;
  readln;
end.