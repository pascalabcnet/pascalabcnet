uses events2u;

event 
  ev: procedure(i: integer);
  
procedure Test(i: integer);
begin
  //assert(i=34);
end;

procedure Test2;
begin  
end;

function Test3(i: integer): integer;
begin
  Result := i + 1;
end;

function Test4: char;
begin
  Result := 'j';  
end;

begin
  ev += Test;
  ev(34);
  ev -= Test;
  ev1 += Test2;
  ev2 += Test3;
  ev3 += Test4;
  
  RaiseEvent1;
  assert(RaiseEvent2(3) = 4);
  assert(RaiseEvent3() = 'j');
end.