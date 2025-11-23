procedure p<T>(s: set of T; f: T->T);
begin
  foreach var x in s do
  begin
    var y := f(x);
    Print(y);
  end;
end;

begin
  var s: set of integer := [1,2,3,7,2,4];
  p(s,x->x);
end.
