unit u_realsingle1;
var r : real;
    s : single;
    
begin
  r := 3.14; s := 3.14;
  assert(r = 3.14);
  s := 3.14;
  assert(s = single(3.14));
  
end.