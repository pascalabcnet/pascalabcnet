type
  t1 = record 
    i: integer :=1;
  end;
  
function f1: sequence of t1;
begin
  var o: t1;
  yield o;
end;

begin
  var i := 0; 
  foreach var it in f1 do
  begin
    assert(it.i = 1);
    Inc(i);
  end;
  assert(i = 1);
end.