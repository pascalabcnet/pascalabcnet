uses System, System.Collections.Generic;
type TClass = class
end;

begin
  var lst: IList<TClass> := new List<TClass>;
  var obj: TClass := new TClass();
  lst.Add(obj);
  assert(lst[0]=obj);
  assert(lst.Count=1);
  lst.Add(obj);
  foreach it: TClass in lst do
  begin
    assert(it = obj);
  end;
  var lst2: ICollection<TClass> := lst;
  lst2.Clear();
  assert(lst2.Count = 0);
end.