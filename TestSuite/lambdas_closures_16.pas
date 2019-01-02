procedure p1(self: string);
begin
  var p: procedure := procedure->self[1] := self[2];
  p;
  assert(self = 'bbc');
end;

begin 
  p1('abc');
end.