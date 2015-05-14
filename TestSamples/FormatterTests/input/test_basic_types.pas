var b:byte;
	i:integer;
	l:longint;
	r:real;
	bol:boolean;

function f:integer;
begin
  f:=1;
end;

begin
  i:=0;
  i:=257;
  
  b:=255+b;
  r:=b;
  writeln(r);
  b:=b+150;
  writeln(b);
  i:=+1000;
  i:=+10;
  writeln(i);
  r:=b/10;
  writeln(r);
  i:=f+1; //ÎØÈÁÊÀ!
  writeln(i);
  readln;
end.