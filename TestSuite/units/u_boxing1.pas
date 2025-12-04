unit u_boxing1;
procedure test(o: object);
begin
  assert(integer(o) = 123);
end;
begin
  var i := 123;
  var j := object(i);
  assert(integer(j) = 123);
  test(object(i));
end.