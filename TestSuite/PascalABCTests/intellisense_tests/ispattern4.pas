function f1(o: object) :=
(o is byte(var b)) and (b{@var b: byte;@}=5);

begin
  var o: object := byte(5);
  if (o is byte(var b)) and (b{@var b: byte;@}=5) then
end.