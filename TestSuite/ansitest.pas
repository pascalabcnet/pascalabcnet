begin
  var arr: array['�'..'�'] of integer;
  arr['�'] := 2;
  assert(arr['�'] = 2);
  var c := '�';
  var i := 0;
  case c of
    '�': i := 3;
    '�': i := 5;
  end;
  assert(i = 5);
end.