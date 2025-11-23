begin
  var l: List<integer>;
  match l with
    nil: assert(true);
  else
    assert(false);
  end;
end.