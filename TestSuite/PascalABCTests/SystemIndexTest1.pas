begin
  var s := 'qwerty';
  Assert(s[^1] = 'y');
  {Assert(s[:^1] = 'qwert');
  var a := Arr(1..10);
  Assert(a[^1] = 10);
  Assert(a[:^1].SequenceEqual(Arr(1..9)));
  var l := Lst(11..20);
  Assert(l[^1] = 20);
  Assert(l[:^1].SequenceEqual(Arr(11..19)));
  Assert(a[^2::-2].SequenceEqual(Arr(9, 7, 5, 3, 1)));}
end.