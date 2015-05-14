unit u_case1;
begin
  var i := 6;
  case i of
    0..3 : i := 1;
    6 : i := 2;
    7..10 : i := 3;
  end;
  assert(i=2);
  i := 15;
  case i of
    2,5,1,3 : i := 1;
    12,13,14,15,18 : i := 2;
    31..40 : i := 3;
  end;
  assert(i=2);
end.