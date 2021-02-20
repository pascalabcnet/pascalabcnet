const n = 10;

var a,b: array [1..n] of integer;
    i,j,t,min,max,sum: integer;
    av: real;
begin
  for i:=1 to n do
    a[i] := Random(100);

  for i:=1 to n do
    write(a[i],' ');
  writeln;

  b := a;
  for i:=1 to n-1 do
    for j:=n downto 2 do
      if b[j-1]>b[j] then
      begin
        t := b[j-1];
        b[j-1] := b[j];
        b[j] := t;
      end;

  for i:=1 to n do
    write(b[i],', ');
  writeln;

  min := MaxInt;
  max := -MaxInt;
  sum := 0;
  for i:=1 to n do
  begin
    if a[i]<min then
      min := a[i];
    if a[i]>max then
      max := a[i];
    sum := sum + a[i];
  end;
  av := sum/n;

  writeln(min,' ',max);
  writeln(sum,' ',av);
end.