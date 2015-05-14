type proc=procedure;
     arr=array[1..2] of proc;

var a:arr;

type
     c=class
       function f:arr;
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
  a[1]:=p;
  a[1];
  cc := c.create;
  cc.f[1];
  readln;
end.