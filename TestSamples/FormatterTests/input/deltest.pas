
type
tp=function(i:integer):integer;
tp2=function(s:string):integer;
tp3=function:string;

sf1=function:integer;
sf2=function:sf1;
sf3=function:sf2;

cc=class
constructor create;
begin

end;

function func(i:integer):integer;
begin
 writeln('func from cc class called, i= '+i.tostring);
end;

class function func2(i:integer):integer;
begin
 writeln('static func2 called, i= '+i.tostring);
end;
end;


function ff(i:integer):integer;
begin
ff:=1;
writeln('ff called with: '+i.tostring);
end;

function FF2(i:integer):integer;
begin
FF2:=2;
writeln('FF2 called with: '+i.tostring);
end;

function nf1:integer;
begin
nf1:=88;
end;

function nf2:sf1;
begin
nf2:=nf1;
end;

function nf3:sf2;
begin
nf3:=nf2;
end;

var
tt:tp;
c:cc;
tt2:tp2;
ii:integer;
tt3:tp3;
ss:string;
ob:System.object;
SFF:sf3;
r:real;
 
begin

tt:=ff;
tt(1);
tt:=FF2;
tt(2);
tt:=ff;
tt(3);
tt:=FF2;
tt(4);
tt:=ff;
tt(5);
writeln('OK');

c:=cc.Create;
tt:=c.func;
tt(5);

tt:=cc.func2;
tt(10);

tt2:=integer.parse;
ii:=tt2('12345');
writeln(ii);

ob:=object.create;
tt3:=ob.ToString;
ss:=tt3;
writeln(ss);

SFF:=nf3;
r:=SFF;
writeln(r);

r:=nf3();
writeln(r);

writeln('OK');

end.