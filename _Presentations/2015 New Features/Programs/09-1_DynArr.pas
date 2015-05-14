var a: array of integer;

begin
  a := Seq(2,5,7,10);
  writeln(a);
  a := SeqRandom(10);
  writeln(a);
  var b := Seq('a','e','i','o','u','y');
  writeln(b);
  a := SeqFill(1,5);
  writeln(a);
  a := ReadSeqInteger(5);
  writeln(a);
end.