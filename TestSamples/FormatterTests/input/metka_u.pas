unit metka_u;

interface

procedure p;

implementation

label a;

var
  i: integer;

procedure p;
label b;
begin
  goto b;
  writeln('ERROR');
  b: writeln('OK');
end;


initialization
  i := 0;
  a:
  writeln('Initialization works!');
  if i<10 then
  begin
    i := i + 1;
    goto a;
  end;
  
finalization
  //goto a;
  
end.