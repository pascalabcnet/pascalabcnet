const 
  h = Round(1.3);
  h2 = h;
  
procedure Test;
const dh = h;
begin
  assert(dh = 1);
end;

begin
  Test;
  assert(h2 = h);
end.