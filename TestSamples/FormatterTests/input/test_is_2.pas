type c=class 
constructor create;
begin
end;
end;
var cc:c;
begin
  cc:=c.create;
  Writeln((cc is c).tostring);
  Writeln(cc is c);
  readln;
end.