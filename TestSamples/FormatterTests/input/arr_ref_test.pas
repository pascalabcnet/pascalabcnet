
type t=class
aa:array of integer;
constructor create;
begin
//setlength(aa,1);
end;
end;

procedure p(var i:integer);
begin
i:=i+1;
end;

var
a:array[1..10] of integer;
tt:t;
begin
//a[1]:=1;
//writeln(a[1]);
//p(a[1]);
//writeln(a[1]);
tt:=t.create;
setlength(tt.aa,10);
writeln('ok');
//tt.aa[1]:=10;
tt.aa[1]:=1;
writeln(tt.aa[1]);
tt.aa[1]:=2;
writeln(tt.aa[1]);
readln;
end.