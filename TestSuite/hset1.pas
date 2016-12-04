begin
  var h1 := HSet(1,2,3);
  var h2 := HSet(1,2,3);
  assert(h1=h2);
  h1 := HSet(1,2,3);
  h2 := HSet(1,2);
  assert(h1 > h2);
  assert(h2 < h1);
end.