// Генерация больших простых чисел
begin
  Println('Большие простые числа: ');
  var count := 0;
  var beg := Random(1000000000)+2;
  for var i:=beg to beg+5000 do
  begin
    var f := True;
    var j := 2;
    var r := Round(Sqrt(i));
    while f and (j<=r) do
      if i mod j = 0 then f := False
        else j += 1;
    if f then
    begin
      Print(i);
      count += 1;
      if count mod 8 = 0 then 
        Println;
    end;
  end;
end.
