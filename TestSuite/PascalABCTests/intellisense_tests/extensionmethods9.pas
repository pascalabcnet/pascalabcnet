begin
  var ch: char;
  ch.IsDigit{@(расширение) function char.IsDigit(): boolean;@};
  char.IsDigit{@static function char.IsDigit(c: char): boolean;@}(ch);
  var a: array of byte;
  a.Reverse{@(расширение sequence of T) function Reverse<byte>(): sequence of byte;@};
end.