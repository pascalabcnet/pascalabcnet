
type proc=procedure;

var a:array of proc;

type 
     c=class
       function f:array of proc;
       begin
         result:=a;
       end;
     end;
     
procedure p;
begin
  writeln('p');
end;

var cc:c;

begin
  setlength(a,1);
  a[0]:=p;
  a[0];
  cc := new c;
  cc.f[0];
  readln;
end.