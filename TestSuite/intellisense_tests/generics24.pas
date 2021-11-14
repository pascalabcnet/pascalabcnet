//!semantic
begin
  Arr(1,2,3).Aggregate(1.0, (y, x{@parameter x: integer;@}) -> 
      (y{@parameter y: real;@} * (x mod 10)));
  ReadArrInteger(10).Where((x, i) -> i{@parameter i: integer;@} mod 2 = 0);
end.