type
  t1=class end;

procedure operator+=<T>(a,b:t1) := exit;

begin
  var a := new t1;
  a += new t1;
end.