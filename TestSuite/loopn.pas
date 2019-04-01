begin
  var n := 10;
  loop n do
    Print(1);
  var p: procedure;
  p := ()->begin
    Print(n);
  end;
  Assert(1=1);
end.