type TClass<T> = class(IEnumerable<T>)
public function GetEnumerator: IEnumerator<T>;
begin
  
end;

public function System.Collections.IEnumerable.GetEnumerator: System.Collections.IEnumerator;
begin
  
end;
end;
begin
  var i := 1;
  var seq1 := i.Iterate(x->x+1);
  var lst := new List<integer>;
  foreach var j in seq1 do
  begin
    lst += j;
    if j > 10 then
      break;
  end;
  assert(lst[5] = 6);
end.