type
  t1 = class
    function f1: char? := #1;
  end;
  
  function f2: char? := #1;
  
begin
  var b := t1.Create.f1 = #1;
  assert(b);
  var c := #1;
  assert(t1.Create.f1 = c);
  var nc: char? := #1;
  assert(t1.Create.f1 = nc);
  assert(f2 = c);
end.