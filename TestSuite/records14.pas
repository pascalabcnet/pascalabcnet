{$reference ClassLibrary1.dll}

var
  a := new ClassLibrary1.Rec1(1, 2);
  b := new ClassLibrary1.Rec1(1, 2);
  c := new ClassLibrary1.Rec1(3, 4);

begin
  assert(typeof(ClassLibrary1.Rec1).IsValueType);
  assert((a.a = 1) and (a.b = 2));
  assert((c.a = 3) and (c.b = 4));
  assert(a = b);
  assert(not (a <> b));
  assert(a <> c);
  assert(not (a = c));
  var today := DateTime.Today;
  assert(today = today);
end.
