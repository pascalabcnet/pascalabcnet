// Методы строк .IsInteger и .IsReal
begin
  var s := '123.4 3 5 6.6 a v 67';
  var (si,sr) := (0,0.0);
  foreach var w in s.ToWords do
    if w.IsInteger then
      si += w.ToInteger
    else if w.IsReal then
      sr += w.Toreal;
  Print(si,sr);  
end.

