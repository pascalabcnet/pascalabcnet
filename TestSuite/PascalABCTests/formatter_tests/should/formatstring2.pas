begin
  var a := 2;
  var b := 3;
  var c := 4;
  var s := $'Hello {a}';
  assert(s = 'Hello 2');
  s := $'{a},{b},{c}';
  assert(s = '2,3,4');
end.