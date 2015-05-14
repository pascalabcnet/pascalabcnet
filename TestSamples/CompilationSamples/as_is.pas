var i:integer;
    r:real;
    o:object;
    b:boolean;

begin
  o:=i;
  b:=o is real;
  writeln(b);
  writeln((o is integer).tostring);
  readln;
end.