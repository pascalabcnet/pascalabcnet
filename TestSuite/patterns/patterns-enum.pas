begin
  var l: (A,B,C);
  var e := A;
  match e with
    A, B, C: ;
  else
    assert(false);
  end;
end.