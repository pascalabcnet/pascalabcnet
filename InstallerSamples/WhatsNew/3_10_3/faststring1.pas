// 6.0 sec PascalABC.NET vs 11.4 sec Python
begin
  var mx := 0;
  for var n := 4 to 9999 do
  begin
    var s: faststring := '4' + '1' * n;

    while ('411' in s) or ('1111' in s) do
      s.Replace('411', '14', 1).Replace('1111', '1', 1);
    
    mx := max(mx, s.ToString.Sum(d -> d.todigit));
  end;
  print(mx, Milliseconds / 1000);
end.