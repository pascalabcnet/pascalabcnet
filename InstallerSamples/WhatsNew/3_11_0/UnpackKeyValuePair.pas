// 3.11. Распаковка KeyValuePair
begin
  var t := 2 to 3.5;
  var (a,b) := t;
  Println(a,b);
  foreach var (a1,b1) in Dict(1 to 7, 22 to 555) do
    Println(a1,b1);
end.