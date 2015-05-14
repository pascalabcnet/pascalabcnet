var b : array of integer;
    c: array of array of integer;
begin
  b := new integer[10];
  var a:=new byte[10];
  a[0]:=10;
  write(a[0]);
  //var a := new integer[10];
  c := new array of integer[1];
  c[0] := new integer[1];
  c[0,0]:=11;
  writeln(c[0,0]);
end.