
type tb=class

constructor create;
begin
writeln('tB constructor called');
end;

procedure pr;virtual;

procedure pr2;virtual;overload;
begin
 writeln('pr2 from BASE class called');
end;

procedure pr2(i:integer);virtual;overload;
begin
 writeln('pr2 WITH INTEGER from BASE class called');
end;

end;

type td=class (tb)

constructor create;
begin
writeln('tD constructor called');
end;

procedure pr;override;

procedure pr2;override;overload;
begin
 writeln('pr2 from DERIVED class called');
end;

procedure pr2(i:integer);virtual;overload;
begin
 writeln('pr2 WITH INTEGER from DERIVED class called');
end;

end;

procedure tb.pr;
begin

writeln('pr of tB called');

end;

procedure td.pr;
begin

writeln('pr of tD called');

end;

var
t:tb;
t2:td;

begin

writeln('begin test');

t:=tb.create;
t2:=td.create;

t.pr;

t2.pr;

t.pr2;

t2.pr2;

t.pr2(1);

t2.pr2(2);

t:=td.create;

t.pr;

t.pr2;

t.pr2(3);

writeln('end test');
writeln('OK');
System.Console.Readline;

end.
