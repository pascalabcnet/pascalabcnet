var global := 2;
var tip := 'int';
procedure assign_global(var a :Integer);
begin
  a := global;
end;

procedure t();
type global = Integer;
begin
  var a : global = 3;
  Print(a);
end;

procedure k<tip>();
begin
  var a : tip = nil;
end;

begin
  t();
end.