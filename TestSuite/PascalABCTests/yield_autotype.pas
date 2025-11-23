function f1: sequence of byte;
begin
  var ii := 66;
  var o := ii;
  yield ii;
end;

begin 
  Assert(f1.First = 66);
end.