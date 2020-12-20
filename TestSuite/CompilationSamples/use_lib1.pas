{$reference 'lib1.dll'}

procedure x := exit;

begin
  T.p(x);
  assert(T.i = 1);
end.