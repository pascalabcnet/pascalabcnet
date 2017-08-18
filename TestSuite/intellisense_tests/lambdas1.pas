procedure p(f:complex->real);
begin
  
end;

procedure test;
begin
  var a := new System.Collections.Generic.List<string>();
  a.Add('abc');
  a.Add('def');
  var b := a.SelectMany(x{@parameter x: string;@} -> x).Select(x{@parameter x: string;@} -> x);
  foreach var c in b do
    writeln(c);
end;
begin
  var f1:complex->real:=x{@parameter x: Complex;@}->Sqr(x.Real)+Sqr(x.Imaginary);
  var f2: function(c: complex): real := x{@parameter x: Complex;@}->Sqr(x.Real)+Sqr(x.Imaginary);
  f1 := x{@parameter x: Complex;@}->Sqr(x.Real)+Sqr(x.Imaginary);
  var f3:(real,complex)->real:=(y{@parameter y: real;@},x{@parameter x: Complex;@})->Sqr(x.Real)+Sqr(x.Imaginary);
  p(x{@parameter x: Complex;@}->Sqr(x.Real)+Sqr(x.Imaginary));
end.