unit u_typedconst3;
const cc1 : 1..7 = 2;
type Digits = (one,two, three, four);

const rec : record a : 1..4; end = (a:cc1);
      rec2 : record a : 1..4; end = (a:3);
      rec3 : record a : one..three; end = (a:two);
      rec4 : record a : set of 1..4; end = (a:[1..5]);
      
begin
  assert(rec.a = 2);
  assert(rec2.a = 3);
  assert(rec3.a = two);
  assert(rec4.a = [1..5]);
end.