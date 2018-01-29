begin
  var w: word;
  var a{var a: real;} := w / 2;
  var a2{var a2: integer;} := w div 2;
  var b: byte;
  var a3{var a3: real;} := b / 2;
  var a4{var a4: integer;} := b div 2;
  var a5{var a5: integer;} := w * 2;
  writeln(a.GetType);
  writeln(a2.GetType);
  writeln(a5.GetType);
end.