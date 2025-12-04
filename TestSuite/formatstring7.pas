begin
  var s1 := $'{{0}} {1}';
  assert(s1 = '{0} 1');
  var s2 := $'';
  var s3 := $'{{0}}';
  assert(s3 = '{0}');
  s3 := $'{{1 {0}';
  assert(s3 = '{1 0');
end.