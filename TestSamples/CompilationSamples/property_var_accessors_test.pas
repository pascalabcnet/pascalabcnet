type
  c=class
    x:integer;
    property xp:integer read x write x;
  end;
  
var cc:c;
begin
  cc:=new c;
  cc.xp:=10;
  writeln(cc.xp);
  readln;
end.