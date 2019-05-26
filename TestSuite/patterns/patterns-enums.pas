begin
  var l: (A, B, C);
  match l with
    l.B: assert(false);
    l.A: assert(true);
  end;
end.