type
  TClass = class
    auto property X: integer := 1;
    auto property Y: string := 'abc';
    static auto property Z: double := 3.14;
  end;

begin
  var a := new TClass();
  assert(a.X = 1);
  assert(a.Y.Equals('abc'));
  assert(TClass.Z = 3.14);
  
  TCLass.Z := 1.3;
  a.X := 2;
  a.Y := '';
  assert(a.X = 2);
  assert(a.Y.Equals(''));
  assert(TClass.Z = 1.3);
end.