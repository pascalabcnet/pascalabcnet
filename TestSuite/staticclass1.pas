var i: integer;
type TClass = static class
class j: integer;

class property p: integer read j write j;

class procedure Test;
begin
  i := 1;
end;

end;

begin
  TClass.Test;
  assert(i = 1);
  TClass.p := 2;
  assert(TClass.p = 2);
end.