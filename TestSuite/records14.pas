{$reference ClassLibrary1.dll}

var
  a := new ClassLibrary1.Rec1(1, 2);
  b := new ClassLibrary1.Rec1(1, 2);
  c := new ClassLibrary1.Rec1(3, 4);

begin
  assert(a = b);
  assert(not (a <> b));
  assert(a <> c);
  assert(not (a = c));
  var today := DateTime.Today;
  assert(today = today);
end.