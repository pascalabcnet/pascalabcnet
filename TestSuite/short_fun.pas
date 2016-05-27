procedure p:=write( 3);

function f: integer:=4;

type
  A = class
    procedure p:=write( 1);
  
  function f: integer:=2;
end ;

begin
  var a1 := new A;
  assert(a1.f=2);
  a1.p;
  write(3);
  assert(f=4);
  p;
end.