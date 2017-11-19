const 
  h {@const h: integer = Round(1.3);@}= 
  Round{@function Round(x: real): integer;@}(1.3);
  l{@const l: integer = 1;@} = 1;
  l2 = l{@const l: integer = 1;@}+1;
  r{@const r: real = sin(2);@} = sin(2);
  
begin
end.