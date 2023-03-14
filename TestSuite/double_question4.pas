function f := default(string);
begin
  var s := (f??'').PadRight(3);
  assert(s = '   ');
end.