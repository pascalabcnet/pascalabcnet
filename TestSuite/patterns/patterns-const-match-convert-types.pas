begin
  var l: byte;
  l := 1;
  match l with
    1: assert(true);
  else
    assert(false);
  end;
end.