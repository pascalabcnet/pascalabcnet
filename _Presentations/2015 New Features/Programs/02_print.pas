const n=20;

begin
  var a := 1;
  var b := 1;
  for var i:=3 to n do
  begin
    var c := a + b;
    Print(c);
    a := b;
    b := c;
  end;
end.