uses system;

var a:array[1..10] of byte;
    //b:array[1..10] of integer;
    //i:integer;
begin
  //i:=100;
  //b[1]:=10;
  a[1]:=Convert.ToByte(10);
  writeln(a[1]);
  readln;
end.