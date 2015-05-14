
unit cycle_unit;

interface

uses cycle_unit2;

type tmyclass = class
     ff : integer;
     constructor create;
     begin
       writeln('tmyclass.create');
     end;
     end;

procedure test(a : TClass);

procedure SayHello;
procedure SayHello2;

implementation

uses cycle_unit3;

procedure test(a : TClass);
begin
 a.Hello;
end;

procedure SayHello;
begin
 writeln('SayHello');
 my_proc(new tmyclass);
 test(new tclass);
end;
procedure SayHello2;
begin
 writeln('SayHello2');
end;

end.
