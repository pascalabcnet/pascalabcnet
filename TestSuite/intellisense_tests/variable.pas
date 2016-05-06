begin
  var a1{@var a1: integer;@} := 2;
  var a2{@var a2: char;@} := '3';
  var a3{@var a3: string;@} := '32';
  var a4{@var a4: real;@} := 2.3;
  var a5{@var a5: boolean;@} := true;
  var a6{@var a6: byte;@}: byte;
  var a7{@var a7: string [5];@}: string[5];
  var a8{@var a8: List<integer>;@} := new List<integer>;
  var a9{@var a9: real;@} := 2+3.4;
  var a10{@var a10: List<integer>;@} := a8+2;
  var a11{@var a11: integer;@} := a8[2];
  writeln(a1{@var a1: integer;@}+a4);
end.