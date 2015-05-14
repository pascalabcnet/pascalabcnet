var i:integer;
	t:system.type;
	l:longint;
	b:byte;

function f:System.Collections.Hashtable;
begin
  f:=System.Collections.Hashtable.Create();
end;

begin
  i:=1;
  writeln(i.gettype.tostring);
  writeln(2.tostring);
  writeln(i.tostring);
  writeln(2.GetHashCode.tostring);
  writeln(i.GetHashCode.tostring);
  writeln(2.gettype.tostring);
  writeln(i.gettype.tostring);
  i:=3.GetHashCode;
  writeln(i);
  writeln($ff.gettype.tostring);
  writeln(b.gettype.tostring);
  writeln(' '.gettype.tostring);
  writeln('  '.gettype.tostring);
  writeln(i.gettype.gettype.tostring);
  writeln(l.gettype.tostring);
  writeln(f.gettype.tostring);
  writeln(1000000000000000000.gettype.tostring);{}
  readln;
end.