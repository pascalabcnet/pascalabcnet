uses System, System.Collections.Generic;

var lst : List<integer>;

begin
lst := new List<integer>();
lst.Add(3);
lst.Add(4);
lst.Add(5);
foreach i : integer in lst do
i := 54;
end.