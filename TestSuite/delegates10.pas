var i: integer;

function f1: procedure;
begin
  Inc(i);
  Result := procedure ()->exit;
end;

procedure test(p: procedure);
begin
  p();  
end;

begin
  var p: procedure;
  p := f1();
  p();
  assert(i = 1);
  test(f1);
  assert(i = 2);
  p := f1 <> nil ? f1 : nil;
  assert(i = 4);
end.