begin
  var l: ^integer;
  l := nil;
  match l with
    nil: assert(true);
  else
    assert(false);
  end;
end.