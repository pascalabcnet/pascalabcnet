// forward-объявления для локальных подпрограмм запрещены
procedure f(i:integer); forward;

procedure f(i: integer);
 procedure g(i: integer); forward;
 procedure l(i: integer); forward;
 procedure q;
 var u: real;
 begin
 end;
 procedure g(i: integer);
 var j: real;
 begin
   j:=5;
   writeln(i,' ',j);
 end;
 procedure l(i: integer);
 var j: real;
 begin
   j:=5;
   writeln(j,' ',i);
 end;
begin
  g(i);
  l(i);
end;

begin
  f(7);
end.
