
unit sec_unit2;

interface


type

super_type = class
 a, b, c : string;
end;

tp2=class(super_type)

f : real;

procedure proc3;
procedure proc2(i:integer);overload;
procedure proc2(i,j:integer);overload;

function f1:real;
function f2(i:integer):integer;overload;
function f2(i,j:integer):integer;overload;

end;


implementation

procedure tp2.proc3;
procedure nested;
begin
 writeln(f); writeln(f1);
end;

begin
 f := 3.14;
 writeln('proc called');
 nested;
end;

procedure tp2.proc2(i:integer);
procedure nested;
begin
i := 77777;
end;

begin
 nested;
 writeln('proc2 with integer '+i.ToString);
end;

procedure tp2.proc2(i,j:integer);
begin
 writeln('proc2 with 2 integers '+i.ToString+'   '+j.tostring);
end;

function tp2.f1:real;
begin
 writeln('f1 called');
 f1:=2.7;
end;

function tp2.f2(i:integer):integer;
begin
 writeln('f2 with integer called '+i.tostring);
 f2:=i;
end;

function tp2.f2(i,j:integer):integer;
begin
 writeln('f2 with 2 integer called ' + i.tostring+'  '+j.tostring);
 f2:=i+j;
end;


end.
