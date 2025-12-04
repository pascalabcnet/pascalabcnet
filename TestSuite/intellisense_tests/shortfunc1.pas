type
  t1=class
    function f1:real;
  end;

function t1.f1 := integer(0);

begin
  var r{@var r: real;@} := (new t1).f1;
end.