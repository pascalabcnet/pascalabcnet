uses test_debug_unit;

function geti:integer;
function getin:integer;
begin
  getin:=1;
end;
begin
  geti:=getin;
end;

type c=class
       constructor create;
       var i:integer;
       begin
         readln;
         i:=100;
       end;
       end;{}


var a:array[1..10] of integer;
    i:integer;
    cc:c;

begin
  i:=geti;
  a[f]:=10;
  i:=f;
  a[geti]:=11;
  writeln(a[f]);
  writeln(a[geti]);
  i:=10;
  case i of
    10:begin
         writeln('xxxx');
       end;
    else 
      i:=0;
  end;
  cc:=c.create;
  readln;{}
end.