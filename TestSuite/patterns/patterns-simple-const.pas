begin
  var a := 3.14;
  match a with
    1.0, 2.0, 3.0: assert(false);
    3.13, 3.14, 3.15, 3.16: ;
    123.0: assert(false);
  end;
end.