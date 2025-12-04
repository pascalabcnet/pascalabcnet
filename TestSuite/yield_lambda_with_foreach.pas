function f1: sequence of string;
begin
  var p: ()->() := procedure->
  begin
    foreach var i in Arr(0) do;
  end;
  yield 'a';
end;

begin
  Assert(f1.First = 'a'); 
end.