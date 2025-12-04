type TClass = class
class i: integer := 2;

class constructor Create;
begin
  var lmb: integer -> integer := x -> i;
  assert(lmb(2) = 2);
end;
end;

begin
 var obj := new TClass;
end.