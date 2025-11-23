//üя
begin
  var arr: array['а'..'я'] of integer;
  arr['д'] := 2;
  assert(arr['д'] = 2);
  var c := 'д';
  var i := 0;
  case c of
    'л': i := 3;
    'д': i := 5;
  end;
  assert(i = 5);
end.