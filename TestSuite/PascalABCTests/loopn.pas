begin
  var n := 10;
  loop n do
    Assert(n=n);
  var p: procedure;
  p := ()->begin
    Assert(n=n);
  end; 
end.