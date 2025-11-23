begin
  var arr: array['à'..'ÿ'] of integer;
  arr['ä'] := 2;
  assert(arr['ä'] = 2);
  var c := 'ä';
  var i := 0;
  case c of
    'ë': i := 3;
    'ä': i := 5;
  end;
  assert(i = 5);
end.