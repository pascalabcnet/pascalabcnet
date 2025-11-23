var a,b : integer;
begin
a := 34;
case a of
0..12 : b := 12;
13..25 : b := 25;
26..34 : b := 34;
end;
assert(b=34)
end.
