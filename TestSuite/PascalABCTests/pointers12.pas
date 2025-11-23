type
 TPerson = record
  fam: string[15];
  name: string[10];
  id: integer;
 end;
type
  PPerson = ^TPerson; 
var x: array[1..3] of TPerson;
    p: PPerson;

procedure test1;
begin
  var p1: PPerson := @x[1];
  for var i := 1 to 1000 do
  begin
    p1^.name := 'abc';
    assert(p1^.name = 'abc');
  end;
  
end;
procedure test2;
begin
  var p2: PPerson := @x[2];
  for var i := 1 to 1000 do
  begin
    p2^.name := 'abc';
    assert(p2^.name = 'abc');
  end;
end;
begin
  {$omp parallel sections}
  begin
    test1;
    test2;
  end;
  p := @x[1];
  p^.name := 'abc';
  assert(p^.name = 'abc');
  p := @x[2];
  p^.name := 'def';
  assert(p^.name = 'def');
end.