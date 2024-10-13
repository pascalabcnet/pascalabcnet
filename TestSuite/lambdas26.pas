begin
  var p := (p0: procedure)->
  begin
    p0();
  end;
  var i := 0;
  p(()->begin Inc(i) end);
  assert(i = 1);
end.