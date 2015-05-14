var i:=2;
    pi:pinteger;
    p:pointer;

begin
  //implicit
  p:=@i;
  //pi:=p;//delphi
  
  //explicit
  pi:=pinteger(p);
  writeln(pi^);
  i:=integer(p);
  writeln(i);
  p:=pointer(i);
  p:=pointer(@i);
  //pi:=pinteger(i);
end.