const c = 2;

begin
  var l := 2;
  match l with
    c: assert(true);
  else
    assert(false);
  end;
end.