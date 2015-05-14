
unit sec_unit;

interface

uses sec_unit2,{PascalABCSystem}unit1;

type

base_tp = class(super_type)
 g : integer;
end;

tp=class

f : real;

obj : tp2;

procedure proc;
procedure proc2(i:integer);overload;
procedure proc2(i,j:integer);overload;

function f1:real;
function f2(i:integer):integer;overload;
function f2(i,j:integer):integer;overload;

end;

const pi = 3.141592;

procedure proc1;
procedure proc2;
procedure proc3(s : string);overload;
procedure proc3(i : integer);overload;

function sin(x : real) : real;

function cos(x : real) : real;

function tan(x : real) : real;

function ln(x : real) : real;

implementation

type TImplType = class
     p : real;
	end;

procedure tp.proc;
procedure nested;
var tt : TImplType;
begin
 writeln(f); writeln(f1);
end;

begin
 f := 3.14;
 writeln('proc called');u1proc; //obj.proc3;
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

function sin(x : real) : real;
begin
sin := System.Math.Sin(x);
end;

function cos(x : real) : real;
begin
cos := System.Math.Cos(x);
end;

function tan(x : real) : real;
begin
tan := System.Math.Tan(x);
end;

function ln(x : real) : real;
begin
ln := System.Math.Log(x);
end;

procedure proc1;
begin
 writeln('proc1 called.');
end;


procedure proc2;
begin
 writeln('proc2 called.');
end;

procedure proc3(s : string);
begin
 writeln('this is '+s); 
end;

procedure proc3(i : integer);
begin
 writeln('this is'+i.toString);
end;

end.