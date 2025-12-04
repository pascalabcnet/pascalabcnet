begin
  var s := $'{{';
  assert(s = '{');
  s := $'{{{{';
  assert(s = '{{');
end.