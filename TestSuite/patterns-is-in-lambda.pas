begin
  var res := Arr(object(new List<integer>), object('asd'), object(1), object(2.0)).First(x -> x is string(s));
  Assert(res = 'asd');
end.