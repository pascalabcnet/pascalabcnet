procedure p0<T>(o: T; p: byte->()) := p(2);

procedure p1() := exit;

var i: integer;
begin
  var p2: ()->() := ()->Inc(i);
  
  p0(0, b-> p1()        );
  p0(0, b-> p2.Invoke() ); 
  p0(0, b-> p2()        );
  assert(i = 2);
end.