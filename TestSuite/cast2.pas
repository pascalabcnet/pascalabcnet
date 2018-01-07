var i: integer;

procedure p(a: byte);
begin 
  i := 1;
end;

type
  fb = function: byte;

begin
  var a: function: byte := ()->2;
  p(fb(a));
  assert(i = 1);
  var b := fb(a);
  assert(b = 2);
  assert(fb(a).ToString = 'cast2.fb');
end.