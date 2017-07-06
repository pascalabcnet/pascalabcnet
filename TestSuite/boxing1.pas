procedure test2(params arr: array of object);
begin
  assert(integer(arr[0]) = 123);
end;

procedure test(o: object);
begin
  assert(integer(o) = 123);
end;

begin
  var i := 123;
  var j := object(i);
  assert(integer(j) = 123);
  test(object(i));
  test2(object(i),2);
  writeln(object(i).ToString);
  assert(object(i).ToString = '123');
end.