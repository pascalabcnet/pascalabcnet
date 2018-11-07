begin
  var d := new Dictionary<byte, string->string>;
  d.Add(5, s->s*2);
  d[0]('abc').IndexOf{@function string.IndexOf(value: char): integer;@}('a');
end.