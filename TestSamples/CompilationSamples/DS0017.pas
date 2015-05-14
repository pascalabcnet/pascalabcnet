type c= class
  x:integer;
  constructor(x:integer);
  begin
    self.x:=x;
    writeln(x);
  end;
  end;

begin
  //Writeln(new c(10));
  Writeln((new c(10)).x=10?'OK':'ERROR');
  readln;

end.