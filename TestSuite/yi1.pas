function f1: sequence of byte;
begin
  var action: System.Action;
  
  yield 2;
end;

begin 
  Assert(f1.First=2)
end.