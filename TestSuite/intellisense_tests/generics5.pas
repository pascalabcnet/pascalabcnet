function f1<T>(self: sequence of T): (sequence of byte, sequence of word);
begin
  Result := (
    Seq(byte(0)),
    Seq(word(0))
  );
end;

begin
  var a{@var a: (IEnumerable<byte>,IEnumerable<word>);@} := f1(Seq(0));
end.