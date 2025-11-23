unit u_generics9;

begin
  var s := '1234';
  assert(s.All(char.IsDigit));
end.