type
  t1 = auto class
    i := 0;
  end;

begin
  var o: t1;
  o.i{@var t1.i: integer;@} := 2;
end.