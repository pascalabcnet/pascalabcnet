type TClass = auto class
  i: integer;
end;

function operator implicit<T>(s: sequence of T): array of T; extensionmethod;
begin 
  Result := s.ToArray;
end;

begin
  var a: array of TClass;
  var s := Seq(new TClass(1), new TClass(2));
  a := s;
  assert(a[0].i = 1);
  assert(a[1].i = 2);
  var a2: array of integer;
  var s2 := Seq(1, 2);
  a2 := s2;
  assert(a2[0] = 1);
  assert(a2[1] = 2);
end.