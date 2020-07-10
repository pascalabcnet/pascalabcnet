uses System;

var i: integer;

procedure Test;
begin
  Inc(i);
end;

type TProc = procedure(i: integer);

begin
  var d: Delegate := ()->Test();
  Action(d);
  assert(i = 1);
  var d2: Delegate := ()->Test();
  Action(d2);
  assert(i = 2);
end.