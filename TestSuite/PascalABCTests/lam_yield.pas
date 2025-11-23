function f1: sequence of byte->byte;
begin
  yield b->b*b;
end;

begin 
  var q := f1().First;
  Assert(q(2)=4);
end.