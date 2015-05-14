unit u_emptyarr1;
var arr: array of integer := new integer[0]();
begin
  assert(arr.Length = 0);
end.