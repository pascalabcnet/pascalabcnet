// Генерация больших простых чисел
begin
  writeln('Большие простые числа: ');
  var count := 0;
  var beg := Random(1000000000)+2;
  for var i:=beg to beg+5000 do
  begin
    var f := True;
    var j := 2;
    var r := round(sqrt(i));
    while f and (j<=r) do
      if i mod j = 0 then f := false
        else j += 1;
    if f then
    begin
      write(i,' ');
      Inc(count);
      if count mod 5 = 0 then writeln;
    end;
  end;
end.
