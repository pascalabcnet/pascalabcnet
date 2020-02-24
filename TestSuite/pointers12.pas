type
 TPerson = record
  fam: string[15];
  name: string[10];
  id: integer;
 end;
type
  PPerson = ^TPerson; 
var x: array[1..1] of TPerson;
    p: PPerson;
begin
  p := @x[1];
  p^.name := 'abc';
  assert(p^.name = 'abc');
end.