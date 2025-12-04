var a13{@var a13: integer;@} := Round(2);

begin
  var a1{@var a1: integer;@} := 2;
  var a2{@var a2: char;@} := '3';
  var a3{@var a3: string;@} := '32';
  var a4{@var a4: real;@} := 2.3;
  var a5{@var a5: boolean;@} := true;
  var a6{@var a6: byte;@}: byte;
  var a7{@var a7: string[5];@}: string[5];
  var a8{@var a8: List<integer>;@} := new List<integer>;
  var a9{@var a9: real;@} := 2+3.4;
  var a10{@var a10: sequence of integer;@} := a8+2;
  var a11{@var a11: integer;@} := a8[2];
  var a12 := sin{@function Sin(x: real): real;@}(2);
  var a14{@var a14: real;@} := 5/5;
  var a15{@var a15: integer;@} := 5 div 5;
  var a16{@var a16: int64;@} := int64(5) div 5;
  var a17{@var a17: string;@} := 'a'+'b';
  var a18{@var a18: string;@} := a3+a2;
  var a19{@var a19: string;@} := a2+a3;
  var a20{@var a20: List<integer>;@} := a8 + a8;
  var a21{@var a21: boolean;@} := 2 in a8;
  writeln(a1{@var a1: integer;@}+a4);
end.