procedure p:=write( 3);

function f: integer:=4;

type
  A = class
    procedure p:=write( 1);
  
  function f: integer:=2;
end ;

begin
  var a1 := new A;
  write(a1.f);
  a1.p;
  write(3);
  write(f);
  p;
end.