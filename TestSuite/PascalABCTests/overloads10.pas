function f1: sequence of integer:= Seq(1,2);

begin
  var s := f1+f1;
  assert(1 in s);
end.