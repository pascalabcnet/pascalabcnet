type c=class 
  constructor;
  begin
    writeln('OK');
  end;
end;
var cc:c;

begin
  cc:=new c;
  new c;
  readln;
end.