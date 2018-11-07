function mydivrem(a,b: integer; var r: integer): integer;
begin
  
end;

function mydivrem(a,b: int64; var r: int64): int64;
begin
  
end;
begin
  var rem32: integer;
  var rem64: int64;
  System.Math.DivRem{@static function Math.DivRem(a: integer; b: integer; var result: integer): integer;@}(1, 1, rem32);
  System.Math.DivRem{@static function Math.DivRem(a: int64; b: int64; var result: int64): int64;@}(1, 1, rem64);
  mydivrem{@function mydivrem(a: integer; b: integer; var r: integer): integer;@}(1, 1, rem32);
  mydivrem{@function mydivrem(a: int64; b: int64; var r: int64): int64;@}(1, 1, rem64);
end.