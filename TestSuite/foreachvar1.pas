{begin
  var s := [1,2,3];
  foreach var x in s do
  begin
    var y := x + x;
  end;
end.}
type
  TPair = auto class
    Key: string;
    Value: integer;
  end;
begin
  var A := Arr(new TPair('a',3),new TPair('c',1),new TPair('b',4));
  foreach var p in A do
    writeln(p.Key, '->', p.Value);
end.