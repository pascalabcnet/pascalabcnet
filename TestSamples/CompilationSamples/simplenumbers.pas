
// Генерация больших простых чисел
var i,j,r,count: integer;
    f: boolean;
    beg: integer;
begin
  count:=0;
  beg:=Random(1000000000);
  for i:=beg to beg+1000 do
  begin
    f:=true;
    j:=2;
    r:=round(sqrt(i));
    while f and (j<=r) do
      if i mod j = 0 then f:=false
        else j:=j+1;
    if f then
    begin
      write(i.tostring+' '.tostring);
      Inc(count);
      if count mod 5 = 0 then writeln;
    end;
  end;
  writeln;
end.
