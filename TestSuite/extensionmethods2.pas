uses System, System.Collections.Generic;

type TArr = array of integer;

var lst: List<integer>;
    arr: array of integer := (5,6,7);
    
begin
  lst := new List<integer>();
  lst.Add(2);
  lst.Add(3);
  lst.Add(4);
  assert(lst.Average() = 3);
end.