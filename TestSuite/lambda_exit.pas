begin
  var i := 0;
  var p: procedure := procedure ->
  begin
    i := 1;
    exit;
    i := 2;
  end;
  p;
  assert(i = 1);
end.