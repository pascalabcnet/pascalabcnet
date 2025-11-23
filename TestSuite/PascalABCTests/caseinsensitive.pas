uses system, system.Collections.generic;
type TCLASS = class
public function tostring: STRING; OVERRIDE;
begin
  RESULT := 'ok';
end;
END;

BEGIN
  var lst := new list<integer>;
  lst.add(2);
  assert(lst[0]=2);
  assert(lst.toarray.first=2);
  lst := new system.collections.generic.list<integer>;
  var obj := new tclass;
  assert(obj.Tostring = 'ok');
END.