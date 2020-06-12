type
  T = byte -> ();

var p: T;

begin
  if p <> nil then p{@var p: T;@}(2);
  p{(procedure Action<>.Invoke(obj: byte); virtual;)}(2);
end.