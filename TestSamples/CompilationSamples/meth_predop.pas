type
tp=class

f : real;

constructor create;

procedure proc;
procedure proc2(i:integer);overload;
procedure proc2(i,j:integer);overload;

function f1:real;
function f2(i:integer):integer;overload;
function f2(i,j:integer):integer;overload;

end;

constructor tp.Create;
begin
 writeln('constructor called');
end;

procedure tp.proc;
procedure nested;
begin
 writeln(sin(f)); writeln(f1);
end;

begin
 f := 3.14;
 writeln('proc called');
 nested;
end;

procedure tp.proc2(i:integer);
procedure nested;
begin
i := 77777;
end;

begin
 nested;
 writeln('proc2 with integer '+i.ToString);
end;

procedure tp.proc2(i,j:integer);
begin
 writeln('proc2 with 2 integers '+i.ToString+'   '+j.tostring);
end;

function tp.f1:real;
begin
 writeln('f1 called');
 f1:=2.7;
end;

function tp.f2(i:integer):integer;
begin
 writeln('f2 with integer called '+i.tostring);
 f2:=i;
end;

function tp.f2(i,j:integer):integer;
begin
 writeln('f2 with 2 integer called ' + i.tostring+'  '+j.tostring);
 f2:=i+j;
end;

procedure test_var(var a : integer);
procedure nested;
begin
 writeln(a.tostring);
end;

begin
 nested; 
end;

var
t:tp;

var
r:real;
i : integer;
begin

t:=tp.Create;
t.proc;
t.proc2(1);
t.proc2(1,2);
r:=t.f1;
r:=t.f2(1);
r:=t.f2(1,2);
i := 500;
test_var(i);
end.
