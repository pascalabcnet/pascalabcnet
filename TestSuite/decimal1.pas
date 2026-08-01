begin
  var d: decimal := 1;
  d := d + decimal(2);
  assert(d = decimal(3));

  var converted := decimal(2.5);
  assert(converted = decimal(2.5));
end.
