begin
  var ch: char;
  var n: integer;
  (ch*n).Replace{@function string.Replace(oldChar: char; newChar: char): string;@}(#0,#32);
  (2+3).Between{@(расширение) function integer.Between(a: integer; b: integer): boolean;@}(2,3);
end.