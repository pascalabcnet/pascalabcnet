
type
t=class
 constructor create;overload;
 constructor create(i:integer);overload;
 constructor create(r:real);overload;
 procedure pr;
end;

constructor t.create(i:integer);

var
value:real;

//procedure int_method;
//begin
 //writeln(value);
//end;

begin
 value:=2.718281828459045;
 //int_method;
 writeln('int constructor called');
end;

constructor t.create(r:real);

var
value:real;

//procedure int_method;
//begin
 //writeln(value);
//end;
begin
 value:=3.14159;
 //int_method;
 writeln('real constructor called');
end;

constructor t.create;

var
s:string;

//procedure int_method;
//begin
 //writeln('Out string: '+s);
//end;

begin
 s:='Some text string.';
 //int_method;
 writeln('Constructor called');
end;

procedure t.pr;
begin
 writeln('pr called');
end;

procedure pr2;
begin
 writeln('pr2 called');
end;

var
 tt:t;

begin

tt:=t.create;

tt.pr;

tt:=t.create(1);

tt:=t.create(1.0);

pr2;

System.Console.Writeline('OK');
System.Console.readline;

end.
