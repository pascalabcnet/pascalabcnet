
type
t1=procedure;
t2=procedure;

var
tt1:t1;
tt2:t2;

procedure proc;
begin
writeln('proc called');
end;

begin

if (tt1=nil) then
begin
 writeln('tt1 == null');
end
else
begin
 writeln('tt1 != null');
end;

if (tt2=nil) then
begin
 writeln('tt2 == null');
end
else
begin
 writeln('tt2 != null');
end;

tt1:=tt2;

if (tt1=nil) then
begin
 writeln('tt1 == null');
end
else
begin
 writeln('tt1 != null');
end;

if (tt2=nil) then
begin
 writeln('tt2 == null');
end
else
begin
 writeln('tt2 != null');
end;

tt2:=proc;

if (tt1=nil) then
begin
 writeln('tt1 == null');
end
else
begin
 writeln('tt1 != null');
end;

if (tt2=nil) then
begin
 writeln('tt2 == null');
end
else
begin
 writeln('tt2 != null');
end;

tt2;
tt2+=proc;

tt1:=tt2;

if (tt1=nil) then
begin
 writeln('tt1 == null');
end
else
begin
 writeln('tt1 != null');
end;

if (tt2=nil) then
begin
 writeln('tt2 == null');
end
else
begin
 writeln('tt2 != null');
end;

tt2;

tt1;

readln;
end.